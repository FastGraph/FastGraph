using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Services;
using QuikGraph.Serialization.DirectedGraphML;

namespace QuikGraph.Serialization
{
    /// <summary>
    /// Algorithm that creates a <see cref="DirectedGraph"/> from a given directed graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class DirectedGraphMLAlgorithm<TVertex, TEdge> : AlgorithmBase<IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly VertexIdentity<TVertex> _vertexIdentities;

        [NotNull]
        private readonly EdgeIdentity<TVertex, TEdge> _edgeIdentities;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectedGraphMLAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="vertexIdentities">Vertex identity method.</param>
        /// <param name="edgeIdentities">Edge identity method.</param>
        public DirectedGraphMLAlgorithm(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            [NotNull] VertexIdentity<TVertex> vertexIdentities,
            [NotNull] EdgeIdentity<TVertex, TEdge> edgeIdentities)
            : base(visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertexIdentities != null);
            Contract.Requires(edgeIdentities != null);
#endif

            _vertexIdentities = vertexIdentities;
            _edgeIdentities = edgeIdentities;
        }

        /// <summary>
        /// Gets the resulting <see cref="DirectedGraphML.DirectedGraph"/>.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        public DirectedGraph DirectedGraph { get; private set; }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ICancelManager cancelManager = Services.CancelManager;
            DirectedGraph = new DirectedGraph();

            var nodes = new List<DirectedGraphNode>(VisitedGraph.VertexCount);
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                if (cancelManager.IsCancelling)
                    return;

                var node = new DirectedGraphNode { Id = _vertexIdentities(vertex) };
                OnFormatNode(vertex, node);
                nodes.Add(node);
            }

            DirectedGraph.Nodes = nodes.ToArray();

            var links = new List<DirectedGraphLink>(VisitedGraph.EdgeCount);
            foreach (TEdge edge in VisitedGraph.Edges)
            {
                if (cancelManager.IsCancelling)
                    return;

                var link = new DirectedGraphLink
                {
                    Label = _edgeIdentities(edge),
                    Source = _vertexIdentities(edge.Source),
                    Target = _vertexIdentities(edge.Target)
                };
                OnFormatEdge(edge, link);
                links.Add(link);
            }

            DirectedGraph.Links = links.ToArray();

            OnFormatGraph();
        }

        #endregion

        /// <summary>
        /// Fired when a new node is added to the <see cref="DirectedGraph"/>.
        /// </summary>
        public event Action<TVertex, DirectedGraphNode> FormatNode;

        private void OnFormatNode([NotNull] TVertex vertex, [NotNull] DirectedGraphNode node)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertex != null);
            Contract.Requires(node != null);
#endif

            FormatNode?.Invoke(vertex, node);
        }

        /// <summary>
        /// Fired when a new link is added to the <see cref="DirectedGraph"/>.
        /// </summary>
        public event Action<TEdge, DirectedGraphLink> FormatEdge;

        private void OnFormatEdge([NotNull] TEdge edge, [NotNull] DirectedGraphLink link)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edge != null);
            Contract.Requires(link != null);
#endif

            FormatEdge?.Invoke(edge, link);
        }

        /// <summary>
        /// Fired when the graph is about to be returned.
        /// </summary>
        public event Action<IVertexAndEdgeListGraph<TVertex, TEdge>, DirectedGraph> FormatGraph;

        private void OnFormatGraph()
        {
            FormatGraph?.Invoke(VisitedGraph, DirectedGraph);
        }
    }
}
