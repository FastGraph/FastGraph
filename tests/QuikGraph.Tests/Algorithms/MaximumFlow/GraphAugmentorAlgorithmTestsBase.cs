using System;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.MaximumFlow;

namespace QuikGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Base class for graph augmentor algorithms.
    /// </summary>
    internal abstract class GraphAugmentorAlgorithmTestsBase
    {
        protected static void CreateAndSetSuperSource_Test<TGraph>(
            [NotNull] GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            bool added = false;
            const int superSource = 1;
            algorithm.SuperSourceAdded += vertex =>
            {
                added = true;
                Assert.AreEqual(superSource, vertex);
            };

            algorithm.Compute();
            Assert.IsTrue(added);
            Assert.AreEqual(superSource, algorithm.SuperSource);
        }

        protected static void CreateAndSetSuperSink_Test<TGraph>(
            [NotNull] GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            bool added = false;
            const int superSink = 2;
            algorithm.SuperSinkAdded += vertex =>
            {
                added = true;
                Assert.AreEqual(superSink, vertex);
            };

            algorithm.Compute();
            Assert.IsTrue(added);
            Assert.AreEqual(superSink, algorithm.SuperSink);
        }

        protected static void RunAugmentation_Test<TGraph>(
            [NotNull, InstantHandle]
            Func<
                IMutableVertexAndEdgeSet<int, Edge<int>>,
                GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph>
            > createAlgorithm,
            [CanBeNull, InstantHandle] Action<IMutableVertexAndEdgeSet<int, Edge<int>>> setupGraph = null)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            int vertexCount = graph.VertexCount;
            // Single run
            GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm = createAlgorithm(graph);
            Assert.IsFalse(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount, algorithm.VisitedGraph.VertexCount);

            algorithm.Compute();

            Assert.IsTrue(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount + 2, algorithm.VisitedGraph.VertexCount);

            // Multiple runs
            graph = new AdjacencyGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            algorithm = createAlgorithm(graph);
            Assert.IsFalse(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount, algorithm.VisitedGraph.VertexCount);

            algorithm.Compute();

            Assert.IsTrue(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount + 2, algorithm.VisitedGraph.VertexCount);

            algorithm.Rollback();

            Assert.IsFalse(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount, algorithm.VisitedGraph.VertexCount);

            algorithm.Compute();

            Assert.IsTrue(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount + 2, algorithm.VisitedGraph.VertexCount);

            // Disposed algorithm
            graph = new AdjacencyGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            using (algorithm = createAlgorithm(graph))
            {
                Assert.IsFalse(algorithm.Augmented);
                Assert.IsNotNull(algorithm.AugmentedEdges);
                Assert.AreEqual(vertexCount, algorithm.VisitedGraph.VertexCount);

                algorithm.Compute();

                Assert.IsTrue(algorithm.Augmented);
                Assert.IsNotNull(algorithm.AugmentedEdges);
                Assert.AreEqual(vertexCount + 2, algorithm.VisitedGraph.VertexCount);
            }
            Assert.AreEqual(vertexCount, graph.VertexCount);
        }

        protected static void RunAugmentation_Test<TGraph>(
            [NotNull, InstantHandle]
            Func<
                IMutableBidirectionalGraph<int, Edge<int>>,
                GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph>
            > createAlgorithm,
            [CanBeNull, InstantHandle] Action<IMutableBidirectionalGraph<int, Edge<int>>> setupGraph = null)
            where TGraph : IMutableBidirectionalGraph<int, Edge<int>>
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            int vertexCount = graph.VertexCount;
            // Single run
            GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm = createAlgorithm(graph);
            Assert.IsFalse(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount, algorithm.VisitedGraph.VertexCount);

            algorithm.Compute();

            Assert.IsTrue(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount + 2, algorithm.VisitedGraph.VertexCount);

            // Multiple runs
            graph = new BidirectionalGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            algorithm = createAlgorithm(graph);
            Assert.IsFalse(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount, algorithm.VisitedGraph.VertexCount);

            algorithm.Compute();

            Assert.IsTrue(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount + 2, algorithm.VisitedGraph.VertexCount);

            algorithm.Rollback();

            Assert.IsFalse(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount, algorithm.VisitedGraph.VertexCount);

            algorithm.Compute();

            Assert.IsTrue(algorithm.Augmented);
            Assert.IsNotNull(algorithm.AugmentedEdges);
            Assert.AreEqual(vertexCount + 2, algorithm.VisitedGraph.VertexCount);

            // Disposed algorithm
            graph = new BidirectionalGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            using (algorithm = createAlgorithm(graph))
            {
                Assert.IsFalse(algorithm.Augmented);
                Assert.IsNotNull(algorithm.AugmentedEdges);
                Assert.AreEqual(vertexCount, algorithm.VisitedGraph.VertexCount);

                algorithm.Compute();

                Assert.IsTrue(algorithm.Augmented);
                Assert.IsNotNull(algorithm.AugmentedEdges);
                Assert.AreEqual(vertexCount + 2, algorithm.VisitedGraph.VertexCount);
            }
            Assert.AreEqual(vertexCount, graph.VertexCount);
        }

        protected static void RunAugmentation_Throws_Test<TGraph>(
            [NotNull, InstantHandle] GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            // Multiple runs without clean
            Assert.DoesNotThrow(algorithm.Compute);
            Assert.Throws<InvalidOperationException>(algorithm.Compute);
        }
    }
}