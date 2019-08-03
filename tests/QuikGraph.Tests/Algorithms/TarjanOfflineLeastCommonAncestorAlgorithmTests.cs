using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="TarjanOfflineLeastCommonAncestorAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class TarjanOfflineLeastCommonAncestorAlgorithmTests
    {
        #region Helpers

        public void TarjanOfflineLeastCommonAncestorAlgorithm<TVertex, TEdge>(
            [NotNull] IVertexListGraph<TVertex, TEdge> graph,
            [NotNull] TVertex root,
            [NotNull] SEquatableEdge<TVertex>[] pairs)
            where TEdge : IEdge<TVertex>
        {
            TryFunc<SEquatableEdge<TVertex>, TVertex> lca = graph.OfflineLeastCommonAncestor(root, pairs);
            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(graph);
            using (predecessors.Attach(dfs))
                dfs.Compute(root);

            foreach (SEquatableEdge<TVertex> pair in pairs)
            {
                if (lca(pair, out TVertex _))
                {
                    Assert.IsTrue(predecessors.VertexPredecessors.IsPredecessor(root, pair.Source));
                    Assert.IsTrue(predecessors.VertexPredecessors.IsPredecessor(root, pair.Target));
                }
            }
        }

        #endregion

        [Test]
        public void TarjanOfflineLeastCommonAncestorAlgorithmAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
            {
                if (graph.VertexCount == 0)
                    continue;

                var pairs = new List<SEquatableEdge<string>>();
                foreach (string v in graph.Vertices)
                {
                    foreach (string w in graph.Vertices)
                    {
                        if (!v.Equals(w))
                            pairs.Add(new SEquatableEdge<string>(v, w));
                    }
                }

                int count = 0;
                foreach (string root in graph.Vertices)
                {
                    TarjanOfflineLeastCommonAncestorAlgorithm(
                        graph,
                        root,
                        pairs.ToArray());

                    if (count++ > 10)
                        break;
                }
            }
        }
    }
}
