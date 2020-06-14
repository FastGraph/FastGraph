using System;
using JetBrains.Annotations;

namespace QuikGraph.MSAGL
{
    /// <summary>
    /// MSAGL graph populator (with string formatting).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class MsaglToStringGraphPopulator<TVertex, TEdge> : MsaglDefaultGraphPopulator<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsaglToStringGraphPopulator{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to convert to MSAGL graph.</param>
        /// <param name="formatProvider">Graph format provider.</param>
        /// <param name="format">Graph format.</param>
        public MsaglToStringGraphPopulator(
            [NotNull] IEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] IFormatProvider formatProvider,
            [CanBeNull] string format)
            : base(visitedGraph)
        {
            FormatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
            Format = string.IsNullOrEmpty(format) ? "{0}" : format;
        }

        /// <summary>
        /// Graph format provider.
        /// </summary>
        [NotNull]
        public IFormatProvider FormatProvider { get; }

        /// <summary>
        /// Graph format.
        /// </summary>
        [NotNull]
        public string Format { get; }

        /// <inheritdoc />
        protected override string GetVertexId(TVertex vertex)
        {
            return string.Format(FormatProvider, Format, vertex);
        }
    }
}