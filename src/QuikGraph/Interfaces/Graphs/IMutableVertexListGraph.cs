namespace QuikGraph
{
    /// <summary>
    /// A mutable vertex list graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IMutableVertexListGraph<TVertex, TEdge>
        : IMutableIncidenceGraph<TVertex, TEdge>
        , IMutableVertexSet<TVertex>
         where TEdge : IEdge<TVertex>
    {
    }
}