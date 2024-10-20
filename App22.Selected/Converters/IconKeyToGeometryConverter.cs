using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace App22.Selected.Converters;

public class IconKeyToGeometryConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string iconKey)
        {
            return Application.Current.Resources[iconKey] as Geometry;
        }

        // If empty, return a hollow circle
        return Geometry.Parse("M 50,50 m -24,0 a 24,24 0 1,1 48,0 a 24,24 0 1,1 -48,0Z");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}