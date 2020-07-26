using System;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.MaximumFlow;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="AllVerticesGraphAugmentorAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class AllVerticesGraphAugmentorAlgorithmTests : GraphAugmentorAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunAugmentationAndCheck(
            [NotNull] IMutableVertexAndEdgeListGraph<string, Edge<string>> graph)
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
                augmentor.EdgeAdded += edge => { added = true; };

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
            [NotNull] IVertexSet<TVertex> graph,
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            [NotNull] AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
            int vertexCount)
            where TEdge : IEdge<TVertex>
        {
            Assert.AreEqual(vertexCount + 2 /* Source + Sink */, graph.VertexCount);
            Assert.IsTrue(graph.ContainsVertex(augmentor.SuperSource));
            Assert.IsTrue(graph.ContainsVertex(augmentor.SuperSink));
        }

        private static void VerifySourceConnector<TVertex, TEdge>(
            [NotNull] IVertexListGraph<TVertex, TEdge> graph,
            [NotNull] AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex vertex in graph.Vertices)
            {
                if (vertex.Equals(augmentor.SuperSource))
                    continue;
                if (vertex.Equals(augmentor.SuperSink))
                    continue;
                Assert.IsTrue(graph.ContainsEdge(augmentor.SuperSource, vertex));
            }
        }

        private static void VerifySinkConnector<TVertex, TEdge>(
            [NotNull] IVertexListGraph<TVertex, TEdge> graph,
            [NotNull] AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> augmentor)
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex vertex in graph.Vertices)
            {
                if (vertex.Equals(augmentor.SuperSource))
                    continue;
                if (vertex.Equals(augmentor.SuperSink))
                    continue;
                Assert.IsTrue(graph.ContainsEdge(vertex, augmentor.SuperSink));
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

            algorithm = new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, vertexFactory, edgeFactory);
            AssertAlgorithmProperties(algorithm, graph, vertexFactory, edgeFactory);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                AllVerticesGraphAugmentorAlgorithm<TVertex, TEdge> algo,
                IMutableVertexAndEdgeSet<TVertex, TEdge> g,
                VertexFactory<int> vFactory,
                EdgeFactory<int, Edge<int>> eFactory)
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
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(graph, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(graph, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null));

            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new AllVerticesGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, null));
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