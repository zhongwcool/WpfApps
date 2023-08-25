using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace App16.Python.Control;

public partial class FloatButton : UserControl
{
    public FloatButton()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(FloatButton), new PropertyMetadata(null));

    public Geometry Icon
    {
        get => (Geometry)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly DependencyProperty KiwiProperty =
        DependencyProperty.Register(nameof(Kiwi), typeof(string), typeof(FloatButton),
            new PropertyMetadata(string.Empty));

    public string Kiwi
    {
        get => (string)GetValue(KiwiProperty);
        set => SetValue(KiwiProperty, value);
    }
}