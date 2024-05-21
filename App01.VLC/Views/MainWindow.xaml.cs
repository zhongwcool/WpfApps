using System.Windows;
using System.Windows.Input;
using App01.VLC.Controls;
using App01.VLC.Models;
using App01.VLC.ViewModels;

namespace App01.VLC.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();

        Loaded += async (_, _) =>
        {
            if (DataContext is MainViewModel vm) await vm.LoadDataAsync();
        };
    }

    private UIElement _vlcview;

    private void OnItemClick(object sender, RoutedEventArgs e)
    {
        if (((FrameworkElement)sender).DataContext is not Channel channel) return;
        _vlcview = new VlcView()
        {
            DataContext = channel
        };
        ViewGrid.Children.Add(_vlcview);
    }

    private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        //按Esc键移除VlcView
        if (e.Key != Key.Escape) return;
        ViewGrid.Children.Remove(_vlcview);
    }
}