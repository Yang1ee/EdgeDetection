
namespace EdgeDetection.ImageProcessing
{
    public interface IEdgeDetectionOperator
    {
        byte[,] ApplyEdgeDetection(byte[,] imageData);
    }
}
