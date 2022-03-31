using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp8.Control;

public class MyHeaderedContentControl : HeaderedContentControl
{
    public static readonly DependencyProperty ClickToHideProperty =
        DependencyProperty.Register("ClickToHide", typeof(bool), typeof(MyHeaderedContentControl),
            new PropertyMetadata(null));

    public bool ClickToHide
    {
        get => (bool)GetValue(ClickToHideProperty);
        set => SetValue(ClickToHideProperty, value);
    }

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseLeftButtonDown(e);

        var element = e.OriginalSource as FrameworkElement;
        var name = element?.Name;

        switch (name)
        {
            case null:
                return;
            case "PART_Title":
            case "PART_Back":
                if (!ClickToHide) return;
                if (element.Parent is not Grid elementHeader) return;
                var elementRoot = elementHeader.Parent as StackPanel;
                if (elementRoot?.FindName("PART_Content") is not Border elementContent) return;
                elementContent.Visibility = elementContent.Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
                break;
        }

        e.Handled = true;
    }
}