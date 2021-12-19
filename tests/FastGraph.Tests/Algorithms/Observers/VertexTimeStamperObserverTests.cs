#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.Search;

namespace FastGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Tests for <see cref="VertexTimeStamperObserver{TVertex}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class VertexTimeStamperObserverTests : ObserverTestsBase
    {
        [Test]
        public void Constructor()
        {
            var recorder = new VertexTimeStamperObserver<int>();
            recorder.DiscoverTimes.Should().BeEmpty();
            recorder.FinishTimes.Should().BeEmpty();

            var discoverTimes = new Dictionary<int, int>();
            recorder = new VertexTimeStamperObserver<int>(discoverTimes);
            recorder.DiscoverTimes.Should().BeSameAs(discoverTimes);
            recorder.FinishTimes.Should().BeNull();

            discoverTimes = new Dictionary<int, int>
            {
                [1] = 0,
                [2] = 1,
                [3] = 3,
                [4] = 2
            };
            recorder = new VertexTimeStamperObserver<int>(discoverTimes);
            recorder.DiscoverTimes.Should().BeSameAs(discoverTimes);
            recorder.FinishTimes.Should().BeNull();

            discoverTimes = new Dictionary<int, int>();
            var finishTimes = new Dictionary<int, int>();
            recorder = new VertexTimeStamperObserver<int>(
                discoverTimes,
                finishTimes);
            recorder.DiscoverTimes.Should().BeSameAs(discoverTimes);
            recorder.FinishTimes.Should().BeSameAs(finishTimes);

            discoverTimes = new Dictionary<int, int>
            {
                [1] = 0,
                [2] = 1,
                [3] = 3,
                [4] = 2
            };
            finishTimes = new Dictionary<int, int>
            {
                [1] = 4,
                [2] = 6,
                [3] = 5,
                [4] = 7
            };
            recorder = new VertexTimeStamperObserver<int>(
                discoverTimes,
                finishTimes);
            discoverTimes.Should().BeEquivalentTo(recorder.DiscoverTimes);
            finishTimes.Should().BeEquivalentTo(recorder.FinishTimes);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new VertexTimeStamperObserver<int>(default)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                new VertexTimeStamperObserver<int>(new Dictionary<int, int>(), default)).Should().Throw<ArgumentNullException>();
            Invoking(() =>
                new VertexTimeStamperObserver<int>(default, new Dictionary<int, int>())).Should().Throw<ArgumentNullException>();
            Invoking(() => new VertexTimeStamperObserver<int>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore ObjectCreationAsStatement
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void Attach()
        {
            // DFS is used for tests but result may change if using another search algorithm
            // or another starting point
            {
                var recorder = new VertexTimeStamperObserver<int>();

                var graph = new AdjacencyGraph<int, Edge<int>>();

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    recorder.DiscoverTimes.Should().BeEmpty();
                    recorder.FinishTimes.Should().BeEmpty();
                }
            }

            {
                var recorder = new VertexTimeStamperObserver<int>();

                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    new Dictionary<int, int>
                    {
                        [1] = 0,
                        [2] = 2
                    }.Should().BeEquivalentTo(recorder.DiscoverTimes);

                    new Dictionary<int, int>
                    {
                        [1] = 1,
                        [2] = 3
                    }.Should().BeEquivalentTo(recorder.FinishTimes);
                }
            }

            {
                var recorder = new VertexTimeStamperObserver<int>();

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

                    new Dictionary<int, int>
                    {
                        [1] = 0,
                        [2] = 1,
                        [3] = 4,
                        [4] = 5
                    }.Should().BeEquivalentTo(recorder.DiscoverTimes);

                    new Dictionary<int, int>
                    {
                        [1] = 3,
                        [2] = 2,
                        [3] = 7,
                        [4] = 6
                    }.Should().BeEquivalentTo(recorder.FinishTimes);
                }
            }
        }

        [Test]
        public void Attach_Throws()
        {
            Attach_Throws_Test(new VertexTimeStamperObserver<int>());
        }
    }
}
