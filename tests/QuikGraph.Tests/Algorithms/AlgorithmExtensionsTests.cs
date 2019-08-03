using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="AlgorithmExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class AlgorithmExtensionsTests
    {
        #region Helpers

        private static void Roots<T>([NotNull] IVertexAndEdgeListGraph<T, Edge<T>> graph)
        {
            var roots = new HashSet<T>(graph.Roots());
            foreach (Edge<T> edge in graph.Edges)
                Assert.IsFalse(roots.Contains(edge.Target));
        }

        #endregion

        [Test]
        public void AdjacencyGraphRoots()
        {
            var graph = new AdjacencyGraph<string, Edge<string>>();
            graph.AddVertex("A");
            graph.AddVertex("B");
            graph.AddVertex("C");

            graph.AddEdge(new Edge<string>("A", "B"));
            graph.AddEdge(new Edge<string>("B", "C"));

            string[] roots = graph.Roots().ToArray();
            Assert.AreEqual(1, roots.Length);
            Assert.AreEqual("A", roots[0]);
        }

        [Test]
        public void AllAdjacencyGraphRoots()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                Roots(graph);
        }
    }
}
