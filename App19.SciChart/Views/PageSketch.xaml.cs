using System;
using System.Windows.Controls;
using System.Windows.Media;
using App19.ViewModels;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;

namespace App19.Views;

public partial class PageLineChart : UserControl
{
    public PageLineChart()
    {
        InitializeComponent();
        ReduceBackground();

        LineChart0();
        LineChart1();
        LineChart2();
        ColumnChart1();
    }

    private void LineChart0()
    {
        SciChart0.DataContext = new Chart0ViewModel(false, true);
    }

    private void ColumnChart1()
    {
        var dataSeries = new UniformXyDataSeries<double> { SeriesName = "Histogram" };

        var yValues = new[]
            { 0.1, 0.2, 0.4, 0.8, 1.1, 1.5, 2.4, 4.6, 8.1, 11.7, 14.4, 16.0, 13.7, 10.1, 6.4, 3.5, 2.5, 1.4, 0.4, 0.1 };

        using (SciChart1.SuspendUpdates())
        {
            foreach (var t in yValues)
                // DataSeries for appending data
                dataSeries.Append(t);

            columnSeries.DataSeries = dataSeries;
        }

        SciChart1.ZoomExtents();
    }

    private void LineChart2()
    {
        var mountainDataSeries = new XyDataSeries<DateTime, double>();

        // 生成正弦函数的数据 y=A*sin(ωx + φ)
        var a = 1.0; // 振幅
        var w = 2 * Math.PI / 10.0; // 角频率
        var phi = Math.PI / 2.0; // 初相位
        // 推算过去12个月的月份
        var currentMonth = DateTime.Now;
        currentMonth = currentMonth.AddMonths(-13);

        // 输出函数值
        for (var x = 1; x <= 13; x++)
        {
            var y = a * Math.Sin(w * x + phi);
            var date = currentMonth.AddMonths(x);
            mountainDataSeries.Append(date, y);
        }

        MountainSeries1.DataSeries = mountainDataSeries.ToSpline(10);
        ScatterSeries1.DataSeries = mountainDataSeries;
    }

    private void LineChart1()
    {
        var lineDataSeries = new XyDataSeries<double>();

        var random = new Random();
        for (var x = 0; x < 12; x++)
        {
            // Compute a slope function with some noise
            var y = x != 0 ? Math.Pow(x + random.NextDouble(), 0.291) : Math.Pow((double)x + 1, 0.3);
            lineDataSeries.Append(x, y);
        }

        MountainSeries0.DataSeries = lineDataSeries.ToSpline(10);
        ScatterSeries0.DataSeries = lineDataSeries;
    }

    private void ReduceBackground()
    {
        if (Background is not SolidColorBrush brush) return;
        // 创建具有修改透明度的新 Brush
        var modifiedBackground = new SolidColorBrush(brush.Color)
        {
            Opacity = 0.6 // 设置新的透明度
        };

        // 将按钮的背景色设置为修改后的 Brush
        Background = modifiedBackground;
    }
}