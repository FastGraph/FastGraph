#if SUPPORTS_CONTRACTS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace QuikGraph.Collections
{
    /// <summary>
    /// Contract class for <see cref="IEdgeList{TVertex, TEdge}"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [ContractClassFor(typeof(IEdgeList<,>))]
    internal abstract class EdgeListContract<TVertex, TEdge> : IEdgeList<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        IEdgeList<TVertex, TEdge> IEdgeList<TVertex, TEdge>.Clone()
        {
            Contract.Ensures(Contract.Result<IEdgeList<TVertex, TEdge>>() != null);

            throw new NotImplementedException();
        }

        void IEdgeList<TVertex, TEdge>.TrimExcess()
        {
            throw new NotImplementedException();
        }

        #region Others

        int IList<TEdge>.IndexOf(TEdge item)
        {
            throw new NotImplementedException();
        }

        void IList<TEdge>.Insert(int index, TEdge item)
        {
            throw new NotImplementedException();
        }

        void IList<TEdge>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        TEdge IList<TEdge>.this[int index]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        void ICollection<TEdge>.Add(TEdge item)
        {
            throw new NotImplementedException();
        }

        void ICollection<TEdge>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<TEdge>.Contains(TEdge item)
        {
            throw new NotImplementedException();
        }

        void ICollection<TEdge>.CopyTo(TEdge[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<TEdge>.Count => throw new NotImplementedException();

        bool ICollection<TEdge>.IsReadOnly => throw new NotImplementedException();

        bool ICollection<TEdge>.Remove(TEdge item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<TEdge> IEnumerable<TEdge>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

#if SUPPORTS_CLONEABLE
        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
#endif

        #endregion
    }
}
#endif