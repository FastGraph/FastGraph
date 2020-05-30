using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.Condensation;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="EdgeMergeCondensatedGraphRenderer{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class EdgeMergeCondensatedGraphRendererTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, MergedEdge<int, Edge<int>>>();
            var algorithm = new EdgeMergeCondensatedGraphRenderer<int, Edge<int>>(graph);
            Assert.AreSame(graph, algorithm.VisitedGraph);
            Assert.IsNotNull(algorithm.Graphviz);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new EdgeMergeCondensatedGraphRenderer<int, Edge<int>>(null));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> GenerateTestCases
        {
            [UsedImplicitly]
            get
            {
                // Empty graph
                var graph = new AdjacencyGraph<int, MergedEdge<int, Edge<int>>>();
                yield return new TestCaseData(
                    graph,
                    @"digraph G {" + Environment.NewLine +
                    @"node [fontname=""Tahoma"", fontsize=8.25, shape=box, style=filled, fillcolor=""#FFFFE0FF""];" + Environment.NewLine +
                    @"edge [fontname=""Tahoma"", fontsize=8.25];" + Environment.NewLine +
                    @"}");

                // Not empty graph
                graph = new AdjacencyGraph<int, MergedEdge<int, Edge<int>>>();
                graph.AddVertexRange(new[] { 4, 8 });

                var edge12 = new Edge<int>(1, 2);
                var edge13 = new Edge<int>(1, 3);
                var edge23 = new Edge<int>(2, 3);
                var edge38 = new Edge<int>(3, 8);
                var edge42 = new Edge<int>(4, 2);
                var edge43 = new Edge<int>(4, 3);
                var edge44 = new Edge<int>(4, 4);
                var edge45 = new Edge<int>(4, 5);
                var edge57 = new Edge<int>(5, 7);
                var edge71 = new Edge<int>(7, 1);
                var edge82 = new Edge<int>(8, 2);

                var mergeEdge1 = new MergedEdge<int, Edge<int>>(8, 8);
                mergeEdge1.Edges.Add(edge82);
                mergeEdge1.Edges.Add(edge23);
                mergeEdge1.Edges.Add(edge38);

                var mergeEdge2 = new MergedEdge<int, Edge<int>>(4, 4);
                mergeEdge2.Edges.Add(edge44);

                var mergeEdge3 = new MergedEdge<int, Edge<int>>(4, 8);
                mergeEdge3.Edges.Add(edge43);
                mergeEdge3.Edges.Add(edge38);

                var mergeEdge4 = new MergedEdge<int, Edge<int>>(4, 8);
                mergeEdge4.Edges.Add(edge42);
                mergeEdge4.Edges.Add(edge23);
                mergeEdge4.Edges.Add(edge38);

                var mergeEdge5 = new MergedEdge<int, Edge<int>>(4, 8);
                mergeEdge5.Edges.Add(edge45);
                mergeEdge5.Edges.Add(edge57);
                mergeEdge5.Edges.Add(edge71);
                mergeEdge5.Edges.Add(edge13);
                mergeEdge5.Edges.Add(edge38);

                var mergeEdge6 = new MergedEdge<int, Edge<int>>(4, 8);
                mergeEdge6.Edges.Add(edge45);
                mergeEdge6.Edges.Add(edge57);
                mergeEdge6.Edges.Add(edge71);
                mergeEdge6.Edges.Add(edge12);
                mergeEdge6.Edges.Add(edge23);
                mergeEdge6.Edges.Add(edge38);

                graph.AddEdgeRange(new[]
                {
                    mergeEdge1, mergeEdge2, mergeEdge3,
                    mergeEdge4, mergeEdge5, mergeEdge6
                });
                yield return new TestCaseData(
                    graph,
                    @"digraph G {" + Environment.NewLine +
                    @"node [fontname=""Tahoma"", fontsize=8.25, shape=box, style=filled, fillcolor=""#FFFFE0FF""];" + Environment.NewLine +
                    @"edge [fontname=""Tahoma"", fontsize=8.25];" + Environment.NewLine +
                    @"0 [label=""4""];" + Environment.NewLine +
                    @"1 [label=""8""];" + Environment.NewLine +
                    @"0 -> 0 [label=""1\n  4 -> 4\n""];" + Environment.NewLine +
                    @"0 -> 1 [label=""2\n  4 -> 3\n  3 -> 8\n""];" + Environment.NewLine +
                    @"0 -> 1 [label=""3\n  4 -> 2\n  2 -> 3\n  3 -> 8\n""];" + Environment.NewLine +
                    @"0 -> 1 [label=""5\n  4 -> 5\n  5 -> 7\n  7 -> 1\n  1 -> 3\n  3 -> 8\n""];" + Environment.NewLine +
                    @"0 -> 1 [label=""6\n  4 -> 5\n  5 -> 7\n  7 -> 1\n  1 -> 2\n  2 -> 3\n  3 -> 8\n""];" + Environment.NewLine +
                    @"1 -> 1 [label=""3\n  8 -> 2\n  2 -> 3\n  3 -> 8\n""];" + Environment.NewLine +
                    @"}");
            }
        }

        [TestCaseSource(nameof(GenerateTestCases))]
        public void Generate(
            [NotNull] AdjacencyGraph<int, MergedEdge<int, Edge<int>>> graph,
            [NotNull] string expectedDot)
        {
            var dotEngine = new TestDotEngine
            {
                ExpectedDot = expectedDot
            };

            var algorithm = new EdgeMergeCondensatedGraphRenderer<int, Edge<int>>(graph);
            algorithm.Generate(dotEngine, "NotSaved.dot");
        }
    }
}