using System;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="ReversedBidirectionalGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class ReversedBidirectionalGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            var graph = new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);

            wrappedGraph = new BidirectionalGraph<int, Edge<int>>(true);
            graph = new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph);

            wrappedGraph = new BidirectionalGraph<int, Edge<int>>(false);
            graph = new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph);
            AssertGraphProperties(graph, false);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                ReversedBidirectionalGraph<TVertex, TEdge> g,
                bool parallelEdges = true)
                where TEdge : IEdge<TVertex>
            {
                Assert.IsTrue(g.IsDirected);
                Assert.AreEqual(parallelEdges, g.AllowParallelEdges);
                Assert.IsNotNull(g.OriginalGraph);
                AssertEmptyGraph(g);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new ReversedBidirectionalGraph<int, Edge<int>>(null));
        }

        #region Add Vertex => has effect

        [Test]
        public void AddVertex()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            AddVertex_ImmutableGraph_WithUpdate(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        #endregion

        #region Add Edge => has effect

        [Test]
        public void AddEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            var graph = new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph);

            var edge = new Edge<int>(1, 2);
            wrappedGraph.AddVertex(1);
            wrappedGraph.AddVertex(2);
            wrappedGraph.AddEdge(edge);

            AssertHasEdges(graph, new[] { edge });  // Graph is updated
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph));
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var wrappedGraph = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            ContainsVertex_EquatableVertex_ImmutableGraph_Test(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph));
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ReversedBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            ContainsEdge_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, EquatableEdge<int>>();
            ContainsEdge_EquatableEdge_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, EquatableEdge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            ContainsEdge_SourceTarget_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ReversedBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            ContainsEdge_NullThrows_ReversedTest(graph);
            ContainsEdge_SourceTarget_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            OutEdge_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void OutEdge_Throws()
        {
            var wrappedGraph1 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ReversedBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);
            OutEdge_NullThrows_Test(graph1);

            var wrappedGraph2 = new BidirectionalGraph<int, Edge<int>>();
            OutEdge_Throws_ImmutableGraph_ReversedTest(
                wrappedGraph2,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph2));
        }

        [Test]
        public void OutEdges()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            OutEdges_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void OutEdges_Throws()
        {
            var wrappedGraph1 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ReversedBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);
            OutEdges_NullThrows_Test(graph1);

            var wrappedGraph2 = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph2 = new ReversedBidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph2);
            OutEdges_Throws_Test(graph2);
        }

        #endregion

        #region In Edges

        [Test]
        public void InEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            InEdge_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void InEdge_Throws()
        {
            var wrappedGraph1 = new BidirectionalGraph<int, Edge<int>>();
            InEdge_Throws_ImmutableGraph_ReversedTest(
                wrappedGraph1,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph1));

            var wrappedGraph2 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph2 = new ReversedBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph2);
            InEdge_NullThrows_Test(graph2);
        }

        [Test]
        public void InEdges()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            InEdges_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void InEdges_Throws()
        {
            var wrappedGraph1 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph1 = new ReversedBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph1);
            InEdges_NullThrows_Test(graph1);

            var wrappedGraph2 = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph2 = new ReversedBidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph2);
            InEdges_Throws_Test(graph2);
        }

        #endregion

        [Test]
        public void Degree()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            Degree_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void Degree_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            var graph = new ReversedBidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>(wrappedGraph);
            Degree_Throws_Test(graph);
        }

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            TryGetEdge_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ReversedBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetEdge_Throws_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            TryGetEdges_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void TryGetEdges_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ReversedBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            TryGetOutEdges_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ReversedBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetOutEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetInEdges()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            TryGetInEdges_ImmutableGraph_ReversedTest(
                wrappedGraph,
                () => new ReversedBidirectionalGraph<int, Edge<int>>(wrappedGraph));
        }

        [Test]
        public void TryGetInEdges_Throws()
        {
            var wrappedGraph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var graph = new ReversedBidirectionalGraph<TestVertex, Edge<TestVertex>>(wrappedGraph);
            TryGetInEdges_Throws_Test(graph);
        }

        #endregion
    }
}