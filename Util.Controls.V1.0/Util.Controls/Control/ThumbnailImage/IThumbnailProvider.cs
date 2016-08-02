using System;
using System.Windows.Media;

namespace Util.Controls
{
    /// <summary>
    /// 缩略图创建服务接口
    /// </summary>
    public interface IThumbnailProvider
    {
        /// <summary>
        /// 创建缩略图。fileName:文件路径；width:图片宽度；height:高度
        /// </summary>
        ImageSource GenereateThumbnail(object fileSource, double width, double height);
    }
}