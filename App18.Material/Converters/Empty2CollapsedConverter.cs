using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace App18.Material.Converters;

public class Empty2CollapsedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string link) return Visibility.Collapsed;
        return string.IsNullOrEmpty(link) ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}