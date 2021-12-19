#nullable enable

using Microsoft.Msagl.Drawing;

namespace FastGraph.MSAGL
{
    /// <summary>
    /// Default MSAGL graph populator.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class MsaglDefaultGraphPopulator<TVertex, TEdge> : MsaglGraphPopulator<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private Dictionary<TVertex, string>? _verticesIds;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsaglDefaultGraphPopulator{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to convert to MSAGL graph.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="visitedGraph"/> is <see langword="null"/>.</exception>
        public MsaglDefaultGraphPopulator(IEdgeListGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }

        /// <inheritdoc />
        protected override void OnStarted(EventArgs args)
        {
            base.OnStarted(args);

            _verticesIds = new Dictionary<TVertex, string>(VisitedGraph.VertexCount);
        }

        /// <inheritdoc />
        protected override void OnFinished(EventArgs args)
        {
            _verticesIds = default;

            base.OnFinished(args);
        }

        /// <inheritdoc />
        protected override Node AddNode(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            string id = GetVertexId(vertex);
            _verticesIds!.Add(vertex, id);
            Node node = MsaglGraph!.AddNode(id);
            node.Attr.Shape = Shape.Box;
            node.LabelText = GetVertexLabel(id, vertex);
            return node;
        }

        /// <summary>
        /// Gets the vertex identifier.
        /// </summary>
        /// <param name="vertex">Vertex to get id.</param>
        /// <returns>Vertex id.</returns>
        protected virtual string GetVertexId(TVertex vertex)
        {
            return _verticesIds!.Count.ToString();
        }

        /// <summary>
        /// Gets the vertex label.
        /// </summary>
        /// <param name="id">Vertex id.</param>
        /// <param name="vertex">Vertex to get label.</param>
        /// <returns>Vertex label.</returns>
        protected virtual string GetVertexLabel(string id, TVertex vertex)
        {
            return $"{id}: {vertex}";
        }

        /// <inheritdoc />
        protected override Edge AddEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            return MsaglGraph!.AddEdge(
                _verticesIds![edge.Source],
                _verticesIds[edge.Target]);
        }
    }
}
