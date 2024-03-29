﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace App10.Weather.Converter;

public class IndexToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return new BitmapImage(new Uri($"pack://application:,,,/Resources/{App.CurrentTheme}/99@1x.png"));

        if (value is int index)
        {
            return new BitmapImage(new Uri($"pack://application:,,,/Resources/{App.CurrentTheme}/{index}@1x.png"));
        }

        if (value is string index0)
        {
            return new BitmapImage(new Uri($"pack://application:,,,/Resources/{App.CurrentTheme}/{index0}@1x.png"));
        }

        return new BitmapImage(new Uri($"pack://application:,,,/Resources/{App.CurrentTheme}/99@1x.png"));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Cannot convert point collection to double");
    }
}