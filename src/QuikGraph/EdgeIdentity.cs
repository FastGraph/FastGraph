using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuikGraph
{
    public delegate string EdgeIdentity<TVertex, TEdge>(TEdge edge)
        where TEdge : IEdge<TVertex>;
}
