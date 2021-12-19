#nullable enable

using JetBrains.Annotations;

namespace FastGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Contains Vertex

        protected static void ContainsVertex_Test(
            IMutableVertexSet<TestVertex> graph)
        {
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var otherVertex1 = new TestVertex("1");

            graph.ContainsVertex(vertex1).Should().BeFalse();
            graph.ContainsVertex(vertex2).Should().BeFalse();
            graph.ContainsVertex(otherVertex1).Should().BeFalse();

            graph.AddVertex(vertex1);
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeFalse();

            graph.AddVertex(vertex2);
            graph.ContainsVertex(vertex2).Should().BeTrue();

            graph.AddVertex(otherVertex1);
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeTrue();
        }

        protected static void ContainsVertex_ImmutableGraph_Test(
            IMutableVertexSet<TestVertex> wrappedGraph,
            [InstantHandle] Func<IImplicitVertexSet<TestVertex>> createGraph)
        {
            IImplicitVertexSet<TestVertex> graph = createGraph();

            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var otherVertex1 = new TestVertex("1");

            graph.ContainsVertex(vertex1).Should().BeFalse();
            graph.ContainsVertex(vertex2).Should().BeFalse();
            graph.ContainsVertex(otherVertex1).Should().BeFalse();

            wrappedGraph.AddVertex(vertex1);
            graph = createGraph();
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeFalse();

            wrappedGraph.AddVertex(vertex2);
            graph = createGraph();
            graph.ContainsVertex(vertex2).Should().BeTrue();

            wrappedGraph.AddVertex(otherVertex1);
            graph = createGraph();
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeTrue();
        }

        protected static void ContainsVertex_OnlyEdges_Test(
            EdgeListGraph<TestVertex, Edge<TestVertex>> graph)
        {
            var vertex1 = new TestVertex("1");
            var toVertex1 = new TestVertex("target 1");
            var vertex2 = new TestVertex("2");
            var toVertex2 = new TestVertex("target 2");
            var otherVertex1 = new TestVertex("1");
            var toOtherVertex1 = new TestVertex("target 1");

            graph.ContainsVertex(vertex1).Should().BeFalse();
            graph.ContainsVertex(vertex2).Should().BeFalse();
            graph.ContainsVertex(otherVertex1).Should().BeFalse();

            graph.AddEdge(new Edge<TestVertex>(vertex1, toVertex1));
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeFalse();

            graph.AddEdge(new Edge<TestVertex>(vertex2, toVertex2));
            graph.ContainsVertex(vertex2).Should().BeTrue();

            graph.AddEdge(new Edge<TestVertex>(otherVertex1, toOtherVertex1));
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeTrue();
        }

        protected static void ContainsVertex_EquatableVertex_Test(
            IMutableVertexSet<EquatableTestVertex> graph)
        {
            var vertex1 = new EquatableTestVertex("1");
            var vertex2 = new EquatableTestVertex("2");
            var otherVertex1 = new EquatableTestVertex("1");

            graph.ContainsVertex(vertex1).Should().BeFalse();
            graph.ContainsVertex(vertex2).Should().BeFalse();
            graph.ContainsVertex(otherVertex1).Should().BeFalse();

            graph.AddVertex(vertex1);
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeTrue();

            graph.AddVertex(vertex2);
            graph.ContainsVertex(vertex2).Should().BeTrue();

            graph.AddVertex(otherVertex1);
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeTrue();
        }

        protected static void ContainsVertex_EquatableVertex_ImmutableGraph_Test(
            IMutableVertexSet<EquatableTestVertex> wrappedGraph,
            [InstantHandle] Func<IImplicitVertexSet<EquatableTestVertex>> createGraph)
        {
            IImplicitVertexSet<EquatableTestVertex> graph = createGraph();

            var vertex1 = new EquatableTestVertex("1");
            var vertex2 = new EquatableTestVertex("2");
            var otherVertex1 = new EquatableTestVertex("1");

            graph.ContainsVertex(vertex1).Should().BeFalse();
            graph.ContainsVertex(vertex2).Should().BeFalse();
            graph.ContainsVertex(otherVertex1).Should().BeFalse();

            wrappedGraph.AddVertex(vertex1);
            graph = createGraph();
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeTrue();

            wrappedGraph.AddVertex(vertex2);
            graph = createGraph();
            graph.ContainsVertex(vertex2).Should().BeTrue();

            wrappedGraph.AddVertex(otherVertex1);
            graph = createGraph();
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeTrue();
        }

        protected static void ContainsVertex_EquatableVertex_OnlyEdges_Test(
            EdgeListGraph<EquatableTestVertex, Edge<EquatableTestVertex>> graph)
        {
            var vertex1 = new EquatableTestVertex("1");
            var toVertex1 = new EquatableTestVertex("target 1");
            var vertex2 = new EquatableTestVertex("2");
            var toVertex2 = new EquatableTestVertex("target 2");
            var otherVertex1 = new EquatableTestVertex("1");
            var toOtherVertex1 = new EquatableTestVertex("target 1");

            graph.ContainsVertex(vertex1).Should().BeFalse();
            graph.ContainsVertex(vertex2).Should().BeFalse();
            graph.ContainsVertex(otherVertex1).Should().BeFalse();

            graph.AddEdge(new Edge<EquatableTestVertex>(vertex1, toVertex1));
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeTrue();

            graph.AddEdge(new Edge<EquatableTestVertex>(vertex2, toVertex2));
            graph.ContainsVertex(vertex2).Should().BeTrue();

            graph.AddEdge(new Edge<EquatableTestVertex>(otherVertex1, toOtherVertex1));
            graph.ContainsVertex(vertex1).Should().BeTrue();
            graph.ContainsVertex(otherVertex1).Should().BeTrue();
        }

        protected static void ContainsVertex_Throws_Test<TVertex>(
            IImplicitVertexSet<TVertex> graph)
            where TVertex : notnull
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
#pragma warning disable CS8604
            Invoking(() => graph.ContainsVertex(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8604
        }

        #endregion
    }
}
