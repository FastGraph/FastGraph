#nullable enable

using JetBrains.Annotations;
using FastGraph.Algorithms;
using static FastGraph.Tests.AssertHelpers;

namespace FastGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Base class for search algorithms.
    /// </summary>
    internal abstract class SearchAlgorithmTestsBase : RootedAlgorithmTestsBase
    {
        #region Test helpers

        protected static void TryGetTargetVertex_Test<TVertex, TGraph>(
            RootedSearchAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : notnull, new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            algorithm.TryGetTargetVertex(out _).Should().BeFalse();

            var vertex = new TVertex();
            algorithm.SetTargetVertex(vertex);
            algorithm.TryGetTargetVertex(out TVertex? target).Should().BeTrue();
            AssertEqual(vertex, target);
        }

        protected static void SetTargetVertex_Test<TGraph>(
            RootedSearchAlgorithmBase<int, TGraph> algorithm)
            where TGraph : IImplicitVertexSet<int>
        {
            int targetVertexChangeCount = 0;
            algorithm.TargetVertexChanged += (_, _) => ++targetVertexChangeCount;

            const int vertex1 = 0;
            algorithm.SetTargetVertex(vertex1);
            targetVertexChangeCount.Should().Be(1);
            algorithm.TryGetTargetVertex(out int target);
            target.Should().Be(vertex1);

            // Not changed
            algorithm.SetTargetVertex(vertex1);
            targetVertexChangeCount.Should().Be(1);
            algorithm.TryGetTargetVertex(out target);
            target.Should().Be(vertex1);

            const int vertex2 = 1;
            algorithm.SetTargetVertex(vertex2);
            targetVertexChangeCount.Should().Be(2);
            algorithm.TryGetTargetVertex(out target);
            target.Should().Be(vertex2);

            algorithm.SetTargetVertex(vertex1);
            targetVertexChangeCount.Should().Be(3);
            algorithm.TryGetTargetVertex(out target);
            target.Should().Be(vertex1);
        }

        protected static void SetTargetVertex_Throws_Test<TVertex, TGraph>(
            RootedSearchAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : notnull
            where TGraph : IImplicitVertexSet<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => algorithm.SetTargetVertex(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
        }

        protected static void ClearTargetVertex_Test<TVertex, TGraph>(
            RootedSearchAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : notnull, new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            int targetVertexChangeCount = 0;
            // ReSharper disable once AccessToModifiedClosure
            algorithm.TargetVertexChanged += (_, _) => ++targetVertexChangeCount;

            algorithm.ClearTargetVertex();
            targetVertexChangeCount.Should().Be(0);

            var vertex = new TVertex();
            SetTargetVertex(vertex);
            algorithm.ClearTargetVertex();
            targetVertexChangeCount.Should().Be(1);

            algorithm.ClearTargetVertex();
            targetVertexChangeCount.Should().Be(1);

            #region Local function

            void SetTargetVertex(TVertex v)
            {
                algorithm.SetTargetVertex(v);
                targetVertexChangeCount = 0;
            }

            #endregion
        }

        protected static void ComputeWithoutRoot_Throws_Test<TGraph>(
            IMutableVertexSet<int> graph,
            [InstantHandle] Func<RootedSearchAlgorithmBase<int, TGraph>> createAlgorithm)
            where TGraph : IImplicitVertexSet<int>
        {
            RootedSearchAlgorithmBase<int, TGraph> algorithm = createAlgorithm();
            Invoking(algorithm.Compute).Should().Throw<InvalidOperationException>();

            // Source (and target) vertex set but not to a vertex in the graph
            const int vertex1 = 1;
            algorithm = createAlgorithm();
            algorithm.SetRootVertex(vertex1);
            algorithm.SetTargetVertex(vertex1);
            Invoking(algorithm.Compute).Should().Throw<VertexNotFoundException>();

            const int vertex2 = 2;
            graph.AddVertex(vertex1);
            algorithm = createAlgorithm();
            algorithm.SetRootVertex(vertex1);
            algorithm.SetTargetVertex(vertex2);
            Invoking(algorithm.Compute).Should().Throw<VertexNotFoundException>();
        }

        protected static void ComputeWithRootAndTarget_Test<TGraph>(
            RootedSearchAlgorithmBase<int, TGraph> algorithm)
            where TGraph : IImplicitVertexSet<int>
        {
            const int start = 0;
            const int end = 1;
            Invoking(() => algorithm.Compute(start, end)).Should().NotThrow();
            algorithm.TryGetRootVertex(out int root).Should().BeTrue();
            algorithm.TryGetTargetVertex(out int target).Should().BeTrue();
            AssertEqual(start, root);
            AssertEqual(end, target);
        }

        protected static void ComputeWithRootAndTarget_Throws_Test<TGraph>(
            IMutableVertexSet<int> graph,
            RootedSearchAlgorithmBase<int, TGraph> algorithm)
            where TGraph : IImplicitVertexSet<int>
        {
            const int start = 1;
            const int end = 2;

            Invoking(() => algorithm.Compute(start)).Should().Throw<ArgumentException>();
            graph.AddVertex(start);

            Invoking(() => algorithm.Compute(start)).Should().Throw<InvalidOperationException>();

            Invoking(() => algorithm.Compute(start, end)).Should().Throw<ArgumentException>();
        }

        protected static void ComputeWithRootAndTarget_Throws_Test<TVertex, TGraph>(
            RootedSearchAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : notnull, new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            var start = new TVertex();
            var end = new TVertex();

            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => algorithm.Compute(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.Compute(start, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.Compute(default, end)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.Compute(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion
    }
}
