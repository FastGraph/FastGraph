using System;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph
{
    public class VertexEventArgs<TVertex> : EventArgs
    {
        private readonly TVertex vertex;
        public VertexEventArgs(TVertex vertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
#endif

            this.vertex = vertex;
        }

        public TVertex Vertex
        {
            get { return this.vertex; }
        }
    }

    public delegate void VertexAction<in TVertex>(TVertex vertex);

    public delegate void VertexEventHandler<TVertex>(
        Object sender,
        VertexEventArgs<TVertex> e);
}
