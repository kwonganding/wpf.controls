using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace System.Utility.Helper
{
    /// <summary>
    /// 获取随机值的辅助类
    /// </summary>
    public partial class Random
    {
        public const string NUMBER_AND_CHAR = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,T,U,W,X,Y,Z";
        public const string NUMBER = "0,1,2,3,4,5,6,7,8,9";
        public const string CHAR = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";

        #region NextNum：获取min到max间的随机数（包括min和max）
        /// <summary>
        /// 获取min到max间的随机数（包括min和max）
        /// </summary>
        public static int NextNum(int min, int max)
        {
            System.Random ran = new System.Random(Guid.NewGuid().GetHashCode());
            //默认是不包含max值的
            return ran.Next(min, max + 1);
        }
        #endregion

        #region NextString：获取一个随机字符串，指定字符串大小和字符集
        /// <summary>
        /// 获取一个随机字符串，指定字符串大小和字符集（字符集已定义常量）
        /// </summary>
        public static string NextString(int size, string source = NUMBER_AND_CHAR)
        {
            if (size <= 0 || string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            string[] chars = source.Split(',');
            int len = chars.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var value = chars[NextNum(0, len - 1)];
                if (i == 0 && (value == "0" || value == "O"))
                {
                    value = NextString(1, source);
                }
                sb.Append(value);
            }
            return sb.ToString();
        }
        #endregion

        /// <summary>
        /// 获取随机boolean值
        /// </summary>
        public static bool NextBoolean()
        {
            return new System.Random(Guid.NewGuid().GetHashCode()).NextDouble() > 0.5;
        }

        /// <summary>
        /// 获取随机枚举值值
        /// </summary>
        public static T NextEnum<T>() where T : struct
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidCastException("必须是枚举类型。");
            var values = Enum.GetValues(type);
            return (T)values.GetValue(NextNum(0, values.Length - 1));
        }

        /// <summary>
        /// 获取随机datetime值，默认时间范围：1970, 1, 1——2099, 1, 1
        /// </summary>
        public static DateTime NextDatetime()
        {
            return NextDatetime(new DateTime(1970, 1, 1), new DateTime(2099, 1, 1));
        }

        /// <summary>
        /// 获取随机datetime值，指定时间范围min，max
        /// </summary>
        public static DateTime NextDatetime(DateTime min, DateTime max)
        {
            var random = new System.Random(Guid.NewGuid().GetHashCode());
            var ticks = min.Ticks + (long)((max.Ticks - min.Ticks) * random.NextDouble());
            return new DateTime(ticks);
        }

        /// <summary>
        /// 获取随机颜色值，默认颜色区间start=1，end=255
        /// </summary>
        public static Color NextColor(int start = 1, int end = 255)
        {
            var r = Random.NextNum(1, 255);
            return Color.FromRgb((byte)Random.NextNum(1, 255), (byte)Random.NextNum(1, 255), (byte)Random.NextNum(1, 255));
        }
    }
}
