using System;
using System.Linq;
using System.ComponentModel;
using System.Net;
using System.Data;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Util.Controls
{
    /*
     * 较大的图片，视频，网络图片要做缓存处理：缓存缩略图为本地文件，或内存缩略图对象。
     */

    /// <summary>
    /// 缩略图图片显示控件，同时支持图片和视频缩略图
    /// </summary>
    public class ThumbnailImage : Image
    {
        /// <summary>
        /// 是否启用缓存,默认false不启用
        /// </summary>
        public bool CacheEnable
        {
            get { return (bool)GetValue(CacheEnableProperty); }
            set { SetValue(CacheEnableProperty, value); }
        }
        /// <summary>
        /// 是否启用缓存,默认false不启用.默认缓存时间是180秒
        /// </summary>
        public static readonly DependencyProperty CacheEnableProperty =
            DependencyProperty.Register("CacheEnable", typeof(bool), typeof(ThumbnailImage), new PropertyMetadata(false));

        /// <summary>
        /// 缓存时间，单位秒。默认180秒
        /// </summary>
        public int CacheTime
        {
            get { return (int)GetValue(CacheTimeProperty); }
            set { SetValue(CacheTimeProperty, value); }
        }
        public static readonly DependencyProperty CacheTimeProperty =
            DependencyProperty.Register("CacheTime", typeof(int), typeof(ThumbnailImage), new PropertyMetadata(180));

        /// <summary>
        /// 是否启用异步加载，网络图片建议启用，本地图可以不需要。默认不起用异步
        /// </summary>
        public bool AsyncEnable
        {
            get { return (bool)GetValue(AsyncEnableProperty); }
            set { SetValue(AsyncEnableProperty, value); }
        }
        public static readonly DependencyProperty AsyncEnableProperty =
            DependencyProperty.Register("AsyncEnable", typeof(bool), typeof(ThumbnailImage), new PropertyMetadata(false));

        /// <summary>
        /// 缩略图类型，默认Image图片
        /// </summary>
        public EnumThumbnail ThumbnailType
        {
            get { return (EnumThumbnail)GetValue(ThumbnailTypeProperty); }
            set { SetValue(ThumbnailTypeProperty, value); }
        }
        public static readonly DependencyProperty ThumbnailTypeProperty =
            DependencyProperty.Register("ThumbnailType", typeof(EnumThumbnail), typeof(ThumbnailImage), new PropertyMetadata(EnumThumbnail.Image));

        /// <summary>
        /// 缩略图数据源：文件物理路径
        /// </summary>
        public object ThumbnailSource
        {
            get { return GetValue(ThumbnailSourceProperty); }
            set { SetValue(ThumbnailSourceProperty, value); }
        }
        public static readonly DependencyProperty ThumbnailSourceProperty = DependencyProperty.Register("ThumbnailSource", typeof(object),
            typeof(ThumbnailImage), new PropertyMetadata(OnSourcePropertyChanged));

        /// <summary>
        /// 缩略图
        /// </summary>
        protected static ThumbnailProviderFactory ThumbnailProviderFactory = new ThumbnailProviderFactory();

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Loaded += ThumbnailImage_Loaded;
        }

        void ThumbnailImage_Loaded(object sender, RoutedEventArgs e)
        {
            BindSource(this);
        }

        /// <summary>
        /// 属性更改处理事件
        /// </summary>
        private static void OnSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ThumbnailImage img = sender as ThumbnailImage;
            if (img == null) return;
            if (!img.IsLoaded) return;
            BindSource(img);
        }
        private static void BindSource(ThumbnailImage image)
        {
            var w = image.Width;
            var h = image.Height;
            object source = image.ThumbnailSource;
            //bind
            if (image.AsyncEnable)
            {
                BindThumbnialAync(image, source, w, h);
            }
            else
            {
                BindThumbnial(image, source, w, h);
            }
        }

        /// <summary>
        /// 绑定缩略图
        /// </summary>
        private static void BindThumbnial(ThumbnailImage image, object fileSource, double w, double h)
        {
            IThumbnailProvider thumbnailProvider = ThumbnailProviderFactory.GetInstance(image.ThumbnailType);
            image.Dispatcher.BeginInvoke(new Action(() =>
            {
                var cache = image.CacheEnable;
                var time = image.CacheTime;
                ImageSource img = null;
                if (cache)
                {
                    img = CacheManager.GetCache<ImageSource>(fileSource.GetHashCode().ToString(), time, () =>
                    {
                        return thumbnailProvider.GenereateThumbnail(fileSource, w, h);
                    });
                }
                else img = thumbnailProvider.GenereateThumbnail(fileSource, w, h);
                image.Source = img;
            }), DispatcherPriority.ApplicationIdle);
        }

        /// <summary>
        /// 异步线程池绑定缩略图
        /// </summary>
        private static void BindThumbnialAync(ThumbnailImage image, object fileSource, double w, double h)
        {
            if (fileSource == null) return;
            IThumbnailProvider thumbnailProvider = ThumbnailProviderFactory.GetInstance(image.ThumbnailType);
            var cache = image.CacheEnable;
            var time = image.CacheTime;
            System.Utility.Executer.TryRunByThreadPool(() =>
            {
                ImageSource img = null;
                if (cache)
                {
                    img = CacheManager.GetCache<ImageSource>(fileSource.GetHashCode().ToString(), time, () =>
                    {
                        return thumbnailProvider.GenereateThumbnail(fileSource, w, h);
                    });
                }
                else img = thumbnailProvider.GenereateThumbnail(fileSource, w, h);
                image.Dispatcher.BeginInvoke(new Action(() => { image.Source = img; }), DispatcherPriority.ApplicationIdle);
            });
        }
    }
}