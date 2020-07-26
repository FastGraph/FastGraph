namespace QuikGraph
{
    /// <summary>
    /// A mutable vertex and edge list graph with vertices of type
    /// <typeparamref name="TVertex"/> and edges of type <typeparamref name="TEdge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IMutableVertexAndEdgeListGraph<TVertex, TEdge>
        : IMutableVertexListGraph<TVertex, TEdge>
        , IMutableVertexAndEdgeSet<TVertex, TEdge>
        , IVertexAndEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}