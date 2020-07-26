using System;
using System.Linq;
using NUnit.Framework;
using QuikGraph.Algorithms.Assignment;

namespace QuikGraph.Tests.Algorithms.Assignment
{
    /// <summary>
    /// Tests for <see cref="HungarianAlgorithm"/>.
    /// </summary>
    [TestFixture]
    internal class HungarianAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            int[,] costs = new int[0,0];
            var algorithm = new HungarianAlgorithm(costs);
            Assert.IsNull(algorithm.AgentsTasks);

            costs = new[,]
            {
                { 1, 2, 3 },
                { 1, 2, 3 },
            };
            algorithm = new HungarianAlgorithm(costs);
            Assert.IsNull(algorithm.AgentsTasks);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new HungarianAlgorithm(null));
        }

        [Test]
        public void SimpleAssignment()
        {
            int[,] matrix =
            {
                { 1, 2, 3 },
                { 3, 3, 3 },
                { 3, 3, 2 }
            };
            var algorithm = new HungarianAlgorithm(matrix);
            int[] tasks = algorithm.Compute();

            Assert.AreEqual(tasks[0], 0);
            Assert.AreEqual(tasks[1], 1);
            Assert.AreEqual(tasks[2], 2);
        }

        [Test]
        public void JobAssignment()
        {
            // J = Job | W = Worker
            //     J1  J2  J3  J4
            // W1  82  83  69  92
            // W2  77  37  49  92
            // W3  11  69  5   86
            // W4  8   9   98  23

            int[,] matrix =
            {
                { 82, 83, 69, 92 },
                { 77, 37, 49, 92 },
                { 11, 69, 5,  86 },
                { 8,  9,  98, 23 }
            };
            var algorithm = new HungarianAlgorithm(matrix);
            algorithm.Compute();

            Assert.IsNotNull(algorithm.AgentsTasks);
            int[] tasks = algorithm.AgentsTasks;
            Assert.AreEqual(tasks[0], 2); // J1 to be done by W3
            Assert.AreEqual(tasks[1], 1); // J2 to be done by W2
            Assert.AreEqual(tasks[2], 0); // J3 to be done by W1
            Assert.AreEqual(tasks[3], 3); // J4 to be done by W4
        }

        [Test]
        public void SimpleAssignmentIterations()
        {
            int[,] matrix =
            {
                { 1, 2, 3 },
                { 3, 3, 3 },
                { 3, 3, 2 }
            };
            var algorithm = new HungarianAlgorithm(matrix);
            HungarianIteration[] iterations = algorithm.GetIterations().ToArray();

            int[] tasks = algorithm.AgentsTasks;
            Assert.AreEqual(tasks[0], 0);
            Assert.AreEqual(tasks[1], 1);
            Assert.AreEqual(tasks[2], 2);

            Assert.AreEqual(iterations.Length, 3);
            CollectionAssert.AreEqual(
                new[]
                {
                    new[,] { { 0, 1, 2 }, { 0, 0, 0 }, { 1, 1, 0 } },
                    new[,] { { 0, 1, 2 }, { 0, 0, 0 }, { 1, 1, 0 } },
                    new[,] { { 0, 1, 2 }, { 0, 0, 0 }, { 1, 1, 0 } }
                },
                iterations.Select(iteration => iteration.Matrix));
            CollectionAssert.AreEqual(
                new[]
                {
                    new[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } },
                    new[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } },
                    new[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } }
                },
                iterations.Select(iteration => iteration.Mask));
            CollectionAssert.AreEqual(
                new[]
                {
                    new[] { false, false, false },
                    new[] { false, false, false },
                    new[] { false, false, false }
                },
                iterations.Select(iteration => iteration.RowsCovered));
            CollectionAssert.AreEqual(
                new[]
                {
                    new[] { false, false, false },
                    new[] { true,  true,  true },
                    new[] { true,  true,  true }
                },
                iterations.Select(iteration => iteration.ColumnsCovered));
            CollectionAssert.AreEqual(
                new[]
                {
                    HungarianAlgorithm.Steps.Init,
                    HungarianAlgorithm.Steps.Step1,
                    HungarianAlgorithm.Steps.End
                },
                iterations.Select(iteration => iteration.Step));
        }

        [Test]
        public void JobAssignmentIterations()
        {
            // J = Job | W = Worker
            //     J1  J2  J3  J4
            // W1  82  83  69  92
            // W2  77  37  49  92
            // W3  11  69  5   86
            // W4  8   9   98  23

            int[,] matrix =
            {
                { 82, 83, 69, 92 },
                { 77, 37, 49, 92 },
                { 11, 69, 5,  86 },
                { 8,  9,  98, 23 }
            };
            var algorithm = new HungarianAlgorithm(matrix);
            HungarianIteration[] iterations = algorithm.GetIterations().ToArray();

            Assert.IsNotNull(algorithm.AgentsTasks);
            int[] tasks = algorithm.AgentsTasks;
            Assert.AreEqual(tasks[0], 2); // J1 to be done by W3
            Assert.AreEqual(tasks[1], 1); // J2 to be done by W2
            Assert.AreEqual(tasks[2], 0); // J3 to be done by W1
            Assert.AreEqual(tasks[3], 3); // J4 to be done by W4

            Assert.AreEqual(iterations.Length, 11);
            CollectionAssert.AreEqual(
                new[]
                {
                    new[,] { { 13, 14, 0, 23 }, { 40, 0, 12, 55 }, { 6, 64, 0, 81 }, { 0, 1, 90, 15 } },
                    new[,] { { 13, 14, 0, 23 }, { 40, 0, 12, 55 }, { 6, 64, 0, 81 }, { 0, 1, 90, 15 } },
                    new[,] { { 13, 14, 0, 23 }, { 40, 0, 12, 55 }, { 6, 64, 0, 81 }, { 0, 1, 90, 15 } },
                    new[,] { { 13, 14, 0, 8 }, { 40, 0, 12, 40 }, { 6, 64, 0, 66 }, { 0, 1, 90, 0 } },
                    new[,] { { 13, 14, 0, 8 }, { 40, 0, 12, 40 }, { 6, 64, 0, 66 }, { 0, 1, 90, 0 } },
                    new[,] { { 13, 14, 0, 8 }, { 40, 0, 12, 40 }, { 6, 64, 0, 66 }, { 0, 1, 90, 0 } },
                    new[,] { { 7, 14, 0, 2 }, { 34, 0, 12, 34 }, { 0, 64, 0, 60 }, { 0, 7, 96, 0 } },
                    new[,] { { 7, 14, 0, 2 }, { 34, 0, 12, 34 }, { 0, 64, 0, 60 }, { 0, 7, 96, 0 } },
                    new[,] { { 7, 14, 0, 2 }, { 34, 0, 12, 34 }, { 0, 64, 0, 60 }, { 0, 7, 96, 0 } },
                    new[,] { { 7, 14, 0, 2 }, { 34, 0, 12, 34 }, { 0, 64, 0, 60 }, { 0, 7, 96, 0 } },
                    new[,] { { 7, 14, 0, 2 }, { 34, 0, 12, 34 }, { 0, 64, 0, 60 }, { 0, 7, 96, 0 } }
                },
                iterations.Select(iteration => iteration.Matrix));
            CollectionAssert.AreEqual(
                new[]
                {
                    new[,] { { 0, 0, 1, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 1, 0, 0, 0 } },
                    new[,] { { 0, 0, 1, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 1, 0, 0, 0 } },
                    new[,] { { 0, 0, 1, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 1, 0, 0, 0 } },
                    new[,] { { 0, 0, 1, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 1, 0, 0, 0 } },
                    new[,] { { 0, 0, 1, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 1, 0, 0, 2 } },
                    new[,] { { 0, 0, 1, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 1, 0, 0, 2 } },
                    new[,] { { 0, 0, 1, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 1, 0, 0, 2 } },
                    new[,] { { 0, 0, 1, 0 }, { 0, 1, 0, 0 }, { 2, 0, 0, 0 }, { 1, 0, 0, 2 } },
                    new[,] { { 0, 0, 1, 0 }, { 0, 1, 0, 0 }, { 1, 0, 0, 0 }, { 0, 0, 0, 1 } },
                    new[,] { { 0, 0, 1, 0 }, { 0, 1, 0, 0 }, { 1, 0, 0, 0 }, { 0, 0, 0, 1 } },
                    new[,] { { 0, 0, 1, 0 }, { 0, 1, 0, 0 }, { 1, 0, 0, 0 }, { 0, 0, 0, 1 } }
                },
                iterations.Select(iteration => iteration.Mask));
            CollectionAssert.AreEqual(
                new[]
                {
                    new[] { false, false, false, false },
                    new[] { false, false, false, false },
                    new[] { false, false, false, false },
                    new[] { false, false, false, false },
                    new[] { false, false, false, true },
                    new[] { false, false, false, true },
                    new[] { false, false, false, true },
                    new[] { false, false, false, true },
                    new[] { false, false, false, false },
                    new[] { false, false, false, false },
                    new[] { false, false, false, false }
                },
                iterations.Select(iteration => iteration.RowsCovered));
            CollectionAssert.AreEqual(
                new[]
                {
                    new[] { false, false, false, false },
                    new[] { true,  true,  true,  false },
                    new[] { true,  true,  true,  false },
                    new[] { true,  true,  true,  false },
                    new[] { false, true,  true,  false },
                    new[] { false, true,  true,  false },
                    new[] { false, true,  true,  false },
                    new[] { false, true,  true,  false },
                    new[] { false, false, false, false },
                    new[] { true,  true,  true,  true },
                    new[] { true,  true,  true,  true }
                },
                iterations.Select(iteration => iteration.ColumnsCovered));
            CollectionAssert.AreEqual(
                new[]
                {
                    HungarianAlgorithm.Steps.Init,
                    HungarianAlgorithm.Steps.Step1,
                    HungarianAlgorithm.Steps.Step2,
                    HungarianAlgorithm.Steps.Step4,
                    HungarianAlgorithm.Steps.Step2,
                    HungarianAlgorithm.Steps.Step2,
                    HungarianAlgorithm.Steps.Step4,
                    HungarianAlgorithm.Steps.Step2,
                    HungarianAlgorithm.Steps.Step3,
                    HungarianAlgorithm.Steps.Step1,
                    HungarianAlgorithm.Steps.End
                },
                iterations.Select(iteration => iteration.Step));
        }
    }
}