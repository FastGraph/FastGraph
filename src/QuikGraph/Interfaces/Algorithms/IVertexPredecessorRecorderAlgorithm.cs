namespace QuikGraph.Algorithms
{
    /// <summary>
    /// An algorithm that exposes events to compute vertices predecessors (in directed graph).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IVertexPredecessorRecorderAlgorithm<TVertex, out TEdge>
        : ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Fired when a vertex starts to be treated.
        /// </summary>
        event VertexAction<TVertex> StartVertex;

        /// <summary>
        /// Fired when a vertex is fully treated.
        /// </summary>
        event VertexAction<TVertex> FinishVertex;
    }
}
