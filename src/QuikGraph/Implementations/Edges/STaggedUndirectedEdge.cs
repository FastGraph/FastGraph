using System;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Collections.Generic;
using System.Diagnostics.Contracts;
#endif
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using QuikGraph.Constants;

namespace QuikGraph
{
    /// <summary>
    /// A struct based <see cref="IUndirectedEdge{TVertex}"/> implementation that supports tagging.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TTag">Tag type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}<->{" + nameof(Target) + "}:{" + nameof(Tag) + "}")]
    [StructLayout(LayoutKind.Auto)]
    public struct SUndirectedTaggedEdge<TVertex, TTag> : IUndirectedEdge<TVertex>, ITagged<TTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SUndirectedTaggedEdge{Vertex, TTag}"/> struct.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <param name="tag">Edge tag.</param>
        public SUndirectedTaggedEdge([NotNull] TVertex source, [NotNull] TVertex target, [CanBeNull] TTag tag)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(Comparer<TVertex>.Default.Compare(source, target) <= 0);
            Contract.Ensures(Contract.ValueAtReturn(out this).Source.Equals(source));
            Contract.Ensures(Contract.ValueAtReturn(out this).Target.Equals(target));
#endif

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
        public override string ToString()
        {
            return string.Format(EdgeConstants.TaggedUndirectedEdgeFormatString, Source, Target, Tag);
        }
    }
}
