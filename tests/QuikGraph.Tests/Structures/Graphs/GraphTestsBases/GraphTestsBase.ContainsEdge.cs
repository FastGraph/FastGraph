using System;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Contains Edge

        protected static void ContainsEdge_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge4);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(1, 0)));
        }

        protected static void ContainsEdge_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IEdgeSet<int, Edge<int>>> createGraph)
        {
            IEdgeSet<int, Edge<int>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(1, 0)));
        }

        protected static void ContainsEdge_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge4);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(1, 0)));
        }

        protected static void ContainsEdge_ForbiddenParallelEdges_ImmutableVertices_Test(
            [NotNull] IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddEdge(edge3);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddEdge(edge4);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(10, 11)));
            // Source not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(10, 1)));
            // Target not in graph
            Assert.IsFalse(graph.ContainsEdge(new Edge<int>(1, 10)));
        }

        protected static void ContainsEdge_ImmutableGraph_ReversedTest(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IEdgeSet<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            IEdgeSet<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var reversedEdge1 = new SReversedEdge<int, Edge<int>>(edge1);
            var edge2 = new Edge<int>(1, 3);
            var reversedEdge2 = new SReversedEdge<int, Edge<int>>(edge2);
            var edge3 = new Edge<int>(2, 1);
            var reversedEdge3 = new SReversedEdge<int, Edge<int>>(edge3);
            var edge4 = new Edge<int>(2, 2);
            var reversedEdge4 = new SReversedEdge<int, Edge<int>>(edge4);
            var otherEdge1 = new Edge<int>(1, 2);
            var reversedOtherEdge1 = new SReversedEdge<int, Edge<int>>(otherEdge1);

            Assert.IsFalse(graph.ContainsEdge(reversedEdge1));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge2));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge3));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge4));
            Assert.IsFalse(graph.ContainsEdge(reversedOtherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(reversedEdge1));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge2));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge3));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge4));
            Assert.IsFalse(graph.ContainsEdge(reversedOtherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(reversedEdge1));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge2));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge3));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge4));
            Assert.IsFalse(graph.ContainsEdge(reversedOtherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(reversedEdge1));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge2));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge3));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge4));
            Assert.IsFalse(graph.ContainsEdge(reversedOtherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(reversedEdge1));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge2));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge3));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge4));
            Assert.IsFalse(graph.ContainsEdge(reversedOtherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(reversedEdge1));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge2));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge3));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge4));
            Assert.IsTrue(graph.ContainsEdge(reversedOtherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(
                graph.ContainsEdge(
                    new SReversedEdge<int, Edge<int>>(
                        new Edge<int>(0, 10))));
            // Source not in graph
            Assert.IsFalse(
                graph.ContainsEdge(
                    new SReversedEdge<int, Edge<int>>(
                        new Edge<int>(0, 1))));
            // Target not in graph
            Assert.IsFalse(
                graph.ContainsEdge(
                    new SReversedEdge<int, Edge<int>>(
                        new Edge<int>(1, 0))));
        }

        protected static void ContainsEdge_EquatableEdge_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, EquatableEdge<int>> graph)
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge4);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(1, 0)));
        }

        protected static void ContainsEdge_EquatableEdge_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, EquatableEdge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IEdgeSet<int, EquatableEdge<int>>> createGraph)
        {
            IEdgeSet<int, EquatableEdge<int>> graph = createGraph();

            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(1, 0)));
        }

        protected static void ContainsEdge_EquatableEdge_EdgesOnly_Test(
            [NotNull] EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(edge4);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddVerticesAndEdge(otherEdge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(0, 10)));
            // Source not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(0, 1)));
            // Target not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(1, 0)));
        }

        protected static void ContainsEdge_EquatableEdges_ForbiddenParallelEdges_ImmutableVertices_Test(
            [NotNull] IMutableEdgeListGraph<int, EquatableEdge<int>> graph)
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.IsFalse(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsFalse(graph.ContainsEdge(otherEdge1));

            graph.AddEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsFalse(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsFalse(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddEdge(edge3);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsFalse(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            graph.AddEdge(edge4);
            Assert.IsTrue(graph.ContainsEdge(edge1));
            Assert.IsTrue(graph.ContainsEdge(edge2));
            Assert.IsTrue(graph.ContainsEdge(edge3));
            Assert.IsTrue(graph.ContainsEdge(edge4));
            Assert.IsTrue(graph.ContainsEdge(otherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(10, 11)));
            // Source not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(10, 1)));
            // Target not in graph
            Assert.IsFalse(graph.ContainsEdge(new EquatableEdge<int>(1, 10)));
        }

        protected static void ContainsEdge_EquatableEdge_ImmutableGraph_ReversedTest(
            [NotNull] IMutableVertexAndEdgeSet<int, EquatableEdge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IEdgeSet<int, SReversedEdge<int, EquatableEdge<int>>>> createGraph)
        {
            IEdgeSet<int, SReversedEdge<int, EquatableEdge<int>>> graph = createGraph();

            var edge1 = new EquatableEdge<int>(1, 2);
            var reversedEdge1 = new SReversedEdge<int, EquatableEdge<int>>(edge1);
            var edge2 = new EquatableEdge<int>(1, 3);
            var reversedEdge2 = new SReversedEdge<int, EquatableEdge<int>>(edge2);
            var edge3 = new EquatableEdge<int>(2, 1);
            var reversedEdge3 = new SReversedEdge<int, EquatableEdge<int>>(edge3);
            var edge4 = new EquatableEdge<int>(2, 2);
            var reversedEdge4 = new SReversedEdge<int, EquatableEdge<int>>(edge4);
            var otherEdge1 = new EquatableEdge<int>(1, 2);
            var reversedOtherEdge1 = new SReversedEdge<int, EquatableEdge<int>>(otherEdge1);

            Assert.IsFalse(graph.ContainsEdge(reversedEdge1));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge2));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge3));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge4));
            Assert.IsFalse(graph.ContainsEdge(reversedOtherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(reversedEdge1));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge2));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge3));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge4));
            Assert.IsTrue(graph.ContainsEdge(reversedOtherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(reversedEdge1));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge2));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge3));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge4));
            Assert.IsTrue(graph.ContainsEdge(reversedOtherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(reversedEdge1));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge2));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge3));
            Assert.IsFalse(graph.ContainsEdge(reversedEdge4));
            Assert.IsTrue(graph.ContainsEdge(reversedOtherEdge1));

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(reversedEdge1));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge2));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge3));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge4));
            Assert.IsTrue(graph.ContainsEdge(reversedOtherEdge1));

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(reversedEdge1));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge2));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge3));
            Assert.IsTrue(graph.ContainsEdge(reversedEdge4));
            Assert.IsTrue(graph.ContainsEdge(reversedOtherEdge1));

            // Both vertices not in graph
            Assert.IsFalse(
                graph.ContainsEdge(
                    new SReversedEdge<int, EquatableEdge<int>>(
                        new EquatableEdge<int>(0, 10))));
            // Source not in graph
            Assert.IsFalse(
                graph.ContainsEdge(
                    new SReversedEdge<int, EquatableEdge<int>>(
                        new EquatableEdge<int>(0, 1))));
            // Target not in graph
            Assert.IsFalse(
                graph.ContainsEdge(
                    new SReversedEdge<int, EquatableEdge<int>>(
                        new EquatableEdge<int>(1, 0))));
        }

        protected static void ContainsEdge_SourceTarget_Test(
            [NotNull] IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.IsFalse(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(1, 3));
            Assert.IsFalse(graph.ContainsEdge(3, 1));

            graph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(graph.ContainsEdge(2, 2));

            // Vertices is not present in the graph
            Assert.IsFalse(graph.ContainsEdge(0, 4));
            Assert.IsFalse(graph.ContainsEdge(1, 4));
            Assert.IsFalse(graph.ContainsEdge(4, 1));
        }

        protected static void ContainsEdge_SourceTarget_ImmutableGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IIncidenceGraph<int, Edge<int>>> createGraph)
        {
            IIncidenceGraph<int, Edge<int>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.IsFalse(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(1, 3));
            Assert.IsFalse(graph.ContainsEdge(3, 1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(2, 2));

            // Vertices is not present in the graph
            Assert.IsFalse(graph.ContainsEdge(0, 4));
            Assert.IsFalse(graph.ContainsEdge(1, 4));
            Assert.IsFalse(graph.ContainsEdge(4, 1));
        }

        protected static void ContainsEdge_SourceTarget_ForbiddenParallelEdges_Test(
            [NotNull] BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.IsFalse(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            graph.AddEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            graph.AddEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(1, 3));
            Assert.IsFalse(graph.ContainsEdge(3, 1));

            graph.AddEdge(edge3);
            Assert.IsTrue(graph.ContainsEdge(2, 2));

            // Vertices is not present in the graph
            Assert.IsFalse(graph.ContainsEdge(4, 5));
            Assert.IsFalse(graph.ContainsEdge(1, 4));
            Assert.IsFalse(graph.ContainsEdge(4, 1));
        }

        protected static void ContainsEdge_SourceTarget_ImmutableGraph_ReversedTest(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            IIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.IsFalse(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.IsFalse(graph.ContainsEdge(1, 2));
            Assert.IsTrue(graph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.IsFalse(graph.ContainsEdge(1, 3));
            Assert.IsTrue(graph.ContainsEdge(3, 1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(2, 2));

            // Vertices is not present in the graph
            Assert.IsFalse(graph.ContainsEdge(0, 4));
            Assert.IsFalse(graph.ContainsEdge(1, 4));
            Assert.IsFalse(graph.ContainsEdge(4, 1));
        }

        protected static void ContainsEdge_SourceTarget_UndirectedGraph_Test(
            [NotNull] IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.IsFalse(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            graph.AddVerticesAndEdge(edge1);
            Assert.IsTrue(graph.ContainsEdge(1, 2));
            Assert.IsTrue(graph.ContainsEdge(2, 1));

            graph.AddVerticesAndEdge(edge2);
            Assert.IsTrue(graph.ContainsEdge(1, 3));
            Assert.IsTrue(graph.ContainsEdge(3, 1));

            graph.AddVerticesAndEdge(edge3);
            Assert.IsTrue(graph.ContainsEdge(2, 2));

            // Vertices is not present in the graph
            Assert.IsFalse(graph.ContainsEdge(0, 4));
            Assert.IsFalse(graph.ContainsEdge(1, 4));
            Assert.IsFalse(graph.ContainsEdge(4, 1));
        }

        protected static void ContainsEdge_SourceTarget_ImmutableGraph_UndirectedGraph_Test(
            [NotNull] IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [NotNull, InstantHandle] Func<IImplicitUndirectedGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            IImplicitUndirectedGraph<int, Edge<int>> graph = createGraph();
            Assert.IsFalse(graph.ContainsEdge(1, 2));
            Assert.IsFalse(graph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(1, 2));
            Assert.IsTrue(graph.ContainsEdge(2, 1));

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(1, 3));
            Assert.IsTrue(graph.ContainsEdge(3, 1));

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.IsTrue(graph.ContainsEdge(2, 2));

            // Vertices is not present in the graph
            Assert.IsFalse(graph.ContainsEdge(0, 4));
            Assert.IsFalse(graph.ContainsEdge(1, 4));
            Assert.IsFalse(graph.ContainsEdge(4, 1));
        }

        protected static void ContainsEdge_Throws_Test<TVertex, TEdge>(
            [NotNull] IEdgeSet<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null));
        }

        protected static void ContainsEdge_Throws_ReversedTest<TVertex, TEdge>(
            [NotNull] IEdgeSet<TVertex, SReversedEdge<TVertex, TEdge>> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(default));
        }

        protected static void ContainsEdge_SourceTarget_Throws_Test<TVertex>(
            [NotNull] IIncidenceGraph<TVertex, Edge<TVertex>> graph)
            where TVertex : class, new()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(new TVertex(), null));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, new TVertex()));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void ContainsEdge_SourceTarget_Throws_ReversedTest<TVertex>(
            [NotNull] IIncidenceGraph<TVertex, SReversedEdge<TVertex, Edge<TVertex>>> graph)
            where TVertex : class, new()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(new TVertex(), null));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, new TVertex()));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void ContainsEdge_SourceTarget_Throws_UndirectedGraph_Test<TVertex>(
            [NotNull] IImplicitUndirectedGraph<TVertex, Edge<TVertex>> graph)
            where TVertex : class, new()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(new TVertex(), null));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, new TVertex()));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion
    }
}
