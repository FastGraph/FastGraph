﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using FastGraph.Algorithms.Condensation;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="CondensatedGraphRenderer{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class CondensatedGraphRendererTests
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

        [Test]
        public void Generate_WithEscape()
        {
            const string vertex1 = "Vertex1&/<>@~|";
            const string vertex2 = "Vertex2æéèêë£¤¶ÀÁÂÃÄÅ";
            const string vertex3 = "\"Vertex3\"\nΣη← ♠\\[]()";
            const string vertex4 = "Vertex4∴∞⇐ℜΩ÷嗷娪";
            var subGraph1 = new AdjacencyGraph<string, Edge<string>>();
            subGraph1.AddVerticesAndEdgeRange(new[]
            {
                new Edge<string>(vertex1, vertex2),
                new Edge<string>(vertex2, vertex2),
                new Edge<string>(vertex3, vertex1)
            });

            var subGraph2 = new AdjacencyGraph<string, Edge<string>>();
            subGraph2.AddVerticesAndEdgeRange(new[]
            {
                new Edge<string>(vertex1, vertex1),
                new Edge<string>(vertex1, vertex2),
                new Edge<string>(vertex2, vertex3),
                new Edge<string>(vertex2, vertex4),
                new Edge<string>(vertex3, vertex4)
            });

            var graph = new AdjacencyGraph<AdjacencyGraph<string, Edge<string>>, CondensedEdge<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>>();
            var condensedEdge = new CondensedEdge<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(subGraph1, subGraph2);
            condensedEdge.Edges.Add(new Edge<string>(vertex1, vertex2));
            condensedEdge.Edges.Add(new Edge<string>(vertex3, vertex1));
            graph.AddVerticesAndEdgeRange(new[] { condensedEdge });

            const string expectedVertex1 = @"Vertex1&/<>@~|";
            const string expectedVertex2 = @"Vertex2æéèêë£¤¶ÀÁÂÃÄÅ";
            const string expectedVertex3 = @"\""Vertex3\""\nΣη← ♠\\[]()";
            const string expectedVertex4 = @"Vertex4∴∞⇐ℜΩ÷嗷娪";
            string expectedDot =
                @"digraph G {" + Environment.NewLine +
                @"node [fontname=""Tahoma"", fontsize=8.25, shape=box, style=filled, fillcolor=""#FFFFE0FF""];" + Environment.NewLine +
                @"edge [fontname=""Tahoma"", fontsize=8.25];" + Environment.NewLine +
                @"0 [label=""3-3\n  " + expectedVertex1 + @"\n  " + expectedVertex2 + @"\n  " + expectedVertex3 + @"\n  " + expectedVertex1 + @" -> " + expectedVertex2 + @"\n  " + expectedVertex2 + @" -> " + expectedVertex2 + @"\n  " + expectedVertex3 + @" -> " + expectedVertex1 + @"\n""];" + Environment.NewLine +
                @"1 [label=""4-5\n  " + expectedVertex1 + @"\n  " + expectedVertex2 + @"\n  " + expectedVertex3 + @"\n  " + expectedVertex4 + @"\n  " + expectedVertex1 + @" -> " + expectedVertex1 + @"\n  " + expectedVertex1 + @" -> " + expectedVertex2 + @"\n  " + expectedVertex2 + @" -> " + expectedVertex3 + @"\n  " + expectedVertex2 + @" -> " + expectedVertex4 + @"\n  " + expectedVertex3 + @" -> " + expectedVertex4 + @"\n""];" + Environment.NewLine +
                @"0 -> 1 [label=""2\n  " + expectedVertex1 + @" -> " + expectedVertex2 + @"\n  " + expectedVertex3 + @" -> " + expectedVertex1 + @"\n""];" + Environment.NewLine +
                @"}";

            var dotEngine = new TestDotEngine
            {
                ExpectedDot = expectedDot
            };

            var algorithm = new CondensatedGraphRenderer<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(graph);
            algorithm.Generate(dotEngine, "NotSaved.dot");
        }
    }
}
