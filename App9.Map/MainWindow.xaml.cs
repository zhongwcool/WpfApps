using System;
using System.Windows;

namespace App9.Map;

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

    private void ButtonChangeCenter_OnClick(object sender, RoutedEventArgs e)
    {
        WebBrowser.InvokeScript("randomZoomCenter");
    }

    private void ButtonAddPolygon_OnClick(object sender, RoutedEventArgs e)
    {
        WebBrowser.InvokeScript("setMapCenter", new object[] { 120.721603, 31.367004 });
    }
}