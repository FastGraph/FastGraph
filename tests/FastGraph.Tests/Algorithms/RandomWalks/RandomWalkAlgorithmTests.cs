#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Observers;
using FastGraph.Algorithms.RandomWalks;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.RandomWalks
{
    /// <summary>
    /// Tests for <see cref="RandomWalkAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class RandomWalkAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunRandomWalkAndCheck<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            if (graph.VertexCount == 0)
                return;

            foreach (TVertex root in graph.Vertices)
            {
                RandomWalkAlgorithm<TVertex, TEdge> walker1 = CreateAlgorithm();
                bool calledStart1 = false;
                bool calledEnd1 = false;
                var encounteredEdges1 = new List<TEdge>();
                walker1.StartVertex += vertex =>
                {
                    calledStart1.Should().BeFalse();
                    calledStart1 = true;
                    vertex.Should().Be(root);
                };
                walker1.TreeEdge += edge =>
                {
                    edge.Should().NotBeNull();
                    encounteredEdges1.Add(edge);
                };
                walker1.EndVertex += vertex =>
                {
                    calledEnd1.Should().BeFalse();
                    calledEnd1 = true;
                    vertex.Should().NotBeNull();
                };

                RandomWalkAlgorithm<TVertex, TEdge> walker2 = CreateAlgorithm();
                bool calledStart2 = false;
                bool calledEnd2 = false;
                var encounteredEdges2 = new List<TEdge>();
                walker2.StartVertex += vertex =>
                {
                    calledStart2.Should().BeFalse();
                    calledStart2 = true;
                    vertex.Should().Be(root);
                };
                walker2.TreeEdge += edge =>
                {
                    edge.Should().NotBeNull();
                    encounteredEdges2.Add(edge);
                };
                walker2.EndVertex += vertex =>
                {
                    calledEnd2.Should().BeFalse();
                    calledEnd2 = true;
                    vertex.Should().NotBeNull();
                };

                RandomWalkAlgorithm<TVertex, TEdge> walker3 = CreateAlgorithm();
                bool calledStart3 = false;
                bool calledEnd3 = false;
                var encounteredEdges3 = new List<TEdge>();
                walker3.StartVertex += vertex =>
                {
                    calledStart3.Should().BeFalse();
                    calledStart3 = true;
                    vertex.Should().Be(root);
                };
                walker3.TreeEdge += edge =>
                {
                    edge.Should().NotBeNull();
                    encounteredEdges3.Add(edge);
                };
                walker3.EndVertex += vertex =>
                {
                    calledEnd3.Should().BeFalse();
                    calledEnd3 = true;
                    vertex.Should().NotBeNull();
                };

                var vis1 = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis1.Attach(walker1))
                    walker1.Generate(root);
                calledStart1.Should().BeTrue();
                calledEnd1.Should().BeTrue();

                walker2.SetRootVertex(root);
                var vis2 = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis2.Attach(walker2))
                    walker2.Compute();
                calledStart2.Should().BeTrue();
                calledEnd2.Should().BeTrue();

                var vis3 = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis3.Attach(walker3))
                    walker3.Generate(root, 100);
                calledStart3.Should().BeTrue();
                calledEnd3.Should().BeTrue();

                vis1.Edges.Should().BeEquivalentTo(encounteredEdges1);
                vis1.Edges.Should().BeEquivalentTo(encounteredEdges2);
                vis1.Edges.Should().BeEquivalentTo(encounteredEdges3);
                vis1.Edges.Should().BeEquivalentTo(vis2.Edges);
                vis1.Edges.Should().BeEquivalentTo(vis3.Edges);
            }

            #region Local function

            RandomWalkAlgorithm<TVertex, TEdge> CreateAlgorithm()
            {
                var walker = new RandomWalkAlgorithm<TVertex, TEdge>(graph)
                {
                    EdgeChain = new NormalizedMarkovEdgeChain<TVertex, TEdge>
                    {
                        Rand = new Random(123456)
                    }
                };

                return walker;
            }

            #endregion
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var chain = new WeightedMarkovEdgeChain<int, Edge<int>>(new Dictionary<Edge<int>, double>());
            EdgePredicate<int, Edge<int>> predicate = _ => true;
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph, chain);
            AssertAlgorithmProperties(algorithm, graph, chain);

            algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph)
            {
                EndPredicate = predicate
            };
            AssertAlgorithmProperties(algorithm, graph, p: predicate);

            algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph)
            {
                EdgeChain = chain
            };
            AssertAlgorithmProperties(algorithm, graph, chain);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                RandomWalkAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g,
                IEdgeChain<TVertex, TEdge>? c = default,
                EdgePredicate<TVertex, TEdge>? p = default)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                if (c is null)
                    algo.EdgeChain.Should().NotBeNull();
                else
                    algo.EdgeChain.Should().BeSameAs(c);
                algo.EndPredicate.Should().Be(p);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var chain = new WeightedMarkovEdgeChain<int, Edge<int>>(new Dictionary<Edge<int>, double>());

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new RandomWalkAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new RandomWalkAlgorithm<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new RandomWalkAlgorithm<int, Edge<int>>(default, chain)).Should().Throw<ArgumentNullException>();
            Invoking(() => new RandomWalkAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph, chain);
            Invoking(() => algorithm.EdgeChain = default).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new RandomWalkAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(
                () => new RandomWalkAlgorithm<int, Edge<int>>(graph));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new RandomWalkAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        [TestCaseSource(nameof(AdjacencyGraphs_All))]
        public void RandomWalk(TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string> testGraph)
        {
            RunRandomWalkAndCheck(testGraph.Instance);
        }

        [Test]
        public void RandomWalkWithPredicate()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            var edge4 = new Edge<int>(3, 4);
            var edge5 = new Edge<int>(4, 5);
            var edge6 = new Edge<int>(5, 4);
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge1, edge2, edge3, edge4, edge5, edge6
            });
            var chain = new NormalizedMarkovEdgeChain<int, Edge<int>>
            {
                Rand = new Random(123456)
            };

            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph, chain)
            {
                EndPredicate = edge => edge == edge4
            };

            var encounteredEdges = new List<Edge<int>>();
            algorithm.TreeEdge += edge => encounteredEdges.Add(edge);
            algorithm.EndVertex += vertex =>
            {
                vertex.Should().Be(3);
            };

            algorithm.Generate(1, int.MaxValue);

            encounteredEdges.Should().NotBeEmpty();
            encounteredEdges.Last().Target.Should().Be(3);
            (edge2 == encounteredEdges.Last()
             ||
             edge3 == encounteredEdges.Last()).Should().BeTrue();
        }

        [Test]
        public void RandomWalk_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);

            Invoking(() => algorithm.Generate(1)).Should().Throw<VertexNotFoundException>();
            Invoking(() => algorithm.Generate(1, 12)).Should().Throw<VertexNotFoundException>();
        }

        private static readonly IEnumerable<TestCaseData> AdjacencyGraphs_All =
            TestGraphFactory
                .SampleAdjacencyGraphs()
                .Select(t => new TestCaseData(t) { TestName = t.DescribeForTestCase() })
                .Memoize();
    }
}
