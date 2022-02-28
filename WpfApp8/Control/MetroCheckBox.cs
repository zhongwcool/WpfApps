using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp8.Control;

public class MetroCheckBox : CheckBox
{
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(ImageSource), typeof(MetroCheckBox), new PropertyMetadata(null));


    static MetroCheckBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroCheckBox),
            new FrameworkPropertyMetadata(typeof(MetroCheckBox)));
    }

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}