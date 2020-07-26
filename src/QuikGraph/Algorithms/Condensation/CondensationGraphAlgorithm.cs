using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms.ConnectedComponents;

namespace QuikGraph.Algorithms.Condensation
{
    /// <summary>
    /// Algorithm that condensate a graph with strongly (or not) components.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    public sealed class CondensationGraphAlgorithm<TVertex, TEdge, TGraph> : AlgorithmBase<IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CondensationGraphAlgorithm{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public CondensationGraphAlgorithm([NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }

        /// <summary>
        /// Condensed graph.
        /// </summary>
        public IMutableBidirectionalGraph<TGraph, CondensedEdge<TVertex, TEdge, TGraph>> CondensedGraph { get; private set; }

        /// <summary>
        /// Gets or sets the strongly connected components flag.
        /// Indicates if the algorithm should do strongly connected components or not.
        /// </summary>
        public bool StronglyConnected { get; set; } = true;

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            // Create condensed graph
            CondensedGraph = new BidirectionalGraph<TGraph, CondensedEdge<TVertex, TEdge, TGraph>>(false);
            if (VisitedGraph.VertexCount == 0)
                return;

            // Compute strongly connected components
            var components = new Dictionary<TVertex, int>(VisitedGraph.VertexCount);
            int componentCount = ComputeComponentCount(components);

            ThrowIfCancellationRequested();

            // Create vertices list
            var condensedVertices = new Dictionary<int, TGraph>(componentCount);
            for (int i = 0; i < componentCount; ++i)
            {
                var vertex = new TGraph();
                condensedVertices.Add(i, vertex);
                CondensedGraph.AddVertex(vertex);
            }

            // Adding vertices
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                condensedVertices[components[vertex]].AddVertex(vertex);
            }

            ThrowIfCancellationRequested();

            // Condensed edges
            var condensedEdges = new Dictionary<EdgeKey, CondensedEdge<TVertex, TEdge, TGraph>>(componentCount);

            // Iterate over edges and condensate graph
            foreach (TEdge edge in VisitedGraph.Edges)
            {
                // Get component ids
                int sourceID = components[edge.Source];
                int targetID = components[edge.Target];

                // Get vertices
                TGraph sources = condensedVertices[sourceID];
                if (sourceID == targetID)
                {
                    sources.AddEdge(edge);
                    continue;
                }

                // At last add edge
                var edgeKey = new EdgeKey(sourceID, targetID);
                if (!condensedEdges.TryGetValue(edgeKey, out CondensedEdge<TVertex, TEdge, TGraph> condensedEdge))
                {
                    TGraph targets = condensedVertices[targetID];

                    condensedEdge = new CondensedEdge<TVertex, TEdge, TGraph>(sources, targets);
                    condensedEdges.Add(edgeKey, condensedEdge);
                    CondensedGraph.AddEdge(condensedEdge);
                }

                condensedEdge.Edges.Add(edge);
            }
        }

        #endregion

        [Pure]
        private int ComputeComponentCount([NotNull] IDictionary<TVertex, int> components)
        {
            IConnectedComponentAlgorithm<TVertex, TEdge, IVertexListGraph<TVertex, TEdge>> componentAlgorithm;
            if (StronglyConnected)
            {
                componentAlgorithm = new StronglyConnectedComponentsAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    components);
            }
            else
            {
                componentAlgorithm = new WeaklyConnectedComponentsAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    components);
            }

            componentAlgorithm.Compute();

            return componentAlgorithm.ComponentCount;
        }

        private struct EdgeKey : IEquatable<EdgeKey>
        {
            private readonly int _sourceID;
            private readonly int _targetID;

            public EdgeKey(int sourceID, int targetID)
            {
                _sourceID = sourceID;
                _targetID = targetID;
            }

            /// <inheritdoc />
            public bool Equals(EdgeKey other)
            {
                return _sourceID == other._sourceID
                       && _targetID == other._targetID;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                return obj is EdgeKey edgeKey
                       && Equals(edgeKey);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return HashCodeHelpers.Combine(_sourceID, _targetID);
            }
        }
    }
}