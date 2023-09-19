using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace App03.Network.Control;

public class SimpleButton : Button
{
    public static readonly DependencyProperty ModeProperty =
        DependencyProperty.Register(nameof(Mode), typeof(ButtonType), typeof(SimpleButton),
            new PropertyMetadata(ButtonType.IconText));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(SimpleButton),
            new PropertyMetadata(null));

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(SimpleButton),
            new PropertyMetadata(new CornerRadius(0)));

    public static readonly DependencyProperty MouseOverForegroundProperty =
        DependencyProperty.Register(nameof(MouseOverForeground), typeof(Brush), typeof(SimpleButton),
            new PropertyMetadata());

    public static readonly DependencyProperty MousePressedForegroundProperty =
        DependencyProperty.Register(nameof(MousePressedForeground), typeof(Brush), typeof(SimpleButton),
            new PropertyMetadata());

    public static readonly DependencyProperty MouseOverBorderBrushProperty =
        DependencyProperty.Register(nameof(MouseOverBorderBrush), typeof(Brush), typeof(SimpleButton),
            new PropertyMetadata());

    public static readonly DependencyProperty MouseOverBackgroundProperty =
        DependencyProperty.Register(nameof(MouseOverBackground), typeof(Brush), typeof(SimpleButton),
            new PropertyMetadata());

    public static readonly DependencyProperty MousePressedBackgroundProperty =
        DependencyProperty.Register(nameof(MousePressedBackground), typeof(Brush), typeof(SimpleButton),
            new PropertyMetadata());

    static SimpleButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleButton),
            new FrameworkPropertyMetadata(typeof(SimpleButton)));
    }

    public ButtonType Mode
    {
        get => (ButtonType)GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public Brush MouseOverForeground
    {
        get => (Brush)GetValue(MouseOverForegroundProperty);
        set => SetValue(MouseOverForegroundProperty, value);
    }

    public Brush MousePressedForeground
    {
        get => (Brush)GetValue(MousePressedForegroundProperty);
        set => SetValue(MousePressedForegroundProperty, value);
    }

    public Brush MouseOverBorderBrush
    {
        get => (Brush)GetValue(MouseOverBorderBrushProperty);
        set => SetValue(MouseOverBorderBrushProperty, value);
    }

    public Brush MouseOverBackground
    {
        get => (Brush)GetValue(MouseOverBackgroundProperty);
        set => SetValue(MouseOverBackgroundProperty, value);
    }

    public Brush MousePressedBackground
    {
        get => (Brush)GetValue(MousePressedBackgroundProperty);
        set => SetValue(MousePressedBackgroundProperty, value);
    }
}

public enum ButtonType
{
    Normal,
    Icon,
    Text,
    IconText
}