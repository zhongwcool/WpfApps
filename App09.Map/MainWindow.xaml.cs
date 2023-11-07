using System;
using System.Windows;
using App09.Map.Helper;
using ModernWpf;

namespace App09.Map;

public partial class MainWindow : Window
{
    private readonly string _sUrl = AppDomain.CurrentDomain.BaseDirectory + "gd.htm";

    public MainWindow()
    {
        InitializeComponent();
        WebBrowser.Loaded += (_, _) =>
        {
            var uri = new Uri(_sUrl);
            WebBrowser.Navigate(uri);
        };
    }

    private void ButtonRandom_OnClick(object sender, RoutedEventArgs e)
    {
        WebBrowser.InvokeScript("randomZoomCenter");
    }

    private void ButtonReset_OnClick(object sender, RoutedEventArgs e)
    {
        WebBrowser.InvokeScript("setMapCenter", new object[] { 120.721603, 31.367004 });
    }

    private void OnThemeButtonClick(object sender, RoutedEventArgs e)
    {
        DispatcherHelper.RunOnMainThread(() =>
        {
            if (ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark)
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            }
            else
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            }
        });
    }
}