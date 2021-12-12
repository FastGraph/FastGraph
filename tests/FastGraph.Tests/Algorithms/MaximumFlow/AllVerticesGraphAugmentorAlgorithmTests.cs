#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.MaximumFlow;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="AllVerticesGraphAugmentorAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class AllVerticesGraphAugmentorAlgorithmTests : GraphAugmentorAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunAugmentationAndCheck(
            IMutableVertexAndEdgeListGraph<string, Edge<string>> graph)
        {
            int vertexCount = graph.VertexCount;
            int edgeCount = graph.EdgeCount;
            int vertexId = graph.VertexCount + 1;

            using (var augmentor = new AllVerticesGraphAugmentorAlgorithm<string, Edge<string>>(
                graph,
                () => (vertexId++).ToString(),
                (s, t) => new Edge<string>(s, t)))
            {
                bool added = false;
                augmentor.EdgeAdded += _ => { added = true; };

                augmentor.Compute();
                Assert.IsTrue(added);
                VerifyVertexCount(graph, augmentor, vertexCount);
                VerifySourceConnector(graph, augmentor);
                VerifySinkConnector(graph, augmentor);
            }

            Assert.AreEqual(graph.VertexCount, vertexCount);
            Assert.AreEqual(graph.EdgeCount, edgeCount);
        }

        private static void VerifyVertexCount<TVertex, TEdge>(
            IVertexSet<TVertex> graph,
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
            int vertexCount)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            Assert.AreEqual(vertexCount + 2 /* Source + Sink */, graph.VertexCount);
            Assert.IsTrue(graph.ContainsVertex(augmentor.SuperSource!));
            Assert.IsTrue(graph.ContainsVertex(augmentor.SuperSink!));
        }

        private static void VerifySourceConnector<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph,
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex vertex in graph.Vertices)
            {
                if (vertex.Equals(augmentor.SuperSource))
                    continue;
                if (vertex.Equals(augmentor.SuperSink))
                    continue;
                Assert.IsTrue(graph.ContainsEdge(augmentor.SuperSource!, vertex));
            }
        }

        private static void VerifySinkConnector<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph,
            AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex vertex in graph.Vertices)
            {
                if (vertex.Equals(augmentor.SuperSource))
                    continue;
                if (vertex.Equals(augmentor.SuperSink))
                    continue;
                Assert.IsTrue(graph.ContainsEdge(vertex, augmentor.SuperSink!));
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);
            AssertAlgorithmProperties(algorithm, graph, vertexFactory, edgeFactory);

            algorithm = new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, vertexFactory, edgeFactory);
            AssertAlgorithmProperties(algorithm, graph, vertexFactory, edgeFactory);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> algo,
                IMutableVertexAndEdgeSet<TVertex, TEdge> g,
                VertexFactory<int> vFactory,
                EdgeFactory<int, Edge<int>> eFactory)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.IsFalse(algo.Augmented);
                CollectionAssert.IsEmpty(algo.AugmentedEdges);
                Assert.AreSame(vFactory, algo.VertexFactory);
                Assert.AreSame(eFactory, algo.EdgeFactory);
                Assert.AreEqual(default(TVertex), algo.SuperSource);
                Assert.AreEqual(default(TVertex), algo.SuperSink);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(graph, default, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, default));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, default, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, vertexFactory, default));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(graph, default, default));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default));

            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, default, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, default, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, vertexFactory, default));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, default, vertexFactory, default));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, graph, default, default));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(default, default, default, default));
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Graph augmentor

        [Test]
        public void CreateAndSetSuperSource()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var algorithm = new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);

            CreateAndSetSuperSource_Test(algorithm);
        }

        [Test]
        public void CreateAndSetSuperSink()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var algorithm = new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);

            CreateAndSetSuperSink_Test(algorithm);
        }

        [Test]
        public void RunAugmentation()
        {
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            RunAugmentation_Test(
                graph => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory));
        }

        [Test]
        public void RunAugmentation_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var algorithm = new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);

            RunAugmentation_Throws_Test(algorithm);
        }

        #endregion

        [Test]
        public void AllVerticesAugmentor()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                RunAugmentationAndCheck(graph);
        }
    }
}
