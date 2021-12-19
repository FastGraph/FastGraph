#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms.Exploration;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.Exploration
{
    /// <summary>
    /// Tests for <see cref="CloneableVertexGraphExplorerAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class CloneableVertexGraphExplorerAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        [Pure]
        private static IEnumerable<ITransitionFactory<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>> GenerateTransitionFactories(
            out IEnumerable<EquatableCloneableTestVertex> vertices,
            out IEnumerable<Edge<EquatableCloneableTestVertex>> edges)
        {
            var vertex1 = new EquatableCloneableTestVertex("1");
            var vertex2 = new EquatableCloneableTestVertex("2");
            var vertex3 = new EquatableCloneableTestVertex("3");
            var vertex4 = new EquatableCloneableTestVertex("4");
            var vertex5 = new EquatableCloneableTestVertex("5");
            var vertex6 = new EquatableCloneableTestVertex("6");
            var vertex7 = new EquatableCloneableTestVertex("7");
            var vertex8 = new EquatableCloneableTestVertex("8");
            vertices = new[] { vertex1, vertex2, vertex3, vertex4, vertex5, vertex6, vertex7, vertex8 };

            var edge12 = new Edge<EquatableCloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<EquatableCloneableTestVertex>(vertex1, vertex3);
            var edge16 = new Edge<EquatableCloneableTestVertex>(vertex1, vertex6);
            var edge32 = new Edge<EquatableCloneableTestVertex>(vertex3, vertex2);
            var edge45 = new Edge<EquatableCloneableTestVertex>(vertex4, vertex5);
            var edge54 = new Edge<EquatableCloneableTestVertex>(vertex5, vertex4);
            var edge56 = new Edge<EquatableCloneableTestVertex>(vertex5, vertex6);
            var edge57 = new Edge<EquatableCloneableTestVertex>(vertex5, vertex7);
            var edge63 = new Edge<EquatableCloneableTestVertex>(vertex6, vertex3);
            var edge64 = new Edge<EquatableCloneableTestVertex>(vertex6, vertex4);
            edges = new[] { edge12, edge13, edge16, edge32, edge45, edge54, edge56, edge57, edge63, edge64 };

            return new[]
            {
                new TestTransitionFactory<EquatableCloneableTestVertex>(vertex1, new[] { edge12, edge13, edge16 }),
                new TestTransitionFactory<EquatableCloneableTestVertex>(vertex2, Enumerable.Empty<Edge<EquatableCloneableTestVertex>>()),
                new TestTransitionFactory<EquatableCloneableTestVertex>(vertex3, new[] {edge32}),
                new TestTransitionFactory<EquatableCloneableTestVertex>(vertex4, new[] {edge45}),
                new TestTransitionFactory<EquatableCloneableTestVertex>(vertex5, new[] {edge54, edge56, edge57}),
                new TestTransitionFactory<EquatableCloneableTestVertex>(vertex6, new[] {edge63, edge64}),
                new TestTransitionFactory<EquatableCloneableTestVertex>(vertex7, Enumerable.Empty<Edge<EquatableCloneableTestVertex>>()),
                new TestTransitionFactory<EquatableCloneableTestVertex>(vertex8, Enumerable.Empty<Edge<EquatableCloneableTestVertex>>())
            };
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(default, graph);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                CloneableVertexGraphExplorerAlgorithm<TVertex, TEdge> algo,
                IMutableVertexAndEdgeSet<TVertex, TEdge> g)
                where TVertex : ICloneable
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                algo.AddVertexPredicate.Should().NotBeNull();
                algo.AddEdgePredicate.Should().NotBeNull();
                algo.ExploreVertexPredicate.Should().NotBeNull();
                algo.FinishedSuccessfully.Should().BeFalse();
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(default, default)).Should().Throw<ArgumentNullException>();

            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);
            Invoking(() => algorithm.AddEdgePredicate = default).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.ExploreVertexPredicate = default).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.AddEdgePredicate = default).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.FinishedPredicate = default).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);

            int rootVertexChangeCount = 0;
            algorithm.RootVertexChanged += (_, _) => ++rootVertexChangeCount;

            var vertex1 = new CloneableTestVertex("1");
            algorithm.SetRootVertex(vertex1);
            rootVertexChangeCount.Should().Be(1);
            algorithm.TryGetRootVertex(out CloneableTestVertex? root);
            root.Should().BeSameAs(vertex1);

            // Not changed
            algorithm.SetRootVertex(vertex1);
            rootVertexChangeCount.Should().Be(1);
            algorithm.TryGetRootVertex(out root);
            root.Should().BeSameAs(vertex1);

            var vertex2 = new CloneableTestVertex("2");
            algorithm.SetRootVertex(vertex2);
            rootVertexChangeCount.Should().Be(2);
            algorithm.TryGetRootVertex(out root);
            root.Should().BeSameAs(vertex2);

            algorithm.SetRootVertex(vertex1);
            rootVertexChangeCount.Should().Be(3);
            algorithm.TryGetRootVertex(out root);
            root.Should().BeSameAs(vertex1);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);
            Invoking(algorithm.Compute).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>();
            graph.AddVertexRange(new[] { new EquatableCloneableTestVertex() });
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>(graph);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm.Compute(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            algorithm.TryGetRootVertex(out _).Should().BeFalse();
        }

        #endregion

        #region Factory manipulations

        [Test]
        public void AddTransitionFactory()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);

            var vertex1 = new CloneableTestVertex("1");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>());
            algorithm.AddTransitionFactory(factory1);

            algorithm.ContainsTransitionFactory(factory1).Should().BeTrue();

            var vertex2 = new CloneableTestVertex("2");
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, Enumerable.Empty<Edge<CloneableTestVertex>>());
            algorithm.AddTransitionFactory(factory2);

            algorithm.ContainsTransitionFactory(factory2).Should().BeTrue();

            algorithm.AddTransitionFactory(factory1);

            algorithm.ContainsTransitionFactory(factory1).Should().BeTrue();
        }

        [Test]
        public void AddTransitionFactory_Throws()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm.AddTransitionFactory(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void AddTransitionFactories()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>());
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, Enumerable.Empty<Edge<CloneableTestVertex>>());
            algorithm.AddTransitionFactories(new[] { factory1, factory2 });

            algorithm.ContainsTransitionFactory(factory1).Should().BeTrue();
            algorithm.ContainsTransitionFactory(factory2).Should().BeTrue();

            var vertex3 = new CloneableTestVertex("3");
            var factory3 = new TestTransitionFactory<CloneableTestVertex>(vertex3, Enumerable.Empty<Edge<CloneableTestVertex>>());
            algorithm.AddTransitionFactory(factory3);

            algorithm.ContainsTransitionFactory(factory1).Should().BeTrue();
            algorithm.ContainsTransitionFactory(factory2).Should().BeTrue();
            algorithm.ContainsTransitionFactory(factory3).Should().BeTrue();
        }

        [Test]
        public void AddTransitionFactories_Throws()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => algorithm.AddTransitionFactories(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void RemoveTransitionFactories()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);

            algorithm.RemoveTransitionFactory(default!).Should().BeFalse();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>());
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, Enumerable.Empty<Edge<CloneableTestVertex>>());
            var factory3 = new TestTransitionFactory<CloneableTestVertex>(vertex3, Enumerable.Empty<Edge<CloneableTestVertex>>());
            algorithm.AddTransitionFactories(new[] { factory1, factory2 });

            algorithm.ContainsTransitionFactory(default!).Should().BeFalse();
            algorithm.ContainsTransitionFactory(factory1).Should().BeTrue();
            algorithm.ContainsTransitionFactory(factory2).Should().BeTrue();

            algorithm.RemoveTransitionFactory(factory3).Should().BeFalse();
            algorithm.RemoveTransitionFactory(factory1).Should().BeTrue();
            algorithm.RemoveTransitionFactory(factory1).Should().BeFalse();
            algorithm.RemoveTransitionFactory(factory2).Should().BeTrue();

            var factory4 = new TestTransitionFactory<CloneableTestVertex>(
                vertex1,
                new[]
                {
                    new Edge<CloneableTestVertex>(vertex1, vertex2),
                    new Edge<CloneableTestVertex>(vertex1, vertex3)
                });
            algorithm.AddTransitionFactory(factory4);
            algorithm.ContainsTransitionFactory(factory4).Should().BeTrue();
        }

        [Test]
        public void ContainsTransitionFactories()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);

            var vertex1 = new CloneableTestVertex("1");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, Enumerable.Empty<Edge<CloneableTestVertex>>());

            algorithm.ContainsTransitionFactory(default!).Should().BeFalse();
            algorithm.ContainsTransitionFactory(factory1).Should().BeFalse();

            algorithm.AddTransitionFactory(factory1);

            algorithm.ContainsTransitionFactory(default!).Should().BeFalse();
            algorithm.ContainsTransitionFactory(factory1).Should().BeTrue();

            var vertex2 = new CloneableTestVertex("2");
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, Enumerable.Empty<Edge<CloneableTestVertex>>());
            algorithm.AddTransitionFactory(factory2);

            algorithm.ContainsTransitionFactory(default!).Should().BeFalse();
            algorithm.ContainsTransitionFactory(factory1).Should().BeTrue();
            algorithm.ContainsTransitionFactory(factory2).Should().BeTrue();

            algorithm.RemoveTransitionFactory(factory1);

            algorithm.ContainsTransitionFactory(default!).Should().BeFalse();
            algorithm.ContainsTransitionFactory(factory1).Should().BeFalse();
            algorithm.ContainsTransitionFactory(factory2).Should().BeTrue();
        }

        [Test]
        public void ClearTransitionFactories()
        {
            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph);

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");

            var edge11 = new Edge<CloneableTestVertex>(vertex1, vertex1);
            var edge12 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge23 = new Edge<CloneableTestVertex>(vertex2, vertex3);
            var edge33 = new Edge<CloneableTestVertex>(vertex3, vertex3);

            var factory1 = new TestTransitionFactory<CloneableTestVertex>(new[]
            {
                new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, new[] { edge11, edge12, edge13 }),
                new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, new[] { edge23 })
            });
            algorithm.AddTransitionFactory(factory1);

            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex3, new[] { edge33 });
            algorithm.AddTransitionFactory(factory2);

            algorithm.ClearTransitionFactories();

            algorithm.ContainsTransitionFactory(factory1).Should().BeFalse();
            algorithm.ContainsTransitionFactory(factory2).Should().BeFalse();
        }

        #endregion

        [Test]
        public void GraphExploration()
        {
            var graph = new AdjacencyGraph<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>(graph);

            IEnumerable<ITransitionFactory<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>> factories =
                GenerateTransitionFactories(
                    out IEnumerable<EquatableCloneableTestVertex> vertices,
                    out _);
            EquatableCloneableTestVertex[] verticesArray = vertices.ToArray();

            algorithm.AddTransitionFactories(factories);

            var discoveredVertices = new List<EquatableCloneableTestVertex>(verticesArray);
            algorithm.DiscoverVertex += vertex =>
            {
                discoveredVertices.Remove(vertex).Should().BeTrue();
            };

            algorithm.TreeEdge += v => v.Should().NotBeNull();
            algorithm.BackEdge += v => v.Should().NotBeNull();
            algorithm.EdgeSkipped += _ => Assert.Fail("Edge must not be skipped.");

            algorithm.Compute(verticesArray[0]);

            discoveredVertices.Should().BeEquivalentTo(new[] { verticesArray[7] });
            // Isolated vertex are not considered unexplored
            algorithm.UnExploredVertices.Should().BeEmpty();
            algorithm.FinishedSuccessfully.Should().BeTrue();
        }

        [Test]
        public void GraphExplorationWithPredicates()
        {
            var graph = new AdjacencyGraph<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>(graph);

            IEnumerable<ITransitionFactory<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>> factories =
                GenerateTransitionFactories(
                    out IEnumerable<EquatableCloneableTestVertex> vertices,
                    out IEnumerable<Edge<EquatableCloneableTestVertex>> edges);
            EquatableCloneableTestVertex[] verticesArray = vertices.ToArray();
            Edge<EquatableCloneableTestVertex>[] edgesArray = edges.ToArray();

            algorithm.AddTransitionFactories(factories);

            algorithm.AddVertexPredicate = vertex => !vertex.Equals(verticesArray[1]); // vertex2
            algorithm.ExploreVertexPredicate = vertex => !vertex.Equals(verticesArray[4]); // vertex5
            algorithm.AddEdgePredicate = edge => edge != edgesArray[1]; // edge13

            var discoveredVertices = new List<EquatableCloneableTestVertex>(verticesArray);
            algorithm.DiscoverVertex += vertex =>
            {
                discoveredVertices.Remove(vertex).Should().BeTrue();
            };

            algorithm.TreeEdge += v => v.Should().NotBeNull();
            algorithm.BackEdge += v => v.Should().NotBeNull();
            var skippedEdge = new List<Edge<EquatableCloneableTestVertex>>();
            algorithm.EdgeSkipped += edge => skippedEdge.Add(edge);

            algorithm.Compute(verticesArray[0]);

            discoveredVertices.Should().BeEquivalentTo(new[] { verticesArray[1], verticesArray[6], verticesArray[7] });
            skippedEdge.Should().BeEquivalentTo(new[] { edgesArray[0], edgesArray[1], edgesArray[3] });
            algorithm.UnExploredVertices.Should().BeEmpty();
            algorithm.FinishedSuccessfully.Should().BeTrue();
        }

        [Test]
        public void GraphExplorationWithEarlyEndingVertex()
        {
            var graph = new AdjacencyGraph<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>(graph);

            IEnumerable<ITransitionFactory<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>> factories =
                GenerateTransitionFactories(
                    out IEnumerable<EquatableCloneableTestVertex> vertices,
                    out _);
            EquatableCloneableTestVertex[] verticesArray = vertices.ToArray();

            algorithm.AddTransitionFactories(factories);

            algorithm.FinishedPredicate =
                new CloneableVertexGraphExplorerAlgorithm
                <
                    EquatableCloneableTestVertex,
                    Edge<EquatableCloneableTestVertex>
                >.DefaultFinishedPredicate(2, int.MaxValue).Test;

            var discoveredVertices = new List<EquatableCloneableTestVertex>(verticesArray);
            algorithm.DiscoverVertex += vertex =>
            {
                discoveredVertices.Remove(vertex).Should().BeTrue();
            };

            algorithm.TreeEdge += v => v.Should().NotBeNull();
            algorithm.BackEdge += v => v.Should().NotBeNull();
            algorithm.EdgeSkipped += _ => Assert.Fail("Edge must not be skipped.");

            algorithm.Compute(verticesArray[0]);

            discoveredVertices.Should().BeEquivalentTo(new[] { verticesArray[3], verticesArray[4], verticesArray[6], verticesArray[7] });
            algorithm.UnExploredVertices.Should().BeEquivalentTo(new[] { verticesArray[1], verticesArray[2], verticesArray[5] });
            algorithm.FinishedSuccessfully.Should().BeFalse();
        }

        [Test]
        public void GraphExplorationWithEarlyEndingEdge()
        {
            var graph = new AdjacencyGraph<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>();
            var algorithm = new CloneableVertexGraphExplorerAlgorithm<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>(graph);

            IEnumerable<ITransitionFactory<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>> factories =
                GenerateTransitionFactories(
                    out IEnumerable<EquatableCloneableTestVertex> vertices,
                    out _);
            EquatableCloneableTestVertex[] verticesArray = vertices.ToArray();

            algorithm.AddTransitionFactories(factories);

            algorithm.FinishedPredicate =
                new CloneableVertexGraphExplorerAlgorithm
                <
                    EquatableCloneableTestVertex,
                    Edge<EquatableCloneableTestVertex>
                >.DefaultFinishedPredicate(int.MaxValue, 3).Test;

            var discoveredVertices = new List<EquatableCloneableTestVertex>(verticesArray);
            algorithm.DiscoverVertex += vertex =>
            {
                discoveredVertices.Remove(vertex).Should().BeTrue();
            };

            algorithm.TreeEdge += v => v.Should().NotBeNull();
            algorithm.BackEdge += v => v.Should().NotBeNull();
            algorithm.EdgeSkipped += _ => Assert.Fail("Edge must not be skipped.");

            algorithm.Compute(verticesArray[0]);

            discoveredVertices.Should().BeEquivalentTo(new[] { verticesArray[3], verticesArray[4], verticesArray[6], verticesArray[7] });
            algorithm.UnExploredVertices.Should().BeEquivalentTo(new[] { verticesArray[5] });
            algorithm.FinishedSuccessfully.Should().BeFalse();
        }

        [Test]
        public void GraphExploration_Throws()
        {
            var vertex1 = new CloneableTestVertex();

            var graph = new AdjacencyGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            graph.AddVertex(vertex1);

            var algorithm = new CloneableVertexGraphExplorerAlgorithm<CloneableTestVertex, Edge<CloneableTestVertex>>(graph)
            {
                AddVertexPredicate = vertex => vertex != vertex1
            };

            Invoking(() => algorithm.Compute(vertex1)).Should().Throw<InvalidOperationException>();
        }
    }
}
