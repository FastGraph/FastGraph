using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.TopologicalSort;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="TopologicalSortAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class TopologicalSortAlgorithmTests
    {
        #region Helpers

        private static void SortCyclic<TVertex, TEdge>([NotNull] IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new TopologicalSortAlgorithm<TVertex, TEdge>(graph);
            algorithm.Compute();
        }

        #endregion

        [Test]
        public void TopologicalSortAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                SortCyclic(graph);
        }

        [Test]
        public void TopologicalSort_DCT8()
        {
            AdjacencyGraph<string, Edge<string>> graph = TestGraphFactory.LoadGraph(GetGraphFilePath("DCT8.graphml"));
            var topologicalSort = new TopologicalSortAlgorithm<string, Edge<string>>(graph);
            topologicalSort.Compute();
        }

        [Test]
        public void OneTwo()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddEdge(new Edge<int>(1, 2));

            var algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(graph);
            var vertices = new List<int>(graph.VertexCount);
            algorithm.Compute(vertices);

            Assert.AreEqual(2, vertices.Count);
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

            var algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(graph);
            var vertices = new List<int>(graph.VertexCount);
            algorithm.Compute(vertices);

            Assert.AreEqual(2, vertices.Count);
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
