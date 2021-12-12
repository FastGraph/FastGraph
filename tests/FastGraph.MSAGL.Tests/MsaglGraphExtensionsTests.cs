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
                var populator = g.CreateMsaglPopulator();
                Assert.IsNotNull(populator);

                populator = g.CreateMsaglPopulator("TestFormat {0}");
                Assert.IsNotNull(populator);

                populator = g.CreateMsaglPopulator("TestFormat {0}", new VertexTestFormatProvider());
                Assert.IsNotNull(populator);

                populator = g.CreateMsaglPopulator(v => v.ToString());
                Assert.IsNotNull(populator);
            }

            #endregion
        }

        [Test]
        public void CreatePopulators_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => MsaglGraphExtensions.CreateMsaglPopulator<int, Edge<int>>(default));

            Assert.Throws<ArgumentNullException>(() => MsaglGraphExtensions.CreateMsaglPopulator<int, Edge<int>>(default, vertex => vertex.ToString()));
            Assert.Throws<ArgumentNullException>(() => MsaglGraphExtensions.CreateMsaglPopulator(graph, default));
            Assert.Throws<ArgumentNullException>(() => MsaglGraphExtensions.CreateMsaglPopulator<int, Edge<int>>(default, default));

            Assert.Throws<ArgumentNullException>(() => MsaglGraphExtensions.CreateMsaglPopulator<int, Edge<int>>(default, "Format {0}"));
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
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
                AssertAreEquivalent(g, msaglGraph);

                var expectedVerticesAdded = new HashSet<int>(g.Vertices);
                msaglGraph = g.IsVerticesEmpty
                    ? g.ToMsaglGraph(NoNodeAdded)
                    : g.ToMsaglGraph(NodeAdded);
                AssertAreEquivalent(g, msaglGraph);
                CollectionAssert.IsEmpty(expectedVerticesAdded);

                expectedVerticesAdded = new HashSet<int>(g.Vertices);
                msaglGraph = g.IsVerticesEmpty
                    ? g.ToMsaglGraph(VertexIdentity, NoNodeAdded)
                    : g.ToMsaglGraph(VertexIdentity, NodeAdded);
                AssertAreEquivalent(g, msaglGraph);
                CollectionAssert.IsEmpty(expectedVerticesAdded);


                var expectedEdgesAdded = new HashSet<Edge<int>>(g.Edges);
                msaglGraph = g.IsEdgesEmpty
                    ? g.ToMsaglGraph(edgeAdded: NoEdgeAdded)
                    : g.ToMsaglGraph(edgeAdded: EdgeAdded);
                AssertAreEquivalent(g, msaglGraph);
                CollectionAssert.IsEmpty(expectedEdgesAdded);

                expectedEdgesAdded = new HashSet<Edge<int>>(g.Edges);
                msaglGraph = g.IsEdgesEmpty
                    ? g.ToMsaglGraph(VertexIdentity, edgeAdded: NoEdgeAdded)
                    : g.ToMsaglGraph(VertexIdentity, edgeAdded: EdgeAdded);
                AssertAreEquivalent(g, msaglGraph);
                CollectionAssert.IsEmpty(expectedEdgesAdded);


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
                AssertAreEquivalent(g, msaglGraph);
                CollectionAssert.IsEmpty(expectedVerticesAdded);
                CollectionAssert.IsEmpty(expectedEdgesAdded);

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
                AssertAreEquivalent(g, msaglGraph);
                CollectionAssert.IsEmpty(expectedVerticesAdded);
                CollectionAssert.IsEmpty(expectedEdgesAdded);

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
                    Assert.IsTrue(expectedVerticesAdded.Remove(args.Vertex));
                }

                void NoEdgeAdded(object sender, MsaglEdgeEventArgs<int, Edge<int>> args)
                {
                    Assert.Fail($"{nameof(MsaglGraphPopulator<object, Edge<object>>.EdgeAdded)} event called.");
                }

                void EdgeAdded(object sender, MsaglEdgeEventArgs<int, Edge<int>> args)
                {
                    Assert.IsTrue(expectedEdgesAdded.Remove(args.Edge));
                }

                #endregion
            }

            #endregion
        }
    }
}
