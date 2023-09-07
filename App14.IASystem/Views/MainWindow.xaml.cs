using System.ComponentModel;
using System.Windows;
using App14.IASystem.Context;
using App14.IASystem.ViewModels;

namespace App14.IASystem.Views;

public partial class MainWindow : Window
{
    private IaContext _context;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _context = new IaContext();
        // this is for demo purposes only, to make it easier
        // to get up and running
        _context.Database.EnsureCreated();

        // bind to the source
        PoolsTab.DataContext = new PoolsTabViewModel(_context);
        DevicesTab.DataContext = new DevicesTabViewModel(_context);
        RecWqmTab.DataContext = new RecsTabViewModel(_context);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        // clean up database connections
        _context.Dispose();
        base.OnClosing(e);
    }

    // to avoid exception "某个 ItemsControl 与它的项源不一致"
    // {
    private void PoolsTab_OnSelected(object sender, RoutedEventArgs e)
    {
        DataGridPools.Items.Refresh();
        DataGridSelectedPool.Items.Refresh();
    }

    private void DevicesTab_OnSelected(object sender, RoutedEventArgs e)
    {
        DataGridDevices.Items.Refresh();
    }

    private void RecsTab_OnSelected(object sender, RoutedEventArgs e)
    {
        DataGridRecs.Items.Refresh();
    }
    // }
}