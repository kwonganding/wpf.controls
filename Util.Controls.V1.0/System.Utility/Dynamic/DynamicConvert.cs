using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 动态类型的数据类型安全转换
    /// </summary>
    public static class DynamicConvert
    {
        #region ToSafeInt
        /// <summary>
        /// 转换为安全的Int
        /// </summary>
        public static int ToSafeInt(object value)
        {
            return value.ToSafeString().ToSafeInt();
        }
        #endregion

        #region ToSafeString
        /// <summary>
        /// 转换为安全的string
        /// </summary>
        public static string ToSafeString(object value)
        {
            return value.ToSafeString();
        }
        #endregion

        #region ToSafeBool
        /// <summary>
        /// 转换为安全的bool
        /// </summary>
        public static bool ToSafeBool(object value)
        {
            return value.ToSafeString().ToSafeBoolean();
        }
        #endregion

        #region ToSafeLong
        /// <summary>
        /// 转换为安全的long
        /// </summary>
        public static long ToSafeLong(object value)
        {
            return value.ToSafeString().ToSafeInt64();
        }
        #endregion

        #region ToSafeDecimal
        /// <summary>
        /// 转换为安全的Decimal
        /// </summary>
        public static decimal ToSafeDecimal(object value)
        {
            return value.ToSafeString().ToSafeDecimal();
        }
        #endregion

        #region ToSafeDouble
        /// <summary>
        /// 转换为安全的Double
        /// </summary>
        public static Double ToSafeDouble(object value)
        {
            return value.ToSafeString().ToSafeDouble();
        }
        #endregion

        #region ToSafeInt16
        /// <summary>
        /// 转换为安全的Int16
        /// </summary>
        public static short ToSafeInt16(object value)
        {
            return value.ToSafeString().ToSafeInt16();
        }
        #endregion

        #region ToEnumByValue
        /// <summary>
        /// 转换为枚举对象
        /// </summary>
        public static T ToEnumByValue<T>(object value, T defaultvalue)
        {
            if (value == null) return defaultvalue;
            return value.ToSafeString().ToEnum<T>();
        }
        #endregion

        #region ToSafeDateTime
        /// <summary>
        /// 转换Linux的时间戳为安全的DateTime，如果转换错误，则会返回DateTime.MaxValue
        /// </summary>
        public static DateTime? ToSafeDateTime(object value, int startYear = 1970)
        {
            var len = value.ToSafeString().ToSafeInt64();

            if (len <= 0)
            {
                return null;
            }
          
            //如果是13位为毫秒 10000
            //如果是10为秒，10000000
            //DateTime.Now.Ticks 是指从DateTime.MinValue之后过了多少时间，10000000为一秒
            int strlen = len.ToString().Length;


            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(startYear, 1, 1));
            if (strlen == 13)
            {
                len *= 10000;
            }
            else if (strlen <= 10)
            {
                len *= 10000000;
            }
            try
            {
                TimeSpan toNow = new TimeSpan(len);
                var dt = dtStart.Add(toNow);
                return dt;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 转换UTC的时间戳为安全的DateTime，如果转换错误，则会返回DateTime.MaxValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startYear"></param>
        /// <returns></returns>
        public static DateTime? ToSafeDateTimeForUTC(object value, int startYear = 1970)
        {
            var len = value.ToSafeString().ToSafeInt64();

            if (len <= 0)
            {
                return null;
            }


            //如果是17位 则 需要UTC 乘10 转换
            //如果是13位为毫秒 10000
            //如果是10为秒，10000000
            //DateTime.Now.Ticks 是指从DateTime.MinValue之后过了多少时间，10000000为一秒
            int strlen = len.ToString().Length;

            if (strlen == 17)
            {
                // Windows file time UTC is in nanoseconds, so multiplying by 10
                DateTime gmtTime = DateTime.FromFileTimeUtc(10 * len);

                // Converting to local time
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(gmtTime, TimeZoneInfo.Local);

                return localTime;
            }

            if (strlen == 18)
            {

                DateTime localTime = DateTime.FromFileTime(len).AddHours(-8);
                
                return localTime;
            }

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(startYear, 1, 1));
            if (strlen == 13)
            {
                len *= 10000;
            }
            else if (strlen <= 10)
            {
                len *= 10000000;
            }
            try
            {
                TimeSpan toNow = new TimeSpan(len);
                var dt = dtStart.Add(toNow);
                return dt;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 右移24的日期
        /// <summary>
        /// 转换UTC的时间戳为安全的DateTime，如果转换错误，则会返回DateTime.MaxValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startYear"></param>
        /// <returns></returns>
        public static DateTime? ToSafeDateTimeForMoveRight24(object value, int startYear = 1970)
        {
            var len = value.ToSafeString().ToSafeInt64();

            if (len <= 0)
            {
                return null;
            }

            // 先右移24位
            len = len >> 24;

            //如果是17位 则 需要UTC 乘10 转换
            //如果是13位为毫秒 10000
            //如果是10为秒，10000000
            //DateTime.Now.Ticks 是指从DateTime.MinValue之后过了多少时间，10000000为一秒
            int strlen = len.ToString().Length;

            if (strlen == 17)
            {
                // Windows file time UTC is in nanoseconds, so multiplying by 10
                DateTime gmtTime = DateTime.FromFileTimeUtc(10 * len);

                // Converting to local time
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(gmtTime, TimeZoneInfo.Local);

                return localTime;
            }

            if (strlen == 18)
            {

                DateTime localTime = DateTime.FromFileTime(len).AddHours(-8);

                return localTime;
            }

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(startYear, 1, 1));
            if (strlen == 13)
            {
                len *= 10000;
            }
            else if (strlen <= 10)
            {
                len *= 10000000;
            }
            try
            {
                TimeSpan toNow = new TimeSpan(len);
                var dt = dtStart.Add(toNow);
                return dt;
            }
            catch
            {
                return null;
            }
        }
        
        #endregion

        #region 16位的日期
        /// <summary>
        /// 解析16位的日期
        /// </summary>
        /// <param name="value">16位数字</param>
        /// <returns>日期</returns>
        public static DateTime? ToSafeFromUnixTime(object value)
        {
            var len = value.ToSafeString().ToSafeInt64();
            if (len <= 0)
            {
                return null;
            }

            var unixTime = len / 1000000;
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime).AddHours(8);
        }
        #endregion

        #region ToDynamicX
        /// <summary>
        /// 转换为Dynamic集合
        /// </summary>
        public static List<dynamic> ToDynamicList(IEnumerable<object> items)
        {
            List<dynamic> res = new List<dynamic>();
            items.ForEach(s => res.Add(s as dynamic));
            return res;
        }
        #endregion

        #region WindowsFiletimeToDateTime
        /// <summary>
        /// 将windows的fileIme时间转化为本地时间
        /// </summary>
        /// <param name="hiDateTime">日期高位码</param>
        /// <param name="lowDateTime">日期地位码</param>
        /// <returns></returns>
        public static DateTime? WindowsFiletimeToDateTime(uint hiDateTime, uint lowDateTime)
        {
            #region 以前
            //long timeutcFormat;

            //// 先将4位高位日期转为数组
            //byte[] highBytes = BitConverter.GetBytes(hiDateTime);

            //// 然后转换为8位
            //Array.Resize(ref highBytes, 8);

            //// 高位字节转为一个4位的long
            //timeutcFormat = BitConverter.ToInt64(highBytes, 0);

            //// 位移移动字节
            //timeutcFormat = timeutcFormat << 32;
            //// 与低位相与，高位有为负的可能
            //timeutcFormat = timeutcFormat | lowDateTime;
            //// 转换成一个utc的日期 
            //var utcDateTime = DateTime.FromFileTimeUtc(timeutcFormat);
            //// 转换成本地的日期
            //return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.Local);
            #endregion
            byte[] tempBytesH = BitConverter.GetBytes(hiDateTime);
            byte[] tempBytesL = BitConverter.GetBytes(lowDateTime);

            List<byte> temp = new List<byte>();
            temp.AddRange(tempBytesL);
            temp.AddRange(tempBytesH);

            long tempValue = BitConverter.ToInt64(temp.ToArray(), 0);

            //DateTime 最长时间 
            DateTime max = DateTime.MaxValue.AddYears(-1600);
            TimeSpan maxSpan = max - (new DateTime(0));
            long maxTick = (long)maxSpan.TotalSeconds * TimeSpan.TicksPerSecond;


            DateTime timeValue = new DateTime();
            if (tempValue >= maxTick)
            {
                timeValue = DateTime.MaxValue;
            }
            else
            {
                DateTime tempTt = new DateTime(tempValue);
                timeValue = TimeZoneInfo.ConvertTimeFromUtc(tempTt.AddYears(1600), TimeZoneInfo.Local);
            }
            return timeValue;
        } 
        #endregion

        #region Clone
        /// <summary>
        /// 拷贝对象上的属性、值
        /// </summary>
        public static void Clone(dynamic source, dynamic target)
        {
            if (source == null || target == null)
            {
                return;
            }
            DynamicX s = source as DynamicX;
            DynamicX t = target as DynamicX;
            if (s == null || t == null)
            {
                return;
            }
            foreach (var m in s.Members)
            {
                t.Set(m.Key, m.Value);
            }
        }
        #endregion
    }
}
