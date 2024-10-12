using System.Windows;
using App22.Selected.ViewModels;

namespace App22.Selected.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}