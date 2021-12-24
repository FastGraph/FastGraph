#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.Search;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="TarjanOfflineLeastCommonAncestorAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class TarjanOfflineLeastCommonAncestorAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        public void RunTarjanOfflineLeastCommonAncestorAndCheck<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph,
            TVertex root,
            SEquatableEdge<TVertex>[] pairs)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            TryFunc<SEquatableEdge<TVertex>, TVertex> lca = graph.OfflineLeastCommonAncestor(root, pairs);
            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(graph);
            using (predecessors.Attach(dfs))
                dfs.Compute(root);

            foreach (SEquatableEdge<TVertex> pair in pairs)
            {
                if (lca(pair, out _))
                {
                    predecessors.VerticesPredecessors.IsPredecessor(root, pair.Source).Should().BeTrue();
                    predecessors.VerticesPredecessors.IsPredecessor(root, pair.Target).Should().BeTrue();
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

            algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(default, graph);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                TarjanOfflineLeastCommonAncestorAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.Ancestors.Should().BeEmpty();
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
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
            algorithm.TryGetVertexPairs(out _).Should().BeFalse();

            graph.AddVertexRange(new[] { 1, 2 });
            algorithm.SetVertexPairs(new[] { new SEquatableEdge<int>(1, 2) });
            algorithm.TryGetVertexPairs(out IEnumerable<SEquatableEdge<int>>? pairs).Should().BeTrue();
            new[] { new SEquatableEdge<int>(1, 2) }.Should().BeEquivalentTo(pairs);
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
            algorithm.TryGetVertexPairs(out _).Should().BeFalse();
            algorithm.SetVertexPairs(pairs);
            algorithm.TryGetVertexPairs(out IEnumerable<SEquatableEdge<int>>? gotPairs).Should().BeTrue();
            pairs.Should().BeEquivalentTo(gotPairs);
        }

        [Test]
        public void SetVertexPairs_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<int, Edge<int>>(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm.SetVertexPairs(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            Invoking(() => algorithm.SetVertexPairs(Enumerable.Empty<SEquatableEdge<int>>())).Should().Throw<ArgumentException>();
            Invoking(() => algorithm.SetVertexPairs(new[] { new SEquatableEdge<int>(1, 2) })).Should().Throw<ArgumentException>();
            graph.AddVertex(1);
            Invoking(() => algorithm.SetVertexPairs(new[] { new SEquatableEdge<int>(1, 2) })).Should().Throw<ArgumentException>();
        }

        [TestCaseSource(nameof(AdjacencyGraphs_SlowTests_FirstSeveralVerticesOfEach))]
        public void TarjanOfflineLeastCommonAncestor(TestGraphInstanceWithSelectedVertex<AdjacencyGraph<string, Edge<string>>, string> testGraph)
        {
            var graph = testGraph.Instance;
            var root = testGraph.SelectedVertex;
            var pairs = new List<SEquatableEdge<string>>();
            foreach (string u in graph.Vertices)
            {
                foreach (string v in graph.Vertices)
                {
                    if (!u.Equals(v))
                        pairs.Add(new SEquatableEdge<string>(u, v));
                }
            }

            RunTarjanOfflineLeastCommonAncestorAndCheck(
                graph,
                root,
                pairs.ToArray());
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
#pragma warning disable CS8625
            Invoking(() => algorithm.Compute(default, pairs)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.Compute(vertex1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.Compute(default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.Compute(vertex3, pairs)).Should().Throw<ArgumentException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute

            algorithm = new TarjanOfflineLeastCommonAncestorAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            algorithm.SetRootVertex(vertex1);
            Invoking(() => algorithm.Compute()).Should().Throw<InvalidOperationException>();
        }

        private static readonly IEnumerable<TestCaseData> AdjacencyGraphs_SlowTests_FirstSeveralVerticesOfEach =
            TestGraphFactory
                .SampleAdjacencyGraphs()
                .Where(t => t.VertexCount != 0)
                .SelectMany(t => t.Select().Take(11))
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();
    }
}
