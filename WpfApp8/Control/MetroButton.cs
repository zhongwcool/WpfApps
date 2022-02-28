using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp8.Control;

public class MetroButton : Button
{
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(ImageSource), typeof(MetroButton), new PropertyMetadata(null));


    static MetroButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroButton),
            new FrameworkPropertyMetadata(typeof(MetroButton)));
    }

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}