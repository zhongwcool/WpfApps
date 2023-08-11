using System;
using System.Globalization;
using System.Windows.Data;
using Humanizer;

namespace App18.Material.Converters;

public class DateToHumanizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DateTime timeAgo) return string.Empty;
        var timeAgoString = timeAgo.Humanize();

        return timeAgoString;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Cannot convert Type To DateTime");
    }
}