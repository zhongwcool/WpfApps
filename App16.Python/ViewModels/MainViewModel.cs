using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace App16.Python.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private ObservableCollection<DateTimePoint> RawSeries { get; }
    public ObservableCollection<ISeries> XYSeries { get; set; }

    private readonly Random _random = new();

    private void AddOneRand()
    {
        var rand = _random.Next(1, 100);
        RawSeries.Add(new DateTimePoint(DateTime.Now, rand));
        while (RawSeries.Count > 10) RawSeries.RemoveAt(0);
    }

    public void AddData(int ret)
    {
        RawSeries.Add(new DateTimePoint(DateTime.Now, ret));
        while (RawSeries.Count > 100) RawSeries.RemoveAt(0);
    }

    public MainViewModel()
    {
        RawSeries = new ObservableCollection<DateTimePoint>();
        AddOneRand();
        XYSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = RawSeries,
                GeometrySize = 4f,
                DataPadding = new LvcPoint(0, 0)
            }
        };
    }

    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            Labeler = value => new DateTime((long)value).ToString("HH:mm:ss.fff"),
            LabelsRotation = 15,
            TextSize = 12,
            SeparatorsPaint = new SolidColorPaint(SKColors.LightGray, 1),
        }
    };

    public Axis[] YAxes { get; set; } =
    {
        new Axis
        {
            LabelsRotation = 0,
            TextSize = 10,
            MinLimit = 0,
            MaxLimit = 100,
        }
    };

    public void Timer_Tick(object? sender, EventArgs e)
    {
        Dispatcher.CurrentDispatcher.Invoke(AddData);
    }
}