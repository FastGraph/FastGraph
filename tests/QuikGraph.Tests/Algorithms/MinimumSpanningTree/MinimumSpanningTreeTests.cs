using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.MinimumSpanningTree;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Collections;
using QuikGraph.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace QuikGraph.Tests.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// Tests for <see cref="PrimMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/> and <see cref="KruskalMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class MinimumSpanningTreeTests : QuikGraphUnitTests
    {
        #region Helpers

        [Pure]
        [NotNull]
        private static UndirectedGraph<string, TaggedEdge<string, double>> GetUndirectedFullGraph(int vertex)
        {
            var random = new Random();
            var graph = new UndirectedGraph<string, TaggedEdge<string, double>>();
            var trueGraph = new UndirectedGraph<string, TaggedEdge<string, double>>();
            var sets = new ForestDisjointSet<string>(vertex);
            for (int i = 0; i < vertex; ++i)
            {
                graph.AddVertex(i.ToString());
                trueGraph.AddVertex(i.ToString());
                sets.MakeSet(i.ToString());
            }

            for (int i = 0; i < vertex; ++i)
            {
                for (int j = i + 1; j < vertex; ++j)
                {
                    graph.AddEdge(
                        new TaggedEdge<string, double>(i.ToString(), j.ToString(), random.Next(100)));
                }
            }

            return graph;
        }

        private static double CompareRoot<TVertex, TEdge>([NotNull] IUndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = graph.AdjacentDegree(edge.Source) + 1;

            TEdge[] prim = graph.MinimumSpanningTreePrim(e => distances[e]).ToArray();
            TEdge[] kruskal = graph.MinimumSpanningTreeKruskal(e => distances[e]).ToArray();

            double primCost = prim.Sum(e => distances[e]);
            double kruskalCost = kruskal.Sum(e => distances[e]);
            if (Math.Abs(primCost - kruskalCost) > double.Epsilon)
            {
                GraphConsoleSerializer.DisplayGraph(graph);
                Assert.Fail("Cost do not match.");
            }

            return kruskalCost;
        }

        private static void Prim<TVertex, TEdge>([NotNull] IUndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = graph.AdjacentDegree(edge.Source) + 1;

            IEnumerable<TEdge> edges = graph.MinimumSpanningTreePrim(e => distances[e]);
            AssertSpanningTree(graph, edges);
        }

        private static void AssertSpanningTree<TVertex, TEdge>(
            [NotNull] IUndirectedGraph<TVertex, TEdge> graph,
            [NotNull, ItemNotNull] IEnumerable<TEdge> tree)
            where TEdge : IEdge<TVertex>
        {
            var spanned = new Dictionary<TVertex, TEdge>();
            foreach (TEdge edge in tree)
            {
                spanned[edge.Source] = spanned[edge.Target] = default;
            }

            // Find vertices that are connected to some edge
            var treeable = new Dictionary<TVertex, TEdge>();
            foreach (TEdge edge in graph.Edges)
                treeable[edge.Source] = treeable[edge.Target] = edge;

            // Ensure they are in the tree
            foreach (TVertex vertex in treeable.Keys)
                Assert.IsTrue(spanned.ContainsKey(vertex), $"{vertex} not in tree.");
        }

        private static void AssertMinimumSpanningTree<TVertex, TEdge>(
            [NotNull] IUndirectedGraph<TVertex, TEdge> graph,
            [NotNull] IMinimumSpanningTreeAlgorithm<TVertex, TEdge> algorithm)
            where TEdge : IEdge<TVertex>
        {
            var edgeRecorder = new EdgeRecorderObserver<TVertex, TEdge>();
            using (edgeRecorder.Attach(algorithm))
                algorithm.Compute();

            AssertSpanningTree(graph, edgeRecorder.Edges);
        }

        private static void Kruskal<TVertex, TEdge>([NotNull] IUndirectedGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = graph.AdjacentDegree(edge.Source) + 1;

            var kruskal = new KruskalMinimumSpanningTreeAlgorithm<TVertex, TEdge>(graph, e => distances[e]);
            AssertMinimumSpanningTree(graph, kruskal);
        }

        private static void PrimSpanningTree<TVertex, TEdge>([NotNull] IUndirectedGraph<TVertex, TEdge> graph, [NotNull] Func<TEdge, double> edgeWeights)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = edgeWeights(edge);

            var prim = new PrimMinimumSpanningTreeAlgorithm<TVertex, TEdge>(graph, e => distances[e]);
            AssertMinimumSpanningTree(graph, prim);
        }

        private static void KruskalSpanningTree<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g, Func<TEdge, double> edgeWeights)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in g.Edges)
                distances[edge] = edgeWeights(edge);

            var prim = new KruskalMinimumSpanningTreeAlgorithm<TVertex, TEdge>(g, e => distances[e]);
            AssertMinimumSpanningTree(g, prim);
        }

        #endregion

        [Test]
        public void Prim10()
        {
            var graph = GetUndirectedFullGraph(10);
            PrimSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void Prim50()
        {
            var graph = GetUndirectedFullGraph(50);
            PrimSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void Prim100()
        {
            var graph = GetUndirectedFullGraph(100);
            PrimSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void Prim200()
        {
            var graph = GetUndirectedFullGraph(200);
            PrimSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void Prim300()
        {
            var graph = GetUndirectedFullGraph(300);
            PrimSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void Prim400()
        {
            var graph = GetUndirectedFullGraph(400);
            PrimSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void Kruskal10()
        {
            var graph = GetUndirectedFullGraph(10);
            KruskalSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void Kruskal50()
        {
            var graph = GetUndirectedFullGraph(50);
            KruskalSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void Kruskal100()
        {
            var graph = GetUndirectedFullGraph(100);
            KruskalSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void Kruskal200()
        {
            var graph = GetUndirectedFullGraph(200);
            KruskalSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void Kruskal300()
        {
            var graph = GetUndirectedFullGraph(300);
            KruskalSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void Kruskal400()
        {
            var graph = GetUndirectedFullGraph(400);
            KruskalSpanningTree(graph, x => x.Tag);
        }

        [Test]
        public void KruskalMinimumSpanningTreeAll()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs())
                Kruskal(graph);
        }

        [Test]
        public void PrimMinimumSpanningTreeAll()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs())
                Prim(graph);
        }

        [Test]
        public void PrimKruskalMinimumSpanningTreeAll()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs())
                CompareRoot(graph);
        }

        [Test]
        public void Prim12240()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));
            graph.AddVerticesAndEdge(new Edge<int>(3, 2));
            graph.AddVerticesAndEdge(new Edge<int>(3, 4));
            graph.AddVerticesAndEdge(new Edge<int>(1, 4));

            double cost = CompareRoot(graph);
            Assert.AreEqual(9, cost);
        }

        [Test]
        public void Prim12240WithDelegate()
        {
            int[] vertices = {1, 2, 3, 4};
            var graph = vertices.ToDelegateUndirectedGraph(
                delegate(int vertex, out IEnumerable<EquatableEdge<int>> adjacentEdges)
                {
                    switch (vertex)
                    {
                        case 1:
                            adjacentEdges = new[] {new EquatableEdge<int>(1, 2), new EquatableEdge<int>(1, 4)};
                            break;
                        case 2:
                            adjacentEdges = new[] {new EquatableEdge<int>(1, 2), new EquatableEdge<int>(3, 1)};
                            break;
                        case 3:
                            adjacentEdges = new[] {new EquatableEdge<int>(3, 2), new EquatableEdge<int>(3, 4)};
                            break;
                        case 4:
                            adjacentEdges = new[] {new EquatableEdge<int>(1, 4), new EquatableEdge<int>(3, 4)};
                            break;
                        default:
                            adjacentEdges = null;
                            break;
                    }

                    return adjacentEdges != null;
                });

            double cost = CompareRoot(graph);
            Assert.AreEqual(9, cost);
        }

        [Test]
        public void Repro12273()
        {
            UndirectedGraph<string, TaggedEdge<string, double>> undirectedGraph = XmlReader
                .Create(GetGraphFilePath("repro12273.xml"))
                .DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    reader => new UndirectedGraph<string, TaggedEdge<string, double>>(),
                    reader => reader.GetAttribute("id"),
                    reader => new TaggedEdge<string, double>(
                        reader.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        reader.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"),
                        int.Parse(reader.GetAttribute("weight") ??
                                  throw new AssertionException("Must have weight attribute"))));

            List<TaggedEdge<string, double>> prim = undirectedGraph.MinimumSpanningTreePrim(e => e.Tag).ToList();
            double primCost = prim.Sum(e => e.Tag);

            List<TaggedEdge<string, double>> kruskal = undirectedGraph.MinimumSpanningTreeKruskal(e => e.Tag).ToList();
            var kruskalCost = kruskal.Sum(e => e.Tag);

            Assert.AreEqual(primCost, 63);
            Assert.AreEqual(primCost, kruskalCost);
        }
    }
}