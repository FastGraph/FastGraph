using System;
using System.Collections.Generic;
using System.Text;

namespace QuikGraph
{
    /// <summary>
    /// A factory of identifiable vertices.
    /// </summary>
    public delegate TVertex IdentifiableVertexFactory<TVertex>(string id);
}
