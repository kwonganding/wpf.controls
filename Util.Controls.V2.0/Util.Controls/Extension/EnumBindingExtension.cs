using System;
using System.Collections;
using System.Windows.Markup;

namespace Util.Controls
{
    /// <summary>
    /// 枚举数据源绑定扩展。
    /// 使用示例：<ComboBox ItemsSource="{EnumBinding EnumType=HandoffBehavior}"/>
    /// </summary>
    [MarkupExtensionReturnType(typeof(IEnumerable))]
    public sealed class EnumBindingExtension : MarkupExtension
    {
        [ConstructorArgument("enumType")]
        public Type Type { get; set; }

        public EnumBindingExtension()
        { }

        public EnumBindingExtension(Type enumType)
        {
            this.Type = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.Type == null || !this.Type.IsEnum)
                throw new ArgumentNullException("enumType", "The argument[enumType] is invalid");
            return Util.Cache.CacheManager.GetCache<EnumItem[]>("EnumItems_Cache_" + Type.FullName, Util.Cache.CacheTime.Time_30min, () =>
            {
                var vs = Enum.GetValues(Type);

                EnumItem[] items = new EnumItem[vs.Length];
                for (int i = 0; i < vs.Length; i++)
                {
                    items[i].Value = vs.GetValue(i);
                    items[i].Display = (items[i].Value as Enum).GetDescription();
                }
                return items;
            });
        }
    }
}