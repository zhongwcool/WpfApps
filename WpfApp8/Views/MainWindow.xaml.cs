using System.Windows;
using WpfApp8.ViewModels;

namespace WpfApp8.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = MainViewModel.CreateInstance();
        
        BtnTest.Click += (s, e) => PopTest.IsOpen = true;
    }

    private void BtnTest_OnClick(object sender, RoutedEventArgs e)
    {
    }
}