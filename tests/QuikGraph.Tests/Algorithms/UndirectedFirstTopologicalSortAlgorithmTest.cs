using NUnit.Framework;
using QuickGraph.Algorithms.TopologicalSort;
using QuickGraph.Serialization;
using QuikGraph.Tests;

namespace QuickGraph.Algorithms
{
    [TestFixture]
    internal partial class UndirectedFirstTopologicalSortAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        public void UndirectedFirstTopologicalSortAll()
        {
            foreach (var g in TestGraphFactory.GetUndirectedGraphs())
                this.Compute(g);
        }

        public void Compute<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var topo =
                new UndirectedFirstTopologicalSortAlgorithm<TVertex, TEdge>(g);
            topo.AllowCyclicGraph = true;
            topo.Compute();
        }
    }
}
