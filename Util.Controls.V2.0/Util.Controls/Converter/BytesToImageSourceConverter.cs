using System;
using System.Globalization;
using System.Windows.Data;

namespace Util.Controls
{
    /// <summary>
    /// 数据流转化为ImageSource的转换器
    /// </summary>
    public class BytesToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var buf = value as byte[];
            if (buf == null) return null;

            var width = parameter.ToSafeDouble();
            return ImageHelper.CreateImageSourceFromByte(buf, (int)width);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}