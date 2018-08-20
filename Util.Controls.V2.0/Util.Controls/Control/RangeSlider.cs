using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Util.Controls
{
    /// <summary>
    /// 上下区间范围选择控件
    /// </summary>
    [DefaultEvent("RangeSelectionChanged"),
    TemplatePart(Name = "PART_RangeSliderContainer", Type = typeof(StackPanel)),
    TemplatePart(Name = "PART_LeftEdge", Type = typeof(RepeatButton)),
    TemplatePart(Name = "PART_RightEdge", Type = typeof(RepeatButton)),
    TemplatePart(Name = "PART_LeftThumb", Type = typeof(Thumb)),
    TemplatePart(Name = "PART_MiddleThumb", Type = typeof(Thumb)),
    TemplatePart(Name = "PART_RightThumb", Type = typeof(Thumb))]
    public sealed class RangeSlider : RangeBase
    {
        private Thumb centerThumb;
        private Thumb leftThumb;
        private Thumb rightThumb;
        private RepeatButton leftButton;
        private RepeatButton rightButton;

        /// <summary>
        /// 选中的范围开始值
        /// </summary>
        public double StartValue
        {
            get { return (double)GetValue(StartValueProperty); }
            set { SetValue(StartValueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for StartValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartValueProperty =
            DependencyProperty.Register("StartValue", typeof(double), typeof(RangeSlider), new PropertyMetadata(OnValueChangedCallback));

        /// <summary>
        /// 选中的范围结束值
        /// </summary>
        public double EndValue
        {
            get { return (double)GetValue(EndValueProperty); }
            set { SetValue(EndValueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for EndValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndValueProperty =
            DependencyProperty.Register("EndValue", typeof(double), typeof(RangeSlider), new PropertyMetadata(OnValueChangedCallback));

        public RangeSlider()
        {
            DependencyPropertyDescriptor.FromProperty(ActualWidthProperty, typeof(RangeSlider)).AddValueChanged(this, delegate { ReCalculateWidths(); });
            DependencyPropertyDescriptor.FromProperty(MinimumProperty, typeof(RangeSlider)).AddValueChanged(this, delegate { ReCalculateWidths(); });
            DependencyPropertyDescriptor.FromProperty(MaximumProperty, typeof(RangeSlider)).AddValueChanged(this, delegate { ReCalculateWidths(); });
        }
        static RangeSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RangeSlider), new FrameworkPropertyMetadata(typeof(RangeSlider)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //var visualElementsContainer = GetTemplateChild("PART_RangeSliderContainer") as StackPanel;
            centerThumb = GetTemplateChild("PART_MiddleThumb") as Thumb;
            leftButton = GetTemplateChild("PART_LeftEdge") as RepeatButton;
            rightButton = GetTemplateChild("PART_RightEdge") as RepeatButton;
            leftThumb = GetTemplateChild("PART_LeftThumb") as Thumb;
            rightThumb = GetTemplateChild("PART_RightThumb") as Thumb;
            //初始化事件
            centerThumb.DragDelta += centerThumb_DragDelta;
            leftThumb.DragDelta += leftThumb_DragDelta;
            rightThumb.DragDelta += rightThumb_DragDelta;
            leftButton.Click += leftButton_Click;
            rightButton.Click += rightButton_Click;
            ReCalculateWidths();
        }

        void rightButton_Click(object sender, RoutedEventArgs e)
        {
            MoveSelection(false);
        }

        void leftButton_Click(object sender, RoutedEventArgs e)
        {
            MoveSelection(true);
        }

        void rightThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MoveThumb(centerThumb, rightButton, e.HorizontalChange);
            ReCalculateRangeSelected(false, true);
            ReCalculateWidths();
        }

        void leftThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MoveThumb(leftButton, centerThumb, e.HorizontalChange);
            ReCalculateRangeSelected(true, false);
        }

        void centerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MoveThumb(leftButton, rightButton, e.HorizontalChange);
            ReCalculateRangeSelected(true, true);
        }

        private static void MoveThumb(FrameworkElement x, FrameworkElement y, double horizonalChange)
        {
            double change = 0;
            if (horizonalChange < 0) //slider went left
                change = GetChangeKeepPositive(x.Width, horizonalChange);
            else if (horizonalChange > 0) //slider went right if(horizontal change == 0 do nothing)
                change = -GetChangeKeepPositive(y.Width, -horizonalChange);

            x.Width += change;
            y.Width -= change;
        }

        private static void OnValueChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RangeSlider slider = (RangeSlider)sender;
            if (e.NewValue == e.OldValue || slider.internalUpdate) return;
            if (e.Property == RangeSlider.StartValueProperty && (slider.StartValue >= slider.EndValue || slider.StartValue < slider.Minimum))
                slider.StartValue = (double)e.OldValue;
            if (e.Property == RangeSlider.EndValueProperty &&
                (slider.EndValue <= slider.StartValue || slider.EndValue > slider.Maximum))
                slider.EndValue = (double)e.OldValue;
            slider.ReCalculateWidths();
        }

        //ensures that the new value (newValue param) is a valid value. returns false if not
        private static double GetChangeKeepPositive(double width, double increment)
        {
            return Math.Max(width + increment, 0) - width;
        }

        double movableRange = 0;
        double movableWidth = 0;
        private void ReCalculateRanges()
        {
            movableRange = Maximum - Minimum - SmallChange;
        }

        //recalculates the movableWidth. called whenever the width of the control changes
        private void ReCalculateWidths()
        {
            ReCalculateRanges();
            if (leftButton != null && rightButton != null && centerThumb != null)
            {
                movableWidth = Math.Max(ActualWidth - rightThumb.ActualWidth - leftThumb.ActualWidth - centerThumb.MinWidth, 1);
                leftButton.Width = Math.Max(movableWidth * (StartValue - Minimum) / movableRange, 0);
                rightButton.Width = Math.Max(movableWidth * (Maximum - EndValue) / movableRange, 0);
                centerThumb.Width = Math.Max(ActualWidth - leftButton.Width - rightButton.Width - rightThumb.ActualWidth - leftThumb.ActualWidth, 0);
            }
        }
        bool internalUpdate = false;
        //recalculates the StartValue called when the left thumb is moved and when the middle thumb is moved
        //recalculates the EndValue called when the right thumb is moved and when the middle thumb is moved
        private void ReCalculateRangeSelected(bool reCalculateStart, bool reCalculateStop)
        {
            internalUpdate = true;
            if (reCalculateStart)
            {
                // Make sure to get exactly Minimum if thumb is at the start
                if (leftButton.Width == 0.0)
                    StartValue = Minimum;
                else
                    StartValue =
                        Math.Max(Minimum, (long)(Minimum + movableRange * leftButton.Width / movableWidth));
            }

            if (reCalculateStop)
            {
                // Make sure to get exactly Maximum if thumb is at the end
                if (rightButton.Width == 0.0)
                    EndValue = Maximum;
                else
                    EndValue =
                        Math.Min(Maximum, (long)(Maximum - movableRange * rightButton.Width / movableWidth));
                internalUpdate = false;
            }
        }

        /// <summary>
        /// moves the current selection with x value
        /// </summary>
        /// <param name="isLeft">True if you want to move to the left</param>
        public void MoveSelection(bool isLeft)
        {
            ReCalculateRanges();
            double widthChange = SmallChange * (EndValue - StartValue)
                * movableWidth / movableRange;

            widthChange = isLeft ? -widthChange : widthChange;
            MoveThumb(leftButton, rightButton, widthChange);
            ReCalculateRangeSelected(true, true);
        }
    }
}
