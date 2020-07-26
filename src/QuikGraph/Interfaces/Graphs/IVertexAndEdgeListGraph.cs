namespace QuikGraph
{
    /// <summary>
    /// A directed graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/> that can be enumerated efficiently.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IVertexAndEdgeListGraph<TVertex, TEdge> 
        : IVertexListGraph<TVertex, TEdge>
        , IEdgeListGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}