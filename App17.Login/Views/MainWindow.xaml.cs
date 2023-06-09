using System.Windows;
using App17.Login.ViewModels;

namespace App17.Login.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;

    public MainWindow()
    {
        InitializeComponent();
        _vm = new MainViewModel();
        DataContext = _vm;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        _vm.Login2();
    }

    private void ButtonData_OnClick(object sender, RoutedEventArgs e)
    {
        _vm.RequestWater();
    }
}