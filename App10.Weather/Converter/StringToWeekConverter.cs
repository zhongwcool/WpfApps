using System;
using System.Globalization;
using System.Windows.Data;

namespace App10.Weather.Converter;

public class StringToWeekConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string dateTimeString) return string.Empty;
        if (string.IsNullOrEmpty(dateTimeString)) return string.Empty;

        var date = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd", CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal);
        var week = CultureInfo.GetCultureInfo("zh-CN").DateTimeFormat.GetDayName(date.DayOfWeek);

        return week;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}