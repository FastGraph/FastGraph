using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;

namespace QuikGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Tests for <see cref="EdgePredecessorRecorderObserver{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class EdgePredecessorRecorderObserverTests : ObserverTestsBase
    {
        [Test]
        public void Constructor()
        {
            var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();
            CollectionAssert.IsEmpty(recorder.EdgesPredecessors);
            CollectionAssert.IsEmpty(recorder.EndPathEdges);

            var predecessors = new Dictionary<Edge<int>, Edge<int>>();
            recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>(predecessors);
            Assert.AreSame(predecessors, recorder.EdgesPredecessors);
            CollectionAssert.IsEmpty(recorder.EndPathEdges);

            predecessors = new Dictionary<Edge<int>, Edge<int>>
            {
                [new Edge<int>(3, 2)] = new Edge<int>(2, 1)
            };
            recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>(predecessors);
            Assert.AreSame(predecessors, recorder.EdgesPredecessors);
            CollectionAssert.IsEmpty(recorder.EndPathEdges);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new EdgePredecessorRecorderObserver<int, Edge<int>>(null));
        }

        [Test]
        public void Attach()
        {
            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.IsEmpty(recorder.EdgesPredecessors);
                    CollectionAssert.IsEmpty(recorder.EndPathEdges);
                }
            }

            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();
                graph.AddVertexRange(new[] { 1, 2 });

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.IsEmpty(recorder.EdgesPredecessors);
                    CollectionAssert.IsEmpty(recorder.EndPathEdges);
                }
            }

            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

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

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEqual(
                        new Dictionary<Edge<int>, Edge<int>>
                        {
                            [edge14] = edge31,
                            [edge24] = edge12,
                            [edge31] = edge13,
                            [edge33] = edge13,
                            [edge34] = edge33
                        },
                        recorder.EdgesPredecessors);
                    CollectionAssert.AreEquivalent(
                        new[] { edge14, edge24, edge34 },
                        recorder.EndPathEdges);
                }
            }

            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

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

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEqual(
                        new Dictionary<Edge<int>, Edge<int>>
                        {
                            [edge13] = edge41,
                            [edge14] = edge31,
                            [edge24] = edge12,
                            [edge31] = edge13,
                            [edge33] = edge13,
                            [edge34] = edge33,
                            [edge41] = edge24
                        },
                        recorder.EdgesPredecessors);
                    CollectionAssert.AreEquivalent(
                        new[] { edge14, edge34 },
                        recorder.EndPathEdges);
                }
            }
        }

        [Test]
        public void Attach_Throws()
        {
            Attach_Throws_Test(new EdgePredecessorRecorderObserver<int, Edge<int>>());
        }

        [Test]
        public void Path()
        {
            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    var edge12 = new Edge<int>(1, 2);
                    // Not in the graph => return the edge itself
                    CollectionAssert.AreEqual(
                        new[] { edge12 },
                        recorder.Path(edge12));
                }
            }

            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

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

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEquivalent(
                        new[] { edge13, edge31, edge14 },
                        recorder.Path(edge14));

                    CollectionAssert.AreEquivalent(
                        new[] { edge13, edge33 },
                        recorder.Path(edge33));
                }
            }

            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

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

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEquivalent(
                        new[] { edge12, edge24, edge41, edge13, edge31, edge14 },
                        recorder.Path(edge14));

                    CollectionAssert.AreEquivalent(
                        new[] { edge12, edge24, edge41, edge13, edge33 },
                        recorder.Path(edge33));
                }
            }
        }

        [Test]
        public void Path_Throws()
        {
            var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => recorder.Path(null));
        }

        [Test]
        public void AllPaths()
        {
            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.IsEmpty(recorder.AllPaths());
                }
            }

            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

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

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEquivalent(
                        new IEnumerable<Edge<int>>[]
                        {
                            new[] { edge12, edge24 },
                            new[] { edge13, edge31, edge14 },
                            new[] { edge13, edge33, edge34 }
                        },
                        recorder.AllPaths());
                }
            }

            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

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

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEquivalent(
                        new IEnumerable<Edge<int>>[]
                        {
                            new[] { edge12, edge24, edge41, edge13, edge31, edge14 },
                            new[] { edge12, edge24, edge41, edge13, edge33, edge34 }
                        },
                        recorder.AllPaths());
                }
            }
        }

        [Test]
        public void MergedPath()
        {
            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    var edge12 = new Edge<int>(1, 2);
                    var colors = new Dictionary<Edge<int>, GraphColor>
                    {
                        [edge12] = GraphColor.Black
                    };

                    // Not in the graph and edge marked as already used!
                    CollectionAssert.IsEmpty(recorder.MergedPath(edge12, colors));

                    // Not in the graph => return the edge itself
                    colors[edge12] = GraphColor.White;
                    CollectionAssert.AreEqual(
                        new[] { edge12 },
                        recorder.MergedPath(edge12, colors));
                }
            }

            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

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

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    Dictionary<Edge<int>, GraphColor> colors = graph.Edges.ToDictionary(
                        edge => edge,
                        edge => GraphColor.White);

                    CollectionAssert.AreEqual(
                        new[] { edge12, edge24 },
                        recorder.MergedPath(edge24, colors));

                    // Already used
                    CollectionAssert.IsEmpty(recorder.MergedPath(edge24, colors));

                    CollectionAssert.AreEqual(
                        new[] { edge13, edge31 },
                        recorder.MergedPath(edge31, colors));
                }
            }

            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

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

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    Dictionary<Edge<int>, GraphColor> colors = graph.Edges.ToDictionary(
                        edge => edge,
                        edge => GraphColor.White);

                    CollectionAssert.AreEqual(
                        new[] { edge12, edge24, edge41 },
                        recorder.MergedPath(edge41, colors));

                    // Already used
                    CollectionAssert.IsEmpty(recorder.MergedPath(edge41, colors));

                    CollectionAssert.AreEqual(
                        new[] { edge13, edge33, edge34 },
                        recorder.MergedPath(edge34, colors));
                }
            }
        }

        [Test]
        public void MergedPath_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();
            Assert.Throws<ArgumentNullException>(
                () => recorder.MergedPath(null, new Dictionary<Edge<int>, GraphColor>()));
            Assert.Throws<ArgumentNullException>(
                () => recorder.MergedPath(new Edge<int>(1, 2), null));
            Assert.Throws<ArgumentNullException>(
                () => recorder.MergedPath(null, null));
            // ReSharper restore AssignNullToNotNullAttribute

            var edge = new Edge<int>(1, 2);
            Assert.Throws<KeyNotFoundException>(
                () => recorder.MergedPath(edge, new Dictionary<Edge<int>, GraphColor>()));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void AllMergedPath()
        {
            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

                var graph = new AdjacencyGraph<int, Edge<int>>();

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.IsEmpty(recorder.AllMergedPaths());
                }
            }

            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

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

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEquivalent(
                        new IEnumerable<Edge<int>>[]
                        {
                            new[] { edge12, edge24 },
                            new[] { edge13, edge31, edge14 },
                            new[] { /* edge13 can't be reused */ edge33, edge34 }
                        },
                        recorder.AllMergedPaths());
                }
            }

            {
                var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

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

                var dfs = new EdgeDepthFirstSearchAlgorithm<int, Edge<int>>(graph);
                using (recorder.Attach(dfs))
                {
                    dfs.Compute();

                    CollectionAssert.AreEquivalent(
                        new IEnumerable<Edge<int>>[]
                        {
                            new[] { edge12, edge24, edge41, edge13, edge31, edge14 },
                            new[] { /* edge12, edge24, edge41, edge13 can't be reused */ edge33, edge34 }
                        },
                        recorder.AllMergedPaths());
                }
            }
        }
    }
}