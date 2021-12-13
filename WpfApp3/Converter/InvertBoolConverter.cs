using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp3.Converter;

public class InvertBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool sour) return !sour;

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool dest) return !dest;

        return Binding.DoNothing;
    }
}