using System;
using System.IO;
using System.Net;
using System.Text;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class GraphvizExtensionsTests
    {
        [Test]
        public void ToGraphviz()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(3, 1)
            });
            graph.AddVertexRange(new[] { 4, 5 });

            string expectedDot =
                "digraph G {" + Environment.NewLine
                 + "0;" + Environment.NewLine
                 + "1;" + Environment.NewLine
                 + "2;" + Environment.NewLine
                 + "3;" + Environment.NewLine
                 + "4;" + Environment.NewLine
                 + "0 -> 1;" + Environment.NewLine
                 + "1 -> 2;" + Environment.NewLine
                 + "2 -> 0;" + Environment.NewLine
                 + "}";
            string dotGraph = graph.ToGraphviz();
            Assert.AreEqual(expectedDot, dotGraph);
        }

        [Test]
        public void ToGraphvizWithInit()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            wrappedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 4)
            });
            wrappedGraph.AddVertex(5);
            var clusteredGraph = new ClusteredAdjacencyGraph<int, Edge<int>>(wrappedGraph);
            ClusteredAdjacencyGraph<int, Edge<int>> subGraph1 = clusteredGraph.AddCluster();
            subGraph1.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(6, 7),
                new Edge<int>(7, 8)
            });
            ClusteredAdjacencyGraph<int, Edge<int>> subGraph2 = clusteredGraph.AddCluster();
            subGraph2.AddVerticesAndEdge(new Edge<int>(9, 10));
            subGraph2.AddVertex(11);

            string expectedDot =
                "digraph G {" + Environment.NewLine
                + "node [shape=diamond];" + Environment.NewLine
                + "edge [tooltip=\"Test Edge\"];" + Environment.NewLine
                + "subgraph cluster1 {" + Environment.NewLine
                + "5 [label=\"Test Vertex 6\"];" + Environment.NewLine
                + "6 [label=\"Test Vertex 7\"];" + Environment.NewLine
                + "7 [label=\"Test Vertex 8\"];" + Environment.NewLine
                + "5 -> 6;" + Environment.NewLine
                + "6 -> 7;" + Environment.NewLine
                + "}" + Environment.NewLine
                + "subgraph cluster2 {" + Environment.NewLine
                + "8 [label=\"Test Vertex 9\"];" + Environment.NewLine
                + "9 [label=\"Test Vertex 10\"];" + Environment.NewLine
                + "10 [label=\"Test Vertex 11\"];" + Environment.NewLine
                + "8 -> 9;" + Environment.NewLine
                + "}" + Environment.NewLine
                + "0 [label=\"Test Vertex 1\"];" + Environment.NewLine
                + "1 [label=\"Test Vertex 2\"];" + Environment.NewLine
                + "2 [label=\"Test Vertex 3\"];" + Environment.NewLine
                + "3 [label=\"Test Vertex 4\"];" + Environment.NewLine
                + "4 [label=\"Test Vertex 5\"];" + Environment.NewLine
                + "0 -> 1;" + Environment.NewLine
                + "0 -> 2;" + Environment.NewLine
                + "1 -> 3;" + Environment.NewLine
                + "}";
            string dotGraph = clusteredGraph.ToGraphviz(algorithm =>
            {
                algorithm.CommonVertexFormat.Shape = GraphvizVertexShape.Diamond;
                algorithm.CommonEdgeFormat.ToolTip = "Test Edge";
                algorithm.FormatVertex += (sender, args) =>
                {
                    args.VertexFormat.Label = $"Test Vertex {args.Vertex}";
                };
            });
            Assert.AreEqual(expectedDot, dotGraph);
        }

        [Test]
        public void ToGraphvizWithInit_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.ToGraphviz(null));
        }

        #region Test classes

        private class TestWebRequestCreate : IWebRequestCreate
        {
            [NotNull]
            private static readonly object LockObject = new object();

            private static WebRequest _nextRequest;

            private static WebRequest NextRequest
            {
                set
                {
                    lock (LockObject)
                    {
                        _nextRequest = value;
                    }
                }
            }

            /// <inheritdoc />
            public WebRequest Create(Uri uri)
            {
                return _nextRequest;
            }

            public static void CreateFailTestRequest()
            {
                var request = new TestWebRequest();
                NextRequest = request;
            }

            public static void CreateTestRequest([NotNull] string response)
            {
                var request = new TestWebRequest(response);
                NextRequest = request;
            }
        }

        private class TestWebRequest : WebRequest
        {
            [NotNull]
            private readonly MemoryStream _requestStream = new MemoryStream();
            
            [CanBeNull]
            private readonly MemoryStream _responseStream;

            /// <inheritdoc />
            public override string Method { get; set; }

            /// <inheritdoc />
            public override string ContentType { get; set; }

            /// <inheritdoc />
            public override long ContentLength { get; set; }

            public TestWebRequest()
            {
            }

            public TestWebRequest([NotNull] string response)
            {
                _responseStream = new MemoryStream(Encoding.UTF8.GetBytes(response));
            }

            /// <inheritdoc />
            public override Stream GetRequestStream()
            {
                return _requestStream;
            }

            /// <inheritdoc />
            public override WebResponse GetResponse()
            {
                return new TestWebResponse(_responseStream);
            }
        }

        private class TestWebResponse : WebResponse
        {
            [CanBeNull]
            private readonly Stream _responseStream;

            public TestWebResponse([CanBeNull] Stream responseStream)
            {
                _responseStream = responseStream;
            }

            /// <inheritdoc />
            public override Stream GetResponseStream()
            {
                return _responseStream;
            }
        }

        #endregion

        #region Test helpers

        [Pure]
        [NotNull]
        private static AdjacencyGraph<int, Edge<int>> CreateTestGraph()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(3, 1)
            });
            graph.AddVertexRange(new[] { 4, 5 });
            return graph;
        }

        #endregion

        [Test]
        public void ToSvg()
        {
            const string expectedSvg = "Mock SVG content";
            WebRequest.RegisterPrefix(
                GraphvizExtensions.DotToSvgApiEndpoint,
                new TestWebRequestCreate());
            TestWebRequestCreate.CreateTestRequest(expectedSvg);

            AdjacencyGraph<int, Edge<int>> graph = CreateTestGraph();

            Assert.AreEqual(expectedSvg, graph.ToSvg());
        }

        [Test]
        public void ToSvg_Failure()
        {
            WebRequest.RegisterPrefix(
                GraphvizExtensions.DotToSvgApiEndpoint,
                new TestWebRequestCreate());
            TestWebRequestCreate.CreateFailTestRequest();

            AdjacencyGraph<int, Edge<int>> graph = CreateTestGraph();

            Assert.IsEmpty(graph.ToSvg());
        }

        [Test]
        public void ToSvgWithInit()
        {
            const string expectedSvg = "Mock SVG content";
            WebRequest.RegisterPrefix(
                GraphvizExtensions.DotToSvgApiEndpoint,
                new TestWebRequestCreate());
            TestWebRequestCreate.CreateTestRequest(expectedSvg);

            AdjacencyGraph<int, Edge<int>> graph = CreateTestGraph();

            Assert.AreEqual(
                expectedSvg,
                graph.ToSvg(algorithm =>
                {
                    algorithm.CommonVertexFormat.ToolTip = "Test vertex";
                }));
        }

        [Test]
        public void ToSvgWithInit_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.ToSvg(null));
        }

        [Test]
        public void ToSvgWithInit_Failure()
        {
            WebRequest.RegisterPrefix(
                GraphvizExtensions.DotToSvgApiEndpoint,
                new TestWebRequestCreate());
            TestWebRequestCreate.CreateFailTestRequest();

            AdjacencyGraph<int, Edge<int>> graph = CreateTestGraph();

            Assert.IsEmpty(
                graph.ToSvg(
                    algorithm =>
                    {
                        algorithm.CommonVertexFormat.ToolTip = "Test vertex";
                    }));
        }

        [Test]
        public void DotToSvg_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => GraphvizExtensions.ToSvg(null));
        }
    }
}