using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Util.Controls
{
    /// <summary>
    /// 鼠标进入旋转动画
    /// </summary>
    public sealed class RotateAnimationBehavior : Behavior<UIElement>
    {
        /// <summary>
        /// 旋转角度
        /// </summary>
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(RotateAnimationBehavior), new PropertyMetadata(180D));

        /// <summary>
        /// 动画时间-秒
        /// </summary>
        public double DurationSecond
        {
            get { return (double)GetValue(DurationSecondProperty); }
            set { SetValue(DurationSecondProperty, value); }
        }
        public static readonly DependencyProperty DurationSecondProperty =
            DependencyProperty.Register("DurationSecond", typeof(double), typeof(RotateAnimationBehavior), new PropertyMetadata(0.2D));

        private DoubleAnimation _DoubleAnimation;

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.RenderTransformOrigin = new Point(0.5, 0.5);
            this.AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
            this.AssociatedObject.MouseEnter += AssociatedObject_MouseEnter;
            this.AssociatedObject.RenderTransform = new RotateTransform();
            _DoubleAnimation = new DoubleAnimation();
            Timeline.SetDesiredFrameRate(_DoubleAnimation, 30);
            _DoubleAnimation.Duration = TimeSpan.FromSeconds(this.DurationSecond);
        }
        private void AssociatedObject_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _DoubleAnimation.To = this.Angle;
            this.AssociatedObject.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, _DoubleAnimation);
        }

        private void AssociatedObject_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _DoubleAnimation.To = 0;
            this.AssociatedObject.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, _DoubleAnimation);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this._DoubleAnimation = null;
            this.AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;
            this.AssociatedObject.MouseEnter -= AssociatedObject_MouseEnter;
        }
    }

    //<i:Interaction.Behaviors>
    //               <local:RotateAnimationBehavior Angle = "180" DurationSecond="0.5"/>
    //           </i:Interaction.Behaviors>
}
