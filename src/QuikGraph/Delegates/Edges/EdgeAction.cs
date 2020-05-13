using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Delegate to perform an action involving the <paramref name="edge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="edge">Edge to treat.</param>
    public delegate void EdgeAction<TVertex, in TEdge>([NotNull] TEdge edge)
        where TEdge : IEdge<TVertex>;
}