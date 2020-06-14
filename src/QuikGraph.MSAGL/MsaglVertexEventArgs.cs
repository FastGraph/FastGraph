using System.Diagnostics.Contracts;
using Microsoft.Msagl.Drawing;

namespace QuikGraph.MSAGL
{
    public sealed class MsaglVertexEventArgs<TVertex> : VertexEventArgs<TVertex>
    {
        private readonly Node node;

        public MsaglVertexEventArgs(TVertex vertex, Node node)
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

    public delegate void MsaglVertexNodeEventHandler<Vertex>(
        object sender,
        MsaglVertexEventArgs<Vertex> args);
}