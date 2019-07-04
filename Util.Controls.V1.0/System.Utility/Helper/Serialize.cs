using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace System.Utility.Helper
{
    /// <summary>
    /// 序列化的辅助操作类
    /// </summary>
    public class Serialize
    {
        #region 将对象序列化到XML文件中
        /// <summary>
        /// 将对象序列化到XML文件中
        /// </summary>
        /// <typeparam name="T">要序列化的类，即instance的类名</typeparam>
        /// <param name="instance">要序列化的对象</param>
        /// <param name="xmlFile">Xml文件名，表示保存序列化数据的位置</param>
        public static void SerializeToXML<T>(T instance, string xmlFile)
        {
            //创建XML序列化对象
            var serializer = new XmlSerializer(typeof(T));

            //创建文件流
            using (FileStream fs = new FileStream(xmlFile, FileMode.Create))
            {
                //开始序列化对象
                serializer.Serialize(fs, instance);
            }
        }
        #endregion

        #region 将XML文件反序列化为对象
        /// <summary>
        /// 将XML文件反序列化为对象
        /// </summary>
        /// <typeparam name="T">要获取的类</typeparam>
        /// <param name="xmlFile">Xml文件名，即保存序列化数据的位置</param>        
        public static T DeSerializeFromXML<T>(string xmlFile) where T : class
        {
            //创建XML序列化对象
            var serializer = new XmlSerializer(typeof(T));

            //创建文件流
            using (FileStream fs = new FileStream(xmlFile, FileMode.Open))
            {
                //开始反序列化对象
                return serializer.Deserialize(fs) as T;
            }
        }
        #endregion

        #region 将对象序列化到二进制文件中
        /// <summary>
        /// 将对象序列化到二进制文件中
        /// </summary>
        /// <param name="instance">要序列化的对象</param>
        /// <param name="fileName">文件名，保存二进制序列化数据的位置,后缀一般为.bin</param>
        public static void SerializeToBinary(object instance, string fileName)
        {
            //创建二进制序列化对象
            BinaryFormatter serializer = new BinaryFormatter();

            //创建文件流
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                //开始序列化对象
                serializer.Serialize(fs, instance);
            }
        }
        #endregion

        #region 将二进制文件反序列化为对象
        /// <summary>
        /// 将二进制文件反序列化为对象
        /// </summary> <typeparam name="T">要获取的类</typeparam>
        /// <param name="fileName">文件名，保存二进制序列化数据的位置</param>        
        public static T DeSerializeFromBinary<T>(string fileName) where T : class
        {
            //创建二进制序列化对象
            BinaryFormatter serializer = new BinaryFormatter();

            //创建文件流
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                //开始反序列化对象-
                return serializer.Deserialize(fs) as T;
            }
        }
        #endregion

        #region 将对象序列化成字符串
        /// <summary>
        /// 将对象序列化
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string SerializeToXML<T>(T instance)
        {
            using (var sw = new StringWriter())
            {
                var xs = new XmlSerializer(instance.GetType());
                xs.Serialize(sw, instance);
                return sw.ToString();
            }
        } 
        #endregion

        #region 将字符串范序列化成对象
        /// <summary>
        /// 将字符串范序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T DeSerializeToObject<T>(string s) where T : class
        {
            using (var sr = new StringReader(s))
            {
                var xz = new XmlSerializer(typeof(T));
                return (T)xz.Deserialize(sr);
            }
        } 
        #endregion
    }
}
