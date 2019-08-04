using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using QuikGraph.Constants;

namespace QuikGraph
{
    /// <summary>
    /// A struct based <see cref="IEdge{TVertex}"/> implementation that supports tagging and is equatable.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TTag">Tag type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay("{" + nameof(Source) + "}->{" + nameof(Target) + "}:{" + nameof(Tag) + "}")]
    public struct STaggedEquatableEdge<TVertex, TTag> : IEdge<TVertex>, IEquatable<STaggedEquatableEdge<TVertex, TTag>>, ITagged<TTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="STaggedEquatableEdge{TVertex, TTag}"/> struct.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <param name="tag">Edge tag.</param>
        public STaggedEquatableEdge([NotNull] TVertex source, [NotNull] TVertex target, [CanBeNull] TTag tag)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            Source = source;
            Target = target;
            _tag = tag;
            TagChanged = null;
        }

        /// <inheritdoc />
        public TVertex Source { get; }

        /// <inheritdoc />
        public TVertex Target { get; }

        /// <inheritdoc />
        public event EventHandler TagChanged;

        private void OnTagChanged([NotNull] EventArgs args)
        {
            TagChanged?.Invoke(this, args);
        }

        private TTag _tag;

        /// <inheritdoc />
        public TTag Tag
        {
            get => _tag;
            set
            {
                if (Equals(_tag, value))
                    return;

                _tag = value;
                OnTagChanged(EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is STaggedEquatableEdge<TVertex, TTag> edge
                   && Equals(edge);
        }

        /// <inheritdoc />
        public bool Equals(STaggedEquatableEdge<TVertex, TTag> other)
        {
            return Source.Equals(other.Source) && Target.Equals(other.Target);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCodeHelpers.Combine(Source.GetHashCode(), Target.GetHashCode());
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(EdgeConstants.TaggedEdgeFormatString, Source, Target, Tag);
        }
    }
}
