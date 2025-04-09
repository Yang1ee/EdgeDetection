// ImageHandler.cs
using System.Drawing;
using System.Drawing.Imaging;

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

        public static Bitmap ToBitmap(byte[,] imageData)
        {
            int width = imageData.GetLength(0);
            int height = imageData.GetLength(1);
            Bitmap bmp = new(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    byte gray = imageData[x, y];
                    bmp.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            return bmp;
        }
    }
}
