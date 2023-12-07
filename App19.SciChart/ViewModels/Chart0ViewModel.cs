using System.Collections.Generic;
using App19.Views.Column;
using CommunityToolkit.Mvvm.ComponentModel;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;

namespace App19.ViewModels;

public class Chart0ViewModel : ObservableObject
{
    public Chart0ViewModel(bool isOneHundredPercent, bool isSideBySide)
    {
        RenderableSeriesViewModels = new List<IRenderableSeriesViewModel>();

        _isOneHundredPercent = isOneHundredPercent;
        IsSideBySide = isSideBySide;

        var xValues = DashboardDataHelper.GetXValues();
        var yValues = DashboardDataHelper.GetYValues();
        var columnStyleKeys = DashboardDataHelper.GetColumnStyleKeys();
        for (var i = 0; i < 5; i++)
        {
            var dataSeries = new XyDataSeries<double, double> { SeriesName = "Series " + (i + 1) };
            dataSeries.Append(xValues, yValues[i]);

            var seriesViewModel = isSideBySide
                ? GetStackedColumn(dataSeries, isOneHundredPercent, columnStyleKeys[i], i.ToString())
                : GetStackedColumn(dataSeries, isOneHundredPercent, columnStyleKeys[i], "");
            RenderableSeriesViewModels.Add(seriesViewModel);
        }
    }

    private static IRenderableSeriesViewModel GetStackedColumn(IXyDataSeries dataSeries, bool isOneHundredPercent,
        string styleKey, string group)
    {
        return new StackedColumnRenderableSeriesViewModel
        {
            DataSeries = dataSeries,
            StyleKey = styleKey,
            IsOneHundredPercent = isOneHundredPercent,
            StackedGroupId = group
        };
    }

    private static IRenderableSeriesViewModel GetStackedMountain(IXyDataSeries dataSeries, bool isOneHundredPercent,
        string styleKey)
    {
        return new StackedMountainRenderableSeriesViewModel
        {
            DataSeries = dataSeries,
            StyleKey = styleKey,
            IsOneHundredPercent = isOneHundredPercent
        };
    }

    public bool IsSideBySide { get; }

    private bool _isZoomExtendsAnimated = true;

    public bool IsZoomExtendsAnimated
    {
        get => _isZoomExtendsAnimated;
        set
        {
            _isZoomExtendsAnimated = value;
            OnPropertyChanged();
        }
    }

    public IViewportManager ViewportManager { get; } = new DefaultViewportManager();

    private readonly bool _isOneHundredPercent;
    public string AxisFormatting => _isOneHundredPercent ? "#0'%'" : "";

    public List<IRenderableSeriesViewModel> RenderableSeriesViewModels { get; set; }
}