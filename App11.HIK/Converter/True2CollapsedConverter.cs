using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace App11.HIK.Converter;

public class True2CollapsedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool status) return Visibility.Visible;
        return status ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}