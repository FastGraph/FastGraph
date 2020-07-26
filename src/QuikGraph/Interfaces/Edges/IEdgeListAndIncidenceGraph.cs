namespace QuikGraph
{
    /// <summary>
    /// Represents an incidence graph whose edges can be enumerated.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IEdgeListAndIncidenceGraph<TVertex,TEdge> 
        : IEdgeListGraph<TVertex,TEdge>
        , IIncidenceGraph<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}