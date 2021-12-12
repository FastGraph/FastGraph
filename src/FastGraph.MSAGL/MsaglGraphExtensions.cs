#nullable enable

using Microsoft.Msagl.Drawing;

namespace FastGraph.MSAGL
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
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        public static MsaglGraphPopulator<TVertex, TEdge> CreateMsaglPopulator<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
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
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        public static MsaglGraphPopulator<TVertex, TEdge> CreateMsaglPopulator<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> graph,
            string? format,
            IFormatProvider? formatProvider = default)
            where TVertex : notnull
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
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertexIdentity"/> is <see langword="null"/>.</exception>
        public static MsaglGraphPopulator<TVertex, TEdge> CreateMsaglPopulator<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> graph,
            VertexIdentity<TVertex> vertexIdentity)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            return new MsaglIdentifiableGraphPopulator<TVertex, TEdge>(graph, vertexIdentity);
        }

        /// <summary>
        /// Converts <paramref name="graph"/> to an <see cref="T:Microsoft.Msagl.Drawing.Graph"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to MSAGL graph.</param>
        /// <param name="nodeAdded">Node added delegate.</param>
        /// <param name="edgeAdded">Edge added delegate.</param>
        /// <returns>MSAGL Graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        public static Graph ToMsaglGraph<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> graph,
            MsaglVertexNodeEventHandler<TVertex>? nodeAdded = default,
            MsaglEdgeEventHandler<TVertex, TEdge>? edgeAdded = default)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            MsaglGraphPopulator<TVertex, TEdge> populator = CreateMsaglPopulator(graph);
            try
            {
                if (nodeAdded != default)
                {
                    populator.NodeAdded += nodeAdded;
                }

                if (edgeAdded != default)
                {
                    populator.EdgeAdded += edgeAdded;
                }

                populator.Compute();
                return populator.MsaglGraph!;
            }
            finally
            {
                if (nodeAdded != default)
                {
                    populator.NodeAdded -= nodeAdded;
                }

                if (edgeAdded != default)
                {
                    populator.EdgeAdded -= edgeAdded;
                }
            }
        }

        /// <summary>
        /// Converts <paramref name="graph"/> to an <see cref="T:Microsoft.Msagl.Drawing.Graph"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to MSAGL graph.</param>
        /// <param name="vertexIdentity">Delegate that given a vertex return its identifier.</param>
        /// <param name="nodeAdded">Node added delegate.</param>
        /// <param name="edgeAdded">Edge added delegate.</param>
        /// <returns>MSAGL Graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertexIdentity"/> is <see langword="null"/>.</exception>
        public static Graph ToMsaglGraph<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> graph,
            VertexIdentity<TVertex> vertexIdentity,
            MsaglVertexNodeEventHandler<TVertex>? nodeAdded = default,
            MsaglEdgeEventHandler<TVertex, TEdge>? edgeAdded = default)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            MsaglGraphPopulator<TVertex, TEdge> populator = CreateMsaglPopulator(graph, vertexIdentity);
            try
            {
                if (nodeAdded != default)
                {
                    populator.NodeAdded += nodeAdded;
                }

                if (edgeAdded != default)
                {
                    populator.EdgeAdded += edgeAdded;
                }

                populator.Compute();
                return populator.MsaglGraph!;
            }
            finally
            {
                if (nodeAdded != default)
                {
                    populator.NodeAdded -= nodeAdded;
                }

                if (edgeAdded != default)
                {
                    populator.EdgeAdded -= edgeAdded;
                }
            }
        }
    }
}
