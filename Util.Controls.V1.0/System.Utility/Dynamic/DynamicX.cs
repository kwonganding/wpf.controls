using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace System
{
    /// <summary>
    /// 自定义实现的动态类型，提供运行时的赋值、取值等基本操作
    /// </summary>
    [Serializable]
    public class DynamicX : DynamicObject, IDynamicEnable
    {
        /// <summary>
        /// 存储成员键值的字典
        /// </summary>
        public Dictionary<string, object> Members;

        public DynamicX()
        {
            this.Members = new Dictionary<string, object>();
        }


        /// <summary>
        /// 设置指定属性值
        /// </summary>
        public void Set(string propertyName, object value)
        {
            this.TrySetMember(new SetPropertyBinder(propertyName), value);
        }

        /// <summary>
        /// 获取指定属性名称的值
        /// </summary>
        public object Get(string propertyName)
        {
            object value;
            this.TryGetMember(new GetPropertyBinder(propertyName), out value);
            return value;
        }

        #region DynamicObject Override

        /// <summary>
        /// 获取动态成员值
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            this.Members.TryGetValue(binder.Name, out result);
            return true;
        }

        /// <summary>
        /// 设置动态成员值
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.Members[binder.Name] = value;
            return true;
        }

        /// <summary>
        /// 获取动态成员
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.Members.Keys;
        }
        #endregion

        /// <summary>
        /// 根据编码名称字典转换为格式化的字符串
        /// </summary>
        /// <param name="codenames"></param>
        /// <returns></returns>
        public string ToString(Dictionary<string, string> codenames)
        {
            if (this.Members.IsInvalid())
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            this.Members.ForEach(s =>
                                     {
                                         if (s.Key != "XLYLogString")
                                         {
                                             string name = codenames.TryGetValue(s.Key, out name) ? name : s.Key;
                                             sb.Append(name).Append(" : ").Append(s.Value.ToSafeString());
                                             sb.Append("\t");
                                         }
                                     });
            return sb.ToString().TrimEnd(Environment.NewLine);
        }

        /// <summary>
        /// 动态类型扩展
        /// </summary>
        public DynamicX Dynamic
        {
            get { return this._Dynamic; }
            set { this._Dynamic = value; }
        }
        private DynamicX _Dynamic;
    }
}
