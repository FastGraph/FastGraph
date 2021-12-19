using FluentAssertions.Execution;

#nullable enable

namespace FastGraph.Tests.Structures
{
    /// <summary>
    /// Base class for tests relative to edges structures.
    ///</summary>
    internal abstract class EdgeTestsBase
    {
        [CustomAssertion]
        private static void AssertAreEqual<T>(T? left, T? right)
        {
            bool isValueType = typeof(T).IsValueType;
            if (isValueType)
                right.Should().Be(left);
            else
                right.Should().BeSameAs(left);
        }

        [CustomAssertion]
        protected static void CheckStructEdge<T>(IEdge<T> edge, T? source, T? target)
            where T : notnull
        {
            using (_ = new AssertionScope())
            {
                AssertAreEqual(source, edge.Source);
                AssertAreEqual(target, edge.Target);
            }
        }

        [CustomAssertion]
        protected static void CheckEdge<T>(IEdge<T> edge, T source, T target)
            where T : notnull
        {
            using (_ = new AssertionScope())
            {
                edge.Source.Should().NotBeNull();
                AssertAreEqual(source, edge.Source);

                edge.Target.Should().NotBeNull();
                AssertAreEqual(target, edge.Target);
            }
        }

        [CustomAssertion]
        protected static void CheckTermEdge<T>(ITermEdge<T> edge, T source, T target, int sourceTerm, int targetTerm)
            where T : notnull
        {
            CheckEdge(edge, source, target);
            edge.SourceTerminal.Should().Be(sourceTerm);
            edge.TargetTerminal.Should().Be(targetTerm);
        }

        [CustomAssertion]
        protected static void CheckTaggedEdge<TVertex, TEdge, TTag>(TEdge edge, TVertex source, TVertex target, TTag? tag)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>, ITagged<TTag>
        {
            CheckEdge(edge, source, target);
            AssertAreEqual(tag, edge.Tag);
        }

        [CustomAssertion]
        protected static void CheckStructTaggedEdge<TVertex, TEdge, TTag>(TEdge edge, TVertex? source, TVertex? target, TTag? tag) where TEdge : IEdge<TVertex>, ITagged<TTag>
            where TVertex : notnull
        {
            CheckStructEdge(edge, source, target);
            AssertAreEqual(tag, edge.Tag);
        }
    }
}
