using System;
using System.Windows.Media;

namespace Util.Controls
{
    /// <summary>
    /// 本地图片缩略图创建服务
    /// </summary>
    internal class ImageThumbnailProvider : IThumbnailProvider
    {
        /// <summary>
        /// 创建缩略图。fileName:文件路径；width:图片宽度；height:高度
        /// </summary>
        public ImageSource GenereateThumbnail(object fileName, double width, double height)
        {
            try
            {
                var path = fileName.ToSafeString();
                if (path.IsInvalid()) return null;
                return System.Utility.Helper.Images.CreateImageSourceThumbnia(path, width, height);
            }
            catch
            {
                return null;
            }
        }
    }
}