using System.Collections.Generic;
using Microsoft.Pex.Framework;
using NUnit.Framework;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Algorithms.Search;
using QuickGraph.Serialization;
using QuikGraph.Tests;

namespace QuickGraph.Tests.Algorithms
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

        [PexMethod]
        public void TarjanOfflineLeastCommonAncestorAlgorithm<TVertex, TEdge>(
            [PexAssumeNotNull]IVertexListGraph<TVertex, TEdge> g,
            [PexAssumeNotNull]TVertex root,
            [PexAssumeNotNull]SEquatableEdge<TVertex>[] pairs
            )
            where TEdge : IEdge<TVertex>
        {
            var lca = g.OfflineLeastCommonAncestorTarjan(root, pairs);
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
