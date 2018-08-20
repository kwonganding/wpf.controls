using System;
using System.Globalization;
using System.Windows.Data;

namespace Util.Controls
{
    /// <summary>
    ///获取枚举描述信息
    /// </summary>
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ev = value as Enum;
            return ev.GetDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}