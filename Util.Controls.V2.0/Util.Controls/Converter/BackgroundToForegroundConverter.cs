using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Util.Controls
{
    /// <summary>
    /// 根据背景色获取前景色。获取反色值
    /// </summary>
    public class BackgroundToForegroundConverter : IValueConverter
    {
        private Color IdealTextColor(Color bg)
        {
            const int nThreshold = 105;
            var bgDelta = System.Convert.ToInt32((bg.R -255) + (bg.G -255) + (bg.B -255));
            var foreColor = (255 - bgDelta < nThreshold) ? Colors.Black : Colors.White;
            return foreColor;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush)
            {
                var idealForegroundColor = ColorHelper.GenerateInvertColor((value as SolidColorBrush).Color);
                var foreGroundBrush = new SolidColorBrush(idealForegroundColor);
                foreGroundBrush.Freeze();
                return foreGroundBrush;
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// 获取一个颜色浅色转换器
    /// </summary>
    public class LightBrushConverter : IValueConverter
    {
        private Color IdealTextColor(Color bg)
        {
            const int nThreshold = 105;
            var bgDelta = System.Convert.ToInt32((bg.R * 0.299) + (bg.G * 0.587) + (bg.B * 0.114));
            var foreColor = (255 - bgDelta < nThreshold) ? Colors.Black : Colors.White;
            return foreColor;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush)
            {
                var idealForegroundColor = ColorHelper.GenerateGradientColor(((SolidColorBrush)value).Color,12,2);
                var foreGroundBrush = new SolidColorBrush(idealForegroundColor);
                foreGroundBrush.Freeze();
                return foreGroundBrush;
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}