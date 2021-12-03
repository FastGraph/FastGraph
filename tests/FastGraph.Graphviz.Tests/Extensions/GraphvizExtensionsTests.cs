using System;
using System.Collections.Generic;
using NUnit.Framework;
using FastGraph.Graphviz.Dot;

namespace FastGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizExtensions"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizExtensionsTests
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
        public void ToGraphviz_DelegateGraph()
        {
            int[] vertices = { 1, 2, 3, 4, 5 };
            var graph = new DelegateVertexAndEdgeListGraph<int, Edge<int>>(
                vertices,
                (int vertex, out IEnumerable<Edge<int>> outEdges) =>
                {
                    if (vertex == 1)
                    {
                        outEdges = new[] { new Edge<int>(1, 2), new Edge<int>(1, 3) };
                        return true;
                    }

                    if (vertex == 2)
                    {
                        outEdges = new[] { new Edge<int>(2, 4) };
                        return true;
                    }

                    if (vertex is 3 or 4 or 5)
                    {
                        outEdges = new Edge<int>[] { };
                        return true;
                    }

                    outEdges = null;
                    return false;
                });

            string expectedDot =
                @"digraph G {" + Environment.NewLine
                + @"0;" + Environment.NewLine
                + @"1;" + Environment.NewLine
                + @"2;" + Environment.NewLine
                + @"3;" + Environment.NewLine
                + @"4;" + Environment.NewLine
                + @"0 -> 1;" + Environment.NewLine
                + @"0 -> 2;" + Environment.NewLine
                + @"1 -> 3;" + Environment.NewLine
                + @"}";
            string dotGraph = graph.ToGraphviz();
            Assert.AreEqual(expectedDot, dotGraph);
        }

        [Test]
        public void ToGraphviz_EquatableEdgeDelegateGraph()
        {
            int[] vertices = { 1, 2, 3, 4, 5 };
            var graph = new DelegateVertexAndEdgeListGraph<int, EquatableEdge<int>>(
                vertices,
                (int vertex, out IEnumerable<EquatableEdge<int>> outEdges) =>
                {
                    if (vertex == 1)
                    {
                        outEdges = new[] { new EquatableEdge<int>(1, 2), new EquatableEdge<int>(1, 3) };
                        return true;
                    }

                    if (vertex == 2)
                    {
                        outEdges = new[] { new EquatableEdge<int>(2, 4) };
                        return true;
                    }

                    if (vertex is 3 or 4 or 5)
                    {
                        outEdges = new EquatableEdge<int>[] { };
                        return true;
                    }

                    outEdges = null;
                    return false;
                });

            string expectedDot =
                @"digraph G {" + Environment.NewLine
                + @"0;" + Environment.NewLine
                + @"1;" + Environment.NewLine
                + @"2;" + Environment.NewLine
                + @"3;" + Environment.NewLine
                + @"4;" + Environment.NewLine
                + @"0 -> 1;" + Environment.NewLine
                + @"0 -> 2;" + Environment.NewLine
                + @"1 -> 3;" + Environment.NewLine
                + @"}";
            string dotGraph = graph.ToGraphviz();
            Assert.AreEqual(expectedDot, dotGraph);
        }

        [Test]
        public void ToGraphvizWithEmptyInit()
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
            string dotGraph = graph.ToGraphviz(algorithm =>
            {
                algorithm.FormatCluster += (_, _) =>
                {
                };
                algorithm.FormatVertex += (_, _) =>
                {
                };
                algorithm.FormatEdge += (_, _) =>
                {
                };
            });
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
                @"digraph G {" + Environment.NewLine
                + @"node [shape=diamond];" + Environment.NewLine
                + @"edge [tooltip=""Test Edge""];" + Environment.NewLine
                + @"subgraph cluster1 {" + Environment.NewLine
                + @"5 [label=""Test Vertex 6""];" + Environment.NewLine
                + @"6 [label=""Test Vertex 7""];" + Environment.NewLine
                + @"7 [label=""Test Vertex 8""];" + Environment.NewLine
                + @"5 -> 6;" + Environment.NewLine
                + @"6 -> 7;" + Environment.NewLine
                + @"}" + Environment.NewLine
                + @"subgraph cluster2 {" + Environment.NewLine
                + @"8 [label=""Test Vertex 9""];" + Environment.NewLine
                + @"9 [label=""Test Vertex 10""];" + Environment.NewLine
                + @"10 [label=""Test Vertex 11""];" + Environment.NewLine
                + @"8 -> 9;" + Environment.NewLine
                + @"}" + Environment.NewLine
                + @"0 [label=""Test Vertex 1""];" + Environment.NewLine
                + @"1 [label=""Test Vertex 2""];" + Environment.NewLine
                + @"2 [label=""Test Vertex 3""];" + Environment.NewLine
                + @"3 [label=""Test Vertex 4""];" + Environment.NewLine
                + @"4 [label=""Test Vertex 5""];" + Environment.NewLine
                + @"0 -> 1;" + Environment.NewLine
                + @"0 -> 2;" + Environment.NewLine
                + @"1 -> 3;" + Environment.NewLine
                + @"}";
            string dotGraph = clusteredGraph.ToGraphviz(algorithm =>
            {
                algorithm.CommonVertexFormat.Shape = GraphvizVertexShape.Diamond;
                algorithm.CommonEdgeFormat.ToolTip = "Test Edge";
                algorithm.FormatVertex += (_, args) =>
                {
                    args.VertexFormat.Label = $"Test Vertex {args.Vertex}";
                };
            });
            Assert.AreEqual(expectedDot, dotGraph);
        }

        [Test]
        public void ToGraphvizWithInit2()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 4)
            });
            graph.AddVertex(5);

            string expectedDot =
                @"digraph G {" + Environment.NewLine
                + @"node [style=bold];" + Environment.NewLine
                + @"edge [color=""#F0FFFFFF""];" + Environment.NewLine
                + @"0 [tooltip=""Tooltip for Test Vertex 1""];" + Environment.NewLine
                + @"1 [tooltip=""Tooltip for Test Vertex 2""];" + Environment.NewLine
                + @"2 [tooltip=""Tooltip for Test Vertex 3""];" + Environment.NewLine
                + @"3 [tooltip=""Tooltip for Test Vertex 4""];" + Environment.NewLine
                + @"4 [tooltip=""Tooltip for Test Vertex 5""];" + Environment.NewLine
                + @"0 -> 1 [tooltip=""Tooltip for Test Edge 1 -> 2""];" + Environment.NewLine
                + @"0 -> 2 [tooltip=""Tooltip for Test Edge 1 -> 3""];" + Environment.NewLine
                + @"1 -> 3 [tooltip=""Tooltip for Test Edge 2 -> 4""];" + Environment.NewLine
                + @"}";
            string dotGraph = graph.ToGraphviz(algorithm =>
            {
                algorithm.CommonVertexFormat.Style = GraphvizVertexStyle.Bold;
                algorithm.CommonEdgeFormat.StrokeColor = GraphvizColor.Azure;
                algorithm.FormatVertex += (_, args) =>
                {
                    args.VertexFormat.ToolTip = $"Tooltip for Test Vertex {args.Vertex}";
                };
                algorithm.FormatEdge += (_, args) =>
                {
                    args.EdgeFormat.ToolTip = $"Tooltip for Test Edge {args.Edge.Source} -> {args.Edge.Target}";
                };
            });
            Assert.AreEqual(expectedDot, dotGraph);
        }

        [Test]
        public void ToGraphvizWithInit_Record()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 4)
            });
            graph.AddVertex(5);

            string expectedDot =
                @"digraph G {" + Environment.NewLine
                + @"node [tooltip=""Vertex""];" + Environment.NewLine
                + @"edge [tooltip=""Edge""];" + Environment.NewLine
                + @"0 [shape=record, label=""Vertex\ 1 | Generated\ Record""];" + Environment.NewLine
                + @"1 [shape=record, label=""Vertex\ 2 | Custom\ Record | { Top | Bottom }""];" + Environment.NewLine
                + @"2 [shape=box, label=""Vertex 3 label""];" + Environment.NewLine
                + @"3 [shape=record, label=""Vertex\ 4 | Generated\ Record""];" + Environment.NewLine
                + @"4 [shape=record, label=""Vertex\ 5 | Generated\ Record""];" + Environment.NewLine
                + @"0 -> 1;" + Environment.NewLine
                + @"0 -> 2;" + Environment.NewLine
                + @"1 -> 3;" + Environment.NewLine
                + @"}";
            string dotGraph = graph.ToGraphviz(algorithm =>
            {
                algorithm.CommonVertexFormat.ToolTip = "Vertex";
                algorithm.CommonEdgeFormat.ToolTip = "Edge";

                algorithm.FormatVertex += (_, args) =>
                {
                    args.VertexFormat.Shape = GraphvizVertexShape.Record;

                    if (args.Vertex == 2)
                    {
                        args.VertexFormat.Label = @"Vertex\ 2 | Custom\ Record | { Top | Bottom }";
                    }
                    else if (args.Vertex == 3)
                    {
                        args.VertexFormat.Shape = GraphvizVertexShape.Box;
                        args.VertexFormat.Label = @"Vertex 3 label";
                    }
                    else
                    {
                        args.VertexFormat.Record = new GraphvizRecord
                        {
                            Cells = new GraphvizRecordCellCollection(new[]
                            {
                                new GraphvizRecordCell { Text = $"Vertex {args.Vertex}" },
                                new GraphvizRecordCell { Text = "Generated Record" }
                            })
                        };
                    }
                };
            });
            Assert.AreEqual(expectedDot, dotGraph);
        }

        [Test]
        public void ToGraphvizWithInit_Record2()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(1, 3),
                new Edge<int>(2, 4)
            });
            graph.AddVertex(5);

            string expectedDot =
                @"digraph G {" + Environment.NewLine
                + @"node [shape=record];" + Environment.NewLine
                + @"edge [tooltip=""Edge""];" + Environment.NewLine
                + @"0 [label=""Vertex\ 1 | Generated\ Record""];" + Environment.NewLine
                + @"1 [label=""Vertex\ 2 | Custom\ Record | { Top | Bottom }""];" + Environment.NewLine
                + @"2 [shape=box, label=""Vertex 3 label""];" + Environment.NewLine
                + @"3 [label=""Vertex\ 4 | Generated\ Record""];" + Environment.NewLine
                + @"4 [label=""Vertex\ 5 | Generated\ Record""];" + Environment.NewLine
                + @"0 -> 1;" + Environment.NewLine
                + @"0 -> 2;" + Environment.NewLine
                + @"1 -> 3;" + Environment.NewLine
                + @"}";
            string dotGraph = graph.ToGraphviz(algorithm =>
            {
                algorithm.CommonVertexFormat.Shape = GraphvizVertexShape.Record;
                algorithm.CommonEdgeFormat.ToolTip = "Edge";

                algorithm.FormatVertex += (_, args) =>
                {
                    if (args.Vertex == 2)
                    {
                        args.VertexFormat.Label = @"Vertex\ 2 | Custom\ Record | { Top | Bottom }";
                    }
                    else if (args.Vertex == 3)
                    {
                        args.VertexFormat.Shape = GraphvizVertexShape.Box;
                        args.VertexFormat.Label = @"Vertex 3 label";
                    }
                    else
                    {
                        args.VertexFormat.Record = new GraphvizRecord
                        {
                            Cells = new GraphvizRecordCellCollection(new[]
                            {
                                new GraphvizRecordCell { Text = $"Vertex {args.Vertex}" },
                                new GraphvizRecordCell { Text = "Generated Record" }
                            })
                        };
                    }
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
    }
}
