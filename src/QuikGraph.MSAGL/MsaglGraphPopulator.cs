using Microsoft.Msagl.Drawing;
using QuikGraph.Algorithms;

namespace QuikGraph.MSAGL
{
    public abstract class MsaglGraphPopulator<TVertex, TEdge> : AlgorithmBase<IEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        protected MsaglGraphPopulator(IEdgeListGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }

        private Microsoft.Msagl.Drawing.Graph _msaglGraph;
        public Microsoft.Msagl.Drawing.Graph MsaglGraph
        {
            get { return this._msaglGraph; }
        }

        #region Events
        public event MsaglVertexNodeEventHandler<TVertex> NodeAdded;
        protected virtual void OnNodeAdded(MsaglVertexEventArgs<TVertex> e)
        {
            MsaglVertexNodeEventHandler<TVertex> eh = this.NodeAdded;
            if (eh != null)
                eh(this, e);
        }

        public event MsaglEdgeEventHandler<TVertex, TEdge> EdgeAdded;
        protected virtual void OnEdgeAdded(MsaglEdgeEventArgs<TVertex, TEdge> e)
        {
            var eh = this.EdgeAdded;
            if (eh != null)
                eh(this, e);
        }
        #endregion

        protected override void InternalCompute()
        {
            this._msaglGraph = new Microsoft.Msagl.Drawing.Graph("");

            foreach (var v in this.VisitedGraph.Vertices)
            {
                Node node = this.AddNode(v);
                node.UserData = v;
                this.OnNodeAdded(new MsaglVertexEventArgs<TVertex>(v, node));
            }

            foreach (var e in this.VisitedGraph.Edges)
            {
                Microsoft.Msagl.Drawing.Edge edge = this.AddEdge(e);
                edge.UserData = e;
                this.OnEdgeAdded(new MsaglEdgeEventArgs<TVertex, TEdge>(e, edge));
            }
        }

        protected abstract Node AddNode(TVertex v);

        protected abstract Microsoft.Msagl.Drawing.Edge AddEdge(TEdge e);
    }
}