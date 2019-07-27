using NUnit.Framework;
using QuikGraph.Algorithms.TopologicalSort;
using QuikGraph.Serialization;
using QuikGraph.Tests;

namespace QuikGraph.Algorithms
{
    [TestFixture]
    internal partial class UndirectedFirstTopologicalSortAlgorithmTests : QuikGraphUnitTests
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
