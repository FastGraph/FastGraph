#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.Search;

namespace FastGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Tests for <see cref="EdgePredecessorRecorderObserver{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class EdgePredecessorRecorderObserverTests : ObserverTestsBase
    {
        [Test]
        public void Constructor()
        {
            var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();
            recorder.EdgesPredecessors.Should().BeEmpty();
            recorder.EndPathEdges.Should().BeEmpty();

            var predecessors = new Dictionary<Edge<int>, Edge<int>>();
            recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>(predecessors);
            recorder.EdgesPredecessors.Should().BeSameAs(predecessors);
            recorder.EndPathEdges.Should().BeEmpty();

            predecessors = new Dictionary<Edge<int>, Edge<int>>
            {
                [new Edge<int>(3, 2)] = new Edge<int>(2, 1)
            };
            recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>(predecessors);
            recorder.EdgesPredecessors.Should().BeSameAs(predecessors);
            recorder.EndPathEdges.Should().BeEmpty();
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Invoking(() => new EdgePredecessorRecorderObserver<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
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

                    recorder.EdgesPredecessors.Should().BeEmpty();
                    recorder.EndPathEdges.Should().BeEmpty();
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

                    recorder.EdgesPredecessors.Should().BeEmpty();
                    recorder.EndPathEdges.Should().BeEmpty();
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

                    recorder.EdgesPredecessors.Should().BeEquivalentTo(
                        new Dictionary<Edge<int>, Edge<int>>
                        {
                            [edge14] = edge31,
                            [edge24] = edge12,
                            [edge31] = edge13,
                            [edge33] = edge13,
                            [edge34] = edge33
                        });
                    recorder.EndPathEdges.Should().BeEquivalentTo(new[] { edge14, edge24, edge34 });
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

                    recorder.EdgesPredecessors.Should().BeEquivalentTo(
                        new Dictionary<Edge<int>, Edge<int>>
                        {
                            [edge13] = edge41,
                            [edge14] = edge31,
                            [edge24] = edge12,
                            [edge31] = edge13,
                            [edge33] = edge13,
                            [edge34] = edge33,
                            [edge41] = edge24
                        });
                    recorder.EndPathEdges.Should().BeEquivalentTo(new[] { edge14, edge34 });
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
                    recorder.Path(edge12).Should().BeEquivalentTo(new[] { edge12 });
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

                    recorder.Path(edge14).Should().BeEquivalentTo(new[] { edge13, edge31, edge14 });

                    recorder.Path(edge33).Should().BeEquivalentTo(new[] { edge13, edge33 });
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

                    recorder.Path(edge14).Should().BeEquivalentTo(new[] { edge12, edge24, edge41, edge13, edge31, edge14 });

                    recorder.Path(edge33).Should().BeEquivalentTo(new[] { edge12, edge24, edge41, edge13, edge33 });
                }
            }
        }

        [Test]
        public void Path_Throws()
        {
            var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Invoking(() => recorder.Path(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
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

                    recorder.AllPaths().Should().BeEmpty();
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

                    recorder.AllPaths().Should().BeEquivalentTo(new []
                    {
                        new[] { edge12, edge24 },
                        new[] { edge13, edge31, edge14 },
                        new[] { edge13, edge33, edge34 }
                    });
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

                    recorder.AllPaths().Should().BeEquivalentTo(new []
                    {
                        new[] { edge12, edge24, edge41, edge13, edge31, edge14 },
                        new[] { edge12, edge24, edge41, edge13, edge33, edge34 }
                    });
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
                    recorder.MergedPath(edge12, colors).Should().BeEmpty();

                    // Not in the graph => return the edge itself
                    colors[edge12] = GraphColor.White;
                    recorder.MergedPath(edge12, colors).Should().BeEquivalentTo(new[] { edge12 });
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

                    var colors = graph.Edges.ToDictionary(
                        edge => edge,
                        _ => GraphColor.White);

                    new[] { edge12, edge24 }.Should().BeEquivalentTo(recorder.MergedPath(edge24, colors));

                    // Already used
                    recorder.MergedPath(edge24, colors).Should().BeEmpty();

                    new[] { edge13, edge31 }.Should().BeEquivalentTo(recorder.MergedPath(edge31, colors));
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

                    var colors = graph.Edges.ToDictionary(
                        edge => edge,
                        _ => GraphColor.White);

                    new[] { edge12, edge24, edge41 }.Should().BeEquivalentTo(recorder.MergedPath(edge41, colors));

                    // Already used
                    recorder.MergedPath(edge41, colors).Should().BeEmpty();

                    new[] { edge13, edge33, edge34 }.Should().BeEquivalentTo(recorder.MergedPath(edge34, colors));
                }
            }
        }

        [Test]
        public void MergedPath_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var recorder = new EdgePredecessorRecorderObserver<int, Edge<int>>();
            Invoking(() => recorder.MergedPath(default, new Dictionary<Edge<int>, GraphColor>())).Should().Throw<ArgumentNullException>();
            Invoking(() => recorder.MergedPath(new Edge<int>(1, 2), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => recorder.MergedPath(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            // ReSharper restore AssignNullToNotNullAttribute

            var edge = new Edge<int>(1, 2);
            Invoking(() => recorder.MergedPath(edge, new Dictionary<Edge<int>, GraphColor>())).Should().Throw<KeyNotFoundException>();
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

                    recorder.AllMergedPaths().Should().BeEmpty();
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

                    recorder.AllMergedPaths().Should().BeEquivalentTo(new IEnumerable<Edge<int>>[]
                    {
                        new[] { edge12, edge24 },
                        new[] { edge13, edge31, edge14 },
                        new[] { /* edge13 can't be reused */ edge33, edge34 }
                    });
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

                    recorder.AllMergedPaths().Should().BeEquivalentTo(new IEnumerable<Edge<int>>[]
                    {
                        new[] { edge12, edge24, edge41, edge13, edge31, edge14 },
                        new[] { /* edge12, edge24, edge41, edge13 can't be reused */ edge33, edge34 }
                    });
                }
            }
        }
    }
}
