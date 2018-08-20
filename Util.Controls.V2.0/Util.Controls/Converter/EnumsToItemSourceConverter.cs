using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Util.Controls
{
    /// <summary>
    /// 根据一个枚举数组返回一个可绑定的EnumItem（包括枚举对象、枚举描述）集合列表
    /// </summary>
    public class EnumsToItemSourceConverter: IValueConverter
    {
        public object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            if (value == null)
            {
                EnumTypeToItemSourceConverter converter = new EnumTypeToItemSourceConverter();
                return converter.Convert(value, targetType, parameter, culture);
            } 
            var vs = (IList) value;
            var items = new EnumItem[vs.Count];
            for (int i = 0; i < vs.Count; i++)
            {
                items[i].Value = vs[i];
                items[i].Display = (items[i].Value as Enum).GetDescription();
            }
            return items;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
