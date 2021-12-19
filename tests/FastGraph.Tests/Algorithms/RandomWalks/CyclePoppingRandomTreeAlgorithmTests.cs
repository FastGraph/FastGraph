#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms.RandomWalks;
using FastGraph.Algorithms.Search;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.RandomWalks
{
    /// <summary>
    /// Tests for <see cref="CyclePoppingRandomTreeAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class CyclePoppingRandomTreeAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunCyclePoppingRandomTreeAndCheck<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph,
            TVertex root)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var randomChain = new Random(123456);
            var chain = new NormalizedMarkovEdgeChain<TVertex, TEdge>
            {
                Rand = randomChain
            };
            var randomAlgorithm = new Random(123456);
            var algorithm = new CyclePoppingRandomTreeAlgorithm<TVertex, TEdge>(graph, chain)
            {
                Rand = randomAlgorithm
            };
            algorithm.InitializeVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.White, algorithm.VerticesColors[vertex]);
            };

            algorithm.FinishVertex += vertex =>
            {
                Assert.AreEqual(GraphColor.Black, algorithm.VerticesColors[vertex]);
            };

            algorithm.Compute(root);

            Assert.AreEqual(graph.VertexCount, algorithm.VerticesColors.Count);
            foreach (TVertex vertex in graph.Vertices)
            {
                Assert.AreEqual(GraphColor.Black, algorithm.VerticesColors[vertex]);
            }

            AssertIsTree(root, algorithm.Successors);
        }

        [Pure]
        private static IVertexListGraph<TVertex, TEdge> MakeGraph<TVertex, TEdge>(
            TVertex root,
            IDictionary<TVertex, TEdge> successors)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var graph = new AdjacencyGraph<TVertex, TEdge>();
            graph.AddVerticesAndEdgeRange(
                successors
                    .Where(pair => !Equals(pair.Key, root))
                    .Select(pair => pair.Value)
                    .Where(edge => edge != null));
            return graph;
        }

        private static void AssertIsTree<TVertex, TEdge>(
            TVertex root,
            IDictionary<TVertex, TEdge> successors)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            IVertexListGraph<TVertex, TEdge> graph = MakeGraph(root, successors);

            var dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(graph);
            dfs.BackEdge += _ => Assert.Fail("Random constructed tree contains a cycle.");
            dfs.Compute();
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            IMarkovEdgeChain<int, Edge<int>> markovChain1 = new NormalizedMarkovEdgeChain<int, Edge<int>>();
            IMarkovEdgeChain<int, Edge<int>> markovChain2 = new WeightedMarkovEdgeChain<int, Edge<int>>(new Dictionary<Edge<int>, double>());

            var algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph, markovChain1);
            AssertAlgorithmProperties(algorithm, graph, markovChain1);

            algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph, markovChain2);
            AssertAlgorithmProperties(algorithm, graph, markovChain2);

            algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(default, graph, markovChain1);
            AssertAlgorithmProperties(algorithm, graph, markovChain1);

            var random = new Random(123456);
            algorithm.Rand = random;
            AssertAlgorithmProperties(algorithm, graph, markovChain1, random);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                CyclePoppingRandomTreeAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g,
                IMarkovEdgeChain<TVertex, TEdge>? chain = default,
                Random? rand = default)
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                if (chain is null)
                    Assert.IsNotNull(algo.EdgeChain);
                else
                    Assert.AreSame(chain, algo.EdgeChain);
                if (rand is null)
                    Assert.IsNotNull(algo.Rand);
                else
                    Assert.AreSame(rand, algo.Rand);
                CollectionAssert.IsEmpty(algo.Successors);
                CollectionAssert.IsEmpty(algo.VerticesColors);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var chain = new NormalizedMarkovEdgeChain<int, Edge<int>>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(
                () => new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(default));

            Assert.Throws<ArgumentNullException>(
                () => new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph, default));
            Assert.Throws<ArgumentNullException>(
                () => new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(default, chain));
            Assert.Throws<ArgumentNullException>(
                () => new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(default, default));

            Assert.Throws<ArgumentNullException>(
                () => new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(default, graph, default));
            Assert.Throws<ArgumentNullException>(
                () => new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(default, default, chain));
            Assert.Throws<ArgumentNullException>(
                () => new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(default, default, default));

            var algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph, chain);
            Assert.Throws<ArgumentNullException>(() => algorithm.Rand = default);
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var chain = new NormalizedMarkovEdgeChain<int, Edge<int>>();
            var algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph, chain);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var chain = new NormalizedMarkovEdgeChain<int, Edge<int>>();
            var algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph, chain);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var chain = new NormalizedMarkovEdgeChain<TestVertex, Edge<TestVertex>>();
            var algorithm = new CyclePoppingRandomTreeAlgorithm<TestVertex, Edge<TestVertex>>(graph, chain);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var chain = new NormalizedMarkovEdgeChain<int, Edge<int>>();
            var algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph, chain);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var chain = new NormalizedMarkovEdgeChain<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(
                () => new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph, chain));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var chain = new NormalizedMarkovEdgeChain<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph, chain);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var chain = new NormalizedMarkovEdgeChain<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new CyclePoppingRandomTreeAlgorithm<TestVertex, Edge<TestVertex>>(graph, chain));
        }

        #endregion

        [Test]
        public void GetVertexColor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));
            var chain = new NormalizedMarkovEdgeChain<int, Edge<int>>();

            var algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph, chain);
            algorithm.Compute(1);

            Assert.AreEqual(GraphColor.Black, algorithm.GetVertexColor(1));
            Assert.AreEqual(GraphColor.Black, algorithm.GetVertexColor(2));
        }

        [Test]
        public void Repro13160()
        {
            // Create a new graph
            var graph = new BidirectionalGraph<int, Edge<int>>(false);

            // Adding vertices
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                    graph.AddVertex(i * 3 + j);
            }

            // Adding Width edges
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    graph.AddEdge(
                        new Edge<int>(i * 3 + j, i * 3 + j + 1));
                }
            }

            // Adding Length edges
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    graph.AddEdge(
                        new Edge<int>(i * 3 + j, (i + 1) * 3 + j));
                }
            }

            // Create cross edges
            foreach (Edge<int> edge in graph.Edges)
                graph.AddEdge(new Edge<int>(edge.Target, edge.Source));

            // Breaking graph apart
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (i == 1)
                        graph.RemoveVertex(i * 3 + j);
                }
            }

            var randomChain = new Random(123456);
            var chain = new NormalizedMarkovEdgeChain<int, Edge<int>>
            {
                Rand = randomChain
            };
            var randomAlgorithm = new Random(123456);
            var algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph, chain)
            {
                Rand = randomAlgorithm
            };

            Assert.DoesNotThrow(() => algorithm.Compute(2));
            // Successors is not a spanning tree...
        }

        [Test]
        public void SmallGraphWithCycles()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(0, 1),
                new Edge<int>(1, 0),
                new Edge<int>(1, 2),
                new Edge<int>(2, 1)
            });

            RunCyclePoppingRandomTreeAndCheck(graph, 0);
            RunCyclePoppingRandomTreeAndCheck(graph, 1);
            // Not all root fit, consider using RandomTree rather than rooted one
            //RunCyclePoppingRandomTreeAndCheck(graph, 2);
        }

        [Test]
        public void GraphWithCycles()
        {
            var graph = new AdjacencyGraph<char, Edge<char>>(true);
            graph.AddVertexRange("12345");
            var edges = new[]
            {
                new Edge<char>('1', '2'),
                new Edge<char>('1', '3'),
                new Edge<char>('1', '4'),
                new Edge<char>('2', '1'),
                new Edge<char>('2', '3'),
                new Edge<char>('2', '4'),
                new Edge<char>('3', '1'),
                new Edge<char>('3', '2'),
                new Edge<char>('3', '5'),
                new Edge<char>('5', '2')
            };
            graph.AddEdgeRange(edges);

            foreach (char vertex in graph.Vertices)
            {
                RunCyclePoppingRandomTreeAndCheck(graph, vertex);
            }
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void CyclePoppingRandomTree()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests(10))
            {
                foreach (string root in graph.Vertices)
                {
                    RunCyclePoppingRandomTreeAndCheck(graph, root);
                }
            }
        }

        [Test]
        public void IsolatedVertices()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(true);
            graph.AddVertex(0);
            graph.AddVertex(1);

            var algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph);
            algorithm.RandomTree();
            AssertIsTree(0, algorithm.Successors);
            AssertIsTree(1, algorithm.Successors);
        }

        [Test]
        public void IsolatedVerticesWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(true);
            graph.AddVertex(0);
            graph.AddVertex(1);

            var algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph);
            algorithm.RandomTreeWithRoot(0);
            AssertIsTree(0, algorithm.Successors);
        }

        [Test]
        public void RootIsNotAccessible()
        {
            AdjacencyGraph<int, Edge<int>> graph = new AdjacencyGraph<int, Edge<int>>(true);
            graph.AddVertex(0);
            graph.AddVertex(1);
            graph.AddEdge(new Edge<int>(0, 1));

            var algorithm = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph);
            algorithm.RandomTreeWithRoot(0);
            AssertIsTree(0, algorithm.Successors);
        }

        [Pure]
        public static CyclePoppingRandomTreeAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
            where T : notnull
        {
            var graph = new AdjacencyGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);
            var chain = new NormalizedMarkovEdgeChain<T, Edge<T>>();

            var algorithm = new CyclePoppingRandomTreeAlgorithm<T, Edge<T>>(graph, chain);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root!);
            return algorithm;
        }
    }
}
