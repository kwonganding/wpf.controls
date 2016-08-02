//source( Microsoft All-In-One Code Framework ):http://1code.codeplex.com/
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using File = System.Utility.Helper.File;
using Image = System.Windows.Controls.Image;

namespace Util.Controls
{
    /// <summary>
    /// 支持GIF动画图片播放的图片控件，GIF图片源GIFSource
    /// </summary>
    public class AnimatedGIF : Image
    {
        public static readonly DependencyProperty GIFSourceProperty = DependencyProperty.Register(
            "GIFSource", typeof(string), typeof(AnimatedGIF), new PropertyMetadata(OnSourcePropertyChanged));

        /// <summary>
        /// GIF图片源，支持相对路径、绝对路径
        /// </summary>
        public string GIFSource
        {
            get { return (string)GetValue(GIFSourceProperty); }
            set { SetValue(GIFSourceProperty, value); }
        }

        internal Bitmap Bitmap; // Local bitmap member to cache image resource
        internal BitmapSource BitmapSource;
        public delegate void FrameUpdatedEventHandler();

        /// <summary>
        /// Delete local bitmap resource
        /// Reference: http://msdn.microsoft.com/en-us/library/dd183539(VS.85).aspx
        /// </summary>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool DeleteObject(IntPtr hObject);

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Loaded += AnimatedGIF_Loaded;
            this.Unloaded += AnimatedGIF_Unloaded;
        }

        void AnimatedGIF_Unloaded(object sender, RoutedEventArgs e)
        {
            this.StopAnimate();
        }

        void AnimatedGIF_Loaded(object sender, RoutedEventArgs e)
        {
            BindSource(this);
        }

        /// <summary>
        /// Start animation
        /// </summary>
        public void StartAnimate()
        {
            ImageAnimator.Animate(Bitmap, OnFrameChanged);
        }

        /// <summary>
        /// Stop animation
        /// </summary>
        public void StopAnimate()
        {
            ImageAnimator.StopAnimate(Bitmap, OnFrameChanged);
        }

        /// <summary>
        /// Event handler for the frame changed
        /// </summary>
        private void OnFrameChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                   new FrameUpdatedEventHandler(FrameUpdatedCallback));
        }

        private void FrameUpdatedCallback()
        {
            ImageAnimator.UpdateFrames();

            if (BitmapSource != null)
                BitmapSource.Freeze();

            // Convert the bitmap to BitmapSource that can be display in WPF Visual Tree
            BitmapSource = GetBitmapSource(this.Bitmap, this.BitmapSource);
            Source = BitmapSource;
            InvalidateVisual();
        }

        /// <summary>
        /// 属性更改处理事件
        /// </summary>
        private static void OnSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            AnimatedGIF gif = sender as AnimatedGIF;
            if (gif == null) return;
            if (!gif.IsLoaded) return;
            BindSource(gif);
        }
        private static void BindSource(AnimatedGIF gif)
        {
            gif.StopAnimate();
            if (gif.Bitmap != null) gif.Bitmap.Dispose();
            var path = gif.GIFSource;
            if (path.IsInvalid()) return;
            if (!Path.IsPathRooted(path))
            {
                path = File.GetPhysicalPath(path);
            }
            gif.Bitmap = new Bitmap(path);
            gif.BitmapSource = GetBitmapSource(gif.Bitmap, gif.BitmapSource);
            gif.StartAnimate();
        }

        private static BitmapSource GetBitmapSource(Bitmap bmap, BitmapSource bimg)
        {
            IntPtr handle = IntPtr.Zero;

            try
            {
                handle = bmap.GetHbitmap();
                bimg = Imaging.CreateBitmapSourceFromHBitmap(
                    handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                if (handle != IntPtr.Zero)
                    DeleteObject(handle);
            }

            return bimg;
        }
    }
}
