#nullable enable

using JetBrains.Annotations;
using FastGraph.Algorithms.MaximumFlow;

namespace FastGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Base class for graph augmentor algorithms.
    /// </summary>
    internal abstract class GraphAugmentorAlgorithmTestsBase
    {
        protected static void CreateAndSetSuperSource_Test<TGraph>(
            GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            bool added = false;
            const int superSource = 1;
            algorithm.SuperSourceAdded += vertex =>
            {
                added = true;
                vertex.Should().Be(superSource);
            };

            algorithm.Compute();
            added.Should().BeTrue();
            algorithm.SuperSource.Should().Be(superSource);
        }

        protected static void CreateAndSetSuperSink_Test<TGraph>(
            GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            bool added = false;
            const int superSink = 2;
            algorithm.SuperSinkAdded += vertex =>
            {
                added = true;
                vertex.Should().Be(superSink);
            };

            algorithm.Compute();
            added.Should().BeTrue();
            algorithm.SuperSink.Should().Be(superSink);
        }

        protected static void RunAugmentation_Test<TGraph>(
            [InstantHandle]
            Func<
                IMutableVertexAndEdgeSet<int, Edge<int>>,
                GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph>
            > createAlgorithm,
            [InstantHandle] Action<IMutableVertexAndEdgeSet<int, Edge<int>>>? setupGraph = default)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            int vertexCount = graph.VertexCount;
            // Single run
            GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm = createAlgorithm(graph);
            algorithm.Augmented.Should().BeFalse();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount);

            algorithm.Compute();

            algorithm.Augmented.Should().BeTrue();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount + 2);

            // Multiple runs
            graph = new AdjacencyGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            algorithm = createAlgorithm(graph);
            algorithm.Augmented.Should().BeFalse();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount);

            algorithm.Compute();

            algorithm.Augmented.Should().BeTrue();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount + 2);

            algorithm.Rollback();

            algorithm.Augmented.Should().BeFalse();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount);

            algorithm.Compute();

            algorithm.Augmented.Should().BeTrue();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount + 2);

            // Disposed algorithm
            graph = new AdjacencyGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            using (algorithm = createAlgorithm(graph))
            {
                algorithm.Augmented.Should().BeFalse();
                algorithm.AugmentedEdges.Should().NotBeNull();
                algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount);

                algorithm.Compute();

                algorithm.Augmented.Should().BeTrue();
                algorithm.AugmentedEdges.Should().NotBeNull();
                algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount + 2);
            }
            graph.VertexCount.Should().Be(vertexCount);
        }

        protected static void RunAugmentation_Test<TGraph>(
            [InstantHandle]
            Func<
                IMutableBidirectionalGraph<int, Edge<int>>,
                GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph>
            > createAlgorithm,
            [InstantHandle] Action<IMutableBidirectionalGraph<int, Edge<int>>>? setupGraph = default)
            where TGraph : IMutableBidirectionalGraph<int, Edge<int>>
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            int vertexCount = graph.VertexCount;
            // Single run
            GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm = createAlgorithm(graph);
            algorithm.Augmented.Should().BeFalse();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount);

            algorithm.Compute();

            algorithm.Augmented.Should().BeTrue();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount + 2);

            // Multiple runs
            graph = new BidirectionalGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            algorithm = createAlgorithm(graph);
            algorithm.Augmented.Should().BeFalse();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount);

            algorithm.Compute();

            algorithm.Augmented.Should().BeTrue();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount + 2);

            algorithm.Rollback();

            algorithm.Augmented.Should().BeFalse();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount);

            algorithm.Compute();

            algorithm.Augmented.Should().BeTrue();
            algorithm.AugmentedEdges.Should().NotBeNull();
            algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount + 2);

            // Disposed algorithm
            graph = new BidirectionalGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            using (algorithm = createAlgorithm(graph))
            {
                algorithm.Augmented.Should().BeFalse();
                algorithm.AugmentedEdges.Should().NotBeNull();
                algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount);

                algorithm.Compute();

                algorithm.Augmented.Should().BeTrue();
                algorithm.AugmentedEdges.Should().NotBeNull();
                algorithm.VisitedGraph.VertexCount.Should().Be(vertexCount + 2);
            }
            graph.VertexCount.Should().Be(vertexCount);
        }

        protected static void RunAugmentation_Throws_Test<TGraph>(
            [InstantHandle] GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            // Multiple runs without clean
            Invoking(algorithm.Compute).Should().NotThrow();
            Invoking(algorithm.Compute).Should().Throw<InvalidOperationException>();
        }
    }
}
