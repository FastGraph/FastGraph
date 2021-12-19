#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms;

namespace FastGraph.Tests.Algorithms.Contracts
{
    /// <summary>
    /// Tests related to <see cref="IDistancesCollection{TVertex}.GetDistances"/>.
    /// </summary>
    internal sealed class DistancesCollectionGetDistancesContract : DistancesCollectionContractBase
    {
        public DistancesCollectionGetDistancesContract(Type algorithmToTest)
            : base(algorithmToTest)
        {
        }

        [Test]
        public void DistancesForAllVerticesInGraphReturnedWhenAlgorithmHasBeenRun()
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

            IEnumerable<KeyValuePair<int, double>> distances = algorithm.GetDistances();
            distances.Select(pair => pair.Key).Should().BeEquivalentTo(new[] { 1, 2, 3 });
        }

        [Test]
        public void EmptyCollectionReturned_WhenAlgorithmHasNotYetBeenRun()
        {
            var scenario = new ContractScenario<int>
            {
                EdgesInGraph = new[] { new Edge<int>(1, 2) },
                SingleVerticesInGraph = new[] { 3 },
                AccessibleVerticesFromRoot = new[] { 2 },
                Root = 1,
                DoComputation = false
            };

            IDistancesCollection<int> algorithm = CreateAlgorithmAndMaybeDoComputation(scenario);

            IEnumerable<KeyValuePair<int, double>> distances = algorithm.GetDistances();
            distances.Should<KeyValuePair<int, double>>().BeEmpty();
        }
    }
}
