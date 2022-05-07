using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using App05.TaskAndCancel.ViewModels;

namespace App05.TaskAndCancel.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CancellationTokenSource _tokenSource = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = MainViewModel.CreateInstance();

        Task.Factory.StartNew(Test);
    }

    private void Test()
    {
        Dispatcher.Invoke(() => { Info.AppendText($"开始: {DateTime.Now}\n"); });
        Task.WaitAny(new[] { Task.Delay(10000, _tokenSource.Token) },
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
        Task.Factory.StartNew(Test);
    }
}