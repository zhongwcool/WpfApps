using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using App22.Selected.Enums;
using App22.Selected.Models;

namespace App22.Selected.Converters;

public class LockStatusToStyleConverter : IMultiValueConverter
{
    public object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        var item = values[0] as StatusItem;
        if (values[1] is not Border border) return null;
        return item?.Status switch
        {
            LockStatus.Locked => Application.Current.TryFindResource("LockedStyle") as Style,
            LockStatus.Unlocked => Application.Current.TryFindResource("UnlockedStyle") as Style,
            LockStatus.Locking => Application.Current.TryFindResource("LockingStyle") as Style,
            _ => Application.Current.TryFindResource("UnknownStyle") as Style
        };
    }

    public object[] ConvertBack(object? value, Type[] targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}