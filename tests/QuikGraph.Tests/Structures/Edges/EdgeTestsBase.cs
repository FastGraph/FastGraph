using JetBrains.Annotations;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Base class for tests relative to edges structures.
    ///</summary>
    internal abstract class EdgeTestsBase
    {
        private static void AssertAreEqual<T>([CanBeNull] T left, [CanBeNull] T right)
        {
            bool isValueType = typeof(T).IsValueType;
            if (isValueType)
                Assert.AreEqual(left, right);
            else
                Assert.AreSame(left, right);
        }

        protected static void CheckStructEdge<T>([NotNull] IEdge<T> edge, T source, T target)
        {
            AssertAreEqual(source, edge.Source);
            AssertAreEqual(target, edge.Target);
        }

        protected static void CheckEdge<T>([NotNull] IEdge<T> edge, [NotNull] T source, [NotNull] T target)
        {
            Assert.IsNotNull(edge.Source);
            AssertAreEqual(source, edge.Source);

            Assert.IsNotNull(edge.Target);
            AssertAreEqual(target, edge.Target);
        }

        protected static void CheckTermEdge<T>([NotNull] ITermEdge<T> edge, [NotNull] T source, [NotNull] T target, int sourceTerm, int targetTerm)
        {
            CheckEdge(edge, source, target);
            Assert.AreEqual(sourceTerm, edge.SourceTerminal);
            Assert.AreEqual(targetTerm, edge.TargetTerminal);
        }

        protected static void CheckTaggedEdge<TVertex, TEdge, TTag>([NotNull] TEdge edge, [NotNull] TVertex source, [NotNull] TVertex target, [CanBeNull] TTag tag)
            where TEdge : IEdge<TVertex>, ITagged<TTag>
        {
            CheckEdge(edge, source, target);
            AssertAreEqual(tag, edge.Tag);
        }

        protected static void CheckStructTaggedEdge<TVertex, TEdge, TTag>([NotNull] TEdge edge, TVertex source, TVertex target, [CanBeNull] TTag tag)
            where TEdge : IEdge<TVertex>, ITagged<TTag>
        {
            CheckStructEdge(edge, source, target);
            AssertAreEqual(tag, edge.Tag);
        }
    }
}