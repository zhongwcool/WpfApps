using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using App18.Material.ViewModels;

namespace App18.Material.Views;

public partial class PageHome : UserControl
{
    public PageHome()
    {
        InitializeComponent();
        DataContext = new PageHomeViewModel();
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

    private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (e.Handled) return;
        // ListView拦截鼠标滚轮事件
        e.Handled = true;

        // 激发一个鼠标滚轮事件，冒泡给外层ListView接收到
        var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
        eventArg.RoutedEvent = MouseWheelEvent;
        eventArg.Source = sender;
        var parent = ((Control)sender).Parent as UIElement;
        parent.RaiseEvent(eventArg);
    }
}