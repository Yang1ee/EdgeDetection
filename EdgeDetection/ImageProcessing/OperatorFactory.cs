
namespace EdgeDetection.ImageProcessing
{
    public static class OperatorFactory
    {
        public static IEdgeDetectionOperator GetOperator(string type)
        {
            return type.ToLower() switch
            {
                "sobel" => new SobelOperator(),
                "prewitt" => new PrewittOperator(),
                _ => throw new ArgumentException("Invalid operator type")
            };
        }
    }
}
