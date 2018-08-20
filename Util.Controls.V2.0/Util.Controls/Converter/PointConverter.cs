using System.Windows;
using System.Windows.Data;

namespace Util.Controls
{
    public class PointConverter : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length != 2)
                return default(Point);
            var x = (double)values[0];
            var y = (double)values[1];
            return new Point(x, y);
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is Point))
                return new object[] { 0d, 0d };
            var position = (Point)value;
            return new object[] { position.X, position.Y };
        }
    }
}
