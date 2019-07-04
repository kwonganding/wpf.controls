using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace System.Utility.Helper
{
    /// <summary>
    /// 数据可视化分析辅助类
    /// </summary>
    public class DataVisualAnalysis
    {
        #region RangeAnalysis 值域分析
        /// <summary>
        /// 分析属性property的值在在值域value中对应于目标target的数值
        /// scale:目标结果基数值，如为100，则结果为基于value的百分值
        /// property：需要分析的属性名称（必须是数值类型）
        /// attach:分析结果保存到动态属性Dynamic上的属性名称
        /// </summary>
        public static void RangeAnalysis(IEnumerable<object> oitems, double scale, string property, string attach)
        {
            if (oitems == null || !oitems.Any())
            {
                return;
            }
            var items = oitems.ToList();
            if (!items.First().IsInstanceOfT<IDynamicEnable>())
            {
                return;
            }
            int len = items.Count();
            double[] values = new double[len];
            int i = 0;
            items.ForEach(item =>
                              {
                                  var pv = 0D;
                                  if (item is DynamicX)
                                  {
                                      pv = ((item as DynamicX).Get(property).ToSafeString().ToSafeDouble());
                                  }
                                  else
                                  {
                                      pv = System.Utility.Helper.Reflection.GetObjectValue(item, property).ToSafeString().ToSafeDouble();
                                  }
                                  values[i++] = pv;
                              });
            var max = values.Max();
            for (i = 0; i < len; i++)
            {
                var d = items[i] as IDynamicEnable;
                if (d == null) return;
                if (d.Dynamic == null)
                {
                    d.Dynamic = new DynamicX();
                }
                var v = values[i] / max * scale;
                d.Dynamic.Set(attach, v + 0.5);
            }
        }
        #endregion

        #region ColorDistributeAnalysis 颜色分布分析
        /// <summary>
        /// 根据指定集合的指定值属性计算颜色分布：
        /// start,end：起始颜色，建议颜色：Red-DarkKhaki
        /// property：需要计算的属性名称
        /// attach：附加到Dynamic上的动态属性名称
        /// attachColor,true:附加Color，false：附加SolidColorBrush
        /// </summary>
        public static void ColorDistributeAnalysis<TItem>(List<TItem> items, Color start, Color end, string property, string attach, bool attachColor = true)
            where TItem : IDynamicEnable
        {
            if (items == null || items.Count <= 0)
            {
                return;
            }
            //起始颜色
            Int16[] s = new Int16[]
            {
                start.R.ToString().ToInt16(),
                start.G.ToString().ToInt16(),
                start.B.ToString().ToInt16()
            };
            Int16[] e = new Int16[]
            {
                end.R.ToString().ToInt16(),
                end.G.ToString().ToInt16(),
                end.B.ToString().ToInt16(),
            };
            //最大段数
            var maxStep = Math.Abs(s.Max() - e.Min()) / 2;
            //数值
            List<double> values = new List<double>();
            items.ForEach(item =>
                {
                    if (item is DynamicX)
                    {
                        values.Add((item as DynamicX).Get(property).ToSafeString().ToSafeDouble());
                    }
                    else
                    {
                        var v = System.Utility.Helper.Reflection.GetObjectValue<TItem>(item, property).ToSafeString().ToSafeDouble();
                        values.Add(v);
                    }
                });
            var max = values.Max();
            var min = values.Min();
            var sub = max - min;
            //设置颜色
            SetStepColor(items, ref start, attach, attachColor, s, e, maxStep, values, min, sub);
        }

        #region SetStepColor
        private static void SetStepColor<TItem>(List<TItem> items, ref Color start, string attach, bool attachColor, Int16[] s, Int16[] e,
            int maxStep, List<double> values, double min, double sub) where TItem : IDynamicEnable
        {
            Color sc = start;
            //如果值相等，则直接给定颜色
            if (sub == 0)
            {
                items.ForEach(m =>
                {
                    if (m.Dynamic == null)
                    {
                        m.Dynamic = new DynamicX();
                    }
                    if (attachColor)
                    {
                        m.Dynamic.Set(attach, sc);
                    }
                    else
                    {
                        m.Dynamic.Set(attach, new SolidColorBrush(sc));
                    }
                });
                return;
            }
            //根据差值和最大段数分段
            var step = (int)Math.Floor(sub > maxStep ? maxStep : sub);
            var stepv = sub / step;
            //循环处理
            int len = items.Count;
            for (int i = 0; i < len; i++)
            {
                //计算对应的区间
                var n = (int)Math.Floor((values[i] - min) / stepv);
                //渐变色
                Int32[] c = new Int32[3];
                for (int j = 0; j < 3; j++)
                {
                    c[j] = e[j] + (s[j] - e[j]) / step * n;
                }
                if (items[i].Dynamic == null)
                {
                    items[i].Dynamic = new DynamicX();
                }
                if (attachColor)
                {
                    items[i].Dynamic.Set(attach, Color.FromRgb(BitConverter.GetBytes(c[0])[0],
                    BitConverter.GetBytes(c[1])[0], BitConverter.GetBytes(c[2])[0]));
                }
                else
                {
                    items[i].Dynamic.Set(attach, new SolidColorBrush(Color.FromRgb(BitConverter.GetBytes(c[0])[0],
                    BitConverter.GetBytes(c[1])[0], BitConverter.GetBytes(c[2])[0])));
                }
            }
        }
        #endregion

        #endregion
    }
}
