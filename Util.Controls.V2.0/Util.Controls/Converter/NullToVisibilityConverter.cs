using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Util.Controls
{
    /// <summary>
    /// 根据值是否为null控制元素的显示，value==null 不显示
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}