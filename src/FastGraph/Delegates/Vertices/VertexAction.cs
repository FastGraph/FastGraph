#nullable enable

namespace FastGraph
{
    /// <summary>
    /// Delegate to perform an action involving the <paramref name="vertex"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <param name="vertex">Vertex to treat.</param>
    public delegate void VertexAction<in TVertex>(TVertex vertex)
        where TVertex : notnull;
}
