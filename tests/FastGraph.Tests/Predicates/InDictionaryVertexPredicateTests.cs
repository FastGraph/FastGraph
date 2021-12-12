#nullable enable

using NUnit.Framework;
using FastGraph.Predicates;

namespace FastGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="InDictionaryVertexPredicate{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class InDictionaryVertexPredicateTests
    {
        [Test]
        public void Construction()
        {
            Assert.DoesNotThrow(
                // ReSharper disable once ObjectCreationAsStatement
                () => new InDictionaryVertexPredicate<int, Edge<int>>(
                    new Dictionary<int, Edge<int>>()));
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => new InDictionaryVertexPredicate<int, double>(default));
#pragma warning restore CS8625
        }

        [Test]
        public void Predicate()
        {
            var vertexMap = new Dictionary<int, double>();
            var predicate = new InDictionaryVertexPredicate<int, double>(vertexMap);

            Assert.IsFalse(predicate.Test(1));
            Assert.IsFalse(predicate.Test(2));

            vertexMap.Add(2, 12);
            Assert.IsFalse(predicate.Test(1));
            Assert.IsTrue(predicate.Test(2));

            vertexMap.Add(1, 42);
            Assert.IsTrue(predicate.Test(1));
            Assert.IsTrue(predicate.Test(2));

            vertexMap.Remove(2);
            Assert.IsTrue(predicate.Test(1));
            Assert.IsFalse(predicate.Test(2));

            vertexMap.Remove(3);
            Assert.IsTrue(predicate.Test(1));
            Assert.IsFalse(predicate.Test(2));

            vertexMap.Remove(1);
            Assert.IsFalse(predicate.Test(1));
            Assert.IsFalse(predicate.Test(2));
        }

        [Test]
        public void Predicate_Throws()
        {
            var predicate = new InDictionaryVertexPredicate<TestVertex, Edge<TestVertex>>(
                new Dictionary<TestVertex, Edge<TestVertex>>());

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => predicate.Test(default));
#pragma warning restore CS8625
        }
    }
}
