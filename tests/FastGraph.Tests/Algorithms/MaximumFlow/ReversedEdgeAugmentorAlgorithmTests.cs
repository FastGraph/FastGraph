#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms.MaximumFlow;

namespace FastGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="ReversedEdgeAugmentorAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class ReversedEdgeAugmentorAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph, edgeFactory);
            algorithm.VisitedGraph.Should().BeSameAs(graph);
            algorithm.EdgeFactory.Should().BeSameAs(edgeFactory);
            algorithm.Augmented.Should().BeFalse();
            algorithm.AugmentedEdges.Should().BeEmpty();
            algorithm.ReversedEdges.Should().BeEmpty();
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        private static IEnumerable<TestCaseData> AddReversedEdgeTestCases
        {
            [UsedImplicitly]
            get
            {
                EdgeFactory<int, Edge<int>> edgeFactory1 = (source, target) => new Edge<int>(source, target);
                yield return new TestCaseData(edgeFactory1);


                EdgeFactory<int, SEdge<int>> edgeFactory2 = (source, target) => new SEdge<int>(source, target);
                yield return new TestCaseData(edgeFactory2);
            }
        }

        [TestCaseSource(nameof(AddReversedEdgeTestCases))]
        public void AddReversedEdges<TEdge>(EdgeFactory<int, TEdge> edgeFactory)
            where TEdge : IEdge<int>
        {
            TEdge edge12 = edgeFactory(1, 2);
            TEdge edge13 = edgeFactory(1, 3);
            TEdge edge23 = edgeFactory(2, 3);
            TEdge edge32 = edgeFactory(3, 2);

            var graph = new AdjacencyGraph<int, TEdge>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge32
            });

            var algorithm = new ReversedEdgeAugmentorAlgorithm<int, TEdge>(graph, edgeFactory);

            var reverseEdgesAdded = new List<TEdge>();
            algorithm.ReversedEdgeAdded += edge => reverseEdgesAdded.Add(edge);

            algorithm.AddReversedEdges();

            algorithm.Augmented.Should().BeTrue();
            algorithm.AugmentedEdges.Should().NotBeEmpty();
            TEdge[] augmentedEdges = algorithm.AugmentedEdges.ToArray();
            augmentedEdges.Length.Should().Be(2);
            augmentedEdges[0].Source.Should().Be(2);
            augmentedEdges[0].Target.Should().Be(1);
            augmentedEdges[1].Source.Should().Be(3);
            augmentedEdges[1].Target.Should().Be(1);

            algorithm.ReversedEdges.Should().BeEquivalentTo(new Dictionary<TEdge, TEdge>
            {
                [edge12] = augmentedEdges[0],
                [augmentedEdges[0]] = edge12,
                [edge13] = augmentedEdges[1],
                [augmentedEdges[1]] = edge13,
                [edge23] = edge32,
                [edge32] = edge23,
            });

            augmentedEdges.Should().BeEquivalentTo(reverseEdgesAdded);
        }

        [Test]
        public void AddReversedEdges_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph, edgeFactory);
            Invoking(() => algorithm.AddReversedEdges()).Should().NotThrow();
            Invoking(() => algorithm.AddReversedEdges()).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void RemoveReversedEdges()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge32 = new Edge<int>(3, 2);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge32
            });

            var algorithm = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(
                graph,
                (source, target) => new Edge<int>(source, target));
            algorithm.AddReversedEdges();

            algorithm.Augmented.Should().BeTrue();
            algorithm.AugmentedEdges.Should().NotBeEmpty();
            foreach (Edge<int> edge in algorithm.AugmentedEdges)
            {
                algorithm.VisitedGraph.Edges.Should().Contain(edge);
            }
            algorithm.ReversedEdges.Should().NotBeEmpty();

            algorithm.RemoveReversedEdges();

            algorithm.Augmented.Should().BeFalse();
            algorithm.AugmentedEdges.Should().BeEmpty();
            foreach (Edge<int> edge in algorithm.AugmentedEdges)
            {
                algorithm.VisitedGraph.Edges.Should().NotContain(edge);
            }
            algorithm.ReversedEdges.Should().BeEmpty();
        }

        [Test]
        public void RemoveReversedEdges_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph, edgeFactory);
            Invoking(() => algorithm.RemoveReversedEdges()).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Dispose()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph, edgeFactory);
            algorithm.AugmentedEdges.Should().BeEmpty();
            algorithm.ReversedEdges.Should().BeEmpty();
            ((IDisposable)algorithm).Dispose();
            algorithm.AugmentedEdges.Should().BeEmpty();
            algorithm.ReversedEdges.Should().BeEmpty();

            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3),
                new Edge<int>(3, 2)
            });
            algorithm = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph, edgeFactory);
            algorithm.AddReversedEdges();
            algorithm.AugmentedEdges.Should().NotBeEmpty();
            algorithm.ReversedEdges.Should().NotBeEmpty();
            ((IDisposable)algorithm).Dispose();
            algorithm.AugmentedEdges.Should().BeEmpty();
            algorithm.ReversedEdges.Should().BeEmpty();
        }
    }
}
