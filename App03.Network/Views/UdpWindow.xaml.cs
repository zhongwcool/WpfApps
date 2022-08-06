using System;
using System.ComponentModel;
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

public partial class UdpWindow
{
    public UdpWindow(string serverIp)
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
            var index = 0;
            foreach (var info in task.Result)
            {
                if (serverIp.Equals(info.Ip))
                {
                    break;
                }

                index++;
            }

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
        _udpClient?.Close();
        _udpClient = null;
    }

    private static string GetMessageSend(byte[] content, int count)
    {
        return "sent: " + Encoding.UTF8.GetString(content, 0, count);
    }

    private static string GetMessageRecv(byte[] content, int count)
    {
        return "recv: " + Encoding.UTF8.GetString(content, 0, count);
    }

    private UdpClient _udpClient;
    private IPEndPoint _multicast;

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
        var args = (Tuple<NetworkInfo, string>)e.Argument;
        if (null == args) return;

        _udpClient?.Close();
        var ipAddress = IPAddress.Parse(args.Item1.Ip);
        var localEndPoint = new IPEndPoint(ipAddress, 0);
        _udpClient = new UdpClient(localEndPoint);
        _multicast = new IPEndPoint(TrimBroadcastIpAddress(args.Item1.Ip), int.Parse(args.Item2));
        try
        {
            Thread.Sleep(500);
        }
        catch (Exception exception)
        {
            Console.WriteLine(">>>>>>>>" + exception.Message);
            Dispatcher.Invoke(() =>
            {
                MyStatusBar.Background = Brushes.Red;
                StatusInfo.Text = "UDP连接失败:" + exception.Message;
            });

            _udpClient.Close();
            _udpClient = null;
        }
    }

    private static IPAddress TrimBroadcastIpAddress(string localIp)
    {
        var ips = localIp.Split('.');
        return ips.Length == 4 ? IPAddress.Parse(ips[0] + "." + ips[1] + "." + ips[2] + "." + "255") : null;
    }

    private void NetComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var info = (NetworkInfo)NetComboBox.SelectedItem;

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
        //任务开始
        _worker.RunWorkerAsync(new Tuple<NetworkInfo, string>(info, TextPort.Text));
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
            if (null != _udpClient)
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
        while (!token.IsCancellationRequested)
        {
            try
            {
                var task = _udpClient.ReceiveAsync(token);
                var recvBytes = task.Result.Buffer;
                Dispatcher.Invoke(() =>
                {
                    var item = GetMessageRecv(recvBytes, recvBytes.Length);
                    LvReceive.Items.Add(item);
                    LvReceive.ScrollIntoView(item);
                    LvReceive.SelectedIndex = LvReceive.Items.Count - 1;
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    private void Button_Click_Close(object sender, RoutedEventArgs e)
    {
        _worker.CancelAsync();
    }

    private void ButtonSingle_OnClick(object sender, RoutedEventArgs e)
    {
        var data = Encoding.UTF8.GetBytes("hello, now is: " + TimeUtil.GetTimeStamp());
        SendMessage(data);
    }

    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        XuNiBox.Focus();
    }

    private readonly CancellationTokenSource _tokenSourceSend = new();
    private readonly ManualResetEvent _resetEvent = new(true);
    private Task _taskSendContinuous;

    private void ButtonContinuous_OnClick(object sender, RoutedEventArgs e)
    {
        // 禁用持续按钮
        ButtonContinuous.IsEnabled = false;
        _taskSendContinuous ??= Task.Factory.StartNew(TaskSendContinuous, _tokenSourceSend.Token);
        if (null != _taskSendContinuous) _resetEvent.Set();
    }

    private void TaskSendContinuous(object obj)
    {
        var token = (CancellationToken)obj;
        var count = 0;
        while (!token.IsCancellationRequested)
        {
            // 初始化为true时执行WaitOne不阻塞
            _resetEvent.WaitOne();

            var data = Encoding.UTF8.GetBytes("hello, Count: " + count++);
            SendMessage(data);
            Task.WaitAny(new[] { Task.Delay(500, token) }, token);
        }
    }

    private void ButtonPause_OnClick(object sender, RoutedEventArgs e)
    {
        ButtonContinuous.IsEnabled = true;
        _resetEvent.Reset();
    }

    private void SendMessage(byte[] data)
    {
        var task = _udpClient.SendAsync(data, data.Length, _multicast);
        task.ContinueWith(_ =>
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