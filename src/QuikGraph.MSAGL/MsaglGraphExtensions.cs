using System;
using JetBrains.Annotations;

namespace QuikGraph.MSAGL
{
    /// <summary>
    /// Extensions related to MSAGL bridge.
    /// </summary>
    public static class MsaglGraphExtensions
    {
        /// <summary>
        /// Creates an <see cref="MsaglGraphPopulator{TVertex,TEdge}"/>..
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to MSAGL graph.</param>
        /// <returns>Graph populator.</returns>
        public static MsaglGraphPopulator<TVertex, TEdge> CreateMsaglPopulator<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            return new MsaglDefaultGraphPopulator<TVertex, TEdge>(graph);
        }

        /// <summary>
        /// Creates an <see cref="MsaglGraphPopulator{TVertex,TEdge}"/>..
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to MSAGL graph.</param>
        /// <param name="formatProvider">Graph format provider.</param>
        /// <param name="format">Graph format.</param>
        /// <returns>Graph populator.</returns>
        public static MsaglGraphPopulator<TVertex, TEdge> CreateMsaglPopulator<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] IFormatProvider formatProvider,
            [CanBeNull] string format)
            where TEdge : IEdge<TVertex>
        {
            return new MsaglToStringGraphPopulator<TVertex, TEdge>(graph, formatProvider, format);
        }
        
        /// <summary>
        /// Creates an <see cref="MsaglGraphPopulator{TVertex,TEdge}"/>..
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to MSAGL graph.</param>
        /// <param name="verticesIdentities">Delegate that given a vertex return its identifier.</param>
        /// <returns>Graph populator.</returns>
        public static MsaglGraphPopulator<TVertex, TEdge> CreateIdentifiableMsaglPopulator<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] VertexIdentity<TVertex> verticesIdentities)
            where TEdge : IEdge<TVertex>
        {
            return new MsaglIndentifiableGraphPopulator<TVertex, TEdge>(graph, verticesIdentities);
        }

        /// <summary>
        /// Converts <paramref name="graph"/> to an <see cref="Microsoft.Msagl.Drawing.Graph"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to MSAGL graph.</param>
        /// <param name="nodeAdded">Node added delegate.</param>
        /// <param name="edgeAdded">Edge added delegate.</param>
        /// <returns>MSAGL Graph.</returns>
        public static Microsoft.Msagl.Drawing.Graph ToMsaglGraph<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [CanBeNull] MsaglVertexNodeEventHandler<TVertex> nodeAdded,
            [CanBeNull] MsaglEdgeEventHandler<TVertex, TEdge> edgeAdded)
            where TEdge : IEdge<TVertex>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            var populator = CreateMsaglPopulator(graph);
            try
            {
                if (nodeAdded != null)
                    populator.NodeAdded += nodeAdded;
                if (edgeAdded != null)
                    populator.EdgeAdded += edgeAdded;

                populator.Compute();
                return populator.MsaglGraph;
            }
            finally
            {
                if (nodeAdded != null)
                    populator.NodeAdded -= nodeAdded;
                if (edgeAdded != null)
                    populator.EdgeAdded -= edgeAdded;
            }
        }

        /// <summary>
        /// Converts <paramref name="graph"/> to an <see cref="Microsoft.Msagl.Drawing.Graph"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to MSAGL graph.</param>
        /// <param name="verticesIdentities">Delegate that given a vertex return its identifier.</param>
        /// <param name="nodeAdded">Node added delegate.</param>
        /// <param name="edgeAdded">Edge added delegate.</param>
        /// <returns>MSAGL Graph.</returns>
        public static Microsoft.Msagl.Drawing.Graph ToIdentifiableMsaglGraph<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] VertexIdentity<TVertex> verticesIdentities,
            [CanBeNull] MsaglVertexNodeEventHandler<TVertex> nodeAdded,
            [CanBeNull] MsaglEdgeEventHandler<TVertex, TEdge> edgeAdded)
            where TEdge : IEdge<TVertex>
        {
            var populator = CreateIdentifiableMsaglPopulator(graph, verticesIdentities);
            try
            {
                if (nodeAdded != null)
                    populator.NodeAdded += nodeAdded;
                if (edgeAdded != null)
                    populator.EdgeAdded += edgeAdded;

                populator.Compute();
                return populator.MsaglGraph;
            }
            finally
            {
                if (nodeAdded != null)
                    populator.NodeAdded -= nodeAdded;
                if (edgeAdded != null)
                    populator.EdgeAdded -= edgeAdded;
            }
        }
    }
}