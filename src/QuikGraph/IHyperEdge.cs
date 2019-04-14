using System;
using System.Collections.Generic;

namespace QuikGraph
{
    public interface IHyperEdge<TVertex>
    {
        int EndPointCount { get;}
        IEnumerable<TVertex> EndPoints { get;}
    }
}
