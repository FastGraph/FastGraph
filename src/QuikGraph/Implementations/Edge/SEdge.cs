#if SUPPORTS_SERIALIZATION
using System;
#endif
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
    /// The default struct based <see cref="IEdge{TVertex}"/> implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}->{" + nameof(Target) + "}")]
    [StructLayout(LayoutKind.Auto)]
    public struct SEdge<TVertex> : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SEdge{TVertex}"/> struct.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        public SEdge([NotNull] TVertex source, [NotNull] TVertex target)
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
        public override string ToString()
        {
            return string.Format(EdgeConstants.EdgeFormatString, Source, Target);
        }
    }
}
