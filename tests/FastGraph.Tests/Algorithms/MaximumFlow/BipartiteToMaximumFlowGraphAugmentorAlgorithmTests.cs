#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.MaximumFlow;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="BipartiteToMaximumFlowGraphAugmentorAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class BipartiteToMaximumFlowGraphAugmentorAlgorithmTests : GraphAugmentorAlgorithmTestsBase
    {
        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            int[] sourceToVertices = { 1, 2 };
            int[] verticesToSink = { 1, 2 };

            var algorithm = new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(
                graph,
                sourceToVertices,
                verticesToSink,
                vertexFactory,
                edgeFactory);
            AssertAlgorithmProperties(
                algorithm,
                graph,
                sourceToVertices,
                verticesToSink,
                vertexFactory,
                edgeFactory);

            algorithm = new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(
                default,
                graph,
                sourceToVertices,
                verticesToSink,
                vertexFactory,
                edgeFactory);
            AssertAlgorithmProperties(
                algorithm,
                graph,
                sourceToVertices,
                verticesToSink,
                vertexFactory,
                edgeFactory);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                BipartiteToMaximumFlowGraphAugmentorAlgorithm<TVertex, TEdge> algo,
                IMutableVertexAndEdgeSet<TVertex, TEdge> g,
                IEnumerable<TVertex> soToV,
                IEnumerable<TVertex> vToSi,
                VertexFactory<int> vFactory,
                EdgeFactory<int, Edge<int>> eFactory)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.Augmented.Should().BeFalse();
                algo.AugmentedEdges.Should().BeEmpty();
                algo.VertexFactory.Should().BeSameAs(vFactory);
                algo.EdgeFactory.Should().BeSameAs(eFactory);
                algo.SuperSource.Should().Be(default(TVertex));
                algo.SuperSink.Should().Be(default(TVertex));
                algo.SourceToVertices.Should().BeSameAs(soToV);
                algo.VerticesToSink.Should().BeSameAs(vToSi);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            int[] sourceToVertices = { 1, 2 };
            int[] verticesToSink = { 1, 2 };

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, sourceToVertices, verticesToSink, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, default, verticesToSink, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, verticesToSink, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, sourceToVertices, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, sourceToVertices, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, sourceToVertices, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, default, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, default, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, default, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, sourceToVertices, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, sourceToVertices, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, sourceToVertices, verticesToSink, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, default, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, default, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, verticesToSink, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, sourceToVertices, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, default, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, sourceToVertices, verticesToSink, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, default, verticesToSink, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, sourceToVertices, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, sourceToVertices, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, sourceToVertices, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, verticesToSink, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, sourceToVertices, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, sourceToVertices, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, sourceToVertices, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, default, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, default, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, default, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, sourceToVertices, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, sourceToVertices, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, sourceToVertices, verticesToSink, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, verticesToSink, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, verticesToSink, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, sourceToVertices, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, sourceToVertices, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, sourceToVertices, verticesToSink, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, default, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, default, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, sourceToVertices, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, verticesToSink, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, sourceToVertices, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, default, default, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Graph augmentor

        [Test]
        public void CreateAndSetSuperSource()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 3, 4, 5 });
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            int[] sourceToVertices = { 3, 4 };
            int[] verticesToSink = { 3, 5 };

            var algorithm = new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, vertexFactory, edgeFactory);

            CreateAndSetSuperSource_Test(algorithm);
        }

        [Test]
        public void CreateAndSetSuperSink()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 3, 4, 5 });
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            int[] sourceToVertices = { 3, 4 };
            int[] verticesToSink = { 3, 5 };

            var algorithm = new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, vertexFactory, edgeFactory);

            CreateAndSetSuperSink_Test(algorithm);
        }

        [Test]
        public void CreateAndSetSuperSourceOrSink_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            int[] sourceToVertices = { 3, 4 };
            int[] verticesToSink = { 3, 5 };

            var algorithm = new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, vertexFactory, edgeFactory);
            Invoking(() => algorithm.Compute()).Should().Throw<VertexNotFoundException>();
        }

        [Test]
        public void RunAugmentation()
        {
            int nextVertexId = 1;
            VertexFactory<int> vertexFactory = () =>
            {
                if (nextVertexId == 1)
                {
                    nextVertexId = 2;
                    return 1;
                }
                if (nextVertexId == 2)
                {
                    nextVertexId = 1;
                    return 2;
                }
                Assert.Fail("Should not arrive.");
                return 0;
            };
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            int[] sourceToVertices = { };
            int[] verticesToSink = { 4 };

            RunAugmentation_Test(
                graph => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, vertexFactory, edgeFactory),
                graph => graph.AddVertex(4));
        }

        [Test]
        public void RunAugmentation_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 3, 4 });
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            int[] sourceToVertices = { 3, 4 };
            int[] verticesToSink = { };

            var algorithm = new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, vertexFactory, edgeFactory);

            RunAugmentation_Throws_Test(algorithm);
        }

        #endregion
    }
}
