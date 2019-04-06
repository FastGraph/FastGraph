#if SUPPORTS_SERIALIZATION
using System;
#endif

namespace QuickGraph
{
    /// <summary>
    /// An edge factory delegate
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public delegate TEdge CreateEdgeDelegate<TVertex, TEdge>(
        IVertexListGraph<TVertex, TEdge> g,
        TVertex source,
        TVertex target)
        where TEdge : IEdge<TVertex>;
}
