#nullable enable

using NUnit.Framework;
using FastGraph.Predicates;

namespace FastGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="ResidualEdgePredicate{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class ResidualEdgePredicateTests
    {
        [Test]
        public void Construction()
        {
            Invoking((Func<ResidualEdgePredicate<int, Edge<int>>>)(() => new ResidualEdgePredicate<int, Edge<int>>(
                new Dictionary<Edge<int>, double>()))).Should().NotThrow();
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new ResidualEdgePredicate<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void Predicate()
        {
            var predicate = new ResidualEdgePredicate<int, Edge<int>>(
                new Dictionary<Edge<int>, double>());

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge31 = new Edge<int>(3, 1);
            predicate.ResidualCapacities.Add(edge12, -12);
            predicate.ResidualCapacities.Add(edge13, 0);
            predicate.ResidualCapacities.Add(edge31, 1);

            predicate.Test(edge12).Should().BeFalse();
            predicate.Test(edge13).Should().BeFalse();
            predicate.Test(edge31).Should().BeTrue();
        }

        [Test]
        public void Predicate_Throws()
        {
            var predicate = new ResidualEdgePredicate<int, Edge<int>>(
                new Dictionary<Edge<int>, double>());

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => predicate.Test(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625

            var edge12 = new Edge<int>(1, 2);
            Invoking(() => predicate.Test(edge12)).Should().Throw<KeyNotFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}
