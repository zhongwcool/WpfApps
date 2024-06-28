using System.Windows;
using App21.Skeleton.ViewModels;

namespace App21.Skeleton.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}