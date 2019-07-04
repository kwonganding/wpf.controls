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
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace System.Utility.Helper
{
    /// <summary>
    /// 图片处理辅助类
    /// </summary>
    public static class Images
    {
        #region CreateThumbnail：从一个文件流中创建缩略图
        /// <summary>
        /// 从一个文件流中创建缩略图
        /// </summary>
        public static Image CreateThumbnail(Stream fileStream, int width, int height, Color penColor)
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
        public static Image CreateThumbnail(string fileName, int width, int height, Color penColor)
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
        public static Image CreateThumbnail(Image image, int width, int height, Color penColor)
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
        public static Image CreateThumbnail(Bitmap bitmap, int width, int height, Color penColor)
        {
            width = bitmap.Width > width ? width : bitmap.Width;
            height = bitmap.Height > height ? height : bitmap.Height;
            //创建缩略图
            Bitmap thumbnail = new Bitmap(width, height, PixelFormat.Format64bppPArgb);
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
                using (Pen pen = new Pen(penColor, 1)) //Color.Wheat
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

            bitmapSource.Freeze();
            System.Utility.Win32.Win32.DeleteObject(hBitmap);
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
            bitmapSource.Freeze();
            System.Utility.Win32.Win32.DeleteObject(hBitmap);
            sourceImage.Dispose();
            sourceBmp.Dispose();
            return bitmapSource;
        }
        #endregion

        /// <summary>
        /// 从数据流创建缩略图
        /// </summary>
        public static ImageSource CreateImageSourceThumbnia(byte[] data, double width, double height)
        {
            using (Stream stream = new MemoryStream(data, true))
            {
                using (Image img = Image.FromStream(stream))
                {
                    return CreateImageSourceThumbnia(img, width, height);
                }
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
                bs.Freeze();
                image.Dispose();
                System.Utility.Win32.Win32.DeleteObject(ptr);
                return bs;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}
