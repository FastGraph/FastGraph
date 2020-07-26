namespace QuikGraph
{
    /// <summary>
    /// Represents an undirected edge. 
    /// </summary>
    /// <remarks>
    /// Invariant: source must be less or equal to target (using the default comparer).
    /// </remarks>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public interface IUndirectedEdge<out TVertex> : IEdge<TVertex>
    {
    }
}