using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Tests for <see cref="ClusteredAdjacencyGraph{TVertex,TEdge}"/>s.
    /// </summary>
    [TestFixture]
    internal class ClusteredGraphTests
    {
        #region Helpers

        [Pure]
        public bool ContainsVertexParent([NotNull] ClusteredAdjacencyGraph<int, IEdge<int>> cluster, int vertex)
        {
            return cluster.ContainsVertex(vertex)
                   && cluster.Parent != null 
                   && ContainsVertexParent(cluster.Parent, vertex)
                   || cluster.Parent is null;
        }

        [Pure]
        public bool ContainsEdgeParent([NotNull] ClusteredAdjacencyGraph<int, IEdge<int>> cluster, IEdge<int> edge)
        {
            return cluster.ContainsEdge(edge) 
                   && cluster.Parent != null 
                   && ContainsEdgeParent(cluster.Parent, edge)
                   || cluster.Parent is null;
        }

        #endregion

        [Test]
        public void AddingClusterVertex()
        {
            var graph = new AdjacencyGraph<int, IEdge<int>>();
            var clusteredGraph = new ClusteredAdjacencyGraph<int, IEdge<int>>(graph);
            ClusteredAdjacencyGraph<int, IEdge<int>> cluster = clusteredGraph.AddCluster();
            cluster.AddVertex(5);

            Assert.IsTrue(ContainsVertexParent(clusteredGraph, 5));
        }

        [Test]
        public void AddingClusterEdge()
        {
            var graph = new AdjacencyGraph<int, IEdge<int>>();
            var clusteredGraph = new ClusteredAdjacencyGraph<int, IEdge<int>>(graph);
            ClusteredAdjacencyGraph<int, IEdge<int>> cluster1 = clusteredGraph.AddCluster();
            cluster1.AddVertex(5);
            cluster1.AddVertex(6);
            var edge = new TaggedEdge<int, int>(5, 6, 1);
            cluster1.AddEdge(edge);

            Assert.IsTrue(ContainsEdgeParent(clusteredGraph, edge));
        }

        [Test]
        public void RemovingClusterVertex()
        {
            var graph = new AdjacencyGraph<int, IEdge<int>>();
            var clusteredGraph = new ClusteredAdjacencyGraph<int, IEdge<int>>(graph);
            ClusteredAdjacencyGraph<int, IEdge<int>> cluster1 = clusteredGraph.AddCluster();
            ClusteredAdjacencyGraph<int, IEdge<int>> cluster2 = cluster1.AddCluster();
            ClusteredAdjacencyGraph<int, IEdge<int>> cluster3 = cluster2.AddCluster();
            cluster3.AddVertex(5);
            cluster2.RemoveVertex(5);

            Assert.IsFalse(ContainsVertexParent(cluster3, 5));
        }

        [Test]
        public void RemovingClusterEdge()
        {
            var graph = new AdjacencyGraph<int, IEdge<int>>();
            var clusteredGraph = new ClusteredAdjacencyGraph<int, IEdge<int>>(graph);
            ClusteredAdjacencyGraph<int, IEdge<int>> cluster1 = clusteredGraph.AddCluster();
            ClusteredAdjacencyGraph<int, IEdge<int>> cluster2 = cluster1.AddCluster();
            ClusteredAdjacencyGraph<int, IEdge<int>> cluster3 = cluster2.AddCluster();
            cluster3.AddVertex(5);
            cluster3.AddVertex(6);

            var edge = new TaggedEdge<int, int>(5, 6, 1);
            cluster1.RemoveEdge(edge);

            Assert.IsFalse(ContainsEdgeParent(cluster2, edge));
        }
    }
}

