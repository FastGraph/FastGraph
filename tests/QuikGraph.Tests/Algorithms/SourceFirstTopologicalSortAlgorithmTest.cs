using NUnit.Framework;
using QuickGraph.Algorithms.TopologicalSort;
using QuickGraph.Serialization;
using QuikGraph.Tests;

namespace QuickGraph.Algorithms
{
    [TestFixture]
    internal class SourceFirstTopologicalSortAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        public void SortAll()
        {
            foreach(var g in TestGraphFactory.GetAdjacencyGraphs())
                this.Sort(g);
        }

        public void Sort<TVertex, TEdge>(IVertexAndEdgeListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var topo = new SourceFirstTopologicalSortAlgorithm<TVertex, TEdge>(g);
            try
            {
                topo.Compute();
            }
            catch (NonAcyclicGraphException)
            { }
        }

        [Test]
        public void SortAnotherOne()
        {
            var g = new BidirectionalGraph<int, Edge<int>>();

            g.AddVertexRange(new int[5] { 0, 1, 2, 3, 4 });
            g.AddEdge(new Edge<int>(0, 1));
            g.AddEdge(new Edge<int>(1, 2));
            g.AddEdge(new Edge<int>(1, 3));
            g.AddEdge(new Edge<int>(2, 3));
            g.AddEdge(new Edge<int>(3, 4));

            var topo = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(g);
            topo.Compute();
        }

        [Test]
        public void SortDCT()
        {
            var g = TestGraphFactory.LoadBidirectionalGraph(GetGraphFilePath("DCT8.graphml"));

            var topo = new SourceFirstTopologicalSortAlgorithm<string, Edge<string>>(g);
            topo.Compute();
        }
    }
}
