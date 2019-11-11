using System;
using System.Linq;
using NUnit.Framework;
using QuikGraph.Algorithms.ShortestPath;

namespace QuikGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for <see cref="YenShortestPathsAlgorithm{TVertex}"/>.
    /// </summary>
    [TestFixture]
    internal class YenShortestPathsAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            Func<EquatableTaggedEdge<int, double>, double> Weights = e => 1.0;

            var graph = new AdjacencyGraph<int, EquatableTaggedEdge<int, double>>();
            // ReSharper disable ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new YenShortestPathsAlgorithm<int>(graph, 1, 2, int.MaxValue));
            Assert.DoesNotThrow(() => new YenShortestPathsAlgorithm<int>(graph, 1, 2, 10));

            Assert.DoesNotThrow(() => new YenShortestPathsAlgorithm<int>(graph, 1, 2, int.MaxValue, Weights, paths => paths.Where(path => path.Count() > 2)));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new AdjacencyGraph<TestVertex, EquatableTaggedEdge<TestVertex, double>>();
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");

            Assert.Throws<ArgumentNullException>(
                () => new YenShortestPathsAlgorithm<TestVertex>(null, vertex1, vertex2, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => new YenShortestPathsAlgorithm<TestVertex>(graph, null, vertex2, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => new YenShortestPathsAlgorithm<TestVertex>(graph, vertex1, null, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => new YenShortestPathsAlgorithm<TestVertex>(null, null, vertex2, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => new YenShortestPathsAlgorithm<TestVertex>(null, vertex1, null, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => new YenShortestPathsAlgorithm<TestVertex>(graph, null, null, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => new YenShortestPathsAlgorithm<TestVertex>(null, null, null, int.MaxValue));
            // ReSharper restore AssignNullToNotNullAttribute

            Assert.Throws<ArgumentOutOfRangeException>(
                () => new YenShortestPathsAlgorithm<TestVertex>(graph, vertex1, vertex2, 0));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new YenShortestPathsAlgorithm<TestVertex>(graph, vertex1, vertex2, -1));
            // ReSharper restore ObjectCreationAsStatement
        }

        /// <summary>
        /// Attempt to use non existing vertices.
        /// </summary>
        [Test]
        public void YenEmptyGraph()
        {
            var graph = new AdjacencyGraph<char, EquatableTaggedEdge<char, double>>(true);

            var algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '5', 10);
            Assert.Throws<NoPathFoundException>(() => algorithm.Execute());
        }

        /// <summary>
        /// Attempt to use for graph that only have one vertex.
        /// Expecting that Dijkstra’s algorithm couldn't find any ways.
        /// </summary>
        [Test]
        public void YenOneVertex()
        {
            var graph = new AdjacencyGraph<char, EquatableTaggedEdge<char, double>>(true);
            graph.AddVertexRange("1");

            var algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '1', 10);
            Assert.Throws<NoPathFoundException>(() => algorithm.Execute());
        }

        /// <summary>
        /// Attempt to use for loop graph.
        /// Expecting that Dijkstra’s algorithm couldn't find any ways.
        /// </summary>
        [Test]
        public void YenLoop()
        {
            var graph = new AdjacencyGraph<char, EquatableTaggedEdge<char, double>>(true);
            graph.AddVertexRange("1");

            var algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '1', 10);
            graph.AddEdge(new EquatableTaggedEdge<char, double>('1', '1', 7));
            Assert.Throws<NoPathFoundException>(() => algorithm.Execute());
        }

        [Test]
        public void YenNormal()
        {
            AdjacencyGraph<char, EquatableTaggedEdge<char, double>> graph = GenerateGraph();

            // Default weight function and default filter function case
            var algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '5', 10);
            RunYenAndCheck(algorithm, graph);

            // Custom weight function and default filter function case
            algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '5', 10, e => e.Tag);
            RunYenAndCheck(algorithm, graph);

            // Default weight function and custom filter function case
            algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '5', 10, null, e => e);
            RunYenAndCheck(algorithm, graph);

            // Custom weight function and custom filter function case
            algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '5', 10, e => e.Tag, e => e);
            RunYenAndCheck(algorithm, graph);

            #region Local functions

            AdjacencyGraph<char, EquatableTaggedEdge<char, double>> GenerateGraph()
            {
                var g = new AdjacencyGraph<char, EquatableTaggedEdge<char, double>>(true);

                g.AddVertexRange("123456");
                g.AddEdge(new EquatableTaggedEdge<char, double>('1', '2', 7));  // 0
                g.AddEdge(new EquatableTaggedEdge<char, double>('1', '3', 9));  // 1
                g.AddEdge(new EquatableTaggedEdge<char, double>('1', '6', 14)); // 2
                g.AddEdge(new EquatableTaggedEdge<char, double>('2', '3', 10)); // 3
                g.AddEdge(new EquatableTaggedEdge<char, double>('2', '4', 15)); // 4
                g.AddEdge(new EquatableTaggedEdge<char, double>('3', '4', 11)); // 5
                g.AddEdge(new EquatableTaggedEdge<char, double>('3', '6', 2));  // 6
                g.AddEdge(new EquatableTaggedEdge<char, double>('4', '5', 6));  // 7
                g.AddEdge(new EquatableTaggedEdge<char, double>('5', '6', 9));  // 8

                return g;
            }

            void RunYenAndCheck(YenShortestPathsAlgorithm<char> yen, IEdgeListGraph<char, EquatableTaggedEdge<char, double>> g)
            {
                CollectionAssert.IsEmpty(yen.RemovedEdges());

                // Generate simple graph
                // like this https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
                // but with directed edges input graph
                YenShortestPathsAlgorithm<char>.SortedPath[] paths = yen.Execute().ToArray();

                // Expecting to get 3 paths:
                // 1 => 1-3-4-5
                // 2 => 1-2-4-5
                // 3 => 1-2-3-4-5
                // Consistently checking the result
                Assert.AreEqual(3, paths.Length);
                var edges = g.Edges.ToArray();
                // 1
                EquatableTaggedEdge<char, double>[] path0 = paths[0].ToArray();
                Assert.AreEqual(path0[0], edges[1]);
                Assert.AreEqual(path0[1], edges[5]);
                Assert.AreEqual(path0[2], edges[7]);
                // 2
                EquatableTaggedEdge<char, double>[] path1 = paths[1].ToArray();
                Assert.AreEqual(path1[0], edges[0]);
                Assert.AreEqual(path1[1], edges[4]);
                Assert.AreEqual(path1[2], edges[7]);
                // 3
                EquatableTaggedEdge<char, double>[] path2 = paths[2].ToArray();
                Assert.AreEqual(path2[0], edges[0]);
                Assert.AreEqual(path2[1], edges[3]);
                Assert.AreEqual(path2[2], edges[5]);
                Assert.AreEqual(path2[3], edges[7]);

                CollectionAssert.AreEqual(
                    new[] { edges[1], edges[4] },
                    yen.RemovedEdges());
            }

            #endregion
        }
    }
}