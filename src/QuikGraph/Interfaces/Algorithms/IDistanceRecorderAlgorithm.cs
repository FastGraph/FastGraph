namespace QuikGraph.Algorithms
{
    /// <summary>
    /// An algorithm that exposes events to compute a distance map between vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public interface IDistanceRecorderAlgorithm<out TVertex>
    {
        /// <summary>
        /// Fired when a vertex is initialized.
        /// </summary>
        event VertexAction<TVertex> InitializeVertex;

        /// <summary>
        /// Fired when a vertex is discovered.
        /// </summary>
        event VertexAction<TVertex> DiscoverVertex;
    }
}