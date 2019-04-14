using System;

namespace QuikGraph.Algorithms
{
    public interface ITreeBuilderAlgorithm<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        event EdgeAction<TVertex, TEdge> TreeEdge;
    }
}
