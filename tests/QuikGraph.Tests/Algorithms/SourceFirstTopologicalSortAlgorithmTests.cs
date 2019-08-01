using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.TopologicalSort;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="SourceFirstTopologicalSortAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class SourceFirstTopologicalSortAlgorithmTests : QuikGraphUnitTests
    {
        #region Helpers

        private static void Sort<TVertex, TEdge>([NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new SourceFirstTopologicalSortAlgorithm<TVertex, TEdge>(graph);
            try
            {
                algorithm.Compute();
            }
            catch (NonAcyclicGraphException)
            {
            }
        }

        #endregion

        [Test]
        public void SortAll()
        {
            foreach(AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                Sort(graph);
        }

        [Test]
        public void SortAnotherOne()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();

            graph.AddVertexRange(new[] { 0, 1, 2, 3, 4 });
            graph.AddEdge(new Edge<int>(0, 1));
            graph.AddEdge(new Edge<int>(1, 2));
            graph.AddEdge(new Edge<int>(1, 3));
            graph.AddEdge(new Edge<int>(2, 3));
            graph.AddEdge(new Edge<int>(3, 4));

            var algorithm = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();
        }

        [Test]
        public void Sort_DCT8()
        {
            BidirectionalGraph<string, Edge<string>> graph = TestGraphFactory.LoadBidirectionalGraph(GetGraphFilePath("DCT8.graphml"));

            var algorithm = new SourceFirstTopologicalSortAlgorithm<string, Edge<string>>(graph);
            algorithm.Compute();
        }
    }
}
