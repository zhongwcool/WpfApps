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
        var window = new DynaWindow();
        window.ShowDialog();
    }

    private void ButtonXaml_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new XamlWindow();
        window.ShowDialog();
    }
}