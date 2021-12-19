#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Assignment;

namespace FastGraph.Tests.Algorithms.Assignment
{
    /// <summary>
    /// Tests for <see cref="HungarianAlgorithm"/>.
    /// </summary>
    [TestFixture]
    internal sealed class HungarianAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            int[,] costs = new int[0, 0];
            var algorithm = new HungarianAlgorithm(costs);
            algorithm.AgentsTasks.Should().BeNull();

            costs = new[,]
            {
                { 1, 2, 3 },
                { 1, 2, 3 },
            };
            algorithm = new HungarianAlgorithm(costs);
            algorithm.AgentsTasks.Should().BeNull();
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new HungarianAlgorithm(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
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

            tasks[0].Should().Be(0);
            tasks[1].Should().Be(1);
            tasks[2].Should().Be(2);
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

            algorithm.AgentsTasks.Should().NotBeNull();
            int[] tasks = algorithm.AgentsTasks!;
            tasks[0].Should().Be(2); // J1 to be done by W3
            tasks[1].Should().Be(1); // J2 to be done by W2
            tasks[2].Should().Be(0); // J3 to be done by W1
            tasks[3].Should().Be(3); // J4 to be done by W4
        }

        [Test]
        public void SimpleAssignmentIterations_HasTasks()
        {
            int[,] matrix =
            {
                { 1, 2, 3 },
                { 3, 3, 3 },
                { 3, 3, 2 }
            };
            var algorithm = new HungarianAlgorithm(matrix);
            _ = algorithm.Compute();

            int[] tasks = algorithm.AgentsTasks!;
            tasks[0].Should().Be(0);
            tasks[1].Should().Be(1);
            tasks[2].Should().Be(2);
        }

        [Test]
        public void SimpleAssignmentIterations_HasIterations()
        {
            int[,] matrix =
            {
                { 1, 2, 3 },
                { 3, 3, 3 },
                { 3, 3, 2 }
            };
            var algorithm = new HungarianAlgorithm(matrix);
            HungarianIteration[] iterations = algorithm.GetIterations().ToArray();

            iterations.Should().HaveCount(3);
        }

        [Test]
        public void SimpleAssignmentIterations_HasIterationsWith_ExpectedMatrices()
        {
            int[,] matrix =
            {
                { 1, 2, 3 },
                { 3, 3, 3 },
                { 3, 3, 2 }
            };
            var algorithm = new HungarianAlgorithm(matrix);
            HungarianIteration[] iterations = algorithm.GetIterations().ToArray();

            var expectedMatrices =
                new[]
                {
                    new[,] { { 0, 1, 2 }, { 0, 0, 0 }, { 1, 1, 0 } },
                    new[,] { { 0, 1, 2 }, { 0, 0, 0 }, { 1, 1, 0 } },
                    new[,] { { 0, 1, 2 }, { 0, 0, 0 }, { 1, 1, 0 } }
                };
            iterations.Select(iteration => iteration.Matrix).Should().BeEquivalentTo(expectedMatrices);
        }

        [Test]
        public void SimpleAssignmentIterations_HasIterationsWith_ExpectedMasks()
        {
            int[,] matrix =
            {
                { 1, 2, 3 },
                { 3, 3, 3 },
                { 3, 3, 2 }
            };
            var algorithm = new HungarianAlgorithm(matrix);
            HungarianIteration[] iterations = algorithm.GetIterations().ToArray();

            var expectedMasks =
                new[]
                {
                    new byte[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } },
                    new byte[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } },
                    new byte[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } }
                };
            iterations.Select(iteration => iteration.Mask).Should().BeEquivalentTo(expectedMasks);
        }

        [Test]
        public void SimpleAssignmentIterations_HasIterationsWith_ExpectedRowsCovered()
        {
            int[,] matrix =
            {
                { 1, 2, 3 },
                { 3, 3, 3 },
                { 3, 3, 2 }
            };
            var algorithm = new HungarianAlgorithm(matrix);
            HungarianIteration[] iterations = algorithm.GetIterations().ToArray();

            var expectedRowsCovereds =
                new[]
                {
                    new[] { false, false, false },
                    new[] { false, false, false },
                    new[] { false, false, false }
                };
            iterations.Select(iteration => iteration.RowsCovered).Should().BeEquivalentTo(expectedRowsCovereds);
        }

        [Test]
        public void SimpleAssignmentIterations_HasIterationsWith_ExpectedColumnsCovered()
        {
            int[,] matrix =
            {
                { 1, 2, 3 },
                { 3, 3, 3 },
                { 3, 3, 2 }
            };
            var algorithm = new HungarianAlgorithm(matrix);
            HungarianIteration[] iterations = algorithm.GetIterations().ToArray();

            var expectedColumnsCovereds =
                new[]
                {
                    new[] { false, false, false },
                    new[] { true,  true,  true },
                    new[] { true,  true,  true }
                };
            iterations.Select(iteration => iteration.ColumnsCovered).Should().BeEquivalentTo(expectedColumnsCovereds);
        }

        [Test]
        public void SimpleAssignmentIterations_HasIterationsWith_ExpectedSteps()
        {
            int[,] matrix =
            {
                { 1, 2, 3 },
                { 3, 3, 3 },
                { 3, 3, 2 }
            };
            var algorithm = new HungarianAlgorithm(matrix);
            HungarianIteration[] iterations = algorithm.GetIterations().ToArray();

            var expectedSteps =
                new[]
                {
                    HungarianAlgorithm.Steps.Init,
                    HungarianAlgorithm.Steps.Step1,
                    HungarianAlgorithm.Steps.End
                };
            iterations.Select(iteration => iteration.Step).Should().BeEquivalentTo(expectedSteps);
        }

        [Test]
        public void JobAssignmentIterations_AgentTasks()
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

            algorithm.AgentsTasks.Should().NotBeNull();
            int[] tasks = algorithm.AgentsTasks!;
            tasks[0].Should().Be(2); // J1 to be done by W3
            tasks[1].Should().Be(1); // J2 to be done by W2
            tasks[2].Should().Be(0); // J3 to be done by W1
            tasks[3].Should().Be(3); // J4 to be done by W4
        }

        [Test]
        public void JobAssignmentIterations_HasIterations()
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

            iterations.Should().HaveCount(11);
        }

        [Test]
        public void JobAssignmentIterations_HasIterations_WithExpectedMatrices()
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

            var expectedMatrices =
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
                };
            iterations.Select(iteration => iteration.Matrix).Should().BeEquivalentTo(expectedMatrices);
        }

        [Test]
        public void JobAssignmentIterations_HasIterations_WithExpectedMasks()
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

            var expectedMasks =
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
                };
            iterations.Select(iteration => iteration.Mask).Should().BeEquivalentTo(expectedMasks);
        }

        [Test]
        public void JobAssignmentIterations_HasIterations_WithExpectedRowsCovered()
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

            var expectedRowsCovereds =
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
                };
            iterations.Select(iteration => iteration.RowsCovered).Should().BeEquivalentTo(expectedRowsCovereds);
        }

        [Test]
        public void JobAssignmentIterations_HasIterations_WithExpectedColumnsCovered()
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

            var expectedColumnsCovereds =
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
                };
            iterations.Select(iteration => iteration.ColumnsCovered).Should().BeEquivalentTo(expectedColumnsCovereds);
        }

        [Test]
        public void JobAssignmentIterations_HasIterations_WithExpectedSteps()
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

            var expectedSteps =
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
                };
            iterations.Select(iteration => iteration.Step).Should().BeEquivalentTo(expectedSteps);
        }
    }
}
