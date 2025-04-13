using EdgeDetection.ImageProcessing;

namespace EdgeDetection.Tests
{
    [TestClass]
    public class OperatorTests
    {
        [TestMethod]
        public void TestOperatorSelection_Sobel()
        {
            var op = OperatorFactory.GetOperator("sobel");
            Assert.IsInstanceOfType(op, typeof(SobelOperator));
        }

        [TestMethod]
        public void TestOperatorSelection_Prewitt()
        {
            var op = OperatorFactory.GetOperator("Prewitt");
            Assert.IsInstanceOfType(op, typeof(PrewittOperator));
        }
        [TestMethod]
        public void TestEdgeDetection_Sobel()
        {
            byte[,] dummyImage = new byte[3, 3]
            {
                { 255, 255, 255 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            };
            var op = new SobelOperator();
            var result = op.ApplyEdgeDetection(dummyImage);

            // Ensure output is not all zero
            bool hasEdge = false;
            foreach (var val in result)
            {
                if (val > 0)
                {
                    hasEdge = true;
                    break;
                }
            }

            Assert.IsTrue(hasEdge, "Expected edges to be detected.");
        }

        [TestMethod]
        public void TestEdgeDetection_Prewitt()
        {
            byte[,] dummyImage = new byte[3, 3]
            {
                { 255, 255, 255 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            };

            var op = new PrewittOperator();
            var result = op.ApplyEdgeDetection(dummyImage);

            // Ensure output is not all zero
            bool hasEdge = false;
            foreach (var val in result)
            {
                if (val > 0)
                {
                    hasEdge = true;
                    break;
                }
            }

            Assert.IsTrue(hasEdge, "Expected edges to be detected.");
        }

        [TestMethod]
        public void TestSobel_MinimalImage()
        {
            byte[,] image = new byte[1, 1]
            { { 100 } };

            var op = new SobelOperator();
            var result = op.ApplyEdgeDetection(image);
            Assert.AreEqual(1, result.GetLength(0));
            Assert.AreEqual(1, result.GetLength(1));
        }

        [TestMethod]
        public void TestPrewitt_MinimalImage()
        {
            byte[,] image = new byte[1, 1]
            { { 100 } };

            var op = new PrewittOperator();
            var result = op.ApplyEdgeDetection(image);
            Assert.AreEqual(1, result.GetLength(0));
            Assert.AreEqual(1, result.GetLength(1));
        }

        [TestMethod]
        public void TestSobel_OutputDimension()
        {
            byte[,] image = new byte[5, 7]; // Width: 5, Height: 7

            var op = new SobelOperator();
            var result = op.ApplyEdgeDetection(image);
            Assert.AreEqual(5, result.GetLength(0));
            Assert.AreEqual(7, result.GetLength(1));
        }

        [TestMethod]
        public void TestPrewitt_OutputDimension()
        {
            byte[,] image = new byte[5, 7]; // Width: 5, Height: 7

            var op = new PrewittOperator();
            var result = op.ApplyEdgeDetection(image);
            Assert.AreEqual(5, result.GetLength(0));
            Assert.AreEqual(7, result.GetLength(1));
        }
    }
}
