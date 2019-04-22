using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A delegate-based incidence graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class DelegateIncidenceGraph<TVertex, TEdge> : DelegateImplicitGraph<TVertex, TEdge>, IIncidenceGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>, IEquatable<TEdge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateIncidenceGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        public DelegateIncidenceGraph([NotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges)
            : base(tryGetOutEdges)
        {
        }

        #region IIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return TryGetEdge(source, target, out _);
        }

        /// <inheritdoc />
        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            if (TryGetOutEdges(source, out IEnumerable<TEdge> outEdges))
            {
                edges = outEdges.Where(edge => edge.Target.Equals(target));
                return edges.Any();
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            if (TryGetOutEdges(source, out IEnumerable<TEdge> outEdges))
            {
                foreach (var outEdge in outEdges)
                {
                    if (outEdge.Target.Equals(target))
                    {
                        edge = outEdge;
                        return true;
                    }
                }
            }

            edge = default(TEdge);
            return false;
        }

        #endregion
    }
}
