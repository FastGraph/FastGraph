namespace QuikGraph.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// Represents a minimum spanning tree algorithm.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        : IAlgorithm<IUndirectedGraph<TVertex, TEdge>>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}