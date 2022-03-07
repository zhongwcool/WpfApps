using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp8.Control;

public class IconCheckBox : CheckBox
{
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(Geometry), typeof(IconCheckBox), new PropertyMetadata(null));


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