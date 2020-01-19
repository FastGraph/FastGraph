using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuikGraph.Tests.Structures;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Extensions
{
    /// <summary>
    /// Tests related to <see cref="GraphExtensions"/>.
    /// </summary>
    internal class GraphExtensionsTests : GraphTestsBase
    {
        #region Delegate graphs

        [Test]
        public void ToDelegateIncidenceGraph_TryGetDelegate()
        {
            TryFunc<int, IEnumerable<Edge<int>>> tryGetEdges =
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    outEdges = null;
                    return false;
                };

            DelegateIncidenceGraph<int, Edge<int>> graph = tryGetEdges.ToDelegateIncidenceGraph();
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => graph.OutEdges(1));

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            tryGetEdges =
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    if (vertex == 1)
                    {
                        outEdges = new[] { edge12 };
                        return true;
                    }

                    if (vertex == 2)
                    {
                        outEdges = Enumerable.Empty<Edge<int>>();
                        return true;
                    }

                    outEdges = null;
                    return false;
                };

            graph = tryGetEdges.ToDelegateIncidenceGraph();
            AssertHasOutEdges(graph, 1, new[] { edge12 });
            AssertNoOutEdge(graph, 2);

            // Graph can evolve based on the delegate
            tryGetEdges =
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    if (vertex == 1)
                    {
                        outEdges = new[] { edge12 };
                        return true;
                    }

                    if (vertex == 2)
                    {
                        outEdges = new[] { edge21 };
                        return true;
                    }

                    outEdges = null;
                    return false;
                };
            graph = tryGetEdges.ToDelegateIncidenceGraph();
            AssertHasOutEdges(graph, 1, new[] { edge12 });
            AssertHasOutEdges(graph, 2, new[] { edge21 });

            tryGetEdges =
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    if (vertex == 1)
                    {
                        outEdges = Enumerable.Empty<Edge<int>>();
                        return true;
                    }

                    if (vertex == 2)
                    {
                        outEdges = new[] { edge21 };
                        return true;
                    }

                    outEdges = null;
                    return false;
                };
            graph = tryGetEdges.ToDelegateIncidenceGraph();
            AssertNoOutEdge(graph, 1);
            AssertHasOutEdges(graph, 2, new[] { edge21 });
        }

        [Test]
        public void ToDelegateIncidenceGraph_TryGetDelegate_Throws()
        {
            TryFunc<int, IEnumerable<Edge<int>>> tryGetEdges = null;
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => tryGetEdges.ToDelegateIncidenceGraph());
        }

        [Test]
        public void ToDelegateIncidenceGraph_GetDelegate()
        {
            Func<int, IEnumerable<Edge<int>>> getEdges = vertex => null;

            DelegateIncidenceGraph<int, Edge<int>> graph = getEdges.ToDelegateIncidenceGraph();
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => graph.OutEdges(1));

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            getEdges =
                vertex =>
                {
                    if (vertex == 1)
                        return new[] { edge12 };

                    if (vertex == 2)
                        return Enumerable.Empty<Edge<int>>();

                    return null;
                };

            graph = getEdges.ToDelegateIncidenceGraph();
            AssertHasOutEdges(graph, 1, new[] { edge12 });
            AssertNoOutEdge(graph, 2);

            // Graph can evolve based on the delegate
            getEdges =
                vertex =>
                {
                    if (vertex == 1)
                        return new[] { edge12 };

                    if (vertex == 2)
                        return new[] { edge21 };

                    return null;
                };
            graph = getEdges.ToDelegateIncidenceGraph();
            AssertHasOutEdges(graph, 1, new[] { edge12 });
            AssertHasOutEdges(graph, 2, new[] { edge21 });

            getEdges =
                vertex =>
                {
                    if (vertex == 1)
                        return Enumerable.Empty<Edge<int>>();

                    if (vertex == 2)
                        return new[] { edge21 };

                    return null;
                };
            graph = getEdges.ToDelegateIncidenceGraph();
            AssertNoOutEdge(graph, 1);
            AssertHasOutEdges(graph, 2, new[] { edge21 });
        }

        [Test]
        public void ToDelegateIncidenceGraph_GetDelegate_Throws()
        {
            Func<int, IEnumerable<Edge<int>>> getEdges = null;
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => getEdges.ToDelegateIncidenceGraph());
        }

        [Test]
        public void ToDelegateVertexAndEdgeListGraph()
        {
            var dictionary = new Dictionary<int, IEnumerable<Edge<int>>>();
            DelegateVertexAndEdgeListGraph<int, Edge<int>> graph = dictionary.ToDelegateVertexAndEdgeListGraph
            <
                int,
                Edge<int>,
                IEnumerable<Edge<int>>
            >();
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            dictionary.Add(1, new[] { edge12 });
            AssertHasVertices(graph, new[] { 1 });
            AssertNoEdge(graph);    // Vertex 2 is not in graph, so edge is skipped

            dictionary.Add(2, new[] { edge12 });
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12 });

            // Graph can dynamically evolve
            dictionary[2] = new[] { edge21 };
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });

            dictionary[1] = Enumerable.Empty<Edge<int>>();
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge21 });
        }

        [Test]
        public void ToDelegateVertexAndEdgeListGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((Dictionary<int, IEnumerable<Edge<int>>>)null).ToDelegateVertexAndEdgeListGraph<int, Edge<int>, IEnumerable<Edge<int>>>());
        }

        [Test]
        public void ToDelegateVertexAndEdgeListGraph_ConverterEdges()
        {
            var dictionary = new Dictionary<int, int>();
            DelegateVertexAndEdgeListGraph<int, Edge<int>> graph = dictionary.ToDelegateVertexAndEdgeListGraph(pair => Enumerable.Empty<Edge<int>>());
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            graph = dictionary.ToDelegateVertexAndEdgeListGraph(pair =>
            {
                if (pair.Value == 1)
                    return new[] { edge12 };
                return new[] { edge21 };
            });
            AssertEmptyGraph(graph);

            dictionary.Add(1, 1);
            AssertHasVertices(graph, new[] { 1 });
            AssertNoEdge(graph);    // Vertex 2 is not in graph, so edge is skipped

            dictionary.Add(2, 1);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12 });

            // Graph can dynamically evolve
            dictionary[2] = 2;
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });

            dictionary[1] = 2;
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge21 });
        }

        [Test]
        public void ToDelegateVertexAndEdgeListGraph_ConverterEdges_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((Dictionary<int, Edge<int>>) null).ToDelegateVertexAndEdgeListGraph(pair => new[] { pair.Value }));
            Assert.Throws<ArgumentNullException>(
                () => ((Dictionary<int, Edge<int>>) null).ToDelegateVertexAndEdgeListGraph((Converter<KeyValuePair<int, Edge<int>>, IEnumerable<Edge<int>>>)null));

            var dictionary = new Dictionary<int, Edge<int>>();
            Assert.Throws<ArgumentNullException>(
                () => dictionary.ToDelegateVertexAndEdgeListGraph((Converter<KeyValuePair<int, Edge<int>>, IEnumerable<Edge<int>>>)null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ToDelegateVertexAndEdgeListGraph_TryGetDelegate()
        {
            var vertices = new List<int>();
            DelegateVertexAndEdgeListGraph<int, Edge<int>> graph = vertices.ToDelegateVertexAndEdgeListGraph(
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    outEdges = null;
                    return false;
                });
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            graph = vertices.ToDelegateVertexAndEdgeListGraph(
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    if (vertex == 1)
                    {
                        outEdges = new[] { edge12 };
                        return true;
                    }

                    if (vertex == 2)
                    {
                        outEdges = Enumerable.Empty<Edge<int>>();
                        return true;
                    }

                    outEdges = null;
                    return false;
                });
            AssertEmptyGraph(graph);

            vertices.Add(1);
            AssertHasVertices(graph, new[] { 1 });
            AssertNoEdge(graph);    // Vertex 2 is not in graph, so edge is skipped

            vertices.Add(2);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12 });

            // Graph can evolve based on the delegate
            graph = vertices.ToDelegateVertexAndEdgeListGraph(
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    if (vertex == 1)
                    {
                        outEdges = new[] { edge12 };
                        return true;
                    }

                    if (vertex == 2)
                    {
                        outEdges = new[] { edge21 };
                        return true;
                    }

                    outEdges = null;
                    return false;
                });
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });

            graph = vertices.ToDelegateVertexAndEdgeListGraph(
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    if (vertex == 1)
                    {
                        outEdges = Enumerable.Empty<Edge<int>>();
                        return true;
                    }

                    if (vertex == 2)
                    {
                        outEdges = new[] { edge21 };
                        return true;
                    }

                    outEdges = null;
                    return false;
                });
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge21 });
        }

        [Test]
        public void ToDelegateVertexAndEdgeListGraph_TryGetDelegate_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToDelegateVertexAndEdgeListGraph(
                    (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                    {
                        outEdges = null;
                        return false;
                    }));
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToDelegateVertexAndEdgeListGraph((TryFunc<int, IEnumerable<Edge<int>>>)null));

            IEnumerable<int> vertices = Enumerable.Empty<int>();
            Assert.Throws<ArgumentNullException>(
                () => vertices.ToDelegateVertexAndEdgeListGraph((TryFunc<int, IEnumerable<Edge<int>>>)null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ToDelegateVertexAndEdgeListGraph_GetDelegate()
        {
            var vertices = new List<int>();
            DelegateVertexAndEdgeListGraph<int, Edge<int>> graph = vertices.ToDelegateVertexAndEdgeListGraph<int, Edge<int>>(vertex => null);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            graph = vertices.ToDelegateVertexAndEdgeListGraph(
                vertex =>
                {
                    if (vertex == 1)
                        return new[] { edge12 };

                    if (vertex == 2)
                        return Enumerable.Empty<Edge<int>>();

                    return null;
                });
            AssertEmptyGraph(graph);

            vertices.Add(1);
            AssertHasVertices(graph, new[] { 1 });
            AssertNoEdge(graph);    // Vertex 2 is not in graph, so edge is skipped

            vertices.Add(2);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12 });

            // Graph can evolve based on the delegate
            graph = vertices.ToDelegateVertexAndEdgeListGraph(
                vertex =>
                {
                    if (vertex == 1)
                        return new[] { edge12 };

                    if (vertex == 2)
                        return new[] { edge21 };

                    return null;
                });
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });

            graph = vertices.ToDelegateVertexAndEdgeListGraph(
                vertex =>
                {
                    if (vertex == 1)
                        return Enumerable.Empty<Edge<int>>();

                    if (vertex == 2)
                        return new[] { edge21 };

                    return null;
                });
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge21 });
        }

        [Test]
        public void ToDelegateVertexAndEdgeListGraph_GetDelegate_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToDelegateVertexAndEdgeListGraph<int, Edge<int>>(vertex => null));
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToDelegateVertexAndEdgeListGraph((Func<int, IEnumerable<Edge<int>>>)null));

            IEnumerable<int> vertices = Enumerable.Empty<int>();
            Assert.Throws<ArgumentNullException>(
                () => vertices.ToDelegateVertexAndEdgeListGraph((Func<int, IEnumerable<Edge<int>>>)null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ToDelegateBidirectionalIncidenceGraph()
        {
            TryFunc<int, IEnumerable<Edge<int>>> tryGetOutEdges =
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    outEdges = null;
                    return false;
                };
            TryFunc<int, IEnumerable<Edge<int>>> tryGetInEdges =
                (int vertex, out IEnumerable<Edge<int>> inEdges) =>
                {
                    inEdges = null;
                    return false;
                };

            DelegateBidirectionalIncidenceGraph<int, Edge<int>> graph = tryGetOutEdges.ToDelegateBidirectionalIncidenceGraph(tryGetInEdges);
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => graph.OutEdges(1));
            Assert.Throws<VertexNotFoundException>(() => graph.InEdges(1));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            var edge12 = new Edge<int>(1, 2);
            tryGetOutEdges =
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    if (vertex == 1)
                    {
                        outEdges = new[] { edge12 };
                        return true;
                    }

                    if (vertex == 2)
                    {
                        outEdges = Enumerable.Empty<Edge<int>>();
                        return true;
                    }

                    outEdges = null;
                    return false;
                };
            tryGetInEdges =
                (int vertex, out IEnumerable<Edge<int>> inEdges) =>
                {
                    if (vertex == 1)
                    {
                        inEdges = Enumerable.Empty<Edge<int>>();
                        return true;
                    }

                    if (vertex == 2)
                    {
                        inEdges = new[] { edge12 };
                        return true;
                    }

                    inEdges = null;
                    return false;
                };
            graph = tryGetOutEdges.ToDelegateBidirectionalIncidenceGraph(tryGetInEdges);
            AssertHasOutEdges(graph, 1, new[] { edge12 });
            AssertNoOutEdge(graph, 2);
            AssertNoInEdge(graph, 1);
            AssertHasInEdges(graph, 2, new[] { edge12 });

            // Graph can evolve based on the delegate
            var edge21 = new Edge<int>(2, 1);
            var edge23 = new Edge<int>(2, 3);
            tryGetOutEdges =
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    if (vertex == 1)
                    {
                        outEdges = new[] { edge12 };
                        return true;
                    }

                    if (vertex == 2)
                    {
                        outEdges = new[] { edge21, edge23 };
                        return true;
                    }

                    if (vertex == 3)
                    {
                        outEdges = Enumerable.Empty<Edge<int>>();
                        return true;
                    }

                    outEdges = null;
                    return false;
                };
            tryGetInEdges =
                (int vertex, out IEnumerable<Edge<int>> inEdges) =>
                {
                    if (vertex == 1)
                    {
                        inEdges = new[] { edge21 };
                        return true;
                    }

                    if (vertex == 2)
                    {
                        inEdges = new[] { edge12 };
                        return true;
                    }

                    if (vertex == 3)
                    {
                        inEdges = new[] { edge23 };
                        return true;
                    }

                    inEdges = null;
                    return false;
                };
            graph = tryGetOutEdges.ToDelegateBidirectionalIncidenceGraph(tryGetInEdges);
            AssertHasOutEdges(graph, 1, new[] { edge12 });
            AssertHasOutEdges(graph, 2, new[] { edge21, edge23 });
            AssertNoOutEdge(graph, 3);
            AssertHasInEdges(graph, 1, new[] { edge21 });
            AssertHasInEdges(graph, 2, new[] { edge12 });
            AssertHasInEdges(graph, 3, new[] { edge23 });
        }

        [Test]
        public void ToDelegateBidirectionalIncidenceGraph_Throws()
        {
            TryFunc<int, IEnumerable<Edge<int>>> tryGetOutEdges = null;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => tryGetOutEdges.ToDelegateBidirectionalIncidenceGraph(null));
            TryFunc<int, IEnumerable<Edge<int>>> tryGetInEdges =
                (int vertex, out IEnumerable<Edge<int>> inEdges) =>
                {
                    inEdges = null;
                    return false;
                };
            Assert.Throws<ArgumentNullException>(
                () => tryGetOutEdges.ToDelegateBidirectionalIncidenceGraph(tryGetInEdges));

            tryGetOutEdges =
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    outEdges = null;
                    return false;
                };
            Assert.Throws<ArgumentNullException>(
                () => tryGetOutEdges.ToDelegateBidirectionalIncidenceGraph(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ToDelegateUndirectedGraph_TryGetDelegate()
        {
            var vertices = new List<int>();
            DelegateUndirectedGraph<int, Edge<int>> graph = vertices.ToDelegateUndirectedGraph(
                (int vertex, out IEnumerable<Edge<int>> adjacentEdges) =>
                {
                    adjacentEdges = null;
                    return false;
                });
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            graph = vertices.ToDelegateUndirectedGraph(
                (int vertex, out IEnumerable<Edge<int>> adjacentEdges) =>
                {
                    if (vertex == 1 || vertex == 2)
                    {
                        adjacentEdges = new[] { edge12, edge21 };
                        return true;
                    }

                    adjacentEdges = null;
                    return false;
                });
            AssertEmptyGraph(graph);

            vertices.Add(1);
            AssertHasVertices(graph, new[] { 1 });
            AssertNoEdge(graph);    // Vertex 2 is not in graph, so edge is skipped

            vertices.Add(2);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });
            AssertHasAdjacentEdges(graph, 1, new[] { edge12, edge21 });
            AssertHasAdjacentEdges(graph, 2, new[] { edge12, edge21 });

            // Graph can evolve based on the delegate
            vertices.Add(3);
            var edge23 = new Edge<int>(2, 3);
            graph = vertices.ToDelegateUndirectedGraph(
                (int vertex, out IEnumerable<Edge<int>> adjacentEdges) =>
                {
                    if (vertex == 1)
                    {
                        adjacentEdges = new[] { edge12, edge21 };
                        return true;
                    }

                    if (vertex == 2)
                    {
                        adjacentEdges = new[] { edge12, edge21, edge23 };
                        return true;
                    }

                    if (vertex == 3)
                    {
                        adjacentEdges = new[] { edge23 };
                        return true;
                    }

                    adjacentEdges = null;
                    return false;
                });
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge21, edge23 });
            AssertHasAdjacentEdges(graph, 1, new[] { edge12, edge21 });
            AssertHasAdjacentEdges(graph, 2, new[] { edge12, edge21, edge23 });
            AssertHasAdjacentEdges(graph, 3, new[] { edge23 });
        }

        [Test]
        public void ToDelegateUndirectedGraph_TryGetDelegate_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToDelegateUndirectedGraph(
                    (int vertex, out IEnumerable<Edge<int>> adjacentEdges) =>
                    {
                        adjacentEdges = null;
                        return false;
                    }));
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToDelegateUndirectedGraph((TryFunc<int, IEnumerable<Edge<int>>>)null));

            IEnumerable<int> vertices = Enumerable.Empty<int>();
            Assert.Throws<ArgumentNullException>(
                () => vertices.ToDelegateUndirectedGraph((TryFunc<int, IEnumerable<Edge<int>>>)null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ToDelegateUndirectedGraph_GetDelegate()
        {
            var vertices = new List<int>();
            DelegateUndirectedGraph<int, Edge<int>> graph = vertices.ToDelegateUndirectedGraph<int, Edge<int>>(vertex => null);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            graph = vertices.ToDelegateUndirectedGraph(
                vertex =>
                {
                    if (vertex == 1 || vertex == 2)
                        return new[] { edge12, edge21 };
                    return null;
                });
            AssertEmptyGraph(graph);

            vertices.Add(1);
            AssertHasVertices(graph, new[] { 1 });
            AssertNoEdge(graph);    // Vertex 2 is not in graph, so edge is skipped

            vertices.Add(2);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });
            AssertHasAdjacentEdges(graph, 1, new[] { edge12, edge21 });
            AssertHasAdjacentEdges(graph, 2, new[] { edge12, edge21 });

            // Graph can evolve based on the delegate
            vertices.Add(3);
            var edge23 = new Edge<int>(2, 3);
            graph = vertices.ToDelegateUndirectedGraph(
                vertex =>
                {
                    if (vertex == 1)
                        return new[] { edge12, edge21 };

                    if (vertex == 2)
                        return new[] { edge12, edge21, edge23 };

                    if (vertex == 3)
                        return new[] { edge23 };

                    return null;
                });
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge21, edge23 });
            AssertHasAdjacentEdges(graph, 1, new[] { edge12, edge21 });
            AssertHasAdjacentEdges(graph, 2, new[] { edge12, edge21, edge23 });
            AssertHasAdjacentEdges(graph, 3, new[] { edge23 });
        }

        [Test]
        public void ToDelegateUndirectedGraph_GetDelegate_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToDelegateUndirectedGraph<int, Edge<int>>(vertex => null));
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToDelegateUndirectedGraph((Func<int, IEnumerable<Edge<int>>>)null));

            IEnumerable<int> vertices = Enumerable.Empty<int>();
            Assert.Throws<ArgumentNullException>(
                () => vertices.ToDelegateUndirectedGraph((Func<int, IEnumerable<Edge<int>>>)null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Graphs

        [Test]
        public void ToAdjacencyGraph_EdgeArray()
        {
            int[][] edges = { new int[] { }, new int[] { } };
            AdjacencyGraph<int, SEquatableEdge<int>> graph = edges.ToAdjacencyGraph();
            AssertEmptyGraph(graph);

            edges = new[]
            {
                new[] {1, 2, 3},
                new[] {2, 3, 1}
            };
            graph = edges.ToAdjacencyGraph();
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(
                graph, 
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(2, 3),
                    new SEquatableEdge<int>(3, 1)
                });
        }

        [Test]
        public void ToAdjacencyGraph_EdgeArray_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            int[][] edges = null;
            Assert.Throws<ArgumentNullException>(() => edges.ToAdjacencyGraph());

            edges = new int[][]{ };
            Assert.Throws<ArgumentException>(() => edges.ToAdjacencyGraph());
            edges = new[] { new int[]{ } };
            Assert.Throws<ArgumentException>(() => edges.ToAdjacencyGraph());
            edges = new[] { new int[] { }, new int[] { }, new int[] { } };
            Assert.Throws<ArgumentException>(() => edges.ToAdjacencyGraph());

            edges = new[] { new int[] { }, null };
            Assert.Throws<ArgumentNullException>(() => edges.ToAdjacencyGraph());
            edges = new[] { null, new int[] { } };
            Assert.Throws<ArgumentNullException>(() => edges.ToAdjacencyGraph());
            edges = new int[][] { null, null };
            Assert.Throws<ArgumentNullException>(() => edges.ToAdjacencyGraph());

            edges = new[] { new int[] { }, new [] { 1 } };
            Assert.Throws<ArgumentException>(() => edges.ToAdjacencyGraph());
            edges = new[] { new[] { 1 }, new int[] { } };
            Assert.Throws<ArgumentException>(() => edges.ToAdjacencyGraph());
            edges = new[] { new[] { 1, 2 }, new[] { 1 } };
            Assert.Throws<ArgumentException>(() => edges.ToAdjacencyGraph());
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ToAdjacencyGraph_EdgeSet()
        {
            var edges = new List<Edge<int>>();
            AdjacencyGraph<int, Edge<int>> graph = edges.ToAdjacencyGraph<int, Edge<int>>();
            AssertEmptyGraph(graph);

            graph = edges.ToAdjacencyGraph<int, Edge<int>>(false);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            edges.AddRange(new[] { edge12, edge21 });
            graph = edges.ToAdjacencyGraph<int, Edge<int>>();
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });

            // Graph cannot dynamically evolve
            var edge12Bis = new Edge<int>(1, 2);
            edges.Add(edge12Bis);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });

            graph = edges.ToAdjacencyGraph<int, Edge<int>>();
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge12Bis, edge21 });

            graph = edges.ToAdjacencyGraph<int, Edge<int>>(false);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });
        }

        [Test]
        public void ToAdjacencyGraph_EdgeSet_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<Edge<int>>)null).ToAdjacencyGraph<int, Edge<int>>());
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<Edge<int>>)null).ToAdjacencyGraph<int, Edge<int>>(false));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ToAdjacencyGraph_VertexPairs()
        {
            var vertices = new List<SEquatableEdge<int>>();
            AdjacencyGraph<int, SEquatableEdge<int>> graph = vertices.ToAdjacencyGraph();
            AssertEmptyGraph(graph);

            var edge12 = new SEquatableEdge<int>(1, 2);
            var edge23 = new SEquatableEdge<int>(2, 3);
            vertices.AddRange(new[] { edge12, edge23 });
            graph = vertices.ToAdjacencyGraph();
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge23 });
        }

        [Test]
        public void ToAdjacencyGraph_VertexPairs_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<SEquatableEdge<int>>)null).ToAdjacencyGraph());
        }

        [Test]
        public void ToAdjacencyGraph_EdgeSetWithFactory()
        {
            var vertices = new List<int>();
            AdjacencyGraph<int, Edge<int>> graph = vertices.ToAdjacencyGraph(
                vertex => Enumerable.Empty<Edge<int>>());
            AssertEmptyGraph(graph);

            graph = vertices.ToAdjacencyGraph(
                vertex => Enumerable.Empty<Edge<int>>(),
                false);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge12Bis = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            vertices.AddRange(new[] { 1, 2 });
            graph = vertices.ToAdjacencyGraph(
                vertex =>
                {
                    if (vertex == 1)
                        return new[] { edge12, edge12Bis };
                    if (vertex == 2)
                        return new[] { edge21 };
                    return Enumerable.Empty<Edge<int>>();
                });
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge12Bis, edge21 });

            graph = vertices.ToAdjacencyGraph(
                vertex =>
                {
                    if (vertex == 1)
                        return new[] { edge12, edge12Bis };
                    if (vertex == 2)
                        return new[] { edge21 };
                    return Enumerable.Empty<Edge<int>>();
                },
                false);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });
        }

        [Test]
        public void ToAdjacencyGraph_EdgeSetWithFactory_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToAdjacencyGraph(vertex => Enumerable.Empty<Edge<int>>()));
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToAdjacencyGraph<int, Edge<int>>(null));
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToAdjacencyGraph(vertex => Enumerable.Empty<Edge<int>>(), false));
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToAdjacencyGraph<int, Edge<int>>(null, false));

            var vertices = Enumerable.Empty<int>();
            Assert.Throws<ArgumentNullException>(
                () => vertices.ToAdjacencyGraph<int, Edge<int>>(null));
            Assert.Throws<ArgumentNullException>(
                () => vertices.ToAdjacencyGraph<int, Edge<int>>(null, false));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ToArrayAdjacencyGraph()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            ArrayAdjacencyGraph<int, Edge<int>> graph = wrappedGraph.ToArrayAdjacencyGraph();
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });
            graph = wrappedGraph.ToArrayAdjacencyGraph();
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge23 });
        }

        [Test]
        public void ToArrayAdjacencyGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((AdjacencyGraph<int, Edge<int>>)null).ToArrayAdjacencyGraph());
        }

        [Test]
        public void ToBidirectionalGraph_FromDirectedGraph()
        {
            var initialGraph1 = new AdjacencyGraph<int, Edge<int>>();
            IBidirectionalGraph<int, Edge<int>> graph = initialGraph1.ToBidirectionalGraph();
            AssertEmptyGraph(graph);

            var initialGraph2 = new BidirectionalGraph<int, Edge<int>>();
            graph = initialGraph2.ToBidirectionalGraph();
            AssertEmptyGraph(graph);
            Assert.AreSame(initialGraph2, graph);

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            var edge23 = new Edge<int>(2, 3);

            // Graph can dynamically evolve but it will not work when dealing with in-edges
            // stuff when the initial is not a bidirectional graph
            initialGraph1.AddVerticesAndEdgeRange(new[] { edge12, edge21 });
            initialGraph1.AddVertex(3);
            graph = initialGraph1.ToBidirectionalGraph();
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge21 });
            AssertNoInEdge(graph, 3);

            initialGraph1.AddVerticesAndEdge(edge23);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge21, edge23 });
            AssertNoInEdge(graph, 3);


            initialGraph2.AddVerticesAndEdgeRange(new[] { edge12, edge21 });
            initialGraph2.AddVertex(3);
            graph = initialGraph2.ToBidirectionalGraph();
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge21 });
            AssertNoInEdge(graph, 3);

            initialGraph2.AddVerticesAndEdge(edge23);
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge21, edge23 });
            AssertHasInEdges(graph, 3, new[] { edge23 });
        }

        [Test]
        public void ToBidirectionalGraph_FromDirectedGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<int, Edge<int>>)null).ToBidirectionalGraph());
        }

        [Test]
        public void ToBidirectionalGraph_EdgeSet()
        {
            var edges = new List<Edge<int>>();
            BidirectionalGraph<int, Edge<int>> graph = edges.ToBidirectionalGraph<int, Edge<int>>();
            AssertEmptyGraph(graph);

            graph = edges.ToBidirectionalGraph<int, Edge<int>>(false);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            edges.AddRange(new[] { edge12, edge21 });
            graph = edges.ToBidirectionalGraph<int, Edge<int>>();
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });

            // Graph cannot dynamically evolve
            var edge12Bis = new Edge<int>(1, 2);
            edges.Add(edge12Bis);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });

            graph = edges.ToBidirectionalGraph<int, Edge<int>>();
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge12Bis, edge21 });

            graph = edges.ToBidirectionalGraph<int, Edge<int>>(false);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });
        }

        [Test]
        public void ToBidirectionalGraph_EdgeSet_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<Edge<int>>)null).ToBidirectionalGraph<int, Edge<int>>());
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<Edge<int>>)null).ToBidirectionalGraph<int, Edge<int>>(false));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ToBidirectionalGraph_VertexPairs()
        {
            var vertices = new List<SEquatableEdge<int>>();
            BidirectionalGraph<int, SEquatableEdge<int>> graph = vertices.ToBidirectionalGraph();
            AssertEmptyGraph(graph);

            var edge12 = new SEquatableEdge<int>(1, 2);
            var edge23 = new SEquatableEdge<int>(2, 3);
            vertices.AddRange(new[] { edge12, edge23 });
            graph = vertices.ToBidirectionalGraph();
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge23 });
        }

        [Test]
        public void ToBidirectionalGraph_VertexPairs_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<SEquatableEdge<int>>)null).ToBidirectionalGraph());
        }

        [Test]
        public void ToBidirectionalGraph_EdgeSetWithFactory()
        {
            var vertices = new List<int>();
            BidirectionalGraph<int, Edge<int>> graph = vertices.ToBidirectionalGraph(
                vertex => Enumerable.Empty<Edge<int>>());
            AssertEmptyGraph(graph);

            graph = vertices.ToBidirectionalGraph(
                vertex => Enumerable.Empty<Edge<int>>(),
                false);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge12Bis = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            vertices.AddRange(new[]{ 1, 2 });
            graph = vertices.ToBidirectionalGraph(
                vertex =>
                {
                    if (vertex == 1)
                        return new[] { edge12, edge12Bis };
                    if (vertex == 2)
                        return new[] { edge21 };
                    return Enumerable.Empty<Edge<int>>();
                });
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge12Bis, edge21 });

            graph = vertices.ToBidirectionalGraph(
                vertex =>
                {
                    if (vertex == 1)
                        return new[] { edge12, edge12Bis };
                    if (vertex == 2)
                        return new[] { edge21 };
                    return Enumerable.Empty<Edge<int>>();
                },
                false);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });
        }

        [Test]
        public void ToBidirectionalGraph_EdgeSetWithFactory_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToBidirectionalGraph(vertex => Enumerable.Empty<Edge<int>>()));
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToBidirectionalGraph<int, Edge<int>>(null));
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToBidirectionalGraph(vertex => Enumerable.Empty<Edge<int>>(), false));
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<int>)null).ToBidirectionalGraph<int, Edge<int>>(null, false));

            var vertices = Enumerable.Empty<int>();
            Assert.Throws<ArgumentNullException>(
                () => vertices.ToBidirectionalGraph<int, Edge<int>>(null));
            Assert.Throws<ArgumentNullException>(
                () => vertices.ToBidirectionalGraph<int, Edge<int>>(null, false));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ToBidirectionalGraph_FromUndirectedGraph()
        {
            var initialGraph = new UndirectedGraph<int, Edge<int>>();
            BidirectionalGraph<int, Edge<int>> graph = initialGraph.ToBidirectionalGraph();
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            var edge23 = new Edge<int>(2, 3);

            // Graph cannot dynamically evolve
            initialGraph.AddVerticesAndEdgeRange(new[] { edge12, edge21 });
            initialGraph.AddVertex(3);
            graph = initialGraph.ToBidirectionalGraph();
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge21 });
            AssertNoInEdge(graph, 3);

            initialGraph.AddVerticesAndEdge(edge23);
            initialGraph.AddVertex(4);
            AssertHasVertices(graph, new[] { 1, 2, 3 });  // Not added
            AssertHasEdges(graph, new[] { edge12, edge21 }); // Not added
            AssertNoInEdge(graph, 3);                       // Not added
        }

        [Test]
        public void ToBidirectionalGraph_FromUndirectedGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IUndirectedGraph<int, Edge<int>>)null).ToBidirectionalGraph());
        }

        [Test]
        public void ToArrayBidirectionalGraph()
        {
            var wrappedGraph = new BidirectionalGraph<int, Edge<int>>();
            ArrayBidirectionalGraph<int, Edge<int>> graph = wrappedGraph.ToArrayBidirectionalGraph();
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });
            graph = wrappedGraph.ToArrayBidirectionalGraph();
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge23 });
        }

        [Test]
        public void ToArrayBidirectionalGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((BidirectionalGraph<int, Edge<int>>)null).ToArrayBidirectionalGraph());
        }

        [Test]
        public void ToUndirectedGraph()
        {
            var edges = new List<Edge<int>>();
            UndirectedGraph<int, Edge<int>> graph = edges.ToUndirectedGraph<int, Edge<int>>();
            AssertEmptyGraph(graph);

            graph = edges.ToUndirectedGraph<int, Edge<int>>(false);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            edges.AddRange(new[] { edge12, edge21 });
            graph = edges.ToUndirectedGraph<int, Edge<int>>();
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });

            // Graph cannot dynamically evolve
            var edge12Bis = new Edge<int>(1, 2);
            edges.Add(edge12Bis);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge21 });

            graph = edges.ToUndirectedGraph<int, Edge<int>>();
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12, edge12Bis, edge21 });

            graph = edges.ToUndirectedGraph<int, Edge<int>>(false);
            AssertHasVertices(graph, new[] { 1, 2 });
            AssertHasEdges(graph, new[] { edge12 });
        }

        [Test]
        public void ToUndirectedGraph_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<Edge<int>>)null).ToUndirectedGraph<int, Edge<int>>());
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<Edge<int>>)null).ToUndirectedGraph<int, Edge<int>>(false));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ToUndirectedGraph_VertexPairs()
        {
            var vertices = new List<SEquatableEdge<int>>();
            UndirectedGraph<int, SEquatableEdge<int>> graph = vertices.ToUndirectedGraph();
            AssertEmptyGraph(graph);

            var edge12 = new SEquatableEdge<int>(1, 2);
            var edge23 = new SEquatableEdge<int>(2, 3);
            vertices.AddRange(new[] { edge12, edge23 });
            graph = vertices.ToUndirectedGraph();
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge23 });
        }

        [Test]
        public void ToUndirectedGraph_VertexPairs_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IEnumerable<SEquatableEdge<int>>)null).ToUndirectedGraph());
        }

        [Test]
        public void ToArrayUndirectedGraph()
        {
            var wrappedGraph = new UndirectedGraph<int, Edge<int>>();
            ArrayUndirectedGraph<int, Edge<int>> graph = wrappedGraph.ToArrayUndirectedGraph();
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });
            graph = wrappedGraph.ToArrayUndirectedGraph();
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(graph, new[] { edge12, edge23 });
        }

        [Test]
        public void ToArrayUndirectedGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((UndirectedGraph<int, Edge<int>>)null).ToArrayUndirectedGraph());
        }

        [Test]
        public void ToCompressedRowGraph()
        {
            var wrappedGraph = new AdjacencyGraph<int, Edge<int>>();
            CompressedSparseRowGraph<int> graph = wrappedGraph.ToCompressedRowGraph();
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            wrappedGraph.AddVerticesAndEdgeRange(new[] { edge12, edge23 });
            graph = wrappedGraph.ToCompressedRowGraph();
            AssertHasVertices(graph, new[] { 1, 2, 3 });
            AssertHasEdges(
                graph, 
                new[]
                {
                    new SEquatableEdge<int>(1, 2),
                    new SEquatableEdge<int>(2, 3)
                });
        }

        [Test]
        public void ToCompressedRowGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((AdjacencyGraph<int, Edge<int>>)null).ToCompressedRowGraph());
        }

        #endregion
    }
}