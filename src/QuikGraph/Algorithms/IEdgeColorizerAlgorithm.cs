using System;
using System.Collections.Generic;

namespace QuikGraph.Algorithms
{
    public interface IEdgeColorizerAlgorithm<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        IDictionary<TEdge, GraphColor> EdgeColors { get;}
    }
}
