using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Util.Controls
{
    /// <summary>
    /// System.Windows.Media.Color 转到String
    /// </summary>
    public sealed class ColorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var c = (System.Windows.Media.Color) value;
            return c.ToSafeString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var str = value.ToSafeString();
            if (!str.Contains("#"))
            {
                str = str.Insert(0, "#");
            }
            else
            {
                var index = str.LastIndexOf("#", StringComparison.Ordinal);
                if (index != 0)
                {
                    var newStr = str.Replace("#", "");
                    newStr = newStr.Trim();
                    str = newStr.Insert(0, "#");
                }
            }
            try
            {
                var c = ColorConverter.ConvertFromString(str);
                return c;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}