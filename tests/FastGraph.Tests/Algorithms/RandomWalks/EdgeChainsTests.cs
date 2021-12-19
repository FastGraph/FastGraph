#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms.RandomWalks;

namespace FastGraph.Tests.Algorithms.RandomWalks
{
    /// <summary>
    /// Tests for <see cref="RandomWalkAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class EdgeChainsTests
    {
        #region Test helpers

        [Pure]
        private static IVertexAndEdgeListGraph<int, EquatableEdge<int>> CreateGraph1()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new EquatableEdge<int>(1, 2),
                new EquatableEdge<int>(1, 3),
                new EquatableEdge<int>(2, 3),
                new EquatableEdge<int>(2, 5),
                new EquatableEdge<int>(4, 3),
                new EquatableEdge<int>(4, 5),
                new EquatableEdge<int>(4, 7),
                new EquatableEdge<int>(5, 6),
                new EquatableEdge<int>(6, 7),
                new EquatableEdge<int>(7, 4),
                new EquatableEdge<int>(8, 3)
            });

            return graph;
        }

        [Pure]
        private static IVertexAndEdgeListGraph<int, EquatableEdge<int>> CreateGraph2()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new EquatableEdge<int>(1, 2),
                new EquatableEdge<int>(1, 3),
                new EquatableEdge<int>(2, 3),
                new EquatableEdge<int>(3, 4),
                new EquatableEdge<int>(4, 5),
                new EquatableEdge<int>(4, 7),
                new EquatableEdge<int>(5, 2),
                new EquatableEdge<int>(5, 6),
                new EquatableEdge<int>(6, 7),
                new EquatableEdge<int>(7, 4),
                new EquatableEdge<int>(8, 3)
            });

            return graph;
        }

        #endregion

        [Test]
        public void RoundRobinEdgeChain()
        {
            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph1 = CreateGraph1();

            var chain = new RoundRobinEdgeChain<int, EquatableEdge<int>>();
            chain.TryGetSuccessor(graph1, 1, out EquatableEdge<int>? edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(graph1, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(graph1, 3, out edge).Should().BeFalse();

            chain = new RoundRobinEdgeChain<int, EquatableEdge<int>>();
            EquatableEdge<int>[] edges = graph1.Edges.ToArray();
            chain.TryGetSuccessor(edges, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(edges, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(edges, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(edges, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(5);
            // etc.
            chain.TryGetSuccessor(Enumerable.Empty<EquatableEdge<int>>(), 1, out _).Should().BeFalse();


            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph2 = CreateGraph2();

            chain = new RoundRobinEdgeChain<int, EquatableEdge<int>>();
            chain.TryGetSuccessor(graph2, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(graph2, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(graph2, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(3);
            edge.Target.Should().Be(4);
            chain.TryGetSuccessor(graph2, 4, out edge).Should().BeTrue();
            edge!.Source.Should().Be(4);
            edge.Target.Should().Be(5);
            chain.TryGetSuccessor(graph2, 5, out edge).Should().BeTrue();
            edge!.Source.Should().Be(5);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(graph2, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(graph2, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(3);
            edge.Target.Should().Be(4);
            chain.TryGetSuccessor(graph2, 4, out edge).Should().BeTrue();
            edge!.Source.Should().Be(4);
            edge.Target.Should().Be(7);
            // Etc.

            chain = new RoundRobinEdgeChain<int, EquatableEdge<int>>();
            edges = graph2.Edges.ToArray();
            chain.TryGetSuccessor(edges, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(edges, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(edges, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(edges, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(3);
            edge.Target.Should().Be(4);
            // Etc.
        }

        [Test]
        public void NormalizedMarkovEdgeChain()
        {
            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph1 = CreateGraph1();

            var chain = new NormalizedMarkovEdgeChain<int, EquatableEdge<int>>
            {
                Rand = new Random(123456)
            };
            chain.TryGetSuccessor(graph1, 1, out EquatableEdge<int>? edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(graph1, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(graph1, 3, out edge).Should().BeFalse();

            chain = new NormalizedMarkovEdgeChain<int, EquatableEdge<int>>
            {
                Rand = new Random(123456)
            };
            EquatableEdge<int>[] edges = graph1.Edges.ToArray();
            chain.TryGetSuccessor(edges, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            // etc.
            chain.TryGetSuccessor(Enumerable.Empty<EquatableEdge<int>>(), 1, out _).Should().BeFalse();


            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph2 = CreateGraph2();

            chain = new NormalizedMarkovEdgeChain<int, EquatableEdge<int>>
            {
                Rand = new Random(123456)
            };
            chain.TryGetSuccessor(graph2, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(graph2, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(graph2, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(3);
            edge.Target.Should().Be(4);
            chain.TryGetSuccessor(graph2, 4, out edge).Should().BeTrue();
            edge!.Source.Should().Be(4);
            edge.Target.Should().Be(5);
            chain.TryGetSuccessor(graph2, 5, out edge).Should().BeTrue();
            edge!.Source.Should().Be(5);
            edge.Target.Should().Be(6);
            chain.TryGetSuccessor(graph2, 6, out edge).Should().BeTrue();
            edge!.Source.Should().Be(6);
            edge.Target.Should().Be(7);
            chain.TryGetSuccessor(graph2, 7, out edge).Should().BeTrue();
            edge!.Source.Should().Be(7);
            edge.Target.Should().Be(4);
            chain.TryGetSuccessor(graph2, 4, out edge).Should().BeTrue();
            // Etc.

            chain = new NormalizedMarkovEdgeChain<int, EquatableEdge<int>>
            {
                Rand = new Random(123456)
            };
            edges = graph2.Edges.ToArray();
            chain.TryGetSuccessor(edges, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            // Etc.
        }

        [Test]
        public void Constructor_WeightedMarkovEdgeChains()
        {
            var weights = new Dictionary<Edge<int>, double>();
            var chain1 = new WeightedMarkovEdgeChain<int, Edge<int>>(weights);
            chain1.Weights.Should().BeSameAs(weights);

            var chain2 = new VanishingWeightedMarkovEdgeChain<int, Edge<int>>(weights);
            chain2.Weights.Should().BeSameAs(weights);

            chain2 = new VanishingWeightedMarkovEdgeChain<int, Edge<int>>(weights, 2.0);
            chain2.Weights.Should().BeSameAs(weights);
        }

        [Test]
        public void Constructor_WeightedMarkovEdgeChains_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new WeightedMarkovEdgeChain<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new VanishingWeightedMarkovEdgeChain<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new VanishingWeightedMarkovEdgeChain<int, Edge<int>>(default, 2.0)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void WeightedMarkovEdgeChain()
        {
            var weights = new Dictionary<EquatableEdge<int>, double>
            {
                [new EquatableEdge<int>(1, 2)] = 1.0,
                [new EquatableEdge<int>(1, 3)] = 2.0,
                [new EquatableEdge<int>(2, 3)] = -1.0,
                [new EquatableEdge<int>(2, 5)] = 10.0,
                [new EquatableEdge<int>(3, 4)] = 5.0,
                [new EquatableEdge<int>(4, 3)] = 25.0,
                [new EquatableEdge<int>(4, 5)] = 2.0,
                [new EquatableEdge<int>(4, 7)] = 1.0,
                [new EquatableEdge<int>(5, 2)] = 3.0,
                [new EquatableEdge<int>(5, 6)] = 1.0,
                [new EquatableEdge<int>(6, 7)] = 1.5,
                [new EquatableEdge<int>(7, 4)] = 0.0,
                [new EquatableEdge<int>(8, 3)] = 1.0
            };
            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph1 = CreateGraph1();

            var chain = new WeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            chain.TryGetSuccessor(graph1, 1, out EquatableEdge<int>? edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(graph1, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(5);
            chain.TryGetSuccessor(graph1, 5, out edge).Should().BeTrue();
            edge!.Source.Should().Be(5);
            edge.Target.Should().Be(6);
            chain.TryGetSuccessor(graph1, 6, out edge).Should().BeTrue();
            edge!.Source.Should().Be(6);
            edge.Target.Should().Be(7);
            chain.TryGetSuccessor(graph1, 7, out edge).Should().BeTrue();
            edge!.Source.Should().Be(7);
            edge.Target.Should().Be(4);
            chain.TryGetSuccessor(graph1, 4, out edge).Should().BeTrue();
            edge!.Source.Should().Be(4);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(graph1, 3, out edge).Should().BeFalse();

            chain = new WeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            EquatableEdge<int>[] edges = graph1.Edges.ToArray();
            chain.TryGetSuccessor(edges, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(5);
            chain.TryGetSuccessor(edges, 5, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(5);
            chain.TryGetSuccessor(edges, 5, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(3);
            // etc.
            chain.TryGetSuccessor(Enumerable.Empty<EquatableEdge<int>>(), 1, out _).Should().BeFalse();


            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph2 = CreateGraph2();

            chain = new WeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            chain.TryGetSuccessor(graph2, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(graph2, 2, out edge).Should().BeFalse();

            chain = new WeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            edges = graph2.Edges.ToArray();
            chain.TryGetSuccessor(edges, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(3);
            edge.Target.Should().Be(4);
            chain.TryGetSuccessor(edges, 4, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            // Etc.
        }

        [Test]
        public void VanishingWeightedMarkovEdgeChain()
        {
            var weights = new Dictionary<EquatableEdge<int>, double>
            {
                [new EquatableEdge<int>(1, 2)] = 1.0,
                [new EquatableEdge<int>(1, 3)] = 2.0,
                [new EquatableEdge<int>(2, 3)] = -1.0,
                [new EquatableEdge<int>(2, 5)] = 10.0,
                [new EquatableEdge<int>(3, 4)] = 5.0,
                [new EquatableEdge<int>(4, 3)] = 25.0,
                [new EquatableEdge<int>(4, 5)] = 2.0,
                [new EquatableEdge<int>(4, 7)] = 1.0,
                [new EquatableEdge<int>(5, 2)] = 3.0,
                [new EquatableEdge<int>(5, 6)] = 1.0,
                [new EquatableEdge<int>(6, 7)] = 1.5,
                [new EquatableEdge<int>(7, 4)] = 0.25,
                [new EquatableEdge<int>(8, 3)] = 1.0
            };
            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph1 = CreateGraph1();

            var chain = new VanishingWeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            chain.TryGetSuccessor(graph1, 1, out EquatableEdge<int>? edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(graph1, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(5);
            chain.TryGetSuccessor(graph1, 5, out edge).Should().BeTrue();
            edge!.Source.Should().Be(5);
            edge.Target.Should().Be(6);
            chain.TryGetSuccessor(graph1, 6, out edge).Should().BeTrue();
            edge!.Source.Should().Be(6);
            edge.Target.Should().Be(7);
            chain.TryGetSuccessor(graph1, 7, out edge).Should().BeTrue();
            edge!.Source.Should().Be(7);
            edge.Target.Should().Be(4);
            chain.TryGetSuccessor(graph1, 4, out edge).Should().BeTrue();
            edge!.Source.Should().Be(4);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(graph1, 3, out edge).Should().BeFalse();

            weights = new Dictionary<EquatableEdge<int>, double>
            {
                [new EquatableEdge<int>(1, 2)] = 1.0,
                [new EquatableEdge<int>(1, 3)] = 2.0,
                [new EquatableEdge<int>(2, 3)] = -1.0,
                [new EquatableEdge<int>(2, 5)] = 10.0,
                [new EquatableEdge<int>(3, 4)] = 5.0,
                [new EquatableEdge<int>(4, 3)] = 25.0,
                [new EquatableEdge<int>(4, 5)] = 2.0,
                [new EquatableEdge<int>(4, 7)] = 1.0,
                [new EquatableEdge<int>(5, 2)] = 3.0,
                [new EquatableEdge<int>(5, 6)] = 1.0,
                [new EquatableEdge<int>(6, 7)] = 1.5,
                [new EquatableEdge<int>(7, 4)] = 0.25,
                [new EquatableEdge<int>(8, 3)] = 1.0
            };
            chain = new VanishingWeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            EquatableEdge<int>[] edges = graph1.Edges.ToArray();
            chain.TryGetSuccessor(edges, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(5);
            chain.TryGetSuccessor(edges, 5, out edge).Should().BeTrue();
            edge!.Source.Should().Be(2);
            edge.Target.Should().Be(5);
            chain.TryGetSuccessor(edges, 5, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(3);
            // etc.
            chain.TryGetSuccessor(Enumerable.Empty<EquatableEdge<int>>(), 1, out _).Should().BeFalse();


            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph2 = CreateGraph2();

            weights = new Dictionary<EquatableEdge<int>, double>
            {
                [new EquatableEdge<int>(1, 2)] = 1.0,
                [new EquatableEdge<int>(1, 3)] = 2.0,
                [new EquatableEdge<int>(2, 3)] = -1.0,
                [new EquatableEdge<int>(2, 5)] = 10.0,
                [new EquatableEdge<int>(3, 4)] = 5.0,
                [new EquatableEdge<int>(4, 3)] = 25.0,
                [new EquatableEdge<int>(4, 5)] = 2.0,
                [new EquatableEdge<int>(4, 7)] = 1.0,
                [new EquatableEdge<int>(5, 2)] = 3.0,
                [new EquatableEdge<int>(5, 6)] = 1.0,
                [new EquatableEdge<int>(6, 7)] = 1.5,
                [new EquatableEdge<int>(7, 4)] = 0.25,
                [new EquatableEdge<int>(8, 3)] = 1.0
            };
            chain = new VanishingWeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            chain.TryGetSuccessor(graph2, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(graph2, 2, out edge).Should().BeFalse();

            weights = new Dictionary<EquatableEdge<int>, double>
            {
                [new EquatableEdge<int>(1, 2)] = 1.0,
                [new EquatableEdge<int>(1, 3)] = 2.0,
                [new EquatableEdge<int>(2, 3)] = -1.0,
                [new EquatableEdge<int>(2, 5)] = 10.0,
                [new EquatableEdge<int>(3, 4)] = 5.0,
                [new EquatableEdge<int>(4, 3)] = 25.0,
                [new EquatableEdge<int>(4, 5)] = 2.0,
                [new EquatableEdge<int>(4, 7)] = 1.0,
                [new EquatableEdge<int>(5, 2)] = 3.0,
                [new EquatableEdge<int>(5, 6)] = 1.0,
                [new EquatableEdge<int>(6, 7)] = 1.5,
                [new EquatableEdge<int>(7, 4)] = 0.25,
                [new EquatableEdge<int>(8, 3)] = 1.0
            };
            chain = new VanishingWeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            edges = graph2.Edges.ToArray();
            chain.TryGetSuccessor(edges, 1, out edge).Should().BeTrue();
            edge!.Source.Should().Be(3);
            edge.Target.Should().Be(4);
            chain.TryGetSuccessor(edges, 4, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(3);
            chain.TryGetSuccessor(edges, 3, out edge).Should().BeTrue();
            edge!.Source.Should().Be(1);
            edge.Target.Should().Be(2);
            chain.TryGetSuccessor(edges, 2, out edge).Should().BeTrue();
            edge!.Source.Should().Be(5);
            edge.Target.Should().Be(2);
            // Etc.
        }
    }
}
