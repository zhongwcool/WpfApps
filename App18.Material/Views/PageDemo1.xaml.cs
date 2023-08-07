using System.Windows.Controls;
using App18.Material.ViewModels;

namespace App18.Material.Views;

public partial class PageDemo1 : UserControl
{
    public PageDemo1()
    {
        InitializeComponent();
        DataContext = new Demo1ViewModel();
    }
}