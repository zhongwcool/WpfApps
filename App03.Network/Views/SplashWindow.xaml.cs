using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using App03.Network.Utils;

namespace App03.Network.Views;

public partial class SplashWindow : Window
{
    private readonly CancellationTokenSource _tokenSourceTaskGetIp = new();

    public SplashWindow()
    {
        InitializeComponent();

        Cook();
    }

    private async void Cook()
    {
        var token = _tokenSourceTaskGetIp.Token;
        // 获取Local ip数据
        var task = await Task.Run(NetworkUtil.GetLocalIp, token);
        LocalIp = task;
        Dispatcher.Invoke(() => { Title = LocalIp; });

        var task2 = await Task.Run(() =>
        {
            Task.WaitAny(new[] { Task.Delay(1000, token) }, token);
            return 60;
        }, token);

        Dispatcher.Invoke(() =>
        {
            Title = task2.ToString();
            var main = new MainWindow(LocalIp);
            main.Show();
            Close();
        });
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        _tokenSourceTaskGetIp?.Cancel();
    }

    private string LocalIp { get; set; }
}