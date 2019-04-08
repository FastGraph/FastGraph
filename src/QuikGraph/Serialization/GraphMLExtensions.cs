#if SUPPORTS_GRAPHS_SERIALIZATION
using System;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.IO;
using System.Xml;
using System.Xml.Schema;
using QuickGraph.Algorithms;

namespace QuickGraph.Serialization
{
    public static class GraphMLExtensions
    {
        // The following use of XmlWriter.Create fails in Silverlight.

        public static void SerializeToGraphML<TVertex, TEdge, TGraph>(
#if !NET20
            this 
#endif
            TGraph graph,
            string fileName,
            VertexIdentity<TVertex> vertexIdentities,
            EdgeIdentity<TVertex, TEdge> edgeIdentities)
            where TEdge : IEdge<TVertex>
            where TGraph : IEdgeListGraph<TVertex, TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(fileName != null);
            Contract.Requires(fileName.Length > 0);
#endif
            var settings = new XmlWriterSettings() { Indent = true, IndentChars = "    " };
            var writer = XmlWriter.Create(fileName, settings);
            SerializeToGraphML<TVertex, TEdge, TGraph>(graph, writer, vertexIdentities, edgeIdentities);
            writer.Flush();
            writer.Close();
        }

        public static void SerializeToGraphML<TVertex, TEdge, TGraph>(
#if !NET20
            this 
#endif
            TGraph graph,
            string fileName
            )
            where TEdge : IEdge<TVertex>
            where TGraph : IEdgeListGraph<TVertex, TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(fileName != null);
            Contract.Requires(fileName.Length > 0);
#endif

            var settings = new XmlWriterSettings() { Indent = true, IndentChars = "    " };
            var writer = XmlWriter.Create(fileName, settings);
            SerializeToGraphML<TVertex, TEdge, TGraph>(graph, writer);
            writer.Flush();
            writer.Close();
        }

        public static void SerializeToGraphML<TVertex, TEdge,TGraph>(
#if !NET20
            this 
#endif
            TGraph graph,
            XmlWriter writer,
            VertexIdentity<TVertex> vertexIdentities,
            EdgeIdentity<TVertex, TEdge> edgeIdentities)
            where TEdge : IEdge<TVertex>
            where TGraph : IEdgeListGraph<TVertex, TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
            Contract.Requires(writer != null);
#endif

            var serializer = new GraphMLSerializer<TVertex, TEdge,TGraph>();
            serializer.Serialize(writer, graph, vertexIdentities, edgeIdentities);
        }

        public static void SerializeToGraphML<TVertex, TEdge, TGraph>(
#if !NET20
this 
#endif
            TGraph graph,
            XmlWriter writer)
            where TEdge : IEdge<TVertex>
            where TGraph : IEdgeListGraph<TVertex, TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
            Contract.Requires(writer != null);
#endif

            var vertexIdentity = AlgorithmExtensions.GetVertexIdentity<TVertex>(graph);
            var edgeIdentity = AlgorithmExtensions.GetEdgeIdentity<TVertex, TEdge>(graph);

            SerializeToGraphML<TVertex, TEdge, TGraph>(
                graph,
                writer,
                vertexIdentity,
                edgeIdentity
                );
        }

        public static void DeserializeFromGraphML<TVertex, TEdge,TGraph>(
#if !NET20
            this 
#endif
            TGraph graph,
            string fileName,
            IdentifiableVertexFactory<TVertex> vertexFactory,
            IdentifiableEdgeFactory<TVertex, TEdge> edgeFactory
            )
            where TEdge : IEdge<TVertex>
            where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(fileName != null);
            Contract.Requires(fileName.Length > 0);
#endif

            var reader = new StreamReader(fileName);
            DeserializeFromGraphML<TVertex, TEdge,TGraph>(graph, reader, vertexFactory, edgeFactory);
        }

        public static void DeserializeFromGraphML<TVertex, TEdge,TGraph>(
#if !NET20
            this 
#endif
            TGraph graph,
            TextReader reader,
            IdentifiableVertexFactory<TVertex> vertexFactory,
            IdentifiableEdgeFactory<TVertex, TEdge> edgeFactory
            )
            where TEdge : IEdge<TVertex>
            where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
            Contract.Requires(reader != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
#endif

            var settings = new XmlReaderSettings();
            settings.ValidationFlags = XmlSchemaValidationFlags.None;

            settings.XmlResolver = new GraphMLXmlResolver();

            using(var xreader = XmlReader.Create(reader, settings))
                DeserializeFromGraphML<TVertex, TEdge,TGraph>(graph, xreader, vertexFactory, edgeFactory);
        }

        public static void DeserializeFromGraphML<TVertex, TEdge,TGraph>(
#if !NET20
            this 
#endif
            TGraph graph,
            XmlReader reader,
            IdentifiableVertexFactory<TVertex> vertexFactory,
            IdentifiableEdgeFactory<TVertex, TEdge> edgeFactory
            )
            where TEdge : IEdge<TVertex>
            where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
            Contract.Requires(reader != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
#endif

            var serializer = new GraphMLDeserializer<TVertex, TEdge,TGraph>();
            serializer.Deserialize(reader, graph, vertexFactory, edgeFactory);
        }

        public static void DeserializeAndValidateFromGraphML<TVertex, TEdge,TGraph>(
#if !NET20
            this 
#endif
            TGraph graph,
            TextReader reader,
            IdentifiableVertexFactory<TVertex> vertexFactory,
            IdentifiableEdgeFactory<TVertex, TEdge> edgeFactory
            )
            where TEdge : IEdge<TVertex>
            where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(graph != null);
            Contract.Requires(reader != null);
            Contract.Requires(vertexFactory != null);
            Contract.Requires(edgeFactory != null);
#endif

            var serializer = new GraphMLDeserializer<TVertex, TEdge,TGraph>();
            var settings = new XmlReaderSettings();
            // add graphxml schema
            settings.ValidationType = ValidationType.Schema;
            settings.XmlResolver = new GraphMLXmlResolver();
            AddGraphMLSchema<TVertex, TEdge, TGraph>(settings);

            try
            {
                settings.ValidationEventHandler += ValidationEventHandler;

                // reader and validating
                using (var xreader = XmlReader.Create(reader, settings))
                    serializer.Deserialize(xreader, graph, vertexFactory, edgeFactory);
            }
            finally
            {
                settings.ValidationEventHandler -= ValidationEventHandler;
            }
        }

        private static void AddGraphMLSchema<TVertex, TEdge,TGraph>(XmlReaderSettings settings)
            where TEdge : IEdge<TVertex>
            where TGraph : IEdgeListGraph<TVertex, TEdge>
        {
            using (var xsdStream = typeof(GraphMLExtensions).Assembly.GetManifestResourceStream(typeof(GraphMLExtensions), "graphml.xsd"))
            using (var xsdReader = XmlReader.Create(xsdStream, settings))
                settings.Schemas.Add(GraphMLXmlResolver.GraphMLNamespace, xsdReader);
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Error)
                throw new InvalidOperationException(args.Message);
        }
    }
}
#endif