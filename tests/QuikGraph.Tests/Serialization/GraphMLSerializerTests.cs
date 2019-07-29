using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;
using QuikGraph.Tests;

namespace QuikGraph.Serialization
{
    [TestFixture]
    internal class GraphMLSerializerIntegrationTests : QuikGraphUnitTests
    {
        [Test]
        public void DeserializeFromGraphMLNorth()
        {
            foreach (var graphmlFile in TestGraphFactory.GetFilePaths())
            {
                Console.Write(graphmlFile);
                var g = new AdjacencyGraph<string, Edge<string>>();
                using (var reader = new StreamReader(graphmlFile))
                {
                    g.DeserializeFromGraphML(
                        reader,
                        id => id,
                        (source, target, id) => new Edge<string>(source, target)
                        );
                }
                Console.Write(": {0} vertices, {1} edges", g.VertexCount, g.EdgeCount);

                var vertices = new Dictionary<string, string>();
                foreach(var v in g.Vertices)
                    vertices.Add(v, v);

                // check all nodes are loaded
                var settings = new XmlReaderSettings();
                settings.XmlResolver = new GraphMLXmlResolver();
#if SUPPORTS_XML_DTD_PROCESSING
                settings.DtdProcessing = DtdProcessing.Parse;
#else
                settings.ProhibitDtd = false;
#endif
                settings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None;
                using(var xreader = XmlReader.Create(graphmlFile, settings))
                {
                    var doc = new XPathDocument(xreader);
                    foreach (XPathNavigator node in doc.CreateNavigator().Select("/graphml/graph/node"))
                    {
                        string id = node.GetAttribute("id", "");
                        Assert.IsTrue(vertices.ContainsKey(id));
                    }
                    Console.Write(", vertices ok");

                    // check all edges are loaded
                    foreach (XPathNavigator node in doc.CreateNavigator().Select("/graphml/graph/edge"))
                    {
                        string source = node.GetAttribute("source", "");
                        string target = node.GetAttribute("target", "");
                        Assert.IsTrue(g.ContainsEdge(vertices[source], vertices[target]));
                    }
                    Console.Write(", edges ok");
                }
                Console.WriteLine();
            }
        }
    }
}
