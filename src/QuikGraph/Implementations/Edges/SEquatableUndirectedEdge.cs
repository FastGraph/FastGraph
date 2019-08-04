using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using QuikGraph.Constants;

namespace QuikGraph
{
    /// <summary>
    /// The default struct based <see cref="IUndirectedEdge{TVertex}"/> implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}<->{" + nameof(Target) + "}")]
    [StructLayout(LayoutKind.Auto)]
    public struct SEquatableUndirectedEdge<TVertex>: IUndirectedEdge<TVertex>, IEquatable<SEquatableUndirectedEdge<TVertex>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SEquatableUndirectedEdge{TVertex}"/> struct.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        public SEquatableUndirectedEdge([NotNull] TVertex source, [NotNull] TVertex target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (Comparer<TVertex>.Default.Compare(source, target) > 0)
                throw new ArgumentException($"{nameof(source)} must be lower or equals to {nameof(target)}.");

            Source = source;
            Target = target;
        }

        /// <inheritdoc />
        public TVertex Source { get; }

        /// <inheritdoc />
        public TVertex Target { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is SEquatableUndirectedEdge<TVertex> edge 
                   && Equals(edge);
        }

        /// <inheritdoc />
        public bool Equals(SEquatableUndirectedEdge<TVertex> other)
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
            return string.Format(EdgeConstants.UndirectedEdgeFormatString, Source, Target);
        }
    }
}
