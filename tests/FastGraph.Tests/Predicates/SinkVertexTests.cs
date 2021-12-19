#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Predicates;

namespace FastGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="SinkVertexPredicate{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class SinkVertexPredicateTests
    {
        [Test]
        public void Construction()
        {
            Invoking((Func<SinkVertexPredicate<int, Edge<int>>>)(() => new SinkVertexPredicate<int, Edge<int>>(
                new AdjacencyGraph<int, Edge<int>>()))).Should().NotThrow();
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new SinkVertexPredicate<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

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
        public void Predicate<TGraph>(TGraph graph)
            where TGraph
            : IIncidenceGraph<int, Edge<int>>
            , IMutableVertexSet<int>
            , IMutableEdgeListGraph<int, Edge<int>>
        {
            var predicate = new SinkVertexPredicate<int, Edge<int>>(graph);

            graph.AddVertex(1);
            graph.AddVertex(2);
            predicate.Test(1).Should().BeTrue();
            predicate.Test(2).Should().BeTrue();

            graph.AddVertex(3);
            graph.AddEdge(new Edge<int>(1, 3));
            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeTrue();
            predicate.Test(3).Should().BeTrue();

            graph.AddEdge(new Edge<int>(1, 2));
            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeTrue();
            predicate.Test(3).Should().BeTrue();

            var edge23 = new Edge<int>(2, 3);
            graph.AddEdge(edge23);
            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeFalse();
            predicate.Test(3).Should().BeTrue();

            graph.RemoveEdge(edge23);
            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeTrue();
            predicate.Test(3).Should().BeTrue();
        }

        [Test]
        public void Predicate_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var predicate = new SinkVertexPredicate<TestVertex, Edge<TestVertex>>(graph);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => predicate.Test(new TestVertex("1"))).Should().Throw<VertexNotFoundException>();
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => predicate.Test(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}
