using System.Windows;

namespace WpfApp8;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        BtnTest.Click += (s, e) => PopTest.IsOpen = true;
    }

    private void BtnTest_OnClick(object sender, RoutedEventArgs e)
    {
    }
}