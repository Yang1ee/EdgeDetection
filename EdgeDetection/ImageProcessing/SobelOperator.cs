// SobelOperator.cs
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace EdgeDetection.ImageProcessing
{
    // SobelOperator.cs
    public class SobelOperator : IEdgeDetectionOperator
    {
        public byte[,] ApplyEdgeDetection(byte[,] imageData)
        {
            int width = imageData.GetLength(0);
            int height = imageData.GetLength(1);
            Image<Gray, byte> img = new(width, height);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    img.Data[y, x, 0] = imageData[x, y];

            var sobelX = img.Sobel(1, 0, 3);
            var sobelY = img.Sobel(0, 1, 3);

            Image<Gray, byte> resultImg = new(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int val = (int)Math.Sqrt(
                        Math.Pow(sobelX.Data[y, x, 0], 2) +
                        Math.Pow(sobelY.Data[y, x, 0], 2)
                    );
                    resultImg.Data[y, x, 0] = (byte)Math.Min(255, val);
                }
            }

            byte[,] result = new byte[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    result[x, y] = resultImg.Data[y, x, 0];

            return result;
        }
    }

}
