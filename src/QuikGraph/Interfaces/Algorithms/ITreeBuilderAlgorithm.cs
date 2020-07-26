namespace QuikGraph.Algorithms
{
    /// <summary>
    /// An algorithm that exposes an event to build an edge tree.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface ITreeBuilderAlgorithm<TVertex, out TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Fired when an edge is encountered.
        /// </summary>
        event EdgeAction<TVertex, TEdge> TreeEdge;
    }
}