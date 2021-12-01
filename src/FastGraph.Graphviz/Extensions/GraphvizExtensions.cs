using System;
using System.Net.Http;
#if SUPPORTS_SVG_CONVERSION
using System.IO;
using System.Net;
#endif
using JetBrains.Annotations;

namespace FastGraph.Graphviz
{
    /// <summary>
    /// Helper extensions to render graphs to graphviz.
    /// </summary>
    public static class GraphvizExtensions
    {
        /// <summary>
        /// Renders a graph to the Graphviz DOT format.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>Graph serialized in DOT format.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        [Pure]
        [NotNull]
        public static string ToGraphviz<TVertex, TEdge>([NotNull] this IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new GraphvizAlgorithm<TVertex, TEdge>(graph);
            return algorithm.Generate();
        }

        /// <summary>
        /// Renders a graph to the Graphviz DOT format.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <param name="initAlgorithm">Delegate that initializes the DOT generation algorithm.</param>
        /// <returns>Graph serialized in DOT format.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="initAlgorithm"/> is <see langword="null"/>.</exception>
        [Pure]
        [NotNull]
        public static string ToGraphviz<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull, InstantHandle] Action<GraphvizAlgorithm<TVertex, TEdge>> initAlgorithm)
            where TEdge : IEdge<TVertex>
        {
            if (initAlgorithm is null)
                throw new ArgumentNullException(nameof(initAlgorithm));

            var algorithm = new GraphvizAlgorithm<TVertex, TEdge>(graph);
            initAlgorithm(algorithm);
            return algorithm.Generate();
        }

#if SUPPORTS_SVG_CONVERSION
        /// <summary>
        /// Dot to Svg REST API endpoint.
        /// </summary>
        [NotNull]
        public const string DotToSvgApiEndpoint = "https://rise4fun.com/rest/ask/Agl/";

        /// <summary>
        /// Performs a layout of <paramref name="graph"/> from DOT format to an
        /// SVG (Scalable Vector Graphics) file by calling Agl through
        /// the https://rise4fun.com/ REST services.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>The svg graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        [Pure]
        [NotNull]
        [Obsolete("Conversion via external web service no longer supported.", error: true)]
        public static string ToSvg<TVertex, TEdge>([NotNull] this IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            throw new NotSupportedException("Conversion via external web service no longer supported.");
        }

        /// <summary>
        /// Performs a layout of <paramref name="graph"/> from DOT format to an
        /// SVG (Scalable Vector Graphics) file by calling Agl through
        /// the https://rise4fun.com/ REST services.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert.</param>
        /// <param name="initAlgorithm">Delegate that initializes the DOT generation algorithm.</param>
        /// <returns>The svg graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="initAlgorithm"/> is <see langword="null"/>.</exception>
        [Pure]
        [NotNull]
        [Obsolete("Conversion via external web service no longer supported.", error: true)]
        public static string ToSvg<TVertex, TEdge>(
            [NotNull] this IEdgeListGraph<TVertex, TEdge> graph,
            [NotNull, InstantHandle] Action<GraphvizAlgorithm<TVertex, TEdge>> initAlgorithm)
            where TEdge : IEdge<TVertex>
        {
            throw new NotSupportedException("Conversion via external web service no longer supported.");
        }

        /// <summary>
        /// Performs a layout from DOT to  an SVG (Scalable Vector Graphics) file
        /// by calling Agl through the https://rise4fun.com/ REST services.
        /// </summary>
        /// <param name="dot">The dot graph</param>
        /// <returns>The svg graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="dot"/> is <see langword="null"/>.</exception>
        [Pure]
        [NotNull]
        [Obsolete("Conversion via external web service no longer supported.", error: true)]
        public static string ToSvg([NotNull] string dot)
        {
            throw new NotSupportedException("Conversion via external web service no longer supported.");
        }
#endif
    }
}