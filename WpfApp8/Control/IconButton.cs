using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp8.Control;

public class IconButton : Button
{
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(Geometry), typeof(IconButton), new PropertyMetadata(null));


    static IconButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton),
            new FrameworkPropertyMetadata(typeof(IconButton)));
    }

    public Geometry Icon
    {
        get => (Geometry)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}