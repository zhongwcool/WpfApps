using System.Windows.Controls;
using System.Windows.Media;

namespace App18.Material.Views;

public partial class PageMail : UserControl
{
    public PageMail()
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
            Opacity = 0.4 // 设置新的透明度
        };

        // 将按钮的背景色设置为修改后的 Brush
        Background = modifiedBackground;
    }
}