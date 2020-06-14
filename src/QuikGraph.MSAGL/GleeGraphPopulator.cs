using Microsoft.Msagl.Drawing;
using QuikGraph.Algorithms;

namespace QuikGraph.MSAGL
{
    public abstract class GleeGraphPopulator<TVertex, TEdge> : AlgorithmBase<IEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        protected GleeGraphPopulator(IEdgeListGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }

        private Microsoft.Msagl.Drawing.Graph gleeGraph;
        public Microsoft.Msagl.Drawing.Graph GleeGraph
        {
            get { return this.gleeGraph; }
        }

        #region Events
        public event GleeVertexNodeEventHandler<TVertex> NodeAdded;
        protected virtual void OnNodeAdded(GleeVertexEventArgs<TVertex> e)
        {
            GleeVertexNodeEventHandler<TVertex> eh = this.NodeAdded;
            if (eh != null)
                eh(this, e);
        }

        public event GleeEdgeEventHandler<TVertex, TEdge> EdgeAdded;
        protected virtual void OnEdgeAdded(GleeEdgeEventArgs<TVertex, TEdge> e)
        {
            var eh = this.EdgeAdded;
            if (eh != null)
                eh(this, e);
        }
        #endregion

        protected override void InternalCompute()
        {
            this.gleeGraph = new Microsoft.Msagl.Drawing.Graph("");

            foreach (var v in this.VisitedGraph.Vertices)
            {
                Node node = this.AddNode(v);
                node.UserData = v;
                this.OnNodeAdded(new GleeVertexEventArgs<TVertex>(v, node));
            }

            foreach (var e in this.VisitedGraph.Edges)
            {
                Microsoft.Msagl.Drawing.Edge edge = this.AddEdge(e);
                edge.UserData = e;
                this.OnEdgeAdded(new GleeEdgeEventArgs<TVertex, TEdge>(e, edge));
            }
        }

        protected abstract Node AddNode(TVertex v);

        protected abstract Microsoft.Msagl.Drawing.Edge AddEdge(TEdge e);
    }
}