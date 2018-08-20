using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Util.Controls
{
    /// <summary>
    /// 根据某一 <see cref="UIElement"/> 的 <see cref="UIElement.Visibility"/> 切换显示Grid的行或列。
    /// <para>
    /// 行或列按照 <see cref="GridUnitType.Star"/> 显示，支持从转换参数中指定 <see cref="GridUnitType.Star"/> 值。
    /// </para>
    /// </summary>
    public class VisibilityToStarGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Collapsed)
            {
                return new GridLength(0, GridUnitType.Pixel);
            }
            else
            {
                if (parameter == null)
                {
                    return new GridLength(1d, GridUnitType.Star);
                }

                if (!double.TryParse(parameter.ToString(), out double star))
                {
                    throw new ArgumentNullException(nameof(parameter), $"请指定有效的{GridUnitType.Star}值!");
                }

                return new GridLength(star, GridUnitType.Star);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
