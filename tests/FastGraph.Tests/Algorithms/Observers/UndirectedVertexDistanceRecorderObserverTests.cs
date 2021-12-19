#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.Search;

namespace FastGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Tests for <see cref="UndirectedVertexDistanceRecorderObserver{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class UndirectedVertexDistanceRecorderObserverTests : ObserverTestsBase
    {
        [Test]
        public void Constructor()
        {
            Func<Edge<int>, double> edgeWeights = _ => 1.0;
            var recorder = new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(edgeWeights);
            recorder.EdgeWeights.Should().BeSameAs(edgeWeights);
            recorder.DistanceRelaxer.Should().NotBeNull();
            recorder.Distances.Should().NotBeNull();

            var distances = new Dictionary<int, double>();
            recorder = new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(
                edgeWeights,
                DistanceRelaxers.ShortestDistance,
                distances);
            recorder.EdgeWeights.Should().BeSameAs(edgeWeights);
            recorder.DistanceRelaxer.Should().BeSameAs(DistanceRelaxers.ShortestDistance);
            recorder.Distances.Should().BeSameAs(distances);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(default, DistanceRelaxers.ShortestDistance, new Dictionary<int, double>())).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(_ => 1.0, default, new Dictionary<int, double>())).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(_ => 1.0, DistanceRelaxers.ShortestDistance, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(default, default, new Dictionary<int, double>())).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(_ => 1.0, default, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(default, default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Attach()
        {
            // Undirected DFS is used for tests but result may change if using another search algorithm
            // or another starting point
            {
                var recorder = new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(_ => 1.0);

                var graph = new UndirectedGraph<int, Edge<int>>();

                var dfs = new UndirectedDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    recorder.Distances.Should().BeEmpty();
                }
            }

            {
                var recorder = new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(_ => 1.0);

                var graph = new UndirectedGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new UndirectedDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    recorder.Distances.Should().BeEmpty();
                }
            }

            {
                var recorder = new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(_ => 1.0);

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

                    new Dictionary<int, double>
                    {
                        [1] = 0,
                        [2] = 1,
                        [3] = 3,
                        [4] = 2
                    }.Should().BeEquivalentTo(recorder.Distances);
                }
            }
        }

        [Test]
        public void Attach_Throws()
        {
            Attach_Throws_Test(new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(_ => 1.0));
        }
    }
}
