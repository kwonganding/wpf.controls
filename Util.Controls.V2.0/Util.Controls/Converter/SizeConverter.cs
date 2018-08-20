using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Util.Controls
{
    public class SizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length != 2)
                return default(Size);
            var w = (double)values[0];
            var h = (double)values[1];
            return new Size(w, h);
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is Size))
                return new object[] { 0d, 0d };
            var size = (Size)value;
            return new object[] { size.Width, size.Height };
        }
    }
}
