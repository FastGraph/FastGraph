using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.TopologicalSort;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="UndirectedFirstTopologicalSortAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class UndirectedFirstTopologicalSortAlgorithmTests
    {
        #region Helpers

        private static void Compute<TVertex, TEdge>([NotNull] IUndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new UndirectedFirstTopologicalSortAlgorithm<TVertex, TEdge>(graph)
            {
                AllowCyclicGraph = true
            };

            algorithm.Compute();
        }

        #endregion

        [Test]
        public void UndirectedFirstTopologicalSortAll()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs())
                Compute(graph);

            // TODO: Add assertions
        }
    }
}
