using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace App21.Skeleton.Converter;

public class BrushOpacityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not SolidColorBrush brush) return null;
        var opacity = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
        return new SolidColorBrush(brush.Color)
        {
            Opacity = opacity
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}