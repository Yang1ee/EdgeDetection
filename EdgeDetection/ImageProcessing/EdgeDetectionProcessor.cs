
namespace EdgeDetection.ImageProcessing
{
    public class EdgeDetectionProcessor
    {
        private readonly IEdgeDetectionOperator _operator;

        public EdgeDetectionProcessor(IEdgeDetectionOperator edgeOperator)
        {
            _operator = edgeOperator;
        }

        public byte[,] ProcessImage(byte[,] imageData)
        {
            return _operator.ApplyEdgeDetection(imageData);
        }
    }
}
