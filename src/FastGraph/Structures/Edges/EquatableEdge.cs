#nullable enable

using System.Diagnostics;

namespace FastGraph
{
    /// <summary>
    /// An <see cref="IEdge{TVertex}"/> implementation that supports equality (directed edge).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}->{" + nameof(Target) + "}")]
    public class EquatableEdge<TVertex> : Edge<TVertex>, IEquatable<EquatableEdge<TVertex>>
        where TVertex : notnull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquatableEdge{TVertex}"/> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        public EquatableEdge(TVertex source, TVertex target)
            : base(source, target)
        {
        }

        /// <inheritdoc />
        public virtual bool Equals(EquatableEdge<TVertex>? other)
        {
            if (other is null)
                return false;
            return EqualityComparer<TVertex>.Default.Equals(Source, other.Source)
                && EqualityComparer<TVertex>.Default.Equals(Target, other.Target);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return Equals(obj as EquatableEdge<TVertex>);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCodeHelpers.Combine(Source.GetHashCode(), Target.GetHashCode());
        }
    }
}
