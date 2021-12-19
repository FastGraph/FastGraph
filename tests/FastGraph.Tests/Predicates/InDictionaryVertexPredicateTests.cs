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
            Invoking((Func<InDictionaryVertexPredicate<int, Edge<int>>>)(() => new InDictionaryVertexPredicate<int, Edge<int>>(
                new Dictionary<int, Edge<int>>()))).Should().NotThrow();
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new InDictionaryVertexPredicate<int, double>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void Predicate()
        {
            var vertexMap = new Dictionary<int, double>();
            var predicate = new InDictionaryVertexPredicate<int, double>(vertexMap);

            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeFalse();

            vertexMap.Add(2, 12);
            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeTrue();

            vertexMap.Add(1, 42);
            predicate.Test(1).Should().BeTrue();
            predicate.Test(2).Should().BeTrue();

            vertexMap.Remove(2);
            predicate.Test(1).Should().BeTrue();
            predicate.Test(2).Should().BeFalse();

            vertexMap.Remove(3);
            predicate.Test(1).Should().BeTrue();
            predicate.Test(2).Should().BeFalse();

            vertexMap.Remove(1);
            predicate.Test(1).Should().BeFalse();
            predicate.Test(2).Should().BeFalse();
        }

        [Test]
        public void Predicate_Throws()
        {
            var predicate = new InDictionaryVertexPredicate<TestVertex, Edge<TestVertex>>(
                new Dictionary<TestVertex, Edge<TestVertex>>());

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => predicate.Test(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }
    }
}
