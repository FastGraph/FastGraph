using System;
using JetBrains.Annotations;
using Microsoft.Msagl.Drawing;

namespace QuikGraph.MSAGL
{
    /// <summary>
    /// Arguments of an event related to an MSAGL edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [Serializable]
    public class MsaglEdgeEventArgs<TVertex, TEdge> : EdgeEventArgs<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsaglEdgeEventArgs{TVertex, TEdge}"/> class.
        /// </summary>
        /// <param name="edge">Concerned edge.</param>
        /// <param name="msaglEdge">Concerned <see cref="Edge"/>.</param>
        public MsaglEdgeEventArgs([NotNull] TEdge edge, [NotNull] Edge msaglEdge)
            : base(edge)
        {
            MsaglEdge = msaglEdge ?? throw new ArgumentNullException(nameof(msaglEdge));
        }

        /// <summary>
        /// Edge concerned by the event.
        /// </summary>
        [NotNull]
        public Edge MsaglEdge { get; }
    }
}