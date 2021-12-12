#nullable enable

using System.Runtime.InteropServices;
using System.Diagnostics;
using FastGraph.Constants;

namespace FastGraph
{
    /// <summary>
    /// The default implementation of an <see cref="IUndirectedEdge{TVertex}"/> that supports tagging (struct).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TTag">Tag type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay("{" + nameof(Source) + "}->{" + nameof(Target) + "}:{" + nameof(Tag) + "}")]
    public struct STaggedUndirectedEdge<TVertex, TTag> : IUndirectedEdge<TVertex>, ITagged<TTag>
        where TVertex : notnull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="STaggedUndirectedEdge{TVertex, TTag}"/> struct.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <param name="tag">Edge tag.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="target"/> is not lower than <paramref name="source"/> when using <see cref="M:System.Collections.Generic.Comparer{T}.Default"/>.
        /// </exception>
        public STaggedUndirectedEdge(TVertex source, TVertex target, TTag? tag)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            Source = source;
            Target = target;
            _tag = tag;
            TagChanged = default;

            if (Comparer<TVertex>.Default.Compare(source, target) > 0)
                throw new ArgumentException($"{nameof(source)} must be lower than {nameof(target)} in {GetType().Name}.");
        }

        /// <inheritdoc />
        public TVertex Source { get; }

        /// <inheritdoc />
        public TVertex Target { get; }

        /// <inheritdoc />
        public event EventHandler? TagChanged;

        /// <summary>
        /// Event invoker for <see cref="TagChanged"/> event.
        /// </summary>
        /// <param name="args">Event arguments.</param>
        private void OnTagChanged(EventArgs args)
        {
            TagChanged?.Invoke(this, args);
        }

        private TTag? _tag;

        /// <inheritdoc />
        public TTag? Tag
        {
            get => _tag;
            set
            {
                if (EqualityComparer<TTag?>.Default.Equals(_tag, value))
                    return;

                _tag = value;
                OnTagChanged(EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(EdgeConstants.TaggedUndirectedEdgeFormatString, Source, Target, Tag?.ToString() ?? "no tag");
        }
    }
}
