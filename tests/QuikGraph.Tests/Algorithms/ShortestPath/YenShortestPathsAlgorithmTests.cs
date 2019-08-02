using System.Collections.Generic;
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
        /// <summary>
        /// Attempt to use non existing vertices.
        /// </summary>
        [Test]
        public void YenZeroCaseTest()
        {
            AdjacencyGraph<char, TaggedEquatableEdge<char, double>> graph = new AdjacencyGraph<char, TaggedEquatableEdge<char, double>>(true);

            var algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '5', 10);
            Assert.Throws<KeyNotFoundException>(() => algorithm.Execute());
        }

        /// <summary>
        /// Attempt to use for graph that only have one vertex.
        /// Expecting that Dijkstra’s algorithm couldn't find any ways.
        /// </summary>
        [Test]
        public void YenOneVertexCaseTest()
        {
            var graph = new AdjacencyGraph<char, TaggedEquatableEdge<char, double>>(true);
            graph.AddVertexRange("1");

            var algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '1', 10);
            Assert.Throws<NoPathFoundException>(() => algorithm.Execute());
        }

        /// <summary>
        /// Attempt to use for loop graph.
        /// Expecting that Dijkstra’s algorithm couldn't find any ways.
        /// </summary>
        [Test]
        public void YenLoopCaseTest()
        {
            var graph = new AdjacencyGraph<char, TaggedEquatableEdge<char, double>>(true);
            graph.AddVertexRange("1");

            var algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '1', 10);
            graph.AddEdge(new TaggedEquatableEdge<char, double>('1', '1', 7));
            Assert.Throws<NoPathFoundException>(() => algorithm.Execute());
        }

        [Test]
        public void YenNormalCaseTest()
        {
            AdjacencyGraph<char, TaggedEquatableEdge<char, double>> graph = GenerateGraph();

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

            AdjacencyGraph<char, TaggedEquatableEdge<char, double>> GenerateGraph()
            {
                var g = new AdjacencyGraph<char, TaggedEquatableEdge<char, double>>(true);

                g.AddVertexRange("123456");
                g.AddEdge(new TaggedEquatableEdge<char, double>('1', '2', 7));  // 0
                g.AddEdge(new TaggedEquatableEdge<char, double>('1', '3', 9));  // 1
                g.AddEdge(new TaggedEquatableEdge<char, double>('1', '6', 14)); // 2
                g.AddEdge(new TaggedEquatableEdge<char, double>('2', '3', 10)); // 3
                g.AddEdge(new TaggedEquatableEdge<char, double>('2', '4', 15)); // 4
                g.AddEdge(new TaggedEquatableEdge<char, double>('3', '4', 11)); // 5
                g.AddEdge(new TaggedEquatableEdge<char, double>('3', '6', 2));  // 6
                g.AddEdge(new TaggedEquatableEdge<char, double>('4', '5', 6));  // 7
                g.AddEdge(new TaggedEquatableEdge<char, double>('5', '6', 9));  // 8

                return g;
            }

            void RunYenAndCheck(YenShortestPathsAlgorithm<char> yen, AdjacencyGraph<char, TaggedEquatableEdge<char, double>> g)
            {
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
                // 1
                Assert.AreEqual(paths[0].ToArray()[0], g.Edges.ToArray()[1]);
                Assert.AreEqual(paths[0].ToArray()[1], g.Edges.ToArray()[5]);
                Assert.AreEqual(paths[0].ToArray()[2], g.Edges.ToArray()[7]);
                // 2
                Assert.AreEqual(paths[1].ToArray()[0], g.Edges.ToArray()[0]);
                Assert.AreEqual(paths[1].ToArray()[1], g.Edges.ToArray()[4]);
                Assert.AreEqual(paths[1].ToArray()[2], g.Edges.ToArray()[7]);
                // 3
                Assert.AreEqual(paths[2].ToArray()[0], g.Edges.ToArray()[0]);
                Assert.AreEqual(paths[2].ToArray()[1], g.Edges.ToArray()[3]);
                Assert.AreEqual(paths[2].ToArray()[2], g.Edges.ToArray()[5]);
                Assert.AreEqual(paths[2].ToArray()[3], g.Edges.ToArray()[7]);
            }

            #endregion
        }
    }
}