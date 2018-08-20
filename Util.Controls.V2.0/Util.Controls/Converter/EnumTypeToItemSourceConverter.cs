using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Util.Controls
{
    /// <summary>
    /// 转换枚举值为数据源集合；
    /// 使用参数传入枚举类型，例如：ItemsSource="{Binding Converter={x:Static utc:XConverter.EnumToItemsSourceConverter}, ConverterParameter={x:Type utc:EnumThumbnail}}"
    /// 同时必须注意设置SelectedValuePath="Value"，然后直接使用SelectedValue设置、获取枚举对象即可
    /// </summary>
    public class EnumTypeToItemSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = parameter as Type;
            var vs = Enum.GetValues(type);
            EnumItem[] items = new EnumItem[vs.Length];
            for (int i = 0; i < vs.Length; i++)

            {
                items[i].Value = vs.GetValue(i);
                items[i].Display = (items[i].Value as Enum).GetDescription();
            }
            return items;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public struct EnumItem
    {
        public string Display { get; set; }
        public object Value { get; set; }
        public override string ToString()
        {
            return this.Display;
        }
    }
}
