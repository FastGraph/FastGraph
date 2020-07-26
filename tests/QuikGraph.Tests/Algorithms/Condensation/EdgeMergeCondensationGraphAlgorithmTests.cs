using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Condensation;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Tests for <see cref="EdgeMergeCondensationGraphAlgorithm{TVertex, TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class EdgeMergeCondensationGraphAlgorithmTests
    {
        #region Test helpers

        private static void RunEdgesCondensationAndCheck<TVertex, TEdge>(
            [NotNull] IBidirectionalGraph<TVertex, TEdge> graph,
            [NotNull] VertexPredicate<TVertex> predicate)
            where TEdge : IEdge<TVertex>
        {
            IMutableBidirectionalGraph<TVertex, MergedEdge<TVertex, TEdge>> condensedGraph =
                graph.CondensateEdges(predicate);

            Assert.IsNotNull(condensedGraph);
            Assert.LessOrEqual(condensedGraph.VertexCount, graph.VertexCount);

            TVertex[] vertices = condensedGraph.Vertices.ToArray();
            foreach (MergedEdge<TVertex, TEdge> edge in condensedGraph.Edges)
            {
                Assert.Contains(edge.Source, vertices);
                Assert.Contains(edge.Target, vertices);

                Assert.Positive(edge.Edges.Count);
                Assert.Contains(edge.Edges.First().Source, vertices);
                Assert.Contains(edge.Edges.Last().Target, vertices);
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            VertexPredicate<int> vertexPredicate = vertex => true;
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
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.AreSame(predicate, algo.VertexPredicate);
                Assert.AreSame(cg, algo.CondensedGraph);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            VertexPredicate<int> vertexPredicate = vertex => true;
            var graph = new BidirectionalGraph<int, Edge<int>>();
            var condensedGraph = new BidirectionalGraph<int, MergedEdge<int, Edge<int>>>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(graph, condensedGraph, null));
            Assert.Throws<ArgumentNullException>(
                () => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(graph, null, vertexPredicate));
            Assert.Throws<ArgumentNullException>(
                () => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(null, condensedGraph, vertexPredicate));
            Assert.Throws<ArgumentNullException>(
                () => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(graph, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(null, condensedGraph, null));
            Assert.Throws<ArgumentNullException>(
                () => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(null, null, vertexPredicate));
            Assert.Throws<ArgumentNullException>(
                () => new EdgeMergeCondensationGraphAlgorithm<int, Edge<int>>(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [NotNull, ItemNotNull]
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
        public void EdgeCondensationAllVertices([NotNull] IBidirectionalGraph<int, Edge<int>> graph)
        {
            IMutableBidirectionalGraph<int, MergedEdge<int, Edge<int>>> condensedGraph =
                graph.CondensateEdges(v => true);

            Assert.IsNotNull(condensedGraph);
            Assert.AreEqual(graph.VertexCount, condensedGraph.VertexCount);
            Assert.AreEqual(graph.EdgeCount, condensedGraph.EdgeCount);
            CollectionAssert.AreEquivalent(graph.Vertices, condensedGraph.Vertices);
            CollectionAssert.AreEquivalent(graph.Edges, condensedGraph.Edges.SelectMany(e => e.Edges));
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

            Assert.IsNotNull(condensedGraph);
            Assert.AreEqual(2, condensedGraph.VertexCount);
            Assert.AreEqual(6, condensedGraph.EdgeCount);
            CollectionAssert.AreEquivalent(new[] { 4, 8 }, condensedGraph.Vertices);
            CollectionAssert.AreEquivalent(new[] { edge82, edge23, edge38 }, condensedGraph.Edges.ElementAt(0).Edges);
            CollectionAssert.AreEquivalent(new[] { edge44 }, condensedGraph.Edges.ElementAt(1).Edges);
            CollectionAssert.AreEquivalent(new[] { edge43, edge38 }, condensedGraph.Edges.ElementAt(2).Edges);
            CollectionAssert.AreEquivalent(new[] { edge42, edge23, edge38 }, condensedGraph.Edges.ElementAt(3).Edges);
            CollectionAssert.AreEquivalent(new[] { edge45, edge57, edge71, edge13, edge38 }, condensedGraph.Edges.ElementAt(4).Edges);
            CollectionAssert.AreEquivalent(new[] { edge45, edge57, edge71, edge12, edge23, edge38 }, condensedGraph.Edges.ElementAt(5).Edges);
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void EdgeCondensation()
        {
            var rand = new Random(123456);
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_SlowTests())
            {
                RunEdgesCondensationAndCheck(graph, v => true);
                RunEdgesCondensationAndCheck(graph, v => rand.Next(0, 1) == 1);
            }
        }
    }
}