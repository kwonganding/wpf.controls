using System;
using System.Windows.Media;

namespace Util.Controls
{
    /// <summary>
    /// 本地视频缩略图创建服务
    /// 使用工具ffmpeg提取帧图片
    /// </summary>
    internal class VedioThumbnailProvider : IThumbnailProvider
    {
        private readonly string _ToolPath;
        private readonly string _Frame = "00:00:01";

        #region VedioThumbnailProvider-构造函数（初始化）

        /// <summary>
        ///  VedioThumbnailProvider-构造函数（初始化）
        /// </summary>
        public VedioThumbnailProvider()
        {
            this._ToolPath = System.Utility.Helper.File.GetPhysicalPath(@"toolkit/Ffmpeg/ffmpeg");
        }

        #endregion

        /// <summary>
        /// 创建缩略图。fileName:文件路径；width:图片宽度；height:高度
        /// </summary>
        public ImageSource GenereateThumbnail(object fileName, double width, double height)
        {
            try
            {
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}