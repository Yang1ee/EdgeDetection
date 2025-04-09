
namespace EdgeDetection.ImageProcessing
{
    public static class OperatorFactory
    {
        public static IEdgeDetectionOperator GetOperator(string type)
        {
            return type switch
            {
                "Sobel" => new SobelOperator(),
                "Prewitt" => new PrewittOperator(),
                _ => throw new ArgumentException("Invalid operator type")
            };
        }
    }
}
