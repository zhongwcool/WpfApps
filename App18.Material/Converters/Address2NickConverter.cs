using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace App18.Material.Converters;

public class Address2NickConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not List<string> tos) return value.ToString();
        var names = tos.Select(to => to.Contains('<') ? to[..to.IndexOf('<')] : to).ToList();

        return $"To: {string.Join(", ", names)}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Cannot convert Type To DateTime");
    }
}