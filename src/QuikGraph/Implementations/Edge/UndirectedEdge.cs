#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuikGraph.Constants;

namespace QuikGraph
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
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedEdge{TVertex}"/> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        public UndirectedEdge(TVertex source, TVertex target)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(Comparer<TVertex>.Default.Compare(source, target) <= 0);
            Contract.Ensures(Source.Equals(source));
            Contract.Ensures(Target.Equals(target));
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
            return string.Format(EdgeConstants.UndirectedEdgeFormatString, Source, Target);
        }
    }
}
