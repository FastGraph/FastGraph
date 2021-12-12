#nullable enable

using System.Diagnostics;
using FastGraph.Constants;

namespace FastGraph
{
    /// <summary>
    /// The default <see cref="IUndirectedEdge{TVertex}"/> implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}<->{" + nameof(Target) + "}")]
    public class UndirectedEdge<TVertex> : IUndirectedEdge<TVertex>
        where TVertex : notnull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedEdge{TVertex}"/> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="target"/> is not lower than <paramref name="source"/> when using <see cref="M:System.Collections.Generic.Comparer{T}.Default"/>.
        /// </exception>
        public UndirectedEdge(TVertex source, TVertex target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (Comparer<TVertex>.Default.Compare(source, target) > 0)
                throw new ArgumentException($"{nameof(source)} must be lower than {nameof(target)} in {GetType().Name}.");

            Source = source;
            Target = target;
        }

        /// <inheritdoc />
        public TVertex Source { get; }

        /// <inheritdoc />
        public TVertex Target { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(EdgeConstants.UndirectedEdgeFormatString, Source, Target);
        }
    }
}
