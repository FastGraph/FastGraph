using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.TopologicalSort;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="TopologicalSortAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class TopologicalSortAlgorithmTests
    {
        #region Test helpers

        private static void RunTopologicalSortAndCheck<TVertex, TEdge>([NotNull] IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new TopologicalSortAlgorithm<TVertex, TEdge>(graph);
            algorithm.Compute();

            Assert.IsNotNull(algorithm.SortedVertices);
            Assert.AreEqual(graph.VertexCount, algorithm.SortedVertices.Length);
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(graph, -10);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(graph, 0);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(graph, 10);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                TopologicalSortAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.IsNull(algo.SortedVertices);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new TopologicalSortAlgorithm<int, Edge<int>>(null));
        }

        [Test]
        public void OneTwo()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(new Edge<int>(1, 2));

            var algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(graph, graph.VertexCount);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 1, 2 },
                algorithm.SortedVertices);
        }

        // Trying to see if order of vertices affects the topological sort order
        [Test]
        public void TwoOne()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();

            // Deliberately adding 1 and then 2, before adding edge (2, 1).
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(new Edge<int>(2, 1));

            var algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(graph, graph.VertexCount);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 2, 1 },
                algorithm.SortedVertices);
        }

        [Test]
        public void SimpleGraph()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(2, 6),
                new Edge<int>(2, 8),
                new Edge<int>(4, 2),
                new Edge<int>(4, 5),
                new Edge<int>(5, 6),
                new Edge<int>(7, 5),
                new Edge<int>(7, 8)
            });

            var algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 7, 4, 5, 1, 2, 8, 6, 3 },
                algorithm.SortedVertices);
        }

        [Test]
        public void ForestGraph()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(3, 4),

                new Edge<int>(5, 6)
            });

            var algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            CollectionAssert.AreEqual(
                new[] { 5, 6, 0, 1, 2, 3, 4 },
                algorithm.SortedVertices);
        }

        [Test]
        public void GraphWithSelfEdge_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(2, 2),
                new Edge<int>(3, 4)
            });

            var algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(graph);
            Assert.Throws<NonAcyclicGraphException>(() => algorithm.Compute());
        }

        [Test]
        public void TopologicalSort()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                RunTopologicalSortAndCheck(graph);
        }

        [Test]
        public void TopologicalSort_DCT8()
        {
            AdjacencyGraph<string, Edge<string>> graph = TestGraphFactory.LoadGraph(GetGraphFilePath("DCT8.graphml"));
            RunTopologicalSortAndCheck(graph);
        }

        [Test]
        public void TopologicalSort_Throws()
        {
            var cyclicGraph = new AdjacencyGraph<int, Edge<int>>();
            cyclicGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(1, 4),
                new Edge<int>(3, 1)
            });

            var algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(cyclicGraph);
            Assert.Throws<NonAcyclicGraphException>(() => algorithm.Compute());
        }

        #region Test classes

        private class Letter
        {
            private readonly char _char;

            public Letter(char letter)
            {
                _char = letter;
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return _char.ToString();
            }
        }

        #endregion

        [Test]
        public void FacebookSeattleWordPuzzle()
        {
            /* A puzzle from Facebook Seattle opening party:
            http://www.facebook.com/note.php?note_id=146727365346299
            You are given a list of relationships between the letters in a single word, all of which are in the form: 
            "The first occurrence of A comes before N occurrences of B." 
            You can safely assume that you have all such relationships except for any in which N would be 0. 
            Determine the original word, then go to http://www.facebook.com/seattle/[insert-word-here] to find the second part of the puzzle.

            The first occurrence of 'e' comes before 1 occurrence of 's'.
            The first occurrence of 'i' comes before 1 occurrence of 'n'.
            The first occurrence of 'i' comes before 1 occurrence of 'i'.
            The first occurrence of 'n' comes before 2 occurrences of 'e'.
            The first occurrence of 'e' comes before 1 occurrence of 'e'.
            The first occurrence of 'i' comes before 1 occurrence of 'v'.
            The first occurrence of 'n' comes before 1 occurrence of 'i'.
            The first occurrence of 'n' comes before 1 occurrence of 'v'.
            The first occurrence of 'i' comes before 1 occurrence of 's'.
            The first occurrence of 't' comes before 1 occurrence of 's'.
            The first occurrence of 'v' comes before 1 occurrence of 's'.
            The first occurrence of 'v' comes before 2 occurrences of 'e'.
            The first occurrence of 't' comes before 2 occurrences of 'e'.
            The first occurrence of 'i' comes before 2 occurrences of 'e'.
            The first occurrence of 'v' comes before 1 occurrence of 't'.
            The first occurrence of 'n' comes before 1 occurrence of 't'.
            The first occurrence of 'v' comes before 1 occurrence of 'i'.
            The first occurrence of 'i' comes before 1 occurrence of 't'.
            The first occurrence of 'n' comes before 1 occurrence of 's'.
            */

            var graph = new AdjacencyGraph<Letter, Edge<Letter>>();

            // A more generalized algorithm would handle duplicate letters automatically.
            // This is the quick and dirty solution.
            var i1 = new Letter('i');
            var i2 = new Letter('i');
            var e1 = new Letter('e');
            var e2 = new Letter('e');

            var s = new Letter('s');
            var n = new Letter('n');
            var t = new Letter('t');
            var v = new Letter('v');

            graph.AddVertexRange(new List<Letter> { e1, e2, s, i1, i2, n, t, v });

            graph.AddEdge(new Edge<Letter>(e1, s));
            graph.AddEdge(new Edge<Letter>(i1, n));
            graph.AddEdge(new Edge<Letter>(i1, i2));
            graph.AddEdge(new Edge<Letter>(n, e1));
            graph.AddEdge(new Edge<Letter>(n, e2));
            graph.AddEdge(new Edge<Letter>(e1, e2));
            graph.AddEdge(new Edge<Letter>(i1, v));
            graph.AddEdge(new Edge<Letter>(n, e1));
            graph.AddEdge(new Edge<Letter>(n, v));
            graph.AddEdge(new Edge<Letter>(i1, s));
            graph.AddEdge(new Edge<Letter>(t, s));
            graph.AddEdge(new Edge<Letter>(v, s));
            graph.AddEdge(new Edge<Letter>(v, e1));
            graph.AddEdge(new Edge<Letter>(v, e2));
            graph.AddEdge(new Edge<Letter>(t, e1));
            graph.AddEdge(new Edge<Letter>(t, e2));
            graph.AddEdge(new Edge<Letter>(i1, e1));
            graph.AddEdge(new Edge<Letter>(i1, e2));
            graph.AddEdge(new Edge<Letter>(v, t));
            graph.AddEdge(new Edge<Letter>(n, t));
            graph.AddEdge(new Edge<Letter>(v, i2));
            graph.AddEdge(new Edge<Letter>(i1, t));
            graph.AddEdge(new Edge<Letter>(n, s));

            var sort = new TopologicalSortAlgorithm<Letter, Edge<Letter>>(graph);
            sort.Compute();

            var builder = new StringBuilder();
            foreach (Letter item in sort.SortedVertices)
            {
                builder.Append(item);
            }
            string word = builder.ToString();

            Assert.AreEqual("invitees", word);
        }
    }
}