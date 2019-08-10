using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;
using QuikGraph.Serialization;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;

namespace QuikGraph.Tests.Serialization
{
    /// <summary>
    /// Tests relative to XML serialization.
    /// </summary>
    [TestFixture]
    internal class XmlSerializationTests
    {
        [Test]
        public void DeserializeFromXml()
        {
            XPathDocument document = new XPathDocument(GetGraphFilePath("repro12273.xml"));
            UndirectedGraph<string, TaggedEdge<string, double>> undirectedGraph = document.DeserializeFromXml(
                "graph",
                "node",
                "edge",
                nav => new UndirectedGraph<string, TaggedEdge<string, double>>(),
                nav => nav.GetAttribute("id", ""),
                nav => new TaggedEdge<string, double>(
                    nav.GetAttribute("source", ""),
                    nav.GetAttribute("target", ""),
                    int.Parse(nav.GetAttribute("weight", ""))));

            using (XmlReader reader = XmlReader.Create(GetGraphFilePath("repro12273.xml")))
            {
                UndirectedGraph<string, TaggedEdge<string, double>> undirectedGraph2 = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    r => new UndirectedGraph<string, TaggedEdge<string, double>>(),
                    r => r.GetAttribute("id"),
                    r => new TaggedEdge<string, double>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"),
                        int.Parse(r.GetAttribute("weight") ?? throw new AssertionException("Must have weight attribute"))));

                Assert.AreEqual(undirectedGraph.VertexCount, undirectedGraph2.VertexCount);
                Assert.AreEqual(undirectedGraph.EdgeCount, undirectedGraph2.EdgeCount);
            }
        }
    }
}
