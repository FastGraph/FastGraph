#nullable enable

using System.Diagnostics;

namespace FastGraph
{
    /// <summary>
    /// The default struct based <see cref="IUndirectedEdge{TVertex}"/> implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}<->{" + nameof(Target) + "}")]
    public class EquatableUndirectedEdge<TVertex> : UndirectedEdge<TVertex>, IEquatable<EquatableUndirectedEdge<TVertex>>
        where TVertex : notnull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquatableUndirectedEdge{TVertex}"/> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        public EquatableUndirectedEdge(TVertex source, TVertex target)
            : base(source, target)
        {
        }

        /// <inheritdoc />
        public virtual bool Equals(EquatableUndirectedEdge<TVertex>? other)
        {
            if (other is null)
                return false;
            return EqualityComparer<TVertex>.Default.Equals(Source, other.Source)
                && EqualityComparer<TVertex>.Default.Equals(Target, other.Target);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return Equals(obj as EquatableUndirectedEdge<TVertex>);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCodeHelpers.Combine(Source.GetHashCode(), Target.GetHashCode());
        }
    }
}
