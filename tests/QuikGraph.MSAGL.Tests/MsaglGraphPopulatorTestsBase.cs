using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.MSAGL.Tests.MsaglGraphTestHelpers;

namespace QuikGraph.MSAGL.Tests
{
    /// <summary>
    /// Base class for tests relative to <see cref="MsaglGraphPopulator{TVertex,TEdge}"/>.
    /// </summary>
    internal class MsaglGraphPopulatorTestsBase
    {
        protected static void Compute_Test<TPopulator>(
            [NotNull, InstantHandle] Func<IEdgeListGraph<int, Edge<int>>, TPopulator> createPopulator)
            where TPopulator : MsaglGraphPopulator<int, Edge<int>>
        {
            // Empty graph
            var graph = new AdjacencyGraph<int, Edge<int>>();
            MsaglGraphPopulator<int, Edge<int>> populator = createPopulator(graph);
            populator.Compute();
            AssertAreEquivalent(graph, populator.MsaglGraph);

            // Only vertices
            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2, 3 });
            populator = createPopulator(graph);
            populator.Compute();
            AssertAreEquivalent(graph, populator.MsaglGraph);

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
            AssertAreEquivalent(graph, populator.MsaglGraph);

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
            AssertAreEquivalent(graph, populator.MsaglGraph);

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
            AssertAreEquivalent(graph, populator.MsaglGraph);

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
            AssertAreEquivalent(undirectedGraph, populator.MsaglGraph);
        }

        [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
        protected static void Handlers_Test<TPopulator>(
            [NotNull, InstantHandle] Func<IEdgeListGraph<int, Edge<int>>, TPopulator> createPopulator)
            where TPopulator : MsaglGraphPopulator<int, Edge<int>>
        {
            // Empty graph
            var graph = new AdjacencyGraph<int, Edge<int>>();
            MsaglGraphPopulator<int, Edge<int>> populator = createPopulator(graph);
            populator.NodeAdded += (sender, args) => Assert.Fail($"{nameof(MsaglGraphPopulator<object, Edge<object>>.NodeAdded)} event called.");
            populator.EdgeAdded += (sender, args) => Assert.Fail($"{nameof(MsaglGraphPopulator<object, Edge<object>>.EdgeAdded)} event called.");
            populator.Compute();

            // Only vertices
            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2, 3 });
            populator = createPopulator(graph);
            var expectedVerticesAdded = new HashSet<int> { 1, 2, 3 };
            populator.NodeAdded += (sender, args) =>
            {
                Assert.IsTrue(expectedVerticesAdded.Remove(args.Vertex));
                Assert.AreEqual(args.Vertex, args.Node.UserData);
            };
            populator.EdgeAdded += (sender, args) => Assert.Fail($"{nameof(MsaglGraphPopulator<object, Edge<object>>.EdgeAdded)} event called.");
            populator.Compute();
            CollectionAssert.IsEmpty(expectedVerticesAdded);

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
            populator.NodeAdded += (sender, args) =>
            {
                Assert.IsTrue(expectedVerticesAdded.Remove(args.Vertex));
                Assert.AreEqual(args.Vertex, args.Node.UserData);
            };
            populator.EdgeAdded += (sender, args) =>
            {
                Assert.IsTrue(expectedEdgesAdded.Remove(args.Edge));
                Assert.AreSame(args.Edge, args.MsaglEdge.UserData);
            };
            populator.Compute();
            CollectionAssert.IsEmpty(expectedVerticesAdded);
            CollectionAssert.IsEmpty(expectedEdgesAdded);
        }
    }
}