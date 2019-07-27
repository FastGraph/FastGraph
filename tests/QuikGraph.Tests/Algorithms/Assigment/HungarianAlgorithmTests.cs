using System.Linq;
using NUnit.Framework;
using QuikGraph.Algorithms.Assignment;

namespace QuikGraph.Tests.Algorithms.Assignment
{
    [TestFixture]
    internal class HungarianAlgorithmTests : QuikGraphUnitTests
    {
        [Test]
        public void Compute()
        {
            int[,] matrix = { { 1, 2, 3 }, { 3, 3, 3 }, { 3, 3, 2 } };
            var algorithm = new HungarianAlgorithm(matrix);
            int[] res = algorithm.Compute();
            Assert.AreEqual(res[0], 0);
            Assert.AreEqual(res[1], 1);
            Assert.AreEqual(res[2], 2);
        }

        [Test]
        public void GetIterations()
        {
            int[,] matrix = { { 1, 2, 3 }, { 3, 3, 3 }, { 3, 3, 2 } };
            var algorithm = new HungarianAlgorithm(matrix);
            HungarianIteration[] iterations = algorithm.GetIterations().ToArray();
            int[] res = algorithm.AgentsTasks;
            Assert.AreEqual(res[0], 0);
            Assert.AreEqual(res[1], 1);
            Assert.AreEqual(res[2], 2);
            Assert.AreEqual(iterations.Length, 3);
        }
    }
}
