using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;

namespace QuikGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Tests for <see cref="UndirectedVertexDistanceRecorderObserver{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class UndirectedVertexDistanceRecorderObserverTests : ObserverTestsBase
    {
        [Test]
        public void Constructor()
        {
            Func<Edge<int>, double> edgeWeights = edge => 1.0;
            var recorder = new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(edgeWeights);
            Assert.AreSame(edgeWeights, recorder.EdgeWeights);
            Assert.IsNotNull(recorder.DistanceRelaxer);
            Assert.IsNotNull(recorder.Distances);

            var distances = new Dictionary<int, double>();
            recorder = new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(
                edgeWeights,
                DistanceRelaxers.ShortestDistance,
                distances);
            Assert.AreSame(edgeWeights, recorder.EdgeWeights);
            Assert.AreSame(DistanceRelaxers.ShortestDistance,recorder.DistanceRelaxer);
            Assert.AreSame(distances, recorder.Distances);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(null));

            Assert.Throws<ArgumentNullException>(
                () => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(null, DistanceRelaxers.ShortestDistance, new Dictionary<int, double>()));
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0, null, new Dictionary<int, double>()));
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0, DistanceRelaxers.ShortestDistance, null));
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(null, null, new Dictionary<int, double>()));
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Attach()
        {
            // Undirected DFS is used for tests but result may change if using another search algorithm
            // or another starting point
            {
                var recorder = new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0);

                var graph = new UndirectedGraph<int, Edge<int>>();

                var dfs = new UndirectedDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.IsEmpty(recorder.Distances);
                }
            }

            {
                var recorder = new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0);

                var graph = new UndirectedGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new UndirectedDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.IsEmpty(recorder.Distances);
                }
            }

            {
                var recorder = new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0);

                var edge12 = new Edge<int>(1, 2);
                var edge14 = new Edge<int>(1, 4);
                var edge31 = new Edge<int>(3, 1);
                var edge33 = new Edge<int>(3, 3);
                var edge34 = new Edge<int>(3, 4);
                var edge42 = new Edge<int>(4, 2);
                var graph = new UndirectedGraph<int, Edge<int>>();
                graph.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge14, edge31, edge33, edge34, edge42
                });

                var dfs = new UndirectedDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEqual(
                        new Dictionary<int, double>
                        {
                            [1] = 0,
                            [2] = 1,
                            [3] = 3,
                            [4] = 2
                        },
                        recorder.Distances);
                }
            }
        }

        [Test]
        public void Attach_Throws()
        {
            Attach_Throws_Test(new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0));
        }
    }
}