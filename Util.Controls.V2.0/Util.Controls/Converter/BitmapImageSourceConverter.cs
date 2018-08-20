using System;
using System.Globalization;
using System.Windows.Data;

namespace Util.Controls
{
    /// <summary>
    /// 位图图像源值转换器。
    /// <para>
    /// 支持从文件流、图片文件转换；支持设置图片解码宽度，以减少内存消耗。
    /// </para>
    /// </summary>
    public class BitmapImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var width = parameter.ToSafeDouble();
            switch (value)
            {
                case byte[] buffer:
                    return ImageHelper.CreateImageSourceFromByte(buffer, (int)width);
                case string fileName:
                    return ImageHelper.CreateImageSourceFromFile(fileName, (int)width);
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
