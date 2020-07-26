using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Predicates;

namespace QuikGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="SinkVertexPredicate{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal class SinkVertexPredicateTests
    {
        [Test]
        public void Construction()
        {
            Assert.DoesNotThrow(
                // ReSharper disable once ObjectCreationAsStatement
                () => new SinkVertexPredicate<int, Edge<int>>(
                    new AdjacencyGraph<int, Edge<int>>()));
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new SinkVertexPredicate<int, Edge<int>>(null));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> PredicateTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(new AdjacencyGraph<int, Edge<int>>());
                yield return new TestCaseData(new BidirectionalGraph<int, Edge<int>>());
            }
        }

        [TestCaseSource(nameof(PredicateTestCases))]
        public void Predicate<TGraph>([NotNull] TGraph graph)
            where TGraph 
            : IIncidenceGraph<int, Edge<int>>
            , IMutableVertexSet<int>
            , IMutableEdgeListGraph<int, Edge<int>>
        {
            var predicate = new SinkVertexPredicate<int, Edge<int>>(graph);

            graph.AddVertex(1);
            graph.AddVertex(2);
            Assert.IsTrue(predicate.Test(1));
            Assert.IsTrue(predicate.Test(2));

            graph.AddVertex(3);
            graph.AddEdge(new Edge<int>(1, 3));
            Assert.IsFalse(predicate.Test(1));
            Assert.IsTrue(predicate.Test(2));
            Assert.IsTrue(predicate.Test(3));

            graph.AddEdge(new Edge<int>(1, 2));
            Assert.IsFalse(predicate.Test(1));
            Assert.IsTrue(predicate.Test(2));
            Assert.IsTrue(predicate.Test(3));

            var edge23 = new Edge<int>(2, 3);
            graph.AddEdge(edge23);
            Assert.IsFalse(predicate.Test(1));
            Assert.IsFalse(predicate.Test(2));
            Assert.IsTrue(predicate.Test(3));

            graph.RemoveEdge(edge23);
            Assert.IsFalse(predicate.Test(1));
            Assert.IsTrue(predicate.Test(2));
            Assert.IsTrue(predicate.Test(3));
        }

        [Test]
        public void Predicate_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var predicate = new SinkVertexPredicate<TestVertex, Edge<TestVertex>>(graph);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => predicate.Test(new TestVertex("1")));
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => predicate.Test(null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}