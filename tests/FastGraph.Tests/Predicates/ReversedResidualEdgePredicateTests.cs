#nullable enable

using NUnit.Framework;
using FastGraph.Predicates;

namespace FastGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="ReversedResidualEdgePredicate{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class ReversedResidualEdgePredicateTests
    {
        [Test]
        public void Construction()
        {
            Invoking((Func<ReversedResidualEdgePredicate<int, Edge<int>>>)(() => new ReversedResidualEdgePredicate<int, Edge<int>>(
                new Dictionary<Edge<int>, double>(),
                new Dictionary<Edge<int>, Edge<int>>()))).Should().NotThrow();
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new ReversedResidualEdgePredicate<int, Edge<int>>(default, new Dictionary<Edge<int>, Edge<int>>())).Should().Throw<ArgumentNullException>();
            Invoking(() => new ReversedResidualEdgePredicate<int, Edge<int>>(new Dictionary<Edge<int>, double>(), default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new ReversedResidualEdgePredicate<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Predicate()
        {
            var predicate = new ReversedResidualEdgePredicate<int, Edge<int>>(
                new Dictionary<Edge<int>, double>(),
                new Dictionary<Edge<int>, Edge<int>>());

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            var edge13 = new Edge<int>(1, 3);
            var edge31 = new Edge<int>(3, 1);
            predicate.ReversedEdges.Add(edge12, edge21);
            predicate.ReversedEdges.Add(edge21, edge12);
            predicate.ReversedEdges.Add(edge13, edge31);
            predicate.ReversedEdges.Add(edge31, edge13);
            predicate.ResidualCapacities.Add(edge12, -12);
            predicate.ResidualCapacities.Add(edge21, 12);
            predicate.ResidualCapacities.Add(edge13, 0);
            predicate.ResidualCapacities.Add(edge31, 1);

            predicate.Test(edge12).Should().BeTrue();
            predicate.Test(edge21).Should().BeFalse();
            predicate.Test(edge13).Should().BeTrue();
            predicate.Test(edge31).Should().BeFalse();
        }

        [Test]
        public void Predicate_Throws()
        {
            var predicate = new ReversedResidualEdgePredicate<int, Edge<int>>(
                new Dictionary<Edge<int>, double>(),
                new Dictionary<Edge<int>, Edge<int>>());

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => predicate.Test(default)).Should().Throw<ArgumentNullException>();

            var edge12 = new Edge<int>(1, 2);
            Invoking(() => predicate.Test(edge12)).Should().Throw<KeyNotFoundException>();

            predicate.ReversedEdges.Add(edge12, new Edge<int>(2, 1));
            Invoking(() => predicate.Test(edge12)).Should().Throw<KeyNotFoundException>();
#pragma warning restore CS8625
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}
