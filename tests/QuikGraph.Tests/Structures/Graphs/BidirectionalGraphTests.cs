using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="BidirectionalGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class BidirectionalGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AssertGraphProperties(graph);

            graph = new BidirectionalGraph<int, Edge<int>>(true);
            AssertGraphProperties(graph);

            graph = new BidirectionalGraph<int, Edge<int>>(false);
            AssertGraphProperties(graph, false);

            graph = new BidirectionalGraph<int, Edge<int>>(true, 12);
            AssertGraphProperties(graph);

            graph = new BidirectionalGraph<int, Edge<int>>(false, 12);
            AssertGraphProperties(graph, false);

            graph = new BidirectionalGraph<int, Edge<int>>(true, 42, 12);
            AssertGraphProperties(graph, edgeCapacity: 12);

            graph = new BidirectionalGraph<int, Edge<int>>(false, 42, 12);
            AssertGraphProperties(graph, false, 12);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                BidirectionalGraph<TVertex, TEdge> g,
                bool parallelEdges = true,
                int edgeCapacity = 0)
                where TEdge : IEdge<TVertex>
            {
                Assert.IsTrue(g.IsDirected);
                Assert.AreEqual(parallelEdges, g.AllowParallelEdges);
                AssertEmptyGraph(g);
                Assert.AreEqual(edgeCapacity, g.EdgeCapacity);
                Assert.AreSame(typeof(int), g.VertexType);
                Assert.AreSame(typeof(Edge<int>), g.EdgeType);
            }

            #endregion
        }

        #region Add Vertices

        [Test]
        public void AddVertex()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            AddVertex_Test(graph);
        }

        [Test]
        public void AddVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            AddVertex_Throws_Test(graph);
        }

        [Test]
        public void AddVertex_EquatableVertex()
        {
            var graph = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            AddVertex_EquatableVertex_Test(graph);
        }

        [Test]
        public void AddVertexRange()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            AddVertexRange_Test(graph);
        }

        [Test]
        public void AddVertexRange_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            AddVertexRange_Throws_Test(graph);
        }

        #endregion

        #region Add Edges

        [Test]
        public void AddEdge_ParallelEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AddEdge_ParallelEdges_Test(graph);
        }

        [Test]
        public void AddEdge_ParallelEdges_EquatableEdge()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            AddEdge_ParallelEdges_EquatableEdge_Test(graph);
        }

        [Test]
        public void AddEdge_NoParallelEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>(false);
            AddEdge_NoParallelEdges_Test(graph);
        }

        [Test]
        public void AddEdge_NoParallelEdges_EquatableEdge()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>(false);
            AddEdge_NoParallelEdges_EquatableEdge_Test(graph);
        }

        [Test]
        public void AddEdge_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AddEdge_Throws_Test(graph);
        }

        [Test]
        public void AddEdgeRange()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>(false);
            AddEdgeRange_Test(graph);
        }

        [Test]
        public void AddEdgeRange_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AddEdgeRange_Throws_Test(graph);
        }

        #endregion

        #region Add Vertices & Edges

        [Test]
        public void AddVerticesAndEdge()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AddVerticesAndEdge_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdge_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AddVerticesAndEdge_Throws_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdgeRange()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>(false);
            AddVerticesAndEdgeRange_Test(graph);
        }

        [Test]
        public void AddVerticesAndEdgeRange_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>(false);
            AddVerticesAndEdgeRange_Throws_Test(graph);
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_Test(graph);
        }

        [Test]
        public void ContainsVertex_EquatableVertex()
        {
            var graph = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            ContainsVertex_EquatableVertex_Test(graph);
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Contains Edge

        [Test]
        public void ContainsEdge()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            ContainsEdge_Test(graph);
        }

        [Test]
        public void ContainsEdge_EquatableEdge()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            ContainsEdge_EquatableEdge_Test(graph);
        }

        [Test]
        public void ContainsEdge_SourceTarget()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            ContainsEdge_SourceTarget_Test(graph);
        }

        [Test]
        public void ContainsEdge_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            ContainsEdge_NullThrows_Test(graph);
            ContainsEdge_SourceTarget_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            OutEdge_Test(graph);
        }

        [Test]
        public void OutEdge_Throws()
        {
            var graph1 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            OutEdge_NullThrows_Test(graph1);

            var graph2 = new BidirectionalGraph<int, Edge<int>>();
            OutEdge_Throws_Test(graph2);
        }

        [Test]
        public void OutEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            OutEdges_Test(graph);
        }

        [Test]
        public void OutEdges_Throws()
        {
            var graph1 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            OutEdges_NullThrows_Test(graph1);

            var graph2 = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            OutEdges_Throws_Test(graph2);
        }

        #endregion

        #region In Edges

        [Test]
        public void InEdge()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            InEdge_Test(graph);
        }

        [Test]
        public void InEdge_Throws()
        {
            var graph1 = new BidirectionalGraph<int, Edge<int>>();
            InEdge_Throws_Test(graph1);

            var graph2 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            InEdge_NullThrows_Test(graph2);
        }

        [Test]
        public void InEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            InEdges_Test(graph);
        }

        [Test]
        public void InEdges_Throws()
        {
            var graph1 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            InEdges_NullThrows_Test(graph1);

            var graph2 = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            InEdges_Throws_Test(graph2);
        }

        #endregion

        [Test]
        public void Degree()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            Degree_Test(graph);
        }

        [Test]
        public void Degree_Throws()
        {
            var graph = new BidirectionalGraph<EquatableTestVertex, Edge<EquatableTestVertex>>();
            Degree_Throws_Test(graph);
        }

        #region Try Get Edges

        [Test]
        public void TryGetEdge()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            TryGetEdge_Test(graph);
        }

        [Test]
        public void TryGetEdge_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            TryGetEdge_Throws_Test(graph);
        }

        [Test]
        public void TryGetEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            TryGetEdges_Test(graph);
        }

        [Test]
        public void TryGetEdges_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            TryGetEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetOutEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            TryGetOutEdges_Test(graph);
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            TryGetOutEdges_Throws_Test(graph);
        }

        [Test]
        public void TryGetInEdges()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            TryGetInEdges_Test(graph);
        }

        [Test]
        public void TryGetInEdges_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            TryGetInEdges_Throws_Test(graph);
        }

        #endregion

        #region Merge

        public void Merge_Test(
            [NotNull] IEnumerable<int> setupVertices,
            [NotNull, ItemNotNull] IEnumerable<EquatableEdge<int>> setupEdges,
            int vertexToMerge,
            int expectedEdgesAdded,
            int expectedEdgesRemoved,
            [NotNull, ItemNotNull] IEnumerable<EquatableEdge<int>> expectedEdges)
        {
            int verticesAdded = 0;
            int edgesAdded = 0;
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();

            int[] verticesArray = setupVertices.ToArray();
            graph.AddVertexRange(verticesArray);
            graph.AddEdgeRange(setupEdges);

            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++verticesAdded;
            };
            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesAdded;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            graph.MergeVertex(vertexToMerge, (source, target) => new EquatableEdge<int>(source, target));
            CheckCounters();
            AssertHasVertices(graph, verticesArray.Except(new[] { vertexToMerge }));
            AssertHasEdges(graph, expectedEdges);

            #region Local function

            void CheckCounters()
            {
                Assert.AreEqual(0, verticesAdded);
                Assert.AreEqual(1, verticesRemoved);
                Assert.AreEqual(expectedEdgesAdded, edgesAdded);
                Assert.AreEqual(expectedEdgesRemoved, edgesRemoved);
                verticesRemoved = 0;
                edgesAdded = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void Merge1()
        {
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge13Bis = new EquatableEdge<int>(1, 3);
            var edge21 = new EquatableEdge<int>(2, 1);
            var edge23 = new EquatableEdge<int>(2, 3);
            var edge34 = new EquatableEdge<int>(3, 4);
            var edge35 = new EquatableEdge<int>(3, 5);
            var edge35Bis = new EquatableEdge<int>(3, 5);
            var edge45 = new EquatableEdge<int>(4, 5);

            Merge_Test(
                new[] { 1, 2, 3, 4, 5 },
                new[] { edge13, edge13Bis, edge21, edge23, edge34, edge35, edge35Bis, edge45 },
                3,
                9,
                6,
                new[]
                {
                    edge21, edge45,
                    new EquatableEdge<int>(1, 4),
                    new EquatableEdge<int>(1, 5),
                    new EquatableEdge<int>(1, 5),

                    new EquatableEdge<int>(1, 4),
                    new EquatableEdge<int>(1, 5),
                    new EquatableEdge<int>(1, 5),

                    new EquatableEdge<int>(2, 4),
                    new EquatableEdge<int>(2, 5),
                    new EquatableEdge<int>(2, 5)
                });
        }

        [Test]
        public void Merge2()
        {
            var edge23 = new EquatableEdge<int>(2, 3);
            var edge31 = new EquatableEdge<int>(3, 1);
            var edge33 = new EquatableEdge<int>(3, 3);
            var edge34 = new EquatableEdge<int>(3, 4);

            Merge_Test(
                new[] { 1, 2, 3, 4 },
                new[] { edge23, edge31, edge33, edge34 },
                3,
                2,
                4,
                new[]
                {
                    new EquatableEdge<int>(2, 1),
                    new EquatableEdge<int>(2, 4)
                });
        }

        [Test]
        public void Merge3()
        {
            var edge34 = new EquatableEdge<int>(3, 4);

            Merge_Test(
                new[] { 1, 2, 3, 4 },
                new[] { edge34 },
                1,
                0,
                0,
                new[] { edge34 });
        }

        [Test]
        public void Merge_Throws()
        {
            var graph1 = new BidirectionalGraph<int, Edge<int>>();
            Assert.Throws<VertexNotFoundException>(
                () => graph1.MergeVertex(1, (source, target) => new Edge<int>(source, target)));

            var graph2 = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            Assert.Throws<ArgumentNullException>(
                // ReSharper disable AssignNullToNotNullAttribute
                () => graph2.MergeVertex(null, (source, target) => new Edge<TestVertex>(source, target)));
            Assert.Throws<ArgumentNullException>(
                () => graph2.MergeVertex(new TestVertex("1"), null));
            Assert.Throws<ArgumentNullException>(
                () => graph2.MergeVertex(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public void MergeIf_Test(
            [NotNull] IEnumerable<int> setupVertices,
            [NotNull, ItemNotNull] IEnumerable<EquatableEdge<int>> setupEdges,
            [NotNull, InstantHandle] VertexPredicate<int> vertexPredicate,
            int expectedVerticesRemoved,
            int expectedEdgesAdded,
            int expectedEdgesRemoved,
            [NotNull] IEnumerable<int> expectedVertices,
            [NotNull, ItemNotNull] IEnumerable<EquatableEdge<int>> expectedEdges)
        {
            int verticesAdded = 0;
            int edgesAdded = 0;
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();

            graph.AddVertexRange(setupVertices);
            graph.AddEdgeRange(setupEdges);

            graph.VertexAdded += v =>
            {
                Assert.IsNotNull(v);
                ++verticesAdded;
            };
            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            graph.EdgeAdded += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesAdded;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            graph.MergeVerticesIf(vertexPredicate, (source, target) => new EquatableEdge<int>(source, target));
            CheckCounters();
            AssertHasVertices(graph, expectedVertices);
            EquatableEdge<int>[] edges = expectedEdges.ToArray();
            if (!edges.Any())
                AssertNoEdge(graph);
            else
                AssertHasEdges(graph, edges);

            #region Local function

            void CheckCounters()
            {
                Assert.AreEqual(0, verticesAdded);
                Assert.AreEqual(expectedVerticesRemoved, verticesRemoved);
                Assert.AreEqual(expectedEdgesAdded, edgesAdded);
                Assert.AreEqual(expectedEdgesRemoved, edgesRemoved);
                verticesRemoved = 0;
                edgesAdded = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void MergeIf1()
        {
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge13Bis = new EquatableEdge<int>(1, 3);
            var edge21 = new EquatableEdge<int>(2, 1);
            var edge23 = new EquatableEdge<int>(2, 3);
            var edge34 = new EquatableEdge<int>(3, 4);
            var edge35 = new EquatableEdge<int>(3, 5);
            var edge35Bis = new EquatableEdge<int>(3, 5);
            var edge45 = new EquatableEdge<int>(4, 5);

            MergeIf_Test(
                new[] { 1, 2, 3, 4, 5 },
                new[] { edge13, edge13Bis, edge21, edge23, edge34, edge35, edge35Bis, edge45 },
                vertex => vertex == 3 || vertex == 4,
                1 + 1,
                9 + 3,
                6 + 4,
                new[] { 1, 2, 5 },
                new[]
                {
                    edge21,
                    new EquatableEdge<int>(1, 5),
                    new EquatableEdge<int>(1, 5),

                    new EquatableEdge<int>(1, 5),
                    new EquatableEdge<int>(1, 5),

                    new EquatableEdge<int>(2, 5),
                    new EquatableEdge<int>(2, 5),


                    new EquatableEdge<int>(1, 5),
                    new EquatableEdge<int>(1, 5),
                    new EquatableEdge<int>(2, 5)
                });
        }

        [Test]
        public void MergeIf2()
        {
            var edge23 = new EquatableEdge<int>(2, 3);
            var edge31 = new EquatableEdge<int>(3, 1);
            var edge33 = new EquatableEdge<int>(3, 3);
            var edge34 = new EquatableEdge<int>(3, 4);

            MergeIf_Test(
                new[] { 1, 2, 3, 4 },
                new[] { edge23, edge31, edge33, edge34 },
                vertex => vertex == 3 || vertex == 4,
                1 + 1,
                2 + 0,
                4 + 1,
                new[] { 1, 2 },
                new[]
                {
                    new EquatableEdge<int>(2, 1)
                });
        }

        [Test]
        public void MergeIf3()
        {
            var edge34 = new EquatableEdge<int>(3, 4);

            MergeIf_Test(
                new[] { 1, 2, 3, 4 },
                new[] { edge34 },
                vertex => vertex == 1 || vertex == 2,
               1 + 1,
                0 + 0,
                0 + 0,
                new[] { 3, 4 },
                new[] { edge34 });
        }

        [Test]
        public void MergeIf4()
        {
            var edge34 = new EquatableEdge<int>(3, 4);

            MergeIf_Test(
                new[] { 1, 2, 3, 4 },
                new[] { edge34 },
                vertex => vertex == 1 || vertex == 3,
                1 + 1,
                0 + 0,
                0 + 1,
                new[] { 2, 4 },
                Enumerable.Empty<EquatableEdge<int>>());
        }

        [Test]
        public void MergeIf_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.MergeVerticesIf(null, (source, target) => new Edge<int>(source, target)));
            Assert.Throws<ArgumentNullException>(() => graph.MergeVerticesIf(vertex => true, null));
            Assert.Throws<ArgumentNullException>(() => graph.MergeVerticesIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #region Remove Vertices

        [Test]
        public void RemoveVertex()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            RemoveVertex_Test(graph);
        }

        [Test]
        public void RemoveVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            RemoveVertex_Throws_Test(graph);
        }

        [Test]
        public void RemoveVertexIf()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            RemoveVertexIf_Test(graph);
        }

        [Test]
        public void RemoveVertexIf_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            RemoveVertexIf_Throws_Test(graph);
        }

        #endregion

        #region Remove Edges

        [Test]
        public void RemoveEdge()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            RemoveEdge_Test(graph);
        }

        [Test]
        public void RemoveEdge_EquatableEdge()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            RemoveEdge_EquatableEdge_Test(graph);
        }

        [Test]
        public void RemoveEdge_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            RemoveEdge_Throws_Test(graph);
        }

        [Test]
        public void RemoveEdgeIf()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            RemoveEdgeIf_Test(graph);
        }

        [Test]
        public void RemoveEdgeIf_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            RemoveEdgeIf_Throws_Test(graph);
        }

        [Test]
        public void RemoveOutEdgeIf()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            RemoveOutEdgeIf_Test(graph);
        }

        [Test]
        public void RemoveOutEdgeIf_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            RemoveOutEdgeIf_Throws_Test(graph);
        }

        [Test]
        public void RemoveInEdgeIf()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            RemoveInEdgeIf_Test(graph);
        }

        [Test]
        public void RemoveInEdgeIf_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            RemoveInEdgeIf_Throws_Test(graph);
        }

        #endregion

        #region Clear

        [Test]
        public void Clear()
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.VertexRemoved += v =>
            {
                Assert.IsNotNull(v);
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertEmptyGraph(graph);

            graph.Clear();
            AssertEmptyGraph(graph);
            CheckCounters(0, 0);

            graph.AddVerticesAndEdge(new Edge<int>(1, 2));
            graph.AddVerticesAndEdge(new Edge<int>(2, 3));
            graph.AddVerticesAndEdge(new Edge<int>(3, 1));

            graph.Clear();
            AssertEmptyGraph(graph);
            CheckCounters(3, 3);

            graph.AddVerticesAndEdge(new Edge<int>(1, 2));
            graph.AddVerticesAndEdge(new Edge<int>(3, 2));
            graph.AddVerticesAndEdge(new Edge<int>(3, 1));
            graph.AddVerticesAndEdge(new Edge<int>(3, 3));

            graph.Clear();
            AssertEmptyGraph(graph);
            CheckCounters(3, 4);

            #region Local function

            void CheckCounters(int expectedVerticesRemoved, int expectedEdgesRemoved)
            {
                Assert.AreEqual(expectedVerticesRemoved, verticesRemoved);
                Assert.AreEqual(expectedEdgesRemoved, edgesRemoved);
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void ClearOutEdges()
        {
            int edgesRemoved = 0;

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertEmptyGraph(graph);

            // Clear 1 => not in graph
            graph.ClearOutEdges(1);
            AssertEmptyGraph(graph);

            // Clear 1 => In graph but no out edges
            graph.AddVertex(1);
            graph.ClearOutEdges(1);
            AssertHasVertices(graph, new[] { 1 });
            AssertNoEdge(graph);
            CheckCounter(0);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });

            // Clear out 1
            graph.ClearOutEdges(1);

            AssertHasEdges(graph, new[] { edge23 });
            CheckCounter(1);

            var edge13 = new Edge<int>(1, 3);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge31, edge32 });

            // Clear out 3
            graph.ClearOutEdges(3);

            AssertHasEdges(graph, new[] { edge12, edge13, edge23 });
            CheckCounter(2);

            // Clear out 1
            graph.ClearOutEdges(1);

            AssertHasEdges(graph, new[] { edge23 });
            CheckCounter(2);

            // Clear out 2 = Clear
            graph.ClearOutEdges(2);

            AssertNoEdge(graph);
            CheckCounter(1);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void ClearOutEdges_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new BidirectionalGraph<TestVertex, Edge<TestVertex>>().ClearOutEdges(null));
        }

        [Test]
        public void ClearInEdges()
        {
            int edgesRemoved = 0;

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertEmptyGraph(graph);

            // Clear 1 => not in graph
            graph.ClearInEdges(1);
            AssertEmptyGraph(graph);
            CheckCounter(0);

            // Clear 1 => In graph but no in edges
            graph.AddVertex(1);
            graph.ClearInEdges(1);
            AssertHasVertices(graph, new[] { 1 });
            AssertNoEdge(graph);
            CheckCounter(0);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });

            // Clear in 2
            graph.ClearInEdges(2);

            AssertHasEdges(graph, new[] { edge23 });
            CheckCounter(1);

            var edge13 = new Edge<int>(1, 3);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge31, edge32 });

            // Clear in 3
            graph.ClearInEdges(3);

            AssertHasEdges(graph, new[] { edge12, edge31, edge32 });
            CheckCounter(2);

            // Clear in 1
            graph.ClearInEdges(1);

            AssertHasEdges(graph, new[] { edge12, edge32 });
            CheckCounter(1);

            // Clear 2 = Clear
            graph.ClearInEdges(2);

            AssertNoEdge(graph);
            CheckCounter(2);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void ClearInEdges_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new BidirectionalGraph<TestVertex, Edge<TestVertex>>().ClearInEdges(null));
        }

        [Test]
        public void ClearEdges()
        {
            int edgesRemoved = 0;

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.EdgeRemoved += e =>
            {
                Assert.IsNotNull(e);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            AssertEmptyGraph(graph);

            // Clear 1 => not in graph
            graph.ClearEdges(1);
            AssertEmptyGraph(graph);
            CheckCounter(0);

            // Clear 1 => In graph but not in/out edges
            graph.AddVertex(1);
            graph.ClearEdges(1);
            AssertHasVertices(graph, new[] { 1 });
            AssertNoEdge(graph);
            CheckCounter(0);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });

            // Clear 2
            graph.ClearEdges(2);

            AssertNoEdge(graph);
            CheckCounter(2);

            var edge13 = new Edge<int>(1, 3);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge13, edge31, edge32 });

            // Clear 3
            graph.ClearEdges(3);

            AssertHasEdges(graph, new[] { edge12 });
            CheckCounter(3);

            // Clear 1 = clear
            graph.ClearEdges(1);

            AssertNoEdge(graph);
            CheckCounter(1);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.AreEqual(expectedRemovedEdges, edgesRemoved);
                edgesRemoved = 0;
            }

            #endregion
        }

        [Test]
        public void ClearEdges_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new BidirectionalGraph<TestVertex, Edge<TestVertex>>().ClearEdges(null));
        }

        #endregion

        [Test]
        public void Clone()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            AssertEmptyGraph(graph);

            var clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            clonedGraph = new BidirectionalGraph<int, Edge<int>>(graph);
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            clonedGraph = (BidirectionalGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertEmptyGraph(clonedGraph);

            graph.AddVertexRange(new[] { 1, 2, 3 });
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertNoEdge(graph);

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            clonedGraph = (BidirectionalGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertNoEdge(clonedGraph);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            graph.AddVerticesAndEdgeRange(new[] { edge1, edge2, edge3 });
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = new BidirectionalGraph<int, Edge<int>>(graph);
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (BidirectionalGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            graph.AddVertex(4);
            AssertHasVertices(graph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(graph, new[] { edge1, edge2, edge3 });

            clonedGraph = graph.Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });

            clonedGraph = (BidirectionalGraph<int, Edge<int>>)((ICloneable)graph).Clone();
            Assert.IsNotNull(clonedGraph);
            AssertHasVertices(clonedGraph, new[] { 1, 2, 3, 4 });
            AssertHasEdges(clonedGraph, new[] { edge1, edge2, edge3 });
        }

        [Test]
        public void Clone_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new BidirectionalGraph<int, Edge<int>>(null));
        }

        [Test]
        public void TrimEdgeExcess()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>(true, 12)
            {
                EdgeCapacity = 50
            };

            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(1, 4)
            });

            Assert.DoesNotThrow(() => graph.TrimEdgeExcess());
        }
    }
}