using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace App08.Metro.Control;

public class IconCheckBox : CheckBox
{
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(IconCheckBox), new PropertyMetadata(null));


    static IconCheckBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IconCheckBox),
            new FrameworkPropertyMetadata(typeof(IconCheckBox)));
    }

    public Geometry Icon
    {
        get => (Geometry)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}