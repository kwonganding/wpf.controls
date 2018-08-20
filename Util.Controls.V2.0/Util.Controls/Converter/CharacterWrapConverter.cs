using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Util.Controls
{
    public class CharacterWrapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            str = string.Join("\u200B", str.ToCharArray());//处理中英文混合断行问题
            str = str.Replace("\u200B\r\n\u200B", "");
            str = str.Replace("\u200B\r\u200B", "");
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            return str.Replace("\u200B", "");
        }
    }
}
