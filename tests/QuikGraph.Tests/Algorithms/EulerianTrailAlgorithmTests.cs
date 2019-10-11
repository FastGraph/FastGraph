using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="EulerianTrailAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class EulerianTrailAlgorithmTests
    {
        #region Helpers

        private static void ComputeTrail<TVertex, TEdge>(
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull, InstantHandle] Func<TVertex, TVertex, TEdge> edgeFactory)
            where TEdge : IEdge<TVertex>
        {
            if (graph.VertexCount == 0)
                return;

            int circuitCount = EulerianTrailAlgorithm<TVertex, TEdge>.ComputeEulerianPathCount(graph);
            if (circuitCount == 0)
                return;

            var algorithm = new EulerianTrailAlgorithm<TVertex, TEdge>(graph);
            algorithm.AddTemporaryEdges((s, t) => edgeFactory(s, t));
            algorithm.Compute();
            var trails = algorithm.Trails();
            algorithm.RemoveTemporaryEdges();

            // Lets make sure all the edges are in the trail
            var edgeColors = new Dictionary<TEdge, GraphColor>(graph.EdgeCount);
            foreach (TEdge edge in graph.Edges)
                edgeColors.Add(edge, GraphColor.White);

            foreach (ICollection<TEdge> trail in trails)
            {
                foreach (TEdge edge in trail)
                    Assert.IsTrue(edgeColors.ContainsKey(edge));
            }
        }

        #endregion

        [Test]
        public void EulerianTrailAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_TMP())
            {
                ComputeTrail(graph, (s, t) => new Edge<string>(s, t));
            }
        }
    }
}
