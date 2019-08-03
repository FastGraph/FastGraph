using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.Search;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="UndirectedDepthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class UndirectedDepthFirstAlgorithmSearchTests
    {
        #region Helpers

        public void RunUndirectedDepthFirstSearchAndCheck<TVertex, TEdge>(
            [NotNull] IUndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var discoverTimes = new Dictionary<TVertex, int>();
            var finishTimes = new Dictionary<TVertex, int>();
            int time = 0;
            var dfs = new UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>(graph);

            dfs.StartVertex += args =>
            {
                Assert.AreEqual(dfs.VerticesColors[args], GraphColor.White);
                Assert.IsFalse(parents.ContainsKey(args));
                parents[args] = args;
            };

            dfs.DiscoverVertex += args =>
            {
                Assert.AreEqual(dfs.VerticesColors[args], GraphColor.Gray);
                Assert.AreEqual(dfs.VerticesColors[parents[args]], GraphColor.Gray);

                discoverTimes[args] = time++;
            };

            dfs.ExamineEdge += (sender, args) =>
            {
                Assert.AreEqual(dfs.VerticesColors[args.Source], GraphColor.Gray);
            };

            dfs.TreeEdge += (sender, args) =>
            {
                var source = args.Source;
                var target = args.Target;
                Assert.AreEqual(dfs.VerticesColors[target], GraphColor.White);
                parents[target] = source;
            };

            dfs.BackEdge += (sender, args) =>
            {
                Assert.AreEqual(dfs.VerticesColors[args.Target], GraphColor.Gray);
            };

            dfs.ForwardOrCrossEdge += (sender, args) =>
            {
                Assert.AreEqual(dfs.VerticesColors[args.Target], GraphColor.Black);
            };

            dfs.FinishVertex += args =>
            {
                Assert.AreEqual(dfs.VerticesColors[args], GraphColor.Black);
                finishTimes[args] = time++;
            };

            dfs.Compute();

            // Check
            // All vertices should be black
            foreach (TVertex vertex in graph.Vertices)
            {
                Assert.IsTrue(dfs.VerticesColors.ContainsKey(vertex));
                Assert.AreEqual(dfs.VerticesColors[vertex], GraphColor.Black);
            }

            foreach (TVertex u in graph.Vertices)
            {
                foreach (TVertex v in graph.Vertices)
                {
                    if (!u.Equals(v))
                    {
                        Assert.IsTrue(
                            finishTimes[u] < discoverTimes[v]
                            || finishTimes[v] < discoverTimes[u]
                            || (discoverTimes[v] < discoverTimes[u] && finishTimes[u] < finishTimes[v] && IsDescendant(parents, u, v))
                            || (discoverTimes[u] < discoverTimes[v] && finishTimes[v] < finishTimes[u] && IsDescendant(parents, v, u)));
                    }
                }
            }
        }

        #endregion

        [Test]
        public void UndirectedDepthFirstSearchAll()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs())
                RunUndirectedDepthFirstSearchAndCheck(graph);
        }
    }
}
