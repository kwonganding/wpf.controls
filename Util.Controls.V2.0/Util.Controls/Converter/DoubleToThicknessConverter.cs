using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Util.Controls
{
    /// <summary>
    /// 获取Thickness固定值double
    /// </summary>
    public class DoubleToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String thickness = value.ToString();

            Thickness borderThickness = new Thickness(System.Convert.ToDouble(thickness, System.Globalization.CultureInfo.InvariantCulture),
                System.Convert.ToDouble(thickness, System.Globalization.CultureInfo.InvariantCulture),
                System.Convert.ToDouble(thickness, System.Globalization.CultureInfo.InvariantCulture),
                System.Convert.ToDouble(thickness, System.Globalization.CultureInfo.InvariantCulture));

            return borderThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}