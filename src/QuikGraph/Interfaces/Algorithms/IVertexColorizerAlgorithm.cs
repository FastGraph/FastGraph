using System;
using System.Collections.Generic;

namespace QuikGraph.Algorithms
{
    public interface IVertexColorizerAlgorithm<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        GraphColor GetVertexColor(TVertex v);
    }
}
