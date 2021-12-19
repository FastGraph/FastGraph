#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
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
            Assert.IsFalse(algorithm.TryGetRootVertex(out _));

            var vertex = new TVertex();
            algorithm.SetRootVertex(vertex);
            Assert.IsTrue(algorithm.TryGetRootVertex(out TVertex? root));
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
            Assert.AreEqual(1, rootVertexChangeCount);
            algorithm.TryGetRootVertex(out int root);
            Assert.AreEqual(vertex1, root);

            // Not changed
            algorithm.SetRootVertex(vertex1);
            Assert.AreEqual(1, rootVertexChangeCount);
            algorithm.TryGetRootVertex(out root);
            Assert.AreEqual(vertex1, root);

            const int vertex2 = 1;
            algorithm.SetRootVertex(vertex2);
            Assert.AreEqual(2, rootVertexChangeCount);
            algorithm.TryGetRootVertex(out root);
            Assert.AreEqual(vertex2, root);

            algorithm.SetRootVertex(vertex1);
            Assert.AreEqual(3, rootVertexChangeCount);
            algorithm.TryGetRootVertex(out root);
            Assert.AreEqual(vertex1, root);
        }

        protected static void SetRootVertex_Throws_Test<TVertex, TGraph>(
            RootedAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : notnull
            where TGraph : IImplicitVertexSet<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Assert.Throws<ArgumentNullException>(() => algorithm.SetRootVertex(default));
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
            Assert.AreEqual(0, rootVertexChangeCount);

            var vertex = new TVertex();
            SetRootVertex(vertex);
            algorithm.ClearRootVertex();
            Assert.AreEqual(1, rootVertexChangeCount);

            algorithm.ClearRootVertex();
            Assert.AreEqual(1, rootVertexChangeCount);

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
            Assert.DoesNotThrow(algorithm.Compute);

            graph.AddVertexRange(new[] { 1, 2 });
            algorithm = createAlgorithm();
            Assert.DoesNotThrow(algorithm.Compute);
        }

        protected static void ComputeWithoutRoot_Throws_Test<TVertex, TGraph>(
            [InstantHandle] Func<RootedAlgorithmBase<TVertex, TGraph>> createAlgorithm)
            where TVertex : notnull, new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            RootedAlgorithmBase<TVertex, TGraph> algorithm = createAlgorithm();
            Assert.Throws<InvalidOperationException>(algorithm.Compute);

            // Source vertex set but not to a vertex in the graph
            algorithm = createAlgorithm();
            algorithm.SetRootVertex(new TVertex());
            Assert.Throws<VertexNotFoundException>(algorithm.Compute);
        }

        protected static void ComputeWithRoot_Test<TVertex, TGraph>(
            RootedAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : notnull, new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            var vertex = new TVertex();
            Assert.DoesNotThrow(() => algorithm.Compute(vertex));
            Assert.IsTrue(algorithm.TryGetRootVertex(out TVertex? root));
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
            Assert.Throws<ArgumentNullException>(() => algorithm.Compute(default));
#pragma warning restore CS8625
            Assert.IsFalse(algorithm.TryGetRootVertex(out _));

            // Vertex not in the graph
            algorithm = createAlgorithm();
            Assert.Throws<ArgumentException>(() => algorithm.Compute(new TVertex()));
        }

        #endregion
    }
}
