using System;
using JetBrains.Annotations;
using Microsoft.Msagl.Drawing;

namespace QuikGraph.MSAGL
{
    /// <summary>
    /// MSAGL graph populator (using identifiable vertices).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class MsaglIndentifiableGraphPopulator<TVertex, TEdge> : MsaglGraphPopulator<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly VertexIdentity<TVertex> _verticesIdentities;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsaglIndentifiableGraphPopulator{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to convert to MSAGL graph.</param>
        /// <param name="verticesIdentities">Delegate that given a vertex return its identifier.</param>
        public MsaglIndentifiableGraphPopulator(
            [NotNull] IEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] VertexIdentity<TVertex> verticesIdentities)
            : base(visitedGraph)
        {
            _verticesIdentities = verticesIdentities ?? throw new ArgumentNullException(nameof(verticesIdentities));
        }

        /// <inheritdoc />
        protected override Node AddNode(TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return MsaglGraph.AddNode(_verticesIdentities(vertex));
        }

        /// <inheritdoc />
        protected override Edge AddEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            return MsaglGraph.AddEdge(
                _verticesIdentities(edge.Source),
                _verticesIdentities(edge.Target));
        }
    }
}