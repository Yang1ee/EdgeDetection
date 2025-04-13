using Emgu.CV;
using Emgu.CV.Structure;

namespace EdgeDetection.ImageProcessing
{
    // prewitt operator
    public class PrewittOperator : IEdgeDetectionOperator
    {
        public byte[,] ApplyEdgeDetection(byte[,] imageData)
        {
            int height = imageData.GetLength(0);
            int width = imageData.GetLength(1);


            Image<Gray, byte> img = new(width, height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    img.Data[y, x, 0] = imageData[y, x];

            // Prewitt kernels
            float[,] kernelX = new float[,]
            {
                { -1, 0, 1 },
                { -1, 0, 1 },
                { -1, 0, 1 }
                };

            float[,] kernelY = new float[,]
            {
                {  1,  1,  1 },
                {  0,  0,  0 },
                { -1, -1, -1 }
            };

            ConvolutionKernelF kx = new(kernelX);
            ConvolutionKernelF ky = new(kernelY);

            var gx = img.Convolution(kx);
            var gy = img.Convolution(ky);


            byte[,] result = new byte[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var dx = gx.Data[y, x, 0];
                    var dy = gy.Data[y, x, 0];

                    int magnitude = (int)Math.Sqrt(dx * dx + dy * dy);
                    result[y, x] = (byte)Math.Min(255, magnitude);
                }
            }

            return result;
        }
    }
}
