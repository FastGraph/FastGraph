#nullable enable

using System.Diagnostics.CodeAnalysis;
using Microsoft.Msagl.Drawing;
using NUnit.Framework;
using static FastGraph.MSAGL.Tests.MsaglGraphTestHelpers;

namespace FastGraph.MSAGL.Tests
{
    /// <summary>
    /// Tests related to <see cref="MsaglGraphExtensions"/>.
    /// </summary>
    internal sealed class MsaglGraphExtensionsTests
    {
        #region Test classes

        private class VertexTestFormatProvider : IFormatProvider
        {
            public object GetFormat(Type? formatType)
            {
#pragma warning disable CS8603
                return default;
#pragma warning restore CS8603
            }
        }

        #endregion

        [Test]
        public void CreatePopulators()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            CreatePopulators_Test(graph);

            var undirectedGraph = new UndirectedGraph<int, Edge<int>>();
            CreatePopulators_Test(undirectedGraph);

            #region Local function

            void CreatePopulators_Test(IEdgeListGraph<int, Edge<int>> g)
            {
                g.CreateMsaglPopulator().Should().NotBeNull();

                g.CreateMsaglPopulator("TestFormat {0}").Should().NotBeNull();

                g.CreateMsaglPopulator("TestFormat {0}", new VertexTestFormatProvider()).Should().NotBeNull();

                g.CreateMsaglPopulator(v => v.ToString()).Should().NotBeNull();
            }

            #endregion
        }

        [Test]
        public void CreatePopulators_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => MsaglGraphExtensions.CreateMsaglPopulator<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => MsaglGraphExtensions.CreateMsaglPopulator<int, Edge<int>>(default, vertex => vertex.ToString())).Should().Throw<ArgumentNullException>();
            Invoking(() => graph.CreateMsaglPopulator(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => MsaglGraphExtensions.CreateMsaglPopulator<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => MsaglGraphExtensions.CreateMsaglPopulator<int, Edge<int>>(default, "Format {0}")).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void ToMsaglGraph()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ToMsaglGraph_Test(graph);

            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2, 4 });
            ToMsaglGraph_Test(graph);

            graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(2, 5),
                new Edge<int>(3, 4),
                new Edge<int>(4, 3)
            });
            graph.AddVertex(6);
            ToMsaglGraph_Test(graph);

            var undirectedGraph = new UndirectedGraph<int, Edge<int>>();
            undirectedGraph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(2, 5),
                new Edge<int>(3, 4),
                new Edge<int>(4, 3)
            });
            undirectedGraph.AddVertex(6);
            ToMsaglGraph_Test(undirectedGraph);

            #region Local function

            // ReSharper disable once InconsistentNaming
            void ToMsaglGraph_Test(IEdgeListGraph<int, Edge<int>> g)
            {
                Graph msaglGraph = g.ToMsaglGraph();
                msaglGraph.Should().BeOfType<Graph>().BeEquivalentTo(g);

                var expectedVerticesAdded = new HashSet<int>(g.Vertices);
                msaglGraph = g.IsVerticesEmpty
                    ? g.ToMsaglGraph(NoNodeAdded)
                    : g.ToMsaglGraph(NodeAdded);
                msaglGraph.Should().BeOfType<Graph>().BeEquivalentTo(g);
                expectedVerticesAdded.Should().BeEmpty();

                expectedVerticesAdded = new HashSet<int>(g.Vertices);
                msaglGraph = g.IsVerticesEmpty
                    ? g.ToMsaglGraph(VertexIdentity, NoNodeAdded)
                    : g.ToMsaglGraph(VertexIdentity, NodeAdded);
                msaglGraph.Should().BeOfType<Graph>().BeEquivalentTo(g);
                expectedVerticesAdded.Should().BeEmpty();


                var expectedEdgesAdded = new HashSet<Edge<int>>(g.Edges);
                msaglGraph = g.IsEdgesEmpty
                    ? g.ToMsaglGraph(edgeAdded: NoEdgeAdded)
                    : g.ToMsaglGraph(edgeAdded: EdgeAdded);
                msaglGraph.Should().BeOfType<Graph>().BeEquivalentTo(g);
                expectedEdgesAdded.Should().BeEmpty();

                expectedEdgesAdded = new HashSet<Edge<int>>(g.Edges);
                msaglGraph = g.IsEdgesEmpty
                    ? g.ToMsaglGraph(VertexIdentity, edgeAdded: NoEdgeAdded)
                    : g.ToMsaglGraph(VertexIdentity, edgeAdded: EdgeAdded);
                msaglGraph.Should().BeOfType<Graph>().BeEquivalentTo(g);
                expectedEdgesAdded.Should().BeEmpty();


                expectedVerticesAdded = new HashSet<int>(g.Vertices);
                expectedEdgesAdded = new HashSet<Edge<int>>(g.Edges);
                if (g.IsVerticesEmpty && g.IsEdgesEmpty)
                {
                    msaglGraph = g.ToMsaglGraph(NoNodeAdded, NoEdgeAdded);
                }
                else if (g.IsVerticesEmpty)
                {
                    msaglGraph = g.ToMsaglGraph(NoNodeAdded, EdgeAdded);
                }
                else if (g.IsEdgesEmpty)
                {
                    msaglGraph = g.ToMsaglGraph(NodeAdded, NoEdgeAdded);
                }
                else
                {
                    msaglGraph = g.ToMsaglGraph(NodeAdded, EdgeAdded);
                }
                msaglGraph.Should().BeOfType<Graph>().BeEquivalentTo(g);
                expectedVerticesAdded.Should().BeEmpty();
                expectedEdgesAdded.Should().BeEmpty();

                expectedVerticesAdded = new HashSet<int>(g.Vertices);
                expectedEdgesAdded = new HashSet<Edge<int>>(g.Edges);
                if (g.IsVerticesEmpty && g.IsEdgesEmpty)
                {
                    msaglGraph = g.ToMsaglGraph(VertexIdentity, NoNodeAdded, NoEdgeAdded);
                }
                else if (g.IsVerticesEmpty)
                {
                    msaglGraph = g.ToMsaglGraph(VertexIdentity, NoNodeAdded, EdgeAdded);
                }
                else if (g.IsEdgesEmpty)
                {
                    msaglGraph = g.ToMsaglGraph(VertexIdentity, NodeAdded, NoEdgeAdded);
                }
                else
                {
                    msaglGraph = g.ToMsaglGraph(VertexIdentity, NodeAdded, EdgeAdded);
                }
                msaglGraph.Should().BeOfType<Graph>().BeEquivalentTo(g);
                expectedVerticesAdded.Should().BeEmpty();
                expectedEdgesAdded.Should().BeEmpty();

                #region Local functions

                string VertexIdentity(int vertex)
                {
                    return $"id{vertex}";
                }

                void NoNodeAdded(object sender, MsaglVertexEventArgs<int> args)
                {
                    Assert.Fail($"{nameof(MsaglGraphPopulator<object, Edge<object>>.NodeAdded)} event called.");
                }

                void NodeAdded(object sender, MsaglVertexEventArgs<int> args)
                {
                    expectedVerticesAdded.Remove(args.Vertex).Should().BeTrue();
                }

                void NoEdgeAdded(object sender, MsaglEdgeEventArgs<int, Edge<int>> args)
                {
                    Assert.Fail($"{nameof(MsaglGraphPopulator<object, Edge<object>>.EdgeAdded)} event called.");
                }

                void EdgeAdded(object sender, MsaglEdgeEventArgs<int, Edge<int>> args)
                {
                    expectedEdgesAdded.Remove(args.Edge).Should().BeTrue();
                }

                #endregion
            }

            #endregion
        }
    }
}
