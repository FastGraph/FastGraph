using System;
using NUnit.Framework;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;

namespace QuikGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Tests for <see cref="EdgeRecorderObserver{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class EdgeRecorderObserverTests : ObserverTestsBase
    {
        [Test]
        public void Constructor()
        {
            var recorder = new EdgeRecorderObserver<int, Edge<int>>();
            CollectionAssert.IsEmpty(recorder.Edges);

            var edge12 = new Edge<int>(1, 2);
            var edge22 = new Edge<int>(2, 2);
            var edge31 = new Edge<int>(3, 1);
            recorder = new EdgeRecorderObserver<int, Edge<int>>(new[]
            {
                edge12, edge22, edge31
            });
            CollectionAssert.AreEqual(
                new[] { edge12, edge22, edge31 },
                recorder.Edges);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new EdgeRecorderObserver<int, Edge<int>>(null));
        }

        [Test]
        public void Attach()
        {
            // DFS is used for tests but result may change if using another search algorithm
            // or another starting point
            {
                var recorder = new EdgeRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.IsEmpty(recorder.Edges);
                }
            }

            {
                var recorder = new EdgeRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.IsEmpty(recorder.Edges);
                }
            }

            {
                var edge12 = new Edge<int>(1, 2);
                var recorder = new EdgeRecorderObserver<int, Edge<int>>(new[] { edge12 });

                var edge23 = new Edge<int>(2, 3);
                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEqual(
                        new[] { edge12, edge12, edge23 },  // Add without checking if edge already exists
                        recorder.Edges);
                }
            }

            {
                var recorder = new EdgeRecorderObserver<int, Edge<int>>();

                var edge12 = new Edge<int>(1, 2);
                var edge32 = new Edge<int>(3, 2);   // Is not reachable
                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVerticesAndEdgeRange(new[] { edge12, edge32 });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEqual(
                        new[] { edge12 },  // 3 -> 2 is not reachable (wrong orientation)
                        recorder.Edges);
                }
            }

            {
                var recorder = new EdgeRecorderObserver<int, Edge<int>>();

                var edge12 = new Edge<int>(1, 2);
                var edge22 = new Edge<int>(2, 2);
                var edge23 = new Edge<int>(2, 3);
                var edge34 = new Edge<int>(3, 4);
                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge22, edge23, edge34
                });

                var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEqual(
                        new[] { edge12, edge23, edge34 },   // Self edge skipped
                        recorder.Edges);
                }
            }
        }

        [Test]
        public void Attach_Throws()
        {
            Attach_Throws_Test(new EdgeRecorderObserver<int, Edge<int>>());
        }
    }
}