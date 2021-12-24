#nullable enable

using System.Collections.Immutable;
using System.Xml;
using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms;
using FastGraph.Algorithms.MinimumSpanningTree;
using FastGraph.Algorithms.Observers;
using FastGraph.Collections;
using FastGraph.Serialization;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;

namespace FastGraph.Tests.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// Base class for minimum spanning tree tests.
    /// </summary>
    internal abstract class MinimumSpanningTreeTestsBase
    {
        #region Test helpers

        [Pure]
        protected static UndirectedGraph<string, TaggedEdge<string, double>> GetUndirectedCompleteGraph(int vertex)
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
                        new TaggedEdge<string, double>(
                            i.ToString(),
                            j.ToString(),
                            random.Next(100)));
                }
            }

            return graph;
        }

        [CustomAssertion]
        private static void HavePrimAndKruskalCostsFromRoot<TVertex, TEdge>(AndWhichConstraint<ObjectAssertions, IUndirectedGraph<TVertex, TEdge>> actual, double expectedCost)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in actual.Subject.Edges)
                distances[edge] = actual.Subject.AdjacentDegree(edge.Source) + 1;

            TEdge[] prim = actual.Subject.MinimumSpanningTreePrim(e => distances[e]).ToArray();
            TEdge[] kruskal = actual.Subject.MinimumSpanningTreeKruskal(e => distances[e]).ToArray();
            double primCost = prim.Sum(e => distances[e]);
            double kruskalCost = kruskal.Sum(e => distances[e]);

            primCost.Should().BeApproximately(kruskalCost, double.Epsilon).And.BeApproximately(expectedCost, double.Epsilon);
        }

        [CustomAssertion]
        private static void AssertSpanningEdgesFromSourceGraph<TVertex, TEdge>(
            GenericCollectionAssertions<TEdge> actualSpanningEdges,
            IUndirectedGraph<TVertex, TEdge> expectedMatchingSourceGraph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var actualSpannedVertices = SelectDistinctVertices<TVertex, TEdge>(actualSpanningEdges.Subject);

            var expectedMatchingSourceGraphVertices = SelectDistinctVertices<TVertex, TEdge>(expectedMatchingSourceGraph.Edges);

            expectedMatchingSourceGraphVertices.Should().BeSubsetOf(actualSpannedVertices);
        }

        private static void AssertMinimumSpanningTree<TVertex, TEdge>(
            IUndirectedGraph<TVertex, TEdge> sourceGraph,
            IMinimumSpanningTreeAlgorithm<TVertex, TEdge> algorithm)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var edgeRecorder = new EdgeRecorderObserver<TVertex, TEdge>();
            using (edgeRecorder.Attach(algorithm))
                algorithm.Compute();

            AssertSpanningEdgesFromSourceGraph(edgeRecorder.Edges.Should(), sourceGraph);
        }

        protected static void PrimSpanningTree<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> graph, Func<TEdge, double> edgeWeights)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = edgeWeights(edge);

            var prim = new PrimMinimumSpanningTreeAlgorithm<TVertex, TEdge>(graph, e => distances[e]);
            AssertMinimumSpanningTree(graph, prim);
        }

        protected static void Prim<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = graph.AdjacentDegree(edge.Source) + 1;

            IEnumerable<TEdge> edges = graph.MinimumSpanningTreePrim(e => distances[e]);
            AssertSpanningEdgesFromSourceGraph(edges.Should(), graph);
        }

        protected static void KruskalSpanningTree<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> graph, Func<TEdge, double> edgeWeights)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = edgeWeights(edge);

            var kruskal = new KruskalMinimumSpanningTreeAlgorithm<TVertex, TEdge>(graph, e => distances[e]);
            AssertMinimumSpanningTree(graph, kruskal);
        }

        protected static void Kruskal<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = graph.AdjacentDegree(edge.Source) + 1;

            IEnumerable<TEdge> edges = graph.MinimumSpanningTreeKruskal(e => distances[e]);
            AssertSpanningEdgesFromSourceGraph(edges.Should(), graph);
        }

        #endregion

        [Test]
        public void SimpleComparePrimKruskal()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));
            graph.AddVerticesAndEdge(new Edge<int>(3, 2));
            graph.AddVerticesAndEdge(new Edge<int>(3, 4));
            graph.AddVerticesAndEdge(new Edge<int>(1, 4));

            HavePrimAndKruskalCostsFromRoot(graph.Should().BeAssignableTo<IUndirectedGraph<int, Edge<int>>>(), 9);
        }

        [Test]
        public void DelegateComparePrimKruskal()
        {
            int[] vertices = { 1, 2, 3, 4 };
            var graph = vertices.ToDelegateUndirectedGraph(
                (int vertex, out IEnumerable<EquatableEdge<int>>? adjacentEdges) =>
                {
                    switch (vertex)
                    {
                        case 1:
                            adjacentEdges = new[] { new EquatableEdge<int>(1, 2), new EquatableEdge<int>(1, 4) };
                            break;
                        case 2:
                            adjacentEdges = new[] { new EquatableEdge<int>(1, 2), new EquatableEdge<int>(3, 1) };
                            break;
                        case 3:
                            adjacentEdges = new[] { new EquatableEdge<int>(3, 2), new EquatableEdge<int>(3, 4) };
                            break;
                        case 4:
                            adjacentEdges = new[] { new EquatableEdge<int>(1, 4), new EquatableEdge<int>(3, 4) };
                            break;
                        default:
                            adjacentEdges = default;
                            break;
                    }

                    return adjacentEdges != default;
                });

            HavePrimAndKruskalCostsFromRoot(graph.Should().BeAssignableTo<IUndirectedGraph<int, EquatableEdge<int>>>(), 9);
        }

        [Test]
        public void TestGraph()
        {
            UndirectedGraph<string, TaggedEdge<string, double>> undirectedGraph = XmlReader
                .Create(TestGraphSourceProvider.Instance.testGraph.SourcePath!)
                .DeserializeFromXml(
                    "graph",
                    "node",
                    "edge",
                    "",
                    _ => new UndirectedGraph<string, TaggedEdge<string, double>>(),
                    reader => reader.GetAttribute("id")!,
                    reader => new TaggedEdge<string, double>(
                        reader.GetAttribute("source") ?? throw new AssertionException("Must have source attribute"),
                        reader.GetAttribute("target") ?? throw new AssertionException("Must have target attribute"),
                        int.Parse(reader.GetAttribute("weight") ?? throw new AssertionException("Must have weight attribute"))));

            TaggedEdge<string, double>[] prim = undirectedGraph.MinimumSpanningTreePrim<string, TaggedEdge<string, double>>(e => e.Tag).ToArray<TaggedEdge<string, double>>();
            double primCost = prim.Sum<TaggedEdge<string, double>>(e => e.Tag);

            TaggedEdge<string, double>[] kruskal = undirectedGraph.MinimumSpanningTreeKruskal<string, TaggedEdge<string, double>>(e => e.Tag).ToArray<TaggedEdge<string, double>>();
            double kruskalCost = kruskal.Sum<TaggedEdge<string, double>>(e => e.Tag);

            primCost.Should().Be(63);
            kruskalCost.Should().Be(primCost);
        }

        private static IImmutableSet<TVertex> SelectDistinctVertices<TVertex, TEdge>(IEnumerable<TEdge> edges)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            var resultBuilder = ImmutableHashSet<TVertex>.Empty.ToBuilder();

            foreach (var edge in edges)
            {
                resultBuilder.Add(edge.Source);
                resultBuilder.Add(edge.Target);
            }

            return resultBuilder.ToImmutable();
        }
    }
}
