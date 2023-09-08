using System.Windows;
using System.Windows.Controls;
using App10.Weather.Models;

namespace App10.Weather.Control;

public partial class DailyView : UserControl
{
    public DailyView()
    {
        InitializeComponent();
    }

    #region Property

    public static readonly DependencyProperty BodyProperty =
        DependencyProperty.Register(nameof(Body), typeof(Daily), typeof(DailyView), new PropertyMetadata(null));

    public Daily Body
    {
        get => (Daily)GetValue(BodyProperty);
        set => SetValue(BodyProperty, value);
    }

    #endregion
}