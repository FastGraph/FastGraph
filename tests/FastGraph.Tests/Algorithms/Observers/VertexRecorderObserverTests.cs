#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.Search;

namespace FastGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Tests for <see cref="VertexRecorderObserver{TVertex}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class VertexRecorderObserverTests : ObserverTestsBase
    {
        [Test]
        public void Constructor()
        {
            var recorder = new VertexRecorderObserver<int>();
            recorder.Vertices.Should().BeEmpty();

            recorder = new VertexRecorderObserver<int>(new[] { 1, 2, 3 });
            new[] { 1, 2, 3 }.Should().BeEquivalentTo(recorder.Vertices);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new VertexRecorderObserver<int>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void Attach()
        {
            // DFS is used for tests but result may change if using another search algorithm
            // or another starting point
            {
                var recorder = new VertexRecorderObserver<int>();

                var graph = new AdjacencyGraph<int, Edge<int>>();

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    recorder.Vertices.Should().BeEmpty();
                }
            }

            {
                var recorder = new VertexRecorderObserver<int>();

                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    new[] { 1, 2 }.Should().BeEquivalentTo(recorder.Vertices);
                }
            }

            {
                var recorder = new VertexRecorderObserver<int>(new[] { 1 });

                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    new[] { 1, 1, 2 }.Should().BeEquivalentTo(recorder.Vertices);
                }
            }

            {
                var recorder = new VertexRecorderObserver<int>();

                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVerticesAndEdgeRange(new[]
                {
                    new Edge<int>(1, 2),
                    new Edge<int>(2, 2),
                    new Edge<int>(3, 4)
                });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    new[] { 1, 2, 3, 4 }.Should().BeEquivalentTo(recorder.Vertices);
                }
            }
        }

        [Test]
        public void Attach_Throws()
        {
            Attach_Throws_Test(new VertexRecorderObserver<int>());
        }
    }
}
