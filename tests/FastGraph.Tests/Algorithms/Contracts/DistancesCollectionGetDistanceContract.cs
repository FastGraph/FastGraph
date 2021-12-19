#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms;

namespace FastGraph.Tests.Algorithms.Contracts
{
    /// <summary>
    /// Tests related to <see cref="IDistancesCollection{TVertex}.GetDistance"/>.
    /// </summary>
    internal sealed class DistancesCollectionGetDistanceContract : DistancesCollectionContractBase
    {
        public DistancesCollectionGetDistanceContract(Type algorithmToTest)
            : base(algorithmToTest)
        {
        }

        [Test]
        public void ExceptionThrown_WhenVertexDoesNotExistInGraph()
        {
            var scenario = new ContractScenario<int>
            {
                EdgesInGraph = new[] { new Edge<int>(1, 2) },
                AccessibleVerticesFromRoot = new[] { 2 },
                Root = 1,
                DoComputation = true
            };

            IDistancesCollection<int> algorithm = CreateAlgorithmAndMaybeDoComputation(scenario);

            Invoking(() => { double _ = algorithm.GetDistance(3); }).Should().Throw<VertexNotFoundException>();
        }

        [Test]
        public void ExceptionThrown_WhenAlgorithmHasNotYetBeenComputed()
        {
            var scenario = new ContractScenario<int>
            {
                EdgesInGraph = new[] { new Edge<int>(1, 2) },
                SingleVerticesInGraph = new int[0],
                AccessibleVerticesFromRoot = new[] { 2 },
                Root = 1,
                DoComputation = false
            };

            IDistancesCollection<int> algorithm = CreateAlgorithmAndMaybeDoComputation(scenario);

            Invoking(() => { double _ = algorithm.GetDistance(2); }).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void ExceptionThrown_WhenTargetVertexIsNull()
        {
            var scenario = new ContractScenario<string>
            {
                EdgesInGraph = new[] { new Edge<string>("1", "2") },
                SingleVerticesInGraph = new string[0],
                AccessibleVerticesFromRoot = new[] { "2" },
                Root = "1",
                DoComputation = false
            };

            IDistancesCollection<string> algorithm = CreateAlgorithmAndMaybeDoComputation(scenario);

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => { double _ = algorithm.GetDistance(default); }).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void NoExceptionThrown_WhenVertexIsAccessibleFromRoot()
        {
            var scenario = new ContractScenario<int>
            {
                EdgesInGraph = new[] { new Edge<int>(1, 2) },
                AccessibleVerticesFromRoot = new[] { 2 },
                Root = 1,
                DoComputation = true
            };

            IDistancesCollection<int> algorithm = CreateAlgorithmAndMaybeDoComputation(scenario);

            Invoking(() => { double _ = algorithm.GetDistance(2); }).Should().NotThrow();
        }

        [Test]
        public void NoExceptionThrown_WhenVertexExistsButIsInaccessibleFromRoot()
        {
            var scenario = new ContractScenario<int>
            {
                EdgesInGraph = new[] { new Edge<int>(1, 2) },
                SingleVerticesInGraph = new[] { 3 },
                AccessibleVerticesFromRoot = new[] { 2 },
                Root = 1,
                DoComputation = true
            };

            IDistancesCollection<int> algorithm = CreateAlgorithmAndMaybeDoComputation(scenario);

            Invoking(() => { double _ = algorithm.GetDistance(3); }).Should().NotThrow();
        }
    }
}
