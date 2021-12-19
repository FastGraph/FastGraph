#nullable enable

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.Msagl.Drawing;
using NUnit.Framework;
using static FastGraph.MSAGL.Tests.MsaglGraphTestHelpers;

namespace FastGraph.MSAGL.Tests
{
    /// <summary>
    /// Base class for tests relative to <see cref="MsaglGraphPopulator{TVertex,TEdge}"/>.
    /// </summary>
    internal class MsaglGraphPopulatorTestsBase
    {
        protected static void Compute_Test<TPopulator>(
            [InstantHandle] Func<IEdgeListGraph<int, Edge<int>>, TPopulator> createPopulator)
            where TPopulator : MsaglGraphPopulator<int, Edge<int>>
        {
            // Empty graph
            var graph = new AdjacencyGraph<int, Edge<int>>();
            MsaglGraphPopulator<int, Edge<int>> populator = createPopulator(graph);
            populator.Compute();
            populator.MsaglGraph!.Should().BeOfType<Graph>().BeEquivalentTo(graph);

            // Only vertices
            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2, 3 });
            populator = createPopulator(graph);
            populator.Compute();
            populator.MsaglGraph!.Should().BeOfType<Graph>().BeEquivalentTo(graph);

            // With vertices and edges
            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 3)
            });
            graph.AddVertexRange(new[] { 5, 6 });
            populator = createPopulator(graph);
            populator.Compute();
            populator.MsaglGraph!.Should().BeOfType<Graph>().BeEquivalentTo(graph);

            // With cycles
            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(4, 1)
            });
            populator = createPopulator(graph);
            populator.Compute();
            populator.MsaglGraph!.Should().BeOfType<Graph>().BeEquivalentTo(graph);

            // With self edge
            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 2),
                new Edge<int>(3, 1)
            });
            populator = createPopulator(graph);
            populator.Compute();
            populator.MsaglGraph!.Should().BeOfType<Graph>().BeEquivalentTo(graph);

            // Undirected graph
            var undirectedGraph = new UndirectedGraph<int, Edge<int>>();
            undirectedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1)
            });
            populator = createPopulator(undirectedGraph);
            populator.Compute();
            populator.MsaglGraph!.Should().BeOfType<Graph>().BeEquivalentTo(undirectedGraph);
        }

        protected static void Handlers_Test<TPopulator>(
            [InstantHandle] Func<IEdgeListGraph<int, Edge<int>>, TPopulator> createPopulator)
            where TPopulator : MsaglGraphPopulator<int, Edge<int>>
        {
            // Empty graph
            var graph = new AdjacencyGraph<int, Edge<int>>();
            MsaglGraphPopulator<int, Edge<int>> populator = createPopulator(graph);
            populator.NodeAdded += (_, _) => Assert.Fail($"{nameof(MsaglGraphPopulator<object, Edge<object>>.NodeAdded)} event called.");
            populator.EdgeAdded += (_, _) => Assert.Fail($"{nameof(MsaglGraphPopulator<object, Edge<object>>.EdgeAdded)} event called.");
            populator.Compute();

            // Only vertices
            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2, 3 });
            populator = createPopulator(graph);
            var expectedVerticesAdded = new HashSet<int> { 1, 2, 3 };
            populator.NodeAdded += (_, args) =>
            {
                expectedVerticesAdded.Remove(args.Vertex).Should().BeTrue();
                args.Node.UserData.Should().Be(args.Vertex);
            };
            populator.EdgeAdded += (_, _) => Assert.Fail($"{nameof(MsaglGraphPopulator<object, Edge<object>>.EdgeAdded)} event called.");
            populator.Compute();
            expectedVerticesAdded.Should().BeEmpty();

            // With vertices and edges
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge23 });
            graph.AddVertexRange(new[] { 5, 6 });
            populator = createPopulator(graph);
            expectedVerticesAdded = new HashSet<int> { 1, 2, 3, 5, 6 };
            var expectedEdgesAdded = new HashSet<Edge<int>> { edge12, edge13, edge23 };
            populator.NodeAdded += (_, args) =>
            {
                expectedVerticesAdded.Remove(args.Vertex).Should().BeTrue();
                args.Node.UserData.Should().Be(args.Vertex);
            };
            populator.EdgeAdded += (_, args) =>
            {
                expectedEdgesAdded.Remove(args.Edge).Should().BeTrue();
                args.MsaglEdge.UserData.Should().BeSameAs(args.Edge);
            };
            populator.Compute();
            expectedVerticesAdded.Should().BeEmpty();
            expectedEdgesAdded.Should().BeEmpty();
        }
    }
}
