#nullable enable

using Microsoft.Msagl.Drawing;
using FastGraph.Algorithms;

namespace FastGraph.MSAGL
{
    /// <summary>
    /// Base class for MSAGL graph populator.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public abstract class MsaglGraphPopulator<TVertex, TEdge> : AlgorithmBase<IEdgeListGraph<TVertex, TEdge>>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsaglGraphPopulator{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to convert to MSAGL graph.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        protected MsaglGraphPopulator(IEdgeListGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }

        /// <summary>
        /// MSAGL graph corresponding to <see cref="AlgorithmBase{TGraph}.VisitedGraph"/>.
        /// </summary>
        public Graph? MsaglGraph { get; private set; }

        #region Events

        /// <summary>
        /// Fired when a node is added to the graph.
        /// </summary>
        public event MsaglVertexNodeEventHandler<TVertex>? NodeAdded;

        /// <summary>
        /// Called when a <see cref="T:Microsoft.Msagl.Drawing.Node"/> is added.
        /// </summary>
        /// <param name="args">Event arguments.</param>
        protected virtual void OnNodeAdded(MsaglVertexEventArgs<TVertex> args)
        {
            NodeAdded?.Invoke(this, args);
        }

        /// <summary>
        /// Fired when an edge is added to the graph.
        /// </summary>
        public event MsaglEdgeEventHandler<TVertex, TEdge>? EdgeAdded;

        /// <summary>
        /// Called when an <see cref="T:Microsoft.Msagl.Drawing.Edge"/> is added.
        /// </summary>
        /// <param name="args">Event arguments.</param>
        protected virtual void OnEdgeAdded(MsaglEdgeEventArgs<TVertex, TEdge> args)
        {
            EdgeAdded?.Invoke(this, args);
        }

        #endregion

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            MsaglGraph = new Graph(string.Empty)
            {
                Directed = VisitedGraph.IsDirected
            };

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                Node node = AddNode(vertex);
                node.UserData = vertex;
                OnNodeAdded(new MsaglVertexEventArgs<TVertex>(vertex, node));
            }

            foreach (TEdge edge in VisitedGraph.Edges)
            {
                Edge msaglEdge = AddEdge(edge);
                msaglEdge.UserData = edge;
                OnEdgeAdded(new MsaglEdgeEventArgs<TVertex, TEdge>(edge, msaglEdge));
            }
        }

        #endregion

        /// <summary>
        /// Called when a <paramref name="vertex"/> should be added to the graph.
        /// </summary>
        /// <param name="vertex">Vertex to add.</param>
        /// <returns>Added <see cref="T:Microsoft.Msagl.Drawing.Node"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        protected abstract Node AddNode(TVertex vertex);

        /// <summary>
        /// Called when an <paramref name="edge"/> should be added to the graph.
        /// </summary>
        /// <param name="edge">Edge to add.</param>
        /// <returns>Added <see cref="T:Microsoft.Msagl.Drawing.Edge"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edge"/> is <see langword="null"/>.</exception>
        protected abstract Edge AddEdge(TEdge edge);
    }
}
