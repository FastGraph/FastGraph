namespace QuikGraph
{
    /// <summary>
    /// A directed graph data structure where out-edges can be traversed,
    /// i.e. a vertex set + implicit graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IVertexListGraph<TVertex, TEdge>
        : IIncidenceGraph<TVertex, TEdge>
        , IVertexSet<TVertex>
        where TEdge : IEdge<TVertex>
    {
    }
}