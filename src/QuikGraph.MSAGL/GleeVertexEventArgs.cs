using System.Diagnostics.Contracts;
using Microsoft.Msagl.Drawing;

namespace QuikGraph.MSAGL
{
    public sealed class GleeVertexEventArgs<TVertex> : VertexEventArgs<TVertex>
    {
        private readonly Node node;

        public GleeVertexEventArgs(TVertex vertex, Node node)
            : base(vertex)
        {
            Contract.Requires(node != null);
            this.node = node;
        }

        public Node Node
        {
            get { return this.node; }
        }
    }

    public delegate void GleeVertexNodeEventHandler<Vertex>(
        object sender,
        GleeVertexEventArgs<Vertex> args);
}