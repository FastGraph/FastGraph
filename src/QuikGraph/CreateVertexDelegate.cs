#if SUPPORTS_SERIALIZATION
using System;
#endif

namespace QuikGraph
{
    /// <summary>
    /// A vertex factory delegate.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public delegate TVertex CreateVertexDelegate<TVertex, TEdge>(IVertexListGraph<TVertex,TEdge> g) 
        where TEdge : IEdge<TVertex>;
}
