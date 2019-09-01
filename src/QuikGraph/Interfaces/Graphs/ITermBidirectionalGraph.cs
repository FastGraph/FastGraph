using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A directed graph with vertices of type <typeparamref name="TVertex"/>
    /// and terminal edges of type <typeparamref name="TEdge"/>, that is efficient
    /// to traverse both in and out edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface ITermBidirectionalGraph<TVertex, TEdge> : IBidirectionalGraph<TVertex, TEdge>
        where TEdge : ITermEdge<TVertex>
    {
        /// <summary>
        /// Gets the number of out terminals on the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Number of out terminals.</returns>
        [Pure]
        int OutTerminalCount([NotNull] TVertex vertex);

        /// <summary>
        /// Checks if the requested out terminal is empty or not for the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <returns>True if the out terminal is empty, false otherwise.</returns>
        [Pure]
        bool IsOutEdgesEmptyAt([NotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> out degree for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <returns>The <paramref name="vertex"/> out degree on terminal <paramref name="terminal"/>.</returns>
        [Pure]
        int OutDegreeAt([NotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> out edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <returns>The <paramref name="vertex"/> out-edges on terminal <paramref name="terminal"/>.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        IEnumerable<TEdge> OutEdgesAt([NotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Tries to get the <paramref name="vertex"/> out-edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <param name="edges">Out-edges found, otherwise null.</param>
        /// <returns>True if <paramref name="vertex"/> was found or/and out-edges were found, false otherwise.</returns>
        [Pure]
        [ContractAnnotation("=> true, edges:notnull;=> false, edges:null")]
        bool TryGetOutEdgesAt([NotNull] TVertex vertex, int terminal, [ItemNotNull] out IEnumerable<TEdge> edges);

        /// <summary>
        /// Gets the number of in terminals on the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Number of in terminals.</returns>
        [Pure]
        int InTerminalCount([NotNull] TVertex vertex);

        /// <summary>
        /// Checks if the requested in terminal is empty or not for the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">In terminal index.</param>
        /// <returns>True if the in terminal is empty, false otherwise.</returns>
        [Pure]
        bool IsInEdgesEmptyAt([NotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> in degree for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">In terminal index.</param>
        /// <returns>The <paramref name="vertex"/> in degree on terminal <paramref name="terminal"/>.</returns>
        [Pure]
        int InDegreeAt([NotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> in-edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">In terminal index.</param>
        /// <returns>The <paramref name="vertex"/> in-edges on terminal <paramref name="terminal"/>.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        IEnumerable<TEdge> InEdgesAt([NotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Tries to get the <paramref name="vertex"/> in-edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <param name="edges">In-edges found, otherwise null.</param>
        /// <returns>True if <paramref name="vertex"/> was found or/and in-edges were found, false otherwise.</returns>
        [Pure]
        [ContractAnnotation("=> true, edges:notnull;=> false, edges:null")]
        bool TryGetInEdgesAt([NotNull] TVertex vertex, int terminal, [ItemNotNull] out IEnumerable<TEdge> edges);
    }
}