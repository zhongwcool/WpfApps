using System.Windows;
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

        Loaded += async (sender, args) =>
        {
            if (DataContext is MainViewModel vm) await vm.LoadDataAsync();
        };
    }

    private void OnItemClick(object sender, RoutedEventArgs e)
    {
        if (((FrameworkElement)sender).DataContext is not Channel channel) return;
        var view = new VlcView()
        {
            DataContext = channel
        };
        ViewGrid.Children.Add(view);
    }
}