using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Algorithms.MaximumFlow;

namespace QuikGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="GraphBalancerAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class GraphBalancingAlgorithmTests
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
            Assert.AreSame(graph, algorithm.VisitedGraph);
            Assert.AreSame(vertexFactory, algorithm.VertexFactory);
            Assert.AreSame(edgeFactory, algorithm.EdgeFactory);
            Assert.IsFalse(algorithm.Balanced);
            Assert.AreEqual(1, algorithm.Source);
            Assert.AreEqual(2, algorithm.Sink);
            Assert.IsNotNull(algorithm.Capacities);
            Assert.AreEqual(graph.EdgeCount, algorithm.Capacities.Count);
            CollectionAssert.IsEmpty(algorithm.SurplusVertices);
            CollectionAssert.IsEmpty(algorithm.SurplusEdges);
            CollectionAssert.IsEmpty(algorithm.DeficientVertices);
            CollectionAssert.IsEmpty(algorithm.DeficientEdges);
            Assert.AreEqual(default(int), algorithm.BalancingSource);
            Assert.AreEqual(default(Edge<int>), algorithm.BalancingSourceEdge);
            Assert.AreEqual(default(int), algorithm.BalancingSink);
            Assert.AreEqual(default(Edge<int>), algorithm.BalancingSinkEdge);

            algorithm = new GraphBalancerAlgorithm<int, Edge<int>>(graph, 1, 2, vertexFactory, edgeFactory, capacities);
            Assert.AreSame(graph, algorithm.VisitedGraph);
            Assert.AreSame(vertexFactory, algorithm.VertexFactory);
            Assert.AreSame(edgeFactory, algorithm.EdgeFactory);
            Assert.IsFalse(algorithm.Balanced);
            Assert.AreEqual(1, algorithm.Source);
            Assert.AreEqual(2, algorithm.Sink);
            Assert.AreSame(capacities, algorithm.Capacities);
            CollectionAssert.IsEmpty(algorithm.SurplusVertices);
            CollectionAssert.IsEmpty(algorithm.SurplusEdges);
            CollectionAssert.IsEmpty(algorithm.DeficientVertices);
            CollectionAssert.IsEmpty(algorithm.DeficientEdges);
            Assert.AreEqual(default(int), algorithm.BalancingSource);
            Assert.AreEqual(default(Edge<int>), algorithm.BalancingSourceEdge);
            Assert.AreEqual(default(int), algorithm.BalancingSink);
            Assert.AreEqual(default(Edge<int>), algorithm.BalancingSinkEdge);
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
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, vertex2, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, vertex2, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, vertex2, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, vertex2, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, null, null));

            Assert.Throws<ArgumentException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graphWithVertex1, vertex1, vertex2, vertexFactory, edgeFactory));


            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, vertex2, vertexFactory, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, vertexFactory, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, null, vertexFactory, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, null, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, vertexFactory, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, vertexFactory, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, vertex2, null, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, vertex2, vertexFactory, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, vertex2, vertexFactory, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, vertexFactory, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, null, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, vertexFactory, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, vertexFactory, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, null, null, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, null, vertexFactory, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, null, vertexFactory, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, null, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, null, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, vertexFactory, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, null, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, vertexFactory, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, vertexFactory, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, null, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, vertexFactory, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, vertexFactory, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, vertex2, null, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, vertex2, null, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, vertex2, vertexFactory, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, null, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, vertexFactory, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, vertexFactory, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, null, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, null, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, vertexFactory, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, null, null, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, null, null, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, null, edgeFactory, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, vertexFactory, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, vertexFactory, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, null, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, null, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, vertexFactory, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, null, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, null, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, vertexFactory, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, null, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, vertex2, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, vertexFactory, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, null, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, null, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, null, edgeFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, vertexFactory, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, vertex2, null, null, capacities));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, vertex1, null, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, null, null, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(null, null, null, null, null, null));

            Assert.Throws<ArgumentException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graph, vertex1, vertex2, vertexFactory, edgeFactory, capacities));
            Assert.Throws<ArgumentException>(
                () => new GraphBalancerAlgorithm<TestVertex, Edge<TestVertex>>(graphWithVertex1, vertex1, vertex2, vertexFactory, edgeFactory, capacities));
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
            algorithm.BalancingSourceAdded += vertex => Assert.AreEqual(source, vertex);
            algorithm.BalancingSinkAdded += vertex => Assert.AreEqual(sink, vertex);
            var surplusSet = new HashSet<int> { 2, 5, 8 };
            algorithm.SurplusVertexAdded += vertex => Assert.IsTrue(surplusSet.Remove(vertex));
            var deficitSet = new HashSet<int> { 6 };
            algorithm.DeficientVertexAdded += vertex => Assert.IsTrue(deficitSet.Remove(vertex));

            algorithm.Balance();

            Assert.IsTrue(algorithm.Balanced);
            Assert.AreEqual(source, algorithm.Source);
            Assert.AreEqual(sink, algorithm.Sink);
            CollectionAssert.IsEmpty(surplusSet);
            CollectionAssert.IsEmpty(deficitSet);
            CollectionAssert.AreEquivalent(new[] { 2, 5, 8 },algorithm.SurplusVertices);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    new EquatableEdge<int>(algorithm.BalancingSource, 2),
                    new EquatableEdge<int>(algorithm.BalancingSource, 5),
                    new EquatableEdge<int>(algorithm.BalancingSource, 8)
                },
                algorithm.SurplusEdges);
            CollectionAssert.AreEquivalent(new[] { 6 }, algorithm.DeficientVertices);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    new EquatableEdge<int>(6, algorithm.BalancingSink)
                },
                algorithm.DeficientEdges);
            Assert.AreEqual(9, algorithm.BalancingSource);
            Assert.AreEqual(new EquatableEdge<int>(algorithm.BalancingSource, source), algorithm.BalancingSourceEdge);
            Assert.AreEqual(10, algorithm.BalancingSink);
            Assert.AreEqual(new EquatableEdge<int>(sink, algorithm.BalancingSink), algorithm.BalancingSinkEdge);
        }

        [Test]
        public void Balance_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2 });
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new GraphBalancerAlgorithm<int, Edge<int>>(graph, 1, 2, vertexFactory, edgeFactory);

            Assert.DoesNotThrow(() => algorithm.Balance());
            Assert.Throws<InvalidOperationException>(() => algorithm.Balance());
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

            Assert.IsTrue(algorithm.Balanced);

            algorithm.UnBalance();

            Assert.IsFalse(algorithm.Balanced);
            Assert.AreEqual(1, algorithm.Source);
            Assert.AreEqual(3, algorithm.Sink);
            CollectionAssert.IsEmpty(algorithm.SurplusVertices);
            CollectionAssert.IsEmpty(algorithm.SurplusEdges);
            CollectionAssert.IsEmpty(algorithm.DeficientVertices);
            CollectionAssert.IsEmpty(algorithm.DeficientEdges);
            Assert.AreEqual(default(int), algorithm.BalancingSource);
            Assert.AreEqual(default(Edge<int>), algorithm.BalancingSourceEdge);
            Assert.AreEqual(default(int), algorithm.BalancingSink);
            Assert.AreEqual(default(Edge<int>), algorithm.BalancingSinkEdge);
        }

        [Test]
        public void UnBalance_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2 });
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new GraphBalancerAlgorithm<int, Edge<int>>(graph, 1, 2, vertexFactory, edgeFactory);

            Assert.Throws<InvalidOperationException>(() => algorithm.UnBalance());
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
            Assert.Throws<ArgumentNullException>(() => algorithm.GetBalancingIndex(null));
        }
    }
}