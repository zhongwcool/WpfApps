using System.Windows;
using System.Windows.Controls;

namespace App18.Material.Views;

public partial class PageDemo2 : UserControl
{
    public PageDemo2()
    {
        InitializeComponent();
    }

    private void Grid_MouseLeftButtonDown(object sender, RoutedEventArgs e)
    {
        var grid = sender as Grid;
        var textBlock = grid?.FindName("TxtBrush") as TextBlock;
        Clipboard.SetDataObject(textBlock?.Text);
        //_snackbarMessageQueue.Enqueue("Copied to clipboard");
    }
}