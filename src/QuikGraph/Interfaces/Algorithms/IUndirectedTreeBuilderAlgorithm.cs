using System;

namespace QuikGraph.Algorithms
{
    public interface IUndirectedTreeBuilderAlgorithm<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        event UndirectedEdgeAction<TVertex, TEdge> TreeEdge;
    }
}
