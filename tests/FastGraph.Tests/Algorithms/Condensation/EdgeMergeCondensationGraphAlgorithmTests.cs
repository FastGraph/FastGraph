#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.Condensation;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Tests for <see cref="EdgeMergeCondensationGraphAlgorithm{TVertex, TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class EdgeMergeCondensationGraphAlgorithmTests
    {
        #region Test helpers

        private static void RunEdgesCondensationAndCheck<TVertex, TEdge>(
            IBidirectionalGraph<TVertex, TEdge> graph,
            VertexPredicate<TVertex> predicate)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            IMutableBidirectionalGraph<TVertex, MergedEdge<TVertex, TEdge>> condensedGraph =
                graph.CondensateEdges(predicate);

            condensedGraph.Should().NotBeNull();
            condensedGraph.VertexCount.Should().BeLessThanOrEqualTo(graph.VertexCount);

            TVertex[] vertices = condensedGraph.Vertices.ToArray();
            foreach (MergedEdge<TVertex, TEdge> edge in condensedGraph.Edges)
            {
                vertices.Should().Contain(edge.Source);
                vertices.Should().Contain(edge.Target);

                edge.Edges.Count.Should().BePositive();
                vertices.Should().Contain(edge.Edges.First().Source);
                vertices.Should().Contain(edge.Edges.Last().Target);
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            VertexPredicate<int> vertexPredicate = _ => true;
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var condensedGraph = new BidirectionalGraph<int, MergedEdge<int, Edge<int>>>();
            var algorithm = new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(graph, condensedGraph, vertexPredicate);
            AssertAlgorithmProperties(algorithm, graph, condensedGraph, vertexPredicate);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                EdgeMergeCondensationGraphAlgorithm<TVertex, TEdge> algo,
                IBidirectionalGraph<TVertex, TEdge> g,
                IMutableBidirectionalGraph<TVertex, MergedEdge<TVertex, TEdge>> cg,
                VertexPredicate<TVertex> predicate)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.VertexPredicate.Should().BeSameAs(predicate);
                algo.CondensedGraph.Should().BeSameAs(cg);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            VertexPredicate<int> vertexPredicate = _ => true;
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var condensedGraph = new BidirectionalGraph<int, MergedEdge<int, Edge<int>>>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(graph, condensedGraph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(graph, default, vertexPredicate)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(default, condensedGraph, vertexPredicate)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(default, condensedGraph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(default, default, vertexPredicate)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        private static IEnumerable<TestCaseData> EdgeCondensationAllVerticesTestCases
        {
            [UsedImplicitly]
            get
            {
                var edge12 = new Edge<int>(1, 2);
                var edge13 = new Edge<int>(1, 3);
                var edge23 = new Edge<int>(2, 3);
                var edge42 = new Edge<int>(4, 2);
                var edge43 = new Edge<int>(4, 3);

                var edge45 = new Edge<int>(4, 5);

                var edge56 = new Edge<int>(5, 6);
                var edge57 = new Edge<int>(5, 7);
                var edge76 = new Edge<int>(7, 6);

                var edge71 = new Edge<int>(7, 1);

                var edge89 = new Edge<int>(8, 9);

                var edge82 = new Edge<int>(8, 2);

                var graph1 = new BidirectionalGraph<int, Edge<int>>();
                graph1.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge13, edge23, edge42, edge43, edge45,
                    edge56, edge57, edge76, edge71, edge89, edge82
                });

                yield return new TestCaseData(graph1);

                var graph2 = new BidirectionalGraph<int, Edge<int>>();
                graph2.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge13, edge23, edge42, edge43,
                    edge56, edge57, edge76, edge89
                });

                yield return new TestCaseData(graph2);
            }
        }

        [TestCaseSource(nameof(EdgeCondensationAllVerticesTestCases))]
        public void EdgeCondensationAllVertices(IBidirectionalGraph<int, Edge<int>> graph)
        {
            IMutableBidirectionalGraph<int, MergedEdge<int, Edge<int>>> condensedGraph =
                graph.CondensateEdges(_ => true);

            condensedGraph.Should().NotBeNull();
            condensedGraph.VertexCount.Should().Be(graph.VertexCount);
            condensedGraph.EdgeCount.Should().Be(graph.EdgeCount);
            condensedGraph.Vertices.Should().BeEquivalentTo(graph.Vertices);
            condensedGraph.Edges.SelectMany(e => e.Edges).Should().BeEquivalentTo(graph.Edges);
        }

        [Test]
        public void EdgeCondensationSomeVertices()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge38 = new Edge<int>(3, 8);
            var edge42 = new Edge<int>(4, 2);
            var edge43 = new Edge<int>(4, 3);
            var edge44 = new Edge<int>(4, 4);

            var edge45 = new Edge<int>(4, 5);

            var edge56 = new Edge<int>(5, 6);
            var edge57 = new Edge<int>(5, 7);
            var edge76 = new Edge<int>(7, 6);

            var edge71 = new Edge<int>(7, 1);

            var edge89 = new Edge<int>(8, 9);

            var edge82 = new Edge<int>(8, 2);

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge38, edge42, edge43, edge44,
                edge45, edge56, edge57, edge76, edge71, edge89, edge82
            });

            IMutableBidirectionalGraph<int, MergedEdge<int, Edge<int>>> condensedGraph =
                graph.CondensateEdges(v => v == 4 || v == 8);

            condensedGraph.Should().NotBeNull();
            condensedGraph.VertexCount.Should().Be(2);
            condensedGraph.EdgeCount.Should().Be(6);
            condensedGraph.Vertices.Should().BeEquivalentTo(new[] { 4, 8 });
            condensedGraph.Edges.ElementAt(0).Edges.Should().BeEquivalentTo(new[] { edge82, edge23, edge38 });
            condensedGraph.Edges.ElementAt(1).Edges.Should().BeEquivalentTo(new[] { edge44 });
            condensedGraph.Edges.ElementAt(2).Edges.Should().BeEquivalentTo(new[] { edge43, edge38 });
            condensedGraph.Edges.ElementAt(3).Edges.Should().BeEquivalentTo(new[] { edge42, edge23, edge38 });
            condensedGraph.Edges.ElementAt(4).Edges.Should().BeEquivalentTo(new[] { edge45, edge57, edge71, edge13, edge38 });
            condensedGraph.Edges.ElementAt(5).Edges.Should().BeEquivalentTo(new[] { edge45, edge57, edge71, edge12, edge23, edge38 });
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void EdgeCondensation()
        {
            var rand = new Random(123456);
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_SlowTests())
            {
                RunEdgesCondensationAndCheck(graph, _ => true);
                RunEdgesCondensationAndCheck(graph, _ => rand.Next(0, 1) == 1);
            }
        }
    }
}
