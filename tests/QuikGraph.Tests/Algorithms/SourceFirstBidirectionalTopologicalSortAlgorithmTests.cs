using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.TopologicalSort;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="SourceFirstBidirectionalTopologicalSortAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class SourceFirstBidirectionalTopologicalSortAlgorithmTests : QuikGraphUnitTests
    {
        #region Helpers

        private static void Sort<TVertex, TEdge>([NotNull] IBidirectionalGraph<TVertex, TEdge> graph, TopologicalSortDirection direction)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<TVertex, TEdge>(graph, direction);
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
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs())
            {
                Sort(graph, TopologicalSortDirection.Forward);
                Sort(graph, TopologicalSortDirection.Backward);
            }
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

            var algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Forward);
            algorithm.Compute();

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<int, Edge<int>>(graph, TopologicalSortDirection.Backward);
            algorithm.Compute();
        }

        [Test]
        public void Sort_DCT8()
        {
            BidirectionalGraph<string, Edge<string>> graph = TestGraphFactory.LoadBidirectionalGraph(GetGraphFilePath("DCT8.graphml"));

            var algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<string, Edge<string>>(graph, TopologicalSortDirection.Forward);
            algorithm.Compute();

            algorithm = new SourceFirstBidirectionalTopologicalSortAlgorithm<string, Edge<string>>(graph, TopologicalSortDirection.Backward);
            algorithm.Compute();
        }
    }
}
