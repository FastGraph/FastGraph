using NUnit.Framework;
using QuikGraph.Algorithms.AssigmentProblem;
using QuikGraph.Tests;

namespace QuikGraph.Tests.Algorithms.AssigmentProblem
{
    [TestFixture]
    internal class HungarianAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        public void RunCheck()
        {
            var matrix = new[,] { { 1, 2, 3 }, { 3, 3, 3 }, { 3, 3, 2 } };
            var algorithm = new HungarianAlgorithm(matrix);
            var res = algorithm.Run();
            Assert.AreEqual(res[0], 0);
            Assert.AreEqual(res[1], 1);
            Assert.AreEqual(res[2], 2);
        }

        [Test]
        public void IterationsCheck()
        {
            var matrix = new[,] { { 1, 2, 3 }, { 3, 3, 3 }, { 3, 3, 2 } };
            var algorithm = new HungarianAlgorithm(matrix);
            var iterations = algorithm.GetIterations();
            var res = algorithm.AgentsTasks;
            Assert.AreEqual(res[0], 0);
            Assert.AreEqual(res[1], 1);
            Assert.AreEqual(res[2], 2);
            Assert.AreEqual(iterations.Count, 3);
        }
    }
}
