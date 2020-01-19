using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.MaximumFlow;
using QuikGraph.Algorithms.RandomWalks;
using QuikGraph.Algorithms.TopologicalSort;
using QuikGraph.Collections;
using QuikGraph.Tests.Structures;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Extensions
{
    /// <summary>
    /// Tests related to <see cref="AlgorithmExtensions"/>.
    /// </summary>
    internal class AlgorithmExtensionsTests : GraphTestsBase
    {
        [Test]
        public void GetIndexer()
        {
            var dictionary1 = new Dictionary<int, double>();
            Func<int, double> indexer1 = AlgorithmExtensions.GetIndexer(dictionary1);

            Assert.Throws<KeyNotFoundException>(() => indexer1(12));

            dictionary1[12] = 42.0;
            Assert.AreEqual(42.0, indexer1(12));

            var dictionary2 = new Dictionary<TestVertex, TestVertex>();
            Func<TestVertex, TestVertex> indexer2 = AlgorithmExtensions.GetIndexer(dictionary2);

            var key = new TestVertex("1");
            var keyBis = new TestVertex("1");
            Assert.Throws<KeyNotFoundException>(() => indexer2(key));

            var value = new TestVertex("2");
            dictionary2[key] = value;
            Assert.AreSame(value, indexer2(key));

            Assert.Throws<KeyNotFoundException>(() => indexer2(keyBis));
        }

        [Test]
        public void GetIndexer_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => AlgorithmExtensions.GetIndexer<int, double>(null));
        }

        [Test]
        public void GetVertexIdentity()
        {
            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            VertexIdentity<int> vertexIdentity1 = AlgorithmExtensions.GetVertexIdentity(graph1);

            Assert.AreEqual("12", vertexIdentity1(12));
            Assert.AreEqual("42", vertexIdentity1(42));
            // Check identity didn't change
            Assert.AreEqual("12", vertexIdentity1(12));
            Assert.AreEqual("42", vertexIdentity1(42));

            var graph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            VertexIdentity<TestVertex> vertexIdentity2 = AlgorithmExtensions.GetVertexIdentity(graph2);

            var vertex1 = new TestVertex("12");
            var vertex2 = new TestVertex("42");
            Assert.AreEqual("0", vertexIdentity2(vertex1));
            Assert.AreEqual("1", vertexIdentity2(vertex2));
            // Check identity didn't change
            Assert.AreEqual("0", vertexIdentity2(vertex1));
            Assert.AreEqual("1", vertexIdentity2(vertex2));
        }

        [Test]
        public void GetVertexIdentity_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => AlgorithmExtensions.GetVertexIdentity<int>(null));
        }

        [Test]
        public void GetEdgeIdentity()
        {
            var graph1 = new AdjacencyGraph<int, Edge<int>>();
            EdgeIdentity<int, Edge<int>> edgeIdentity1 = AlgorithmExtensions.GetEdgeIdentity(graph1);

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(2, 3);
            var edge3 = new Edge<int>(1, 2);
            Assert.AreEqual("0", edgeIdentity1(edge1));
            Assert.AreEqual("1", edgeIdentity1(edge2));
            Assert.AreEqual("2", edgeIdentity1(edge3));
            // Check identity didn't change
            Assert.AreEqual("0", edgeIdentity1(edge1));
            Assert.AreEqual("1", edgeIdentity1(edge2));
            Assert.AreEqual("2", edgeIdentity1(edge3));

            var graph2 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            EdgeIdentity<TestVertex, Edge<TestVertex>> edgeIdentity2 = AlgorithmExtensions.GetEdgeIdentity(graph2);

            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var vertex3 = new TestVertex("3");
            var edge4 = new Edge<TestVertex>(vertex1, vertex2);
            var edge5 = new Edge<TestVertex>(vertex2, vertex3);
            var edge6 = new Edge<TestVertex>(vertex1, vertex2);
            Assert.AreEqual("0", edgeIdentity2(edge4));
            Assert.AreEqual("1", edgeIdentity2(edge5));
            Assert.AreEqual("2", edgeIdentity2(edge6));
            // Check identity didn't change
            Assert.AreEqual("0", edgeIdentity2(edge4));
            Assert.AreEqual("1", edgeIdentity2(edge5));
            Assert.AreEqual("2", edgeIdentity2(edge6));
        }

        [Test]
        public void GetEdgeIdentity_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => AlgorithmExtensions.GetEdgeIdentity<int, Edge<int>>(null));
            Assert.Throws<ArgumentNullException>(() => AlgorithmExtensions.GetEdgeIdentity<TestVertex, Edge<TestVertex>>(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void TreeBreadthFirstSearch()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge35 = new Edge<int>(3, 5);
            var edge36 = new Edge<int>(3, 6);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge24, edge35, edge36
            });
            graph.AddVertex(7);

            TryFunc<int, IEnumerable<Edge<int>>> pathAccessor = graph.TreeBreadthFirstSearch(1);

            Assert.IsFalse(pathAccessor(7, out _));

            Assert.IsTrue(pathAccessor(5, out IEnumerable<Edge<int>> path));
            CollectionAssert.AreEqual(new[] { edge13, edge35 }, path);
        }

        [Test]
        public void TreeBreadthFirstSearch_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).TreeBreadthFirstSearch(vertex));
            Assert.Throws<ArgumentNullException>(() => graph.TreeBreadthFirstSearch(null));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).TreeBreadthFirstSearch(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void TreeDepthFirstSearch()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge35 = new Edge<int>(3, 5);
            var edge36 = new Edge<int>(3, 6);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge23, edge24, edge35, edge36
            });
            graph.AddVertex(7);

            TryFunc<int, IEnumerable<Edge<int>>> pathAccessor = graph.TreeDepthFirstSearch(1);

            Assert.IsFalse(pathAccessor(7, out _));

            Assert.IsTrue(pathAccessor(5, out IEnumerable<Edge<int>> path));
            CollectionAssert.AreEqual(new[] { edge12, edge23, edge35 }, path);
        }

        [Test]
        public void TreeDepthFirstSearch_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).TreeDepthFirstSearch(vertex));
            Assert.Throws<ArgumentNullException>(() => graph.TreeDepthFirstSearch(null));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).TreeDepthFirstSearch(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void TreeCyclePoppingRandom()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 3);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 2);
            var edge7 = new Edge<int>(3, 5);
            var edge8 = new Edge<int>(3, 6);
            var edge9 = new Edge<int>(4, 1);
            var edge10 = new Edge<int>(4, 2);
            var edge11 = new Edge<int>(4, 5);
            var edge12 = new Edge<int>(4, 6);
            var edge13 = new Edge<int>(5, 6);
            var edge14 = new Edge<int>(6, 2);
            var edge15 = new Edge<int>(6, 3);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge1, edge2, edge3, edge4, edge5, edge6,
                edge7, edge8, edge9, edge10, edge11,
                edge12, edge13, edge14, edge15
            });
            graph.AddVertex(7);

            TryFunc<int, IEnumerable<Edge<int>>> pathAccessor = graph.TreeCyclePoppingRandom(2);

            Assert.IsFalse(pathAccessor(7, out _));

            // Would require more tests...
        }

        [Test]
        public void TreeCyclePoppingRandom_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            var chain = new NormalizedMarkovEdgeChain<TestVertex, Edge<TestVertex>>();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).TreeCyclePoppingRandom(vertex));
            Assert.Throws<ArgumentNullException>(() => graph.TreeCyclePoppingRandom(null));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).TreeCyclePoppingRandom(null));

            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).TreeCyclePoppingRandom(vertex, chain));
            Assert.Throws<ArgumentNullException>(() => graph.TreeCyclePoppingRandom(null, chain));
            Assert.Throws<ArgumentNullException>(() => graph.TreeCyclePoppingRandom(vertex, null));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).TreeCyclePoppingRandom(null, chain));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).TreeCyclePoppingRandom(vertex, null));
            Assert.Throws<ArgumentNullException>(() => graph.TreeCyclePoppingRandom(null, null));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).TreeCyclePoppingRandom(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #region Shortest paths

        [Test]
        public void ShortestPaths_Dijkstra_AStar_BellmanFord_Dag()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge18 = new Edge<int>(1, 8);
            var edge24 = new Edge<int>(2, 4);
            var edge25 = new Edge<int>(2, 5);
            var edge26 = new Edge<int>(2, 6);
            var edge34 = new Edge<int>(3, 4);
            var edge45 = new Edge<int>(4, 5);
            var edge46 = new Edge<int>(4, 6);
            var edge56 = new Edge<int>(5, 6);
            var edge67 = new Edge<int>(6, 7);
            var edge810 = new Edge<int>(8, 10);
            var edge95 = new Edge<int>(9, 5);
            var edge109 = new Edge<int>(10, 9);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge18, edge24, edge25,
                edge26, edge34, edge45, edge46, edge56,
                edge67, edge810, edge95, edge109
            });

            TryFunc<int, IEnumerable<Edge<int>>>[] algorithmResults = 
            {
                graph.ShortestPathsDijkstra(edge => 1.0, 2),
                graph.ShortestPathsAStar(edge => 1.0, vertex => 1.0, 2),
                graph.ShortestPathsBellmanFord(edge => 1.0, 2, out _),
                graph.ShortestPathsDag(edge => 1.0, 2)
            };

            foreach (TryFunc<int, IEnumerable<Edge<int>>> result in algorithmResults)
            {
                CheckResult(result);
            }

            #region Local function

            void CheckResult(TryFunc<int, IEnumerable<Edge<int>>> pathAccessor)
            {
                Assert.IsNotNull(pathAccessor);

                Assert.IsFalse(pathAccessor(1, out _));

                Assert.IsTrue(pathAccessor(7, out IEnumerable<Edge<int>> path));
                CollectionAssert.AreEqual(new[] { edge26, edge67 }, path);

                Assert.IsTrue(pathAccessor(4, out path));
                CollectionAssert.AreEqual(new[] { edge24 }, path);
            }

            #endregion
        }

        [Test]
        public void ShortestPaths_BellmanFord_NegativeCycle()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge24 = new Edge<int>(2, 4);
            var edge41 = new Edge<int>(4, 1);
            
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge24, edge41
            });

            TryFunc<int, IEnumerable<Edge<int>>> pathAccessor = graph.ShortestPathsBellmanFord(
                edge =>
                {
                    if (edge == edge12)
                        return 12.0;
                    if (edge == edge24)
                        return -42.0;
                    if (edge == edge41)
                        return 22.0;
                    return 1.0;
                },
                1,
                out bool foundNegativeCycle);
            Assert.IsNotNull(pathAccessor);
            Assert.IsTrue(foundNegativeCycle);

            // Path accessors is usable but will generate a stack overflow
            // if accessing path using edge in the negative cycle.
        }

        [Test]
        public void ShortestPathsDijkstra_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDijkstra(edge => 1.0, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsDijkstra(null, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsDijkstra(edge => 1.0, null));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDijkstra(null, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsDijkstra(null, null));
            Assert.Throws<ArgumentNullException>(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDijkstra(edge => 1.0, null));
            Assert.Throws<ArgumentNullException>(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDijkstra(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ShortestPathsAStar_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsAStar(edge => 1.0, v => 1.0, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsAStar(null, v => 1.0, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsAStar(edge => 1.0, null, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsAStar(edge => 1.0, v => 1.0, null));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsAStar(null, v => 1.0, vertex));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsAStar(edge => 1.0, null, vertex));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsAStar(edge => 1.0, v => 1.0, null));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsAStar(null, null, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsAStar(null, v => 1.0, null));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsAStar(edge => 1.0, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsAStar(null, null, vertex));
            Assert.Throws<ArgumentNullException>(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsAStar(null, v => 1.0, null));
            Assert.Throws<ArgumentNullException>(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsAStar(edge => 1.0, null, null));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsAStar(null, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsAStar(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ShortestPathsBellmanFord_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsBellmanFord(edge => 1.0, vertex, out _));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsBellmanFord(null, vertex, out _));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsBellmanFord(edge => 1.0, null, out _));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsBellmanFord(null, vertex, out _));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsBellmanFord(null, null, out _));
            Assert.Throws<ArgumentNullException>(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsBellmanFord(edge => 1.0, null, out _));
            Assert.Throws<ArgumentNullException>(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsBellmanFord(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ShortestPathsDag_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDag(edge => 1.0, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsDag(null, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsDag(edge => 1.0, null));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDag(null, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsDag(null, null));
            Assert.Throws<ArgumentNullException>(() => 
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDag(edge => 1.0, null));
            Assert.Throws<ArgumentNullException>(() =>
                ((IVertexAndEdgeListGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDag(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ShortestPaths_UndirectedDijkstra()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge18 = new Edge<int>(1, 8);
            var edge45 = new Edge<int>(4, 5);
            var edge46 = new Edge<int>(4, 6);
            var edge56 = new Edge<int>(5, 6);
            var edge67 = new Edge<int>(6, 7);
            var edge810 = new Edge<int>(8, 10);

            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge18, edge45,
                edge46, edge56, edge67, edge810
            });
            graph.AddVertex(9);

            TryFunc<int, IEnumerable<Edge<int>>> pathAccessor = graph.ShortestPathsDijkstra(edge => 1.0, 2);
            Assert.IsNotNull(pathAccessor);

            Assert.IsFalse(pathAccessor(9, out _));

            Assert.IsTrue(pathAccessor(8, out IEnumerable<Edge<int>> path));
            CollectionAssert.AreEqual(new[] { edge12, edge18 }, path);

            Assert.IsTrue(pathAccessor(1, out path));
            CollectionAssert.AreEqual(new[] { edge12 }, path);
        }

        [Test]
        public void ShortestPathsUndirectedDijkstra_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IUndirectedGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDijkstra(edge => 1.0, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsDijkstra(null, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsDijkstra(edge => 1.0, null));
            Assert.Throws<ArgumentNullException>(
                () => ((IUndirectedGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDijkstra(null, vertex));
            Assert.Throws<ArgumentNullException>(() => graph.ShortestPathsDijkstra(null, null));
            Assert.Throws<ArgumentNullException>(() =>
                ((IUndirectedGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDijkstra(edge => 1.0, null));
            Assert.Throws<ArgumentNullException>(() =>
                ((IUndirectedGraph<TestVertex, Edge<TestVertex>>)null).ShortestPathsDijkstra(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region K-Shortest path

        [Test]
        public void RankedShortestPathHoffmanPavley()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge18 = new Edge<int>(1, 8);
            var edge21 = new Edge<int>(2, 1);
            var edge24 = new Edge<int>(2, 4);
            var edge25 = new Edge<int>(2, 5);
            var edge26 = new Edge<int>(2, 6);
            var edge33 = new Edge<int>(3, 3);
            var edge34 = new Edge<int>(3, 4);
            var edge45 = new Edge<int>(4, 5);
            var edge46 = new Edge<int>(4, 6);
            var edge56 = new Edge<int>(5, 6);
            var edge67 = new Edge<int>(6, 7);
            var edge810 = new Edge<int>(8, 10);
            var edge95 = new Edge<int>(9, 5);
            var edge109 = new Edge<int>(10, 9);

            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge13, edge18, edge21, edge24,
                edge25, edge26, edge33, edge34, edge45,
                edge46, edge56, edge67, edge810, edge95,
                edge109
            });

            IEnumerable<IEnumerable<Edge<int>>> paths = graph.RankedShortestPathHoffmanPavley(edge => 1.0, 1, 5, 5);
            CollectionAssert.AreEqual(
                new[]
                {
                    new[] { edge12, edge25 },
                    new[] { edge13, edge34, edge45 },
                    new[] { edge12, edge24, edge45 },
                    new[] { edge18, edge810, edge109, edge95 }
                },
                paths);

            paths = graph.RankedShortestPathHoffmanPavley(edge => 1.0, 1, 5);
            CollectionAssert.AreEqual(
                new[]
                {
                    new[] { edge12, edge25 },
                    new[] { edge13, edge34, edge45 },
                    new[] { edge12, edge24, edge45 }
                },
                paths);
        }

        [Test]
        public void RankedShortestPathHoffmanPavley_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();

            var vertex = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>)null).RankedShortestPathHoffmanPavley(edge => 1.0, vertex, vertex, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => graph.RankedShortestPathHoffmanPavley(null, vertex, vertex, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => graph.RankedShortestPathHoffmanPavley(edge => 1.0, null, vertex, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => graph.RankedShortestPathHoffmanPavley(edge => 1.0, vertex, null, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>)null).RankedShortestPathHoffmanPavley(null, vertex, vertex, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>)null).RankedShortestPathHoffmanPavley(edge => 1.0, null, vertex, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>)null).RankedShortestPathHoffmanPavley(edge => 1.0, vertex, null, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => graph.RankedShortestPathHoffmanPavley(null, null, vertex, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => graph.RankedShortestPathHoffmanPavley(null, vertex, null, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => graph.RankedShortestPathHoffmanPavley(edge => 1.0, null, null, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>)null).RankedShortestPathHoffmanPavley(null, null, vertex, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>)null).RankedShortestPathHoffmanPavley(null, vertex, null, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => graph.RankedShortestPathHoffmanPavley(null, null, null, int.MaxValue));
            Assert.Throws<ArgumentNullException>(
                () => ((IBidirectionalGraph<TestVertex, Edge<TestVertex>>)null).RankedShortestPathHoffmanPavley(null, null, null, int.MaxValue));
            // ReSharper restore AssignNullToNotNullAttribute

            Assert.Throws<ArgumentOutOfRangeException>(
                () => graph.RankedShortestPathHoffmanPavley(edge => 1.0, vertex, vertex, 0));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => graph.RankedShortestPathHoffmanPavley(edge => 1.0, vertex, vertex, -1));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        [Pure]
        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> CreateSinksTestCases(
            [NotNull, InstantHandle] Func<IMutableVertexAndEdgeSet<int, Edge<int>>> createGraph)
        {
            yield return new TestCaseData(
                createGraph(),
                Enumerable.Empty<int>());

            var edge12 = new Edge<int>(1, 2);
            var edge14 = new Edge<int>(1, 4);
            var edge22 = new Edge<int>(2, 2);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge25 = new Edge<int>(2, 5);
            var edge35 = new Edge<int>(3, 5);
            var edge41 = new Edge<int>(4, 1);
            var edge45 = new Edge<int>(4, 5);
            var edge46 = new Edge<int>(4, 6);

            IMutableVertexAndEdgeSet<int, Edge<int>> cycleGraph = createGraph();
            cycleGraph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge24, edge41
            });
            yield return new TestCaseData(
                cycleGraph,
                Enumerable.Empty<int>());

            IMutableVertexAndEdgeSet<int, Edge<int>> cycleGraph2 = createGraph();
            cycleGraph2.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge24, edge25, edge35, edge41, edge22
            });
            yield return new TestCaseData(
                cycleGraph2,
                new[] { 5 });

            IMutableVertexAndEdgeSet<int, Edge<int>> graph1 = createGraph();
            graph1.AddVerticesAndEdgeRange(new[]
            {
                edge22
            });
            yield return new TestCaseData(
                graph1,
                Enumerable.Empty<int>());

            IMutableVertexAndEdgeSet<int, Edge<int>> graph2 = createGraph();
            graph2.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge23, edge24, edge35, edge45
            });
            yield return new TestCaseData(
                graph2,
                new[] { 5 });

            IMutableVertexAndEdgeSet<int, Edge<int>> graph3 = createGraph();
            graph3.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge24, edge35, edge45, edge46
            });
            yield return new TestCaseData(
                graph3,
                new[] { 5, 6 });
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> SinksTestCases
        {
            [UsedImplicitly]
            get
            {
                IEnumerable<TestCaseData> testCases = CreateSinksTestCases(() => new AdjacencyGraph<int, Edge<int>>())
                    .Concat(CreateSinksTestCases(() => new BidirectionalGraph<int, Edge<int>>()));
                foreach (TestCaseData testCase in testCases)
                {
                    yield return testCase;
                }
            }
        }

        [TestCaseSource(nameof(SinksTestCases))]
        public void Sinks(
            [NotNull] IVertexListGraph<int, Edge<int>> graph,
            [NotNull] IEnumerable<int> expectedSinks)
        {
            CollectionAssert.AreEquivalent(expectedSinks, graph.Sinks());
        }

        [Test]
        public void Sinks_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexListGraph<int, Edge<int>>)null).Sinks().ToArray());
        }

        [Pure]
        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> CreateRootsTestCases(
            [NotNull, InstantHandle] Func<IMutableVertexAndEdgeSet<int, Edge<int>>> createGraph)
        {
            yield return new TestCaseData(
                createGraph(),
                Enumerable.Empty<int>());

            var edge12 = new Edge<int>(1, 2);
            var edge14 = new Edge<int>(1, 4);
            var edge22 = new Edge<int>(2, 2);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge25 = new Edge<int>(2, 5);
            var edge35 = new Edge<int>(3, 5);
            var edge41 = new Edge<int>(4, 1);
            var edge45 = new Edge<int>(4, 5);
            var edge46 = new Edge<int>(4, 6);

            IMutableVertexAndEdgeSet<int, Edge<int>> cycleGraph = createGraph();
            cycleGraph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge24, edge41
            });
            yield return new TestCaseData(
                cycleGraph,
                Enumerable.Empty<int>());

            IMutableVertexAndEdgeSet<int, Edge<int>> cycleGraph2 = createGraph();
            cycleGraph2.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge24, edge25, edge35, edge41, edge22
            });
            yield return new TestCaseData(
                cycleGraph2,
                new[] { 3 });

            IMutableVertexAndEdgeSet<int, Edge<int>> graph1 = createGraph();
            graph1.AddVerticesAndEdgeRange(new[]
            {
                edge22
            });
            yield return new TestCaseData(
                graph1,
                Enumerable.Empty<int>());

            IMutableVertexAndEdgeSet<int, Edge<int>> graph2 = createGraph();
            graph2.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge23, edge24, edge35, edge45
            });
            yield return new TestCaseData(
                graph2,
                new[] { 1 });

            IMutableVertexAndEdgeSet<int, Edge<int>> graph3 = createGraph();
            graph3.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge24, edge35, edge45, edge46
            });
            yield return new TestCaseData(
                graph3,
                new[] { 1, 3 });
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> RootsTestCases
        {
            [UsedImplicitly]
            get
            {
                return CreateRootsTestCases(() => new AdjacencyGraph<int, Edge<int>>());
            }
        }

        [TestCaseSource(nameof(RootsTestCases))]
        public void Roots_NotBidirectional(
            [NotNull] IVertexListGraph<int, Edge<int>> graph,
            [NotNull] IEnumerable<int> expectedRoots)
        {
            CollectionAssert.AreEquivalent(expectedRoots, graph.Roots());
        }

        [Test]
        public void AdjacencyGraphRoots()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                CheckRoots(graph);

            #region Local function

            void CheckRoots<T>(IVertexAndEdgeListGraph<T, Edge<T>> graph)
            {
                var roots = new HashSet<T>(graph.Roots());
                foreach (Edge<T> edge in graph.Edges)
                    Assert.IsFalse(roots.Contains(edge.Target));
            }

            #endregion
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> BidirectionalRootsTestCases
        {
            [UsedImplicitly]
            get
            {
                return CreateRootsTestCases(() => new BidirectionalGraph<int, Edge<int>>());
            }
        }

        [TestCaseSource(nameof(BidirectionalRootsTestCases))]
        public void Roots_Bidirectional(
            [NotNull] IBidirectionalGraph<int, Edge<int>> graph,
            [NotNull] IEnumerable<int> expectedRoots)
        {
            CollectionAssert.AreEquivalent(expectedRoots, graph.Roots());
        }

        [Test]
        public void Roots_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexListGraph<int, Edge<int>>)null).Roots().ToArray());
            Assert.Throws<ArgumentNullException>(
                () => ((IBidirectionalGraph<int, Edge<int>>)null).Roots().ToArray());
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> IsolatedVerticesTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(
                    new BidirectionalGraph<int, Edge<int>>(),
                    Enumerable.Empty<int>());

                var edge12 = new Edge<int>(1, 2);
                var edge14 = new Edge<int>(1, 4);
                var edge22 = new Edge<int>(2, 2);
                var edge23 = new Edge<int>(2, 3);
                var edge24 = new Edge<int>(2, 4);
                var edge26 = new Edge<int>(2, 6);
                var edge35 = new Edge<int>(3, 5);
                var edge36 = new Edge<int>(3, 6);
                var edge41 = new Edge<int>(4, 1);
                var edge45 = new Edge<int>(4, 5);
                var edge46 = new Edge<int>(4, 6);

                var cycleGraph = new BidirectionalGraph<int, Edge<int>>();
                cycleGraph.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge24, edge41
                });
                yield return new TestCaseData(
                    cycleGraph,
                    Enumerable.Empty<int>());

                var cycleGraph2 = new BidirectionalGraph<int, Edge<int>>();
                cycleGraph2.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge24, edge41, edge22
                });
                yield return new TestCaseData(
                    cycleGraph2,
                    Enumerable.Empty<int>());

                var cycleGraph3 = new BidirectionalGraph<int, Edge<int>>();
                cycleGraph3.AddVerticesAndEdgeRange(new[]
                {
                    edge22
                });
                yield return new TestCaseData(
                    cycleGraph3,
                    Enumerable.Empty<int>());

                var cycleGraph4 = new BidirectionalGraph<int, Edge<int>>();
                cycleGraph4.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge22, edge24, edge41
                });
                cycleGraph4.AddVertex(5);
                yield return new TestCaseData(
                    cycleGraph4,
                    new[] { 5 });

                var graph1 = new BidirectionalGraph<int, Edge<int>>();
                graph1.AddVertexRange(new[] { 4, 5 });
                graph1.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge23, edge26, edge36
                });
                yield return new TestCaseData(
                    graph1,
                    new[] { 4, 5 });

                var graph2 = new BidirectionalGraph<int, Edge<int>>();
                graph2.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge14, edge23, edge24, edge26, edge35, edge45, edge46
                });
                yield return new TestCaseData(
                    graph2,
                    Enumerable.Empty<int>());
            }
        }

        [TestCaseSource(nameof(IsolatedVerticesTestCases))]
        public void IsolatedVertices(
            [NotNull] IBidirectionalGraph<int, Edge<int>> graph,
            [NotNull] IEnumerable<int> expectedIsolatedVertices)
        {
            CollectionAssert.AreEquivalent(expectedIsolatedVertices, graph.IsolatedVertices());
        }

        [Test]
        public void IsolatedVertices_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((BidirectionalGraph<int, Edge<int>>)null).IsolatedVertices());
        }

        #region Topological sort

        [Test]
        public void TopologicalSort()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(3, 5),
                new Edge<int>(5, 7),
                new Edge<int>(6, 3),
                new Edge<int>(6, 7)
            });

            CollectionAssert.AreEqual(
                new[] { 6, 3, 5, 7, 1, 2, 4 },
                graph.TopologicalSort());
        }

        [Test]
        public void TopologicalSort_Undirected()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(3, 5),
                new Edge<int>(5, 7),
                new Edge<int>(6, 7)
            });

            CollectionAssert.AreEqual(
                new[] { 1, 3, 5, 7, 6, 2, 4 },
                graph.TopologicalSort());
        }

        [Test]
        public void TopologicalSort_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.TopologicalSort((IVertexListGraph<int, Edge<int>>) null));

            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.TopologicalSort((IUndirectedGraph<int, Edge<int>>)null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void SourceFirstTopologicalSort()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(3, 5),
                new Edge<int>(5, 7),
                new Edge<int>(6, 3),
                new Edge<int>(6, 7)
            });

            CollectionAssert.AreEqual(
                new[] { 6, 3, 1, 5, 2, 7, 4 },
                graph.SourceFirstTopologicalSort());
        }

        [Test]
        public void SourceFirstTopologicalSort_Undirected()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(3, 5),
                new Edge<int>(5, 7),
                new Edge<int>(6, 7)
            });

            CollectionAssert.AreEqual(
                new[] { 4, 6, 2, 7, 1, 5, 3 },
                graph.SourceFirstTopologicalSort());
        }

        [Test]
        public void SourceFirstTopologicalSort_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.SourceFirstTopologicalSort((IVertexAndEdgeListGraph<int, Edge<int>>)null));

            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.SourceFirstTopologicalSort((IUndirectedGraph<int, Edge<int>>)null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void SourceFirstBidirectionalTopologicalSort()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(3, 5),
                new Edge<int>(5, 7),
                new Edge<int>(6, 3),
                new Edge<int>(6, 7)
            });

            CollectionAssert.AreEqual(
                new[] { 6, 3, 1, 5, 2, 7, 4 },
                graph.SourceFirstBidirectionalTopologicalSort());

            CollectionAssert.AreEqual(
                new[] { 6, 3, 1, 5, 2, 7, 4 },
                graph.SourceFirstBidirectionalTopologicalSort(TopologicalSortDirection.Forward));

            CollectionAssert.AreEqual(
                new[] { 4, 7, 2, 5, 1, 3, 6 },
                graph.SourceFirstBidirectionalTopologicalSort(TopologicalSortDirection.Backward));
        }

        [Test]
        public void SourceFirstBidirectionalTopologicalSort_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.SourceFirstBidirectionalTopologicalSort((IBidirectionalGraph<int, Edge<int>>)null));

            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.SourceFirstBidirectionalTopologicalSort((IBidirectionalGraph<int, Edge<int>>)null, TopologicalSortDirection.Forward));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.SourceFirstBidirectionalTopologicalSort((IBidirectionalGraph<int, Edge<int>>)null, TopologicalSortDirection.Backward));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Connected components

        [Test]
        public void ConnectedComponents()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 4),
                new Edge<int>(2, 3),
                new Edge<int>(3, 1),
                new Edge<int>(4, 5),
                new Edge<int>(5, 6),
                new Edge<int>(6, 7),
                new Edge<int>(7, 5),

                new Edge<int>(8, 9)
            });

            var components = new Dictionary<int, int>();

            Assert.AreEqual(2, graph.ConnectedComponents(components));
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [1] = 0,
                    [2] = 0,
                    [3] = 0,
                    [4] = 0,
                    [5] = 0,
                    [6] = 0,
                    [7] = 0,
                    [8] = 1,
                    [9] = 1
                },
                components);
        }

        [Test]
        public void ConnectedComponents_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.ConnectedComponents(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.ConnectedComponents<int, Edge<int>>(null, components));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.ConnectedComponents<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void IncrementalConnectedComponent()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 0, 1, 2, 3 });
            using (graph.IncrementalConnectedComponents(
                out Func<KeyValuePair<int, IDictionary<int, int>>> getComponents))
            {
                KeyValuePair<int, IDictionary<int, int>> current = getComponents();
                Assert.AreEqual(4, current.Key);

                graph.AddEdge(new Edge<int>(0, 1));
                current = getComponents();
                Assert.AreEqual(3, current.Key);

                graph.AddEdge(new Edge<int>(2, 3));
                current = getComponents();
                Assert.AreEqual(2, current.Key);

                graph.AddEdge(new Edge<int>(1, 3));
                current = getComponents();
                Assert.AreEqual(1, current.Key);

                graph.AddVertex(4);
                current = getComponents();
                Assert.AreEqual(2, current.Key);
            }
        }

        [Test]
        public void IncrementalConnectedComponent_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.IncrementalConnectedComponents<int, Edge<int>>(null, out _));
        }

        [Test]
        public void StronglyConnectedComponents()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 4),
                new Edge<int>(2, 3),
                new Edge<int>(3, 1),
                new Edge<int>(4, 5),
                new Edge<int>(5, 6),
                new Edge<int>(6, 7),
                new Edge<int>(7, 5)
            });

            var components = new Dictionary<int, int>();

            Assert.AreEqual(3, graph.StronglyConnectedComponents(components));
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [1] = 2,
                    [2] = 2,
                    [3] = 2,
                    [4] = 1,
                    [5] = 0,
                    [6] = 0,
                    [7] = 0
                },
                components);
        }

        [Test]
        public void StronglyConnectedComponents_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.StronglyConnectedComponents(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.StronglyConnectedComponents<int, Edge<int>>(null, components));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.StronglyConnectedComponents<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void WeaklyConnectedComponents()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 4),
                new Edge<int>(2, 3),
                new Edge<int>(3, 1),
                new Edge<int>(4, 5),
                new Edge<int>(5, 6),
                new Edge<int>(6, 7),
                new Edge<int>(7, 5),
                
                new Edge<int>(8, 9)
            });

            var components = new Dictionary<int, int>();

            Assert.AreEqual(2, graph.WeaklyConnectedComponents(components));
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [1] = 0,
                    [2] = 0,
                    [3] = 0,
                    [4] = 0,
                    [5] = 0,
                    [6] = 0,
                    [7] = 0,
                    [8] = 1,
                    [9] = 1
                },
                components);
        }

        [Test]
        public void WeaklyConnectedComponents_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.WeaklyConnectedComponents(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.WeaklyConnectedComponents<int, Edge<int>>(null, components));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.WeaklyConnectedComponents<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void StronglyCondensedGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.CondensateStronglyConnected<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(null));
        }

        [Test]
        public void WeaklyCondensedGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.CondensateWeaklyConnected<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(null));
        }

        [Test]
        public void EdgesCondensedGraph_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.CondensateEdges((IBidirectionalGraph<int, Edge<int>>)null, v => true));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.CondensateEdges(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.CondensateEdges((IBidirectionalGraph<int, Edge<int>>)null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> OddVerticesTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(
                    new AdjacencyGraph<int, Edge<int>>(),
                    Enumerable.Empty<int>());

                var edge12 = new Edge<int>(1, 2);
                var edge14 = new Edge<int>(1, 4);
                var edge22 = new Edge<int>(2, 2);
                var edge23 = new Edge<int>(2, 3);
                var edge24 = new Edge<int>(2, 4);
                var edge25 = new Edge<int>(2, 5);
                var edge26 = new Edge<int>(2, 6);
                var edge35 = new Edge<int>(3, 5);
                var edge41 = new Edge<int>(4, 1);
                var edge45 = new Edge<int>(4, 5);
                var edge46 = new Edge<int>(4, 6);

                var cycleGraph = new AdjacencyGraph<int, Edge<int>>();
                cycleGraph.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge24, edge41
                });
                yield return new TestCaseData(
                    cycleGraph,
                    Enumerable.Empty<int>());

                var cycleGraph2 = new AdjacencyGraph<int, Edge<int>>();
                cycleGraph2.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge24, edge41, edge22
                });
                yield return new TestCaseData(
                    cycleGraph2,
                    Enumerable.Empty<int>());

                var cycleGraph3 = new AdjacencyGraph<int, Edge<int>>();
                cycleGraph3.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge24, edge25, edge35, edge41, edge22
                });
                yield return new TestCaseData(
                    cycleGraph3,
                    new[] { 2, 3 });

                var cycleGraph4 = new AdjacencyGraph<int, Edge<int>>();
                cycleGraph4.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge22, edge24, edge25, edge35, edge41, edge45
                });
                yield return new TestCaseData(
                    cycleGraph4,
                    new[] { 2, 3, 4, 5 });

                var graph1 = new AdjacencyGraph<int, Edge<int>>();
                graph1.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge14, edge23, edge24, edge35, edge45
                });
                yield return new TestCaseData(
                    graph1,
                    new[] { 2, 4 });

                var graph2 = new AdjacencyGraph<int, Edge<int>>();
                graph2.AddVerticesAndEdgeRange(new[]
                {
                    edge12, edge14, edge23, edge24, edge26, edge35, edge45, edge46
                });
                yield return new TestCaseData(
                    graph2,
                    Enumerable.Empty<int>());
            }
        }

        [TestCaseSource(nameof(OddVerticesTestCases))]
        public void OddVertices(
            [NotNull] IVertexAndEdgeListGraph<int, Edge<int>> graph,
            [NotNull] IEnumerable<int> expectedOddVertices)
        {
            CollectionAssert.AreEquivalent(expectedOddVertices, graph.OddVertices());
        }

        [Test]
        public void OddVertices_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((AdjacencyGraph<int, Edge<int>>)null).OddVertices());
        }

        [Pure]
        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> CreateIsDirectedAcyclicGraphTestCases(
            [NotNull, InstantHandle] Func<IMutableVertexAndEdgeSet<int, Edge<int>>> createGraph)
        {
            // Empty graph
            yield return new TestCaseData(createGraph())
            {
                ExpectedResult = true
            };

            var edge12 = new Edge<int>(1, 2);
            var edge14 = new Edge<int>(1, 4);
            var edge23 = new Edge<int>(2, 3);
            var edge24 = new Edge<int>(2, 4);

            // Not empty acyclic
            var adjacencyGraph = createGraph();
            adjacencyGraph.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge23, edge24
            });
            yield return new TestCaseData(adjacencyGraph)
            {
                ExpectedResult = true
            };

            // Not acyclic
            var edge22 = new Edge<int>(2, 2);
            var cyclicGraph1 = createGraph();
            cyclicGraph1.AddVerticesAndEdge(edge22);
            yield return new TestCaseData(cyclicGraph1)
            {
                ExpectedResult = false
            };

            var cyclicGraph2 = createGraph();
            cyclicGraph2.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge22, edge23, edge24
            });
            yield return new TestCaseData(cyclicGraph2)
            {
                ExpectedResult = false
            };

            var edge41 = new Edge<int>(4, 1);
            var cyclicGraph3 = createGraph();
            cyclicGraph3.AddVerticesAndEdgeRange(new[]
            {
                edge12, edge14, edge23, edge24, edge41
            });
            yield return new TestCaseData(cyclicGraph3)
            {
                ExpectedResult = false
            };
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> IsDirectedAcyclicGraphTestCases
        {
            [UsedImplicitly]
            get
            {
                IEnumerable<TestCaseData> testCases = CreateIsDirectedAcyclicGraphTestCases(() => new AdjacencyGraph<int, Edge<int>>())
                    .Concat(CreateIsDirectedAcyclicGraphTestCases(() => new BidirectionalGraph<int, Edge<int>>()));
                foreach (TestCaseData testCase in testCases)
                {
                    yield return testCase;
                }
            }
        }

        [TestCaseSource(nameof(IsDirectedAcyclicGraphTestCases))]
        public bool IsDirectedAcyclicGraph([NotNull] IVertexListGraph<int, Edge<int>> graph)
        {
            return graph.IsDirectedAcyclicGraph();
        }

        [Test]
        public void IsDirectedAcyclicGraph_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((AdjacencyGraph<int, Edge<int>>)null).IsDirectedAcyclicGraph());
        }

        [Test]
        public void ComputePredecessorCost()
        {
            var predecessors = new Dictionary<int, Edge<int>>();
            var edgeCosts = new Dictionary<Edge<int>, double>();

            Assert.AreEqual(0, AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 1));

            var edge12 = new Edge<int>(1, 2);
            predecessors[2] = edge12;
            edgeCosts[edge12] = 12;
            Assert.AreEqual(0, AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 1));
            Assert.AreEqual(12, AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 2));

            var edge31 = new Edge<int>(3, 1);
            predecessors[1] = edge31;
            edgeCosts[edge31] = -5;
            var edge34 = new Edge<int>(3, 4);
            predecessors[4] = edge34;
            edgeCosts[edge34] = 42;

            Assert.AreEqual(-5, AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 1));
            Assert.AreEqual(7, AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 2));
            Assert.AreEqual(0, AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 3));
            Assert.AreEqual(42, AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, 4));
        }

        [Test]
        public void ComputePredecessorCost_Throws()
        {
            var predecessors = new Dictionary<TestVertex, Edge<TestVertex>>();
            var edgeCosts = new Dictionary<Edge<TestVertex>, double>();
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.ComputePredecessorCost(null, edgeCosts, vertex1));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.ComputePredecessorCost(predecessors, null, vertex1));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.ComputePredecessorCost<TestVertex, Edge<TestVertex>>(null, null, vertex1));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.ComputePredecessorCost(predecessors, null, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.ComputePredecessorCost<TestVertex, Edge<TestVertex>>(null, null, null));

            // Wrong usage
            predecessors[vertex2] = new Edge<TestVertex>(vertex1, vertex2);
            Assert.Throws<KeyNotFoundException>(
                () => AlgorithmExtensions.ComputePredecessorCost(predecessors, edgeCosts, vertex2));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ComputeDisjointSet()
        {
            var emptyGraph = new UndirectedGraph<int, Edge<int>>();
            IDisjointSet<int> disjointSet = emptyGraph.ComputeDisjointSet();
            Assert.AreEqual(0, disjointSet.ElementCount);
            Assert.AreEqual(0, disjointSet.SetCount);

            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2, 3, 4 });
            disjointSet = graph.ComputeDisjointSet();
            Assert.AreEqual(4, disjointSet.ElementCount);
            Assert.AreEqual(4, disjointSet.SetCount);

            graph.AddEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(1, 4)
            });
            graph.AddVertex(5);
            disjointSet = graph.ComputeDisjointSet();
            Assert.AreEqual(5, disjointSet.ElementCount);
            Assert.AreEqual(2, disjointSet.SetCount);
            Assert.IsTrue(disjointSet.AreInSameSet(1, 2));
            Assert.IsTrue(disjointSet.AreInSameSet(1, 3));
            Assert.IsTrue(disjointSet.AreInSameSet(1, 4));
            Assert.IsFalse(disjointSet.AreInSameSet(1, 5));
        }

        [Test]
        public void ComputeDisjointSet_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((UndirectedGraph<int, Edge<int>>)null).ComputeDisjointSet());
        }

        [Test]
        public void MinimumSpanningTreePrim_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedGraph<int, Edge<int>>().MinimumSpanningTreePrim(null));
            Assert.Throws<ArgumentNullException>(
                () => ((UndirectedGraph<int, Edge<int>>)null).MinimumSpanningTreePrim(edge => 1.0));
            Assert.Throws<ArgumentNullException>(
                () => ((UndirectedGraph<int, Edge<int>>)null).MinimumSpanningTreePrim(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void MinimumSpanningTreeKruskal_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedGraph<int, Edge<int>>().MinimumSpanningTreeKruskal(null));
            Assert.Throws<ArgumentNullException>(
                () => ((UndirectedGraph<int, Edge<int>>)null).MinimumSpanningTreeKruskal(edge => 1.0));
            Assert.Throws<ArgumentNullException>(
                () => ((UndirectedGraph<int, Edge<int>>)null).MinimumSpanningTreeKruskal(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void OfflineLeastCommonAncestor_Throws()
        {
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var graph1 = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            graph1.AddVertexRange(new[] { vertex1, vertex2 });
            var pairs1 = new[] { new SEquatableEdge<TestVertex>(vertex1, vertex2) };
            
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexListGraph<TestVertex, Edge<TestVertex>>)null).OfflineLeastCommonAncestor(vertex1, pairs1));
            Assert.Throws<ArgumentNullException>(
                () => graph1.OfflineLeastCommonAncestor(null, pairs1));
            Assert.Throws<ArgumentNullException>(
                () => graph1.OfflineLeastCommonAncestor(vertex1, null));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexListGraph<TestVertex, Edge<TestVertex>>)null).OfflineLeastCommonAncestor(null, pairs1));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexListGraph<TestVertex, Edge<TestVertex>>)null).OfflineLeastCommonAncestor(vertex1, null));
            Assert.Throws<ArgumentNullException>(
                () => graph1.OfflineLeastCommonAncestor(null, null));
            Assert.Throws<ArgumentNullException>(
                () => ((IVertexListGraph<TestVertex, Edge<TestVertex>>)null).OfflineLeastCommonAncestor(null, null));

            var pairs2 = new[] { new SEquatableEdge<int>(1, 2) };
            var graph2 = new AdjacencyGraph<int, Edge<int>>();
            Assert.Throws<ArgumentException>(
                () => graph2.OfflineLeastCommonAncestor(1, pairs2));

            var graph3 = new AdjacencyGraph<int, Edge<int>>();
            graph3.AddVertex(1);
            Assert.Throws<ArgumentException>(
                () => graph3.OfflineLeastCommonAncestor(1, pairs2));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void MaximumFlow_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertexRange(new[] { 1, 2 });
            Func<Edge<int>, double> capacities = edge => 1.0;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var reverseEdgesAlgorithm = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(graph, edgeFactory);

            Assert.Throws<ArgumentException>(
                () => graph.MaximumFlow(capacities, 1, 1, out _, edgeFactory, reverseEdgesAlgorithm));

            Assert.Throws<InvalidOperationException>(
                () => graph.MaximumFlow(capacities, 1, 2, out _, edgeFactory, reverseEdgesAlgorithm));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> CloneTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(new AdjacencyGraph<int, EquatableEdge<int>>());
                yield return new TestCaseData(new BidirectionalGraph<int, EquatableEdge<int>>());
            }
        }

        [TestCaseSource(nameof(CloneTestCases))]
        public void Clone([NotNull] IMutableVertexAndEdgeSet<int, EquatableEdge<int>> cloned)
        {
            var emptyGraph1 = new AdjacencyGraph<int, EquatableEdge<int>>();
            emptyGraph1.Clone(v => v, (edge, v1, v2) => new EquatableEdge<int>(v1, v2), cloned);
            AssertEmptyGraph(cloned);

            cloned.Clear();
            var notEmptyGraph = new AdjacencyGraph<int, EquatableEdge<int>>();
            notEmptyGraph.AddVerticesAndEdgeRange(new[]
            {
                new EquatableEdge<int>(1, 2),
                new EquatableEdge<int>(2, 2),
                new EquatableEdge<int>(2, 3),
                new EquatableEdge<int>(3, 1)
            });
            notEmptyGraph.Clone(v => v, (edge, v1, v2) => new EquatableEdge<int>(v1, v2), cloned);
            AssertHasVertices(cloned, new[] { 1, 2, 3 });
            AssertHasEdges(
                cloned,
                new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 2),
                    new EquatableEdge<int>(2, 3),
                    new EquatableEdge<int>(3, 1)
                });

            // Clone is not empty
            cloned.Clear();
            cloned.AddVerticesAndEdge(new EquatableEdge<int>(1, 4));
            notEmptyGraph.Clone(v => v, (edge, v1, v2) => new EquatableEdge<int>(v1, v2), cloned);
            // Clone has been cleaned and then re-filled
            AssertHasVertices(cloned, new[] { 1, 2, 3 });
            AssertHasEdges(
                cloned,
                new[]
                {
                    new EquatableEdge<int>(1, 2),
                    new EquatableEdge<int>(2, 2),
                    new EquatableEdge<int>(2, 3),
                    new EquatableEdge<int>(3, 1)
                });
        }

        [Test]
        public void Clone_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var clone = new AdjacencyGraph<int, Edge<int>>();

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone(null, v => v, (e, v1, v2) => e, clone));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone(graph, null, (e, v1, v2) => e, clone));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone(graph, v => v, null, clone));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone(graph, v => v, (e, v1, v2) => e, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone(null, null, (e, v1, v2) => e, clone));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone(null, v => v, null, clone));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone<int, Edge<int>>(null, v => v, (e, v1, v2) => e, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone(graph, null, null, clone));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone(graph, null, (e, v1, v2) => e, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone(graph, v => v, null, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone(null, null, null, clone));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone<int, Edge<int>>(null, null, (e, v1, v2) => e, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone(graph, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => AlgorithmExtensions.Clone<int, Edge<int>>(null, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}