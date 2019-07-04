using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


namespace System
{
    /// <summary>
    /// 时间类型的扩展方法
    /// </summary>
    public static partial class TypeExtension
    {
        #region ToDateStringFromNow：根据当前时间格式时间为多少时间之前的字符串
        /// <summary>
        /// 根据当前时间格式时间为多少时间之前的字符串
        /// </summary>
        public static string ToDateStringFromNow(this DateTime dt)
        {
            if (!dt.IsValid())
            {
                return string.Empty;
            }
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 365)
            {
                return string.Format("{0}年前", (int)Math.Floor(span.TotalDays / 365));
            }
            else if (span.TotalDays > 30)
            {
                return string.Format("{0}个月前", (int)Math.Floor(span.TotalDays / 30));
            }
            else if (span.TotalDays > 7)
            {
                return string.Format("{0}周前", (int)Math.Floor(span.TotalDays / 7));
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            //未来的时间
            if (dt.Date == DateTime.Now.Date)
            {
                return "今天";
            }
            span = dt - DateTime.Now;
            int day = (int)Math.Floor(Math.Abs(span.TotalDays));
            if (day == 1)
            {
                return "明天";
            }
            else if (day == 2)
            {
                return "后天";
            }
            else if (day < 7)
            {
                return string.Format("{0}天后", day);
            }
            else if (day < 30)
            {
                return string.Format("{0}周后", day / 7);
            }
            else if (day < 365)
            {
                return string.Format("{0}月后", day / 30);
            }
            return string.Format("{0}年后", day / 365);
        }

        /// <summary>
        /// 根据当前时间格式时间为多少时间之前的字符串
        /// </summary>
        public static string ToDateStringFromNow(this DateTime? dt)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToDateStringFromNow();
            }
            return string.Empty;
        }

        #endregion

        #region ToDateTime：将字符串按照格式转换为日期时间对象
        /// <summary>
        /// 将字符串按照格式转换为日期时间对象，若转换失败返回异常
        /// 可以提供丰富灵活的转换处理，如1204/yyMM ; 201308 20:09 /yyyyMM HH:ss
        /// </summary>
        public static DateTime ToDateTime(this string value, string format, string culture = "zh-CHS")
        {
            DateTime result;
            //扩展：区域性标识符是标准国际数值缩写，如en-US表示美国
            //“zh-Hans”（简体中文）和“zh-Hant”（繁体中文） ： 旧名称“zh-CHS”和“zh-CHT”
            if (DateTime.TryParseExact(value, format, new CultureInfo("zh-CHS"),
                Globalization.DateTimeStyles.None, out result))
            {
                return result;
            }
            else
            {
                throw new Exception(string.Format("字符串（{0}）不能按照格式（{1}）解析为日期时间", value, format));
            }
        }
        #endregion

        #region ToDateTime：转换为日期时间
        /// <summary>
        /// 转换为日期时间
        /// </summary>
        public static DateTime ToDateTime(this string dateTime)
        {
            DateTime result;
            if (DateTime.TryParse(dateTime, out result))
            {
                return result;
            }
            throw new InvalidCastException("\"" + dateTime + "\"不是有效的时间格式，请确认。");
        }
        #endregion

        #region ToSafeDateTime：安全的转换为日期时间
        /// <summary>
        /// 转换为日期时间
        /// </summary>
        public static DateTime? ToSafeDateTime(this string dateTime)
        {
            DateTime result;
            if (DateTime.TryParse(dateTime, out result))
            {
                return result;
            }
            return null;
        }
        #endregion

        #region ToUTCTime：转换为UTC字串为日期时间
        /// <summary>
        /// 转换UTC字符串eg:13 Oct 2014 03:50:20 +0800为日期时间
        /// eg:Mon, 13 Oct 2014 03:50:20 +0800 (CST) 
        /// </summary>
        public static DateTime? ToUTCTime(this string dateTime)
        {
            DateTime? result = ToSafeDateTime(dateTime);
            if (result != null) return result;

            Text.RegularExpressions.Regex rgex = new Text.RegularExpressions.Regex(@"(?i)\d+ [a-z]+ \d{4} \d+:\d+:\d+ [-\+]\d+");
            return ToSafeDateTime(rgex.Match(dateTime).Value);
        }
        #endregion

        #region IsValid：是否为有效日期
        /// <summary>
        /// 是否为有效日期
        /// </summary>
        public static bool IsValid(this DateTime value)
        {
            if (DateTime.MinValue == value || DateTime.MaxValue == value)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region IsValid：是否为有效日期
        /// <summary>
        /// 是否为有效日期
        /// </summary>
        public static bool IsValid(this DateTime? value)
        {
            if (value == null || DateTime.MinValue == value || DateTime.MaxValue == value)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region ToDateString：转换为字符

        #region ToDateString
        /// <summary>
        /// 转换为日期字符串，格式：yyyy-MM-dd。
        /// 如果值非法（null,min,max）返回emtpy
        /// </summary>
        public static string ToDateString(this DateTime value)
        {
            return value.IsValid() ? value.ToString("yyyy-MM-dd") : string.Empty;
        }

        #endregion

        #region ToDateString
        /// <summary>
        /// 转换为日期字符串，格式：yyyy-MM-dd。
        /// 如果值非法（null,min,max）返回emtpy
        /// </summary>
        public static string ToDateString(this DateTime? value)
        {
            if (value != null) return value.IsValid() ? value.Value.ToString("yyyy-MM-dd") : string.Empty;
            return string.Empty;
        }

        /// <summary>
        /// 转换为日期字符串，格式：yyyy-MM-dd。
        /// 如果值非法（null,min,max）返回emtpy
        /// </summary>
        public static string ToDateString(this DateTime? value, string format, string defaultStr = "")
        {
            if (value != null) return value.IsValid() ? value.Value.ToString(format) : defaultStr;
            return defaultStr;
        }

        #endregion

        #region ToDateTimeString
        /// <summary>
        /// 转换为日期时间字符串，格式：yyyy-MM-dd HH:mm:ss，若时分秒为0则只返回日期
        /// 如果值非法（null,min,max）返回emtpy
        /// </summary>
        public static string ToDateTimeString(this DateTime value)
        {
            if (!value.IsValid())
            {
                return string.Empty;
            }
            if (value.Hour == 0 && value.Minute == 0 && value.Second == 0)
            {
                return value.ToDateString();
            }
            return value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion

        #region ToDateTimeString
        /// <summary>
        /// 转换为日期字符串，格式：yyyy-MM-dd HH:mm:ss
        /// 如果值非法（null,min,max）返回emtpy
        /// </summary>
        public static string ToDateTimeString(this DateTime? value)
        {
            if (value.IsValid())
            {
                return value.Value.ToDateTimeString();
            }
            return string.Empty;
        }
        #endregion

        #endregion
    }
}
