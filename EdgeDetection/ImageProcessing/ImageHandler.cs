using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace EdgeDetection.ImageProcessing
{

    public static class ImageHandler
    {
        public static byte[,] LoadImage(string path)
        {
            using Bitmap bmp = new(path);
            byte[,] result = new byte[bmp.Width, bmp.Height];

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color pixel = bmp.GetPixel(x, y);
                    byte gray = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                    result[x, y] = gray;
                }
            }
            return result;
        }
        /// <summary>
        /// Read the image from file and convert into the byte data
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[,] LoadImageAsGrayscaleArray(string path)
        {
            using Bitmap bmp = new(path);
            int width = bmp.Width;
            int height = bmp.Height;
            byte[,] imageData = new byte[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    System.Drawing.Color pixel = bmp.GetPixel(x, y);
                    byte gray = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                    imageData[x, y] = gray;
                }

            return imageData;
        }
        /// <summary>
        /// Convert image byte data to Bitmap format
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        public static Bitmap ConvertByteArrayToBitmap(byte[,] imageData)
        {
            int width = imageData.GetLength(0);
            int height = imageData.GetLength(1);
            Bitmap bmp = new(width, height);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    byte value = imageData[x, y];
                    bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(value, value, value));
                }

            return bmp;
        }
        /// <summary>
        /// Convert Bitmap to bitmapImage. BitmapImage is use for display in the image viewer in WPF
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static BitmapImage ConvertToBitmapImage(Bitmap bitmap)
        {
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);

            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = ms;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            return image;
        }

    }
}
