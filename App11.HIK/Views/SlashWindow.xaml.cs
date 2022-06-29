using System.Windows;

namespace App11.HIK.Views;

public partial class SlashWindow : Window
{
    public SlashWindow()
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
}