using System.Globalization;
using System.Threading;
using System.Windows;

namespace App09.Map;

public partial class BingMapWindow : Window
{
    public BingMapWindow()
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        InitializeComponent();
        EntryMaximizedWindow();
    }

    private void EntryMaximizedWindow()
    {
        WindowState = WindowState.Maximized;
        Topmost = false;
    }
}