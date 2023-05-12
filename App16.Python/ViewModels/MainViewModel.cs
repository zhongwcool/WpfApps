using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;
using App16.Python.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;

namespace App16.Python.ViewModels;

public class MainViewModel : ObservableObject
{
    public SeriesCollection LineSeriesCollection { get; set; }

    // X轴的Label
    public List<string> Labels { get; set; } = new();

    public MainViewModel()
    {
        AxisXMax = TabelShowCount - 1;
        AxisXMin = 0;
        AxisYMax = 10;
        AxisYMin = 0;

        // ValueList = new ChartValues<double>();
        LineSeriesCollection = new SeriesCollection();

        CustomFormatterX = CustomFormattersX;
        CustomFormatterY = CustomFormattersY;

        var lineseries = new LineSeries
        {
            DataLabels = true,
            Values = ValueList
        };
        LineSeriesCollection.Add(lineseries);

        ValueList.CollectionChanged += ValueListOnCollectionChanged;
    }

    private void ValueListOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var valueListCount = ValueList.Count;

        if (valueListCount >= TabelCapacity)
        {
            Labels.RemoveRange(0, valueListCount - TabelShowCount);
            ValueList.CollectionChanged -= ValueListOnCollectionChanged;
            ValueList.Clear();
            ValueList.AddRange(_shadowValue.GetList());
            AxisXMax = TabelShowCount - 1;
            AxisXMin = 0;
            ValueList.CollectionChanged += ValueListOnCollectionChanged;
            return;
        }

        //y轴的设置
        if (valueListCount >= TabelShowCount)
        {
            AxisXMax = valueListCount - 1;
            AxisXMin = valueListCount - TabelShowCount;
        }
    }

    private readonly Random _random = new();

    private double _axisXMax;

    public double AxisXMax
    {
        get => _axisXMax;
        set => SetProperty(ref _axisXMax, value);
    }

    private double _axisXMin;

    public double AxisXMin
    {
        get => _axisXMin;
        set => SetProperty(ref _axisXMin, value);
    }

    private double _axisYMax;

    public double AxisYMax
    {
        get => _axisYMax;
        set => SetProperty(ref _axisYMax, value);
    }

    private double _axisYMin;

    public double AxisYMin
    {
        get => _axisYMin;
        set => SetProperty(ref _axisYMin, value);
    }

    public Func<double, string> CustomFormatterX { get; set; }
    public Func<double, string> CustomFormatterY { get; set; }

    private string CustomFormattersX(double val)
    {
        return $"{val}天";
    }

    private string CustomFormattersY(double val)
    {
        return $"{val}mV";
    }

    //绑定的X轴数据
    private ChartValues<double> ValueList { get; set; } = new();

    //表中最大容纳个数
    private const int TabelShowCount = 10;
    private readonly CircularQueue<double> _shadowValue = new(TabelShowCount);
    private const int TabelCapacity = 100;

    //曲线图的总个数
    // private int ValueListCount = 0;

    public void Timer_Tick(object? sender, EventArgs e)
    {
        Dispatcher.CurrentDispatcher.Invoke(() =>
        {
            //更新横坐标时间
            Labels.Add(DateTime.Now.ToString("HH:mm:ss.fff"));

            var trend = _random.Next(-10, 100);
            //向图表中添加数据
            ValueList.Add(trend);
            _shadowValue.Enqueue(trend);

            //确保Y轴曲线不会超过图表
            var maxY = (int)ValueList.Max();
            AxisYMax = maxY + 15;

            //Y轴保持数据居中（曲线会上下晃动）
            //int minY = ValueList.Count == 1 ? 0 : (int)ValueList.Min();
            //AxisYMin = minY - 10;
        });
    }
}