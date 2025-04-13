using Emgu.CV;
using Emgu.CV.Structure;

namespace EdgeDetection.ImageProcessing
{
    public class SobelOperator : IEdgeDetectionOperator
    {
        public byte[,] ApplyEdgeDetection(byte[,] imageData)
        {
            int height = imageData.GetLength(0); //row
            int width = imageData.GetLength(1);  //col

            Image<Gray, byte> img = new(width, height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    img.Data[y, x, 0] = imageData[y, x];

            // Sobel Kernel
            var sobelX = img.Sobel(1, 0, 3);
            var sobelY = img.Sobel(0, 1, 3);

            byte[,] result = new byte[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var gx = sobelX.Data[y, x, 0];
                    var gy = sobelY.Data[y, x, 0];

                    int magnitude = (int)Math.Sqrt(gx * gx + gy * gy);
                    result[y, x] = (byte)Math.Min(255, magnitude);
                }
            }

            return result;
        }
    }

}
