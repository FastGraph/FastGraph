using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QuikGraph
{
    /// <summary>
    /// Implementation for a bidirectional terminal graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("VertexCount = {" + nameof(VertexCount) + "}, EdgeCount = {" + nameof(EdgeCount) + "}")]
    public class TermBidirectionalGraph<TVertex, TEdge> : BidirectionalGraph<TVertex, TEdge>, IMutableTermBidirectionalGraph<TVertex, TEdge>
        where TEdge : ITermEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TermBidirectionalGraph{TVertex,TEdge}"/> class.
        /// </summary>
        public TermBidirectionalGraph()
            : base(true, -1)
        {
        }

        #region ITermBidirectionalGraph<TVertex,TEdge>

        /// <summary>
        /// <see cref="OutTerminalCount"/> is not implemented for this kind of graph.
        /// </summary>
        public int OutTerminalCount(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="IsOutEdgesEmptyAt"/> is not implemented for this kind of graph.
        /// </summary>
        public bool IsOutEdgesEmptyAt(TVertex vertex, int terminal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="OutDegreeAt"/> is not implemented for this kind of graph.
        /// </summary>
        public int OutDegreeAt(TVertex vertex, int terminal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="OutEdgesAt"/> is not implemented for this kind of graph.
        /// </summary>
        public IEnumerable<TEdge> OutEdgesAt(TVertex vertex, int terminal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="TryGetOutEdgesAt"/> is not implemented for this kind of graph.
        /// </summary>
        public bool TryGetOutEdgesAt(TVertex vertex, int terminal, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="InTerminalCount"/> is not implemented for this kind of graph.
        /// </summary>
        public int InTerminalCount(TVertex vertex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="IsInEdgesEmptyAt"/> is not implemented for this kind of graph.
        /// </summary>
        public bool IsInEdgesEmptyAt(TVertex vertex, int terminal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="InDegreeAt"/> is not implemented for this kind of graph.
        /// </summary>
        public int InDegreeAt(TVertex vertex, int terminal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="InEdgesAt"/> is not implemented for this kind of graph.
        /// </summary>
        public IEnumerable<TEdge> InEdgesAt(TVertex vertex, int terminal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="TryGetInEdgesAt"/> is not implemented for this kind of graph.
        /// </summary>
        public bool TryGetInEdgesAt(TVertex vertex, int terminal, out IEnumerable<TEdge> edges)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
