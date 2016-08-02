using System;

namespace Util.Controls
{
    /// <summary>
    /// 缩略图创建服务简单工厂
    /// </summary>
    public class ThumbnailProviderFactory : System.Utility.Patterns.ISimpleFactory<EnumThumbnail, IThumbnailProvider>
    {
        /// <summary>
        /// 根据key获取实例
        /// </summary>
        public virtual IThumbnailProvider GetInstance(EnumThumbnail key)
        {
            switch (key)
            {
                case EnumThumbnail.Image:
                    return Singleton<ImageThumbnailProvider>.GetInstance();
                case EnumThumbnail.Vedio:
                    return Singleton<VedioThumbnailProvider>.GetInstance();
                case EnumThumbnail.WebImage:
                    return Singleton<WebImageThumbnailProvider>.GetInstance();
            }
            return null;
        }
    }
}