using System;
using System.Net;
using System.Windows.Media;

namespace Util.Controls
{
    /// <summary>
    /// 网络图片缩略图创建服务
    /// </summary>
    internal class WebImageThumbnailProvider : IThumbnailProvider
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
                var request = WebRequest.Create(path);
                request.Timeout = 20000;
                var stream = request.GetResponse().GetResponseStream();
                var img = System.Drawing.Image.FromStream(stream);
                return System.Utility.Helper.Images.CreateImageSourceThumbnia(img, width, height);
            }
            catch
            {
                return null;
            }
        }
    }
}