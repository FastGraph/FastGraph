#nullable enable

using JetBrains.Annotations;
using FastGraph.Algorithms;
using static FastGraph.Tests.AssertHelpers;

namespace FastGraph.Tests.Algorithms
{
    /// <summary>
    /// Base class for rooted algorithm tests.
    /// </summary>
    internal abstract class RootedAlgorithmTestsBase
    {
        #region Test helpers

        protected static void TryGetRootVertex_Test<TVertex, TGraph>(
            RootedAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : notnull, new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            algorithm.TryGetRootVertex(out _).Should().BeFalse();

            var vertex = new TVertex();
            algorithm.SetRootVertex(vertex);
            algorithm.TryGetRootVertex(out TVertex? root).Should().BeTrue();
            AssertEqual(vertex, root);
        }

        protected static void SetRootVertex_Test<TGraph>(
            RootedAlgorithmBase<int, TGraph> algorithm)
            where TGraph : IImplicitVertexSet<int>
        {
            int rootVertexChangeCount = 0;
            algorithm.RootVertexChanged += (_, _) => ++rootVertexChangeCount;

            const int vertex1 = 0;
            algorithm.SetRootVertex(vertex1);
            rootVertexChangeCount.Should().Be(1);
            algorithm.TryGetRootVertex(out int root);
            root.Should().Be(vertex1);

            // Not changed
            algorithm.SetRootVertex(vertex1);
            rootVertexChangeCount.Should().Be(1);
            algorithm.TryGetRootVertex(out root);
            root.Should().Be(vertex1);

            const int vertex2 = 1;
            algorithm.SetRootVertex(vertex2);
            rootVertexChangeCount.Should().Be(2);
            algorithm.TryGetRootVertex(out root);
            root.Should().Be(vertex2);

            algorithm.SetRootVertex(vertex1);
            rootVertexChangeCount.Should().Be(3);
            algorithm.TryGetRootVertex(out root);
            root.Should().Be(vertex1);
        }

        protected static void SetRootVertex_Throws_Test<TVertex, TGraph>(
            RootedAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : notnull
            where TGraph : IImplicitVertexSet<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => algorithm.SetRootVertex(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
        }

        protected static void ClearRootVertex_Test<TVertex, TGraph>(
            RootedAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : notnull, new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            int rootVertexChangeCount = 0;
            // ReSharper disable once AccessToModifiedClosure
            algorithm.RootVertexChanged += (_, _) => ++rootVertexChangeCount;

            algorithm.ClearRootVertex();
            rootVertexChangeCount.Should().Be(0);

            var vertex = new TVertex();
            SetRootVertex(vertex);
            algorithm.ClearRootVertex();
            rootVertexChangeCount.Should().Be(1);

            algorithm.ClearRootVertex();
            rootVertexChangeCount.Should().Be(1);

            #region Local function

            void SetRootVertex(TVertex v)
            {
                algorithm.SetRootVertex(v);
                rootVertexChangeCount = 0;
            }

            #endregion
        }

        protected static void ComputeWithoutRoot_NoThrows_Test<TGraph>(
            IMutableVertexSet<int> graph,
            [InstantHandle] Func<RootedAlgorithmBase<int, TGraph>> createAlgorithm)
            where TGraph : IImplicitVertexSet<int>
        {
            RootedAlgorithmBase<int, TGraph> algorithm = createAlgorithm();
            Invoking(algorithm.Compute).Should().NotThrow();

            graph.AddVertexRange(new[] { 1, 2 });
            algorithm = createAlgorithm();
            Invoking(algorithm.Compute).Should().NotThrow();
        }

        protected static void ComputeWithoutRoot_Throws_Test<TVertex, TGraph>(
            [InstantHandle] Func<RootedAlgorithmBase<TVertex, TGraph>> createAlgorithm)
            where TVertex : notnull, new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            RootedAlgorithmBase<TVertex, TGraph> algorithm = createAlgorithm();
            Invoking(algorithm.Compute).Should().Throw<InvalidOperationException>();

            // Source vertex set but not to a vertex in the graph
            algorithm = createAlgorithm();
            algorithm.SetRootVertex(new TVertex());
            Invoking(algorithm.Compute).Should().Throw<VertexNotFoundException>();
        }

        protected static void ComputeWithRoot_Test<TVertex, TGraph>(
            RootedAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : notnull, new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            var vertex = new TVertex();
            Invoking(() => algorithm.Compute(vertex)).Should().NotThrow();
            algorithm.TryGetRootVertex(out TVertex? root).Should().BeTrue();
            AssertEqual(vertex, root);
        }

        protected static void ComputeWithRoot_Throws_Test<TVertex, TGraph>(
            [InstantHandle] Func<RootedAlgorithmBase<TVertex, TGraph>> createAlgorithm)
            where TVertex : class, new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            RootedAlgorithmBase<TVertex, TGraph> algorithm = createAlgorithm();
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm.Compute(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            algorithm.TryGetRootVertex(out _).Should().BeFalse();

            // Vertex not in the graph
            algorithm = createAlgorithm();
            Invoking(() => algorithm.Compute(new TVertex())).Should().Throw<ArgumentException>();
        }

        #endregion
    }
}
