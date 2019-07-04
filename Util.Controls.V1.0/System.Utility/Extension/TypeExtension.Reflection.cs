using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    /// <summary>
    /// 反射操作扩展
    /// </summary>
    public static partial class TypeExtension
    {
        #region IsInstanceOfType：判断当前对象是否为指定类型的实例

        /// <summary>
        /// 判断当前对象是否为指定类型的实例
        /// </summary>
        public static bool IsInstanceOfType(this Type instanceType, Type type)
        {
            if (instanceType == type)
            {
                return true;
            }

            foreach (Type iface in instanceType.GetInterfaces())
            {
                if (iface == type)
                {
                    return true;
                }
            }

            Type sType = instanceType;
            while (instanceType != Constants.ObjectType)
            {
                if (type == instanceType)
                {
                    return true;
                }
                instanceType = instanceType.BaseType;
            }

            //Type dType = type;
            //while (dType != null && dType != Constants.ObjectType)
            //{
            //    if (dType == sType)
            //    {
            //        return true;
            //    }
            //    dType = dType.BaseType;
            //}
            return false;
        }

        /// <summary>
        /// 判断当前对象是否为指定类型的实例
        /// </summary>
        public static bool IsInstanceOfType(this object instance, Type type)
        {
            if (instance == null) return false;
            Type instanceType = instance.GetType();

            return IsInstanceOfType(instanceType, type);
        }

        /// <summary>
        /// 判断当前对象是否为指定类型的实例
        /// </summary>
        public static bool IsInstanceOfT<T>(this Type instanceType)
        {
            return IsInstanceOfType(instanceType, typeof(T));
        }

        /// <summary>
        /// 判断当前对象是否为指定类型的实例
        /// </summary>
        public static bool IsInstanceOfT<T>(this object instance)
        {
            if (instance == null) return false;
            Type instanceType = instance.GetType();
            return IsInstanceOfType(instanceType, typeof(T));
        }

        #endregion

        #region IsCompatibleType：判断两种类型是否兼容
        /// <summary>
        /// 判断两种类型是否兼容
        /// </summary>
        public static bool IsCompatibleType(this Type type1, Type type2)
        {
            if (type1 == type2)
                return true;

            if (type1.IsEnum && Enum.GetUnderlyingType(type1) == type2)
                return true;

            Type underlyingType1 = Nullable.GetUnderlyingType(type1);
            Type underlyingType2 = Nullable.GetUnderlyingType(type2);

            if (underlyingType1 != null && underlyingType2 != null)
                return underlyingType1.IsCompatibleType(underlyingType2);

            return false;
        }
        #endregion

        #region IsImplementsInterface：判断当前类型是否实现了指定接口
        /// <summary>
        /// 判断当前类型是否实现了指定接口
        /// </summary>
        public static bool IsImplementsInterface(this Type objectType, Type interfaceType)
        {
            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException(string.Format("查找类似 指定的类型{0}不是接口类型。", interfaceType.Name), "interfaceType");
            }
            foreach (Type type in objectType.GetInterfaces())
            {
                if (type == interfaceType)
                {
                    return true;
                }
                if (type.Name.Contains("`"))
                {
                    if (type.Name == interfaceType.Name && type.Assembly == interfaceType.Assembly)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region GetValue-PropertyInfo
        /// <summary>
        /// 或其指定对象的属性值
        /// 其实也可以用dynamic更简单。
        /// </summary>
        public static object GetValue(this PropertyInfo pi, object obj)
        {
            if (pi == null || obj == null)
            {
                return null;
            }
            var get = pi.GetGetMethod();
            if (get == null)
            {
                return null;
            }
            return get.Invoke(obj, Constants.EmptyObjectArray);
        }
        #endregion

        #region CreateInstance：创建指定类型Type的实例
        /// <summary>
        /// 创建指定类型Type的实例，若创建出错返回null
        /// </summary>
        public static object CreateInstance(this Type @this)
        {
            try
            {
                var obj = Activator.CreateInstance(@this);
                return obj;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
