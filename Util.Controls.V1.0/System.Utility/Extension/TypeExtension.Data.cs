using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Dynamic;

namespace System
{
    /// <summary>
    /// 数据库类型的扩展
    /// </summary>
    public static partial class TypeExtension
    {
        #region ToDynamicCollection
        /// <summary>
        /// 转换为动态类型集合
        /// </summary>
        public static List<dynamic> ToDynamicCollection(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0 || dt.Columns.Count <= 0)
            {
                return new List<dynamic>();
            }
            var items = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                    items.Add(dyn);
                }
            }
            return items;
        }
        #endregion

        #region ToDynamic
        /// <summary>
        /// 把IDataReader的数据转换为动态类型
        /// 注意：如果数据库字段名称为“new”或“_”开头，则在前面添加关键字"xly"
        /// </summary>
        public static dynamic ToDynamic(this IDataReader reader)
        {
            if (reader == null)
            {
                return new ExpandoObject();
            }
            int len = reader.FieldCount;
            if (len <= 0)
            {
                return new ExpandoObject();
            }
            DynamicX item = new DynamicX();
            try
            {
                for (int i = 0; i < len; i++)
                {
                    //过滤关键字
                    var name = reader.GetName(i);
                    if (name.Equals("new") || name.StartsWith("_"))
                    {
                        name = name.Insert(0, "xly");
                    }
                    object value = null;
                    try
                    {
                        value = reader.GetValue(i);
                    }
                    catch{}
                    item.TrySetMember(new SetPropertyBinder(name), value);
                }
            }
            catch { }

            //reaturn
            return item;
        }
        #endregion

    }
}
