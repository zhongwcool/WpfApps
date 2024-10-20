using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using App22.Selected.Models;

namespace App22.Selected.Converters;

public class StatusToStyleConverter : IMultiValueConverter
{
    public object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        var item = values[0] as StatusItem;
        if (values[1] is not Border border) return null;
        return item?.Status switch
        {
            "Locked" => border.TryFindResource("LockedStyle") as Style,
            "Unlocked" => border.TryFindResource("UnlockedStyle") as Style,
            _ => null
        };
    }

    public object[] ConvertBack(object? value, Type[] targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}