using System.Windows;
using App05.TaskAndCancel.ViewModels;

namespace App05.TaskAndCancel.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = MainViewModel.CreateInstance();
    }
}