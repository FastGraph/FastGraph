namespace QuikGraph
{
    /// <summary>
    /// Represents a directed edge with terminal indexes.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public interface ITermEdge<out TVertex> : IEdge<TVertex>
    {
        /// <summary>
        /// Index of terminal on source vertex to which this edge is attached.
        /// </summary>
        int SourceTerminal { get; }

        /// <summary>
        /// Index of terminal on target vertex to which this edge is attached.
        /// </summary>
        int TargetTerminal { get; }
    }
}