using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="TarjanOfflineLeastCommonAncestorAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class TarjanOfflineLeastCommonAncestorAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        public void RunTarjanOfflineLeastCommonAncestorAndCheck<TVertex, TEdge>(
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
                    Assert.IsTrue(predecessors.VerticesPredecessors.IsPredecessor(root, pair.Source));
                    Assert.IsTrue(predecessors.VerticesPredecessors.IsPredecessor(root, pair.Target));
                }
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(null, graph);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                TarjanOfflineLeastCommonAncestorAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                CollectionAssert.IsEmpty(algo.Ancestors);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(null));
            Assert.Throws<ArgumentNullException>(
                () => new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(
                () => new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(graph));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 0, 1 });
            var algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(graph);
            algorithm.SetVertexPairs(new[] { new SEquatableEdge<int>(0, 1) });
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new TarjanOfflineLeastCommonAncestorAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        [Test]
        public void TryGetVertexPairs()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(graph);
            Assert.IsFalse(algorithm.TryGetVertexPairs(out _));

            graph.AddVertexRange(new[] { 1, 2 });
            algorithm.SetVertexPairs(new[] { new SEquatableEdge<int>(1, 2) });
            Assert.IsTrue(algorithm.TryGetVertexPairs(out IEnumerable<SEquatableEdge<int>> pairs));
            CollectionAssert.AreEqual(
                new[] { new SEquatableEdge<int>(1, 2) },
                pairs);
        }

        [Test]
        public void SetVertexPairs()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2 });
            var algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(graph);

            var pairs = new[]
            {
                new SEquatableEdge<int>(1, 2),
                new SEquatableEdge<int>(2, 1)
            };
            Assert.IsFalse(algorithm.TryGetVertexPairs(out _));
            algorithm.SetVertexPairs(pairs);
            Assert.IsTrue(algorithm.TryGetVertexPairs(out IEnumerable<SEquatableEdge<int>> gotPairs));
            CollectionAssert.AreEqual(pairs, gotPairs);
        }

        [Test]
        public void SetVertexPairs_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm.SetVertexPairs(null));
            Assert.Throws<ArgumentException>(() => algorithm.SetVertexPairs(Enumerable.Empty<SEquatableEdge<int>>()));
            Assert.Throws<ArgumentException>(() => algorithm.SetVertexPairs(new[] { new SEquatableEdge<int>(1, 2) }));
            graph.AddVertex(1);
            Assert.Throws<ArgumentException>(() => algorithm.SetVertexPairs(new[] { new SEquatableEdge<int>(1, 2) }));
        }

        [Test]
        public void TarjanOfflineLeastCommonAncestor()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests())
            {
                if (graph.VertexCount == 0)
                    continue;

                var pairs = new List<SEquatableEdge<string>>();
                foreach (string u in graph.Vertices)
                {
                    foreach (string v in graph.Vertices)
                    {
                        if (!u.Equals(v))
                            pairs.Add(new SEquatableEdge<string>(u, v));
                    }
                }

                int count = 0;
                foreach (string root in graph.Vertices)
                {
                    RunTarjanOfflineLeastCommonAncestorAndCheck(
                        graph,
                        root,
                        pairs.ToArray());

                    if (count++ > 10)
                        break;
                }
            }
        }

        [Test]
        public void TarjanOfflineLeastCommonAncestor_Throws()
        {
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var vertex3 = new TestVertex("3");
            var pairs = new[] { new SEquatableEdge<TestVertex>(vertex1, vertex2) };
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            graph.AddVertexRange(new[] { vertex1, vertex2 });
            var algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<TestVertex, Edge<TestVertex>>(graph);

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm.Compute(null, pairs));
            Assert.Throws<ArgumentNullException>(() => algorithm.Compute(vertex1, null));
            Assert.Throws<ArgumentNullException>(() => algorithm.Compute(null, null));
            Assert.Throws<ArgumentException>(() => algorithm.Compute(vertex3, pairs));
            // ReSharper restore AssignNullToNotNullAttribute

            algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            algorithm.SetRootVertex(vertex1);
            Assert.Throws<InvalidOperationException>(() => algorithm.Compute());
        }
    }
}