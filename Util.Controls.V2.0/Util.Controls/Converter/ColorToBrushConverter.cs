using System;
using System.Globalization;
using System.Windows.Data;

namespace Util.Controls
{
    /// <summary>
    /// System.Windows.Media.Color 转到SolidColorBrush
    /// </summary>
    public sealed class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var c = (System.Windows.Media.Color)value;
            return new System.Windows.Media.SolidColorBrush(c);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var c = (System.Windows.Media.SolidColorBrush)value;
            return c.Color;
        }
    }
}