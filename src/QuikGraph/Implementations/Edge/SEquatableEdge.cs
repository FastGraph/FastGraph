using System;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using QuikGraph.Constants;

namespace QuikGraph
{
    /// <summary>
    /// A struct based <see cref="IEdge{TVertex}"/> implementation that supports equality.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}->{" + nameof(Target) + "}")]
    [StructLayout(LayoutKind.Auto)]
    public struct SEquatableEdge<TVertex> : IEdge<TVertex>, IEquatable<SEquatableEdge<TVertex>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SEquatableEdge{TVertex}"/> struct.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        public SEquatableEdge([NotNull] TVertex source, [NotNull] TVertex target)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
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
            return obj is SEquatableEdge<TVertex> edge
                   && Equals(edge);
        }

        /// <inheritdoc />
        public bool Equals(SEquatableEdge<TVertex> other)
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
            return string.Format(EdgeConstants.EdgeFormatString, Source, Target);
        }
    }
}
