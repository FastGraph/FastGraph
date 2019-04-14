using System;
using System.Collections.Generic;
using System.Text;

namespace QuikGraph.Algorithms.MinimumSpanningTree
{
    public interface IMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        : IAlgorithm<IUndirectedGraph<TVertex, TEdge>>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}
