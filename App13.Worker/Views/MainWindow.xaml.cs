using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace App13.Worker.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        if (_worker is { IsBusy: true })
        {
            _worker.CancelAsync();
        }
    }

    private void BtnSynchronous_Click(object sender, RoutedEventArgs e)
    {
        const int max = 10000;
        PbCalculation.Value = 0;
        LvResults.Items.Clear();

        var result = 0;
        for (var i = 0; i < max; i++)
        {
            if (i % 42 == 0)
            {
                LvResults.Items.Add(i);
                result++;
            }

            Thread.Sleep(1);
            PbCalculation.Value = Convert.ToInt32(((double)i / max) * 100);
        }

        MessageBox.Show("Numbers between 0 and 10000 divisible by 7: " + result);
    }

    private BackgroundWorker _worker;

    private void BtnAsynchronous_Click(object sender, RoutedEventArgs e)
    {
        PbCalculation.Value = 0;
        LvResults.Items.Clear();

        _worker = new BackgroundWorker();
        _worker.WorkerReportsProgress = true;
        _worker.WorkerSupportsCancellation = true; //允许取消
        _worker.DoWork += Worker_DoWork;
        _worker.ProgressChanged += Worker_ProgressChanged;
        _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        _worker.RunWorkerAsync(10000);

        BtnAsynchronous.IsEnabled = false;
        BtnCancel.IsEnabled = true;
    }

    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        var bgWorker = sender as BackgroundWorker;
        var max = 0;
        if (e.Argument != null)
        {
            max = (int)e.Argument;
        }

        var result = 0;
        for (var i = 0; i < max; i++)
        {
            var progressPercentage = Convert.ToInt32((double)i / max * 100);
            if (i % 42 == 0)
            {
                result++;
                bgWorker?.ReportProgress(progressPercentage, i);
            }
            else
            {
                bgWorker?.ReportProgress(progressPercentage);
            }

            //在操作的过程中需要检查用户是否取消了当前的操作。
            if (bgWorker?.CancellationPending == true)
            {
                e.Cancel = true;
                break;
            }

            Thread.Sleep(1);
        }

        e.Result = result;
    }

    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        PbCalculation.Value = e.ProgressPercentage;
        if (e.UserState != null)
        {
            LvResults.Items.Add(e.UserState);
            LvResults.ScrollIntoView(e.UserState);
        }

        LvResults.SelectedIndex = LvResults.Items.Count - 1;
    }

    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        //如果用户取消了当前操作就关闭窗口。
        if (!e.Cancelled)
        {
            //计算结果信息：e.Result
            MessageBox.Show("Numbers between 0 and 10000 divisible by 7: " + e.Result);
        }

        //计算已经结束，需要禁用取消按钮。
        BtnCancel.IsEnabled = false;
        BtnAsynchronous.IsEnabled = true;

        //计算过程中的异常会被抓住，在这里可以进行处理。
        if (e.Error == null) return;
        var errorType = e.Error.GetType();
        switch (errorType.Name)
        {
            case "ArgumentNullException":
            case "MyException":
                //do something.
                break;
            default:
                //do something.
                break;
        }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        if (_worker.IsBusy)
        {
            _worker.CancelAsync();
        }
    }
}