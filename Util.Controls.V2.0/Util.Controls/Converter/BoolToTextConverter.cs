using System;
using System.Globalization;
using System.Windows.Data;

namespace Util.Controls
{
    /// <summary>
    /// bool转换为文本描述，根据parameter转发，parameter格式如：是;否，开启;未开启
    /// </summary>
    public sealed class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var arr = parameter.ToSafeString().Split(';');
            if (arr.Length < 2) return Binding.DoNothing;
            var b = value.ToSafeString().ToBoolean();
            return b ? arr[0] : arr[1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}