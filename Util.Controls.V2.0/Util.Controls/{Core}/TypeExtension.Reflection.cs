using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Util.Controls
{
    /// <summary>
    /// 反射操作扩展
    /// </summary>
    public static partial class TypeExtension
    {
        private static readonly BindingFlags MethodFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags
                .Static;
        #region Type：IsCompatibleType：判断两种类型是否相互兼容
        /// <summary>
        /// 判断两种类型是否相互兼容
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

        #region Type：IsImplementsInterface：判断当前类型是否实现了指定接口
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

        #region PropertyInfo：GetValue-PropertyInfo
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

        #region Type：CreateInstance：创建指定类型Type的实例
        /// <summary>
        /// 创建指定类型Type的实例，若创建出错则返回null并忽略异常。
        /// </summary>
        public static object CreateInstance(this Type @this)
        {
            return CreateInstance(@this, (ex) => Console.WriteLine(ex.ToLogString()));
        }

        /// <summary>
        /// 创建指定类型的实例，若创建出错则返回null。
        /// </summary>
        /// <param name="this">要创建实例的类型。</param>
        /// <param name="callback">创建类型实例时引发异常后的错误处理回调。</param>
        /// <returns></returns>
        public static object CreateInstance(this Type @this, Action<Exception> callback)
        {
            try
            {
                return DangerousCreateInstance(@this);
            }
            catch (Exception ex)
            {
                callback?.Invoke(ex);
                return null;
            }
        }

        /// <summary>
        /// 创建指定类型的实例。
        /// <para>
        /// 该方法使用类型进程反射创建实例，调用者应捕获可能引发的异常。
        /// </para>
        /// </summary>
        /// <param name="this">要创建实例的类型。</param>
        /// <returns>返回创建的类型的实例。</returns>
        public static object DangerousCreateInstance(this Type @this)
        {
            var obj = Activator.CreateInstance(@this);
            return obj;
        }

        #endregion

        #region T：IsTypeOrInheritsOf

        /// <summary>
        ///  判断类型@this是否目标类型type的同类或子类（包括继承关系）
        ///  注意：扩展方法对象必须是Type
        /// </summary>
        public static bool IsTypeOrInheritsOf(this Type @this, Type type)
        {
            Type objectType = @this;

            while (true)
            {
                if (objectType.Equals(type))
                {
                    return true;
                }

                if ((objectType == objectType.BaseType) || (objectType.BaseType == null))
                {
                    return false;
                }

                objectType = objectType.BaseType;
            }
        }

        #endregion

        #region T：InvokeMethod

        public static T ExcuteMethod<T>(this object host, string methodName, bool methodisStatic, object[] param)
        {
            if (host == null || string.IsNullOrEmpty(methodName))
                return default(T);
            var type = host.GetType();
            var methodInfos = type.GetMethods(MethodFlags);
            var parmlength = param == null ? 0 : param.Length;
            var find = methodInfos.FirstOrDefault(o => o.Name == methodName && o.GetParameters().Length == parmlength);
            if (find != null)
                return (T)find.Invoke(methodisStatic ? null : host, MethodFlags, null, param,
                    CultureInfo.CurrentCulture);
            return default(T);
        }

        public static T ExcuteMethod<T>(this Type host, string methodName, bool methodisStatic, object[] param)
        {
            if (host == null || string.IsNullOrEmpty(methodName))
                return default(T);
            var methodInfos = host.GetMethods(MethodFlags);
            var parmlength = param == null ? 0 : param.Length;
            var find = methodInfos.FirstOrDefault(o => o.Name == methodName && o.GetParameters().Length == parmlength);
            if (find != null)
                return (T)find.Invoke(methodisStatic ? null : Activator.CreateInstance(host), MethodFlags, null, param,
                    CultureInfo.CurrentCulture);
            return default(T);
        }

        /// <summary>
        ///     An object extension method that executes the method on a different thread, and waits for the result.
        /// parameters可以为null
        /// </summary>
        public static T InvokeMethod<T>(this object obj, string methodName, params object[] parameters)
        {
            var type = obj.GetType();
            MethodInfo method = (parameters == null || parameters[0] == null) ? type.GetMethod(methodName) : type.GetMethod(methodName, parameters.Select(o => o.GetType()).ToArray());
            var p = (parameters == null || parameters[0] == null) ? null : parameters;
            object value = method.Invoke(obj, p);
            return (value is T ? (T)value : default(T));
        }

        #endregion

        #region T：SetPropertyValue,SetFieldValue

        /// <summary>
        ///     A T extension method that sets property value.
        /// </summary>
        public static void SetPropertyValue<T>(this T @this, string propertyName, object value)
        {
            Type type = @this.GetType();
            PropertyInfo property = type.GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            property.SetValue(@this, value, null);
        }

        /// <summary>
        /// 安全设置属性值,如果未找到属性则不处理.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertySafeValue<T>(this T @this, string propertyName, object value)
        {
            Type type = @this.GetType();
            PropertyInfo property = type.GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (property == null) return;
            property.SetValue(@this, value, null);
        }

        /// <summary>
        ///     A T extension method that sets property value.
        /// </summary>
        public static object GetPropertyValue(this object @this, string propertyName)
        {
            Type type = @this.GetType();
            PropertyInfo property = type.GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            return property.GetValue(@this);
        }

        /// <summary>
        /// 设置对象字段值，支持继承对象中的字段，如果当期对象没搜索到，会递归到基类搜索，直到Object
        ///     A T extension method that sets field value.
        /// </summary>
        public static void SetFieldValue<T>(this T @this, string fieldName, object value)
        {
            Type type = @this.GetType();
            FieldInfo field = null;
            do
            {
                field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                type = type.BaseType;
            } while (field == null && type != Constants.ObjectType);
            if (field == null) return;
            try
            {
                field.SetValue(@this, value);
            }
            catch (Exception ex) { }
        }

        #endregion
    }
}
