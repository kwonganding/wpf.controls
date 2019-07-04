using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 公共常量定义
    /// </summary>
    public static class Constants
    {
        public static readonly object[] EmptyObjectArray = new object[0];
        public static readonly string[] EmptyStringArrray = new string[0];

        public static readonly Type VoidType = typeof(void);
        public static readonly Type ObjectType = typeof(object);
        public static readonly Type StringType = typeof(string);
        public static readonly Type DoubleType = typeof(double);
        public static readonly Type DateTimeType = typeof(DateTime);
        public static readonly Type NullableDateTimeType = typeof(DateTime?);
        public static readonly Type NullGenericListType = typeof(List<>);
        public static readonly Type NullableType = typeof(Nullable<>);
        public static readonly Type EnumType = typeof(Enum);
    }
}
