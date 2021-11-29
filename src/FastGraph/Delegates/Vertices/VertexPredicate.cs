using JetBrains.Annotations;

namespace FastGraph
{
    /// <summary>
    /// Delegate to perform a check on the given <paramref name="vertex"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <param name="vertex">Vertex to check condition.</param>
    /// <returns>True if the <paramref name="vertex"/> matches the predicate, false otherwise.</returns>
    public delegate bool VertexPredicate<in TVertex>([NotNull] TVertex vertex);
}