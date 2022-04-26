using System;
using System.Threading;
using System.Windows;

namespace App05.Task;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CancellationTokenSource _tokenSource = new();

    public MainWindow()
    {
        InitializeComponent();
        System.Threading.Tasks.Task.Factory.StartNew(Test);
    }

    private void Test()
    {
        Dispatcher.Invoke(() => { Info.AppendText($"开始: {DateTime.Now}\n"); });
        System.Threading.Tasks.Task.WaitAny(new[] { System.Threading.Tasks.Task.Delay(10000, _tokenSource.Token) },
            _tokenSource.Token);
        Dispatcher.Invoke(() => { Info.AppendText($"结束: {DateTime.Now}\n"); });
    }

    private void ButtonStop_OnClick(object sender, RoutedEventArgs e)
    {
        _tokenSource.Cancel();
        Info.AppendText($"取消了Task.Wait: {DateTime.Now}\n");
    }

    private void ButtonRenew_OnClick(object sender, RoutedEventArgs e)
    {
        _tokenSource.Dispose();
        _tokenSource = new CancellationTokenSource();
        System.Threading.Tasks.Task.Factory.StartNew(Test);
    }
}