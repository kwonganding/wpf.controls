using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows.Media;

namespace Util.Controls
{
    /// <summary>
    /// 颜色操作辅助类
    /// </summary>
    public static class ColorHelper
    {
        #region GenerateGradientBrush

        /// <summary>
        /// 获取指定颜色的渐变色值，指定总深度maxlevel和当前层级index
        /// </summary>
        public static Color GenerateGradientColor(Color color, int maxlevel, int index)
        {
            maxlevel = Math.Max(maxlevel, index);
            Int16[] ss = new Int16[]
            {
                color.R,
                color.G,
                color.B,
            };
            Int16 max = 255;
            Int16[] ee = new Int16[]
            {
                (Int16)(max-ss[0]),
                (Int16)(max-ss[1]),
                (Int16)(max-ss[2]),
            };
            //result
            Int32[] rr = new Int32[3];
            for (int j = 0; j < 3; j++)
            {
                rr[j] = ss[j] + (ee[j] - ss[j]) * index / maxlevel;
            }
            var rcc = Color.FromArgb(color.A, BitConverter.GetBytes(rr[0])[0],
                BitConverter.GetBytes(rr[1])[0], BitConverter.GetBytes(rr[2])[0]);
            return rcc;
        }

        /// <summary>
        /// 创建降级的渐变色集合。
        /// </summary>
        /// <param name="brush">开始颜色</param>
        /// <param name="count">需要的数量</param>
        /// <returns></returns>
        public static IList<SolidColorBrush> GenerateGradientBrushes(SolidColorBrush brush, int count, int maxlevel = 0)
        {
            var items = new List<SolidColorBrush>(count);
            maxlevel = maxlevel == 0 ? count * 2 : maxlevel;
            for (int i = 0; i < count; i++)
            {
                var nb = new SolidColorBrush(GenerateGradientColor(brush.Color, maxlevel, i));
                nb.Freeze();
                items.Add(nb);
            }
            return items;
        }

        #endregion

        public static Color GenerateInvertColor(Color color)
        {
            return Color.FromArgb(color.A, BitConverter.GetBytes(255 - color.R)[0],
                BitConverter.GetBytes(255 - color.G)[0], BitConverter.GetBytes(255 - color.B)[0]);
        }

        /// <summary>
        /// 获取指定颜色的深色值
        /// </summary>
        public static Color GetHeightColor(Color color)
        {
            try
            {
                //起始颜色
                Int16[] s = new Int16[]
                            {
                                color.R.ToString().ToInt16(),
                                color.G.ToString().ToInt16(),
                                color.B.ToString().ToInt16()
                            };
                Color end = Color.FromRgb(11, 12, 12);
                Int16[] e = new Int16[]
                            {
                                end.R.ToString().ToInt16(),
                                end.G.ToString().ToInt16(),
                                end.B.ToString().ToInt16(),
                            };
                //区间
                int step = 2;
                var n = 1;
                //渐变色
                Int32[] c = new Int32[3];
                for (int j = 0; j < 3; j++)
                {
                    c[j] = e[j] + (s[j] - e[j]) / step * n;
                }
                var res = Color.FromRgb(BitConverter.GetBytes(c[0])[0],
                    BitConverter.GetBytes(c[1])[0], BitConverter.GetBytes(c[2])[0]);
                return res;
            }
            catch
            {
                return Colors.DarkGray;
            }
        }

        /// <summary>
        /// 格式示例：#0AFF11。不包含alpha值
        /// </summary>
        public static string ToRGBString(Color color)
        {
            return string.Format("#{0}{1}{2}", color.R.ToString("X2"), color.G.ToString("X2"), color.B.ToString("X2"));
        }
        /// <summary>
        /// 格式示例：#FF0AFF11。包含alpha值
        /// </summary>
        public static string ToARGBString(Color color)
        {
            return string.Format("#{0}{1}{2}{3}", color.A.ToString("X2"), color.R.ToString("X2"), color.G.ToString("X2"), color.B.ToString("X2"));
        }

        /// <summary>
        /// 格式示例：#0AFF11FF。包含alpha值
        /// </summary>
        public static string ToRGBAString(Color color)
        {
            return string.Format("#{0}{1}{2}{3}", color.R.ToString("X2"), color.G.ToString("X2"), color.B.ToString("X2"), color.A.ToString("X2"));
        }

        /// <summary>
        /// 转换RGB字符串（#FF0AFF11，包含alpha值）为Color
        /// </summary>
        public static Color ConvertFormARGB(string value)
        {
            if (value.Length == 9 && value[0] == '#')
            {
                byte a, r, g, b;

                if (byte.TryParse(value.Substring(1, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out a) &&
                    byte.TryParse(value.Substring(3, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out r) &&
                    byte.TryParse(value.Substring(5, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out g) &&
                    byte.TryParse(value.Substring(7, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out b))
                {
                    return Color.FromArgb(a, r, g, b);
                }
            }
            return Colors.Black;
        }

        /// <summary>
        /// 转换RGB字符串（#0AFF11，不包含alpha值）为Color
        /// </summary>
        public static Color ConvertFormRGB(string value)
        {
            if (value.Length == 7 && value[0] == '#')
            {
                byte r, g, b;

                if (byte.TryParse(value.Substring(1, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out r) &&
                    byte.TryParse(value.Substring(3, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out g) &&
                    byte.TryParse(value.Substring(5, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out b))
                {
                    return Color.FromRgb(r, g, b);
                }
            }
            return Colors.Black;
        }


        public static Color GetColorFromString(string value)
        {
            Color c = Colors.Gray;

            try
            {
                c = (Color)ColorConverter.ConvertFromString(value);
            }
            catch
            {

            }

            return c;
        }

        public static Brush GetBrushFromString(string value)
        {
            Brush b = Brushes.Gray;

            try
            {
                Color c = (Color)ColorConverter.ConvertFromString(value);

                b = new SolidColorBrush(c);
            }
            catch
            {

            }

            return b;
        }

        /// <summary>
        /// 获取相对的颜色（即 接近反色）
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Color GetRelativeColor(Color source)
        {
            Color c = new Color();

            c.A = source.A;

            c.R = GetRelativeValue(source.R);
            c.G = GetRelativeValue(source.G);
            c.B = GetRelativeValue(source.B);

            return c;
        }

        private static byte GetRelativeValue(byte b)
        {
            byte result = 0;
            if (b < 32)
            {
                result = (byte)(256 - b - 32);
            }
            else if (b > 224)
            {
                result = (byte)(256 - b + 32);
            }
            else if (b >= 96 && b < 128)
            {
                result = 224;
            }
            else if (b >= 128 && b < 160)
            {
                result = 32;
            }
            else
            {
                result = (byte)(256 - b);
            }

            return result;
        }
    }
}
