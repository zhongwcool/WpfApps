using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;

namespace App19.SciChart.Audio;

public class AudioAnalyzerPaletteProvider : IFillPaletteProvider
{
    public HeatmapColorPalette Palette { get; set; }

    public void OnBeginSeriesDraw(IRenderableSeries rSeries)
    {
    }

    public Brush OverrideFillBrush(IRenderableSeries rSeries, int index, IPointMetadata metadata)
    {
        if (Palette == null) return Brushes.Transparent;
        var color = GetColorInternal(rSeries, index);
        return new SolidColorBrush(color);
    }

    private Color GetColorInternal(IRenderableSeries rSeries, int index)
    {
        var value = (double)rSeries.DataSeries.YValues[index];
        return Palette.GetColor(value).ToColor();
    }
}