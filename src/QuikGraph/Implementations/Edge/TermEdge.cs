#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;
using QuikGraph.Constants;

namespace QuikGraph
{
    /// <summary>
    /// The default <see cref="ITermEdge{TVertex}"/> implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}->{" + nameof(Target) + "}")]
    public class TermEdge<TVertex> : ITermEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TermEdge{TVertex}"/> class
        /// using source/target vertices and source/target terminals.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <param name="sourceTerminal">The source terminal.</param>
        /// <param name="targetTerminal">The target terminal.</param>
        public TermEdge([NotNull] TVertex source, [NotNull] TVertex target, int sourceTerminal, int targetTerminal)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Requires(sourceTerminal >= 0);
            Contract.Requires(targetTerminal >= 0);
            Contract.Ensures(Source.Equals(source));
            Contract.Ensures(Target.Equals(target));
            Contract.Ensures(SourceTerminal.Equals(sourceTerminal));
            Contract.Ensures(TargetTerminal.Equals(targetTerminal));
#endif

            Source = source;
            Target = target;
            SourceTerminal = sourceTerminal;
            TargetTerminal = targetTerminal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TermEdge{TVertex}"/> class
        /// using source/target vertices and zero terminals.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        public TermEdge(TVertex source, TVertex target)
            : this(source, target, 0, 0)
        {
        }

        /// <inheritdoc />
        public TVertex Source { get; }

        /// <inheritdoc />
        public TVertex Target { get; }

        /// <inheritdoc />
        public int SourceTerminal { get; }

        /// <inheritdoc />
        public int TargetTerminal { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(EdgeConstants.EdgeTerminalFormatString, Source, SourceTerminal, Target, TargetTerminal);
        }
    }
}
