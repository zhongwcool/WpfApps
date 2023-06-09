using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace App16.Python.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ObservableCollection<DateTimePoint> _observableValues;
    public ObservableCollection<ISeries> Series { get; set; }

    private readonly Random _random = new();

    [RelayCommand]
    public void AddItem()
    {
        var randomValue = _random.Next(1, 100);
        _observableValues.Add(new DateTimePoint(DateTime.Now, randomValue));
    }

    [RelayCommand]
    public void RemoveItem()
    {
        if (_observableValues.Count == 0) return;
        _observableValues.RemoveAt(0);
    }

    public void AddData(int ret)
    {
        _observableValues.Add(new DateTimePoint(DateTime.Now, ret));
        RemoveItem();
    }

    public MainViewModel()
    {
        // Use ObservableCollections to let the chart listen for changes (or any INotifyCollectionChanged). // mark
        _observableValues = new ObservableCollection<DateTimePoint>
        {
            // new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(12000)), 0),
            // new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(11000)), 0),
            // new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(10000)), 0),
            // new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(9000)), 0),
            new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(8000)), 0),
            new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(7000)), 0),
            new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(6000)), 0),
            new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(5000)), 0),
            new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(4000)), 0),
            new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(3000)), 0),
            new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(2000)), 0),
            new(DateTime.Now.Subtract(TimeSpan.FromMilliseconds(1000)), 0),
        };

        Series = new ObservableCollection<ISeries>()
        {
            new LineSeries<DateTimePoint>
            {
                Values = _observableValues,
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

            // when using a date time type, let the library know your unit // mark
            UnitWidth = TimeSpan.FromMilliseconds(1000).Ticks,
            // The MinStep property forces the separator to be greater than 1 day.
            MinStep = TimeSpan.FromMilliseconds(50).Ticks // mark
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

            // when using a date time type, let the library know your unit // mark
            UnitWidth = 10,

            // The MinStep property forces the separator to be greater than 1 day.
            MinStep = 1 // mark
        }
    };

    public void Timer_Tick(object? sender, EventArgs e)
    {
        Dispatcher.CurrentDispatcher.Invoke(() =>
        {
            RemoveItem();
            AddItem();
        });
    }
}