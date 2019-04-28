using System;

namespace QuikGraph.Algorithms
{
    public interface IVertexTimeStamperAlgorithm<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        event VertexAction<TVertex> DiscoverVertex;
        event VertexAction<TVertex> FinishVertex;
    }
}
