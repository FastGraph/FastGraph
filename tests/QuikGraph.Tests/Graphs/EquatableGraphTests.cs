using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests related to "equals" of graphs.
    /// </summary>
    [TestFixture]
    internal class EquatableGraphTests
    {
        #region Test helpers

        private class VertexTestComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x == y;
            }

            public int GetHashCode(int obj)
            {
                return obj.GetHashCode();
            }
        }

        private class EdgeTestComparer : IEqualityComparer<Edge<int>>
        {
            public bool Equals(Edge<int> x, Edge<int> y)
            {
                if (x is null)
                    return y is null;
                if (y is null)
                    return false;
                return x.Source == y.Source && x.Target == y.Target;
            }

            public int GetHashCode(Edge<int> obj)
            {
                return obj.GetHashCode();
            }
        }

        #endregion

        #region Test cases

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> EquateWithComparerTestCases
        {
            [UsedImplicitly]
            get
            {
                var vertexComparer = new VertexTestComparer();
                var edgeComparer = new EdgeTestComparer();

                yield return new TestCaseData(null, null, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                var emptyAdjacencyGraph1 = new AdjacencyGraph<int, Edge<int>>();
                var emptyAdjacencyGraph2 = new AdjacencyGraph<int, Edge<int>>();
                yield return new TestCaseData(emptyAdjacencyGraph1, null, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(null, emptyAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(emptyAdjacencyGraph1, emptyAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(emptyAdjacencyGraph1, emptyAdjacencyGraph2, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(emptyAdjacencyGraph2, emptyAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                var adjacencyGraph1 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph1.AddVertex(1);

                var adjacencyGraph2 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph2.AddVertex(1);

                yield return new TestCaseData(emptyAdjacencyGraph1, adjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph1, emptyAdjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph1, adjacencyGraph2, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                var edge12 = new Edge<int>(1, 2);
                var adjacencyGraph3 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph3.AddVerticesAndEdge(edge12);

                var adjacencyGraph4 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph4.AddVerticesAndEdge(edge12);

                var adjacencyGraph5 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph5.AddVertexRange(new[] { 1, 2 });

                yield return new TestCaseData(adjacencyGraph1, adjacencyGraph3, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph1, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph4, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph4, adjacencyGraph3, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph5, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph5, adjacencyGraph3, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                var edge12Bis = new Edge<int>(1, 2);
                var adjacencyGraph6 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph6.AddVerticesAndEdge(edge12Bis);

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph6, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph6, adjacencyGraph3, vertexComparer, edgeComparer)
                {
                    ExpectedResult = true
                };

                var edge13 = new Edge<int>(1, 3);
                var adjacencyGraph7 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph7.AddVerticesAndEdge(edge13);

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph7, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph7, adjacencyGraph3, vertexComparer, edgeComparer)
                {
                    ExpectedResult = false
                };
            }
        }

        #endregion

        [TestCaseSource(nameof(EquateWithComparerTestCases))]
        public bool EquateWithComparer(
            [NotNull] IVertexAndEdgeListGraph<int, Edge<int>> g,
            [NotNull] IVertexAndEdgeListGraph<int, Edge<int>> h,
            [NotNull] IEqualityComparer<int> vertexEquality,
            [NotNull] IEqualityComparer<Edge<int>> edgeEquality)
        {
            return EquateGraphs.Equate(g, h, vertexEquality, edgeEquality);
        }

        [Test]
        public void EquateWithComparer_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => EquateGraphs.Equate<int, Edge<int>>(null, null, EqualityComparer<int>.Default, null));
            Assert.Throws<ArgumentNullException>(
                () => EquateGraphs.Equate<int, Edge<int>>(null, null, null, EqualityComparer<Edge<int>>.Default));
            Assert.Throws<ArgumentNullException>(
                () => EquateGraphs.Equate<int, Edge<int>>(null, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #region Test cases

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> EquateTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(null, null)
                {
                    ExpectedResult = true
                };

                var emptyAdjacencyGraph1 = new AdjacencyGraph<int, Edge<int>>();
                var emptyAdjacencyGraph2 = new AdjacencyGraph<int, Edge<int>>();
                yield return new TestCaseData(emptyAdjacencyGraph1, null)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(null, emptyAdjacencyGraph1)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(emptyAdjacencyGraph1, emptyAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(emptyAdjacencyGraph1, emptyAdjacencyGraph2)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(emptyAdjacencyGraph2, emptyAdjacencyGraph1)
                {
                    ExpectedResult = true
                };

                var adjacencyGraph1 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph1.AddVertex(1);

                var adjacencyGraph2 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph2.AddVertex(1);

                yield return new TestCaseData(emptyAdjacencyGraph1, adjacencyGraph1)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph1, emptyAdjacencyGraph1)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph1, adjacencyGraph2)
                {
                    ExpectedResult = true
                };

                var edge12 = new Edge<int>(1, 2);
                var adjacencyGraph3 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph3.AddVerticesAndEdge(edge12);

                var adjacencyGraph4 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph4.AddVerticesAndEdge(edge12);

                var adjacencyGraph5 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph5.AddVertexRange(new[] { 1, 2 });

                yield return new TestCaseData(adjacencyGraph1, adjacencyGraph3)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph1)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph4)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph4, adjacencyGraph3)
                {
                    ExpectedResult = true
                };

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph5)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph5, adjacencyGraph3)
                {
                    ExpectedResult = false
                };

                var edge12Bis = new Edge<int>(1, 2);
                var adjacencyGraph6 = new AdjacencyGraph<int, Edge<int>>();
                adjacencyGraph6.AddVerticesAndEdge(edge12Bis);

                yield return new TestCaseData(adjacencyGraph3, adjacencyGraph6)
                {
                    ExpectedResult = false
                };

                yield return new TestCaseData(adjacencyGraph6, adjacencyGraph3)
                {
                    ExpectedResult = false
                };
            }
        }

        #endregion

        [TestCaseSource(nameof(EquateTestCases))]
        public bool Equate(
            [NotNull] IVertexAndEdgeListGraph<int, Edge<int>> g,
            [NotNull] IVertexAndEdgeListGraph<int, Edge<int>> h)
        {
            return EquateGraphs.Equate(g, h);
        }
    }
}
