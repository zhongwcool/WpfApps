using System.Windows;

namespace WpfApp3.Views;

public partial class SlashWindow : Window
{
    public SlashWindow()
    {
        InitializeComponent();
    }

    private void ButtonUdp_OnClick(object sender, RoutedEventArgs e)
    {
        UdpWindow.GetInstance().ShowDialog();
    }

    private void ButtonTcp_OnClick(object sender, RoutedEventArgs e)
    {
        TcpWindow.GetInstance().ShowDialog();
    }
}