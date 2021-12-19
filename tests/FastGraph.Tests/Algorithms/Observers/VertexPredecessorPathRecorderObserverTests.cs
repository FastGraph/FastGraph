#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.Search;

namespace FastGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Tests for <see cref="VertexPredecessorPathRecorderObserver{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class VertexPredecessorPathRecorderObserverTests : ObserverTestsBase
    {
        [Test]
        public void Constructor()
        {
            var recorder = new VertexPredecessorPathRecorderObserver<int, Edge<int>>();
            recorder.VerticesPredecessors.Should().BeEmpty();
            recorder.EndPathVertices.Should().BeEmpty();

            var predecessors = new Dictionary<int, Edge<int>>();
            recorder = new VertexPredecessorPathRecorderObserver<int, Edge<int>>(predecessors);
            recorder.VerticesPredecessors.Should().BeSameAs(predecessors);
            recorder.EndPathVertices.Should().BeEmpty();

            predecessors = new Dictionary<int, Edge<int>>
            {
                [1] = new Edge<int>(2, 1)
            };
            recorder = new VertexPredecessorPathRecorderObserver<int, Edge<int>>(predecessors);
            recorder.VerticesPredecessors.Should().BeSameAs(predecessors);
            recorder.EndPathVertices.Should().BeEmpty();
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new VertexPredecessorPathRecorderObserver<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void Attach()
        {
            // DFS is used for tests but result may change if using another search algorithm
            // or another starting point
            {
                var recorder = new VertexPredecessorPathRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    recorder.VerticesPredecessors.Should().BeEmpty();
                    recorder.EndPathVertices.Should().BeEmpty();
                }
            }

            {
                var recorder = new VertexPredecessorPathRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    recorder.VerticesPredecessors.Should().BeEmpty();
                    recorder.EndPathVertices.Should().BeEquivalentTo(new[] { 1, 2 });
                }
            }

            {
                var recorder = new VertexPredecessorPathRecorderObserver<int, Edge<int>>();

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

                    recorder.VerticesPredecessors.Should().BeEquivalentTo(
                        new Dictionary<int, Edge<int>>
                        {
                            [2] = edge12,
                            [3] = edge13,
                            [4] = edge24
                        });
                    recorder.EndPathVertices.Should().BeEquivalentTo(new[] { 3, 4 });
                }
            }

            {
                var recorder = new VertexPredecessorPathRecorderObserver<int, Edge<int>>();

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

                    recorder.VerticesPredecessors.Should().BeEquivalentTo(
                        new Dictionary<int, Edge<int>>
                        {
                            [2] = edge12,
                            [3] = edge13,
                            [4] = edge24
                        });
                    recorder.EndPathVertices.Should().BeEquivalentTo(new[] { 3, 4 });
                }
            }
        }

        [Test]
        public void Attach_Throws()
        {
            Attach_Throws_Test(new VertexPredecessorPathRecorderObserver<int, Edge<int>>());
        }

        [Test]
        public void AllPaths()
        {
            {
                var recorder = new VertexPredecessorPathRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    recorder.AllPaths().Should().BeEmpty();
                }
            }

            {
                var recorder = new VertexPredecessorPathRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    recorder.AllPaths().Should().BeEmpty();
                }
            }

            {
                var recorder = new VertexPredecessorPathRecorderObserver<int, Edge<int>>();

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

                    recorder.AllPaths().Should().BeEquivalentTo(new IEnumerable<Edge<int>>[]
                    {
                        new[] { edge13 },
                        new[] { edge12, edge24 }
                    });
                }
            }

            {
                var recorder = new VertexPredecessorPathRecorderObserver<int, Edge<int>>();

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

                    recorder.AllPaths().Should().BeEquivalentTo(new IEnumerable<Edge<int>>[]
                    {
                        new[] { edge13 },
                        new[] { edge12, edge24 }
                    });
                }
            }
        }
    }
}
