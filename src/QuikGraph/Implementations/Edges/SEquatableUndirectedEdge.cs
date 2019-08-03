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
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
#endif

            if (Comparer<TVertex>.Default.Compare(source, target) > 0)
            {
                throw new ArgumentException("source cannot be greater than target in SEquatableUndirectedEdge");
            }

#if SUPPORTS_CONTRACTS
            Contract.Ensures(Contract.ValueAtReturn(out this).Source.Equals(source));
            Contract.Ensures(Contract.ValueAtReturn(out this).Target.Equals(target));
#endif

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
#if SUPPORTS_CONTRACTS
            Contract.Ensures(
                Contract.Result<bool>() == (Source.Equals(other.Source) && Target.Equals(other.Target)));
#endif

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
