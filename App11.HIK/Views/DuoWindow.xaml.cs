using System.Windows;
using App11.HIK.ViewModels;

namespace App11.HIK.Views;

public partial class DuoWindow : Window
{
    public DuoWindow()
    {
        InitializeComponent();
        DataContext = new DuoViewModel();
    }
}