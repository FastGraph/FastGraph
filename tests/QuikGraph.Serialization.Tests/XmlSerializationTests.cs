using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using static QuikGraph.Tests.GraphTestHelpers;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;

namespace QuikGraph.Serialization.Tests
{
    /// <summary>
    /// Tests relative to XML serialization.
    /// </summary>
    [TestFixture]
    internal class XmlSerializationTests
    {
        #region Serialization

        [NotNull]
        private const string GraphNodeName = "family";

        [NotNull]
        private const string VertexNodeName = "person";

        [NotNull]
        private const string EdgeNodeName = "relationship";

        [NotNull]
        private const string XmlHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        [NotNull]
        private const string Indent = "    ";

        #region Test helpers

        private static void SerializeAndRead(
            [NotNull, InstantHandle] Action<XmlWriter> onSerialize,
            [NotNull, InstantHandle] Action<string> checkSerializedContent)
        {
            var settings = new XmlWriterSettings { Indent = true, IndentChars = Indent };
            using (var memory = new MemoryStream())
            {
                var writer = new StreamWriter(memory);
                try
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                    {
                        onSerialize(xmlWriter);
                    }

                    memory.Position = 0;

                    using (var reader = new StreamReader(memory))
                    {
                        checkSerializedContent(reader.ReadToEnd());
                    }
                }
                finally
                {
                    writer.Dispose();
                }
            }
        }

        #endregion

        [Test]
        public void SerializeToXml_Empty()
        {
            var emptyGraph = new BidirectionalGraph<Person, Edge<Person>>();

            SerializeAndRead(
                writer => emptyGraph.SerializeToXml(
                    writer,
                    person => person.Id,
                    emptyGraph.GetEdgeIdentity(),
                    GraphNodeName,
                    VertexNodeName,
                    EdgeNodeName,
                    ""),
                content =>
                {
                    StringAssert.AreEqualIgnoringCase(
                        $"{XmlHeader}{Environment.NewLine}<{GraphNodeName} />",
                        content);
                });
            }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> XmlSerializationGraphTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(new AdjacencyGraph<Person, TaggedEdge<Person, string>>());
                yield return new TestCaseData(new BidirectionalGraph<Person, TaggedEdge<Person, string>>());
                yield return new TestCaseData(new UndirectedGraph<Person, TaggedEdge<Person, string>>());
            }
        }

        [TestCaseSource(nameof(XmlSerializationGraphTestCases))]
        public void SerializeToXml<TGraph>([NotNull] TGraph graph)
            where TGraph: IMutableVertexAndEdgeSet<Person, TaggedEdge<Person, string>>
        {
            var persons = new List<Person>();
            var jacob = new Person("Jacob", "Hochstetler")
            {
                BirthDate = new DateTime(1712, 01, 01),
                BirthPlace = "Alsace, France",
                DeathDate = new DateTime(1776, 01, 01),
                DeathPlace = "Pennsylvania, USA",
                Gender = Gender.Male
            };
            persons.Add(jacob);

            var john = new Person("John", "Hochstetler")
            {
                BirthDate = new DateTime(1735, 01, 01),
                BirthPlace = "Alsace, France",
                DeathDate = new DateTime(1805, 04, 15),
                DeathPlace = "Summit Mills, PA",
                Gender = Gender.Male
            };
            persons.Add(john);

            var jonathon = new Person("Jonathon", "Hochstetler")
            {
                BirthPlace = "Pennsylvania",
                DeathDate = new DateTime(1823, 05, 08),
                Gender = Gender.Male
            };
            persons.Add(jonathon);

            var emanuel = new Person("Emanuel", "Hochstedler")
            {
                BirthDate = new DateTime(1855, 01, 01),
                DeathDate = new DateTime(1900, 01, 01),
                Gender = Gender.Male
            };
            persons.Add(emanuel);

            var relations = new List<TaggedEdge<Person, string>>
            {
                new TaggedEdge<Person, string>(jacob, john, jacob.ChildRelationshipText),
                new TaggedEdge<Person, string>(john, jonathon, john.ChildRelationshipText),
                new TaggedEdge<Person, string>(jonathon, emanuel, jonathon.ChildRelationshipText)
            };

            foreach (TaggedEdge<Person, string> relation in relations)
            {
                graph.AddVerticesAndEdge(relation);
            }

            SerializeAndRead(
                writer => graph.SerializeToXml(
                    writer,
                    person => person.Id,
                    graph.GetEdgeIdentity(),
                    GraphNodeName,
                    VertexNodeName,
                    EdgeNodeName,
                    ""),
                content =>
                {
                    var expectedSerializedGraph = new StringBuilder($"{XmlHeader}{Environment.NewLine}");
                    expectedSerializedGraph.AppendLine($"<{GraphNodeName}>");

                    foreach (Person person in persons)
                    {
                        expectedSerializedGraph.AppendLine($"{Indent}<{VertexNodeName} id=\"{person.Id}\" />");
                    }

                    for (int i = 0 ; i < relations.Count ; ++i)
                    {
                        expectedSerializedGraph.AppendLine($"{Indent}<{EdgeNodeName} id=\"{i}\" source=\"{relations[i].Source.Id}\" target=\"{relations[i].Target.Id}\" />");
                    }

                    expectedSerializedGraph.Append($"</{GraphNodeName}>");

                    StringAssert.AreEqualIgnoringCase(
                        expectedSerializedGraph.ToString(),
                        content);
                });
        }

        [Test]
        public void SerializationToXml_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new AdjacencyGraph<int, Edge<int>>();
            Assert.Throws<ArgumentNullException>(
                () => graph.SerializeToXml(
                    null,
                    vertex => vertex.ToString(),
                    graph.GetEdgeIdentity(),
                    GraphNodeName,
                    VertexNodeName,
                    EdgeNodeName,
                    ""));

            using (var memory = new MemoryStream())
            using (var writer = new StreamWriter(memory))
            using (XmlWriter xmlWriter = XmlWriter.Create(writer))
            {
                Assert.Throws<ArgumentNullException>(
                    () => ((AdjacencyGraph<int, Edge<int>>)null).SerializeToXml(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        graph.GetEdgeIdentity(),
                        GraphNodeName,
                        VertexNodeName,
                        EdgeNodeName,
                        ""));

                Assert.Throws<ArgumentNullException>(
                    () => graph.SerializeToXml(
                        xmlWriter,
                        null,
                        graph.GetEdgeIdentity(),
                        GraphNodeName,
                        VertexNodeName,
                        EdgeNodeName,
                        ""));

                Assert.Throws<ArgumentNullException>(
                    () => graph.SerializeToXml<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        null,
                        GraphNodeName,
                        VertexNodeName,
                        EdgeNodeName,
                        ""));

                Assert.Throws<ArgumentException>(
                    () => graph.SerializeToXml(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        graph.GetEdgeIdentity(),
                        null,
                        VertexNodeName,
                        EdgeNodeName,
                        ""));

                Assert.Throws<ArgumentException>(
                    () => graph.SerializeToXml(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        graph.GetEdgeIdentity(),
                        "",
                        VertexNodeName,
                        EdgeNodeName,
                        ""));

                Assert.Throws<ArgumentException>(
                    () => graph.SerializeToXml(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        graph.GetEdgeIdentity(),
                        GraphNodeName,
                        null,
                        EdgeNodeName,
                        ""));

                Assert.Throws<ArgumentException>(
                    () => graph.SerializeToXml(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        graph.GetEdgeIdentity(),
                        GraphNodeName,
                        "",
                        EdgeNodeName,
                        ""));

                Assert.Throws<ArgumentException>(
                    () => graph.SerializeToXml(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        graph.GetEdgeIdentity(),
                        GraphNodeName,
                        VertexNodeName,
                        null,
                        ""));

                Assert.Throws<ArgumentException>(
                    () => graph.SerializeToXml(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        graph.GetEdgeIdentity(),
                        GraphNodeName,
                        VertexNodeName,
                        "",
                        ""));

                Assert.Throws<ArgumentNullException>(
                    () => graph.SerializeToXml(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        graph.GetEdgeIdentity(),
                        GraphNodeName,
                        VertexNodeName,
                        EdgeNodeName,
                        null));
            }
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #region Deserialization

        [NotNull]
        private const string EmptyGraphFileName = "emptyGraph.xml";

        [NotNull]
        private const string TestGraphFileName = "testGraph.xml";

        #region Test helpers

        private static void AssetTestGraphContent<TEdge, TGraph>(
            [NotNull] TGraph graph,
            [NotNull, InstantHandle] Func<string, string, double, TEdge> edgeFactory)
            where TEdge : IEdge<string>
            where TGraph : IVertexSet<string>, IEdgeSet<string, TEdge>
        {
            AssertHasVertices(
                graph,
                new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            AssertHasEdges(
                graph,
                new[]
                {
                    edgeFactory("1", "2", 6.0),
                    edgeFactory("1", "4", 11.0),
                    edgeFactory("1", "5", 5.0),
                    edgeFactory("2", "5", 8.0),
                    edgeFactory("2", "3", 15.0),
                    edgeFactory("2", "4", 18.0),
                    edgeFactory("2", "7", 11.0),
                    edgeFactory("3", "4", 8.0),
                    edgeFactory("3", "8", 18.0),
                    edgeFactory("3", "9", 6.0),
                    edgeFactory("4", "6", 10.0),
                    edgeFactory("4", "7", 7.0),
                    edgeFactory("4", "11", 17.0),
                    edgeFactory("5", "6", 15.0),
                    edgeFactory("5", "7", 9.0),
                    edgeFactory("6", "11", 3.0),
                    edgeFactory("7", "8", 9.0),
                    edgeFactory("7", "9", 4.0),
                    edgeFactory("7", "11", 12.0),
                    edgeFactory("7", "10", 13.0),
                    edgeFactory("8", "9", 14.0),
                    edgeFactory("8", "12", 5.0),
                    edgeFactory("9", "10", 19.0),
                    edgeFactory("10", "12", 2.0),
                    edgeFactory("11", "12", 7.0)
                });
        }

        #endregion

        [Test]
        public void DeserializeFromXml_Document_Empty()
        {
            XPathDocument document = new XPathDocument(GetGraphFilePath(EmptyGraphFileName));

            // Directed graph
            AdjacencyGraph<string, Edge<string>> adjacencyGraph = document.DeserializeFromXml(
                "graph",
                "node",
                "edge",
                nav => new AdjacencyGraph<string, Edge<string>>(),
                nav => nav.GetAttribute("id", ""),
                nav => new Edge<string>(
                    nav.GetAttribute("source", ""),
                    nav.GetAttribute("target", "")));
            AssertEmptyGraph(adjacencyGraph);

            // Directed bidirectional graph
            BidirectionalGraph<string, Edge<string>> bidirectionalGraph = document.DeserializeFromXml(
                "graph",
                "node",
                "edge",
                nav => new BidirectionalGraph<string, Edge<string>>(),
                nav => nav.GetAttribute("id", ""),
                nav => new Edge<string>(
                    nav.GetAttribute("source", ""),
                    nav.GetAttribute("target", "")));
            AssertEmptyGraph(bidirectionalGraph);

            // Undirected graph
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
            AssertEmptyGraph(undirectedGraph);
        }

        [Test]
        public void DeserializeFromXml_Document()
        {
            XPathDocument document = new XPathDocument(GetGraphFilePath(TestGraphFileName));

            // Directed graph
            AdjacencyGraph<string, EquatableEdge<string>> adjacencyGraph = document.DeserializeFromXml(
                "graph",
                "node",
                "edge",
                nav => new AdjacencyGraph<string, EquatableEdge<string>>(),
                nav => nav.GetAttribute("id", ""),
                nav => new EquatableEdge<string>(
                    nav.GetAttribute("source", ""),
                    nav.GetAttribute("target", "")));
            AssetTestGraphContent(
                adjacencyGraph,
                (v1, v2, weight) => new EquatableEdge<string>(v1, v2));

            // Directed bidirectional graph
            BidirectionalGraph<string, EquatableEdge<string>> bidirectionalGraph = document.DeserializeFromXml(
                "graph",
                "node",
                "edge",
                nav => new BidirectionalGraph<string, EquatableEdge<string>>(),
                nav => nav.GetAttribute("id", ""),
                nav => new EquatableEdge<string>(
                    nav.GetAttribute("source", ""),
                    nav.GetAttribute("target", "")));
            AssetTestGraphContent(
                bidirectionalGraph,
                (v1, v2, weight) => new EquatableEdge<string>(v1, v2));

            // Undirected graph
            UndirectedGraph<string, EquatableTaggedEdge<string, double>> undirectedGraph = document.DeserializeFromXml(
                "graph",
                "node",
                "edge",
                nav => new UndirectedGraph<string, EquatableTaggedEdge<string, double>>(),
                nav => nav.GetAttribute("id", ""),
                nav => new EquatableTaggedEdge<string, double>(
                    nav.GetAttribute("source", ""),
                    nav.GetAttribute("target", ""),
                    int.Parse(nav.GetAttribute("weight", ""))));
            AssetTestGraphContent(
                undirectedGraph,
                (v1, v2, weight) => new EquatableTaggedEdge<string, double>(v1, v2, weight));
        }

        [Test]
        public void DeserializeFromXml_Document_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((XPathDocument) null).DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    nav => new AdjacencyGraph<string, EquatableEdge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new EquatableEdge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            XPathDocument document = new XPathDocument(GetGraphFilePath(TestGraphFileName));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                null,
                "node",
                "edge",
                nav => new AdjacencyGraph<string, Edge<string>>(),
                nav => nav.GetAttribute("id", ""),
                nav => new Edge<string>(
                    nav.GetAttribute("source", ""),
                    nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                    "",
                    "node",
                    "edge",
                    nav => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                    "graph",
                    null,
                    "edge",
                    nav => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                    "graph",
                    "",
                    "edge",
                    nav => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                    "graph",
                    "node",
                    null,
                    nav => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                    "graph",
                    "node",
                    "",
                    nav => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentNullException>(
                () => document.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    null,
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentNullException>(
                () => document.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    nav => new AdjacencyGraph<string, Edge<string>>(),
                    null,
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentNullException>(
                () => document.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    nav => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    null));

            // No graph node found
            Assert.Throws<InvalidOperationException>(
                () => document.DeserializeFromXml(
                    "g",    // No node named "g" for the graph
                    "node",
                    "edge",
                    nav => new AdjacencyGraph<string, EquatableEdge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new EquatableEdge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void DeserializeFromXml_Reader_Empty()
        {
            using (XmlReader reader = XmlReader.Create(GetGraphFilePath(EmptyGraphFileName)))
            {
                AdjacencyGraph<string, Edge<string>> adjacencyGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    r => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute")));
                AssertEmptyGraph(adjacencyGraph);
            }

            using (XmlReader reader = XmlReader.Create(GetGraphFilePath(EmptyGraphFileName)))
            {
                BidirectionalGraph<string, Edge<string>> bidirectionalGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    r => new BidirectionalGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute")));
                AssertEmptyGraph(bidirectionalGraph);
            }

            using (XmlReader reader = XmlReader.Create(GetGraphFilePath(EmptyGraphFileName)))
            {
                UndirectedGraph<string, TaggedEdge<string, double>> undirectedGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    r => new UndirectedGraph<string, TaggedEdge<string, double>>(),
                    r => r.GetAttribute("id"),
                    r => new TaggedEdge<string, double>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"),
                        int.Parse(
                            r.GetAttribute("weight") ?? throw new AssertionException("Must have weight attribute"))));
                AssertEmptyGraph(undirectedGraph);
            }
        }

        [Test]
        public void DeserializeFromXml_Reader()
        {
            using (XmlReader reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                AdjacencyGraph<string, EquatableEdge<string>> adjacencyGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    r => new AdjacencyGraph<string, EquatableEdge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new EquatableEdge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute")));
                AssetTestGraphContent(
                    adjacencyGraph,
                    (v1, v2, weight) => new EquatableEdge<string>(v1, v2));
            }

            using (XmlReader reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                BidirectionalGraph<string, EquatableEdge<string>> bidirectionalGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    r => new BidirectionalGraph<string, EquatableEdge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new EquatableEdge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute")));
                AssetTestGraphContent(
                    bidirectionalGraph,
                    (v1, v2, weight) => new EquatableEdge<string>(v1, v2));
            }

            using (XmlReader reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                UndirectedGraph<string, EquatableTaggedEdge<string, double>> undirectedGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    r => new UndirectedGraph<string, EquatableTaggedEdge<string, double>>(),
                    r => r.GetAttribute("id"),
                    r => new EquatableTaggedEdge<string, double>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"),
                        int.Parse(
                            r.GetAttribute("weight") ?? throw new AssertionException("Must have weight attribute"))));
                AssetTestGraphContent(
                    undirectedGraph,
                    (v1, v2, weight) => new EquatableTaggedEdge<string, double>(v1, v2, weight));
            }
        }

        [Test]
        public void DeserializeFromXml_Reader_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((XmlReader)null).DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    r => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

            using (XmlReader reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    "",
                    null,
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    "",
                    r => new AdjacencyGraph<string, Edge<string>>(),
                    null,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    "",
                    r => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id"),
                    null));


                Assert.Throws<ArgumentNullException>(
                    () => reader.DeserializeFromXml(
                        null,
                        r => r.Name == "vertex",
                        r => r.Name == "edge",
                        r => new AdjacencyGraph<string, Edge<string>>(),
                        r => r.GetAttribute("id"),
                        r => new Edge<string>(
                            r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                            r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentNullException>(
                    () => reader.DeserializeFromXml(
                        r => r.Name == "graph",
                        null,
                        r => r.Name == "edge",
                        r => new AdjacencyGraph<string, Edge<string>>(),
                        r => r.GetAttribute("id"),
                        r => new Edge<string>(
                            r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                            r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentNullException>(
                    () => reader.DeserializeFromXml(
                        r => r.Name == "graph",
                        r => r.Name == "vertex",
                        null,
                        r => new AdjacencyGraph<string, Edge<string>>(),
                        r => r.GetAttribute("id"),
                        r => new Edge<string>(
                            r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                            r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));


                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    null,
                    "node",
                    "edge",
                    "",
                    r => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    "",
                    "node",
                    "edge",
                    "",
                    r => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    "graph",
                    null,
                    "edge",
                    "",
                    r => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    "graph",
                    "",
                    "edge",
                    "",
                    r => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    "graph",
                    "node",
                    null,
                    "",
                    r => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "",
                    "",
                    r => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    null,
                    r => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));


                // No graph node found
                Assert.Throws<InvalidOperationException>(() => reader.DeserializeFromXml(
                    "g",    // No node named "g" for the graph
                    "node",
                    "edge",
                    "",
                    r => new AdjacencyGraph<string, Edge<string>>(), 
                    r => r.GetAttribute("id"),
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));
            }
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion
    }
}