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

        // 获取Local ip数据
        var task = Task.Run(NetworkUtil.GetLocalIp, _tokenSourceTaskGetIp.Token);
        task.ContinueWith(_ =>
        {
            LocalIp = task.Result;
            Dispatcher.Invoke(() =>
            {
                var main = new MainWindow(LocalIp);
                main.Show();
                Close();
            });
        });
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        _tokenSourceTaskGetIp?.Cancel();
    }

    private string LocalIp { get; set; }
}