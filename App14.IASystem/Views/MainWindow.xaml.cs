using System.Windows;
using App14.IASystem.ViewModels;

namespace App14.IASystem.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        PoolsTab.DataContext = new PoolsTabViewModel();
        DevicesTab.DataContext = new DevicesTabViewModel();
        RecWqmTab.DataContext = new RecsTabViewModel();
    }
}