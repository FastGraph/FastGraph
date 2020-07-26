using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Algorithms.MaximumFlow;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="BipartiteToMaximumFlowGraphAugmentorAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class BipartiteToMaximumFlowGraphAugmentorAlgorithmTests : GraphAugmentorAlgorithmTestsBase
    {
        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var sourceToVertices = new[] { 1, 2 };
            var verticesToSink = new[] { 1, 2 };

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
                null,
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
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.IsFalse(algo.Augmented);
                CollectionAssert.IsEmpty(algo.AugmentedEdges);
                Assert.AreSame(vFactory, algo.VertexFactory);
                Assert.AreSame(eFactory, algo.EdgeFactory);
                Assert.AreEqual(default(TVertex), algo.SuperSource);
                Assert.AreEqual(default(TVertex), algo.SuperSink);
                Assert.AreSame(soToV, algo.SourceToVertices);
                Assert.AreSame(vToSi, algo.VerticesToSink);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var sourceToVertices = new[] { 1, 2 };
            var verticesToSink = new[] { 1, 2 };

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, sourceToVertices, verticesToSink, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, null, verticesToSink, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, verticesToSink, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, sourceToVertices, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, sourceToVertices, verticesToSink, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, sourceToVertices, verticesToSink, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, null, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, null, verticesToSink, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, null, verticesToSink, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, verticesToSink, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, verticesToSink, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, sourceToVertices, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, sourceToVertices, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, sourceToVertices, verticesToSink, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, null, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, null, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, verticesToSink, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, sourceToVertices, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, null, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, null, null));

            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, sourceToVertices, verticesToSink, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, null, verticesToSink, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, sourceToVertices, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, sourceToVertices, verticesToSink, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, sourceToVertices, verticesToSink, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, verticesToSink, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, sourceToVertices, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, sourceToVertices, verticesToSink, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, sourceToVertices, verticesToSink, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, null, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, null, verticesToSink, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, null, verticesToSink, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, sourceToVertices, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, sourceToVertices, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, sourceToVertices, verticesToSink, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, verticesToSink, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, verticesToSink, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, sourceToVertices, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, sourceToVertices, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, sourceToVertices, verticesToSink, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, null, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, null, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, sourceToVertices, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, verticesToSink, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, sourceToVertices, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, null, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, null, null, null));
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
            var sourceToVertices = new[] { 3, 4 };
            var verticesToSink = new[] { 3, 5 };

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
            var sourceToVertices = new[] { 3, 4 };
            var verticesToSink = new[] { 3, 5 };

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
            var sourceToVertices = new[] { 3, 4 };
            var verticesToSink = new[] { 3, 5 };

            var algorithm = new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, vertexFactory, edgeFactory);
            Assert.Throws<VertexNotFoundException>(() => algorithm.Compute());
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
            var sourceToVertices = new int[] { };
            var verticesToSink = new[] { 4 };

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
            var sourceToVertices = new[] { 3, 4 };
            var verticesToSink = new int[] { };

            var algorithm = new BipartiteToMaximumFlowGraphAugmentorAlgorithm<int, Edge<int>>(graph, sourceToVertices, verticesToSink, vertexFactory, edgeFactory);

            RunAugmentation_Throws_Test(algorithm);
        }

        #endregion
    }
}