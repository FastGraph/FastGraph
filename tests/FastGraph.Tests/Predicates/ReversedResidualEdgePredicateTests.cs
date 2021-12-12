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
            Assert.DoesNotThrow(
                // ReSharper disable once ObjectCreationAsStatement
                () => new ReversedResidualEdgePredicate<int, Edge<int>>(
                    new Dictionary<Edge<int>, double>(),
                    new Dictionary<Edge<int>, Edge<int>>()));
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => new ReversedResidualEdgePredicate<int, Edge<int>>(default, new Dictionary<Edge<int>, Edge<int>>()));
            Assert.Throws<ArgumentNullException>(() => new ReversedResidualEdgePredicate<int, Edge<int>>(new Dictionary<Edge<int>, double>(), default));
            Assert.Throws<ArgumentNullException>(() => new ReversedResidualEdgePredicate<int, Edge<int>>(default, default));
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

            Assert.IsTrue(predicate.Test(edge12));
            Assert.IsFalse(predicate.Test(edge21));
            Assert.IsTrue(predicate.Test(edge13));
            Assert.IsFalse(predicate.Test(edge31));
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
            Assert.Throws<ArgumentNullException>(() => predicate.Test(default));

            var edge12 = new Edge<int>(1, 2);
            Assert.Throws<KeyNotFoundException>(() => predicate.Test(edge12));

            predicate.ReversedEdges.Add(edge12, new Edge<int>(2, 1));
            Assert.Throws<KeyNotFoundException>(() => predicate.Test(edge12));
#pragma warning restore CS8625
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}
