using Microsoft.Msagl.Drawing;

namespace QuikGraph.MSAGL
{
    public sealed class GleeEdgeEventArgs<TVertex, TEdge> : EdgeEventArgs<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly Microsoft.Msagl.Drawing.Edge gedge;
        public Microsoft.Msagl.Drawing.Edge GEdge
        {
            get { return this.gedge; }
        }

        public GleeEdgeEventArgs(TEdge edge, Microsoft.Msagl.Drawing.Edge gedge)
            : base(edge)
        {
            this.gedge = gedge;
        }
    }

    public delegate void GleeEdgeEventHandler<Vertex, Edge>(
        object sender,
        GleeEdgeEventArgs<Vertex, Edge> e)
        where Edge : IEdge<Vertex>;
}