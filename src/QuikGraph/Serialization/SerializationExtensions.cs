using System;
using System.Xml;
#if SUPPORTS_GRAPHS_SERIALIZATION
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.XPath;
#endif
using JetBrains.Annotations;

namespace QuikGraph.Serialization
{
    /// <summary>
    /// Extensions to serialize/deserialize graphs.
    /// </summary>
    public static class SerializationExtensions
    {
#if SUPPORTS_GRAPHS_SERIALIZATION
        /// <summary>
        /// Serializes the <paramref name="graph"/> to the <paramref name="stream"/> using the .NET serialization binary formatter.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="stream">Stream in which serializing the graph.</param>
        public static void SerializeToBinary<TVertex, TEdge>(
            [NotNull] this IGraph<TVertex, TEdge> graph,
            [NotNull] Stream stream)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
            Contract.Requires(stream != null);
            Contract.Requires(stream.CanWrite);
#endif

            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, graph);
        }

        /// <summary>
        /// Deserializes a graph instance from a <paramref name="stream"/> that was serialized using the .NET serialization binary formatter.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="stream">Stream from which deserializing the graph.</param>
        /// <returns>Deserialized graph.</returns>
        [JetBrains.Annotations.Pure]
        public static TGraph DeserializeFromBinary<TVertex, TEdge, TGraph>([NotNull] this Stream stream)
            where TGraph : IGraph<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(stream != null);
            Contract.Requires(stream.CanRead);
#endif

            var formatter = new BinaryFormatter();
            object result = formatter.Deserialize(stream);
            return (TGraph)result;
        }

        /// <summary>
        /// Deserializes a graph instance from a generic XML stream, using an <see cref="XPathDocument"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="document">Input XML document.</param>
        /// <param name="graphXPath">XPath expression to the graph node. The first node is considered.</param>
        /// <param name="verticesXPath">XPath expression from the graph node to the vertex nodes.</param>
        /// <param name="edgesXPath">XPath expression from the graph node to the edge nodes.</param>
        /// <param name="graphFactory">Delegate that instantiates the empty graph instance, given the graph node.</param>
        /// <param name="vertexFactory">Delegate that instantiates a vertex instance, given the vertex node.</param>
        /// <param name="edgeFactory">Delegate that instantiates an edge instance, given the edge node.</param>
        /// <returns>Deserialized graph.</returns>
        /// <exception cref="InvalidOperationException">
        /// If the <paramref name="document"/> does not allow to get an XML navigator
        /// or if the <paramref name="graphXPath"/> does not allow to get graph node.
        /// </exception>
        [JetBrains.Annotations.Pure]
        public static TGraph DeserializeFromXml<TVertex, TEdge, TGraph>(
            [NotNull] this IXPathNavigable document,
            [NotNull] string graphXPath,
            [NotNull] string verticesXPath,
            [NotNull] string edgesXPath,
            [NotNull, InstantHandle] Func<XPathNavigator, TGraph> graphFactory,
            [NotNull, InstantHandle] Func<XPathNavigator, TVertex> vertexFactory,
            [NotNull, InstantHandle] Func<XPathNavigator, TEdge> edgeFactory)
            where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(document != null);
            Contract.Requires(graphXPath != null);
            Contract.Requires(verticesXPath != null);
            Contract.Requires(edgesXPath != null);
            Contract.Requires(graphFactory != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
#endif

            XPathNavigator navigator = document.CreateNavigator();
            if (navigator is null)
                throw new InvalidOperationException("Document does not allow to get an XML navigator.");

            XPathNavigator graphNode = navigator.SelectSingleNode(graphXPath);
            if (graphNode is null)
                throw new InvalidOperationException("Could not find graph node.");
            TGraph graph = graphFactory(graphNode);
            foreach (XPathNavigator vertexNode in graphNode.Select(verticesXPath))
            {
                TVertex vertex = vertexFactory(vertexNode);
                graph.AddVertex(vertex);
            }

            foreach (XPathNavigator edgeNode in graphNode.Select(edgesXPath))
            {
                TEdge edge = edgeFactory(edgeNode);
                graph.AddEdge(edge);
            }

            return graph;
        }
#endif

        /// <summary>
        /// Deserializes a graph instance from a generic XML stream, using an <see cref="XmlReader"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="reader">Input XML reader.</param>
        /// <param name="graphPredicate">Predicate that returns a value indicating if the current XML node is a graph. The first match is considered.</param>
        /// <param name="vertexPredicate">Predicate that returns a value indicating if the current XML node is a vertex.</param>
        /// <param name="edgePredicate">Predicate that returns a value indicating if the current XML node is an edge.</param>
        /// <param name="graphFactory">Delegate that instantiates the empty graph instance, given the graph node.</param>
        /// <param name="vertexFactory">Delegate that instantiates a vertex instance, given the vertex node.</param>
        /// <param name="edgeFactory">Delegate that instantiates an edge instance, given the edge node.</param>
        /// <returns>Deserialized graph.</returns>
        /// <exception cref="InvalidOperationException">If the graph node cannot be found.</exception>
        [JetBrains.Annotations.Pure]
        public static TGraph DeserializeFromXml<TVertex, TEdge, TGraph>(
            [NotNull] this XmlReader reader,
            [NotNull, InstantHandle] Predicate<XmlReader> graphPredicate,
            [NotNull, InstantHandle] Predicate<XmlReader> vertexPredicate,
            [NotNull, InstantHandle] Predicate<XmlReader> edgePredicate,
            [NotNull, InstantHandle] Func<XmlReader, TGraph> graphFactory,
            [NotNull, InstantHandle] Func<XmlReader, TVertex> vertexFactory,
            [NotNull, InstantHandle] Func<XmlReader, TEdge> edgeFactory)
            where TGraph : class, IMutableVertexAndEdgeSet<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(reader != null);
            Contract.Requires(graphPredicate != null);
            Contract.Requires(vertexPredicate != null);
            Contract.Requires(edgePredicate != null);
            Contract.Requires(graphFactory != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
#endif

            // Find the graph node
            TGraph graph = null;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && graphPredicate(reader))
                {
                    graph = graphFactory(reader);
                    break;
                }
            }

            if (graph is null)
                throw new InvalidOperationException("Could not find graph node.");

            using (XmlReader graphReader = reader.ReadSubtree())
            {
                while (graphReader.Read())
                {
                    if (graphReader.NodeType == XmlNodeType.Element)
                    {
                        if (vertexPredicate(graphReader))
                        {
                            TVertex vertex = vertexFactory(graphReader);
                            graph.AddVertex(vertex);
                        }
                        else if (edgePredicate(reader))
                        {
                            TEdge edge = edgeFactory(graphReader);
                            graph.AddEdge(edge);
                        }
                    }
                }
            }

            return graph;
        }

        /// <summary>
        /// Deserializes a graph from a generic XML stream, using an <see cref="XmlReader"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="reader">Input XML reader.</param>
        /// <param name="namespaceUri">XML namespace.</param>
        /// <param name="graphElementName">Name of the XML node holding graph information. The first node is considered.</param>
        /// <param name="vertexElementName">Name of the XML node holding vertex information.</param>
        /// <param name="edgeElementName">Name of the XML node holding edge information.</param>
        /// <param name="graphFactory">Delegate that instantiates the empty graph instance, given the graph node.</param>
        /// <param name="vertexFactory">Delegate that instantiates a vertex instance, given the vertex node.</param>
        /// <param name="edgeFactory">Delegate that instantiates an edge instance, given the edge node.</param>
        /// <returns>Deserialized graph.</returns>
        /// <exception cref="InvalidOperationException">If the graph node cannot be found.</exception>
        [JetBrains.Annotations.Pure]
        public static TGraph DeserializeFromXml<TVertex, TEdge, TGraph>(
            [NotNull] this XmlReader reader,
            [NotNull] string graphElementName,
            [NotNull] string vertexElementName,
            [NotNull] string edgeElementName,
            [NotNull] string namespaceUri,
            [NotNull, InstantHandle] Func<XmlReader, TGraph> graphFactory,
            [NotNull, InstantHandle] Func<XmlReader, TVertex> vertexFactory,
            [NotNull, InstantHandle] Func<XmlReader, TEdge> edgeFactory)
            where TGraph : class, IMutableVertexAndEdgeSet<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(reader != null);
            Contract.Requires(graphElementName != null);
            Contract.Requires(vertexElementName != null);
            Contract.Requires(edgeElementName != null);
            Contract.Requires(graphFactory != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
#endif

            return DeserializeFromXml(
                reader,
                r => r.Name == graphElementName && r.NamespaceURI == namespaceUri,
                r => r.Name == vertexElementName && r.NamespaceURI == namespaceUri,
                r => r.Name == edgeElementName && r.NamespaceURI == namespaceUri,
                graphFactory,
                vertexFactory,
                edgeFactory);
        }

        /// <summary>
        /// Serializes a graph instance to a generic XML stream, using an <see cref="XmlWriter"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="graph">Graph to serialize.</param>
        /// <param name="writer">XML writer.</param>
        /// <param name="vertexIdentity">The vertex identity.</param>
        /// <param name="edgeIdentity">The edge identity.</param>
        /// <param name="graphElementName">Name of the graph element.</param>
        /// <param name="vertexElementName">Name of the vertex element.</param>
        /// <param name="edgeElementName">Name of the edge element.</param>
        /// <param name="namespaceUri">XML namespace.</param>
        public static void SerializeToXml<TVertex, TEdge, TGraph>(
            [NotNull] this TGraph graph,
            [NotNull] XmlWriter writer,
            [NotNull, InstantHandle] VertexIdentity<TVertex> vertexIdentity,
            [NotNull, InstantHandle] EdgeIdentity<TVertex, TEdge> edgeIdentity,
            [NotNull] string graphElementName,
            [NotNull] string vertexElementName,
            [NotNull] string edgeElementName,
            [NotNull] string namespaceUri)
            where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            SerializeToXml(
                graph,
                writer,
                vertexIdentity,
                edgeIdentity,
                graphElementName,
                vertexElementName,
                edgeElementName,
                namespaceUri,
                null,
                null,
                null);
        }

        /// <summary>
        /// Serializes a graph instance to a generic XML stream, using an <see cref="XmlWriter"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="graph">Graph to serialize.</param>
        /// <param name="writer">XML writer.</param>
        /// <param name="vertexIdentity">The vertex identity.</param>
        /// <param name="edgeIdentity">The edge identity.</param>
        /// <param name="graphElementName">Name of the graph element.</param>
        /// <param name="vertexElementName">Name of the vertex element.</param>
        /// <param name="edgeElementName">Name of the edge element.</param>
        /// <param name="namespaceUri">XMl namespace.</param>
        /// <param name="writeGraphAttributes">Delegate to write graph attributes (optional).</param>
        /// <param name="writeVertexAttributes">Delegate to write vertex attributes (optional).</param>
        /// <param name="writeEdgeAttributes">Delegate to write edge attributes (optional).</param>
        public static void SerializeToXml<TVertex, TEdge, TGraph>(
            [NotNull] this TGraph graph,
            [NotNull] XmlWriter writer,
            [NotNull, InstantHandle] VertexIdentity<TVertex> vertexIdentity,
            [NotNull, InstantHandle] EdgeIdentity<TVertex, TEdge> edgeIdentity,
            [NotNull] string graphElementName,
            [NotNull] string vertexElementName,
            [NotNull] string edgeElementName,
            [NotNull] string namespaceUri,
            [CanBeNull, InstantHandle] Action<XmlWriter, TGraph> writeGraphAttributes,
            [CanBeNull, InstantHandle] Action<XmlWriter, TVertex> writeVertexAttributes,
            [CanBeNull, InstantHandle] Action<XmlWriter, TEdge> writeEdgeAttributes)
            where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
            Contract.Requires(writer != null);
            Contract.Requires(vertexIdentity != null);
            Contract.Requires(edgeIdentity != null);
            Contract.Requires(!string.IsNullOrEmpty(graphElementName));
            Contract.Requires(!string.IsNullOrEmpty(vertexElementName));
            Contract.Requires(!string.IsNullOrEmpty(edgeElementName));
#endif

            writer.WriteStartElement(graphElementName, namespaceUri);

            writeGraphAttributes?.Invoke(writer, graph);

            foreach (TVertex vertex in graph.Vertices)
            {
                writer.WriteStartElement(vertexElementName, namespaceUri);
                writer.WriteAttributeString("id", namespaceUri, vertexIdentity(vertex));
                writeVertexAttributes?.Invoke(writer, vertex);
                writer.WriteEndElement();
            }

            foreach (TEdge edge in graph.Edges)
            {
                writer.WriteStartElement(edgeElementName, namespaceUri);
                writer.WriteAttributeString("id", namespaceUri, edgeIdentity(edge));
                writer.WriteAttributeString("source", namespaceUri, vertexIdentity(edge.Source));
                writer.WriteAttributeString("target", namespaceUri, vertexIdentity(edge.Target));
                writeEdgeAttributes?.Invoke(writer, edge);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}
