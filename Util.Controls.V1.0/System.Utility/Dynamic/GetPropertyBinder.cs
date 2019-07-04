using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace System
{
    /// <summary>
    /// 获取属性
    /// </summary>
    internal class GetPropertyBinder : GetMemberBinder
    {
        public GetPropertyBinder(string propertyName)
            : base(propertyName, false)
        {
        }

        public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            return new DynamicMetaObject(
              System.Linq.Expressions.Expression.Throw(
                System.Linq.Expressions.Expression.New(
                  typeof(InvalidOperationException).GetConstructor(new Type[] { typeof(string) }),
                  new System.Linq.Expressions.Expression[] { System.Linq.Expressions.Expression.Constant("Error") }),
              this.ReturnType), BindingRestrictions.Empty);
        }
    }
}
