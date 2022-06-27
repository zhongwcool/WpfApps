using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace App11.HIK.Converter;

public class TypeToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return new BitmapImage(new Uri($"pack://application:,,,/Resources/Device/{App.CurrentTheme}/0.png"));

        if (value is int index)
        {
            return new BitmapImage(new Uri($"pack://application:,,,/Resources/Device/{App.CurrentTheme}/{index}.png"));
        }

        if (value is string index0)
        {
            return new BitmapImage(new Uri($"pack://application:,,,/Resources/Device/{App.CurrentTheme}/{index0}.png"));
        }

        return new BitmapImage(new Uri($"pack://application:,,,/Resources/Device/{App.CurrentTheme}/0.png"));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Cannot convert Type To ImageSource");
    }
}