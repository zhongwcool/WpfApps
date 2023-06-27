using System.Windows;
using App02.TTS.ViewModels;

namespace App02.TTS.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}