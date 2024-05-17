using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace App01.VLC.Converter;

public class True2VisibleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool status) return Visibility.Collapsed;
        return status ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}