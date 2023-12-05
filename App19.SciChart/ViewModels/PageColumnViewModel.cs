using System;
using System.Collections.Generic;
using App19.Views.Column;
using CommunityToolkit.Mvvm.ComponentModel;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.RenderableSeries;

namespace App19.ViewModels;

public class PageColumnViewModel : ObservableObject
{
    private ChartTypeViewModel _currentChartType;
    private bool _isZoomExtendsAnimated = true;
    private int _selectedChartIndex;

    public PageColumnViewModel()
    {
        ChartTypesSource = new List<ChartTypeViewModel>
        {
            ChartTypeViewModelFactory.New(typeof(IStackedColumnRenderableSeries), false, true),
            ChartTypeViewModelFactory.New(typeof(IStackedColumnRenderableSeries), false, false),
            ChartTypeViewModelFactory.New(typeof(IStackedColumnRenderableSeries), true, false),
            ChartTypeViewModelFactory.New(typeof(IStackedMountainRenderableSeries), false, false),
            ChartTypeViewModelFactory.New(typeof(IStackedMountainRenderableSeries), true, false)
        };

        SpacingModes = new List<SpacingMode> { SpacingMode.Absolute, SpacingMode.Relative };
        SelectedChartIndex = 0;
    }

    public IViewportManager ViewportManager { get; } = new DefaultViewportManager();

    public List<SpacingMode> SpacingModes { get; set; }

    public List<ChartTypeViewModel> ChartTypesSource { get; }

    public ChartTypeViewModel CurrentChartType
    {
        get => _currentChartType;
        set
        {
            _currentChartType = value;
            OnPropertyChanged();

            // Invoke AnimateZoomExtents after binding engine has stabilised 
            ViewportManager.BeginInvoke(() =>
            {
                var duration = _isZoomExtendsAnimated ? TimeSpan.FromMilliseconds(250) : TimeSpan.FromMilliseconds(0);
                ViewportManager.AnimateZoomExtents(duration);
            });
        }
    }

    public int SelectedChartIndex
    {
        get => _selectedChartIndex;
        set
        {
            _selectedChartIndex = value;
            CurrentChartType = ChartTypesSource[value];
            OnPropertyChanged();
        }
    }

    public bool IsZoomExtendsAnimated
    {
        get => _isZoomExtendsAnimated;
        set
        {
            _isZoomExtendsAnimated = value;
            OnPropertyChanged();
        }
    }
}