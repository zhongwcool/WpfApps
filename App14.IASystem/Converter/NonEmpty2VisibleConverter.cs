﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace App14.IASystem.Converter;

public class NonEmpty2VisibleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !string.IsNullOrEmpty((string)value) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}