#nullable enable

using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using JetBrains.Annotations;
using NUnit.Framework;

namespace FastGraph.Serialization.Tests
{
    /// <summary>
    /// Tests relative to XML serialization of some serializable structures.
    /// </summary>
    [TestFixture]
    internal sealed class XmlSerializableStructuresTests
    {
        [NotNull]
        private const string XmlHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        #region Test helpers

        private static void SerializeAndRead<TVertex, TEdge, TGraph>(
            XmlSerializableGraph<TVertex, TEdge, TGraph> graph,
            [InstantHandle] Action<string> checkSerializedContent)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
            where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>, new()
        {
            using (var memory = new MemoryStream())
            {
                var writer = new StreamWriter(memory);
                try
                {
                    var serializer = new XmlSerializer(graph.GetType());
                    serializer.Serialize(writer, graph);

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
            var emptyGraph = new XmlSerializableGraph<Person, XmlSerializableEdge<Person>, AdjacencyGraph<Person, XmlSerializableEdge<Person>>>();

            SerializeAndRead(
                emptyGraph,
                content =>
                {
                    var regex = new Regex($@"{Regex.Escape(XmlHeader)}\s*<graph\s*.*?\s*>\s*<vertices\s*\/>\s*<edges\s*\/>\s*<\/graph>");
                    regex.Match(content).Success.Should().BeTrue();
                });
        }

        [Test]
        public void SerializeToXml()
        {
            var wrappedGraph = new AdjacencyGraph<int, XmlSerializableEdge<int>>();
            wrappedGraph.AddVerticesAndEdgeRange(new[]
            {
                new XmlSerializableEdge<int> { Source = 1, Target = 2 },
                new XmlSerializableEdge<int> { Source = 1, Target = 3 },
                new XmlSerializableEdge<int> { Source = 2, Target = 2 },
            });
            wrappedGraph.AddVertex(4);

            var graph = new XmlSerializableGraph<int, XmlSerializableEdge<int>, AdjacencyGraph<int, XmlSerializableEdge<int>>>(wrappedGraph);
            SerializeAndRead(
                graph,
                content =>
                {
                    var graphContent = new StringBuilder();

                    graphContent.Append(@"<vertices>\s*");
                    foreach (int vertex in graph.Vertices)
                    {
                        graphContent.Append(
                            $@"<vertex>{vertex}<\/vertex>\s*");
                    }
                    graphContent.Append(@"<\/vertices>\s*");

                    graphContent.Append(@"<edges>\s*");
                    foreach (XmlSerializableEdge<int> edge in graph.Edges)
                    {
                        graphContent.Append(
                            $@"<edge>\s*<Source>{edge.Source}<\/Source>\s*<Target>{edge.Target}<\/Target>\s*<\/edge>\s*");
                    }
                    graphContent.Append(@"<\/edges>");

                    var regex = new Regex(
                        $@"{Regex.Escape(XmlHeader)}\s*<graph\s*.*?\s*>\s*{graphContent}\s*<\/graph>");
                    regex.Match(content).Success.Should().BeTrue();
                });
        }

        [Test]
        public void XmlSerializableGraph()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            var graph = new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(wrappedGraph);

            graph.Vertices.Should().BeEmpty();
            graph.Edges.Should().BeEmpty();

            var vertices = new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>.XmlVertexList(wrappedGraph)
            {
                1, 2
            };
            graph.Vertices = vertices;

            graph.Vertices.Should().BeEquivalentTo(new[] { 1, 2 });
            graph.Edges.Should().BeEmpty();

            var edge12 = new Edge<int>(1, 2);
            var edge22 = new Edge<int>(2, 2);
            var edges = new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>.XmlEdgeList(wrappedGraph)
            {
                edge12, edge22
            };
            graph.Edges = edges;

            graph.Vertices.Should().BeEquivalentTo(new[] { 1, 2 });
            graph.Edges.Should().BeEquivalentTo(new[] { edge12, edge22 });
        }

        [Test]
        public void XmlSerializableGraph_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void XmlVertexList()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var vertexList = new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>.XmlVertexList(graph);

            vertexList.Should().BeEmpty();

            var edge12 = new Edge<int>(1, 2);
            var edge22 = new Edge<int>(2, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge22 });

            vertexList.Should().BeEquivalentTo(new[] { 1, 2 });

            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge22 });
            vertexList = new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>.XmlVertexList(graph);

            vertexList.Should().BeEquivalentTo(new[] { 1, 2 });
        }

        [Test]
        public void XmlVertexList_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>.XmlVertexList(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void XmlVertexList_Add()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var vertexList = new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>.XmlVertexList(graph);

            vertexList.Should().BeEmpty();

            vertexList.Add(1);

            vertexList.Should().BeEquivalentTo(new[] { 1 });

            vertexList.Add(2);

            vertexList.Should().BeEquivalentTo(new[] { 1, 2 });
        }

        [Test]
        public void XmlVertexList_Add_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var vertexList = new XmlSerializableGraph<TestVertex, Edge<TestVertex>, AdjacencyGraph<TestVertex, Edge<TestVertex>>>.XmlVertexList(graph);
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => vertexList.Add(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void XmlEdgeList()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var edgeList = new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>.XmlEdgeList(graph);

            edgeList.Should().BeEmpty();

            var edge12 = new Edge<int>(1, 2);
            var edge22 = new Edge<int>(2, 2);
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge22 });

            edgeList.Should().BeEquivalentTo(new[] { edge12, edge22 });

            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[] { edge12, edge22 });
            edgeList = new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>.XmlEdgeList(graph);

            edgeList.Should().BeEquivalentTo(new[] { edge12, edge22 });
        }

        [Test]
        public void XmlEdgeList_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>.XmlEdgeList(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void XmlEdgeList_Add()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var edgeList = new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>.XmlEdgeList(graph);

            edgeList.Should().BeEmpty();

            var edge12 = new Edge<int>(1, 2);
            edgeList.Add(edge12);

            edgeList.Should().BeEquivalentTo(new[] { edge12 });

            var edge22 = new Edge<int>(2, 2);
            edgeList.Add(edge22);

            edgeList.Should().BeEquivalentTo(new[] { edge12, edge22 });
        }

        [Test]
        public void XmlEdgeList_Add_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var edgeList = new XmlSerializableGraph<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>.XmlEdgeList(graph);
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => edgeList.Add(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }
    }
}
