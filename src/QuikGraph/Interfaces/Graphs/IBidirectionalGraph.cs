namespace QuikGraph
{
    /// <summary>
    /// A directed graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/>, that is efficient
    /// to traverse both in and out edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IBidirectionalGraph<TVertex,TEdge> 
        : IVertexAndEdgeListGraph<TVertex,TEdge>
        , IBidirectionalIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}