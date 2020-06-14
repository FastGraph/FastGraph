using System;
using JetBrains.Annotations;
using Microsoft.Msagl.Drawing;

namespace QuikGraph.MSAGL
{
    /// <summary>
    /// Extensions related to MSAGL bridge.
    /// </summary>
    public static class MsaglGraphExtensions
    {
        /// <summary>
        /// Creates an <see cref="MsaglGraphPopulator{TVertex,TEdge}"/>.
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
        /// Creates an <see cref="MsaglGraphPopulator{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to MSAGL graph.</param>
        /// <param name="format">Graph format.</param>
        /// <param name="formatProvider">Graph format provider.</param>
        /// <returns>Graph populator.</returns>
        public static MsaglGraphPopulator<TVertex, TEdge> CreateMsaglPopulator<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [CanBeNull] string format,
            [CanBeNull] IFormatProvider formatProvider = null)
            where TEdge : IEdge<TVertex>
        {
            return new MsaglToStringGraphPopulator<TVertex, TEdge>(graph, format, formatProvider);
        }
        
        /// <summary>
        /// Creates an <see cref="MsaglGraphPopulator{TVertex,TEdge}"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to MSAGL graph.</param>
        /// <param name="vertexIdentity">Delegate that given a vertex return its identifier.</param>
        /// <returns>Graph populator.</returns>
        public static MsaglGraphPopulator<TVertex, TEdge> CreateMsaglPopulator<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] VertexIdentity<TVertex> vertexIdentity)
            where TEdge : IEdge<TVertex>
        {
            return new MsaglIdentifiableGraphPopulator<TVertex, TEdge>(graph, vertexIdentity);
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
        public static Graph ToMsaglGraph<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [CanBeNull] MsaglVertexNodeEventHandler<TVertex> nodeAdded = null,
            [CanBeNull] MsaglEdgeEventHandler<TVertex, TEdge> edgeAdded = null)
            where TEdge : IEdge<TVertex>
        {
            MsaglGraphPopulator<TVertex, TEdge> populator = CreateMsaglPopulator(graph);
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
        /// <param name="vertexIdentity">Delegate that given a vertex return its identifier.</param>
        /// <param name="nodeAdded">Node added delegate.</param>
        /// <param name="edgeAdded">Edge added delegate.</param>
        /// <returns>MSAGL Graph.</returns>
        public static Graph ToMsaglGraph<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] VertexIdentity<TVertex> vertexIdentity,
            [CanBeNull] MsaglVertexNodeEventHandler<TVertex> nodeAdded = null,
            [CanBeNull] MsaglEdgeEventHandler<TVertex, TEdge> edgeAdded = null)
            where TEdge : IEdge<TVertex>
        {
            MsaglGraphPopulator<TVertex, TEdge> populator = CreateMsaglPopulator(graph, vertexIdentity);
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