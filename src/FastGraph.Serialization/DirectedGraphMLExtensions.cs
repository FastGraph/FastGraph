#nullable enable

using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using JetBrains.Annotations;
using FastGraph.Algorithms;
using FastGraph.Serialization.DirectedGraphML;

namespace FastGraph.Serialization
{
    /// <summary>
    /// Directed graph Markup Language extensions.
    /// </summary>
    public static class DirectedGraphMLExtensions
    {
        private static XmlSerializer? _directedGraphSerializer;

        /// <summary>
        /// Gets the DirectedGraph XML serializer.
        /// </summary>
        public static XmlSerializer DirectedGraphSerializer =>
            _directedGraphSerializer ?? (_directedGraphSerializer = new XmlSerializer(typeof(DirectedGraph)));

        /// <summary>
        /// Writes the DGML data structure to the XML writer.
        /// </summary>
        /// <param name="graph">Graph instance to write.</param>
        /// <param name="filePath">Path to the file to write into.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="filePath"/> is <see langword="null"/>.</exception>
        public static void WriteXml(this DirectedGraph graph, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("Must provide a file path.", nameof(filePath));

            using (StreamWriter stream = File.CreateText(filePath))
            {
                WriteXml(graph, stream);
            }
        }

        /// <summary>
        /// Writes the DGML data structure to the <see cref="T:System.Xml.XmlWriter"/>.
        /// </summary>
        /// <param name="graph">Graph instance to write.</param>
        /// <param name="writer">XML writer in which writing graph data.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteXml(this DirectedGraph graph, XmlWriter writer)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            DirectedGraphSerializer.Serialize(writer, graph);
        }

        /// <summary>
        /// Writes the DGML data structure to the <see cref="T:System.Xml.XmlWriter"/>.
        /// </summary>
        /// <param name="graph">Graph instance to write.</param>
        /// <param name="stream">Stream in which writing graph data.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="stream"/> is <see langword="null"/>.</exception>
        public static void WriteXml(this DirectedGraph graph, Stream stream)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            DirectedGraphSerializer.Serialize(stream, graph);
        }

        /// <summary>
        /// Writes the DGML data structure to the <see cref="T:System.IO.TextWriter"/>.
        /// </summary>
        /// <param name="graph">Graph instance to write.</param>
        /// <param name="writer">Text writer in which writing graph data.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public static void WriteXml(this DirectedGraph graph, TextWriter writer)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            DirectedGraphSerializer.Serialize(writer, graph);
        }

        /// <summary>
        /// Populates a DGML graph from a graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to <see cref="DirectedGraph"/>.</param>
        /// <returns>Converted graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        [Pure]
        public static DirectedGraph ToDirectedGraphML<TVertex, TEdge>(
            this IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            return ToDirectedGraphML(
                graph,
                graph.GetVertexIdentity(),
                graph.GetEdgeIdentity());
        }

        /// <summary>
        /// Populates a DGML graph from a graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to <see cref="DirectedGraph"/>.</param>
        /// <param name="verticesColors">Function that gives the color of a vertex.</param>
        /// <returns>Converted graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesColors"/> is <see langword="null"/>.</exception>
        [Pure]
        public static DirectedGraph ToDirectedGraphML<TVertex, TEdge>(
            this IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            Func<TVertex, GraphColor> verticesColors)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            if (verticesColors is null)
                throw new ArgumentNullException(nameof(verticesColors));

            return ToDirectedGraphML(
                graph,
                graph.GetVertexIdentity(),
                graph.GetEdgeIdentity(),
                (vertex, node) =>
                {
                    GraphColor color = verticesColors(vertex);
                    switch (color)
                    {
                        case GraphColor.Black:
                            node.Background = "Black";
                            break;
                        case GraphColor.Gray:
                            node.Background = "LightGray";
                            break;
                        case GraphColor.White:
                            node.Background = "White";
                            break;
                    }
                },
                default);
        }

        /// <summary>
        /// Populates a DGML graph from a graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to <see cref="DirectedGraph"/>.</param>
        /// <param name="vertexIdentity">Vertex identity method.</param>
        /// <param name="edgeIdentity">Edge identity method.</param>
        /// <returns>Converted graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertexIdentity"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeIdentity"/> is <see langword="null"/>.</exception>
        [Pure]
        public static DirectedGraph ToDirectedGraphML<TVertex, TEdge>(
            this IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            VertexIdentity<TVertex> vertexIdentity,
            EdgeIdentity<TVertex, TEdge> edgeIdentity)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            return ToDirectedGraphML(
                graph,
                vertexIdentity,
                edgeIdentity,
                default,
                default);
        }

        /// <summary>
        /// Populates a DGML graph from a graph.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to convert to <see cref="DirectedGraph"/>.</param>
        /// <param name="vertexIdentity">Vertex identity method.</param>
        /// <param name="edgeIdentity">Edge identity method.</param>
        /// <param name="formatNode">Formats a vertex into a <see cref="DirectedGraphNode"/>.</param>
        /// <param name="formatEdge">Formats an edge into a <see cref="DirectedGraphLink"/>.</param>
        /// <returns>Converted graph.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertexIdentity"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeIdentity"/> is <see langword="null"/>.</exception>
        [Pure]
        public static DirectedGraph ToDirectedGraphML<TVertex, TEdge>(
            this IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            VertexIdentity<TVertex> vertexIdentity,
            EdgeIdentity<TVertex, TEdge> edgeIdentity,
            Action<TVertex, DirectedGraphNode>? formatNode,
            Action<TEdge, DirectedGraphLink>? formatEdge)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new DirectedGraphMLAlgorithm<TVertex, TEdge>(
                graph,
                vertexIdentity,
                edgeIdentity);

            if (formatNode != default)
            {
                algorithm.FormatNode += formatNode;
            }

            if (formatEdge != default)
            {
                algorithm.FormatEdge += formatEdge;
            }

            algorithm.Compute();

            return algorithm.DirectedGraph!;
        }

        /// <summary>
        /// Saves and opens the given <paramref name="graph"/> as DGML.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">Graph to open.</param>
        /// <param name="filePath">Path to the file to save.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        public static void OpenAsDGML<TVertex, TEdge>(
            this IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            string? filePath)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));

            if (filePath is null)
            {
                filePath = "graph.DGML";
            }

            graph.ToDirectedGraphML().WriteXml(filePath);

            if (Debugger.IsAttached)
            {
                Process.Start(filePath);
            }
        }
    }
}
