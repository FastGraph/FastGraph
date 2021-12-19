#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.MaximumFlow;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="MultiSourceSinkGraphAugmentorAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class MultiSourceSinkGraphAugmentorAlgorithmTests : GraphAugmentorAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunAugmentationAndCheck(
            IMutableBidirectionalGraph<string, Edge<string>> graph)
        {
            int vertexCount = graph.VertexCount;
            int edgeCount = graph.EdgeCount;
            int vertexId = graph.VertexCount + 1;

            string[] noInEdgesVertices = graph.Vertices.Where(graph.IsInEdgesEmpty).ToArray();
            string[] noOutEdgesVertices = graph.Vertices.Where(graph.IsOutEdgesEmpty).ToArray();

            using (var augmentor = new MultiSourceSinkGraphAugmentorAlgorithm<string, Edge<string>>(
                graph,
                () => (vertexId++).ToString(),
                (s, t) => new Edge<string>(s, t)))
            {
                bool added = false;
                augmentor.EdgeAdded += _ => { added = true; };

                augmentor.Compute();
                added.Should().BeTrue();
                VerifyVertexCount(graph, augmentor, vertexCount);
                VerifySourceConnector(graph, augmentor, noInEdgesVertices);
                VerifySinkConnector(graph, augmentor, noOutEdgesVertices);
            }

            vertexCount.Should().Be(graph.VertexCount);
            edgeCount.Should().Be(graph.EdgeCount);
        }

        private static void VerifyVertexCount<TVertex, TEdge>(
            IVertexSet<TVertex> graph,
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            MultiSourceSinkGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
            int vertexCount)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            graph.VertexCount.Should().Be(vertexCount + 2);
            graph.ContainsVertex(augmentor.SuperSource!).Should().BeTrue();
            graph.ContainsVertex(augmentor.SuperSink!).Should().BeTrue();
        }

        private static void VerifySourceConnector<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph,
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            MultiSourceSinkGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
            TVertex[] noInEdgesVertices)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex vertex in noInEdgesVertices)
            {
                graph.ContainsEdge(augmentor.SuperSource!, vertex).Should().BeTrue();
            }

            foreach (TVertex vertex in graph.Vertices.Except(noInEdgesVertices))
            {
                if (vertex.Equals(augmentor.SuperSource))
                    continue;
                if (vertex.Equals(augmentor.SuperSink))
                    continue;
                graph.ContainsEdge(augmentor.SuperSource!, vertex).Should().BeFalse();
            }
        }

        private static void VerifySinkConnector<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph,
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            MultiSourceSinkGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
            TVertex[] noOutEdgesVertices)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex vertex in noOutEdgesVertices)
            {
                graph.ContainsEdge(vertex, augmentor.SuperSink!).Should().BeTrue();
            }

            foreach (TVertex vertex in graph.Vertices.Except(noOutEdgesVertices))
            {
                if (vertex.Equals(augmentor.SuperSource))
                    continue;
                if (vertex.Equals(augmentor.SuperSink))
                    continue;
                graph.ContainsEdge(vertex, augmentor.SuperSink!).Should().BeFalse();
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);
            AssertAlgorithmProperties(algorithm, graph, vertexFactory, edgeFactory);

            algorithm = new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, vertexFactory, edgeFactory);
            AssertAlgorithmProperties(algorithm, graph, vertexFactory, edgeFactory);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                MultiSourceSinkGraphAugmentorAlgorithm<TVertex, TEdge> algo,
                IMutableBidirectionalGraph<TVertex, TEdge> g,
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
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, default, vertexFactory, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, edgeFactory)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, default, vertexFactory, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Graph augmentor

        [Test]
        public void CreateAndSetSuperSource()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var algorithm = new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);

            CreateAndSetSuperSource_Test(algorithm);
        }

        [Test]
        public void CreateAndSetSuperSink()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var algorithm = new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);

            CreateAndSetSuperSink_Test(algorithm);
        }

        [Test]
        public void RunAugmentation()
        {
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            RunAugmentation_Test(
                graph => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory));
        }

        [Test]
        public void RunAugmentation_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var algorithm = new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);

            RunAugmentation_Throws_Test(algorithm);
        }

        #endregion

        [Test]
        public void MultiSourceSinkGraphAugmentor()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_All())
                RunAugmentationAndCheck(graph);
        }
    }
}
