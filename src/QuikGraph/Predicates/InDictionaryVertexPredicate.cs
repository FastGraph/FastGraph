#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Predicates
{
    /// <summary>
    /// Predicate that tests if a vertex is a vertex map.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TValue">Type of the value associated to vertices.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class InDictionaryVertexPredicate<TVertex, TValue>
    {
        [NotNull]
        private readonly IDictionary<TVertex, TValue> _vertexMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="InDictionaryVertexPredicate{TVertex,TValue}"/> class.
        /// </summary>
        /// <param name="vertexMap">Vertex map.</param>
        public InDictionaryVertexPredicate([NotNull] IDictionary<TVertex, TValue> vertexMap)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(_vertexMap != null);
#endif

            _vertexMap = vertexMap;
        }

        /// <summary>
        /// Checks if the given <paramref name="vertex"/> is in the vertex map.
        /// </summary>
        /// <remarks>Check if the implemented predicate is matched.</remarks>
        /// <param name="vertex">Vertex to check.</param>
        /// <returns>True if the vertex is in the vertex map, false otherwise.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif

        [JetBrains.Annotations.Pure]
        public bool Test([NotNull] TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            return _vertexMap.ContainsKey(vertex);
        }
    }
}
