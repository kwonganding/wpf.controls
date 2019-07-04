using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace System.Utility.Helper
{
    /// <summary>
    /// 颜色操作辅助类
    /// </summary>
    public class ColorHelper
    {
        /// <summary>
        /// 获取指定颜色的浅色值
        /// </summary>
        public static Color GetLowerColor(Color color)
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
                Color end = Color.FromRgb(250, 250, 250);
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


    }
}
