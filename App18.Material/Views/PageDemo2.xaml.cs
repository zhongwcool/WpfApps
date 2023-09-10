using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace App18.Material.Views;

public partial class PageDemo2 : UserControl
{
    public PageDemo2()
    {
        InitializeComponent();
        ReduceBackground();
    }

    private void ReduceBackground()
    {
        if (Background is not SolidColorBrush brush) return;
        // 创建具有修改透明度的新 Brush
        var modifiedBackground = new SolidColorBrush(brush.Color)
        {
            Opacity = 0.6 // 设置新的透明度
        };

        // 将按钮的背景色设置为修改后的 Brush
        Background = modifiedBackground;
    }

    private void Grid_MouseLeftButtonDown(object sender, RoutedEventArgs e)
    {
        var grid = sender as Grid;
        var textBlock = grid?.FindName("TxtBrush") as TextBlock;
        Clipboard.SetDataObject(textBlock?.Text);
        //_snackbarMessageQueue.Enqueue("Copied to clipboard");
    }
}