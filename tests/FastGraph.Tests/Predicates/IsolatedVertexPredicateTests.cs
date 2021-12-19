#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Predicates;

namespace FastGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="IsolatedVertexPredicate{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class IsolatedVertexPredicateTests
    {
        [Test]
        public void Construction()
        {
            Invoking((Func<IsolatedVertexPredicate<int, Edge<int>>>)(() => new IsolatedVertexPredicate<int, Edge<int>>(
                new BidirectionalGraph<int, Edge<int>>()))).Should().NotThrow();
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new IsolatedVertexPredicate<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        private static IEnumerable<TestCaseData> PredicateTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(new BidirectionalGraph<int, Edge<int>>());
            }
        }

        [TestCaseSource(nameof(PredicateTestCases))]
        public void Predicate<TGraph>(TGraph graph)
            where TGraph
            : IBidirectionalGraph<int, Edge<int>>
            , IMutableVertexSet<int>
            , IMutableEdgeListGraph<int, Edge<int>>
        {
            var predicate = new IsolatedVertexPredicate<int, Edge<int>>(graph);

            graph.AddVertex(1);
            graph.AddVertex(2);
            predicate.Test(1).Should().BeTrue();
            predicate.Test(2).Should().BeTrue();

            graph.AddVertex(3);
            var edge13 = new Edge<int>(1, 3);
            graph.AddEdge(edge13);
            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeTrue();
            predicate.Test(3).Should().BeFalse();

            var edge12 = new Edge<int>(1, 2);
            graph.AddEdge(edge12);
            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeFalse();
            predicate.Test(3).Should().BeFalse();

            var edge23 = new Edge<int>(2, 3);
            graph.AddEdge(edge23);
            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeFalse();
            predicate.Test(3).Should().BeFalse();

            graph.RemoveEdge(edge23);
            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeFalse();
            predicate.Test(3).Should().BeFalse();

            graph.RemoveEdge(edge12);
            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeTrue();
            predicate.Test(3).Should().BeFalse();

            graph.RemoveEdge(edge13);
            predicate.Test(1).Should().BeTrue();
            predicate.Test(2).Should().BeTrue();
            predicate.Test(3).Should().BeTrue();
        }

        [Test]
        public void Predicate_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var predicate = new IsolatedVertexPredicate<TestVertex, Edge<TestVertex>>(graph);

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
