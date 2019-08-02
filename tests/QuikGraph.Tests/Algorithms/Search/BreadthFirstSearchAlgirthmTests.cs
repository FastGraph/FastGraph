using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.Search;

namespace QuikGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="BreadthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class BreadthFirstAlgorithmSearchTests
    {
        #region Helpers

        public void RunBfsAndCheck<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] TVertex sourceVertex)
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var distances = new Dictionary<TVertex, int>();
            TVertex currentVertex = default;
            int currentDistance = 0;
            var algorithm = new BreadthFirstSearchAlgorithm<TVertex, TEdge>(graph);

            algorithm.InitializeVertex += u =>
            {
                Assert.AreEqual(algorithm.VerticesColors[u], GraphColor.White);
            };

            algorithm.DiscoverVertex += u =>
            {
                Assert.AreEqual(algorithm.VerticesColors[u], GraphColor.Gray);
                if (u.Equals(sourceVertex))
                {
                    currentVertex = sourceVertex;
                }
                else
                {
                    Assert.IsNotNull(currentVertex);
                    Assert.AreEqual(parents[u], currentVertex);
                    // ReSharper disable once AccessToModifiedClosure
                    Assert.AreEqual(distances[u], currentDistance + 1);
                    Assert.AreEqual(distances[u], distances[parents[u]] + 1);
                }
            };

            algorithm.ExamineEdge += args =>
            {
                Assert.AreEqual(args.Source, currentVertex);
            };

            algorithm.ExamineVertex += args =>
            {
                TVertex u = args;
                currentVertex = u;
                // Ensure that the distances monotonically increase.
                // ReSharper disable AccessToModifiedClosure
                Assert.IsTrue(distances[u] == currentDistance || distances[u] == currentDistance + 1);

                if (distances[u] == currentDistance + 1) // New level
                    ++currentDistance;
                // ReSharper restore AccessToModifiedClosure
            };

            algorithm.TreeEdge += args =>
            {
                TVertex u = args.Source;
                TVertex v = args.Target;

                Assert.AreEqual(algorithm.VerticesColors[v], GraphColor.White);
                Assert.AreEqual(distances[u], currentDistance);
                parents[v] = u;
                distances[v] = distances[u] + 1;
            };

            algorithm.NonTreeEdge += args =>
            {
                TVertex u = args.Source;
                TVertex v = args.Target;

                Assert.IsFalse(algorithm.VerticesColors[v] == GraphColor.White);

                if (algorithm.VisitedGraph.IsDirected)
                {
                    // Cross or back edge
                    Assert.IsTrue(distances[v] <= distances[u] + 1);
                }
                else
                {
                    // Cross edge (or going backwards on a tree edge)
                    Assert.IsTrue(
                        distances[v] == distances[u]
                        || distances[v] == distances[u] + 1
                        || distances[v] == distances[u] - 1);
                }
            };

            algorithm.GrayTarget += args =>
            {
                Assert.AreEqual(algorithm.VerticesColors[args.Target], GraphColor.Gray);
            };

            algorithm.BlackTarget += args =>
            {
                Assert.AreEqual(algorithm.VerticesColors[args.Target], GraphColor.Black);

                foreach (TEdge edge in algorithm.VisitedGraph.OutEdges(args.Target))
                    Assert.IsFalse(algorithm.VerticesColors[edge.Target] == GraphColor.White);
            };

            algorithm.FinishVertex += args =>
            {
                Assert.AreEqual(algorithm.VerticesColors[args], GraphColor.Black);
            };

            parents.Clear();
            distances.Clear();
            currentDistance = 0;

            foreach (TVertex vertex in graph.Vertices)
            {
                distances[vertex] = int.MaxValue;
                parents[vertex] = vertex;
            }

            distances[sourceVertex] = 0;
            algorithm.Compute(sourceVertex);

            // All white vertices should be unreachable from the source.
            foreach (TVertex vertex in graph.Vertices)
            {
                if (algorithm.VerticesColors[vertex] == GraphColor.White)
                {
                    //!IsReachable(start,u,g);
                }
            }

            // The shortest path to a child should be one longer than
            // shortest path to the parent.
            foreach (TVertex vertex in graph.Vertices)
            {
                if (!parents[vertex].Equals(vertex)) // Not the root of the bfs tree
                    Assert.AreEqual(distances[vertex], distances[parents[vertex]] + 1);
            }
        }

        #endregion

        [Test]
        public void BreadthFirstSearchAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
            {
                foreach (string vertex in graph.Vertices)
                    RunBfsAndCheck(graph, vertex);
            }
        }
    }
}
