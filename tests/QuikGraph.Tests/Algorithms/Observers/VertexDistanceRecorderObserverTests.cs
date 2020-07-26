using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;

namespace QuikGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Tests for <see cref="VertexDistanceRecorderObserver{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class VertexDistanceRecorderObserverTests : ObserverTestsBase
    {
        [Test]
        public void Constructor()
        {
            Func<Edge<int>, double> edgeWeights = edge => 1.0;
            var recorder = new VertexDistanceRecorderObserver<int, Edge<int>>(edgeWeights);
            Assert.AreSame(edgeWeights, recorder.EdgeWeights);
            Assert.IsNotNull(recorder.DistanceRelaxer);
            Assert.IsNotNull(recorder.Distances);

            var distances = new Dictionary<int, double>();
            recorder = new VertexDistanceRecorderObserver<int, Edge<int>>(
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
                () => new VertexDistanceRecorderObserver<int, Edge<int>>(null));

            Assert.Throws<ArgumentNullException>(
                () => new VertexDistanceRecorderObserver<int, Edge<int>>(null, DistanceRelaxers.ShortestDistance, new Dictionary<int, double>()));
            Assert.Throws<ArgumentNullException>(
                () => new VertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0, null, new Dictionary<int, double>()));
            Assert.Throws<ArgumentNullException>(
                () => new VertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0, DistanceRelaxers.ShortestDistance, null));
            Assert.Throws<ArgumentNullException>(
                () => new VertexDistanceRecorderObserver<int, Edge<int>>(null, null, new Dictionary<int, double>()));
            Assert.Throws<ArgumentNullException>(
                () => new VertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new VertexDistanceRecorderObserver<int, Edge<int>>(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Attach()
        {
            // DFS is used for tests but result may change if using another search algorithm
            // or another starting point
            {
                var recorder = new VertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0);

                var graph = new AdjacencyGraph<int, Edge<int>>();

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.IsEmpty(recorder.Distances);
                }
            }

            {
                var recorder = new VertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0);

                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.IsEmpty(recorder.Distances);
                }
            }

            {
                var recorder = new VertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0);

                // Graph without cycle
                var edge12 = new Edge<int>(1, 2);
                var edge13 = new Edge<int>(1, 3);
                var edge14 = new Edge<int>(1, 4);
                var edge24 = new Edge<int>(2, 4);
                var edge31 = new Edge<int>(3, 1);
                var edge33 = new Edge<int>(3, 3);
                var edge34 = new Edge<int>(3, 4);
                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge13, edge14, edge24, edge31, edge33, edge34
                });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEqual(
                        new Dictionary<int, double>
                        {
                            [1] = 0,
                            [2] = 1,
                            [3] = 1,
                            [4] = 2
                        },
                        recorder.Distances);
                }
            }

            {
                var recorder = new VertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0);

                // Graph with cycle
                var edge12 = new Edge<int>(1, 2);
                var edge13 = new Edge<int>(1, 3);
                var edge14 = new Edge<int>(1, 4);
                var edge24 = new Edge<int>(2, 4);
                var edge31 = new Edge<int>(3, 1);
                var edge33 = new Edge<int>(3, 3);
                var edge34 = new Edge<int>(3, 4);
                var edge41 = new Edge<int>(4, 1);
                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge13, edge14, edge24, edge31, edge33, edge34, edge41
                });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEqual(
                        new Dictionary<int, double>
                        {
                            [1] = 0,
                            [2] = 1,
                            [3] = 1,
                            [4] = 2
                        },
                        recorder.Distances);
                }
            }
        }

        [Test]
        public void Attach_Throws()
        {
            Attach_Throws_Test(new VertexDistanceRecorderObserver<int, Edge<int>>(edge => 1.0));
        }
    }
}