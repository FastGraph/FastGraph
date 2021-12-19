#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.Search;

namespace FastGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Tests for <see cref="UndirectedVertexPredecessorRecorderObserver{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class UndirectedVertexPredecessorRecorderObserverTests : ObserverTestsBase
    {
        [Test]
        public void Constructor()
        {
            var recorder = new UndirectedVertexPredecessorRecorderObserver<int, Edge<int>>();
            recorder.VerticesPredecessors.Should().BeEmpty();

            var predecessors = new Dictionary<int, Edge<int>>();
            recorder = new UndirectedVertexPredecessorRecorderObserver<int, Edge<int>>(predecessors);
            recorder.VerticesPredecessors.Should().BeSameAs(predecessors);

            predecessors = new Dictionary<int, Edge<int>>
            {
                [1] = new Edge<int>(2, 1)
            };
            recorder = new UndirectedVertexPredecessorRecorderObserver<int, Edge<int>>(predecessors);
            recorder.VerticesPredecessors.Should().BeSameAs(predecessors);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new UndirectedVertexPredecessorRecorderObserver<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void Attach()
        {
            // Undirected DFS is used for tests but result may change if using another search algorithm
            // or another starting point
            {
                var recorder = new UndirectedVertexPredecessorRecorderObserver<int, Edge<int>>();

                var graph = new UndirectedGraph<int, Edge<int>>();

                var dfs = new UndirectedDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    recorder.VerticesPredecessors.Should().BeEmpty();
                }
            }

            {
                var recorder = new UndirectedVertexPredecessorRecorderObserver<int, Edge<int>>();

                var graph = new UndirectedGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new UndirectedDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    recorder.VerticesPredecessors.Should().BeEmpty();
                }
            }

            {
                var recorder = new UndirectedVertexPredecessorRecorderObserver<int, Edge<int>>();

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

                    new Dictionary<int, Edge<int>>
                    {
                        [2] = edge12,
                        [3] = edge34,
                        [4] = edge42
                    }.Should().BeEquivalentTo(recorder.VerticesPredecessors);
                }
            }
        }

        [Test]
        public void Attach_Throws()
        {
            Attach_Throws_Test(new UndirectedVertexPredecessorRecorderObserver<int, Edge<int>>());
        }

        [Test]
        public void TryGetPath()
        {
            {
                var recorder = new UndirectedVertexPredecessorRecorderObserver<int, Edge<int>>();

                var graph = new UndirectedGraph<int, Edge<int>>();

                var dfs = new UndirectedDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    // Vertex not in the graph
                    recorder.TryGetPath(2, out _).Should().BeFalse();
                }
            }

            {
                var recorder = new UndirectedVertexPredecessorRecorderObserver<int, Edge<int>>();

                var graph = new UndirectedGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new UndirectedDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    recorder.TryGetPath(2, out _).Should().BeFalse();
                }
            }

            {
                var recorder = new UndirectedVertexPredecessorRecorderObserver<int, Edge<int>>();

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

                    recorder.TryGetPath(4, out IEnumerable<Edge<int>>? path).Should().BeTrue();
                    new[] { edge12, edge42 }.Should().BeEquivalentTo(path);
                }
            }
        }

        [Test]
        public void TryGetPath_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new UndirectedVertexPredecessorRecorderObserver<TestVertex, Edge<TestVertex>>().TryGetPath(default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }
    }
}
