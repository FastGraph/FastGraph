using System.Diagnostics.Contracts;
using Microsoft.Msagl.Drawing;

namespace QuikGraph.MSAGL
{
    public sealed class GleeIndentifiableGraphPopulator<TVertex, TEdge> : GleeGraphPopulator<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private readonly VertexIdentity<TVertex> vertexIdentities;
        public GleeIndentifiableGraphPopulator(IEdgeListGraph<TVertex, TEdge> visitedGraph, VertexIdentity<TVertex> vertexIdentities)
            : base(visitedGraph)
        {
            Contract.Requires(vertexIdentities != null);

            this.vertexIdentities = vertexIdentities;
        }

        protected override Node AddNode(TVertex v)
        {
            return (Node)this.GleeGraph.AddNode(this.vertexIdentities(v));
        }

        protected override Microsoft.Msagl.Drawing.Edge AddEdge(TEdge e)
        {
            return (Microsoft.Msagl.Drawing.Edge)this.GleeGraph.AddEdge(
                this.vertexIdentities(e.Source),
                this.vertexIdentities(e.Target));
        }
    }
}