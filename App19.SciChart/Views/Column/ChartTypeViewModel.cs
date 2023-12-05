// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ChartTypeViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System;
using System.Collections.Generic;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace App19.Views.Column;

public class ChartTypeViewModel : BaseViewModel
{
    private string _typeName;
    private bool _isOneHundredPercent;

    public ChartTypeViewModel(IEnumerable<IRenderableSeriesViewModel> rSeriesViewModels, Type type,
        bool isOneHundredPercent, bool isSideBySide)
    {
        RenderableSeriesViewModels = rSeriesViewModels;

        _isOneHundredPercent = isOneHundredPercent;
        IsSideBySide = isSideBySide;
        _typeName = GenerateChartName(type);
    }

    #region Properties

    public IEnumerable<IRenderableSeriesViewModel> RenderableSeriesViewModels { get; set; }

    public string TypeName
    {
        get => _typeName;
        set
        {
            _typeName = value;
            OnPropertyChanged("TypeName");
        }
    }

    public bool IsOneHundredPercent
    {
        get => _isOneHundredPercent;
        set
        {
            _isOneHundredPercent = value;
            OnPropertyChanged("TypeName");
        }
    }

    public bool IsSideBySide { get; }

    public string AxisFormatting => _isOneHundredPercent ? "#0'%'" : "";

    #endregion

    private string GenerateChartName(Type type)
    {
        var result = _isOneHundredPercent ? "100% " : "";
        if (type == typeof(IStackedColumnRenderableSeries))
        {
            result += "Stacked columns";
            if (IsSideBySide) result += " side-by-side";
        }
        else
        {
            result += "Stacked mountains";
        }

        return result;
    }
}