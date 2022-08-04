using System.Windows;
using App11.HIK.Utils;

namespace App11.HIK.Views;

public partial class SplashWindow : Window
{
    public SplashWindow()
    {
        InitializeComponent();
    }

    private void ButtonDyna_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new CodeWindow();
        window.ShowDialog();
    }

    private void ButtonXaml_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new XamlWindow();
        window.ShowDialog();
    }

    private void ButtonPage_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new PageInWindow();
        window.ShowDialog();
    }

    private void ButtonToast_OnClick(object sender, RoutedEventArgs e)
    {
        AppUtil.ShowToast("我是这样的");
    }
}