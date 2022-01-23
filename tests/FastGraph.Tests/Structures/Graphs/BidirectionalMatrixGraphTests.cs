#nullable enable

using NUnit.Framework;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="BidirectionalMatrixGraph{TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class BidirectionalMatrixGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(1);
            AssertGraphProperties(graph, 1);

            graph = new BidirectionalMatrixGraph<Edge<int>>(12);
            AssertGraphProperties(graph, 12);

            #region Local function

            void AssertGraphProperties<TEdge>(
                BidirectionalMatrixGraph<TEdge> g,
                int nbVertices)
                where TEdge : class, IEdge<int>
            {
                g.IsDirected.Should().BeTrue();
                g.AllowParallelEdges.Should().BeFalse();
                AssertHasVertices(g, Enumerable.Range(0, nbVertices));
                AssertNoEdge(g);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            Invoking(() => new BidirectionalMatrixGraph<Edge<int>>(0)).Should().Throw<ArgumentException>();
            Invoking(() => new BidirectionalMatrixGraph<Edge<int>>(-1)).Should().Throw<ArgumentException>();
            Invoking(() => new BidirectionalMatrixGraph<Edge<int>>(-42)).Should().Throw<ArgumentException>();
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Add Edges

        [Test]
        public void AddEdge()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(5);
            AddEdge_ForbiddenParallelEdges_Test(graph);
        }

        [Test]
        public void AddEdge_EquatableEdge()
        {
            var graph = new BidirectionalMatrixGraph<EquatableEdge<int>>(5);
            AddEdge_EquatableEdge_ForbiddenParallelEdges_Test(graph);
        }

        [Test]
        public void AddEdge_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(2);

            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Invoking(() => graph.AddEdge(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            graph.AddEdge(new Edge<int>(0, 1));
            Invoking(() => graph.AddEdge(new Edge<int>(0, 1))).Should().Throw<ParallelEdgeNotAllowedException>();

            Invoking(() => graph.AddEdge(new Edge<int>(1, 2))).Should().Throw<VertexNotFoundException>();
            Invoking(() => graph.AddEdge(new Edge<int>(2, 1))).Should().Throw<VertexNotFoundException>();
        }

        [Test]
        public void AddEdgeRange()
        {
            AddEdgeRange_ForbiddenParallelEdges_Test();
        }

        [Test]
        public void AddEdgeRange_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);
            AddEdgeRange_ForbiddenParallelEdges_Throws_Test(graph);
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);

            graph.ContainsVertex(0).Should().BeTrue();
            graph.ContainsVertex(1).Should().BeTrue();
            graph.ContainsVertex(2).Should().BeTrue();
            graph.ContainsVertex(-1).Should().BeFalse();
            graph.ContainsVertex(12).Should().BeFalse();
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(4);
            ContainsEdge_ForbiddenParallelEdges_ImmutableVertices_Test(graph);
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var graph = new BidirectionalMatrixGraph<EquatableEdge<int>>(4);
            ContainsEdge_EquatableEdges_ForbiddenParallelEdges_ImmutableVertices_Test(graph);
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(4);
            ContainsEdge_SourceTarget_ForbiddenParallelEdges_Test(graph);
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);
            ContainsEdge_NullThrows_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(5);
            OutEdge_ImmutableVertices_Test(graph);
        }

        [Test]
        public void OutEdge_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);
            OutEdge_Throws_ImmutableVertices_Test(graph);
        }

        [Test]
        public void OutEdges()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(4);
            OutEdges_ImmutableVertices_Test(graph);
        }

        [Test]
        public void OutEdges_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(2);
            OutEdges_Throws_Matrix_Test(graph);
        }

        #endregion

        #region In Edges

        [Test]
        public void InEdge()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(5);
            InEdge_ImmutableVertices_Test(graph);
        }

        [Test]
        public void InEdge_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);
            InEdge_Throws_ImmutableVertices_Test(graph);
        }

        [Test]
        public void InEdges()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(4);
            InEdges_ImmutableVertices_Test(graph);
        }

        [Test]
        public void InEdges_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(2);
            InEdges_Throws_Matrix_Test(graph);
        }

        #endregion

        [Test]
        public void Degree()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(5);
            Degree_ImmutableVertices_Test(graph);
        }

        [Test]
        public void Degree_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(2);
            Degree_Throws_Matrix_Test(graph);
        }

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(5);
            TryGetEdge_ImmutableVertices_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(5);
            TryGetEdges_ImmutableVertices_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(6);
            TryGetOutEdges_ImmutableVertices_Test(graph);
        }

        [Test]
        public void TryGetInEdges()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(6);
            TryGetInEdges_ImmutableVertices_Test(graph);
        }

        #endregion

        #region Remove Edges

        [Test]
        public void RemoveEdge()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(4);
            RemoveEdge_ImmutableVertices_Test(graph);
        }

        [Test]
        public void RemoveEdge_EquatableEdge()
        {
            var graph = new BidirectionalMatrixGraph<EquatableEdge<int>>(4);
            RemoveEdge_EquatableEdge_ImmutableVertices_Test(graph);
        }

        [Test]
        public void RemoveEdge_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);
            RemoveEdge_Throws_Test(graph);
        }

        [Test]
        public void RemoveEdgeIf_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(1);
            Invoking(() => graph.RemoveEdgeIf(_ => true)).Should().Throw<NotSupportedException>();
        }

        [Test]
        public void RemoveOutEdgeIf()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(4);
            RemoveOutEdgeIf_ImmutableVertices_Test(graph);
        }

        [Test]
        public void RemoveOutEdgeIf_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(2);
            RemoveOutEdgeIf_Throws_Test(graph);
        }

        [Test]
        public void RemoveInEdgeIf()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(4);
            RemoveInEdgeIf_ImmutableVertices_Test(graph);
        }

        [Test]
        public void RemoveInEdgeIf_Throws()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(2);
            RemoveInEdgeIf_Throws_Test(graph);
        }

        #endregion

        #region Clear

        [Test]
        public void Clear()
        {
            int edgesRemoved = 0;

            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertHasVertices(graph, new[] { 0, 1, 2 });
            AssertNoEdge(graph);

            graph.Clear();

            AssertHasVertices(graph, new[] { 0, 1, 2 });
            AssertNoEdge(graph);
            CheckCounter(0);

            graph.AddEdge(new Edge<int>(0, 1));
            graph.AddEdge(new Edge<int>(1, 2));
            graph.AddEdge(new Edge<int>(2, 0));

            graph.Clear();

            AssertHasVertices(graph, new[] { 0, 1, 2 });
            AssertNoEdge(graph);
            CheckCounter(3);

            graph.AddEdge(new Edge<int>(0, 1));
            graph.AddEdge(new Edge<int>(2, 1));
            graph.AddEdge(new Edge<int>(2, 0));
            graph.AddEdge(new Edge<int>(2, 2));

            graph.Clear();

            AssertHasVertices(graph, new[] { 0, 1, 2 });
            AssertNoEdge(graph);
            CheckCounter(4);

            #region Local function

            void CheckCounter(int expectedEdgesRemoved)
            {
                edgesRemoved.Should().Be(expectedEdgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void ClearOutEdges()
        {
            int edgesRemoved = 0;

            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertNoEdge(graph);

            // Clear 6 => not in graph
            graph.ClearOutEdges(6);
            AssertNoEdge(graph);

            // Clear 0 => In graph but no out edges
            graph.ClearOutEdges(0);
            AssertNoEdge(graph);
            CheckCounter(0);

            var edge01 = new Edge<int>(0, 1);
            var edge12 = new Edge<int>(1, 2);
            graph.AddEdgeRange(new[] { edge01, edge12 });

            // Clear out 0
            graph.ClearOutEdges(0);

            AssertHasEdges(graph, new[] { edge12 });
            CheckCounter(1);

            var edge02 = new Edge<int>(0, 2);
            var edge20 = new Edge<int>(2, 0);
            var edge21 = new Edge<int>(2, 1);
            graph.AddEdgeRange(new[] { edge01, edge02, edge20, edge21 });

            // Clear out 2
            graph.ClearOutEdges(2);

            AssertHasEdges(graph, new[] { edge01, edge02, edge12 });
            CheckCounter(2);

            // Clear out 0
            graph.ClearOutEdges(0);

            AssertHasEdges(graph, new[] { edge12 });
            CheckCounter(2);

            // Clear out 1 = Clear
            graph.ClearOutEdges(1);

            AssertNoEdge(graph);
            CheckCounter(1);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void ClearInEdges()
        {
            int edgesRemoved = 0;

            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertNoEdge(graph);

            // Clear 6 => not in graph
            graph.ClearInEdges(6);
            AssertNoEdge(graph);
            CheckCounter(0);

            // Clear 0 => In graph but no in edges
            graph.ClearInEdges(0);
            AssertNoEdge(graph);
            CheckCounter(0);

            var edge01 = new Edge<int>(0, 1);
            var edge12 = new Edge<int>(1, 2);
            graph.AddEdgeRange(new[] { edge01, edge12 });

            // Clear in 1
            graph.ClearInEdges(1);

            AssertHasEdges(graph, new[] { edge12 });
            CheckCounter(1);

            var edge02 = new Edge<int>(0, 2);
            var edge20 = new Edge<int>(2, 0);
            var edge21 = new Edge<int>(2, 1);
            graph.AddEdgeRange(new[] { edge01, edge02, edge20, edge21 });

            // Clear in 2
            graph.ClearInEdges(2);

            AssertHasEdges(graph, new[] { edge01, edge20, edge21 });
            CheckCounter(2);

            // Clear in 0
            graph.ClearInEdges(0);

            AssertHasEdges(graph, new[] { edge01, edge21 });
            CheckCounter(1);

            // Clear 1 = Clear
            graph.ClearInEdges(1);

            AssertNoEdge(graph);
            CheckCounter(2);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void ClearEdges()
        {
            int edgesRemoved = 0;

            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                e.Should().NotBeNull();
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertNoEdge(graph);

            // Clear 6 => not in graph
            graph.ClearEdges(6);
            AssertNoEdge(graph);
            CheckCounter(0);

            // Clear 0 => In graph but not in/out edges
            graph.ClearEdges(0);
            AssertNoEdge(graph);
            CheckCounter(0);

            var edge01 = new Edge<int>(0, 1);
            var edge12 = new Edge<int>(1, 2);
            graph.AddEdgeRange(new[] { edge01, edge12 });

            // Clear 1
            graph.ClearEdges(1);

            AssertNoEdge(graph);
            CheckCounter(2);

            var edge02 = new Edge<int>(0, 2);
            var edge20 = new Edge<int>(2, 0);
            var edge21 = new Edge<int>(2, 1);
            graph.AddEdgeRange(new[] { edge01, edge02, edge20, edge21 });

            // Clear 2
            graph.ClearEdges(2);

            AssertHasEdges(graph, new[] { edge01 });
            CheckCounter(3);

            // Clear 0 = clear
            graph.ClearEdges(0);

            AssertNoEdge(graph);
            CheckCounter(1);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                edgesRemoved.Should().Be(expectedRemovedEdges);
                edgesRemoved = 0;
            }

            #endregion
        }

        #endregion

        [Test]
        public void Clone()
        {
            var graph = new BidirectionalMatrixGraph<Edge<int>>(1);
            AssertHasVertices(graph, new[] { 0 });
            AssertNoEdge(graph);

            var clonedGraph = graph.Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(graph, new[] { 0 });
            AssertNoEdge(graph);

            clonedGraph = (BidirectionalMatrixGraph<Edge<int>>)((ICloneable)graph).Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(graph, new[] { 0 });
            AssertNoEdge(graph);

            var edge1 = new Edge<int>(0, 1);
            var edge2 = new Edge<int>(0, 2);
            var edge3 = new Edge<int>(1, 2);
            graph = new BidirectionalMatrixGraph<Edge<int>>(3);
            graph.AddEdgeRange(new[] { edge1, edge2, edge3 });
            AssertHasVertices(graph, new[] { 0, 1, 2 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(clonedGraph, new[] { 0, 1, 2 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (BidirectionalMatrixGraph<Edge<int>>)((ICloneable)graph).Clone();
            clonedGraph.Should().NotBeNull();
            AssertHasVertices(clonedGraph, new[] { 0, 1, 2 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });
        }
    }
}
