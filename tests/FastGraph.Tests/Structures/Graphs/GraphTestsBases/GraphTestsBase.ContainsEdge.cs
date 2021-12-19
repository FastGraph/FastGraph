#nullable enable

using JetBrains.Annotations;

namespace FastGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Contains Edge

        protected static void ContainsEdge_Test(
            IEdgeSet<int, Edge<int>> graph,
            [InstantHandle] Action<Edge<int>> addVerticesAndEdge)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            graph.ContainsEdge(edge1).Should().BeFalse();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            addVerticesAndEdge(edge1);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            addVerticesAndEdge(edge2);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            addVerticesAndEdge(edge3);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            addVerticesAndEdge(edge4);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            addVerticesAndEdge(otherEdge1);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            graph.ContainsEdge(new Edge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            graph.ContainsEdge(new Edge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            graph.ContainsEdge(new Edge<int>(1, 0)).Should().BeFalse();
        }

        protected static void ContainsEdge_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            ContainsEdge_Test(
                graph,
                edge => graph.AddVerticesAndEdge(edge));
        }

        protected static void ContainsEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IEdgeSet<int, Edge<int>>> createGraph)
        {
            IEdgeSet<int, Edge<int>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            graph.ContainsEdge(edge1).Should().BeFalse();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            graph.ContainsEdge(new Edge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            graph.ContainsEdge(new Edge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            graph.ContainsEdge(new Edge<int>(1, 0)).Should().BeFalse();
        }

        protected static void ContainsEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IEdgeSet<int, SEquatableEdge<int>>> createGraph)
        {
            IEdgeSet<int, SEquatableEdge<int>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var equatableEdge1 = new SEquatableEdge<int>(edge1.Source, edge1.Target);
            var edge2 = new Edge<int>(1, 3);
            var equatableEdge2 = new SEquatableEdge<int>(edge2.Source, edge2.Target);
            var edge3 = new Edge<int>(2, 1);
            var equatableEdge3 = new SEquatableEdge<int>(edge3.Source, edge3.Target);
            var edge4 = new Edge<int>(2, 2);
            var equatableEdge4 = new SEquatableEdge<int>(edge4.Source, edge4.Target);
            var otherEdge1 = new Edge<int>(1, 2);
            var equatableOtherEdge1 = new SEquatableEdge<int>(otherEdge1.Source, otherEdge1.Target);

            graph.ContainsEdge(equatableEdge1).Should().BeFalse();
            graph.ContainsEdge(equatableEdge2).Should().BeFalse();
            graph.ContainsEdge(equatableEdge3).Should().BeFalse();
            graph.ContainsEdge(equatableEdge4).Should().BeFalse();
            graph.ContainsEdge(equatableOtherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            graph.ContainsEdge(equatableEdge1).Should().BeTrue();
            graph.ContainsEdge(equatableEdge2).Should().BeFalse();
            graph.ContainsEdge(equatableEdge3).Should().BeFalse();
            graph.ContainsEdge(equatableEdge4).Should().BeFalse();
            graph.ContainsEdge(equatableOtherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            graph.ContainsEdge(equatableEdge1).Should().BeTrue();
            graph.ContainsEdge(equatableEdge2).Should().BeTrue();
            graph.ContainsEdge(equatableEdge3).Should().BeFalse();
            graph.ContainsEdge(equatableEdge4).Should().BeFalse();
            graph.ContainsEdge(equatableOtherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            graph.ContainsEdge(equatableEdge1).Should().BeTrue();
            graph.ContainsEdge(equatableEdge2).Should().BeTrue();
            graph.ContainsEdge(equatableEdge3).Should().BeTrue();
            graph.ContainsEdge(equatableEdge4).Should().BeFalse();
            graph.ContainsEdge(equatableOtherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            graph.ContainsEdge(equatableEdge1).Should().BeTrue();
            graph.ContainsEdge(equatableEdge2).Should().BeTrue();
            graph.ContainsEdge(equatableEdge3).Should().BeTrue();
            graph.ContainsEdge(equatableEdge4).Should().BeTrue();
            graph.ContainsEdge(equatableOtherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            graph.ContainsEdge(equatableEdge1).Should().BeTrue();
            graph.ContainsEdge(equatableEdge2).Should().BeTrue();
            graph.ContainsEdge(equatableEdge3).Should().BeTrue();
            graph.ContainsEdge(equatableEdge4).Should().BeTrue();
            graph.ContainsEdge(equatableOtherEdge1).Should().BeTrue();

            // Both vertices not in graph
            graph.ContainsEdge(new SEquatableEdge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            graph.ContainsEdge(new SEquatableEdge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            graph.ContainsEdge(new SEquatableEdge<int>(1, 0)).Should().BeFalse();
        }

        protected static void ContainsEdge_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            graph.ContainsEdge(edge1).Should().BeFalse();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            graph.AddVerticesAndEdge(edge1);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            graph.AddVerticesAndEdge(edge2);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            graph.AddVerticesAndEdge(edge3);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            graph.AddVerticesAndEdge(edge4);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            graph.AddVerticesAndEdge(otherEdge1);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            graph.ContainsEdge(new Edge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            graph.ContainsEdge(new Edge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            graph.ContainsEdge(new Edge<int>(1, 0)).Should().BeFalse();
        }

        protected static void ContainsEdge_ForbiddenParallelEdges_ImmutableVertices_Test(
            IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            graph.ContainsEdge(edge1).Should().BeFalse();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            graph.AddEdge(edge1);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            graph.AddEdge(edge2);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            graph.AddEdge(edge3);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            graph.AddEdge(edge4);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            graph.ContainsEdge(new Edge<int>(10, 11)).Should().BeFalse();
            // Source not in graph
            graph.ContainsEdge(new Edge<int>(10, 1)).Should().BeFalse();
            // Target not in graph
            graph.ContainsEdge(new Edge<int>(1, 10)).Should().BeFalse();
        }

        protected static void ContainsEdge_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IEdgeSet<int, SReversedEdge<int, Edge<int>>>> createGraph)
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

            graph.ContainsEdge(reversedEdge1).Should().BeFalse();
            graph.ContainsEdge(reversedEdge2).Should().BeFalse();
            graph.ContainsEdge(reversedEdge3).Should().BeFalse();
            graph.ContainsEdge(reversedEdge4).Should().BeFalse();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            graph.ContainsEdge(reversedEdge1).Should().BeTrue();
            graph.ContainsEdge(reversedEdge2).Should().BeFalse();
            graph.ContainsEdge(reversedEdge3).Should().BeFalse();
            graph.ContainsEdge(reversedEdge4).Should().BeFalse();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            graph.ContainsEdge(reversedEdge1).Should().BeTrue();
            graph.ContainsEdge(reversedEdge2).Should().BeTrue();
            graph.ContainsEdge(reversedEdge3).Should().BeFalse();
            graph.ContainsEdge(reversedEdge4).Should().BeFalse();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            graph.ContainsEdge(reversedEdge1).Should().BeTrue();
            graph.ContainsEdge(reversedEdge2).Should().BeTrue();
            graph.ContainsEdge(reversedEdge3).Should().BeTrue();
            graph.ContainsEdge(reversedEdge4).Should().BeFalse();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            graph.ContainsEdge(reversedEdge1).Should().BeTrue();
            graph.ContainsEdge(reversedEdge2).Should().BeTrue();
            graph.ContainsEdge(reversedEdge3).Should().BeTrue();
            graph.ContainsEdge(reversedEdge4).Should().BeTrue();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            graph.ContainsEdge(reversedEdge1).Should().BeTrue();
            graph.ContainsEdge(reversedEdge2).Should().BeTrue();
            graph.ContainsEdge(reversedEdge3).Should().BeTrue();
            graph.ContainsEdge(reversedEdge4).Should().BeTrue();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeTrue();

            // Both vertices not in graph
            graph.ContainsEdge(
                new SReversedEdge<int, Edge<int>>(
                    new Edge<int>(0, 10))).Should().BeFalse();
            // Source not in graph
            graph.ContainsEdge(
                new SReversedEdge<int, Edge<int>>(
                    new Edge<int>(0, 1))).Should().BeFalse();
            // Target not in graph
            graph.ContainsEdge(
                new SReversedEdge<int, Edge<int>>(
                    new Edge<int>(1, 0))).Should().BeFalse();
        }

        protected static void ContainsEdge_EquatableEdge_Test(
            IEdgeSet<int, EquatableEdge<int>> graph,
            [InstantHandle] Action<EquatableEdge<int>> addVerticesAndEdge)
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            graph.ContainsEdge(edge1).Should().BeFalse();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            addVerticesAndEdge(edge1);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            addVerticesAndEdge(edge2);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            addVerticesAndEdge(edge3);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            addVerticesAndEdge(edge4);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            addVerticesAndEdge(otherEdge1);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            graph.ContainsEdge(new EquatableEdge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            graph.ContainsEdge(new EquatableEdge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            graph.ContainsEdge(new EquatableEdge<int>(1, 0)).Should().BeFalse();
        }

        protected static void ContainsEdge_EquatableEdge_Test(
            IMutableVertexAndEdgeSet<int, EquatableEdge<int>> graph)
        {
            ContainsEdge_EquatableEdge_Test(
                graph,
                edge => graph.AddVerticesAndEdge(edge));
        }

        protected static void ContainsEdge_EquatableEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, EquatableEdge<int>> wrappedGraph,
            [InstantHandle] Func<IEdgeSet<int, EquatableEdge<int>>> createGraph)
        {
            IEdgeSet<int, EquatableEdge<int>> graph = createGraph();

            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            graph.ContainsEdge(edge1).Should().BeFalse();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            graph.ContainsEdge(new EquatableEdge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            graph.ContainsEdge(new EquatableEdge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            graph.ContainsEdge(new EquatableEdge<int>(1, 0)).Should().BeFalse();
        }

        protected static void ContainsEdge_EquatableEdge_EdgesOnly_Test(
            EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            graph.ContainsEdge(edge1).Should().BeFalse();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            graph.AddVerticesAndEdge(edge1);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            graph.AddVerticesAndEdge(edge2);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            graph.AddVerticesAndEdge(edge3);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            graph.AddVerticesAndEdge(edge4);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            graph.AddVerticesAndEdge(otherEdge1);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            graph.ContainsEdge(new EquatableEdge<int>(0, 10)).Should().BeFalse();
            // Source not in graph
            graph.ContainsEdge(new EquatableEdge<int>(0, 1)).Should().BeFalse();
            // Target not in graph
            graph.ContainsEdge(new EquatableEdge<int>(1, 0)).Should().BeFalse();
        }

        protected static void ContainsEdge_EquatableEdges_ForbiddenParallelEdges_ImmutableVertices_Test(
            IMutableEdgeListGraph<int, EquatableEdge<int>> graph)
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            graph.ContainsEdge(edge1).Should().BeFalse();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeFalse();

            graph.AddEdge(edge1);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeFalse();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            graph.AddEdge(edge2);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeFalse();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            graph.AddEdge(edge3);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeFalse();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            graph.AddEdge(edge4);
            graph.ContainsEdge(edge1).Should().BeTrue();
            graph.ContainsEdge(edge2).Should().BeTrue();
            graph.ContainsEdge(edge3).Should().BeTrue();
            graph.ContainsEdge(edge4).Should().BeTrue();
            graph.ContainsEdge(otherEdge1).Should().BeTrue();

            // Both vertices not in graph
            graph.ContainsEdge(new EquatableEdge<int>(10, 11)).Should().BeFalse();
            // Source not in graph
            graph.ContainsEdge(new EquatableEdge<int>(10, 1)).Should().BeFalse();
            // Target not in graph
            graph.ContainsEdge(new EquatableEdge<int>(1, 10)).Should().BeFalse();
        }

        protected static void ContainsEdge_EquatableEdge_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, EquatableEdge<int>> wrappedGraph,
            [InstantHandle] Func<IEdgeSet<int, SReversedEdge<int, EquatableEdge<int>>>> createGraph)
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

            graph.ContainsEdge(reversedEdge1).Should().BeFalse();
            graph.ContainsEdge(reversedEdge2).Should().BeFalse();
            graph.ContainsEdge(reversedEdge3).Should().BeFalse();
            graph.ContainsEdge(reversedEdge4).Should().BeFalse();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            graph.ContainsEdge(reversedEdge1).Should().BeTrue();
            graph.ContainsEdge(reversedEdge2).Should().BeFalse();
            graph.ContainsEdge(reversedEdge3).Should().BeFalse();
            graph.ContainsEdge(reversedEdge4).Should().BeFalse();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            graph.ContainsEdge(reversedEdge1).Should().BeTrue();
            graph.ContainsEdge(reversedEdge2).Should().BeTrue();
            graph.ContainsEdge(reversedEdge3).Should().BeFalse();
            graph.ContainsEdge(reversedEdge4).Should().BeFalse();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            graph.ContainsEdge(reversedEdge1).Should().BeTrue();
            graph.ContainsEdge(reversedEdge2).Should().BeTrue();
            graph.ContainsEdge(reversedEdge3).Should().BeTrue();
            graph.ContainsEdge(reversedEdge4).Should().BeFalse();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            graph.ContainsEdge(reversedEdge1).Should().BeTrue();
            graph.ContainsEdge(reversedEdge2).Should().BeTrue();
            graph.ContainsEdge(reversedEdge3).Should().BeTrue();
            graph.ContainsEdge(reversedEdge4).Should().BeTrue();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            graph.ContainsEdge(reversedEdge1).Should().BeTrue();
            graph.ContainsEdge(reversedEdge2).Should().BeTrue();
            graph.ContainsEdge(reversedEdge3).Should().BeTrue();
            graph.ContainsEdge(reversedEdge4).Should().BeTrue();
            graph.ContainsEdge(reversedOtherEdge1).Should().BeTrue();

            // Both vertices not in graph
            graph.ContainsEdge(
                new SReversedEdge<int, EquatableEdge<int>>(
                    new EquatableEdge<int>(0, 10))).Should().BeFalse();
            // Source not in graph
            graph.ContainsEdge(
                new SReversedEdge<int, EquatableEdge<int>>(
                    new EquatableEdge<int>(0, 1))).Should().BeFalse();
            // Target not in graph
            graph.ContainsEdge(
                new SReversedEdge<int, EquatableEdge<int>>(
                    new EquatableEdge<int>(1, 0))).Should().BeFalse();
        }

        protected static void ContainsEdge_SourceTarget_Test(
            IIncidenceGraph<int, Edge<int>> graph,
            [InstantHandle] Action<Edge<int>> addVerticesAndEdge)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            graph.ContainsEdge(1, 2).Should().BeFalse();
            graph.ContainsEdge(2, 1).Should().BeFalse();

            addVerticesAndEdge(edge1);
            graph.ContainsEdge(1, 2).Should().BeTrue();
            graph.ContainsEdge(2, 1).Should().BeFalse();

            addVerticesAndEdge(edge2);
            graph.ContainsEdge(1, 3).Should().BeTrue();
            graph.ContainsEdge(3, 1).Should().BeFalse();

            addVerticesAndEdge(edge3);
            graph.ContainsEdge(2, 2).Should().BeTrue();

            // Vertices is not present in the graph
            graph.ContainsEdge(0, 4).Should().BeFalse();
            graph.ContainsEdge(1, 4).Should().BeFalse();
            graph.ContainsEdge(4, 1).Should().BeFalse();
        }

        protected static void ContainsEdge_SourceTarget_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_Test(
                graph,
                edge => graph.AddVerticesAndEdge(edge));
        }

        protected static void ContainsEdge_SourceTarget_ImmutableGraph_Test<TEdge>(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IIncidenceGraph<int, TEdge>> createGraph)
            where TEdge : IEdge<int>
        {
            IIncidenceGraph<int, TEdge> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            graph.ContainsEdge(1, 2).Should().BeFalse();
            graph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            graph.ContainsEdge(1, 2).Should().BeTrue();
            graph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            graph.ContainsEdge(1, 3).Should().BeTrue();
            graph.ContainsEdge(3, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            graph.ContainsEdge(2, 2).Should().BeTrue();

            // Vertices is not present in the graph
            graph.ContainsEdge(0, 4).Should().BeFalse();
            graph.ContainsEdge(1, 4).Should().BeFalse();
            graph.ContainsEdge(4, 1).Should().BeFalse();
        }

        protected static void ContainsEdge_SourceTarget_ForbiddenParallelEdges_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            graph.ContainsEdge(1, 2).Should().BeFalse();
            graph.ContainsEdge(2, 1).Should().BeFalse();

            graph.AddEdge(edge1);
            graph.ContainsEdge(1, 2).Should().BeTrue();
            graph.ContainsEdge(2, 1).Should().BeFalse();

            graph.AddEdge(edge2);
            graph.ContainsEdge(1, 3).Should().BeTrue();
            graph.ContainsEdge(3, 1).Should().BeFalse();

            graph.AddEdge(edge3);
            graph.ContainsEdge(2, 2).Should().BeTrue();

            // Vertices is not present in the graph
            graph.ContainsEdge(4, 5).Should().BeFalse();
            graph.ContainsEdge(1, 4).Should().BeFalse();
            graph.ContainsEdge(4, 1).Should().BeFalse();
        }

        protected static void ContainsEdge_SourceTarget_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            IIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            graph.ContainsEdge(1, 2).Should().BeFalse();
            graph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            graph.ContainsEdge(1, 2).Should().BeFalse();
            graph.ContainsEdge(2, 1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            graph.ContainsEdge(1, 3).Should().BeFalse();
            graph.ContainsEdge(3, 1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            graph.ContainsEdge(2, 2).Should().BeTrue();

            // Vertices is not present in the graph
            graph.ContainsEdge(0, 4).Should().BeFalse();
            graph.ContainsEdge(1, 4).Should().BeFalse();
            graph.ContainsEdge(4, 1).Should().BeFalse();
        }

        protected static void ContainsEdge_SourceTarget_UndirectedGraph_Test(
            IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            graph.ContainsEdge(1, 2).Should().BeFalse();
            graph.ContainsEdge(2, 1).Should().BeFalse();

            graph.AddVerticesAndEdge(edge1);
            graph.ContainsEdge(1, 2).Should().BeTrue();
            graph.ContainsEdge(2, 1).Should().BeTrue();

            graph.AddVerticesAndEdge(edge2);
            graph.ContainsEdge(1, 3).Should().BeTrue();
            graph.ContainsEdge(3, 1).Should().BeTrue();

            graph.AddVerticesAndEdge(edge3);
            graph.ContainsEdge(2, 2).Should().BeTrue();

            // Vertices is not present in the graph
            graph.ContainsEdge(0, 4).Should().BeFalse();
            graph.ContainsEdge(1, 4).Should().BeFalse();
            graph.ContainsEdge(4, 1).Should().BeFalse();
        }

        protected static void ContainsEdge_SourceTarget_ImmutableGraph_UndirectedGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            [InstantHandle] Func<IImplicitUndirectedGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            IImplicitUndirectedGraph<int, Edge<int>> graph = createGraph();
            graph.ContainsEdge(1, 2).Should().BeFalse();
            graph.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            graph.ContainsEdge(1, 2).Should().BeTrue();
            graph.ContainsEdge(2, 1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            graph.ContainsEdge(1, 3).Should().BeTrue();
            graph.ContainsEdge(3, 1).Should().BeTrue();

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            graph.ContainsEdge(2, 2).Should().BeTrue();

            // Vertices is not present in the graph
            graph.ContainsEdge(0, 4).Should().BeFalse();
            graph.ContainsEdge(1, 4).Should().BeFalse();
            graph.ContainsEdge(4, 1).Should().BeFalse();
        }

        protected static void ContainsEdge_NullThrows_Test<TVertex, TEdge>(
            IEdgeSet<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
#pragma warning disable CS8625
            Invoking(() => graph.ContainsEdge(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        protected static void ContainsEdge_DefaultNullThrows_Test<TVertex>(
            IEdgeSet<TVertex, SEquatableEdge<TVertex>> graph)
            where TVertex : notnull
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Invoking(() => graph.ContainsEdge(default)).Should().Throw<ArgumentNullException>();
        }

        protected static void ContainsEdge_NullThrows_ReversedTest<TVertex, TEdge>(
            IEdgeSet<TVertex, SReversedEdge<TVertex, TEdge>> graph)
            where TVertex : notnull
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Invoking(() => graph.ContainsEdge(default)).Should().Throw<ArgumentNullException>();
        }

        protected static void ContainsEdge_SourceTarget_Throws_Test<TVertex, TEdge>(
            IIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : notnull, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.ContainsEdge(new TVertex(), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ContainsEdge(default, new TVertex())).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ContainsEdge(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void ContainsEdge_SourceTarget_Throws_UndirectedGraph_Test<TVertex, TEdge>(
            IImplicitUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8604
            Invoking(() => graph.ContainsEdge(new TVertex(), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ContainsEdge(default, new TVertex())).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.ContainsEdge(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }


        protected static void ContainsEdge_UndirectedEdge_UndirectedGraph_Test(
            IMutableUndirectedGraph<int, EquatableEdge<int>> graph1,
            IMutableUndirectedGraph<int, EquatableUndirectedEdge<int>> graph2)
        {
            ///////////////////////////////////
            // ContainsEdge => Source/Target //
            ///////////////////////////////////
            // Equatable Edge
            var equatableEdge1 = new EquatableEdge<int>(1, 2);
            var equatableEdge2 = new EquatableEdge<int>(1, 3);

            graph1.ContainsEdge(1, 2).Should().BeFalse();
            graph1.ContainsEdge(2, 1).Should().BeFalse();

            graph1.AddVerticesAndEdge(equatableEdge1);
            graph1.ContainsEdge(1, 2).Should().BeTrue();
            graph1.ContainsEdge(2, 1).Should().BeTrue();

            graph1.AddVerticesAndEdge(equatableEdge2);
            graph1.ContainsEdge(1, 3).Should().BeTrue();
            graph1.ContainsEdge(3, 1).Should().BeTrue();

            // Vertices is not present in the graph
            graph1.ContainsEdge(0, 4).Should().BeFalse();
            graph1.ContainsEdge(1, 4).Should().BeFalse();
            graph1.ContainsEdge(4, 1).Should().BeFalse();



            // Undirected equatable edge
            var equatableUndirectedEdge1 = new EquatableUndirectedEdge<int>(1, 2);
            var equatableUndirectedEdge2 = new EquatableUndirectedEdge<int>(1, 3);

            graph2.ContainsEdge(1, 2).Should().BeFalse();
            graph2.ContainsEdge(2, 1).Should().BeFalse();

            graph2.AddVerticesAndEdge(equatableUndirectedEdge1);
            graph2.ContainsEdge(1, 2).Should().BeTrue();
            graph2.ContainsEdge(2, 1).Should().BeTrue();

            graph2.AddVerticesAndEdge(equatableUndirectedEdge2);
            graph2.ContainsEdge(1, 3).Should().BeTrue();
            graph2.ContainsEdge(3, 1).Should().BeTrue();

            // Vertices is not present in the graph
            graph2.ContainsEdge(0, 4).Should().BeFalse();
            graph2.ContainsEdge(1, 4).Should().BeFalse();
            graph2.ContainsEdge(4, 1).Should().BeFalse();
        }

        protected static void ContainsEdge_UndirectedEdge_ImmutableGraph_UndirectedGraph_Test(
            IMutableVertexAndEdgeSet<int, EquatableEdge<int>> wrappedGraph1,
            [InstantHandle] Func<IImplicitUndirectedGraph<int, EquatableEdge<int>>> createEquatableEdgeGraph,
            IMutableVertexAndEdgeSet<int, EquatableUndirectedEdge<int>> wrappedGraph2,
            [InstantHandle] Func<IImplicitUndirectedGraph<int, EquatableUndirectedEdge<int>>> createEquatableUndirectedEdgeGraph)
        {
            ///////////////////////////////////
            // ContainsEdge => Source/Target //
            ///////////////////////////////////
            // Equatable Edge
            var equatableEdge1 = new EquatableEdge<int>(1, 2);
            var equatableEdge2 = new EquatableEdge<int>(1, 3);

            IImplicitUndirectedGraph<int, EquatableEdge<int>> graph1 = createEquatableEdgeGraph();
            graph1.ContainsEdge(1, 2).Should().BeFalse();
            graph1.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph1.AddVerticesAndEdge(equatableEdge1);
            graph1 = createEquatableEdgeGraph();
            graph1.ContainsEdge(1, 2).Should().BeTrue();
            graph1.ContainsEdge(2, 1).Should().BeTrue();

            wrappedGraph1.AddVerticesAndEdge(equatableEdge2);
            graph1 = createEquatableEdgeGraph();
            graph1.ContainsEdge(1, 3).Should().BeTrue();
            graph1.ContainsEdge(3, 1).Should().BeTrue();

            // Vertices is not present in the graph
            graph1.ContainsEdge(0, 4).Should().BeFalse();
            graph1.ContainsEdge(1, 4).Should().BeFalse();
            graph1.ContainsEdge(4, 1).Should().BeFalse();



            // Undirected equatable edge
            var equatableUndirectedEdge1 = new EquatableUndirectedEdge<int>(1, 2);
            var equatableUndirectedEdge2 = new EquatableUndirectedEdge<int>(1, 3);

            IImplicitUndirectedGraph<int, EquatableUndirectedEdge<int>> graph2 = createEquatableUndirectedEdgeGraph();
            graph2.ContainsEdge(1, 2).Should().BeFalse();
            graph2.ContainsEdge(2, 1).Should().BeFalse();

            wrappedGraph2.AddVerticesAndEdge(equatableUndirectedEdge1);
            graph2 = createEquatableUndirectedEdgeGraph();
            graph2.ContainsEdge(1, 2).Should().BeTrue();
            graph2.ContainsEdge(2, 1).Should().BeTrue();

            wrappedGraph2.AddVerticesAndEdge(equatableUndirectedEdge2);
            graph2 = createEquatableUndirectedEdgeGraph();
            graph2.ContainsEdge(1, 3).Should().BeTrue();
            graph2.ContainsEdge(3, 1).Should().BeTrue();

            // Vertices is not present in the graph
            graph2.ContainsEdge(0, 4).Should().BeFalse();
            graph2.ContainsEdge(1, 4).Should().BeFalse();
            graph2.ContainsEdge(4, 1).Should().BeFalse();
        }

        #endregion
    }
}
