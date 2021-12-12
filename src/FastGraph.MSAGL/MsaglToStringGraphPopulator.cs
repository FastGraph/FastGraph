#nullable enable

namespace FastGraph.MSAGL
{
    /// <summary>
    /// MSAGL graph populator (with string formatting).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class MsaglToStringGraphPopulator<TVertex, TEdge> : MsaglDefaultGraphPopulator<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsaglToStringGraphPopulator{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to convert to MSAGL graph.</param>
        /// <param name="formatProvider">Graph format provider.</param>
        /// <param name="format">Graph format.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public MsaglToStringGraphPopulator(
            IEdgeListGraph<TVertex, TEdge> visitedGraph,
            string? format = default,
            IFormatProvider? formatProvider = default)
            : base(visitedGraph)
        {
            FormatProvider = formatProvider;
            Format = string.IsNullOrEmpty(format!) ? "{0}" : format!;
        }

        /// <summary>
        /// Vertex format provider.
        /// </summary>
        public IFormatProvider? FormatProvider { get; }

        /// <summary>
        /// Vertex id format.
        /// </summary>
        public string Format { get; }

        /// <inheritdoc />
        protected override string GetVertexId(TVertex vertex)
        {
            return FormatProvider is null
                ? string.Format(Format, vertex)
                : string.Format(FormatProvider, Format, vertex);
        }
    }
}
