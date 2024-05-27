using System;
using System.Globalization;
using System.Windows.Data;

namespace App01.VLC.Converter;

public class StringToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var text = value?.ToString();
        // 这里假设至少需要10个字节才能启用按钮
        return text is { Length: > 10 };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}