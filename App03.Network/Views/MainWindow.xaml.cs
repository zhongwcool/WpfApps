using System.Windows;

namespace App03.Network.Views;

public partial class MainWindow : Window
{
    private string LocalIp { get; set; }

    public MainWindow(string localIp)
    {
        InitializeComponent();
        LocalIp = localIp;
    }

    private void ButtonUdp_OnClick(object sender, RoutedEventArgs e)
    {
        var win = new UdpWindow(LocalIp);
        win.ShowDialog();
    }

    private void ButtonTcp_OnClick(object sender, RoutedEventArgs e)
    {
        var win = new TcpWindow(LocalIp);
        win.ShowDialog();
    }
}