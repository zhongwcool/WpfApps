using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace App08.Metro.Converter;

public class False2CollapsedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (null == value) return Visibility.Visible;
        return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}