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
            Assert.DoesNotThrow(
                // ReSharper disable once ObjectCreationAsStatement
                () => new ResidualEdgePredicate<int, Edge<int>>(
                    new Dictionary<Edge<int>, double>()));
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => new ResidualEdgePredicate<int, Edge<int>>(default));
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

            Assert.IsFalse(predicate.Test(edge12));
            Assert.IsFalse(predicate.Test(edge13));
            Assert.IsTrue(predicate.Test(edge31));
        }

        [Test]
        public void Predicate_Throws()
        {
            var predicate = new ResidualEdgePredicate<int, Edge<int>>(
                new Dictionary<Edge<int>, double>());

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => predicate.Test(default));
#pragma warning restore CS8625

            var edge12 = new Edge<int>(1, 2);
            Assert.Throws<KeyNotFoundException>(() => predicate.Test(edge12));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}
