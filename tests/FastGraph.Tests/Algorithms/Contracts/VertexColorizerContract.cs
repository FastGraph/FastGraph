#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.MaximumFlow;
using FastGraph.Algorithms.RandomWalks;
using FastGraph.Algorithms.Search;
using FastGraph.Algorithms.ShortestPath;
using FastGraph.Algorithms.TSP;
using FastGraph.Tests.Algorithms.MaximumFlow;
using FastGraph.Tests.Algorithms.RandomWalks;
using FastGraph.Tests.Algorithms.Search;
using FastGraph.Tests.Algorithms.ShortestPath;
using FastGraph.Tests.Algorithms.TSP;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace FastGraph.Tests.Algorithms.Contracts
{
    /// <summary>
    /// Contract tests for <see cref="IVertexColorizerAlgorithm{TVertex}"/>.
    /// </summary>
    [TestFixtureSource(typeof(AlgorithmsProvider), nameof(AlgorithmsProvider.VertexColorizers))]
    internal sealed class VertexColorizerContract
    {
        private readonly Type _testedAlgorithm;

        /// <summary/>
        public VertexColorizerContract(Type algorithmToTest)
        {
            _testedAlgorithm = algorithmToTest;
        }

        [SetUp]
        public void IgnoreIfInterfaceIsNotImplemented()
        {
            if (_testedAlgorithm == typeof(TSP<,,>))
            {
                Assert.Ignore("The TSP algorithm does not implement colorizer functionality.");
            }
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

            IVertexColorizerAlgorithm<int> algorithm = CreateAlgorithmAndMaybeDoComputation(scenario);

            Invoking(() => algorithm.GetVertexColor(3)).Should().Throw<VertexNotFoundException>();
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

            IVertexColorizerAlgorithm<int> algorithm = CreateAlgorithmAndMaybeDoComputation(scenario);

            Type expectedExceptionType = GetExpectedExceptionType();
            Invoking(() => algorithm.GetVertexColor(2)).Should().Throw<Exception>().Which.Should().BeAssignableTo(expectedExceptionType);

            #region Local function

            Type GetExpectedExceptionType()
            {
                switch (_testedAlgorithm)
                {
                    case var _ when _testedAlgorithm == typeof(AStarShortestPathAlgorithm<,>):
                    case var _ when _testedAlgorithm == typeof(BellmanFordShortestPathAlgorithm<,>):
                    case var _ when _testedAlgorithm == typeof(DagShortestPathAlgorithm<,>):
                    case var _ when _testedAlgorithm == typeof(DijkstraShortestPathAlgorithm<,>):
                    case var _ when _testedAlgorithm == typeof(UndirectedDijkstraShortestPathAlgorithm<,>):
                        return typeof(NullReferenceException);
                }

                return typeof(VertexNotFoundException);
            }

            #endregion
        }

        [Test]
        public void ColorReturned_WhenVertexIsAccessibleFromRoot()
        {
            var scenario = new ContractScenario<int>
            {
                EdgesInGraph = new[] { new Edge<int>(1, 2) },
                AccessibleVerticesFromRoot = new[] { 2 },
                Root = 1,
                DoComputation = true
            };

            IVertexColorizerAlgorithm<int> algorithm = CreateAlgorithmAndMaybeDoComputation(scenario);

            algorithm.Invoking(a => a.GetVertexColor(2)).Should().NotThrow();
        }

        [Test]
        public void ColorReturned_WhenVertexExistsButIsInaccessibleFromRoot()
        {
            var scenario = new ContractScenario<int>
            {
                EdgesInGraph = new[] { new Edge<int>(1, 2) },
                SingleVerticesInGraph = new[] { 3 },
                AccessibleVerticesFromRoot = new[] { 2 },
                Root = 1,
                DoComputation = true
            };

            IVertexColorizerAlgorithm<int> algorithm = CreateAlgorithmAndMaybeDoComputation(scenario);

            algorithm.Invoking(a => a.GetVertexColor(3)).Should().NotThrow();
        }

        [Pure]
        private IVertexColorizerAlgorithm<T> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
            where T : notnull
        {
            var instantiateAlgorithm = GetAlgorithmFactory<T>();
            return instantiateAlgorithm(scenario);
        }

        [Pure]
        private Func<ContractScenario<T>, IVertexColorizerAlgorithm<T>> GetAlgorithmFactory<T>()
            where T : notnull
        {
            return _testedAlgorithm switch
            {
                _ when _testedAlgorithm == typeof(AStarShortestPathAlgorithm<,>) =>
                    AStartShortestPathAlgorithmTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(BellmanFordShortestPathAlgorithm<,>) =>
                    BellmanFordShortestPathAlgorithmTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(BidirectionalDepthFirstSearchAlgorithm<,>) =>
                    BidirectionalDepthFirstSearchAlgorithmTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(BreadthFirstSearchAlgorithm<,>) =>
                    BreadthFirstAlgorithmSearchTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(CyclePoppingRandomTreeAlgorithm<,>) =>
                    CyclePoppingRandomTreeAlgorithmTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(DagShortestPathAlgorithm<,>) =>
                    DagShortestPathAlgorithmTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(DepthFirstSearchAlgorithm<,>) =>
                    DepthFirstAlgorithmSearchTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(DijkstraShortestPathAlgorithm<,>) =>
                    DijkstraShortestPathAlgorithmTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(EdmondsKarpMaximumFlowAlgorithm<,>) =>
                    EdmondsKarpMaximumFlowAlgorithmTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(TSP<,,>) =>
                    TSPTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(UndirectedBreadthFirstSearchAlgorithm<,>) =>
                    UndirectedBreadthFirstAlgorithmSearchTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(UndirectedDepthFirstSearchAlgorithm<,>) =>
                    UndirectedDepthFirstAlgorithmSearchTests.CreateAlgorithmAndMaybeDoComputation,

                _ when _testedAlgorithm == typeof(UndirectedDijkstraShortestPathAlgorithm<,>) =>
                    UndirectedDijkstraShortestPathAlgorithmTests.CreateAlgorithmAndMaybeDoComputation,

                _ => throw new AssertionException($"No test constructor known for {_testedAlgorithm}.")
            };
        }
    }
}
