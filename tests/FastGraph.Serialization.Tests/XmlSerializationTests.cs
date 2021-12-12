#nullable enable

using System.Text;
using System.Xml;
using System.Xml.XPath;
using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms;
using static FastGraph.Tests.GraphTestHelpers;
using static FastGraph.Tests.FastGraphUnitTestsHelpers;
using static FastGraph.Serialization.Tests.SerializationTestCaseSources;

namespace FastGraph.Serialization.Tests
{
    /// <summary>
    /// Tests relative to XML serialization.
    /// </summary>
    [TestFixture]
    internal sealed class XmlSerializationTests
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
            [InstantHandle] Action<XmlWriter> onSerialize,
            [InstantHandle] Action<string> checkSerializedContent)
        {
            var settings = new XmlWriterSettings { Indent = true, IndentChars = Indent };
            using (var memory = new MemoryStream())
            using (var writer = new StreamWriter(memory))
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
        public void SerializeToXml<TGraph>(TGraph graph)
            where TGraph : IMutableVertexAndEdgeSet<Person, TaggedEdge<Person, string>>
        {
            var jacob = new Person("Jacob", "Hochstetler")
            {
                BirthDate = new DateTime(1712, 01, 01),
                BirthPlace = "Alsace, France",
                DeathDate = new DateTime(1776, 01, 01),
                DeathPlace = "Pennsylvania, USA",
                Gender = Gender.Male
            };

            var john = new Person("John", "Hochstetler")
            {
                BirthDate = new DateTime(1735, 01, 01),
                BirthPlace = "Alsace, France",
                DeathDate = new DateTime(1805, 04, 15),
                DeathPlace = "Summit Mills, PA",
                Gender = Gender.Male
            };

            var jonathon = new Person("Jonathon", "Hochstetler")
            {
                BirthPlace = "Pennsylvania",
                DeathDate = new DateTime(1823, 05, 08),
                Gender = Gender.Male
            };

            var emanuel = new Person("Emanuel", "Hochstedler")
            {
                BirthDate = new DateTime(1855, 01, 01),
                DeathDate = new DateTime(1900, 01, 01),
                Gender = Gender.Male
            };

            graph.AddVerticesAndEdgeRange(
                new TaggedEdge<Person, string>[]
                {
                    new(jacob, john, jacob.ChildRelationshipText),
                    new(john, jonathon, john.ChildRelationshipText),
                    new(jonathon, emanuel, jonathon.ChildRelationshipText)
                });

            SerializeAndRead(
                writer => graph.SerializeToXml(
                    writer,
                    person => person.Id,
                    graph.GetEdgeIdentity(),
                    GraphNodeName,
                    VertexNodeName,
                    EdgeNodeName,
                    ""),
                content => CheckXmlGraphSerialization(graph, content));

            #region Local function

            static void CheckXmlGraphSerialization(
                IEdgeListGraph<Person, TaggedEdge<Person, string>> graph,
                string xmlGraph)
            {
                var expectedSerializedGraph = new StringBuilder($"{XmlHeader}{Environment.NewLine}");
                expectedSerializedGraph.AppendLine($"<{GraphNodeName}>");

                foreach (Person person in graph.Vertices)
                {
                    expectedSerializedGraph.AppendLine($"{Indent}<{VertexNodeName} id=\"{person.Id}\" />");
                }

                TaggedEdge<Person, string>[] relations = graph.Edges.ToArray();
                for (int i = 0; i < relations.Length; ++i)
                {
                    expectedSerializedGraph.AppendLine($"{Indent}<{EdgeNodeName} id=\"{i}\" source=\"{relations[i].Source.Id}\" target=\"{relations[i].Target.Id}\" />");
                }

                expectedSerializedGraph.Append($"</{GraphNodeName}>");

                StringAssert.AreEqualIgnoringCase(
                    expectedSerializedGraph.ToString(),
                    xmlGraph);
            }

            #endregion
        }

        [Test]
        public void SerializationToXml_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8631
#pragma warning disable CS8625
            var graph = new AdjacencyGraph<int, Edge<int>>();
            Assert.Throws<ArgumentNullException>(
                () => graph.SerializeToXml(
                    default,
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
                    () => ((AdjacencyGraph<int, Edge<int>>?)default).SerializeToXml(
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
                        default,
                        graph.GetEdgeIdentity(),
                        GraphNodeName,
                        VertexNodeName,
                        EdgeNodeName,
                        ""));

                Assert.Throws<ArgumentNullException>(
                    () => graph.SerializeToXml<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        default,
                        GraphNodeName,
                        VertexNodeName,
                        EdgeNodeName,
                        ""));

                Assert.Throws<ArgumentException>(
                    () => graph.SerializeToXml(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        graph.GetEdgeIdentity(),
                        default,
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
                        default,
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
                        default,
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
                        default));
            }
#pragma warning restore CS8625
#pragma warning restore CS8631
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
            TGraph graph,
            [InstantHandle] Func<string, string, double, TEdge> edgeFactory)
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
            var document = new XPathDocument(GetGraphFilePath(EmptyGraphFileName));

            // Directed graph
            AdjacencyGraph<string, Edge<string>> adjacencyGraph = document.DeserializeFromXml(
                "graph",
                "node",
                "edge",
                _ => new AdjacencyGraph<string, Edge<string>>(),
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
                _ => new BidirectionalGraph<string, Edge<string>>(),
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
                _ => new UndirectedGraph<string, TaggedEdge<string, double>>(),
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
            var document = new XPathDocument(GetGraphFilePath(TestGraphFileName));

            // Directed graph
            AdjacencyGraph<string, EquatableEdge<string>> adjacencyGraph = document.DeserializeFromXml(
                "graph",
                "node",
                "edge",
                _ => new AdjacencyGraph<string, EquatableEdge<string>>(),
                nav => nav.GetAttribute("id", ""),
                nav => new EquatableEdge<string>(
                    nav.GetAttribute("source", ""),
                    nav.GetAttribute("target", "")));
            AssetTestGraphContent(
                adjacencyGraph,
                (v1, v2, _) => new EquatableEdge<string>(v1, v2));

            // Directed bidirectional graph
            BidirectionalGraph<string, EquatableEdge<string>> bidirectionalGraph = document.DeserializeFromXml(
                "graph",
                "node",
                "edge",
                _ => new BidirectionalGraph<string, EquatableEdge<string>>(),
                nav => nav.GetAttribute("id", ""),
                nav => new EquatableEdge<string>(
                    nav.GetAttribute("source", ""),
                    nav.GetAttribute("target", "")));
            AssetTestGraphContent(
                bidirectionalGraph,
                (v1, v2, _) => new EquatableEdge<string>(v1, v2));

            // Undirected graph
            UndirectedGraph<string, EquatableTaggedEdge<string, double>> undirectedGraph = document.DeserializeFromXml(
                "graph",
                "node",
                "edge",
                _ => new UndirectedGraph<string, EquatableTaggedEdge<string, double>>(),
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
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(
                () => ((XPathDocument?)default).DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    _ => new AdjacencyGraph<string, EquatableEdge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new EquatableEdge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            var document = new XPathDocument(GetGraphFilePath(TestGraphFileName));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                default,
                "node",
                "edge",
                _ => new AdjacencyGraph<string, Edge<string>>(),
                nav => nav.GetAttribute("id", ""),
                nav => new Edge<string>(
                    nav.GetAttribute("source", ""),
                    nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                    "",
                    "node",
                    "edge",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                    "graph",
                    default,
                    "edge",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                    "graph",
                    "",
                    "edge",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                    "graph",
                    "node",
                    default,
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentException>(
                () => document.DeserializeFromXml(
                    "graph",
                    "node",
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentNullException>(
                () => document.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    default,
                    nav => nav.GetAttribute("id", ""),
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentNullException>(
                () => document.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    default,
                    nav => new Edge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));

            Assert.Throws<ArgumentNullException>(
                () => document.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    default));

            // No graph node found
            Assert.Throws<InvalidOperationException>(
                () => document.DeserializeFromXml(
                    "g",    // No node named "g" for the graph
                    "node",
                    "edge",
                    _ => new AdjacencyGraph<string, EquatableEdge<string>>(),
                    nav => nav.GetAttribute("id", ""),
                    nav => new EquatableEdge<string>(
                        nav.GetAttribute("source", ""),
                        nav.GetAttribute("target", ""))));
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void DeserializeFromXml_Reader_Empty()
        {
            using (var reader = XmlReader.Create(GetGraphFilePath(EmptyGraphFileName)))
            {
                AdjacencyGraph<string, Edge<string>> adjacencyGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute")));
                AssertEmptyGraph(adjacencyGraph);
            }

            using (var reader = XmlReader.Create(GetGraphFilePath(EmptyGraphFileName)))
            {
                BidirectionalGraph<string, Edge<string>> bidirectionalGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new BidirectionalGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute")));
                AssertEmptyGraph(bidirectionalGraph);
            }

            using (var reader = XmlReader.Create(GetGraphFilePath(EmptyGraphFileName)))
            {
                UndirectedGraph<string, TaggedEdge<string, double>> undirectedGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new UndirectedGraph<string, TaggedEdge<string, double>>(),
                    r => r.GetAttribute("id")!,
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
            using (var reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                AdjacencyGraph<string, EquatableEdge<string>> adjacencyGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new AdjacencyGraph<string, EquatableEdge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new EquatableEdge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute")));
                AssetTestGraphContent(
                    adjacencyGraph,
                    (v1, v2, _) => new EquatableEdge<string>(v1, v2));
            }

            using (var reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                BidirectionalGraph<string, EquatableEdge<string>> bidirectionalGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new BidirectionalGraph<string, EquatableEdge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new EquatableEdge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute")));
                AssetTestGraphContent(
                    bidirectionalGraph,
                    (v1, v2, _) => new EquatableEdge<string>(v1, v2));
            }

            using (var reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                UndirectedGraph<string, EquatableTaggedEdge<string, double>> undirectedGraph = reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new UndirectedGraph<string, EquatableTaggedEdge<string, double>>(),
                    r => r.GetAttribute("id")!,
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
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(
                () => ((XmlReader?)default).DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

            using (var reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    "",
                    default,
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    default,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    default));


                Assert.Throws<ArgumentNullException>(
                    () => reader.DeserializeFromXml(
                        default,
                        r => r.Name == "vertex",
                        r => r.Name == "edge",
                        _ => new AdjacencyGraph<string, Edge<string>>(),
                        r => r.GetAttribute("id")!,
                        r => new Edge<string>(
                            r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                            r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentNullException>(
                    () => reader.DeserializeFromXml(
                        r => r.Name == "graph",
                        default,
                        r => r.Name == "edge",
                        _ => new AdjacencyGraph<string, Edge<string>>(),
                        r => r.GetAttribute("id")!,
                        r => new Edge<string>(
                            r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                            r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentNullException>(
                    () => reader.DeserializeFromXml(
                        r => r.Name == "graph",
                        r => r.Name == "vertex",
                        default,
                        _ => new AdjacencyGraph<string, Edge<string>>(),
                        r => r.GetAttribute("id")!,
                        r => new Edge<string>(
                            r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                            r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));


                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    default,
                    "node",
                    "edge",
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    "",
                    "node",
                    "edge",
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    "graph",
                    default,
                    "edge",
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    "graph",
                    "",
                    "edge",
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    "graph",
                    "node",
                    default,
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "",
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));

                Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    default,
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));


                // No graph node found
                Assert.Throws<InvalidOperationException>(() => reader.DeserializeFromXml(
                    "g",    // No node named "g" for the graph
                    "node",
                    "edge",
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute("id")!,
                    r => new Edge<string>(
                        r.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"))));
            }
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Serialization/Deserialization

        #region Test Helpers

        [Pure]
        private static int DeserializeVertex(XmlReader reader)
        {
            return int.Parse(reader.GetAttribute("id", "") ?? throw new AssertionException("Unable to deserialize vertex."));
        }

        [Pure]
        private static TOutGraph SerializeDeserialize<TInEdge, TOutEdge, TInGraph, TOutGraph>(
            TInGraph graph,
            [InstantHandle] Func<XmlReader, TOutGraph> deserialize)
            where TInEdge : IEdge<int>, IEquatable<TInEdge>
            where TOutEdge : IEdge<int>, IEquatable<TOutEdge>
            where TInGraph : IEdgeListGraph<int, TInEdge>
            where TOutGraph : IEdgeListGraph<int, TOutEdge>
        {
            Assert.IsNotNull(graph);

            var settings = new XmlWriterSettings { Indent = true, IndentChars = Indent };
            using (var memory = new MemoryStream())
            using (var writer = new StreamWriter(memory))
            {
                // Serialize
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    graph.SerializeToXml(
                        xmlWriter,
                        vertex => vertex.ToString(),
                        graph.GetEdgeIdentity(),
                        "graph",
                        "node",
                        "edge",
                        "");
                }

                memory.Position = 0;

                // Deserialize
                using (XmlReader xmlReader = XmlReader.Create(memory))
                {
                    TOutGraph deserializedGraph = deserialize(xmlReader);
                    Assert.IsNotNull(deserializedGraph);
                    Assert.AreNotSame(graph, deserializedGraph);
                    return deserializedGraph;
                }
            }
        }

        [Pure]
        private static TOutGraph SerializeDeserialize<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<int, EquatableEdge<int>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<int, EquatableEdge<int>>, new()
        {
            return SerializeDeserialize<EquatableEdge<int>, EquatableEdge<int>, TInGraph, TOutGraph>(graph, reader =>
                reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new TOutGraph(),
                    DeserializeVertex,
                    nav => new EquatableEdge<int>(
                        int.Parse(nav.GetAttribute("source", "") ?? throw new AssertionException("Unable to deserialize edge source.")),
                        int.Parse(nav.GetAttribute("target", "") ?? throw new AssertionException("Unable to deserialize edge target.")))));
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_SEdge<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<int, SEquatableEdge<int>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<int, SEquatableEdge<int>>, new()
        {
            return SerializeDeserialize<SEquatableEdge<int>, SEquatableEdge<int>, TInGraph, TOutGraph>(graph, reader =>
                reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new TOutGraph(),
                    DeserializeVertex,
                    nav => new SEquatableEdge<int>(
                        int.Parse(nav.GetAttribute("source", "") ?? throw new AssertionException("Unable to deserialize edge source.")),
                        int.Parse(nav.GetAttribute("target", "") ?? throw new AssertionException("Unable to deserialize edge target.")))));
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Reversed<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<int, SReversedEdge<int, EquatableEdge<int>>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<int, EquatableEdge<int>>, new()
        {
            return SerializeDeserialize<SReversedEdge<int, EquatableEdge<int>>, EquatableEdge<int>, TInGraph, TOutGraph>(graph, reader =>
                reader.DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new TOutGraph(),
                    DeserializeVertex,
                    nav => new EquatableEdge<int>(
                        int.Parse(nav.GetAttribute("source", "") ?? throw new AssertionException("Unable to deserialize edge source.")),
                        int.Parse(nav.GetAttribute("target", "") ?? throw new AssertionException("Unable to deserialize edge target.")))));
        }

        #endregion

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationAdjacencyGraphTestCases))]
        public void XmlSerialization_AdjacencyGraph(AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph1 =
                SerializeDeserialize<AdjacencyGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph1));

            var arrayGraph = new ArrayAdjacencyGraph<int, EquatableEdge<int>>(graph);
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize<ArrayAdjacencyGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(arrayGraph);
            Assert.IsTrue(EquateGraphs.Equate(arrayGraph, deserializedGraph2));
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationAdjacencyGraphTestCases))]
        public void XmlSerialization_AdapterGraph(AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            var bidirectionalAdapterGraph = new BidirectionalAdapterGraph<int, EquatableEdge<int>>(graph);
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<BidirectionalAdapterGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(bidirectionalAdapterGraph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationClusteredAdjacencyGraphTestCases))]
        public void XmlSerialization_ClusteredGraph(ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<ClusteredAdjacencyGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationCompressedGraphTestCases))]
        public void XmlSerialization_CompressedGraph(CompressedSparseRowGraph<int> graph)
        {
            AdjacencyGraph<int, SEquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_SEdge<CompressedSparseRowGraph<int>, AdjacencyGraph<int, SEquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationBidirectionalGraphTestCases))]
        public void XmlSerialization_BidirectionalGraph(BidirectionalGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<BidirectionalGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));

            var arrayGraph = new ArrayBidirectionalGraph<int, EquatableEdge<int>>(graph);
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize<ArrayBidirectionalGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(arrayGraph);
            Assert.IsTrue(EquateGraphs.Equate(arrayGraph, deserializedGraph2));

            var reversedGraph = new ReversedBidirectionalGraph<int, EquatableEdge<int>>(graph);
            BidirectionalGraph<int, EquatableEdge<int>> deserializedGraph3 =
                SerializeDeserialize_Reversed<ReversedBidirectionalGraph<int, EquatableEdge<int>>, BidirectionalGraph<int, EquatableEdge<int>>>(reversedGraph);
            Assert.IsTrue(
                EquateGraphs.Equate(
                    graph,
                    deserializedGraph3,
                    EqualityComparer<int>.Default,
                    LambdaEqualityComparer<EquatableEdge<int>>.Create(
                        (edge1, edge2) => Equals(edge1.Source, edge2.Target) && Equals(edge1.Target, edge2.Source),
                        edge => edge.GetHashCode())));

            var undirectedBidirectionalGraph = new UndirectedBidirectionalGraph<int, EquatableEdge<int>>(graph);
            UndirectedGraph<int, EquatableEdge<int>> deserializedGraph4 =
                SerializeDeserialize<UndirectedBidirectionalGraph<int, EquatableEdge<int>>, UndirectedGraph<int, EquatableEdge<int>>>(undirectedBidirectionalGraph);
            Assert.IsTrue(EquateGraphs.Equate(undirectedBidirectionalGraph, deserializedGraph4));
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationBidirectionalMatrixGraphTestCases))]
        public void XmlSerialization_BidirectionalMatrixGraph(BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<BidirectionalMatrixGraph<EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationUndirectedGraphTestCases))]
        public void XmlSerialization_UndirectedGraph(UndirectedGraph<int, EquatableEdge<int>> graph)
        {
            UndirectedGraph<int, EquatableEdge<int>> deserializedGraph1 =
                SerializeDeserialize<UndirectedGraph<int, EquatableEdge<int>>, UndirectedGraph<int, EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph1));

            var arrayGraph = new ArrayUndirectedGraph<int, EquatableEdge<int>>(graph);
            UndirectedGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize<ArrayUndirectedGraph<int, EquatableEdge<int>>, UndirectedGraph<int, EquatableEdge<int>>>(arrayGraph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph2));
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationEdgeListGraphTestCases))]
        public void XmlSerialization_EdgeListGraph(EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<EdgeListGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.IsTrue(EquateGraphs.Equate(graph, deserializedGraph));
        }

        #endregion
    }
}
