#if SUPPORTS_CONTRACTS
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IEdgeSet{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IEdgeSet<,>))]
    internal abstract class EdgeSetContract<TVertex, TEdge> : IEdgeSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool IEdgeSet<TVertex, TEdge>.IsEdgesEmpty
        {
            get 
            {
                // ReSharper disable once RedundantAssignment, Justification: Code contract.
                IEdgeSet<TVertex, TEdge> explicitThis = this;

                Contract.Ensures(Contract.Result<bool>() == (explicitThis.EdgeCount == 0));

                return default(bool);
            }
        }

        int IEdgeSet<TVertex, TEdge>.EdgeCount
        {
            get
            {
                // ReSharper disable once RedundantAssignment, Justification: Code contract.
                IEdgeSet<TVertex, TEdge> explicitThis = this;

                Contract.Ensures(Contract.Result<int>() == explicitThis.Edges.Count());

                return default(int);
            }
        }

        IEnumerable<TEdge> IEdgeSet<TVertex, TEdge>.Edges
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<TEdge>>() != null);
                Contract.Ensures(Contract.Result<IEnumerable<TEdge>>().All(e => e != null));

                return Enumerable.Empty<TEdge>();            
            }
        }

        [Pure]
        bool IEdgeSet<TVertex, TEdge>.ContainsEdge(TEdge edge)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IEdgeSet<TVertex, TEdge> explicitThis = this;

            Contract.Requires(edge != null);
            Contract.Ensures(
                Contract.Result<bool>() == Contract.Exists(explicitThis.Edges, e => e.Equals(edge)));

            return default(bool);
        }
    }
}
#endif