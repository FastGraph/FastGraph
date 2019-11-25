using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.ConnectedComponents;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Algorithm that checks if a graph is Eulerian.
    /// (has a path using all edges one and only one time).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class IsEulerianGraphAlgorithm<TVertex, TEdge>
        where TEdge : IUndirectedEdge<TVertex>
    {
        [NotNull]
        private readonly UndirectedGraph<TVertex, TEdge> _graph;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsEulerianGraphAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="graph">Graph to check.</param>
        public IsEulerianGraphAlgorithm([NotNull] IUndirectedGraph<TVertex, TEdge> graph)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));

            // Create new graph without parallel edges
            var newGraph = new UndirectedGraph<TVertex, TEdge>(
                false,
                graph.EdgeEqualityComparer);
            newGraph.AddVertexRange(graph.Vertices);
            newGraph.AddEdgeRange(graph.Edges);
            newGraph.RemoveEdgeIf(edge => edge.IsSelfEdge());

            _graph = newGraph;
        }

        private struct TrueIndexes
        {
            public TrueIndexes(int? firstIndex, int? secondIndex)
            {
                FirstIndex = firstIndex;
                SecondIndex = secondIndex;
            }

            public int? FirstIndex { get; }
            public int? SecondIndex { get; }
        }

        [Pure]
        private static TrueIndexes FirstAndSecondIndexOfTrue([NotNull] bool[] data)
        {
            // If no true elements returns (null, null)
            // If only one true element, returns (indexOfTrue, null)
            int? firstIndex = null;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i])
                {
                    if (!firstIndex.HasValue)
                    {
                        firstIndex = i;
                    }
                    else
                    {
                        return new TrueIndexes(firstIndex, i);
                    }
                }
            }

            return new TrueIndexes(firstIndex, null);
        }

        /// <summary>
        /// Gets the component state of the current graph.
        /// </summary>
        /// <returns><see cref="ComponentWithEdges"/> state.</returns>
        [Pure]
        public ComponentWithEdges CheckComponentsWithEdges()
        {
            var componentsAlgorithm = new ConnectedComponentsAlgorithm<TVertex, TEdge>(_graph);
            componentsAlgorithm.Compute();

            bool[] hasEdgesInComponent = new bool[componentsAlgorithm.ComponentCount];
            foreach (KeyValuePair<TVertex, int> verticesAndComponent in componentsAlgorithm.Components)
            {
                hasEdgesInComponent[verticesAndComponent.Value] = !_graph.IsAdjacentEdgesEmpty(verticesAndComponent.Key);
            }

            TrueIndexes trueIndexes = FirstAndSecondIndexOfTrue(hasEdgesInComponent);
            if (!trueIndexes.FirstIndex.HasValue)
                return ComponentWithEdges.NoComponent;

            if (trueIndexes.SecondIndex.HasValue)
                return ComponentWithEdges.ManyComponents;

            return ComponentWithEdges.OneComponent;
        }

        [Pure]
        private bool SatisfiesEulerianCondition([NotNull] TVertex vertex)
        {
            return _graph.AdjacentDegree(vertex) % 2 == 0;
        }

        /// <summary>
        /// Returns true if the graph is Eulerian, otherwise false.
        /// </summary>
        /// <returns>True if the graph is Eulerian, false otherwise.</returns>
        [Pure]
        public bool IsEulerian()
        {
            switch (CheckComponentsWithEdges())
            {
                case ComponentWithEdges.OneComponent:
                    return _graph.Vertices.All(SatisfiesEulerianCondition);
                case ComponentWithEdges.NoComponent:
                    return _graph.VertexCount == 1;
                // Many components
                default:
                    return false;
            }
        }
    }

    /// <summary>
    /// Algorithm that checks if a graph is Eulerian.
    /// (has a path use all edges one and only one time).
    /// </summary>
    public static class IsEulerianGraphAlgorithm
    {
        /// <summary>
        /// Returns true if the <paramref name="graph"/> is Eulerian, otherwise false.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to check.</param>
        /// <returns>True if the <paramref name="graph"/> is Eulerian, false otherwise.</returns>
        [Pure]
        public static bool IsEulerian<TVertex, TEdge>(
            [NotNull] IUndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IUndirectedEdge<TVertex>
        {
            return new IsEulerianGraphAlgorithm<TVertex, TEdge>(graph).IsEulerian();
        }
    }
}