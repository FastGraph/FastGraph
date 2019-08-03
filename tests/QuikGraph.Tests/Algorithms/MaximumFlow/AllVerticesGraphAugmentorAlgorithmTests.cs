using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.MaximumFlow;

namespace QuikGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="AllVerticesGraphAugmentorAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class AllVerticesGraphAugmentorAlgorithmTests
    {
        #region Helpers

        private static void AugmentAndCheck(
            [NotNull] IMutableVertexAndEdgeListGraph<string, Edge<string>> graph)
        {
            int vertexCount = graph.VertexCount;
            int edgeCount = graph.EdgeCount;
            int vertexId = graph.VertexCount + 1;
            using (var augmentor = new AllVerticesGraphAugmentorAlgorithm<string, Edge<string>>(
                graph,
                () => (vertexId++).ToString(),
                (s, t) => new Edge<string>(s, t)))
            {
                augmentor.Compute();
                VerifyCount(graph, augmentor, vertexCount);
                VerifySourceConnector(graph, augmentor);
                VerifySinkConnector(graph, augmentor);
            }

            Assert.AreEqual(graph.VertexCount, vertexCount);
            Assert.AreEqual(graph.EdgeCount, edgeCount);
        }

        private static void VerifyCount<TVertex, TEdge>(
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph,
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            [NotNull] AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
            int vertexCount)
            where TEdge : IEdge<TVertex>
        {
            Assert.AreEqual(vertexCount + 2, graph.VertexCount);
            Assert.IsTrue(graph.ContainsVertex(augmentor.SuperSource));
            Assert.IsTrue(graph.ContainsVertex(augmentor.SuperSink));
        }

        private static void VerifySourceConnector<TVertex, TEdge>(
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex vertex in graph.Vertices)
            {
                if (vertex.Equals(augmentor.SuperSource))
                    continue;
                if (vertex.Equals(augmentor.SuperSink))
                    continue;
                Assert.IsTrue(graph.ContainsEdge(augmentor.SuperSource, vertex));
            }
        }

        private static void VerifySinkConnector<TVertex, TEdge>(
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex vertex in graph.Vertices)
            {
                if (vertex.Equals(augmentor.SuperSink))
                    continue;
                if (vertex.Equals(augmentor.SuperSink))
                    continue;
                Assert.IsTrue(graph.ContainsEdge(vertex, augmentor.SuperSink));
            }
        }

        #endregion

        [Test]
        public void AugmentAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
                AugmentAndCheck(graph);
        }
    }
}
