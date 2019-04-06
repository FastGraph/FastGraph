using System;
using System.Collections;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace QuickGraph.Collections
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IVertexEdgeDictionary<,>))]
#endif
    abstract class IVertexEdgeDictionaryContract<TVertex, TEdge> : IVertexEdgeDictionary<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        IVertexEdgeDictionary<TVertex, TEdge> IVertexEdgeDictionary<TVertex, TEdge>.Clone()
        {
#if SUPPORTS_CONTRACTS
            Contract.Ensures(Contract.Result<IVertexEdgeDictionary<TVertex, TEdge>>() != null);
#endif

            throw new NotImplementedException();
        }

        #region Others

        void IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Add(TVertex key, IEdgeList<TVertex, TEdge> value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.ContainsKey(TVertex key)
        {
            throw new NotImplementedException();
        }

        ICollection<TVertex> IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Keys
            => throw new NotImplementedException();

        bool IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Remove(TVertex key)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.TryGetValue(TVertex key, out IEdgeList<TVertex, TEdge> value)
        {
            throw new NotImplementedException();
        }

        ICollection<IEdgeList<TVertex, TEdge>> IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.Values
            => throw new NotImplementedException();

        IEdgeList<TVertex, TEdge> IDictionary<TVertex, IEdgeList<TVertex, TEdge>>.this[TVertex key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Add(KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Contains(KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.CopyTo(KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Count
            => throw new NotImplementedException();

        bool ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.IsReadOnly => throw new NotImplementedException();

        bool ICollection<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.Remove(KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>> IEnumerable<KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>>>.GetEnumerator()
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

#if SUPPORTS_SERIALIZATION
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
#endif

        #endregion
    }
}
