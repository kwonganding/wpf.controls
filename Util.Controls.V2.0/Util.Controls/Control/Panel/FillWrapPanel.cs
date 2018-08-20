using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Util.Controls
{
    /// <summary>
    /// 充填方式排列（自动处理Item宽度，从而达到处理右侧的空白）
    /// 2016-07-13 20:58:00 wangxi
    /// </summary>
    public class FillWrapPanel : Panel
    {
        #region DependencyProperty
        public static readonly DependencyProperty MinItemWidthProperty =
            DependencyProperty.Register("MinItemWidth", typeof(double), typeof(FillWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static readonly DependencyProperty MaxItemWidthProperty =
            DependencyProperty.Register("MaxItemWidth", typeof(double), typeof(FillWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(double), typeof(FillWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static readonly DependencyProperty RowMarginProperty =
            DependencyProperty.Register("RowMargin", typeof(double), typeof(FillWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static readonly DependencyProperty FloorItemWidthProperty =
            DependencyProperty.Register("FloorItemWidth", typeof(bool), typeof(FillWrapPanel), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange));
        

        /// <summary>
        /// 是否允许Item的Width允许浮点数
        /// </summary>
        public bool FloorItemWidth
        {
            get { return (bool)GetValue(FloorItemWidthProperty); }
            set { SetValue(FloorItemWidthProperty, value); }
        }

        /// <summary>
        /// Item的最小宽度
        /// </summary>
        public double MinItemWidth
        {
            get { return (double)GetValue(MinItemWidthProperty); }
            set { SetValue(MinItemWidthProperty, value); }
        }

        /// <summary>
        /// Item的最大宽度
        /// </summary>
        public double MaxItemWidth
        {
            get { return (double)GetValue(MaxItemWidthProperty); }
            set { SetValue(MaxItemWidthProperty, value); }
        }

        /// <summary>
        /// Item之间间距
        /// </summary>
        public double ItemMargin
        {
            get { return (double)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        /// <summary>
        /// Item行间距
        /// </summary>
        public double RowMargin
        {
            get { return (double)GetValue(RowMarginProperty); }
            set { SetValue(RowMarginProperty, value); }
        }

        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }
            return new Size(0, 0);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var yOffset = 0.0;
            var xOffset = 0.0;
            // 计算列数
            var itemCountInRow = CalculateItemsCountInOneRow(finalSize);
            // 计算Item宽度
            var itemWidth = CalculateItemWidth(finalSize.Width, itemCountInRow);
            for (var i = 0; i < Children.Count;)
            {
                double rowHeight = 0;
                for (var column = 0; column < itemCountInRow && i + column < Children.Count; column++)
                {
                    var child = Children[i + column];
                    child.Arrange(new Rect(xOffset, yOffset, itemWidth, child.DesiredSize.Height));
                    if (child.DesiredSize.Height > rowHeight)
                    {
                        rowHeight = child.DesiredSize.Height;
                    }
                    xOffset += itemWidth + ItemMargin;
                }
                yOffset += rowHeight + RowMargin;
                xOffset = 0.0;
                i += itemCountInRow;
            }
            return base.ArrangeOverride(finalSize);
        }

        private int CalculateItemsCountInOneRow(Size finalSize)
        {
            return (int)Math.Floor(((finalSize.Width + ItemMargin) / (MinItemWidth + ItemMargin)));
        }

        private double CalculateItemWidth(double totalWidth, int itemCountInRow)
        {
            if (itemCountInRow > Children.Count)
            {
                itemCountInRow = Children.Count;
            }
            double itemWidth = (totalWidth - (itemCountInRow - 1) * ItemMargin) / itemCountInRow;

            if (itemWidth > MaxItemWidth)
            {
                itemWidth = MaxItemWidth;
            }
            return FloorItemWidth ? Math.Floor(itemWidth) : itemWidth;
        }
    }
}
