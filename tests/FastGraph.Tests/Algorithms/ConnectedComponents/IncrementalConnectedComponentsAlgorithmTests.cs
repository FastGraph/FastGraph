#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.ConnectedComponents;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Tests for <see cref="IncrementalConnectedComponentsAlgorithm{TVertex,TEdge}"/>
    /// </summary>
    [TestFixture]
    internal sealed class IncrementalConnectedComponentsAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmState(algorithm, graph);

            algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(default, graph);
            AssertAlgorithmState(algorithm, graph);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void InvalidUse()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);

            Invoking(() => { int _ = algorithm.ComponentCount; }).Should().Throw<InvalidOperationException>();
            Invoking(() => algorithm.GetComponents()).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void IncrementalConnectedComponent()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 0, 1, 2, 3 });

            var algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            algorithm.ComponentCount.Should().Be(4);
            algorithm.GetComponents().Key.Should().Be(4);
            algorithm.GetComponents().Value.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [0] = 0,
                [1] = 1,
                [2] = 2,
                [3] = 3
            });

            graph.AddEdge(new Edge<int>(0, 1));
            algorithm.ComponentCount.Should().Be(3);
            algorithm.GetComponents().Key.Should().Be(3);
            algorithm.GetComponents().Value.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [0] = 0,
                [1] = 0,
                [2] = 1,
                [3] = 2
            });

            graph.AddEdge(new Edge<int>(2, 3));
            algorithm.ComponentCount.Should().Be(2);
            algorithm.GetComponents().Key.Should().Be(2);
            algorithm.GetComponents().Value.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [0] = 0,
                [1] = 0,
                [2] = 1,
                [3] = 1
            });

            graph.AddEdge(new Edge<int>(1, 3));
            algorithm.ComponentCount.Should().Be(1);
            algorithm.GetComponents().Key.Should().Be(1);
            algorithm.GetComponents().Value.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [0] = 0,
                [1] = 0,
                [2] = 0,
                [3] = 0
            });

            graph.AddVerticesAndEdge(new Edge<int>(4, 5));
            algorithm.ComponentCount.Should().Be(2);
            algorithm.GetComponents().Key.Should().Be(2);
            algorithm.GetComponents().Value.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [0] = 0,
                [1] = 0,
                [2] = 0,
                [3] = 0,
                [4] = 1,
                [5] = 1
            });

            graph.AddVertex(6);
            algorithm.ComponentCount.Should().Be(3);
            algorithm.GetComponents().Key.Should().Be(3);
            algorithm.GetComponents().Value.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [0] = 0,
                [1] = 0,
                [2] = 0,
                [3] = 0,
                [4] = 1,
                [5] = 1,
                [6] = 2
            });
        }

        [Test]
        public void IncrementalConnectedComponent_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var edge13 = new Edge<int>(1, 3);
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                edge13,
                new Edge<int>(4, 5)
            });
            graph.AddVertex(6);

            using (graph.IncrementalConnectedComponents(out Func<KeyValuePair<int, IDictionary<int, int>>> _))
            {
                Invoking(() => graph.RemoveVertex(6)).Should().Throw<InvalidOperationException>();
                Invoking(() => graph.RemoveEdge(edge13)).Should().Throw<InvalidOperationException>();
            }
        }

        [Test]
        public void IncrementalConnectedComponentMultiRun()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            Invoking(() =>
            {
                algorithm.Compute();
                algorithm.Compute();
            }).Should().NotThrow();
        }

        [Test]
        public void Dispose()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            Invoking(() => algorithm.Dispose()).Should().NotThrow();

            algorithm = new IncrementalConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();
            Invoking(() => algorithm.Dispose()).Should().NotThrow();
        }
    }
}
