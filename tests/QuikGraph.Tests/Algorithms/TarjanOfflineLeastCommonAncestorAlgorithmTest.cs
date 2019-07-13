using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;
using QuikGraph.Serialization;
using QuikGraph.Tests;

namespace QuikGraph.Tests.Algorithms
{
    [TestFixture]
    internal class TarjanOfflineLeastCommonAncestorAlgorithmTest : QuikGraphUnitTests
    {
        [Test]
        public void TarjanOfflineLeastCommonAncestorAlgorithmAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
            {
                if (g.VertexCount == 0) continue;

                var pairs = new List<SEquatableEdge<string>>();
                foreach(var v in g.Vertices)
                    foreach(var w in g.Vertices)
                        if (!v.Equals(w))
                            pairs.Add(new SEquatableEdge<string>(v,w));

                int count = 0;
                foreach (var root in g.Vertices)
                {
                    this.TarjanOfflineLeastCommonAncestorAlgorithm(
                        g,
                        root,
                        pairs.ToArray());
                    if (count++ > 10) break;
                }
            }
        }

        public void TarjanOfflineLeastCommonAncestorAlgorithm<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> g,
            TVertex root,
            SEquatableEdge<TVertex>[] pairs
            )
            where TEdge : IEdge<TVertex>
        {
            var lca = g.OfflineLeastCommonAncestor(root, pairs);
            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(g);
            using(predecessors.Attach(dfs))
                dfs.Compute(root);

            TVertex ancestor;
            foreach(var pair in pairs)
                if (lca(pair, out ancestor))
                {
                    Assert.IsTrue(EdgeExtensions.IsPredecessor(predecessors.VertexPredecessors, root, pair.Source));
                    Assert.IsTrue(EdgeExtensions.IsPredecessor(predecessors.VertexPredecessors, root, pair.Target));
                }
        }
    }
}
