using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace App08.Metro.Control;

public class MetroButton : Button
{
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(MetroButton), new PropertyMetadata(null));


    static MetroButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroButton),
            new FrameworkPropertyMetadata(typeof(MetroButton)));
    }

    public Geometry Icon
    {
        get => (Geometry)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}