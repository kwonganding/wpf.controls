using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Util.Controls
{
    /// <summary>
    /// 获取Thickness固定值double
    /// </summary>
    public class ThicknessToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var thickness = (Thickness)value;
            return thickness.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}