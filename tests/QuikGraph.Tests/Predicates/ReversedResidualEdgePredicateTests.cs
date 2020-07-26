using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Predicates;

namespace QuikGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="ReversedResidualEdgePredicate{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal class ReversedResidualEdgePredicateTests
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
            Assert.Throws<ArgumentNullException>(() => new ReversedResidualEdgePredicate<int, Edge<int>>(null, new Dictionary<Edge<int>, Edge<int>>()));
            Assert.Throws<ArgumentNullException>(() => new ReversedResidualEdgePredicate<int, Edge<int>>(new Dictionary<Edge<int>, double>(), null));
            Assert.Throws<ArgumentNullException>(() => new ReversedResidualEdgePredicate<int, Edge<int>>(null, null));
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
            Assert.Throws<ArgumentNullException>(() => predicate.Test(null));

            var edge12 = new Edge<int>(1, 2);
            Assert.Throws<KeyNotFoundException>(() => predicate.Test(edge12));

            predicate.ReversedEdges.Add(edge12, new Edge<int>(2, 1));
            Assert.Throws<KeyNotFoundException>(() => predicate.Test(edge12));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}