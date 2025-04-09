using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace EdgeDetection.ImageProcessing
{
    // prewitt operator
    public class PrewittOperator : IEdgeDetectionOperator
    {
        public byte[,] ApplyEdgeDetection(byte[,] imageData)
        {
            int width = imageData.GetLength(0);
            int height = imageData.GetLength(1);
            Image<Gray, byte> img = new(width, height);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    img.Data[y, x, 0] = imageData[x, y];

            //core kernerl for the prewitt operator
            float[,] kernelX = new float[,] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
            float[,] kernelY = new float[,] { { 1, 1, 1 }, { 0, 0, 0 }, { -1, -1, -1 } };

            ConvolutionKernelF kx = new(kernelX);
            ConvolutionKernelF ky = new(kernelY);

            var gx = img.Convolution(kx);
            var gy = img.Convolution(ky);

            Image<Gray, byte> resultImg = new(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int val = (int)Math.Sqrt(
                        Math.Pow(gx.Data[y, x, 0], 2) +
                        Math.Pow(gy.Data[y, x, 0], 2)
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
