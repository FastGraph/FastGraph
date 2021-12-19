#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.MaximumFlow;

namespace FastGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="GraphBalancerAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphBalancingAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2 });
            graph.AddVerticesAndEdge(new Edge<int>(1, 3));
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var capacities = new Dictionary<Edge<int>, double>();

            var algorithm = new GraphBalancerAlgorithm<int, Edge<int>>(graph, 1, 2, vertexFactory, edgeFactory);
            algorithm.VisitedGraph.Should().BeSameAs(graph);
            algorithm.VertexFactory.Should().BeSameAs(vertexFactory);
            algorithm.EdgeFactory.Should().BeSameAs(edgeFactory);
            algorithm.Balanced.Should().BeFalse();
            algorithm.Source.Should().Be(1);
            algorithm.Sink.Should().Be(2);
            algorithm.Capacities.Should().NotBeNull();
            algorithm.Capacities.Count.Should().Be(graph.EdgeCount);
            algorithm.SurplusVertices.Should().BeEmpty();
            algorithm.SurplusEdges.Should().BeEmpty();
            algorithm.DeficientVertices.Should().BeEmpty();
            algorithm.DeficientEdges.Should().BeEmpty();
            algorithm.BalancingSource.Should().Be(default);
            algorithm.BalancingSourceEdge.Should().Be(default(Edge<int>));
            algorithm.BalancingSink.Should().Be(default);
            algorithm.BalancingSinkEdge.Should().Be(default(Edge<int>));

            algorithm = new GraphBalancerAlgorithm<int, Edge<int>>(graph, 1, 2, vertexFactory, edgeFactory, capacities);
            algorithm.VisitedGraph.Should().BeSameAs(graph);
            algorithm.VertexFactory.Should().BeSameAs(vertexFactory);
            algorithm.EdgeFactory.Should().BeSameAs(edgeFactory);
            algorithm.Balanced.Should().BeFalse();
            algorithm.Source.Should().Be(1);
            algorithm.Sink.Should().Be(2);
            algorithm.Capacities.Should().BeSameAs(capacities);
            algorithm.SurplusVertices.Should().BeEmpty();
            algorithm.SurplusEdges.Should().BeEmpty();
            algorithm.DeficientVertices.Should().BeEmpty();
            algorithm.DeficientEdges.Should().BeEmpty();
            algorithm.BalancingSource.Should().Be(default);
            algorithm.BalancingSourceEdge.Should().Be(default(Edge<int>));
            algorithm.BalancingSink.Should().Be(default);
            algorithm.BalancingSinkEdge.Should().Be(default(Edge<int>));
        }

        [Test]
        public void Constructor_Throws()
        {
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");

            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graphWithVertex1 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            graphWithVertex1.AddVertex(vertex1);
            VertexFactory<TestVertex> vertexFactory = () => new TestVertex();
            EdgeFactory<TestVertex, Edge<TestVertex>> edgeFactory = (source, target) => new Edge<TestVertex>(source, target);
            var capacities = new Dictionary<Edge<TestVertex>, double>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, vertex2, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, vertex2, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, vertex2, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, vertex2, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, edgeFactory)).Should().Throw<ArgumentException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graphWithVertex1, vertex1, vertex2, vertexFactory, edgeFactory)).Should().Throw<ArgumentException>();


            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, vertex2, vertexFactory, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, vertexFactory, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, default, vertexFactory, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, default, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, vertexFactory, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, vertexFactory, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, vertex2, default, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, vertex2, vertexFactory, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, vertex2, vertexFactory, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, vertexFactory, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, default, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, vertexFactory, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, vertexFactory, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, default, default, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, default, vertexFactory, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, default, vertexFactory, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, default, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, vertexFactory, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, default, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, vertexFactory, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, vertexFactory, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, default, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, vertexFactory, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, vertexFactory, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, vertex2, default, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, vertex2, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, vertex2, vertexFactory, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, default, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, vertexFactory, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, vertexFactory, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, default, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, vertexFactory, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, default, default, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, default, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, default, edgeFactory, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, vertexFactory, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, vertexFactory, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, default, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, vertexFactory, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, default, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, vertexFactory, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, default, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, vertex2, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, vertexFactory, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, default, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, vertexFactory, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, vertex2, default, default, capacities)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, vertex1, default, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, default, default, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(default, default, default, default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, edgeFactory, capacities)).Should().Throw<ArgumentException>();
            Invoking(() => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graphWithVertex1, vertex1, vertex2, vertexFactory, edgeFactory, capacities)).Should().Throw<ArgumentException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Balance()
        {
            const int source = 1;
            const int sink = 3;
            var edge12 = new EquatableEdge<int>(1, 2);
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge23 = new EquatableEdge<int>(2, 3);
            var edge32 = new EquatableEdge<int>(3, 2);
            var edge34 = new EquatableEdge<int>(3, 4);
            var edge35 = new EquatableEdge<int>(3, 5);
            var edge42 = new EquatableEdge<int>(4, 2);
            var edge55 = new EquatableEdge<int>(5, 5);
            var edge67 = new EquatableEdge<int>(6, 7);
            var edge78 = new EquatableEdge<int>(7, 8);

            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge32, edge34,
                edge35, edge42, edge55, edge67, edge78
            });
            int vertexID = 9;
            VertexFactory<int> vertexFactory = () => vertexID++;
            EdgeFactory<int, EquatableEdge<int>> edgeFactory = (s, t) => new EquatableEdge<int>(s, t);

            var algorithm = new GraphBalancerAlgorithm<int, EquatableEdge<int>>(graph, source, sink, vertexFactory, edgeFactory);
            algorithm.BalancingSourceAdded += vertex =>
            {
                vertex.Should().Be(source);
            };
            algorithm.BalancingSinkAdded += vertex =>
            {
                vertex.Should().Be(sink);
            };
            var surplusSet = new HashSet<int> { 2, 5, 8 };
            algorithm.SurplusVertexAdded += vertex =>
            {
                surplusSet.Remove(vertex).Should().BeTrue();
            };
            var deficitSet = new HashSet<int> { 6 };
            algorithm.DeficientVertexAdded += vertex =>
            {
                deficitSet.Remove(vertex).Should().BeTrue();
            };

            algorithm.Balance();

            algorithm.Balanced.Should().BeTrue();
            algorithm.Source.Should().Be(source);
            algorithm.Sink.Should().Be(sink);
            surplusSet.Should().BeEmpty();
            deficitSet.Should().BeEmpty();
            algorithm.SurplusVertices.Should().BeEquivalentTo(new[] { 2, 5, 8 });
            algorithm.SurplusEdges.Should().BeEquivalentTo(new[]
            {
                new EquatableEdge<int>(algorithm.BalancingSource, 2),
                new EquatableEdge<int>(algorithm.BalancingSource, 5),
                new EquatableEdge<int>(algorithm.BalancingSource, 8)
            });
            algorithm.DeficientVertices.Should().BeEquivalentTo(new[] { 6 });
            algorithm.DeficientEdges.Should().BeEquivalentTo(new[]
            {
                new EquatableEdge<int>(6, algorithm.BalancingSink)
            });
            algorithm.BalancingSource.Should().Be(9);
            algorithm.BalancingSourceEdge.Should().Be(new EquatableEdge<int>(algorithm.BalancingSource, source));
            algorithm.BalancingSink.Should().Be(10);
            algorithm.BalancingSinkEdge.Should().Be(new EquatableEdge<int>(sink, algorithm.BalancingSink));
        }

        [Test]
        public void Balance_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2 });
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new GraphBalancerAlgorithm<int, Edge<int>>(graph, 1, 2, vertexFactory, edgeFactory);

            Invoking(() => algorithm.Balance()).Should().NotThrow();
            Invoking(() => algorithm.Balance()).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void UnBalance()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge32 = new Edge<int>(3, 2);
            var edge34 = new Edge<int>(3, 4);
            var edge56 = new Edge<int>(5, 6);

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge32, edge34, edge56
            });
            int vertexID = 6;
            VertexFactory<int> vertexFactory = () => vertexID++;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new GraphBalancerAlgorithm<int, Edge<int>>(graph, 1, 3, vertexFactory, edgeFactory);
            algorithm.Balance();

            algorithm.Balanced.Should().BeTrue();

            algorithm.UnBalance();

            algorithm.Balanced.Should().BeFalse();
            algorithm.Source.Should().Be(1);
            algorithm.Sink.Should().Be(3);
            algorithm.SurplusVertices.Should().BeEmpty();
            algorithm.SurplusEdges.Should().BeEmpty();
            algorithm.DeficientVertices.Should().BeEmpty();
            algorithm.DeficientEdges.Should().BeEmpty();
            algorithm.BalancingSource.Should().Be(default);
            algorithm.BalancingSourceEdge.Should().Be(default(Edge<int>));
            algorithm.BalancingSink.Should().Be(default);
            algorithm.BalancingSinkEdge.Should().Be(default(Edge<int>));
        }

        [Test]
        public void UnBalance_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2 });
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new GraphBalancerAlgorithm<int, Edge<int>>(graph, 1, 2, vertexFactory, edgeFactory);

            Invoking(() => algorithm.UnBalance()).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void GetBalancingIndex_Throws()
        {
            var source = new TestVertex("1");
            var sink = new TestVertex("2");
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            graph.AddVertexRange(new[] { source, sink });
            VertexFactory<TestVertex> vertexFactory = () => new TestVertex();
            EdgeFactory<TestVertex, Edge<TestVertex>> edgeFactory = (s, t) => new Edge<TestVertex>(s, t);

            var algorithm = new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(
                graph, source, sink, vertexFactory, edgeFactory);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm.GetBalancingIndex(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }
    }
}
