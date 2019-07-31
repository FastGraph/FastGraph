using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using NUnit.Framework;
using QuikGraph.Serialization;

namespace QuikGraph.Tests.Serialization
{
    /// <summary>
    /// Tests for <see cref="GraphMLSerializer{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal class GraphMLSerializerIntegrationTests
    {
        [Test]
        public void DeserializeFromGraphMLNorth()
        {
            foreach (string graphMLFilePath in TestGraphFactory.GetFilePaths())
            {
                var graph = new AdjacencyGraph<string, Edge<string>>();
                using (var reader = new StreamReader(graphMLFilePath))
                {
                    graph.DeserializeFromGraphML(
                        reader,
                        id => id,
                        (source, target, id) => new Edge<string>(source, target));
                }

                var vertices = new Dictionary<string, string>();
                foreach (string vertex in graph.Vertices)
                    vertices.Add(vertex, vertex);

                // Check all nodes are loaded
                var settings = new XmlReaderSettings();
                settings.XmlResolver = new GraphMLXmlResolver();
#if SUPPORTS_XML_DTD_PROCESSING
                settings.DtdProcessing = DtdProcessing.Parse;
#else
                settings.ProhibitDtd = false;
#endif
                settings.ValidationFlags = XmlSchemaValidationFlags.None;

                using (XmlReader reader = XmlReader.Create(graphMLFilePath, settings))
                {
                    var document = new XPathDocument(reader);
                    foreach (XPathNavigator node in document.CreateNavigator().Select("/graphml/graph/node"))
                    {
                        string id = node.GetAttribute("id", "");
                        Assert.IsTrue(vertices.ContainsKey(id));
                    }

                    // Check all edges are loaded
                    foreach (XPathNavigator node in document.CreateNavigator().Select("/graphml/graph/edge"))
                    {
                        string source = node.GetAttribute("source", "");
                        string target = node.GetAttribute("target", "");
                        Assert.IsTrue(graph.ContainsEdge(vertices[source], vertices[target]));
                    }
                }
            }
        }
    }
}
