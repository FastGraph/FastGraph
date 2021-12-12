#nullable enable

namespace FastGraph.Algorithms
{
    /// <summary>
    /// Represents an algorithm dealing with graph connected components.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    public interface IConnectedComponentAlgorithm<TVertex, TEdge, out TGraph> : IAlgorithm<TGraph>
        where TVertex : notnull
        where TGraph : IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Number of components.
        /// </summary>
        int ComponentCount { get; }

        /// <summary>
        /// Graph components.
        /// </summary>
        IDictionary<TVertex, int> Components { get; }
    }
}
