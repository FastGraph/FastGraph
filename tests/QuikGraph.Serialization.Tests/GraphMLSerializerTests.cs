using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
#if SUPPORTS_XML_DTD_PROCESSING
using System.Linq;
using System.Xml.Schema;
#endif
using System.Xml;
using System.Xml.XPath;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Tests;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;

namespace QuikGraph.Serialization.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphMLSerializer{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal class GraphMLSerializerTests
    {
        #region Test helpers

        [Pure]
        [NotNull]
        private static TGraph VerifySerialization<TGraph>(
            [NotNull] TGraph graph,
            [NotNull, InstantHandle] Func<TGraph, string> serializeGraph,
            [NotNull, InstantHandle] Func<string, TGraph> deserializeGraph)
        {
            // Serialize the graph to XML
            string xml = serializeGraph(graph);

            // Deserialize a graph from previous serialization
            TGraph serializedGraph = deserializeGraph(xml);

            // Serialize the deserialized graph
            string newXml = serializeGraph(serializedGraph);

            // => Serialization should produce the same result
            Assert.AreEqual(xml, newXml);

            Assert.AreNotSame(graph, serializeGraph);

            return serializedGraph;
        }

        #endregion

        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void GraphMLSerialization_HeaderCheck(bool emitDeclarationOnSerialize, bool emitDeclarationOnDeserialize)
        {
            var graph = new EquatableTestGraph
            {
                String = "graph",
                Int = 42
            };

            var vertex1 = new EquatableTestVertex("v1")
            {
                StringDefault = "foo",
                String = "string",
                Int = 10,
                Long = 20,
                Float = 25.0F,
                Double = 30.0,
                Bool = true
            };

            var vertex2 = new EquatableTestVertex("v2")
            {
                StringDefault = "bar",
                String = "string2",
                Int = 110,
                Long = 120,
                Float = 125.0F,
                Double = 130.0,
                Bool = true
            };

            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);

            var edge1 = new EquatableTestEdge(vertex1, vertex2, "e_1")
            {
                String = "edge",
                Int = 90,
                Long = 100,
                Float = 25.0F,
                Double = 110.0,
                Bool = true
            };

            graph.AddEdge(edge1);

            EquatableTestGraph serializedGraph = VerifySerialization(
                graph,
                g =>
                {
                    using (var writer = new StringWriter())
                    {
                        var settings = new XmlWriterSettings { Indent = true };
                        using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                        {
                            var serializer = new GraphMLSerializer<EquatableTestVertex, EquatableTestEdge, EquatableTestGraph>
                            {
                                EmitDocumentDeclaration = emitDeclarationOnDeserialize
                            };

                            serializer.Serialize(
                                xmlWriter,
                                g,
                                vertex => vertex.ID,
                                edge => edge.ID);
                        }

                        return writer.ToString();
                    }
                },
                xml =>
                {
                    using (var reader = new StringReader(xml))
                    {
                        var serializer = new GraphMLDeserializer<EquatableTestVertex, EquatableTestEdge, EquatableTestGraph>
                        {
                            EmitDocumentDeclaration = emitDeclarationOnDeserialize
                        };

#if SUPPORTS_XML_DTD_PROCESSING
                        var settings = new XmlReaderSettings
                        {
                            ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings,
                            XmlResolver = new GraphMLXmlResolver(),
                            DtdProcessing = DtdProcessing.Ignore
                        };

                        using (XmlReader xmlReader = XmlReader.Create(reader, settings))
                        {
#else
                        var xmlReader = new XmlTextReader(reader);
                        {
                            xmlReader.ProhibitDtd = false;
                            xmlReader.XmlResolver = null;
#endif
                            var g = new EquatableTestGraph();
                            serializer.Deserialize(
                                xmlReader,
                                g,
                                id => new EquatableTestVertex(id),
                                (source, target, id) => new EquatableTestEdge(source, target, id));
                            return g;
                        }
                    }
                });

            Assert.IsTrue(
                EquateGraphs.Equate(
                    graph,
                    serializedGraph));
        }

        #region Serialization

        [NotNull]
        private static readonly string WriteThrowsTestFilePath =
            Path.Combine(GetTemporaryTestDirectory(), "serialization_to_graphml_throws_test.graphml");

        [Test]
        public void SerializeToGraphML_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            // Filepath
            Assert.Throws<ArgumentNullException>(
                () => ((AdjacencyGraph<TestVertex, TestEdge>)null).SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(WriteThrowsTestFilePath));

            var graph = new AdjacencyGraph<TestVertex, TestEdge>();
            Assert.Throws<ArgumentException>(
                () =>  graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>((string)null));
            Assert.Throws<ArgumentException>(
                () => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(""));

            Assert.Throws<ArgumentNullException>(
                () => ((AdjacencyGraph<TestVertex, TestEdge>)null).SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    WriteThrowsTestFilePath,
                    vertex => vertex.ID,
                    edge => edge.ID));

            Assert.Throws<ArgumentException>(
                () => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    (string)null,
                    vertex => vertex.ID,
                    edge => edge.ID));
            Assert.Throws<ArgumentException>(
                () => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    "",
                    vertex => vertex.ID,
                    edge => edge.ID));

            Assert.Throws<ArgumentNullException>(
                () => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    WriteThrowsTestFilePath,
                    null,
                    edge => edge.ID));

            Assert.Throws<ArgumentNullException>(
                () => ((AdjacencyGraph<TestVertex, TestEdge>)null).SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    WriteThrowsTestFilePath,
                    vertex => vertex.ID,
                    null));

            // XML writer
            Assert.Throws<ArgumentNullException>(
                () => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>((XmlWriter)null));
            Assert.Throws<ArgumentNullException>(
                () => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    (XmlWriter)null,
                    vertex => vertex.ID,
                    edge => edge.ID));

            using (XmlWriter writer = XmlWriter.Create(WriteThrowsTestFilePath))
            {
                Assert.Throws<ArgumentNullException>(
                    () => ((AdjacencyGraph<TestVertex, TestEdge>) null).SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(writer));

                Assert.Throws<ArgumentNullException>(
                    () => ((AdjacencyGraph<TestVertex, TestEdge>) null).SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                        writer,
                        vertex => vertex.ID,
                        edge => edge.ID));

                Assert.Throws<ArgumentNullException>(
                    () => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                        writer,
                        null,
                        edge => edge.ID));

                Assert.Throws<ArgumentNullException>(
                    () => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                        writer,
                        vertex => vertex.ID,
                        null));
            }
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void SerializeToGraphML_Throws_InvalidData()
        {
            AssertSerializationFail<TestGraphArrayDefaultValue, NotImplementedException>(new TestGraphArrayDefaultValue());
            AssertSerializationFail<TestGraphNoGetter, TypeInitializationException>(new TestGraphNoGetter());
            AssertSerializationFail<TestGraphNullDefaultValue, TypeInitializationException>(new TestGraphNullDefaultValue());
            AssertSerializationFail<TestGraphWrongDefaultValue, TypeInitializationException>(new TestGraphWrongDefaultValue());
            AssertSerializationFail<TestGraphNotSupportedType, TypeInitializationException>(new TestGraphNotSupportedType());

            AssertSerializationFail<TestGraphObjectDefaultValue, NotSupportedException>(new TestGraphObjectDefaultValue());
            AssertSerializationFail<TestGraphNotSupportedType2, NotSupportedException>(new TestGraphNotSupportedType2());
            AssertSerializationFail<TestGraphNotSupportedType3, NotSupportedException>(new TestGraphNotSupportedType3());
            AssertSerializationFail<TestGraphWrongDefaultValue2, NotSupportedException>(new TestGraphWrongDefaultValue2());

            #region Local function

            void AssertSerializationFail<TGraph, TException>(TGraph g)
                where TGraph : IMutableVertexAndEdgeListGraph<TestVertex, TestEdge>
                where TException : Exception
            {
                Assert.Throws<TException>(
                    () => g.SerializeToGraphML<TestVertex, TestEdge, TGraph>(
                        WriteThrowsTestFilePath,
                        vertex => vertex.ID,
                        edge => edge.ID));
            }

            #endregion
        }

        #endregion

        #region Deserialization

        [NotNull]
        private const string TestGraphFileName = "DCT8.graphml";

        [NotNull]
        private const string MissingAttributeTestGraphFileName = "DCT8_with_missing_attribute.graphml";

        [NotNull]
        private const string MissingSourceTestGraphFileName = "DCT8_with_missing_source_id.graphml";

        [NotNull]
        private const string MissingTargetTestGraphFileName = "DCT8_with_missing_target_id.graphml";

        [NotNull]
        private const string InvalidTagTestGraphFileName = "DCT8_invalid_tag.graphml";

        [NotNull]
        private const string MissingGraphMLTestGraphFileName = "DCT8_missing_graphml_tag.graphml";

        [NotNull]
        private const string MissingGraphTestGraphFileName = "DCT8_missing_graph_tag.graphml";

        [Test]
        public void DeserializeFromGraphML()
        {
            foreach (string graphMLFilePath in TestGraphFactory.GetGraphMLFilePaths())
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
#if SUPPORTS_XML_DTD_PROCESSING
                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Ignore,
                    XmlResolver = new GraphMLXmlResolver(),
                    ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings
                };

                using (XmlReader reader = XmlReader.Create(graphMLFilePath, settings))
                {
#else
                using (var reader = new XmlTextReader(graphMLFilePath))
                {
                    reader.ProhibitDtd = false;
                    reader.XmlResolver = null;
#endif
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

        [Test]
        public void DeserializeFromGraphML_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            // Filepath
            Assert.Throws<ArgumentNullException>(
                () => ((AdjacencyGraph<string, Edge<string>>) null).DeserializeFromGraphML(
                    GetGraphFilePath(TestGraphFileName),
                    id => id,
                    (source, target, id) => new Edge<string>(source, target)));

            var graph = new AdjacencyGraph<string, Edge<string>>();
            Assert.Throws<ArgumentException>(
                () => graph.DeserializeFromGraphML(
                    (string)null,
                    id => id,
                    (source, target, id) => new Edge<string>(source, target)));

            Assert.Throws<ArgumentException>(
                () => graph.DeserializeFromGraphML(
                    "",
                    id => id,
                    (source, target, id) => new Edge<string>(source, target)));

            Assert.Throws<ArgumentNullException>(
                () => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    GetGraphFilePath(TestGraphFileName),
                    null,
                    (source, target, id) => new Edge<string>(source, target)));

            Assert.Throws<ArgumentNullException>(
                () => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    GetGraphFilePath(TestGraphFileName),
                    id => id,
                    null));

            // Text reader
            Assert.Throws<ArgumentNullException>(
                () => graph.DeserializeFromGraphML(
                    (TextReader)null,
                    id => id,
                    (source, target, id) => new Edge<string>(source, target)));

            using (var reader = new StreamReader(GetGraphFilePath(TestGraphFileName)))
            {
                Assert.Throws<ArgumentNullException>(
                    () => ((AdjacencyGraph<string, Edge<string>>)null).DeserializeFromGraphML(
                        reader,
                        id => id,
                        (source, target, id) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(
                    () => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        null,
                        (source, target, id) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(
                    () => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        id => id,
                        null));
            }

            // XML reader
            Assert.Throws<ArgumentNullException>(
                () => graph.DeserializeFromGraphML(
                    (XmlReader)null,
                    id => id,
                    (source, target, id) => new Edge<string>(source, target)));

            using (XmlReader reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                Assert.Throws<ArgumentNullException>(
                    () => ((AdjacencyGraph<string, Edge<string>>) null).DeserializeFromGraphML(
                        reader,
                        id => id,
                        (source, target, id) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(
                    () => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        null,
                        (source, target, id) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(
                    () => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        id => id,
                        null));
            }
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void DeserializeFromGraphML_Throws_InvalidData()
        {
            AssertDeserializationFail<TestGraphArrayDefaultValue, TypeInitializationException>(new TestGraphArrayDefaultValue());
            AssertDeserializationFail<TestGraphNoSetter, TypeInitializationException>(new TestGraphNoSetter());
            AssertDeserializationFail<TestGraphNullDefaultValue, TypeInitializationException>(new TestGraphNullDefaultValue());
            AssertDeserializationFail<TestGraphWrongDefaultValue, TypeInitializationException>(new TestGraphWrongDefaultValue());
            AssertDeserializationFail<TestGraphNotSupportedType, TypeInitializationException>(new TestGraphNotSupportedType());

            var graph = new TestGraph();
            AssertDeserializationFail<TestGraph, ArgumentException>(graph, MissingAttributeTestGraphFileName);
            AssertDeserializationFail<TestGraph, ArgumentException>(graph, MissingSourceTestGraphFileName);
            AssertDeserializationFail<TestGraph, ArgumentException>(graph, MissingTargetTestGraphFileName);
            AssertDeserializationFail<TestGraph, InvalidOperationException>(graph, InvalidTagTestGraphFileName);
            AssertDeserializationFail<TestGraph, InvalidOperationException>(graph, MissingGraphTestGraphFileName);
            AssertDeserializationFail<TestGraph, InvalidOperationException>(graph, MissingGraphMLTestGraphFileName);

            #region Local function

            void AssertDeserializationFail<TGraph, TException>(TGraph g, string fileName = TestGraphFileName)
                where TGraph : IMutableVertexAndEdgeListGraph<TestVertex, TestEdge>
                where TException : Exception
            {
                Assert.Throws<TException>(
                    () => g.DeserializeFromGraphML(
                        GetGraphFilePath(fileName),
                        id => new TestVertex(id),
                        (source, target, id) => new TestEdge(source, target, id)));
            }

            #endregion
        }

        #region Test class

        private class TestClass
        {
            [UsedImplicitly]
            public char Char { get; }
        }

        #endregion

        [Test]
        public void EmitValue_Throws()
        {
            var method = new DynamicMethod("TestMethod", typeof(void), Type.EmptyTypes);
            ILGenerator generator = method.GetILGenerator();
            Assert.Throws<NotSupportedException>(
                () => ILHelpers.EmitValue(
                    generator,
                    typeof(TestClass).GetProperty(nameof(TestClass.Char)) ?? throw new AssertionException("Property must exist."),
                    'a'));
        }

#if SUPPORTS_XML_DTD_PROCESSING
        #region Test data

        [NotNull]
        private static readonly bool[] Bools = { true, false, true, true };

        [NotNull]
        private static readonly int[] Ints = { 2, 3, 45, 3, 44, -2, 3, 5, 99999999 };

        [NotNull]
        private static readonly long[] Longs = { 3, 4, 43, 999999999999999999L, 445, 55, 3, 98, 49789238740598170, 987459, 97239, 234245, 0, -2232 };

        [NotNull]
        private static readonly float[] Floats = { 3.14159265F, 1.1F, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345F };

        [NotNull]
        private static readonly double[] Doubles = { 3.14159265, 1.1, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345 };

        [NotNull, ItemNotNull]
        private static readonly string[] Strings = { "", "Quick", "", "brown", "fox", "jumps", "over", "the", "lazy", "dog", ".", "" };


        [NotNull]
        private static readonly IList<bool> BoolsList = new[] { true, false, true, true };

        [NotNull]
        private static readonly IList<int> IntsList = new[] { 2, 3, 45, 3, 44, -2, 3, 5, 99999999 };

        [NotNull]
        private static readonly IList<long> LongsList = new[] { 3, 4, 43, 999999999999999999L, 445, 55, 3, 98, 49789238740598170, 987459, 97239, 234245, 0, -2232 };

        [NotNull]
        private static readonly IList<float> FloatsList = new[] { 3.14159265F, 1.1F, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345F };

        [NotNull]
        private static readonly IList<double> DoublesList = new[] { 3.14159265, 1.1, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345 };

        [NotNull, ItemNotNull]
        private static readonly IList<string> StringsList = new[] { "", "Quick", "", "brown", "fox", "jumps", "over", "the", "lazy", "dog", ".", "" };

        #endregion

        #region Test helpers

        [Pure]
        [NotNull]
        private static string SerializeGraph1([NotNull] TestGraph graph)
        {
            string filePath = Path.Combine(
                GetTemporaryTestDirectory(),
                $"serialization_to_graphml_test_{Guid.NewGuid().ToString()}.graphml");

            graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(filePath);
            Assert.IsTrue(File.Exists(filePath));
            return File.ReadAllText(filePath);
        }

        [Pure]
        [NotNull]
        private static string SerializeGraph2([NotNull] TestGraph graph)
        {
            string filePath = Path.Combine(
                GetTemporaryTestDirectory(),
                $"serialization_to_graphml_test_{Guid.NewGuid().ToString()}.graphml");

            graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(
                filePath,
                vertex => vertex.ID,
                edge => edge.ID);
            Assert.IsTrue(File.Exists(filePath));
            return File.ReadAllText(filePath);
        }

        [Pure]
        [NotNull]
        private static string SerializeGraph3([NotNull] TestGraph graph)
        {
            using (var writer = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(xmlWriter);
                }

                return writer.ToString();
            }
        }

        [Pure]
        [NotNull]
        private static string SerializeGraph4([NotNull] TestGraph graph)
        {
            using (var writer = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(
                        xmlWriter,
                        vertex => vertex.ID,
                        edge => edge.ID);
                }

                return writer.ToString();
            }
        }

        [Pure]
        [NotNull]
        private static TestGraph DeserializeGraph([NotNull] string xml)
        {
            using (var reader = new StringReader(xml))
            {
                var serializedGraph = new TestGraph();
                serializedGraph.DeserializeAndValidateFromGraphML(
                    reader,
                    id => new TestVertex(id),
                    (source, target, id) => new TestEdge(source, target, id));
                return serializedGraph;
            }
        }

        [Pure]
        [NotNull]
        private static TestGraph VerifySerialization(
            [NotNull] TestGraph graph,
            [NotNull, InstantHandle] Func<TestGraph, string> serializeGraph)
        {
            return VerifySerialization(graph, serializeGraph, DeserializeGraph);
        }

        #endregion

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> GraphSerializationTestCases
        {
            [UsedImplicitly]
            get
            {
                // Second parameter is used to know if serialization
                // will keep objects Ids or use other ones

                Func<TestGraph, string> serialize1 = SerializeGraph1;
                yield return new TestCaseData(serialize1, false);

                Func<TestGraph, string> serialize2 = SerializeGraph2;
                yield return new TestCaseData(serialize2, true);

                Func<TestGraph, string> serialize3 = SerializeGraph3;
                yield return new TestCaseData(serialize3, false);

                Func<TestGraph, string> serialize4 = SerializeGraph4;
                yield return new TestCaseData(serialize4, true);
            }
        }

        [TestCaseSource(nameof(GraphSerializationTestCases))]
        public void GraphMLSerializationWithValidation_WriteVertex(
            [NotNull, InstantHandle] Func<TestGraph, string> serializeGraph,
            bool _)
        {
            var graph = new TestGraph
            {
                Bool = true,
                Double = 1.0,
                Float = 2.0F,
                Int = 10,
                Long = 100,
                String = "foo",
                BoolArray = Bools,
                IntArray = Ints,
                LongArray = Longs,
                FloatArray = Floats,
                DoubleArray = Doubles,
                StringArray = Strings,
                NullArray = null,
                BoolIList = BoolsList,
                IntIList = IntsList,
                LongIList = LongsList,
                FloatIList = FloatsList,
                DoubleIList = DoublesList,
                StringIList = StringsList
            };

            var vertex = new TestVertex("v1")
            {
                StringDefault = "bar",
                String = "string",
                Int = 10,
                Long = 20,
                Float = 25.0F,
                Double = 30.0,
                Bool = true,
                IntArray = new[] { 1, 2, 3, 4 },
                IntIList = new[] { 4, 5, 6, 7 }
            };

            graph.AddVertex(vertex);

            TestGraph serializedGraph = VerifySerialization(graph, serializeGraph);

            Assert.AreEqual(graph.Bool, serializedGraph.Bool);
            Assert.AreEqual(graph.Double, serializedGraph.Double);
            Assert.AreEqual(graph.Float, serializedGraph.Float);
            Assert.AreEqual(graph.Int, serializedGraph.Int);
            Assert.AreEqual(graph.Long, serializedGraph.Long);
            Assert.AreEqual(graph.String, serializedGraph.String);
            CollectionAssert.AreEqual(graph.BoolArray, serializedGraph.BoolArray);
            CollectionAssert.AreEqual(graph.IntArray, serializedGraph.IntArray);
            CollectionAssert.AreEqual(graph.LongArray, serializedGraph.LongArray);
            CollectionAssert.AreEqual(graph.StringArray, serializedGraph.StringArray);
            CollectionAssert.AreEqual(graph.FloatArray, serializedGraph.FloatArray, new FloatComparer(0.001F));
            CollectionAssert.AreEqual(graph.DoubleArray, serializedGraph.DoubleArray, new DoubleComparer(0.0001));
            CollectionAssert.AreEqual(graph.BoolIList, serializedGraph.BoolIList);
            CollectionAssert.AreEqual(graph.IntIList, serializedGraph.IntIList);
            CollectionAssert.AreEqual(graph.LongIList, serializedGraph.LongIList);
            CollectionAssert.AreEqual(graph.StringIList, serializedGraph.StringIList);
            CollectionAssert.AreEqual(graph.FloatIList, serializedGraph.FloatIList, new FloatComparer(0.001F));
            CollectionAssert.AreEqual(graph.DoubleIList, serializedGraph.DoubleIList, new DoubleComparer(0.0001));

            TestVertex serializedVertex = serializedGraph.Vertices.First();
            Assert.AreEqual("bar", serializedVertex.StringDefault);
            Assert.AreEqual(vertex.String, serializedVertex.String);
            Assert.AreEqual(vertex.Int, serializedVertex.Int);
            Assert.AreEqual(vertex.Long, serializedVertex.Long);
            Assert.AreEqual(vertex.Float, serializedVertex.Float);
            Assert.AreEqual(vertex.Double, serializedVertex.Double);
            Assert.AreEqual(vertex.Bool, serializedVertex.Bool);
            CollectionAssert.AreEqual(vertex.IntArray, serializedVertex.IntArray);
            CollectionAssert.AreEqual(vertex.IntIList, serializedVertex.IntIList);
        }

        [TestCaseSource(nameof(GraphSerializationTestCases))]
        public void GraphMLSerializationWithValidation_WriteEdge(
            [NotNull, InstantHandle] Func<TestGraph, string> serializeGraph,
            bool keepIds)
        {
            var graph = new TestGraph
            {
                Bool = true,
                Double = 1.0,
                Float = 2.0F,
                Int = 10,
                Long = 100,
                String = "foo"
            };

            var vertex1 = new TestVertex("v1")
            {
                StringDefault = "foo",
                String = "string",
                Int = 10,
                Long = 20,
                Float = 25.0F,
                Double = 30.0,
                Bool = true
            };

            var vertex2 = new TestVertex("v2")
            {
                StringDefault = "bar",
                String = "string2",
                Int = 110,
                Long = 120,
                Float = 125.0F,
                Double = 130.0,
                Bool = true
            };

            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);

            var edge = new TestEdge(
                vertex1, vertex2,
                "e1",
                "edge",
                90,
                100,
                25.0F,
                110.0,
                true);

            graph.AddEdge(edge);

            TestGraph serializedGraph = VerifySerialization(graph, serializeGraph);

            TestEdge serializedEdge = serializedGraph.Edges.First();
            if (keepIds)
                Assert.AreEqual(edge.ID, serializedEdge.ID);
            Assert.AreEqual(edge.String, serializedEdge.String);
            Assert.AreEqual(edge.Int, serializedEdge.Int);
            Assert.AreEqual(edge.Long, serializedEdge.Long);
            Assert.AreEqual(edge.Float, serializedEdge.Float);
            Assert.AreEqual(edge.Double, serializedEdge.Double);
            Assert.AreEqual(edge.Bool, serializedEdge.Bool);
        }

        [Test]
        public void DeserializeAndValidateFromGraphML_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new AdjacencyGraph<string, Edge<string>>();

            // Text reader
            Assert.Throws<ArgumentNullException>(
                () => graph.DeserializeAndValidateFromGraphML(
                    null,
                    id => id,
                    (source, target, id) => new Edge<string>(source, target)));

            using (var reader = new StreamReader(GetGraphFilePath(TestGraphFileName)))
            {
                Assert.Throws<ArgumentNullException>(
                    () => ((AdjacencyGraph<string, Edge<string>>)null).DeserializeAndValidateFromGraphML(
                        reader,
                        id => id,
                        (source, target, id) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(
                    () => graph.DeserializeAndValidateFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        null,
                        (source, target, id) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(
                    () => graph.DeserializeAndValidateFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        id => id,
                        null));
            }
            // ReSharper restore AssignNullToNotNullAttribute
        }
#endif

        #endregion
    }
}