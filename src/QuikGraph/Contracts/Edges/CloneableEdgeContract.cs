#if SUPPORTS_CONTRACTS
using System;
using System.Diagnostics.Contracts;

namespace QuikGraph.Contracts
{
    /// <summary>
    /// Contract class for <see cref="ICloneableEdge{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(ICloneableEdge<,>))]
    internal abstract class CloneableEdgeContract<TVertex, TEdge> : ICloneableEdge<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        TEdge ICloneableEdge<TVertex, TEdge>.Clone(TVertex source, TVertex target)
        {
            Contract.Requires(source != null);
            Contract.Requires(target != null);
            Contract.Ensures(Contract.Result<TEdge>() != null);
            Contract.Ensures(Contract.Result<TEdge>().Source.Equals(source));
            Contract.Ensures(Contract.Result<TEdge>().Target.Equals(target));

            // ReSharper disable once AssignNullToNotNullAttribute, Justification: Contract class.
            return default(TEdge);
        }

        #region IEdge<TVertex>

        TVertex IEdge<TVertex>.Source => throw new NotImplementedException();

        TVertex IEdge<TVertex>.Target => throw new NotImplementedException();

        #endregion
    }
}
#endif