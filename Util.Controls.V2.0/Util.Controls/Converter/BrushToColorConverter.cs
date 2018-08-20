using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Util.Controls
{
    public sealed class BrushToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var c = (SolidColorBrush) value;
            return c?.Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var c = (Color) value;
            return new SolidColorBrush(c);
        }
    }
}