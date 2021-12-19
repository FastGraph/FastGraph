#nullable enable

using System.Linq.Expressions;
using NUnit.Framework;
using FastGraph.Algorithms;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="MaximumBipartiteMatchingAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class MaximumBipartiteMatchingAlgorithmTests
    {
        #region Test helpers

        private readonly EdgeFactory<string, Edge<string>> _edgeFactory =
            (source, target) => new Edge<string>(source, target);

        private static Expression<Func<TEdge, bool>> CreateExpressionForIsValidMaxMatchEdge<TVertex, TEdge>(TVertex[] vertexSetA, TVertex[] vertexSetB)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            return
                (edge) =>
                    vertexSetA.Contains(edge.Source) && vertexSetB.Contains(edge.Target)
                    || vertexSetB.Contains(edge.Source) && vertexSetA.Contains(edge.Target);
        }

        private static void MaxBipartiteMatch<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph,
            TVertex[] vertexSetA,
            TVertex[] vertexSetB,
            VertexFactory<TVertex> vertexFactory,
            EdgeFactory<TVertex, TEdge> edgeFactory,
            int expectedMatchSize)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            (graph.VertexCount > 0).Should().BeTrue();

            var maxMatch = new MaximumBipartiteMatchingAlgorithm<TVertex, TEdge>(
                graph,
                vertexSetA,
                vertexSetB,
                vertexFactory,
                edgeFactory);

            DateTime startTime = DateTime.Now;

            maxMatch.Compute();

            TimeSpan computeTime = DateTime.Now - startTime;

            (computeTime < TimeSpan.FromMinutes(5)).Should().BeTrue();

            maxMatch.MatchedEdges.Should().OnlyContain(CreateExpressionForIsValidMaxMatchEdge<TVertex, TEdge>(vertexSetA, vertexSetB));

            maxMatch.MatchedEdges.Length.Should().Be(expectedMatchSize);
        }

        private void RunBipartiteMatchAndCheck(
            IEnumerable<Edge<string>> edges,
            IEnumerable<string> setA,
            IEnumerable<string> setB,
            int expectedMatchSize)
        {
            var graph = edges.ToAdjacencyGraph<string, Edge<string>>();

            var vertexFactory = new StringVertexFactory();

            if (graph.VertexCount > 0)
            {
                MaxBipartiteMatch(
                    graph,
                    setA.ToArray(),
                    setB.ToArray(),
                    () => vertexFactory.CreateVertex(),
                    _edgeFactory,
                    expectedMatchSize);
            }
        }

        #endregion

        #region Test classes

        private sealed class StringVertexFactory
        {
            private int _id;

            private readonly string _prefix;

            public StringVertexFactory()
                : this("Super")
            {
            }

            private StringVertexFactory(string prefix)
            {
                _prefix = prefix;
            }

            public string CreateVertex()
            {
                return $"{_prefix}{++_id}";
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            int[] sourceToVertices = { 1, 2 };
            int[] verticesToSink = { 1, 2 };

            var algorithm = new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(
                graph,
                sourceToVertices,
                verticesToSink,
                vertexFactory,
                edgeFactory);
            AssertAlgorithmProperties(
                algorithm,
                graph,
                sourceToVertices,
                verticesToSink,
                vertexFactory,
                edgeFactory);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                MaximumBipartiteMatchingAlgorithm<TVertex, TEdge> algo,
                IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
                IEnumerable<TVertex> soToV,
                IEnumerable<TVertex> vToSi,
                VertexFactory<int> vFactory,
                EdgeFactory<int, Edge<int>> eFactory)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.VertexFactory.Should().BeSameAs(vFactory);
                algo.EdgeFactory.Should().BeSameAs(eFactory);
                algo.SourceToVertices.Should().BeSameAs(soToV);
                algo.VerticesToSink.Should().BeSameAs(vToSi);
                algo.MatchedEdges.Should().BeEmpty();
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            int[] sourceToVertices = { 1, 2 };
            int[] verticesToSink = { 1, 2 };

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, sourceToVertices, verticesToSink, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, default, verticesToSink, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, sourceToVertices, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, default, verticesToSink, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, sourceToVertices, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, sourceToVertices, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, sourceToVertices, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, default, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, default, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, default, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, sourceToVertices, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, sourceToVertices, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, default, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, default, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, default, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, sourceToVertices, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, sourceToVertices, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, sourceToVertices, verticesToSink, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, default, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, default, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, sourceToVertices, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, default, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, default, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, default, verticesToSink, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, sourceToVertices, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(graph, default, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MaximumBipartiteMatchingAlgorithm<int, Edge<int>>(default, default, default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void BipartiteMaxMatchSimple()
        {
            int[] integers = Enumerable.Range(0, 100).ToArray();
            string[] even = integers.Where(n => n % 2 == 0).Select(n => n.ToString()).ToArray();
            string[] odd = integers.Where(n => n % 2 != 0).Select(n => n.ToString()).ToArray();

            // Create the edges from even to odd
            IEnumerable<Edge<string>> edges = TestHelpers.CreateAllPairwiseEdges(even, odd, _edgeFactory);

            int expectedMatchSize = Math.Min(even.Length, odd.Length);
            RunBipartiteMatchAndCheck(edges, even, odd, expectedMatchSize);
        }

        [Test]
        public void BipartiteMaxMatchSimpleReversedEdges()
        {
            int[] integers = Enumerable.Range(0, 100).ToArray();
            string[] even = integers.Where(n => n % 2 == 0).Select(n => n.ToString()).ToArray();
            string[] odd = integers.Where(n => n % 2 != 0).Select(n => n.ToString()).ToArray();

            // Create the edges from odd to even
            IEnumerable<Edge<string>> edges = TestHelpers.CreateAllPairwiseEdges(odd, even, _edgeFactory);

            int expectedMatchSize = Math.Min(even.Length, odd.Length);
            RunBipartiteMatchAndCheck(edges, even, odd, expectedMatchSize);
        }

        [Test]
        public void BipartiteMaxMatchTwoFullyConnectedSets()
        {
            var setA = new List<string>();
            var setB = new List<string>();

            const int nodesInSet1 = 100;
            const int nodesInSet2 = 10;

            // Create a set of vertices in each set which all match each other
            int[] integers = Enumerable.Range(0, nodesInSet1).ToArray();
            string[] even = integers.Where(n => n % 2 == 0).Select(n => n.ToString()).ToArray();
            string[] odd = integers.Where(n => n % 2 != 0).Select(n => n.ToString()).ToArray();
            var edges = TestHelpers.CreateAllPairwiseEdges(even, odd, _edgeFactory).ToList();

            setA.AddRange(even);
            setB.AddRange(odd);

            // Create another set of vertices in each set which all match each other
            integers = Enumerable.Range(nodesInSet1 + 1, nodesInSet2).ToArray();
            even = integers.Where(n => n % 2 == 0).Select(n => n.ToString()).ToArray();
            odd = integers.Where(n => n % 2 != 0).Select(n => n.ToString()).ToArray();
            edges.AddRange(TestHelpers.CreateAllPairwiseEdges(even, odd, _edgeFactory));

            setA.AddRange(even);
            setB.AddRange(odd);

            int expectedMatchSize = Math.Min(setA.Count, setB.Count);
            RunBipartiteMatchAndCheck(edges, setA, setB, expectedMatchSize);
        }

        [Test]
        public void BipartiteMaxMatchUnequalPartitionsTest()
        {
            var setA = new List<string>();
            var setB = new List<string>();

            // Create a bipartite graph with small and large vertex partitions
            const int smallerSetSize = 1;
            const int largerSetSize = 1000;

            // Create a set of vertices in each set which all match each other
            string[] leftNodes = Enumerable.Range(0, smallerSetSize).Select(n => $"L{n}").ToArray();
            string[] rightNodes = Enumerable.Range(0, largerSetSize).Select(n => $"R{n}").ToArray();
            IEnumerable<Edge<string>> edges = TestHelpers.CreateAllPairwiseEdges(leftNodes, rightNodes, _edgeFactory);

            setA.AddRange(leftNodes);
            setB.AddRange(rightNodes);

            int expectedMatchSize = Math.Min(setA.Count, setB.Count);
            RunBipartiteMatchAndCheck(edges, setA, setB, expectedMatchSize);
        }
    }
}
