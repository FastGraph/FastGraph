#nullable enable

using System.Reflection.Emit;
#if SUPPORTS_XML_DTD_PROCESSING
using System.Xml.Schema;
#endif
using System.Xml;
using System.Xml.XPath;
using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Tests;
using static FastGraph.Serialization.Tests.SerializationTestCaseSources;
using static FastGraph.Tests.FastGraphUnitTestsHelpers;

namespace FastGraph.Serialization.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphMLSerializer{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphMLSerializerTests
    {
        #region Test helpers

        [Pure]
        private static TGraph VerifySerialization<TGraph>(
            TGraph graph,
            [InstantHandle] Func<TGraph, string> serializeGraph,
            [InstantHandle] Func<string, TGraph> deserializeGraph)
            where TGraph : notnull
        {
            // Serialize the graph to GraphML
            string graphml = serializeGraph(graph);

            // Deserialize a graph from previous serialization
            TGraph serializedGraph = deserializeGraph(graphml);

            // Serialize the deserialized graph
            string newGraphml = serializeGraph(serializedGraph);

            // => Serialization should produce the same result
            newGraphml.Should().Be(graphml);

            serializeGraph.Should().NotBeSameAs(graph);

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
                        using (var xmlWriter = XmlWriter.Create(writer, settings))
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
                graphml =>
                {
                    using (var reader = new StringReader(graphml))
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

                        using (var xmlReader = XmlReader.Create(reader, settings))
                        {
#else
                        var xmlReader = new XmlTextReader(reader);
                        {
                            xmlReader.ProhibitDtd = false;
                            xmlReader.XmlResolver = default;
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

            EquateGraphs.Equate(
                graph,
                serializedGraph).Should().BeTrue();
        }

        #region Serialization

        private static readonly string WriteThrowsTestFilePath =
            Path.Combine(GetTemporaryTestDirectory(), "serialization_to_graphml_throws_test.graphml");

        [Test]
        public void SerializeToGraphML_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            // Filepath
            Invoking(() => ((AdjacencyGraph<TestVertex, TestEdge>?)default).SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(WriteThrowsTestFilePath)).Should().Throw<ArgumentNullException>();

            var graph = new AdjacencyGraph<TestVertex, TestEdge>();
            Invoking(() => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>((string?)default)).Should().Throw<ArgumentException>();
            Invoking(() => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>("")).Should().Throw<ArgumentException>();

            Invoking(() => ((AdjacencyGraph<TestVertex, TestEdge>?)default).SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                WriteThrowsTestFilePath,
                vertex => vertex.ID,
                edge => edge.ID)).Should().Throw<ArgumentNullException>();

            Invoking(() => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                (string?)default,
                vertex => vertex.ID,
                edge => edge.ID)).Should().Throw<ArgumentException>();
            Invoking(() => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                "",
                vertex => vertex.ID,
                edge => edge.ID)).Should().Throw<ArgumentException>();

            Invoking(() => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                WriteThrowsTestFilePath,
                default,
                edge => edge.ID)).Should().Throw<ArgumentNullException>();

            Invoking(() => ((AdjacencyGraph<TestVertex, TestEdge>?)default).SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                WriteThrowsTestFilePath,
                vertex => vertex.ID,
                default)).Should().Throw<ArgumentNullException>();

            // XML writer
            Invoking(() => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>((XmlWriter?)default)).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                (XmlWriter?)default,
                vertex => vertex.ID,
                edge => edge.ID)).Should().Throw<ArgumentNullException>();

            using (var writer = XmlWriter.Create(WriteThrowsTestFilePath))
            {
                Invoking(() => ((AdjacencyGraph<TestVertex, TestEdge>?)default).SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(writer)).Should().Throw<ArgumentNullException>();

                Invoking(() => ((AdjacencyGraph<TestVertex, TestEdge>?)default).SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    writer,
                    vertex => vertex.ID,
                    edge => edge.ID)).Should().Throw<ArgumentNullException>();

                Invoking(() => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    writer,
                    default,
                    edge => edge.ID)).Should().Throw<ArgumentNullException>();

                Invoking(() => graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    writer,
                    vertex => vertex.ID,
                    default)).Should().Throw<ArgumentNullException>();
            }
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void SerializeToGraphML_Throws_InvalidData()
        {
            AssertSerializationFail<TestGraphArrayDefaultValue, NotSupportedException>(new TestGraphArrayDefaultValue());
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
                Invoking(() => g.SerializeToGraphML<TestVertex, TestEdge, TGraph>(
                    WriteThrowsTestFilePath,
                    vertex => vertex.ID,
                    edge => edge.ID)).Should().Throw<TException>();
            }

            #endregion
        }

        #endregion

        #region Deserialization

        private static readonly NamedTestGraphSource TestGraph = TestGraphSourceProvider.Instance.DCT8;

        private static readonly NamedTestGraphSource MissingAttributeTestGraph = TestGraphSourceProvider.Instance.DCT8_with_missing_attribute;

        private static readonly NamedTestGraphSource MissingSourceTestGraph = TestGraphSourceProvider.Instance.DCT8_with_missing_source_id;

        private static readonly NamedTestGraphSource MissingTargetTestGraph = TestGraphSourceProvider.Instance.DCT8_with_missing_target_id;

        private static readonly NamedTestGraphSource InvalidTagTestGraph = TestGraphSourceProvider.Instance.DCT8_invalid_tag;

        private static readonly NamedTestGraphSource MissingGraphMLTestGraph = TestGraphSourceProvider.Instance.DCT8_missing_graphml_tag;

        private static readonly NamedTestGraphSource MissingGraphTestGraph = TestGraphSourceProvider.Instance.DCT8_missing_graph_tag;

        [TestCaseSource(nameof(GetGraphMLFiles_All))]
        public void DeserializeFromGraphML(NamedTestGraphSource namedTestGraphSource)
        {
            var graph = new AdjacencyGraph<string, Edge<string>>();
            using (var reader = new StringReader(namedTestGraphSource.SourceContent))
            {
                graph.DeserializeFromGraphML(
                    reader,
                    id => id,
                    (source, target, _) => new Edge<string>(source, target));
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

            using (var reader = XmlReader.Create(new StringReader(namedTestGraphSource.SourceContent), settings))
            {
#else
            using (var reader = new XmlTextReader(graphmlFileInfo.FullName))
            {
                reader.ProhibitDtd = false;
                reader.XmlResolver = default;
#endif
                var document = new XPathDocument(reader);

                foreach (XPathNavigator? node in document.CreateNavigator()!.Select("/graphml/graph/node"))
                {
                    string id = node!.GetAttribute("id", "");
                    vertices.ContainsKey(id).Should().BeTrue();
                }

                // Check all edges are loaded
                foreach (XPathNavigator? node in document.CreateNavigator()!.Select("/graphml/graph/edge"))
                {
                    string source = node!.GetAttribute("source", "");
                    string target = node.GetAttribute("target", "");
                    graph.ContainsEdge(vertices[source], vertices[target]).Should().BeTrue();
                }
            }
        }

        [Test]
        public void DeserializeFromGraphML_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8631
            // Filepath
            Invoking(() => ((AdjacencyGraph<string, Edge<string>>?)default).DeserializeFromGraphML(
                TestGraph.SourcePath!,
                id => id,
                (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentNullException>();

            var graph = new AdjacencyGraph<string, Edge<string>>();
            Invoking(() => graph.DeserializeFromGraphML(
                (string?)default,
                id => id,
                (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentException>();

            Invoking(() => graph.DeserializeFromGraphML(
                "",
                id => id,
                (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentException>();

            Invoking(() => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                TestGraph.SourcePath!,
                default,
                (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentNullException>();

            Invoking(() => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                TestGraph.SourcePath!,
                id => id,
                default)).Should().Throw<ArgumentNullException>();

            // Text reader
            Invoking(() => graph.DeserializeFromGraphML(
                (TextReader?)default,
                id => id,
                (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentNullException>();

            using (var reader = new StreamReader(TestGraph.SourcePath!))
            {
                Invoking(() => ((AdjacencyGraph<string, Edge<string>>?)default).DeserializeFromGraphML(
                    reader,
                    id => id,
                    (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentNullException>();

                Invoking(() => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    reader,
                    default,
                    (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentNullException>();

                Invoking(() => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    reader,
                    id => id,
                    default)).Should().Throw<ArgumentNullException>();
            }

            // XML reader
            Invoking(() => graph.DeserializeFromGraphML(
                (XmlReader?)default,
                id => id,
                (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentNullException>();

            using (var reader = XmlReader.Create(TestGraph.SourcePath!))
            {
                Invoking(() => ((AdjacencyGraph<string, Edge<string>>?)default).DeserializeFromGraphML(
                    reader,
                    id => id,
                    (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentNullException>();

                Invoking(() => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    reader,
                    default,
                    (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentNullException>();

                Invoking(() => graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    reader,
                    id => id,
                    default)).Should().Throw<ArgumentNullException>();
            }
#pragma warning restore CS8631
#pragma warning restore CS8625
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
            AssertDeserializationFail<TestGraph, ArgumentException>(graph, MissingAttributeTestGraph.SourcePath!);
            AssertDeserializationFail<TestGraph, ArgumentException>(graph, MissingSourceTestGraph.SourcePath!);
            AssertDeserializationFail<TestGraph, ArgumentException>(graph, MissingTargetTestGraph.SourcePath!);
            AssertDeserializationFail<TestGraph, InvalidOperationException>(graph, InvalidTagTestGraph.SourcePath!);
            AssertDeserializationFail<TestGraph, InvalidOperationException>(graph, MissingGraphTestGraph.SourcePath!);
            AssertDeserializationFail<TestGraph, InvalidOperationException>(graph, MissingGraphMLTestGraph.SourcePath!);

            #region Local function

            void AssertDeserializationFail<TGraph, TException>(TGraph g, string? fileName = null)
                where TGraph : IMutableVertexAndEdgeListGraph<TestVertex, TestEdge>
                where TException : Exception
            {
                fileName ??= TestGraph.SourcePath!;
                Invoking(() => g.DeserializeFromGraphML(
                    fileName,
                    id => new TestVertex(id),
                    (source, target, id) => new TestEdge(source, target, id))).Should().Throw<TException>();
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
            Invoking(() => ILHelpers.EmitValue(
                generator,
                typeof(TestClass).GetProperty(nameof(TestClass.Char)) ?? throw new AssertionException("Property must exist."),
                'a')).Should().Throw<NotSupportedException>();
        }

#if SUPPORTS_XML_DTD_PROCESSING
        #region Test data

        private static readonly bool[] Bools = { true, false, true, true };

        private static readonly int[] Ints = { 2, 3, 45, 3, 44, -2, 3, 5, 99999999 };

        private static readonly long[] Longs = { 3, 4, 43, 999999999999999999L, 445, 55, 3, 98, 49789238740598170, 987459, 97239, 234245, 0, -2232 };

        private static readonly float[] Floats = { 3.14159265F, 1.1F, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345F };

        private static readonly double[] Doubles = { 3.14159265, 1.1, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345 };

        private static readonly string[] Strings = { "", "Quick", "", "brown", "fox", "jumps", "over", "the", "lazy", "dog", ".", "" };


        private static readonly IList<bool> BoolsList = new[] { true, false, true, true };

        private static readonly IList<int> IntsList = new[] { 2, 3, 45, 3, 44, -2, 3, 5, 99999999 };

        private static readonly IList<long> LongsList = new[] { 3, 4, 43, 999999999999999999L, 445, 55, 3, 98, 49789238740598170, 987459, 97239, 234245, 0, -2232 };

        private static readonly IList<float> FloatsList = new[] { 3.14159265F, 1.1F, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345F };

        private static readonly IList<double> DoublesList = new[] { 3.14159265, 1.1, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345 };

        private static readonly IList<string> StringsList = new[] { "", "Quick", "", "brown", "fox", "jumps", "over", "the", "lazy", "dog", ".", "" };

        #endregion

        #region Test helpers

        [Pure]
        private static string SerializeGraph1(TestGraph graph)
        {
            string filePath = Path.Combine(
                GetTemporaryTestDirectory(),
                $"serialization_to_graphml_test_{Guid.NewGuid().ToString()}.graphml");

            graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(filePath);
            File.Exists(filePath).Should().BeTrue();
            return File.ReadAllText(filePath);
        }

        [Pure]
        private static string SerializeGraph2(TestGraph graph)
        {
            string filePath = Path.Combine(
                GetTemporaryTestDirectory(),
                $"serialization_to_graphml_test_{Guid.NewGuid().ToString()}.graphml");

            graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(
                filePath,
                vertex => vertex.ID,
                edge => edge.ID);
            File.Exists(filePath).Should().BeTrue();
            return File.ReadAllText(filePath);
        }

        [Pure]
        private static string SerializeGraph3(TestGraph graph)
        {
            using (var writer = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (var xmlWriter = XmlWriter.Create(writer, settings))
                {
                    graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(xmlWriter);
                }

                return writer.ToString();
            }
        }

        [Pure]
        private static string SerializeGraph4(TestGraph graph)
        {
            using (var writer = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (var xmlWriter = XmlWriter.Create(writer, settings))
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
        private static TestGraph DeserializeGraph(string graphml)
        {
            using (var reader = new StringReader(graphml))
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
        private static TestGraph VerifySerialization(
            TestGraph graph,
            [InstantHandle] Func<TestGraph, string> serializeGraph)
        {
            return VerifySerialization(graph, serializeGraph, DeserializeGraph);
        }

        #endregion

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
            [InstantHandle] Func<TestGraph, string> serializeGraph,
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
                NullArray = default,
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

            serializedGraph.Bool.Should().Be(graph.Bool);
            serializedGraph.Double.Should().Be(graph.Double);
            serializedGraph.Float.Should().Be(graph.Float);
            serializedGraph.Int.Should().Be(graph.Int);
            serializedGraph.Long.Should().Be(graph.Long);
            serializedGraph.String.Should().Be(graph.String);
            serializedGraph.BoolArray.Should().BeEquivalentTo(graph.BoolArray);
            serializedGraph.IntArray.Should().BeEquivalentTo(graph.IntArray);
            serializedGraph.LongArray.Should().BeEquivalentTo(graph.LongArray);
            serializedGraph.StringArray.Should().BeEquivalentTo(graph.StringArray);
            serializedGraph.FloatArray.Should().BeEquivalentTo(graph.FloatArray);
            serializedGraph.DoubleArray.Should().BeEquivalentTo(graph.DoubleArray);
            serializedGraph.BoolIList.Should().BeEquivalentTo(graph.BoolIList);
            serializedGraph.IntIList.Should().BeEquivalentTo(graph.IntIList);
            serializedGraph.LongIList.Should().BeEquivalentTo(graph.LongIList);
            serializedGraph.StringIList.Should().BeEquivalentTo(graph.StringIList);
            serializedGraph.FloatIList.Should().BeEquivalentTo(graph.FloatIList);
            serializedGraph.DoubleIList.Should().BeEquivalentTo(graph.DoubleIList);

            TestVertex serializedVertex = serializedGraph.Vertices.First();
            serializedVertex.StringDefault.Should().Be("bar");
            serializedVertex.String.Should().Be(vertex.String);
            serializedVertex.Int.Should().Be(vertex.Int);
            serializedVertex.Long.Should().Be(vertex.Long);
            serializedVertex.Float.Should().Be(vertex.Float);
            serializedVertex.Double.Should().Be(vertex.Double);
            serializedVertex.Bool.Should().Be(vertex.Bool);
            serializedVertex.IntArray.Should().BeEquivalentTo(vertex.IntArray);
            serializedVertex.IntIList.Should().BeEquivalentTo(vertex.IntIList);
        }

        [TestCaseSource(nameof(GraphSerializationTestCases))]
        public void GraphMLSerializationWithValidation_WriteEdge(
            [InstantHandle] Func<TestGraph, string> serializeGraph,
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
                serializedEdge.ID.Should().Be(edge.ID);
            serializedEdge.String.Should().Be(edge.String);
            serializedEdge.Int.Should().Be(edge.Int);
            serializedEdge.Long.Should().Be(edge.Long);
            serializedEdge.Float.Should().Be(edge.Float);
            serializedEdge.Double.Should().Be(edge.Double);
            serializedEdge.Bool.Should().Be(edge.Bool);
        }

        [Test]
        public void DeserializeAndValidateFromGraphML_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8631
#pragma warning disable CS8625
            var graph = new AdjacencyGraph<string, Edge<string>>();

            // Text reader
            Invoking(() => graph.DeserializeAndValidateFromGraphML(
                default,
                id => id,
                (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentNullException>();

            using (var reader = new StreamReader(TestGraph.SourcePath!))
            {
                Invoking(() => ((AdjacencyGraph<string, Edge<string>>?)default).DeserializeAndValidateFromGraphML(
                    reader,
                    id => id,
                    (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentNullException>();

                Invoking(() => graph.DeserializeAndValidateFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    reader,
                    default,
                    (source, target, _) => new Edge<string>(source, target))).Should().Throw<ArgumentNullException>();

                Invoking(() => graph.DeserializeAndValidateFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    reader,
                    id => id,
                    default)).Should().Throw<ArgumentNullException>();
            }
#pragma warning restore CS8631
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }
#endif

        #endregion

        #region Serialization/Deserialization

        [Pure]
        private static TOutGraph SerializeDeserialize<TInEdge, TOutEdge, TInGraph, TOutGraph>(
            TInGraph graph,
            [InstantHandle] Func<XmlReader, TOutGraph> deserialize)
            where TInEdge : IEdge<int>, IEquatable<TInEdge>
            where TOutEdge : IEdge<int>, IEquatable<TOutEdge>
            where TInGraph : IEdgeListGraph<int, TInEdge>
            where TOutGraph : IEdgeListGraph<int, TOutEdge>
        {
            graph.Should().NotBeNull();

            using (var stream = new MemoryStream())
            {
                // Serialize
                using (var writer = XmlWriter.Create(stream))
                {
                    graph.SerializeToGraphML(
                        writer,
                        vertex => vertex.ToString(),
                        graph.GetEdgeIdentity());
                }

                // Deserialize
                stream.Position = 0;

                // Deserialize
                using (var reader = XmlReader.Create(stream))
                {
                    TOutGraph deserializedGraph = deserialize(reader);
                    deserializedGraph.Should().NotBeNull();
                    deserializedGraph.Should().NotBeSameAs(graph);
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
            {
                var deserializedGraph = new TOutGraph();
                deserializedGraph.DeserializeFromGraphML(
                    reader,
                    int.Parse,
                    (source, target, _) => new EquatableEdge<int>(source, target));
                return deserializedGraph;
            });
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_SEdge<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<int, SEquatableEdge<int>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<int, SEquatableEdge<int>>, new()
        {
            return SerializeDeserialize<SEquatableEdge<int>, SEquatableEdge<int>, TInGraph, TOutGraph>(graph, reader =>
            {
                var deserializedGraph = new TOutGraph();
                deserializedGraph.DeserializeFromGraphML(
                    reader,
                    int.Parse,
                    (source, target, _) => new SEquatableEdge<int>(source, target));
                return deserializedGraph;
            });
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Reversed<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<int, SReversedEdge<int, EquatableEdge<int>>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<int, EquatableEdge<int>>, new()
        {
            return SerializeDeserialize<SReversedEdge<int, EquatableEdge<int>>, EquatableEdge<int>, TInGraph, TOutGraph>(graph, reader =>
            {
                var deserializedGraph = new TOutGraph();
                deserializedGraph.DeserializeFromGraphML(
                    reader,
                    int.Parse,
                    (source, target, _) => new EquatableEdge<int>(source, target));
                return deserializedGraph;
            });
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationAdjacencyGraphTestCases))]
        public void GraphMLSerialization_AdjacencyGraph(AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph1 =
                SerializeDeserialize<AdjacencyGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            EquateGraphs.Equate(graph, deserializedGraph1).Should().BeTrue();

            var arrayGraph = new ArrayAdjacencyGraph<int, EquatableEdge<int>>(graph);
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize<ArrayAdjacencyGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(arrayGraph);
            EquateGraphs.Equate(arrayGraph, deserializedGraph2).Should().BeTrue();
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationAdjacencyGraphTestCases))]
        public void GraphMLSerialization_AdapterGraph(AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            var bidirectionalAdapterGraph = new BidirectionalAdapterGraph<int, EquatableEdge<int>>(graph);
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<BidirectionalAdapterGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(bidirectionalAdapterGraph);
            EquateGraphs.Equate(graph, deserializedGraph).Should().BeTrue();
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationClusteredAdjacencyGraphTestCases))]
        public void GraphMLSerialization_ClusteredGraph(ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<ClusteredAdjacencyGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            EquateGraphs.Equate(graph, deserializedGraph).Should().BeTrue();
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationCompressedGraphTestCases))]
        public void GraphMLSerialization_CompressedGraph(CompressedSparseRowGraph<int> graph)
        {
            AdjacencyGraph<int, SEquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_SEdge<CompressedSparseRowGraph<int>, AdjacencyGraph<int, SEquatableEdge<int>>>(graph);
            EquateGraphs.Equate(graph, deserializedGraph).Should().BeTrue();
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationBidirectionalGraphTestCases))]
        public void GraphMLSerialization_BidirectionalGraph(BidirectionalGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<BidirectionalGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            EquateGraphs.Equate(graph, deserializedGraph).Should().BeTrue();

            var arrayGraph = new ArrayBidirectionalGraph<int, EquatableEdge<int>>(graph);
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize<ArrayBidirectionalGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(arrayGraph);
            EquateGraphs.Equate(arrayGraph, deserializedGraph2).Should().BeTrue();

            var reversedGraph = new ReversedBidirectionalGraph<int, EquatableEdge<int>>(graph);
            BidirectionalGraph<int, EquatableEdge<int>> deserializedGraph3 =
                SerializeDeserialize_Reversed<ReversedBidirectionalGraph<int, EquatableEdge<int>>, BidirectionalGraph<int, EquatableEdge<int>>>(reversedGraph);
            EquateGraphs.Equate(
                graph,
                deserializedGraph3,
                EqualityComparer<int>.Default,
                LambdaEqualityComparer<EquatableEdge<int>>.Create(
                    (edge1, edge2) => Equals(edge1.Source, edge2.Target) && Equals(edge1.Target, edge2.Source),
                    edge => edge.GetHashCode())).Should().BeTrue();

            var undirectedBidirectionalGraph = new UndirectedBidirectionalGraph<int, EquatableEdge<int>>(graph);
            UndirectedGraph<int, EquatableEdge<int>> deserializedGraph4 =
                SerializeDeserialize<UndirectedBidirectionalGraph<int, EquatableEdge<int>>, UndirectedGraph<int, EquatableEdge<int>>>(undirectedBidirectionalGraph);
            EquateGraphs.Equate(undirectedBidirectionalGraph, deserializedGraph4).Should().BeTrue();
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationBidirectionalMatrixGraphTestCases))]
        public void GraphMLSerialization_BidirectionalMatrixGraph(BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<BidirectionalMatrixGraph<EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            EquateGraphs.Equate(graph, deserializedGraph).Should().BeTrue();
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationUndirectedGraphTestCases))]
        public void GraphMLSerialization_UndirectedGraph(UndirectedGraph<int, EquatableEdge<int>> graph)
        {
            UndirectedGraph<int, EquatableEdge<int>> deserializedGraph1 =
                SerializeDeserialize<UndirectedGraph<int, EquatableEdge<int>>, UndirectedGraph<int, EquatableEdge<int>>>(graph);
            EquateGraphs.Equate(graph, deserializedGraph1).Should().BeTrue();

            var arrayGraph = new ArrayUndirectedGraph<int, EquatableEdge<int>>(graph);
            UndirectedGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize<ArrayUndirectedGraph<int, EquatableEdge<int>>, UndirectedGraph<int, EquatableEdge<int>>>(arrayGraph);
            EquateGraphs.Equate(graph, deserializedGraph2).Should().BeTrue();
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationEdgeListGraphTestCases))]
        public void GraphMLSerialization_EdgeListGraph(EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize<EdgeListGraph<int, EquatableEdge<int>>, AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            EquateGraphs.Equate(graph, deserializedGraph).Should().BeTrue();
        }

        #endregion

        private static IEnumerable<TestCaseData> GetGraphMLFiles_All() => TestGraphSourceProvider.Instance.AllGeneralPurpose.Select(namedTestGraphSource => new TestCaseData(namedTestGraphSource));
    }
}
