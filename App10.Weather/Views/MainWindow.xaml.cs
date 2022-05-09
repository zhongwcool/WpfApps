using System;
using System.Linq;
using System.Windows;
using App10.Weather.ViewModels;

namespace App10.Weather.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = MainViewModel.CreateInstance();

        var x = Enumerable.Range(0, 1001).Select(i => i / 10.0).ToArray();
        var y = x.Select(v => Math.Abs(v) < 1e-10 ? 1 : Math.Sin(v) / v).ToArray();
        linegraph.Plot(x, y); // x and y are IEnumerable<double>
        linegraph1.Plot(x, y); // x and y are IEnumerable<double>
    }
}