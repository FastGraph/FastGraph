#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="EulerianTrailAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class EulerianTrailAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void ComputeTrails<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [InstantHandle] Func<TVertex, TVertex, TEdge> edgeFactory,
            out ICollection<TEdge>[] trails,
            out TEdge[] circuit)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            trails = new ICollection<TEdge>[0];
            circuit = new TEdge[0];

            int circuitCount = EulerianTrailAlgorithm<TVertex, TEdge>.ComputeEulerianPathCount(graph);
            if (circuitCount == 0)
                return;

            var algorithm = new EulerianTrailAlgorithm<TVertex, TEdge>(graph);
            algorithm.AddTemporaryEdges((s, t) => edgeFactory(s, t));
            algorithm.Compute();
            trails = algorithm.Trails().ToArray();
            algorithm.RemoveTemporaryEdges();
            algorithm.Circuit.Should().NotBeNull();
            circuit = algorithm.Circuit;

            // Lets make sure all the edges are in the trail
            var edges = new HashSet<TEdge>();
            foreach (TEdge edge in graph.Edges)
                edges.Add(edge).Should().BeTrue();

            foreach (ICollection<TEdge> trail in trails)
            {
                edges.Count.Should().Be(graph.EdgeCount);
                trail.Should().OnlyContain(edge => edges.Contains(edge));
            }
        }

        private static void ComputeTrails<TVertex, TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> graph,
            TVertex root,
            [InstantHandle] Func<TVertex, TVertex, TEdge> edgeFactory,
            out ICollection<TEdge>[] trails,
            out TEdge[] circuit)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            trails = new ICollection<TEdge>[0];
            circuit = new TEdge[0];

            int circuitCount = EulerianTrailAlgorithm<TVertex, TEdge>.ComputeEulerianPathCount(graph);
            if (circuitCount == 0)
                return;

            TEdge[] graphEdges = graph.Edges.ToArray();

            var algorithm = new EulerianTrailAlgorithm<TVertex, TEdge>(graph);
            algorithm.AddTemporaryEdges((s, t) => edgeFactory(s, t));
            TEdge[] augmentedGraphEdges = graph.Edges.ToArray();
            augmentedGraphEdges.Length.Should().BeGreaterThanOrEqualTo(graphEdges.Length);
            TEdge[] temporaryEdges = augmentedGraphEdges.Except(graphEdges).ToArray();
            temporaryEdges.Length.Should().Be(augmentedGraphEdges.Length - graphEdges.Length);

            algorithm.Compute();
            trails = algorithm.Trails(root).ToArray();
            algorithm.RemoveTemporaryEdges();
            algorithm.Circuit.Should().NotBeNull();
            circuit = algorithm.Circuit;

            // Lets make sure all the edges are in the trail
            var edges = new HashSet<TEdge>();
            foreach (TEdge edge in graph.Edges)
                edges.Add(edge).Should().BeTrue();

            foreach (ICollection<TEdge> trail in trails)
            {
                edges.Count.Should().Be(graph.EdgeCount);
                trail.Should().OnlyContain( // Edge in graph or part of temporary ones but is a root
                    edge => edges.Contains(edge)
                            || temporaryEdges.Contains(edge) && Equals(edge.Source, root));
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();

            var algorithm = new EulerianTrailAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new EulerianTrailAlgorithm<int, Edge<int>>(default, graph);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                EulerianTrailAlgorithm<TVertex, TEdge> algo,
                IMutableVertexAndEdgeListGraph<TVertex, TEdge> g)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.Circuit.Should().BeEmpty();
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new EulerianTrailAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new EulerianTrailAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new EulerianTrailAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new EulerianTrailAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new EulerianTrailAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new EulerianTrailAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_NoThrows_Test(
                graph,
                () => new EulerianTrailAlgorithm<int, Edge<int>>(graph));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new EulerianTrailAlgorithm<int, Edge<int>>(graph);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new EulerianTrailAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        private static IEnumerable<TestCaseData> ComputeEulerianPathCountTestCases
        {
            [UsedImplicitly]
            get
            {
                var emptyGraph = new AdjacencyGraph<int, Edge<int>>();
                yield return new TestCaseData(emptyGraph)
                {
                    ExpectedResult = 1
                };

                var moreVerticesThanEdgesGraph = new AdjacencyGraph<int, Edge<int>>();
                moreVerticesThanEdgesGraph.AddVertexRange(new[] { 1, 2 });
                moreVerticesThanEdgesGraph.AddEdge(new Edge<int>(1, 2));
                yield return new TestCaseData(moreVerticesThanEdgesGraph)
                {
                    ExpectedResult = 0
                };

                var sameVerticesAndEdgesCountGraph = new AdjacencyGraph<int, Edge<int>>();
                sameVerticesAndEdgesCountGraph.AddVertexRange(new[] { 1, 2 });
                sameVerticesAndEdgesCountGraph.AddEdgeRange(new[]
                {
                    new Edge<int>(1, 2),
                    new Edge<int>(2, 1)
                });
                yield return new TestCaseData(sameVerticesAndEdgesCountGraph)
                {
                    ExpectedResult = 1
                };

                var sameVerticesAndEdgesCountGraph2 = new AdjacencyGraph<int, Edge<int>>();
                sameVerticesAndEdgesCountGraph2.AddVertexRange(new[] { 1, 2, 3 });
                sameVerticesAndEdgesCountGraph2.AddEdgeRange(new[]
                {
                    new Edge<int>(1, 2),
                    new Edge<int>(2, 1),
                    new Edge<int>(1, 3)
                });
                yield return new TestCaseData(sameVerticesAndEdgesCountGraph2)
                {
                    ExpectedResult = 1
                };

                var moreEdgesThanEdgesGraph = new AdjacencyGraph<int, Edge<int>>();
                moreEdgesThanEdgesGraph.AddVertexRange(new[] { 1, 2, 3, 4, 5 });
                moreEdgesThanEdgesGraph.AddEdgeRange(new[]
                {
                    new Edge<int>(1, 2),
                    new Edge<int>(2, 1),
                    new Edge<int>(1, 3),
                    new Edge<int>(1, 4),
                    new Edge<int>(3, 4),
                    new Edge<int>(3, 4),
                    new Edge<int>(1, 5)
                });
                yield return new TestCaseData(moreEdgesThanEdgesGraph)
                {
                    ExpectedResult = 2
                };
            }
        }

        [TestCaseSource(nameof(ComputeEulerianPathCountTestCases))]
        public int ComputeEulerianPathCount(AdjacencyGraph<int, Edge<int>> graph)
        {
            return EulerianTrailAlgorithm<int, Edge<int>>.ComputeEulerianPathCount(graph);
        }

        [Test]
        public void ComputeEulerianPathCount_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => EulerianTrailAlgorithm<int, Edge<int>>.ComputeEulerianPathCount(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        private static IEnumerable<TestCaseData> AddTemporaryEdgesTestCases
        {
            [UsedImplicitly]
            get
            {
                var emptyGraph = new AdjacencyGraph<int, EquatableEdge<int>>();
                yield return new TestCaseData(emptyGraph, new EquatableEdge<int>[0]);


                var evenVerticesGraph = new AdjacencyGraph<int, EquatableEdge<int>>();
                evenVerticesGraph.AddVertexRange(new[] { 1, 2, 3, 4 });
                evenVerticesGraph.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 3),
                    new EquatableEdge<int>(2, 4),
                    new EquatableEdge<int>(3, 4)
                });
                yield return new TestCaseData(evenVerticesGraph, new EquatableEdge<int>[0]);


                var oddVerticesGraph1 = new AdjacencyGraph<int, EquatableEdge<int>>();
                oddVerticesGraph1.AddVertexRange(new[] { 1, 2, 3 });
                oddVerticesGraph1.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(1, 3)
                });
                yield return new TestCaseData(
                    oddVerticesGraph1,
                    new[]
                    {
                        new EquatableEdge<int>(1, 3)
                    });


                var oddVerticesGraph2 = new AdjacencyGraph<int, EquatableEdge<int>>();
                oddVerticesGraph2.AddVertexRange(new[] { 1, 2, 3, 4, 5 });
                oddVerticesGraph2.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(1, 4),
                    new EquatableEdge<int>(3, 1),
                    new EquatableEdge<int>(1, 5)
                });
                yield return new TestCaseData(
                    oddVerticesGraph2,
                    new[]
                    {
                        new EquatableEdge<int>(1, 4),
                        new EquatableEdge<int>(3, 5)
                    });


                var oddVerticesGraph3 = new AdjacencyGraph<int, EquatableEdge<int>>();
                oddVerticesGraph3.AddVertexRange(new[] { 1, 2, 3, 4, 5 });
                oddVerticesGraph3.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(1, 3),
                    new EquatableEdge<int>(1, 4),
                    new EquatableEdge<int>(3, 4),
                    new EquatableEdge<int>(3, 4),
                    new EquatableEdge<int>(1, 5)
                });
                yield return new TestCaseData(
                    oddVerticesGraph3,
                    new[]
                    {
                        new EquatableEdge<int>(1, 3),
                        new EquatableEdge<int>(4, 5)
                    });
            }
        }

        [TestCaseSource(nameof(AddTemporaryEdgesTestCases))]
        public void AddTemporaryEdges(
            AdjacencyGraph<int, EquatableEdge<int>> graph,
            EquatableEdge<int>[] expectedTemporaryEdges)
        {
            var algorithm = new EulerianTrailAlgorithm<int, EquatableEdge<int>>(graph);
            int edgeCount = graph.EdgeCount;
            EquatableEdge<int>[] tmpEdges = algorithm.AddTemporaryEdges(
                (source, target) => new EquatableEdge<int>(source, target));
            tmpEdges.Should().BeEquivalentTo(expectedTemporaryEdges);

            graph.EdgeCount.Should().Be(edgeCount + tmpEdges.Length);
            EquatableEdge<int>[] graphEdges = graph.Edges.ToArray();
            foreach (EquatableEdge<int> edge in tmpEdges)
            {
                graphEdges.Should().Contain(edge);
            }
        }

        [Test]
        public void AddTemporaryEdges_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new EulerianTrailAlgorithm<int, Edge<int>>(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm.AddTemporaryEdges(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        private static IEnumerable<TestCaseData> RemoveTemporaryEdgesTestCases
        {
            [UsedImplicitly]
            get
            {
                var emptyGraph = new AdjacencyGraph<int, EquatableEdge<int>>();
                yield return new TestCaseData(emptyGraph);


                var evenVerticesGraph = new AdjacencyGraph<int, EquatableEdge<int>>();
                evenVerticesGraph.AddVertexRange(new[] { 1, 2, 3, 4 });
                evenVerticesGraph.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(1, 3),
                    new EquatableEdge<int>(2, 4),
                    new EquatableEdge<int>(3, 4)
                });
                yield return new TestCaseData(evenVerticesGraph);


                var oddVerticesGraph1 = new AdjacencyGraph<int, EquatableEdge<int>>();
                oddVerticesGraph1.AddVertexRange(new[] { 1, 2, 3 });
                oddVerticesGraph1.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(1, 3)
                });
                yield return new TestCaseData(oddVerticesGraph1);


                var oddVerticesGraph2 = new AdjacencyGraph<int, EquatableEdge<int>>();
                oddVerticesGraph2.AddVertexRange(new[] { 1, 2, 3, 4, 5 });
                oddVerticesGraph2.AddEdgeRange(new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(1, 4),
                    new EquatableEdge<int>(3, 1),
                    new EquatableEdge<int>(1, 5)
                });
                yield return new TestCaseData(oddVerticesGraph2);
            }
        }

        [TestCaseSource(nameof(RemoveTemporaryEdgesTestCases))]
        public void RemoveTemporaryEdges(AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            var algorithm = new EulerianTrailAlgorithm<int, EquatableEdge<int>>(graph);
            int edgeCount = graph.EdgeCount;
            EquatableEdge<int>[] tmpEdges = algorithm.AddTemporaryEdges(
                (source, target) => new EquatableEdge<int>(source, target));
            graph.EdgeCount.Should().Be(edgeCount + tmpEdges.Length);

            algorithm.RemoveTemporaryEdges();
            graph.EdgeCount.Should().Be(edgeCount);
        }

        #region Trails

        [TestCaseSource(nameof(LoadGraph_G_42_34))]
        public void NotEulerianTrailGraph(TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string> testGraph)
        {
            var graph = testGraph.Instance;

            // No trails in tests graphs there
            ComputeTrails(
                graph,
                (s, t) => new Edge<string>(s, t),
                out ICollection<Edge<string>>[] trails,
                out Edge<string>[] circuit);
            trails.Should().BeEmpty();
            circuit.Should().BeEmpty();
        }

        [Test]
        public void SingleEulerianTrailGraph()
        {
            var edge1 = new Edge<char>('b', 'c');
            var edge2 = new Edge<char>('f', 'a');
            var edge3 = new Edge<char>('a', 'b');
            var edge4 = new Edge<char>('c', 'd');
            var edge5 = new Edge<char>('e', 'c');
            var edge6 = new Edge<char>('d', 'e');
            var edge7 = new Edge<char>('c', 'f');

            var graph = new AdjacencyGraph<char, Edge<char>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge1, edge2, edge3, edge4, edge5, edge6, edge7
            });

            ComputeTrails(
                graph,
                (s, t) => new Edge<char>(s, t),
                out ICollection<Edge<char>>[] trails,
                out Edge<char>[] circuit);

            Edge<char>[] expectedTrail = { edge3, edge1, edge4, edge6, edge5, edge7, edge2 };
            trails.Length.Should().Be(1);
            trails[0].IsPath<char, Edge<char>>().Should().BeTrue();
            trails[0].Should().BeEquivalentTo(expectedTrail);

            circuit.IsPath<char, Edge<char>>().Should().BeTrue();
            circuit.Should().BeEquivalentTo(expectedTrail);
        }

        [Test]
        public void SingleEulerianTrailGraph2()
        {
            var edge1 = new Edge<char>('b', 'c');
            var edge2 = new Edge<char>('f', 'a');
            var edge3 = new Edge<char>('a', 'b');
            var edge4 = new Edge<char>('c', 'd');
            var edge5 = new Edge<char>('e', 'c');
            var edge6 = new Edge<char>('d', 'e');
            var edge7 = new Edge<char>('c', 'f');
            var edge8 = new Edge<char>('b', 'e');

            var graph = new AdjacencyGraph<char, Edge<char>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8
            });

            ComputeTrails(
                graph,
                (s, t) => new Edge<char>(s, t),
                out ICollection<Edge<char>>[] trails,
                out Edge<char>[] circuit);

            Edge<char>[] expectedTrail = { edge3, edge1, edge4, edge6, edge5, edge7, edge2 };
            trails.Length.Should().Be(1);
            trails[0].IsPath<char, Edge<char>>().Should().BeTrue();
            trails[0].Should().BeEquivalentTo(expectedTrail);

            circuit.IsPath<char, Edge<char>>().Should().BeTrue();
            circuit.Should().BeEquivalentTo(expectedTrail);
        }

        [Test]
        public void SingleEulerianTrailGraph3()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(2, 1);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(3, 1);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(4, 2);
            var edge7 = new Edge<int>(3, 4);
            var edge8 = new Edge<int>(4, 3);
            var edge9 = new Edge<int>(4, 4);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8, edge9
            });

            ComputeTrails(
                graph,
                (s, t) => new Edge<int>(s, t),
                out ICollection<Edge<int>>[] trails,
                out Edge<int>[] circuit);

            Edge<int>[] expectedTrail = { edge3, edge7, edge9, edge6, edge5, edge8, edge4, edge1, edge2 };
            trails.Length.Should().Be(1);
            trails[0].IsPath<int, Edge<int>>().Should().BeTrue();
            trails[0].Should().BeEquivalentTo(expectedTrail);

            circuit.IsPath<int, Edge<int>>().Should().BeTrue();
            circuit.Should().BeEquivalentTo(expectedTrail);
        }

        [Test]
        public void MultipleEulerianTrailsGraph()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(2, 1);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(3, 1);
            var edge5 = new Edge<int>(4, 2);
            var edge6 = new Edge<int>(3, 4);
            var edge7 = new Edge<int>(4, 3);
            var edge8 = new Edge<int>(4, 4);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8
            });

            ComputeTrails(
                graph,
                (s, t) => new Edge<int>(s, t),
                out ICollection<Edge<int>>[] trails,
                out Edge<int>[] circuit);

            Edge<int>[] expectedTrail1 = { edge3, edge6, edge8, edge5 };
            Edge<int>[] expectedTrail2 = { edge7, edge4, edge1, edge2 };
            trails.Length.Should().Be(2);
            trails[0].IsPath<int, Edge<int>>().Should().BeTrue();
            trails[1].IsPath<int, Edge<int>>().Should().BeTrue();
            trails[0].Should().BeEquivalentTo(expectedTrail1);
            trails[1].Should().BeEquivalentTo(expectedTrail2);

            circuit.IsPath<int, Edge<int>>().Should().BeTrue();
            circuit.Length.Should().Be(expectedTrail1.Length + expectedTrail2.Length + 1);
            foreach (Edge<int> edge in expectedTrail1.Concat(expectedTrail2))
            {
                circuit.Should().Contain(edge);
            }
            // + Temporary edge
            circuit.FirstOrDefault(e => e.Source == 2 && e.Target == 4).Should().NotBeNull();
        }

        #endregion

        #region Rooted trails

        [TestCaseSource(nameof(LoadGraph_G_10_0))]
        public void RootedNotEulerianTrailGraph_Throws(TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string> testGraph)
        {
            var graph = testGraph.Instance;
            Invoking(() =>
            {
                ComputeTrails(
                    graph,
                    graph.Vertices.First(),
                    (s, t) => new Edge<string>(s, t),
                    out _,
                    out _);
            }).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void SingleRootedEulerianTrailGraph()
        {
            var edge1 = new Edge<char>('b', 'c');
            var edge2 = new Edge<char>('f', 'a');
            var edge3 = new Edge<char>('a', 'b');
            var edge4 = new Edge<char>('c', 'd');
            var edge5 = new Edge<char>('e', 'c');
            var edge6 = new Edge<char>('d', 'e');
            var edge7 = new Edge<char>('c', 'f');
            var edge8 = new Edge<char>('b', 'e');

            var graph = new AdjacencyGraph<char, Edge<char>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8
            });

            ComputeTrails(
                graph,
                'c',
                (s, t) => new Edge<char>(s, t),
                out ICollection<Edge<char>>[] trails,
                out Edge<char>[] circuit);

            Edge<char>[] expectedTrail = { edge4, edge6, edge5, edge7, edge2, edge3, edge1 };
            trails.Length.Should().Be(1);
            trails[0].IsPath<char, Edge<char>>().Should().BeTrue();
            trails[0].Should().BeEquivalentTo(expectedTrail);
            trails[0].ElementAt(0).Source.Should().Be('c');

            circuit.IsPath<char, Edge<char>>().Should().BeTrue();
            circuit.Should().BeEquivalentTo(expectedTrail);
        }

        [Test]
        public void SingleRootedEulerianTrailGraph2()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(2, 1);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(3, 1);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(4, 2);
            var edge7 = new Edge<int>(3, 4);
            var edge8 = new Edge<int>(4, 3);
            var edge9 = new Edge<int>(4, 4);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8, edge9
            });

            ComputeTrails(
                graph,
                4,
                (s, t) => new Edge<int>(s, t),
                out ICollection<Edge<int>>[] trails,
                out Edge<int>[] circuit);

            Edge<int>[] expectedTrail = { edge9, edge6, edge5, edge8, edge4, edge1, edge2, edge3, edge7 };
            trails.Length.Should().Be(1);
            trails[0].IsPath<int, Edge<int>>().Should().BeTrue();
            trails[0].Should().BeEquivalentTo(expectedTrail);
            trails[0].ElementAt(0).Source.Should().Be(4);

            circuit.IsPath<int, Edge<int>>().Should().BeTrue();
            circuit.Should().BeEquivalentTo(expectedTrail);
        }

        [Test]
        public void MultipleRootedEulerianTrailsGraph()
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(2, 1);
            var edge3 = new EquatableEdge<int>(1, 3);
            var edge4 = new EquatableEdge<int>(3, 1);
            var edge5 = new EquatableEdge<int>(4, 2);
            var edge6 = new EquatableEdge<int>(3, 4);
            var edge7 = new EquatableEdge<int>(4, 3);
            var edge8 = new EquatableEdge<int>(4, 4);

            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8
            });

            // Root 2
            ComputeTrails(
                graph,
                2,
                (s, t) => new EquatableEdge<int>(s, t),
                out ICollection<EquatableEdge<int>>[] trails,
                out EquatableEdge<int>[] circuit);
            EquatableEdge<int>[] trail1 = { edge2, edge3, edge6, edge8, edge5 };
            EquatableEdge<int>[] trail2 = { new EquatableEdge<int>(2, 4), edge7, edge4, edge1 };
            CheckTrails(trails, trail1, trail2);

            circuit.IsPath<int, EquatableEdge<int>>().Should().BeTrue();
            circuit.Length.Should().Be(trail1.Length + trail2.Length);
            foreach (EquatableEdge<int> edge in trail1.Concat(trail2))
            {
                circuit.Should().Contain(edge);
            }

            // Root 3
            ComputeTrails(
                graph,
                3,
                (s, t) => new EquatableEdge<int>(s, t),
                out trails,
                out circuit);
            trail1 = new[] { edge6, edge8, edge5 };
            trail2 = new[] { edge6, edge7, edge4, edge1, edge2, edge3 };
            CheckTrails(trails, trail1, trail2);

            circuit.IsPath<int, EquatableEdge<int>>().Should().BeTrue();
            circuit.Length.Should().Be(trail1.Concat(trail2).Distinct().Count() /* Edge present in both paths */ + 1);
            foreach (EquatableEdge<int> edge in trail1.Concat(trail2))
            {
                circuit.Should().Contain(edge);
            }
            // + Temporary edge
            circuit.FirstOrDefault(e => e.Source == 2 && e.Target == 4).Should().NotBeNull();

            #region Local function

            void CheckTrails(
                IList<ICollection<EquatableEdge<int>>> computedTrails,
                IEnumerable<EquatableEdge<int>> expectedTrail1,
                IEnumerable<EquatableEdge<int>> expectedTrail2)
            {
                computedTrails.Count.Should().Be(2);
                computedTrails[0].IsPath<int, EquatableEdge<int>>().Should().BeTrue();
                computedTrails[1].IsPath<int, EquatableEdge<int>>().Should().BeTrue();
                computedTrails[0].Should().BeEquivalentTo(expectedTrail1);
                computedTrails[1].Should().BeEquivalentTo(expectedTrail2);
            }

            #endregion
        }

        [Test]
        public void RootedEulerianTrails_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new EulerianTrailAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm.Trails(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        #endregion

        private static IEnumerable<TestCaseData> LoadGraph_G_42_34() =>
            new[] { new TestCaseData(TestGraphSourceProvider.Instance.G_42_34.DeferDeserializeAsAdjacencyGraph().CreateInstanceHandle()) };

        private static IEnumerable<TestCaseData> LoadGraph_G_10_0() =>
            new[] { new TestCaseData(TestGraphSourceProvider.Instance.G_10_0.DeferDeserializeAsAdjacencyGraph().CreateInstanceHandle()) };
    }
}
