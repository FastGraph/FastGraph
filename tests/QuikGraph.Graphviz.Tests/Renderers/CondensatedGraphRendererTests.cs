using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.Condensation;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="CondensatedGraphRenderer{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal class CondensatedGraphRendererTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<AdjacencyGraph<int, Edge<int>>, CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>>();
            var algorithm = new CondensatedGraphRenderer<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph);
            Assert.AreSame(graph, algorithm.VisitedGraph);
            Assert.IsNotNull(algorithm.Graphviz);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new CondensatedGraphRenderer<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(null));
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<TestCaseData> GenerateTestCases
        {
            [UsedImplicitly]
            get
            {
                // Empty graph
                var graph = new AdjacencyGraph<AdjacencyGraph<int, Edge<int>>, CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>>();
                yield return new TestCaseData(
                    graph,
                    @"digraph G {" + Environment.NewLine +
                    @"node [fontname=""Tahoma"", fontsize=8.25, shape=box, style=filled, fillcolor=""#FFFFE0FF""];" + Environment.NewLine +
                    @"edge [fontname=""Tahoma"", fontsize=8.25];" + Environment.NewLine +
                    @"}");


                // Cluster graph 1
                var subGraph1 = new AdjacencyGraph<int, Edge<int>>();
                subGraph1.AddVerticesAndEdgeRange(new[]
                {
                    new Edge<int>(1, 2),
                    new Edge<int>(2, 3),
                    new Edge<int>(3, 1)
                });

                var subGraph2 = new AdjacencyGraph<int, Edge<int>>();
                subGraph2.AddVerticesAndEdgeRange(new[]
                {
                    new Edge<int>(1, 1),
                    new Edge<int>(1, 2),
                    new Edge<int>(2, 3),
                    new Edge<int>(3, 2)
                });

                var subGraph3 = new AdjacencyGraph<int, Edge<int>>();
                subGraph3.AddVerticesAndEdgeRange(new[]
                {
                    new Edge<int>(1, 4),
                    new Edge<int>(2, 4)
                });
                subGraph3.AddVertex(3);

                graph = new AdjacencyGraph<AdjacencyGraph<int, Edge<int>>, CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>>();
                graph.AddVerticesAndEdgeRange(new[]
                {
                    new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(subGraph1, subGraph2),
                    new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(subGraph1, subGraph3)
                });

                yield return new TestCaseData(
                    graph,
                    @"digraph G {" + Environment.NewLine +
                    @"node [fontname=""Tahoma"", fontsize=8.25, shape=box, style=filled, fillcolor=""#FFFFE0FF""];" + Environment.NewLine +
                    @"edge [fontname=""Tahoma"", fontsize=8.25];" + Environment.NewLine +
                    @"0 [label=""3-3\n  1\n  2\n  3\n  1 -> 2\n  2 -> 3\n  3 -> 1\n""];" + Environment.NewLine +
                    @"1 [label=""3-4\n  1\n  2\n  3\n  1 -> 1\n  1 -> 2\n  2 -> 3\n  3 -> 2\n""];" + Environment.NewLine +
                    @"2 [label=""4-2\n  1\n  4\n  2\n  3\n  1 -> 4\n  2 -> 4\n""];" + Environment.NewLine +
                    @"0 -> 1 [label=""0\n""];" + Environment.NewLine +
                    @"0 -> 2 [label=""0\n""];" + Environment.NewLine +
                    @"}");


                // Cluster graph 2
                subGraph1 = new AdjacencyGraph<int, Edge<int>>();
                subGraph1.AddVerticesAndEdgeRange(new[]
                {
                    new Edge<int>(1, 2),
                    new Edge<int>(2, 3),
                    new Edge<int>(3, 1)
                });

                subGraph2 = new AdjacencyGraph<int, Edge<int>>();
                subGraph2.AddVerticesAndEdgeRange(new[]
                {
                    new Edge<int>(1, 1),
                    new Edge<int>(1, 2),
                    new Edge<int>(2, 3),
                    new Edge<int>(3, 2)
                });

                subGraph3 = new AdjacencyGraph<int, Edge<int>>();
                subGraph3.AddVerticesAndEdgeRange(new[]
                {
                    new Edge<int>(1, 4),
                    new Edge<int>(2, 4)
                });
                subGraph3.AddVertex(3);

                graph = new AdjacencyGraph<AdjacencyGraph<int, Edge<int>>, CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>>();
                var condensedEdge1 = new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(subGraph1, subGraph2);
                condensedEdge1.Edges.Add(new Edge<int>(1, 2));
                var condensedEdge2 = new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(subGraph1, subGraph3);
                condensedEdge2.Edges.Add(new Edge<int>(2, 1));
                condensedEdge2.Edges.Add(new Edge<int>(3, 4));
                var condensedEdge3 = new CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(subGraph2, subGraph3);
                graph.AddVerticesAndEdgeRange(new[] { condensedEdge1, condensedEdge2, condensedEdge3 });

                yield return new TestCaseData(
                    graph,
                    @"digraph G {" + Environment.NewLine +
                    @"node [fontname=""Tahoma"", fontsize=8.25, shape=box, style=filled, fillcolor=""#FFFFE0FF""];" + Environment.NewLine +
                    @"edge [fontname=""Tahoma"", fontsize=8.25];" + Environment.NewLine +
                    @"0 [label=""3-3\n  1\n  2\n  3\n  1 -> 2\n  2 -> 3\n  3 -> 1\n""];" + Environment.NewLine +
                    @"1 [label=""3-4\n  1\n  2\n  3\n  1 -> 1\n  1 -> 2\n  2 -> 3\n  3 -> 2\n""];" + Environment.NewLine +
                    @"2 [label=""4-2\n  1\n  4\n  2\n  3\n  1 -> 4\n  2 -> 4\n""];" + Environment.NewLine +
                    @"0 -> 1 [label=""1\n  1 -> 2\n""];" + Environment.NewLine +
                    @"0 -> 2 [label=""2\n  2 -> 1\n  3 -> 4\n""];" + Environment.NewLine +
                    @"1 -> 2 [label=""0\n""];" + Environment.NewLine +
                    @"}");
            }
        }

        [TestCaseSource(nameof(GenerateTestCases))]
        public void Generate(
            [NotNull] AdjacencyGraph<AdjacencyGraph<int, Edge<int>>, CondensedEdge<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>> graph,
            [NotNull] string expectedDot)
        {
            var dotEngine = new TestDotEngine
            {
                ExpectedDot = expectedDot
            };

            var algorithm = new CondensatedGraphRenderer<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(graph);
            algorithm.Generate(dotEngine, "NotSaved.dot");
        }
    }
}