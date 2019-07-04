using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Utility.Logger;

namespace System
{
    public static partial class TypeExtension
    {
        /// <summary>
        /// 对象的深拷贝对象。
        /// 注意“T”及其里面的引用类型必须标记为可序列化。
        /// </summary>
        public static T Copy<T>(this T obj) where T : class
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                T CloneObject = default(T);

                try
                {
                    // 反序列化至另一个对象(即创建了一个原对象的深表副本) 
                    CloneObject = (T)bf.Deserialize(ms);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("深拷贝对象失败", ex);
                }

                return CloneObject;
            }
        }

        public static string GetPropertyName<T>(this Expression<Func<T, object>> expr)
        {
            return ((MemberExpression)expr.Body).Member.Name;
        }

        /// <summary>
        /// 有Bug，没有调通，后面来弄。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object Copy1(this object obj)
        {
            Object targetDeepCopyObj;
            Type targetType = obj.GetType();
            //值类型  
            if (targetType.IsValueType)
            {
                targetDeepCopyObj = obj;
            }
            else
            {//引用类型   
                //创建引用对象   
                targetDeepCopyObj = System.Activator.CreateInstance(targetType);

                System.Reflection.MemberInfo[] memberCollection = obj.GetType().GetMembers();

                foreach (System.Reflection.MemberInfo member in memberCollection)
                {
                    if (member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        var field = (System.Reflection.FieldInfo)member;
                        Object fieldValue = field.GetValue(obj);
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(targetDeepCopyObj, Copy(fieldValue));
                        }

                    }
                    else if (member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        var myProperty = (System.Reflection.PropertyInfo)member;
                        MethodInfo info = myProperty.GetSetMethod(false);
                        if (info != null)
                        {
                            object propertyValue = myProperty.GetValue(obj, null);
                            if (propertyValue is ICloneable)
                            {
                                myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                            }
                            else
                            {
                                myProperty.SetValue(targetDeepCopyObj, Copy(propertyValue), null);
                            }
                        }

                    }
                }
            }
            return targetDeepCopyObj;
        }
    }
}
