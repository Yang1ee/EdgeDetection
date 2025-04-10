// ImageHandler.cs
using System.Drawing;
using System.Drawing.Imaging;

namespace EdgeDetection.ImageProcessing
{

    public static class ImageHandler
    {
        // 3x3 Gaussian kernel
        private static readonly double[,] GaussianKernel = {
        { 1, 2, 1 },
        { 2, 4, 2 },
        { 1, 2, 1 }
        };

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

        public static byte[,] ApplyGaussianFilter(byte[,] image)
        {
            int width = image.GetLength(1);
            int height = image.GetLength(0);
            byte[,] result = new byte[height, width];

            // Normalize kernel
            double kernelSum = 16.0;

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    double pixel = 0.0;

                    for (int ky = -1; ky <= 1; ky++)
                    {
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            pixel += image[y + ky, x + kx] * GaussianKernel[ky + 1, kx + 1];
                        }
                    }

                    pixel /= kernelSum;
                    result[y, x] = (byte)Math.Min(255, Math.Max(0, pixel));
                }
            }

            return result;
        }
    }
}
