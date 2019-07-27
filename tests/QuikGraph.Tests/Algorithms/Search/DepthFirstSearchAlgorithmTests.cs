using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Serialization;
using QuikGraph.Tests;

namespace QuikGraph.Algorithms.Search
{
    [TestFixture]
    internal class DepthFirstAlgorithmSearchTests : QuikGraphUnitTests
    {
        private static bool IsDescendant<TVertex>(
            Dictionary<TVertex,TVertex> parents,
            TVertex u, 
            TVertex v)
        {
            TVertex t;
            TVertex p = u;
            do
            {
                t = p;
                p = parents[t];
                if (p.Equals(v))
                    return true;
            }
            while (!t.Equals(p));

            return false;
        }

        [Test]
        public void DepthFirstSearchAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
                this.DepthFirstSearch(g);
        }

        public void DepthFirstSearch<TVertex,TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var discoverTimes = new Dictionary<TVertex, int>();
            var finishTimes = new Dictionary<TVertex, int>();
            int time = 0;
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(g);

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

            dfs.ExamineEdge += args =>
            {
                Assert.AreEqual(dfs.VerticesColors[args.Source], GraphColor.Gray);
            };

            dfs.TreeEdge += args =>
            {
                Assert.AreEqual(dfs.VerticesColors[args.Target], GraphColor.White);
                parents[args.Target] = args.Source;
            };

            dfs.BackEdge += args =>
            {
                Assert.AreEqual(dfs.VerticesColors[args.Target], GraphColor.Gray);
            };

            dfs.ForwardOrCrossEdge += args =>
            {
                Assert.AreEqual(dfs.VerticesColors[args.Target], GraphColor.Black);
            };

            dfs.FinishVertex += args =>
            {
                Assert.AreEqual(dfs.VerticesColors[args], GraphColor.Black);
                finishTimes[args] = time++;
            };

            dfs.Compute();

            // check
            // all vertices should be black
            foreach (var v in g.Vertices)
            {
                Assert.IsTrue(dfs.VerticesColors.ContainsKey(v));
                Assert.AreEqual(dfs.VerticesColors[v], GraphColor.Black);
            }

            foreach (var u in g.Vertices)
            {
                foreach (var v in g.Vertices)
                {
                    if (!u.Equals(v))
                    {
                        Assert.IsTrue(
                            finishTimes[u] < discoverTimes[v]
                            || finishTimes[v] < discoverTimes[u]
                            || (
                            discoverTimes[v] < discoverTimes[u]
                            && finishTimes[u] < finishTimes[v]
                            && IsDescendant(parents, u, v)
                            )
                            || (
                            discoverTimes[u] < discoverTimes[v]
                            && finishTimes[v] < finishTimes[u]
                            && IsDescendant(parents, v, u)
                            )
                            );
                    }
                }
            }
        }
    }
}
