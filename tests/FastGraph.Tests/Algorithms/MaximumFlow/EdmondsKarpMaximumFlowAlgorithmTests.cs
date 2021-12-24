#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.MaximumFlow;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="EdmondsKarpMaximumFlowAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class EdmondsKarpMaximumFlowAlgorithmTests
    {
        #region Test helpers

        private static void EdmondsKarpMaxFlow<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph,
            EdgeFactory<TVertex, TEdge> edgeFactory)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            (graph.VertexCount > 0).Should().BeTrue();

            foreach (TVertex source in graph.Vertices)
            {
                foreach (TVertex sink in graph.Vertices)
                {
                    if (source.Equals(sink))
                        continue;

                    RunMaxFlowAlgorithmAndCheck(graph, edgeFactory, source, sink).Should().BePositive();
                }
            }
        }

        private static double RunMaxFlowAlgorithmAndCheck<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph,
            EdgeFactory<TVertex, TEdge> edgeFactory,
            TVertex source,
            TVertex sink)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var reversedEdgeAugmentorAlgorithm = new ReversedEdgeAugmentorAlgorithm<TVertex, TEdge>(graph, edgeFactory);
            reversedEdgeAugmentorAlgorithm.AddReversedEdges();

            double flow = graph.MaximumFlow(
                _ => 1,
                source, sink,
                out _,
                edgeFactory,
                reversedEdgeAugmentorAlgorithm);

            reversedEdgeAugmentorAlgorithm.RemoveReversedEdges();

            return flow;
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            Func<Edge<int>, double> capacities = _ => 1.0;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var reverseEdgesAlgorithm = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph, edgeFactory);

            var algorithm = new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(
                graph,
                capacities,
                edgeFactory,
                reverseEdgesAlgorithm);
            AssertAlgorithmProperties(
                algorithm,
                graph,
                capacities,
                edgeFactory);

            algorithm = new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(
                default,
                graph,
                capacities,
                edgeFactory,
                reverseEdgesAlgorithm);
            AssertAlgorithmProperties(
                algorithm,
                graph,
                capacities,
                edgeFactory);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                EdmondsKarpMaximumFlowAlgorithm<TVertex, TEdge> algo,
                IMutableVertexAndEdgeListGraph<TVertex, TEdge> g,
                Func<TEdge, double> c,
                EdgeFactory<int, Edge<int>>? eFactory)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.Predecessors.Should().BeEmpty();
                algo.Capacities.Should().BeSameAs(c);
                algo.ResidualCapacities.Should().BeEmpty();
                if (eFactory is null)
                    algo.EdgeFactory.Should().NotBeNull();
                else
                    algo.EdgeFactory.Should().BeSameAs(eFactory);
                algo.ReversedEdges.Should().BeEmpty();
                algo.Source.Should().Be(default(TVertex));
                algo.Sink.Should().Be(default(TVertex));
                algo.MaxFlow.Should().Be(0.0);
                algo.VerticesColors.Should().BeEmpty();
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            var graph2 = new AdjacencyGraph<int, Edge<int>>();
            Func<Edge<int>, double> capacities = _ => 1.0;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var reverseEdgesAlgorithm1 = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph1, edgeFactory);
            var reverseEdgesAlgorithm2 = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph2, edgeFactory);

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, capacities, edgeFactory, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(graph1, default, edgeFactory, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(graph1, capacities, default, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(graph1, capacities, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, edgeFactory, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, capacities, default, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, capacities, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(graph1, default, default, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(graph1, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(graph1, capacities, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, default, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, capacities, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(graph1, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, capacities, edgeFactory, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, graph1, default, edgeFactory, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, graph1, capacities, default, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, graph1, capacities, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, default, edgeFactory, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, capacities, default, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, capacities, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, graph1, default, default, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, graph1, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, graph1, capacities, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, default, default, reverseEdgesAlgorithm1)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, default, edgeFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, capacities, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, graph1, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(default, default, default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute

            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(graph1, capacities, edgeFactory, reverseEdgesAlgorithm2)).Should().Throw<ArgumentException>();
            Invoking(() => new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(graph2, capacities, edgeFactory, reverseEdgesAlgorithm1)).Should().Throw<ArgumentException>();
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void SimpleFlow()
        {
            const string source = "A";
            const string sink = "G";

            var graph = new AdjacencyGraph<string, EquatableTaggedEdge<string, double>>(true);
            graph.AddVertexRange(new[] { "A", "B", "C", "D", "E", "F", "G" });

            // TaggedEdge.Tag is the capacity of the edge
            graph.AddEdgeRange(new[]
            {
                new EquatableTaggedEdge<string, double>("A", "D", 3),
                new EquatableTaggedEdge<string, double>("A", "B", 3),
                new EquatableTaggedEdge<string, double>("B", "C", 4),
                new EquatableTaggedEdge<string, double>("C", "A", 3),
                new EquatableTaggedEdge<string, double>("C", "D", 1),
                new EquatableTaggedEdge<string, double>("D", "E", 2),
                new EquatableTaggedEdge<string, double>("D", "F", 6),
                new EquatableTaggedEdge<string, double>("E", "B", 1),
                new EquatableTaggedEdge<string, double>("C", "E", 2),
                new EquatableTaggedEdge<string, double>("E", "G", 1),
                new EquatableTaggedEdge<string, double>("F", "G", 9)
            });

            // edgeFactory will be used to create the reversed edges to store residual capacities using the ReversedEdgeAugmentorAlgorithm-class.
            // The edgeFactory assigns a capacity of 0.0 for the new edges because the initial (residual) capacity must be 0.0.
            EdgeFactory<string, EquatableTaggedEdge<string, double>> edgeFactory = (sourceNode, targetNode) => new EquatableTaggedEdge<string, double>(sourceNode, targetNode, 0.0);
            var reverseEdgesAlgorithm = new ReversedEdgeAugmentorAlgorithm<string, EquatableTaggedEdge<string, double>>(graph, edgeFactory);
            reverseEdgesAlgorithm.AddReversedEdges();

            var algorithm = new EdmondsKarpMaximumFlowAlgorithm<string, EquatableTaggedEdge<string, double>>(graph, edge => edge.Tag, edgeFactory, reverseEdgesAlgorithm);

            algorithm.Compute(source, sink);

            algorithm.Source.Should().Be(source);
            algorithm.Sink.Should().Be(sink);
            algorithm.MaxFlow.Should().Be(5);
            CheckReversedEdges();
            CheckPredecessors();
            CheckResidualCapacities();

            #region Local function

            void CheckReversedEdges()
            {
                (algorithm.ReversedEdges!.Count % 2 == 0).Should().BeTrue();
                foreach (var pair in algorithm.ReversedEdges)
                {
                    pair.Value.Target.Should().Be(pair.Key.Source);
                    pair.Value.Source.Should().Be(pair.Key.Target);
                }
            }

            void CheckPredecessors()
            {
                algorithm.Predecessors.Count.Should().Be(graph.VertexCount - 1);
                algorithm.Predecessors.Should().BeEquivalentTo(new Dictionary<string, EquatableTaggedEdge<string, double>>
                {
                    ["B"] = new EquatableTaggedEdge<string, double>("A", "B", 3),
                    ["C"] = new EquatableTaggedEdge<string, double>("B", "C", 4),
                    ["D"] = new EquatableTaggedEdge<string, double>("E", "D", 0),
                    ["E"] = new EquatableTaggedEdge<string, double>("C", "E", 2),
                    ["F"] = new EquatableTaggedEdge<string, double>("D", "F", 6),
                    ["G"] = new EquatableTaggedEdge<string, double>("F", "G", 9),
                });
            }

            void CheckResidualCapacities()
            {
                algorithm.ResidualCapacities.Count.Should().Be(graph.EdgeCount);
                algorithm.ResidualCapacities.Should().BeEquivalentTo(new Dictionary<EquatableTaggedEdge<string, double>, double>
                {
                    [new EquatableTaggedEdge<string, double>("A", "B", 3)] = 1,
                    [new EquatableTaggedEdge<string, double>("A", "C", 0)] = 0,
                    [new EquatableTaggedEdge<string, double>("A", "D", 3)] = 0,
                    [new EquatableTaggedEdge<string, double>("B", "A", 0)] = 2,
                    [new EquatableTaggedEdge<string, double>("B", "C", 4)] = 2,
                    [new EquatableTaggedEdge<string, double>("B", "E", 0)] = 0,
                    [new EquatableTaggedEdge<string, double>("C", "A", 3)] = 3,
                    [new EquatableTaggedEdge<string, double>("C", "B", 0)] = 2,
                    [new EquatableTaggedEdge<string, double>("C", "D", 1)] = 0,
                    [new EquatableTaggedEdge<string, double>("C", "E", 2)] = 1,
                    [new EquatableTaggedEdge<string, double>("D", "A", 0)] = 3,
                    [new EquatableTaggedEdge<string, double>("D", "C", 0)] = 1,
                    [new EquatableTaggedEdge<string, double>("D", "E", 2)] = 2,
                    [new EquatableTaggedEdge<string, double>("D", "F", 6)] = 2,
                    [new EquatableTaggedEdge<string, double>("E", "B", 1)] = 1,
                    [new EquatableTaggedEdge<string, double>("E", "C", 0)] = 1,
                    [new EquatableTaggedEdge<string, double>("E", "D", 0)] = 0,
                    [new EquatableTaggedEdge<string, double>("E", "G", 1)] = 0,
                    [new EquatableTaggedEdge<string, double>("F", "D", 0)] = 4,
                    [new EquatableTaggedEdge<string, double>("F", "G", 9)] = 5,
                    [new EquatableTaggedEdge<string, double>("G", "E", 0)] = 1,
                    [new EquatableTaggedEdge<string, double>("G", "F", 0)] = 4,
                });
            }

            #endregion
        }

        [Test]
        public void NotReachableSink()
        {
            const string source = "A";
            const string sink = "G";

            var graph = new AdjacencyGraph<string, TaggedEdge<string, double>>(true);
            graph.AddVertexRange(new[] { "A", "B", "C", "D", "E", "F", "G" });

            // TaggedEdge.Tag is the capacity of the edge
            graph.AddEdgeRange(new[]
            {
                new TaggedEdge<string, double>("A", "D", 3),
                new TaggedEdge<string, double>("A", "B", 3),
                new TaggedEdge<string, double>("B", "C", 4),
                new TaggedEdge<string, double>("C", "A", 3),
                new TaggedEdge<string, double>("C", "D", 1),
                new TaggedEdge<string, double>("D", "E", 2),
                new TaggedEdge<string, double>("D", "F", 6),
                new TaggedEdge<string, double>("E", "B", 1),
                new TaggedEdge<string, double>("C", "E", 2)
            });

            // edgeFactory will be used to create the reversed edges to store residual capacities using the ReversedEdgeAugmentorAlgorithm-class.
            // The edgeFactory assigns a capacity of 0.0 for the new edges because the initial (residual) capacity must be 0.0.
            EdgeFactory<string, TaggedEdge<string, double>> edgeFactory = (sourceNode, targetNode) => new TaggedEdge<string, double>(sourceNode, targetNode, 0.0);
            var reverseEdgesAlgorithm = new ReversedEdgeAugmentorAlgorithm<string, TaggedEdge<string, double>>(graph, edgeFactory);
            reverseEdgesAlgorithm.AddReversedEdges();

            var algorithm = new EdmondsKarpMaximumFlowAlgorithm<string, TaggedEdge<string, double>>(graph, edge => edge.Tag, edgeFactory, reverseEdgesAlgorithm);

            algorithm.Compute(source, sink);

            algorithm.Source.Should().Be(source);
            algorithm.Sink.Should().Be(sink);
            algorithm.VerticesColors.Count.Should().Be(graph.VertexCount);
            foreach (KeyValuePair<string, GraphColor> pair in algorithm.VerticesColors)
            {
                pair.Value.Should().Be(pair.Key == sink ? GraphColor.White : GraphColor.Black);
            }
            algorithm.MaxFlow.Should().Be(0);
        }

        [Test]
        public void GetVertexColor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));
            graph.AddVertex(3);

            Func<Edge<int>, double> capacities = _ => 1.0;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var reverseEdgesAlgorithm = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph, edgeFactory);
            reverseEdgesAlgorithm.AddReversedEdges();

            var algorithm = new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(graph, capacities, edgeFactory, reverseEdgesAlgorithm);
            algorithm.Compute(1, 2);

            algorithm.GetVertexColor(1).Should().Be(GraphColor.Black);
            algorithm.GetVertexColor(2).Should().Be(GraphColor.White);
            algorithm.GetVertexColor(3).Should().Be(GraphColor.White);
        }

        [TestCaseSource(nameof(AdjacencyGraphs_SlowTests))]
        [Category(TestCategories.LongRunning)]
        public void EdmondsKarpMaxFlow(TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string> testGraph)
        {
            EdmondsKarpMaxFlow(testGraph.Instance, (source, target) => new Edge<string>(source, target));
        }

        [Test]
        public void EdmondsKarpMaxFlow_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, TaggedEdge<TestVertex, double>>();
            EdgeFactory<TestVertex, TaggedEdge<TestVertex, double>> edgeFactory = (source, target) => new TaggedEdge<TestVertex, double>(source, target, 0.0);
            var reverseEdgesAlgorithm = new ReversedEdgeAugmentorAlgorithm<TestVertex, TaggedEdge<TestVertex, double>>(graph, edgeFactory);
            reverseEdgesAlgorithm.AddReversedEdges();

            var algorithm = new EdmondsKarpMaximumFlowAlgorithm<TestVertex, TaggedEdge<TestVertex, double>>(graph, edge => edge.Tag, edgeFactory, reverseEdgesAlgorithm);

            var vertex = new TestVertex("1");
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm.Compute(default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.Compute(vertex, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.Compute(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void EdmondsKarpMaxFlow_WrongVertices_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, TaggedEdge<TestVertex, double>>();
            EdgeFactory<TestVertex, TaggedEdge<TestVertex, double>> edgeFactory = (source, target) => new TaggedEdge<TestVertex, double>(source, target, 0.0);
            var reverseEdgesAlgorithm = new ReversedEdgeAugmentorAlgorithm<TestVertex, TaggedEdge<TestVertex, double>>(graph, edgeFactory);
            reverseEdgesAlgorithm.AddReversedEdges();

            var algorithm = new EdmondsKarpMaximumFlowAlgorithm<TestVertex, TaggedEdge<TestVertex, double>>(graph, edge => edge.Tag, edgeFactory, reverseEdgesAlgorithm);

            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            Invoking(() => algorithm.Compute()).Should().Throw<InvalidOperationException>();

            algorithm = new EdmondsKarpMaximumFlowAlgorithm<TestVertex, TaggedEdge<TestVertex, double>>(
                graph,
                edge => edge.Tag,
                edgeFactory,
                reverseEdgesAlgorithm)
            {
                Source = vertex1
            };
            Invoking(() => algorithm.Compute()).Should().Throw<InvalidOperationException>();

            algorithm = new EdmondsKarpMaximumFlowAlgorithm<TestVertex, TaggedEdge<TestVertex, double>>(
                graph,
                edge => edge.Tag,
                edgeFactory,
                reverseEdgesAlgorithm)
            {
                Source = vertex1,
                Sink = vertex2
            };
            Invoking(() => algorithm.Compute()).Should().Throw<VertexNotFoundException>();

            algorithm = new EdmondsKarpMaximumFlowAlgorithm<TestVertex, TaggedEdge<TestVertex, double>>(
                graph,
                edge => edge.Tag,
                edgeFactory,
                reverseEdgesAlgorithm)
            {
                Source = vertex1,
                Sink = vertex2
            };
            graph.AddVertex(vertex1);
            Invoking(() => algorithm.Compute()).Should().Throw<VertexNotFoundException>();
        }

        [Test]
        public void EdmondsKarpMaxFlow_NegativeCapacity_Throws()
        {
            const int source = 1;
            const int sink = 4;

            var graph = new AdjacencyGraph<int, TaggedEdge<int, double>>();

            // TaggedEdge.Tag is the capacity of the edge
            graph.AddVerticesAndEdgeRange(new[]
            {
                new TaggedEdge<int, double>(1, 2, 3),
                new TaggedEdge<int, double>(1, 4, 4),
                new TaggedEdge<int, double>(2, 3, -1),
                new TaggedEdge<int, double>(3, 4, 1)
            });

            EdgeFactory<int, TaggedEdge<int, double>> edgeFactory = (sourceNode, targetNode) => new TaggedEdge<int, double>(sourceNode, targetNode, 0.0);
            var reverseEdgesAlgorithm = new ReversedEdgeAugmentorAlgorithm<int, TaggedEdge<int, double>>(graph, edgeFactory);
            reverseEdgesAlgorithm.AddReversedEdges();

            var algorithm = new EdmondsKarpMaximumFlowAlgorithm<int, TaggedEdge<int, double>>(graph, edge => edge.Tag, edgeFactory, reverseEdgesAlgorithm);

            Invoking(() => algorithm.Compute(source, sink)).Should().Throw<NegativeCapacityException>();
        }

        [Test]
        public void EdmondsKarpMaxFlow_NotAugmented_Throws()
        {
            const int source = 1;
            const int sink = 4;

            var graph = new AdjacencyGraph<int, TaggedEdge<int, double>>();

            // TaggedEdge.Tag is the capacity of the edge
            graph.AddVerticesAndEdgeRange(new[]
            {
                new TaggedEdge<int, double>(1, 2, 3),
                new TaggedEdge<int, double>(1, 4, 4),
                new TaggedEdge<int, double>(2, 3, -1),
                new TaggedEdge<int, double>(3, 4, 1)
            });

            EdgeFactory<int, TaggedEdge<int, double>> edgeFactory = (sourceNode, targetNode) => new TaggedEdge<int, double>(sourceNode, targetNode, 0.0);
            var reverseEdgesAlgorithm = new ReversedEdgeAugmentorAlgorithm<int, TaggedEdge<int, double>>(graph, edgeFactory);
            var algorithm = new EdmondsKarpMaximumFlowAlgorithm<int, TaggedEdge<int, double>>(graph, edge => edge.Tag, edgeFactory, reverseEdgesAlgorithm);

            Invoking(() => algorithm.Compute(source, sink)).Should().Throw<InvalidOperationException>();
        }

        [Pure]
        public static EdmondsKarpMaximumFlowAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
            where T : notnull
        {
            var graph = new AdjacencyGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            double Capacities(Edge<T> edge) => 1.0;
            Edge<T> EdgeFactory(T source, T target) => new Edge<T>(source, target);
            var reverseEdgesAlgorithm = new ReversedEdgeAugmentorAlgorithm<T, Edge<T>>(graph, EdgeFactory);
            reverseEdgesAlgorithm.AddReversedEdges();

            var algorithm = new EdmondsKarpMaximumFlowAlgorithm<T, Edge<T>>(graph, Capacities, EdgeFactory, reverseEdgesAlgorithm);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root!, scenario.AccessibleVerticesFromRoot.First());
            return algorithm;
        }

        private static readonly IEnumerable<TestCaseData> AdjacencyGraphs_SlowTests =
            TestGraphFactory
                .SampleAdjacencyGraphs(100)
                .Where(t => t.VertexCount > 1)
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();
    }
}
