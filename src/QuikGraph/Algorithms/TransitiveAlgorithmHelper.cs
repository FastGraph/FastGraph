using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Helper for transitive algorithms.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    internal class TransitiveAlgorithmHelper<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly BidirectionalGraph<TVertex, TEdge> _graph;

        internal TransitiveAlgorithmHelper([NotNull] BidirectionalGraph<TVertex, TEdge> initialGraph)
        {
            _graph = initialGraph;
        }

        /// <summary>
        /// Runs through the graph and calls <paramref name="action"/>
        /// for each couple of indirect ancestor vertex of a given vertex.
        /// </summary>
        public void InternalCompute(
            [NotNull, InstantHandle] Action<BidirectionalGraph<TVertex, TEdge>, TVertex, TVertex, TEdge> action)
        {

            // Iterate in topological order, track indirect ancestors and remove edges from them to the visited vertex
            var verticesAncestors = new Dictionary<TVertex, HashSet<TVertex>>();
            foreach (TVertex vertexId in _graph.TopologicalSort().ToList()) // Making sure we do not mess enumerator or something
            {
                // TODO think of some heuristic value here. Like (verticesCount / 2) or (verticesCount / 3)
                var vertexPredecessors = new List<TVertex>();
                var vertexAncestors = new HashSet<TVertex>();
                verticesAncestors[vertexId] = vertexAncestors;

                // Get indirect ancestors
                foreach (TEdge inEdge in _graph.InEdges(vertexId))
                {
                    TVertex predecessor = inEdge.Source;
                    vertexPredecessors.Add(predecessor);

                    // Add all the ancestors of the predecessors
                    vertexAncestors.UnionWith(verticesAncestors[predecessor]);
                }

                // Add indirect edges
                foreach (TVertex indirectAncestor in vertexAncestors)
                {
                    _graph.TryGetEdge(
                        indirectAncestor, 
                        vertexId, 
                        out TEdge foundIndirectEdge);

                    action(_graph, indirectAncestor, vertexId, foundIndirectEdge);
                }

                // Add predecessors to ancestors list
                vertexAncestors.UnionWith(vertexPredecessors);
            }
        }
    }
}
