using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Util.Controls
{
    /// <summary>
    /// 图片处理辅助类
    /// </summary>
    public static class ImageHelper
    {
        public static string ImageExtension = ".jpg;.png;.gif;.jpeg;.ico;.bmp";
        public static string ImageIsInvalidMessage = "文件[{0}]不是合法的图片或者图片分辨率超过16384";

        public static int MaxPixel = 16384;
        /****************** Image ******************/

        #region CreateThumbnail：从一个文件流中创建缩略图
        /// <summary>
        /// 从一个文件流中创建缩略图
        /// </summary>
        public static Image CreateThumbnail(Stream fileStream, int width, int height, System.Drawing.Color penColor)
        {
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(fileStream);
            }
            catch
            {
                bitmap = new Bitmap(width, height);
            }
            return CreateThumbnail(bitmap, width, height, penColor);
        }
        #endregion

        #region CreateThumbnail：从一个图片文件创建缩略图
        /// <summary>
        /// 从一个图片文件创建缩略图（绝对路径）
        /// </summary>
        public static Image CreateThumbnail(string fileName, int width, int height, System.Drawing.Color penColor)
        {
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(fileName);
            }
            catch
            {
                bitmap = new Bitmap(width, height);
            }
            return CreateThumbnail(bitmap, width, height, penColor);
        }
        #endregion

        #region CreateThumbnail：根据一个图片创建其缩略图
        /// <summary>
        /// 根据一个图片创建其缩略图
        /// </summary>
        public static Image CreateThumbnail(Image image, int width, int height, System.Drawing.Color penColor)
        {
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(image);
            }
            catch
            {
                bitmap = new Bitmap(width, height);
            }
            return CreateThumbnail(bitmap, width, height, penColor);
        }
        #endregion

        #region CreateThumbnail：根据一个图片创建其缩略图
        /// <summary>
        /// 根据一个图片创建其缩略图
        /// </summary>
        public static Image CreateThumbnail(Bitmap bitmap, int width, int height, System.Drawing.Color borderColor)
        {
            width = bitmap.Width > width ? width : bitmap.Width;
            height = bitmap.Height > height ? height : bitmap.Height;
            //创建缩略图
            Bitmap thumbnail = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format64bppPArgb);
            using (Graphics g = Graphics.FromImage(thumbnail))
            {
                //计算高宽
                int tnWidth = width;
                int tnHeight = height;
                if (bitmap.Width > bitmap.Height)
                {
                    tnHeight = (int)(((float)bitmap.Height / (float)bitmap.Width) * tnWidth);
                }
                else if (bitmap.Width < bitmap.Height)
                {
                    tnWidth = (int)(((float)bitmap.Width / (float)bitmap.Height) * tnHeight);
                }
                int iLeft = (width / 2) - (tnWidth / 2);
                int iTop = (height / 2) - (tnHeight / 2);
                //绘制图形
                g.PixelOffsetMode = PixelOffsetMode.None;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap, iLeft, iTop, tnWidth, tnHeight);
                //绘制边框
                using (System.Drawing.Pen pen = new System.Drawing.Pen(borderColor, 1)) //Color.Wheat
                {
                    g.DrawRectangle(pen, 0, 0, thumbnail.Width - 1, thumbnail.Height - 1);
                }
                return thumbnail;
            }
        }
        #endregion

        #region DrawImage：在指定区域绘制图片(可设置图片透明度) (平铺绘制）
        /// <summary>
        /// 在指定区域绘制图片(可设置图片透明度) (平铺绘制）
        /// Draws the image.
        /// </summary>
        public static void DrawImage(Graphics g, Rectangle rect, Image img, float opacity)
        {
            if (opacity <= 0)
            {
                return;
            }

            using (ImageAttributes imgAttributes = new ImageAttributes())
            {
                SetImageOpacity(imgAttributes, opacity >= 1 ? 1 : opacity);
                Rectangle imageRect = new Rectangle(rect.X, rect.Y + rect.Height / 2 - img.Size.Height / 2, img.Size.Width, img.Size.Height);
                g.DrawImage(img, rect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttributes);
            }
        }
        #endregion

        #region 设置图片透明度(SetImageOpacity)

        /// <summary>
        /// 设置图片透明度.
        /// </summary>
        /// <param name="imgAttributes">The ImageAttributes.</param>
        /// <param name="opacity">透明度，0完全透明，1不透明（The opacity.）</param>
        /// User:Ryan  CreateTime:2011-07-28 15:26.
        public static void SetImageOpacity(ImageAttributes imgAttributes, float opacity)
        {
            float[][] nArray ={ new float[] {1, 0, 0, 0, 0},
                                                new float[] {0, 1, 0, 0, 0},
                                                new float[] {0, 0, 1, 0, 0},
                                                new float[] {0, 0, 0, opacity, 0},
                                                new float[] {0, 0, 0, 0, 1}};
            ColorMatrix matrix = new ColorMatrix(nArray);
            imgAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        }

        #endregion

        /****************** ImageSource ******************/

        #region IsValid

        /// <summary>
        /// 验证图片文件是否合法，返回fasle=文件不是合法的图片文件或者图片分辨率超过16000.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsValid(string fileName)
        {
            if (fileName.IsInvalid())
                return false;
            try
            {
                if (!File.Exists(fileName))
                    return false;
                //判断是图片文件
                var ext = Path.GetExtension(fileName).ToLower();
                if (!ImageExtension.Contains(ext))
                    return false;
                FileInfo info = new FileInfo(fileName);
                if (info.Length <= 0)
                    return false;

                var decoder = BitmapDecoder.Create(new Uri(fileName), BitmapCreateOptions.DelayCreation,
                    BitmapCacheOption.None);
                var height = decoder.Frames[0].PixelHeight;
                var width = decoder.Frames[0].PixelWidth;
                if (height > MaxPixel || width > MaxPixel)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region CreateImageSourceThumbnial：使用fileName创建WPF使用的ImageSource类型缩略图
        /// <summary>
        /// 创建WPF使用的ImageSource类型缩略图（不放大小图）
        /// </summary>
        /// <param name="fileName">本地图片路径</param>
        /// <param name="width">指定宽度</param>
        /// <param name="height">指定高度</param>
        public static ImageSource CreateImageSourceThumbnia(string fileName, double width, double height)
        {
            System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(fileName);
            double rw = width / sourceImage.Width;
            double rh = height / sourceImage.Height;
            var aspect = (float)Math.Min(rw, rh);
            int w = sourceImage.Width, h = sourceImage.Height;
            if (aspect < 1)
            {
                w = (int)Math.Round(sourceImage.Width * aspect); h = (int)Math.Round(sourceImage.Height * aspect);
            }
            Bitmap sourceBmp = new Bitmap(sourceImage, w, h);
            IntPtr hBitmap = sourceBmp.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());
            if (!bitmapSource.IsFrozen && bitmapSource.CanFreeze)
            {
                bitmapSource.Freeze();
            }
            Win32Helper.DeleteObject(hBitmap);
            sourceImage.Dispose();
            sourceBmp.Dispose();
            return bitmapSource;
        }
        #endregion

        #region CreateImageSourceThumbnial：创建WPF使用的ImageSource类型缩略图
        /// <summary>
        /// 使用System.Drawing.Image创建WPF使用的ImageSource类型缩略图（不放大小图）
        /// </summary>
        /// <param name="sourceImage">System.Drawing.Image 对象</param>
        /// <param name="width">指定宽度</param>
        /// <param name="height">指定高度</param>
        public static ImageSource CreateImageSourceThumbnia(System.Drawing.Image sourceImage, double width, double height)
        {
            if (sourceImage == null) return null;
            double rw = width / sourceImage.Width;
            double rh = height / sourceImage.Height;
            var aspect = (float)Math.Min(rw, rh);
            int w = sourceImage.Width, h = sourceImage.Height;
            if (aspect < 1)
            {
                w = (int)Math.Round(sourceImage.Width * aspect); h = (int)Math.Round(sourceImage.Height * aspect);
            }
            Bitmap sourceBmp = new Bitmap(sourceImage, w, h);
            IntPtr hBitmap = sourceBmp.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());
            if (!bitmapSource.IsFrozen && bitmapSource.CanFreeze)
            {
                bitmapSource.Freeze();
            }
            Win32Helper.DeleteObject(hBitmap);
            sourceImage.Dispose();
            sourceBmp.Dispose();
            return bitmapSource;
        }
        #endregion

        /// <summary>
        /// 从数据流创建缩略图
        /// width设置解析图片宽度（高宽会自动按比例缩放），默认为0（原始大小）
        /// </summary>
        public static ImageSource CreateImageSourceFromByte(byte[] data, int width = 0)
        {
            using (Stream stream = new MemoryStream(data, false))
            {
                return CreateImageSourceFromStream(stream, width);
            }
        }

        #region CreateImageSourceFromImage:从一个Bitmap创建ImageSource
        /// <summary>
        /// 从一个Bitmap创建ImageSource
        /// </summary>
        /// <param name="image">Bitmap对象</param>
        /// <returns></returns>
        public static ImageSource CreateImageSourceFromImage(Bitmap image)
        {
            if (image == null) return null;
            try
            {
                IntPtr ptr = image.GetHbitmap();
                BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(ptr, IntPtr.Zero, Int32Rect.Empty,
                                                                        BitmapSizeOptions.FromEmptyOptions());
                if (!bs.IsFrozen && bs.CanFreeze)
                {
                    bs.Freeze();
                }
                image.Dispose();
                Win32Helper.DeleteObject(ptr);
                return bs;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region CreateImageSourceFromFile:从一个图片文件创建ImageSource
        /// <summary>
        /// 从一个图片文件创建ImageSource，支持当前程序的相对路径.
        /// width设置解析图片宽度，默认为0（原始大小）
        /// </summary>
        /// <param name="image">Bitmap对象</param>
        public static ImageSource CreateImageSourceFromFile(string file, int width = 0, int height = 0, bool skipExceptioin = true)
        {
            if (file.IsInvalid()) return null;
            try
            {
                var path = file.TrimStart(' ', '/', '\\');
                //如果是相对路径则转换为绝对路径
                if (!Path.IsPathRooted(path))
                {
                    path = FileHelper.GetPhysicalPath(path);
                }
                if (!System.IO.File.Exists(path)) return null;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnDemand;
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                if (width > 0) bitmapImage.DecodePixelWidth = width;
                if (height > 0) bitmapImage.DecodePixelHeight = height;
                bitmapImage.UriSource = new Uri(path);
                bitmapImage.EndInit();
                if (!bitmapImage.IsFrozen && bitmapImage.CanFreeze)
                {
                    bitmapImage.Freeze();
                }
                return bitmapImage;
            }
            catch (Exception ex)
            {
                if (skipExceptioin)
                    return null;
                ex.Data.Add("file", file);
                throw;
            }
        }
        #endregion

        #region CreateImageSourceFromStream:从一个图片文件创建ImageSource
        /// <summary>
        /// 从一个流创建ImageSource.
        /// width设置解析图片宽度（高宽会自动按比例缩放），默认为0（原始大小）
        /// </summary>
        /// <param name="image">Bitmap对象</param>
        public static BitmapImage CreateImageSourceFromStream(Stream stream, int width = 0)
        {
            if (stream == null ) return null;
            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnDemand;
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                if (width > 0) bitmapImage.DecodePixelWidth = width;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                if (!bitmapImage.IsFrozen && bitmapImage.CanFreeze)
                {
                    bitmapImage.Freeze();
                }
                return bitmapImage;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region ConvertPngToJpeg

        /// <summary>
        /// 转换Png的格式为jpeg的格式
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ConvertPngToJpeg(MemoryStream stream)
        {
            var bitmap = new Bitmap(stream);
            Bitmap thumbnail = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppPArgb);
            using (Graphics g = Graphics.FromImage(thumbnail))
            {
                //绘制图形
                g.PixelOffsetMode = PixelOffsetMode.None;
                g.InterpolationMode = InterpolationMode.Default;
                g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
                byte[] jpgArray;
                using (MemoryStream msJpeg = new MemoryStream())
                {
                    thumbnail.Save(msJpeg, ImageFormat.Jpeg);
                    jpgArray = msJpeg.ToArray();
                }
                return jpgArray;
            }
        }

        #endregion

        #region CreateImageSourceFromStream:从一个图片创建ImageSource       
        /// <summary>
        /// Creates the thumbnail from image source.
        /// </summary>
        /// <param name="imageSource">The image source.</param>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        public static byte[] CreateThumbnailFromImageSource(BitmapSource imageSource, int width)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var scalSize = width * 1.0 / imageSource.Width;
                var scare = new ScaleTransform(scalSize, scalSize);
                var scaredBitMap = new TransformedBitmap(BitmapFrame.Create(imageSource), scare);
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(scaredBitMap));
                encoder.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms.GetBuffer();
            }


        }

        #endregion

        /// <summary>
        /// 保存文件数据为随机PNG图片文件。
        /// </summary>
        /// <param name="imageBuffer">要保存的文件数据。</param>
        public static string SaveToPngFile(byte[] imageBuffer)
        {
            if (imageBuffer.IsInvalid())
            {
                throw new ArgumentException("文件数据不应为null或为空", nameof(imageBuffer));
            }

            using (var stream = new MemoryStream(imageBuffer))
            {
                using (var image = Image.FromStream(stream))
                {
                    var fileName = Path.GetTempFileName();
                    fileName = Path.ChangeExtension(fileName, "png");
                    image.Save(fileName);
                    image.Dispose();
                    return fileName;
                }
            }
        }

        /// <summary>
        /// 获取图片图像大小。
        /// </summary>
        /// <param name="imageFileName">图片文件名。</param>
        /// <param name="width">图片图像宽度。</param>
        /// <param name="height">图片图像高度。</param>
        public static void GetImageDimensions(string imageFileName, out float width, out float height)
        {
            if (string.IsNullOrWhiteSpace(imageFileName))
            {
                throw new ArgumentException("图片文件名不应null或为空", nameof(imageFileName));
            }
            using (FileStream file = new FileStream(imageFileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (Image tif = Image.FromStream(stream: file,
                                                    useEmbeddedColorManagement: false,
                                                    validateImageData: false))
                    {
                        width = tif.PhysicalDimension.Width;
                        height = tif.PhysicalDimension.Height;
                    }
                }
                catch (ArgumentException)
                {
                    //Invalid image file
                    width = 0;
                    height = 0;
                }
            }
        }
    }
}
