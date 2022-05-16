using System.Windows;
using System.Windows.Controls;

namespace App10.Weather.Control;

public partial class DailyView : UserControl
{
    public DailyView()
    {
        InitializeComponent();
    }

    #region Property

    public static readonly DependencyProperty ShortDateProperty =
        DependencyProperty.Register(
            nameof(ShortDate),
            typeof(string),
            typeof(DailyView),
            new PropertyMetadata(string.Empty));

    public string ShortDate
    {
        get => (string)GetValue(ShortDateProperty);
        set => SetValue(ShortDateProperty, value);
    }

    public static readonly DependencyProperty HighProperty =
        DependencyProperty.Register(
            nameof(High),
            typeof(int),
            typeof(DailyView),
            new PropertyMetadata(0));

    public int High
    {
        get => (int)GetValue(HighProperty);
        set => SetValue(HighProperty, value);
    }

    public static readonly DependencyProperty LowProperty =
        DependencyProperty.Register(
            nameof(Low),
            typeof(int),
            typeof(DailyView),
            new PropertyMetadata(0));

    public int Low
    {
        get => (int)GetValue(LowProperty);
        set => SetValue(LowProperty, value);
    }

    public static readonly DependencyProperty SourceIndexProperty =
        DependencyProperty.Register(
            nameof(SourceIndex),
            typeof(int),
            typeof(DailyView),
            new PropertyMetadata(99));

    public int SourceIndex
    {
        get => (int)GetValue(SourceIndexProperty);
        set => SetValue(SourceIndexProperty, value);
    }

    #endregion
}