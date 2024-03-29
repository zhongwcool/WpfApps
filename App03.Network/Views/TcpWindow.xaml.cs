﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using App03.Network.Models;
using App03.Network.Utils;

namespace App03.Network.Views;

public partial class TcpWindow
{
    public TcpWindow(string serverIp)
    {
        InitializeComponent();
        TextServer.Text = serverIp;
        MaskProgressBar.Visibility = Visibility.Visible;

        var task = Task.Run(() =>
        {
            var list = NetworkUtil.GetNetworkInfo();
            return list;
        });

        task.ContinueWith(_ =>
        {
            var index = task.Result.TakeWhile(info => !serverIp.Equals(info.Ip)).Count();

            Dispatcher.Invoke(() =>
            {
                NetComboBox.ItemsSource = task.Result;
                NetComboBox.SelectedIndex = index;
                MaskProgressBar.Visibility = Visibility.Collapsed;
            });
        });
    }

    protected override void OnClosing(CancelEventArgs cancelEventArgs)
    {
        _worker?.CancelAsync();
        _tokenSourceRecv?.Cancel();
        _tokenSourceSend?.Cancel();
        _tokenSourceConn?.Cancel();
        _tcpClient?.Close();
        _tcpClient = null;
    }

    private void NetComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var info = (NetworkInfo)NetComboBox.SelectedItem;
        //停止就连接的接收发送
        _tokenSourceRecv?.Cancel();
        _tokenSourceRecv = null;
        _tokenSourceSend?.Cancel();
        _tokenSourceSend = null;
        //显示等待框
        MaskProgressBar.Visibility = Visibility.Visible;

        _worker = new BackgroundWorker();
        //异步取消 需要增加这个 不然取消失效
        _worker.WorkerSupportsCancellation = true;
        //支持报告进度
        _worker.WorkerReportsProgress = true;
        //注册滚动条事件
        _worker.ProgressChanged += worker_ProgressChanged;
        //注册任务
        _worker.DoWork += worker_DoWork;
        //任务完毕触发
        _worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        _worker.RunWorkerAsync(new Tuple<NetworkInfo, string, string>(info, TextServer.Text, TextPort.Text));
    }

    private BackgroundWorker _worker;

    private static void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
    }

    private CancellationTokenSource _tokenSourceRecv;

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        if (sender is not BackgroundWorker worker) return;
        if (!e.Cancelled)
        {
            if (null != _tcpClient)
            {
                MyStatusBar.Background = Brushes.Green;
                StatusInfo.Text = "连接成功";

                //清除旧的消息
                LvReceive.Items.Clear();
                var item = "连接成功";
                LvReceive.Items.Add(item);
                LvReceive.ScrollIntoView(item);
                LvReceive.SelectedIndex = LvReceive.Items.Count - 1;

                //接收消息
                _tokenSourceRecv ??= new CancellationTokenSource();
                Task.Factory.StartNew(TaskRecv, _tokenSourceRecv.Token);
            }
        }
        else
        {
            // 任务取消
            MaskProgressBar.Visibility = Visibility.Collapsed;
            MyStatusBar.Background = Brushes.Orange;
            StatusInfo.Text = "连接取消";
        }

        worker.DoWork -= worker_DoWork;
        worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
        MaskProgressBar.Visibility = Visibility.Collapsed;
    }

    private void TaskRecv(object obj)
    {
        var token = (CancellationToken)obj;
        var netStream = _tcpClient.GetStream();
        var recvBytes = new byte[1024];
        while (!token.IsCancellationRequested)
        {
            try
            {
                var task = netStream.ReadAsync(recvBytes, 0, recvBytes.Length, token);
                var cnt = task.Result;
                if (cnt <= 0) continue;
                Dispatcher.Invoke(() =>
                {
                    var item = GetMessageRecv(recvBytes, cnt);
                    LvReceive.Items.Add(item);
                    LvReceive.ScrollIntoView(item);
                    LvReceive.SelectedIndex = LvReceive.Items.Count - 1;
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _tokenSourceRecv.Cancel();
                return;
            }
        }
    }

    private static string GetMessageSend(byte[] content, int count)
    {
        return "sent: " + Encoding.UTF8.GetString(content, 0, count);
    }

    private static string GetMessageRecv(byte[] content, int count)
    {
        return "recv: " + Encoding.UTF8.GetString(content, 0, count);
    }

    private TcpClient _tcpClient;
    private readonly CancellationTokenSource _tokenSourceConn = new();

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
        var args = (Tuple<NetworkInfo, string, string>)e.Argument;
        if (null == args) return;

        _tcpClient?.Close();
        var ipAddress = IPAddress.Parse(args.Item1.Ip);
        var localEndPoint = new IPEndPoint(ipAddress, 0);
        _tcpClient = new TcpClient(localEndPoint);
        try
        {
            _tcpClient.ConnectAsync(args.Item2, int.Parse(args.Item3));
            while (!_tcpClient.Connected)
            {
                if (_worker.CancellationPending)
                {
                    e.Cancel = true; //这里才真正取消 
                    return;
                }

                Task.WaitAny(new[] { Task.Delay(100, _tokenSourceConn.Token) }, _tokenSourceConn.Token);
            }

            Task.WaitAny(new[] { Task.Delay(500, _tokenSourceConn.Token) }, _tokenSourceConn.Token);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            Dispatcher.Invoke(() =>
            {
                MyStatusBar.Background = Brushes.Red;
                StatusInfo.Text = "连接失败:" + exception.Message;
            });

            _tcpClient.Close();
            _tcpClient = null;
        }
    }

    private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
    {
        _worker.CancelAsync();
    }

    private void ButtonSingle_OnClick(object sender, RoutedEventArgs e)
    {
        var message = $"hello, now is: {TimeUtil.GetTimeStamp()}";
        var data = Encoding.UTF8.GetBytes(message);
        SendMessage(data);
    }

    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        XuNiBox.Focus();
    }

    private CancellationTokenSource _tokenSourceSend;

    private void CheckBoxContinuous_OnChecked(object sender, RoutedEventArgs e)
    {
        _tokenSourceSend?.Cancel();
        _tokenSourceSend = new CancellationTokenSource();

        Task.Factory.StartNew(TaskSendContinuous, _tokenSourceSend.Token);
    }

    private void CheckBoxContinuous_OnUnchecked(object sender, RoutedEventArgs e)
    {
        _tokenSourceSend?.Cancel();
        _tokenSourceSend = null;
    }

    private void TaskSendContinuous(object obj)
    {
        var token = (CancellationToken)obj;

        var count = 0;
        while (!token.IsCancellationRequested)
        {
            var data = Encoding.UTF8.GetBytes("hello, Count: " + count++);
            SendMessage(data);
            Task.WaitAny(new[] { Task.Delay(500, token) }, token);
        }
    }

    private void SendMessage(byte[] data)
    {
        var netStream = _tcpClient.GetStream();
        var task = netStream.WriteAsync(data, 0, data.Length);
        task?.ContinueWith(_ =>
        {
            Dispatcher.Invoke(() =>
            {
                var item = GetMessageSend(data, data.Length);
                LvReceive.Items.Add(item);
                LvReceive.ScrollIntoView(item);
                LvReceive.SelectedIndex = LvReceive.Items.Count - 1;
            });
        });
    }
}