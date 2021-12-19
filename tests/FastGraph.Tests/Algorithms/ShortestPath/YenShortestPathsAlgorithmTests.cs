#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.ShortestPath;

namespace FastGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for <see cref="YenShortestPathsAlgorithm{TVertex}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class YenShortestPathsAlgorithmTests
    {
        [Test]
        public void Constructor()
        {
            Func<EquatableTaggedEdge<int, double>, double> Weights = _ => 1.0;

            var graph = new AdjacencyGraph<int, EquatableTaggedEdge<int, double>>();
            graph.AddVertexRange(new[] { 1, 2 });
            // ReSharper disable ObjectCreationAsStatement
            Invoking((Func<YenShortestPathsAlgorithm<int>>)(() => new YenShortestPathsAlgorithm<int>(graph, 1, 2, int.MaxValue))).Should().NotThrow();
            Invoking((Func<YenShortestPathsAlgorithm<int>>)(() => new YenShortestPathsAlgorithm<int>(graph, 1, 2, 10))).Should().NotThrow();

            Invoking((Func<YenShortestPathsAlgorithm<int>>)(() => new YenShortestPathsAlgorithm<int>(graph, 1, 2, int.MaxValue, Weights, paths => paths.Where(path => path.Count() > 2)))).Should().NotThrow();
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");

            var graph = new AdjacencyGraph<TestVertex, EquatableTaggedEdge<TestVertex, double>>();
            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(graph, vertex1, vertex2, int.MaxValue)).Should().Throw<ArgumentException>();

            graph = new AdjacencyGraph<TestVertex, EquatableTaggedEdge<TestVertex, double>>();
            graph.AddVertex(vertex1);
            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(graph, vertex1, vertex2, int.MaxValue)).Should().Throw<ArgumentException>();

            graph = new AdjacencyGraph<TestVertex, EquatableTaggedEdge<TestVertex, double>>();
            graph.AddVertex(vertex2);
            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(graph, vertex1, vertex2, int.MaxValue)).Should().Throw<ArgumentException>();

            graph = new AdjacencyGraph<TestVertex, EquatableTaggedEdge<TestVertex, double>>();
            graph.AddVertexRange(new[] { vertex1, vertex2 });

#pragma warning disable CS8625
            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(default, vertex1, vertex2, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(graph, default, vertex2, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(graph, vertex1, default, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(default, default, vertex2, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(default, vertex1, default, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(graph, default, default, int.MaxValue)).Should().Throw<ArgumentNullException>();
            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(default, default, default, int.MaxValue)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute

            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(graph, vertex1, vertex2, 0)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => new YenShortestPathsAlgorithm<TestVertex>(graph, vertex1, vertex2, -1)).Should().Throw<ArgumentOutOfRangeException>();
            // ReSharper restore ObjectCreationAsStatement
        }

        /// <summary>
        /// Attempt to use for simple graph.
        /// Expecting that Dijkstra’s algorithm couldn't find any ways.
        /// </summary>
        [Test]
        public void SimpleNoPathGraph()
        {
            var graph = new AdjacencyGraph<char, EquatableTaggedEdge<char, double>>(true);
            graph.AddVertex('1');

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '1', 10);
            Invoking(() => algorithm.Execute()).Should().Throw<NoPathFoundException>();

            graph.AddVertex('2');
            algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '2', 10);
            Invoking(() => algorithm.Execute()).Should().Throw<NoPathFoundException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        /// <summary>
        /// Attempt to use for loop graph.
        /// Expecting that Dijkstra’s algorithm couldn't find any ways.
        /// </summary>
        [Test]
        public void LoopGraph()
        {
            var graph = new AdjacencyGraph<char, EquatableTaggedEdge<char, double>>(true);
            graph.AddVertexRange("1");

            var algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '1', 10);
            graph.AddEdge(new EquatableTaggedEdge<char, double>('1', '1', 7));
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Invoking(() => algorithm.Execute()).Should().Throw<NoPathFoundException>();
        }

        [Test]
        public void GraphWithCycle()
        {
            var graph = new AdjacencyGraph<char, EquatableTaggedEdge<char, double>>(true);
            graph.AddVertexRange("12345");
            var edges = new[]
            {
                new EquatableTaggedEdge<char, double>('1', '2', 1.0),
                new EquatableTaggedEdge<char, double>('1', '3', 12.0),
                new EquatableTaggedEdge<char, double>('1', '4', 0.5),
                new EquatableTaggedEdge<char, double>('2', '3', 3.0),
                new EquatableTaggedEdge<char, double>('2', '4', 2.0),
                new EquatableTaggedEdge<char, double>('3', '5', 1.0),
                new EquatableTaggedEdge<char, double>('5', '2', 6.0)
            };
            graph.AddEdgeRange(edges);

            var algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '5', 10);
            var paths = algorithm.Execute().ToArray();

            // Expecting to get 2 paths:
            // 1 => 1-2-3-5
            // 2 => 1-3-5
            // Consistently checking the result
            paths.Length.Should().Be(2);
            // 1
            var path0 = paths[0].ToArray();
            edges[0].Should().Be(path0[0]);
            edges[3].Should().Be(path0[1]);
            edges[5].Should().Be(path0[2]);
            // 2
            var path1 = paths[1].ToArray();
            edges[1].Should().Be(path1[0]);
            edges[5].Should().Be(path1[1]);
        }

        [Test]
        public void GraphWithMultiplePaths()
        {
            var graph = new AdjacencyGraph<string, EquatableTaggedEdge<string, double>>(false);
            graph.AddVertexRange(new[] { "A", "B", "C", "D" });
            var edges = new[]
            {
                new EquatableTaggedEdge<string, double>("A", "B", 5),
                new EquatableTaggedEdge<string, double>("A", "C", 6),
                new EquatableTaggedEdge<string, double>("B", "C", 7),
                new EquatableTaggedEdge<string, double>("B", "D", 8),
                new EquatableTaggedEdge<string, double>("C", "D", 9)
            };
            graph.AddEdgeRange(edges);

            var algorithm = new YenShortestPathsAlgorithm<string>(graph, "A", "D", 5);
            var paths = algorithm.Execute().ToArray();

            // Expecting to get 3 paths:
            // 1 => A-B-D
            // 2 => A-C-D
            // 3 => A-B-C-D
            // Consistently checking the result
            paths.Length.Should().Be(3);
            // 1
            var path0 = paths[0].ToArray();
            edges[0].Should().Be(path0[0]);
            edges[3].Should().Be(path0[1]);
            // 2
            var path1 = paths[1].ToArray();
            edges[1].Should().Be(path1[0]);
            edges[4].Should().Be(path1[1]);
            // 3
            var path2 = paths[2].ToArray();
            edges[0].Should().Be(path2[0]);
            edges[2].Should().Be(path2[1]);
            edges[4].Should().Be(path2[2]);
        }

        [Test]
        public void GraphWithMultiplePaths_KShortest()
        {
            var graph = new AdjacencyGraph<char, EquatableTaggedEdge<char, double>>(false);
            graph.AddVertexRange("CDEFGH");
            var edges = new[]
            {
                new EquatableTaggedEdge<char, double>('C', 'D', 3),
                new EquatableTaggedEdge<char, double>('C', 'E', 2),
                new EquatableTaggedEdge<char, double>('D', 'F', 4),
                new EquatableTaggedEdge<char, double>('E', 'D', 1),
                new EquatableTaggedEdge<char, double>('E', 'F', 2),
                new EquatableTaggedEdge<char, double>('E', 'G', 3),
                new EquatableTaggedEdge<char, double>('F', 'G', 2),
                new EquatableTaggedEdge<char, double>('F', 'H', 1),
                new EquatableTaggedEdge<char, double>('G', 'H', 2)
            };
            graph.AddEdgeRange(edges);

            // K = 5
            var algorithmK5 = new YenShortestPathsAlgorithm<char>(graph, 'C', 'H', 5);
            var paths = algorithmK5.Execute().ToArray();

            // Expecting to get 5 paths:
            // 1 => C-E-F-H
            // 2 => C-E-G-H
            // 3 => C-E-F-G-H
            // 4 => C-E-D-F-H
            // 5 => C-D-F-H
            // Consistently checking the result
            paths.Length.Should().Be(5);
            CheckFiveFirstPaths(paths);


            // K = 50
            var algorithmK50 = new YenShortestPathsAlgorithm<char>(graph, 'C', 'H', 50);
            paths = algorithmK50.Execute().ToArray();

            // Expecting to get 7 paths:
            // 1 => C-E-F-H
            // 2 => C-E-G-H
            // 3 => C-E-F-G-H
            // 4 => C-E-D-F-H
            // 5 => C-D-F-H
            // 6 => C-E-D-F-G-H
            // 7 => C-D-F-G-H
            // Consistently checking the result
            paths.Length.Should().Be(7);
            CheckFiveFirstPaths(paths);
            // 6
            var path5 = paths[5].ToArray();
            edges[1].Should().Be(path5[0]);    // C-E
            edges[3].Should().Be(path5[1]);    // E-D
            edges[2].Should().Be(path5[2]);    // D-F
            edges[6].Should().Be(path5[3]);    // F-G
            edges[8].Should().Be(path5[4]);    // G-H
            // 7
            var path6 = paths[6].ToArray();
            edges[0].Should().Be(path6[0]);    // C-D
            edges[2].Should().Be(path6[1]);    // D-F
            edges[6].Should().Be(path6[2]);    // F-G
            edges[8].Should().Be(path6[3]);    // G-H

            #region Local function

            void CheckFiveFirstPaths(YenShortestPathsAlgorithm<char>.SortedPath[] ps)
            {
                // 1
                var path0 = ps[0].ToArray();
                edges[1].Should().Be(path0[0]);    // C-E
                edges[4].Should().Be(path0[1]);    // E-F
                edges[7].Should().Be(path0[2]);    // F-H
                // 2
                var path1 = ps[1].ToArray();
                edges[1].Should().Be(path1[0]);    // C-E
                edges[5].Should().Be(path1[1]);    // E-G
                edges[8].Should().Be(path1[2]);    // G-H
                // 3
                var path2 = ps[2].ToArray();
                edges[1].Should().Be(path2[0]);    // C-E
                edges[4].Should().Be(path2[1]);    // E-F
                edges[6].Should().Be(path2[2]);    // F-G
                edges[8].Should().Be(path2[3]);    // G-H
                // 4
                var path3 = ps[3].ToArray();
                edges[1].Should().Be(path3[0]);    // C-E
                edges[3].Should().Be(path3[1]);    // E-D
                edges[2].Should().Be(path3[2]);    // D-F
                edges[7].Should().Be(path3[3]);    // F-H
                // 5
                var path4 = ps[4].ToArray();
                edges[0].Should().Be(path4[0]);    // C-D
                edges[2].Should().Be(path4[1]);    // D-F
                edges[7].Should().Be(path4[2]);    // F-H
            }

            #endregion
        }

        [Test]
        public void MultipleRunMethods()
        {
            var graph = GenerateGraph(
                out var graphEdges);

            // Default weight function and default filter function case
            var algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '5', 10);
            RunYenAndCheck(algorithm);

            // Custom weight function and default filter function case
            algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '5', 10, e => e.Tag);
            RunYenAndCheck(algorithm);

            // Default weight function and custom filter function case
            algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '5', 10, default, e => e);
            RunYenAndCheck(algorithm);

            // Custom weight function and custom filter function case
            algorithm = new YenShortestPathsAlgorithm<char>(graph, '1', '5', 10, e => e.Tag, e => e);
            RunYenAndCheck(algorithm);

            #region Local functions

            AdjacencyGraph<char, EquatableTaggedEdge<char, double>> GenerateGraph(
                out EquatableTaggedEdge<char, double>[] edges)
            {
                var g = new AdjacencyGraph<char, EquatableTaggedEdge<char, double>>(true);

                g.AddVertexRange("123456");
                edges = new[]
                {
                    new EquatableTaggedEdge<char, double>('1', '2', 7),
                    new EquatableTaggedEdge<char, double>('1', '3', 9),
                    new EquatableTaggedEdge<char, double>('1', '6', 14),
                    new EquatableTaggedEdge<char, double>('2', '3', 10),
                    new EquatableTaggedEdge<char, double>('2', '4', 15),
                    new EquatableTaggedEdge<char, double>('3', '4', 11),
                    new EquatableTaggedEdge<char, double>('3', '6', 2),
                    new EquatableTaggedEdge<char, double>('4', '5', 6),
                    new EquatableTaggedEdge<char, double>('5', '6', 9)
                };
                g.AddEdgeRange(edges);

                return g;
            }

            void RunYenAndCheck(YenShortestPathsAlgorithm<char> yen)
            {
                // Generate simple graph
                // like this https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
                // but with directed edges input graph
                var paths = yen.Execute().ToArray();

                // Expecting to get 3 paths:
                // 1 => 1-3-4-5
                // 2 => 1-2-4-5
                // 3 => 1-2-3-4-5
                // Consistently checking the result
                paths.Length.Should().Be(3);
                // 1
                var path0 = paths[0].ToArray();
                graphEdges[1].Should().Be(path0[0]);
                graphEdges[5].Should().Be(path0[1]);
                graphEdges[7].Should().Be(path0[2]);
                // 2
                var path1 = paths[1].ToArray();
                graphEdges[0].Should().Be(path1[0]);
                graphEdges[4].Should().Be(path1[1]);
                graphEdges[7].Should().Be(path1[2]);
                // 3
                var path2 = paths[2].ToArray();
                graphEdges[0].Should().Be(path2[0]);
                graphEdges[3].Should().Be(path2[1]);
                graphEdges[5].Should().Be(path2[2]);
                graphEdges[7].Should().Be(path2[3]);
            }

            #endregion
        }

        [Test]
        public void SortedPathHashCode()
        {
            var edges = new[]
            {
                new EquatableTaggedEdge<int, double>(1, 2, 1.0),
                new EquatableTaggedEdge<int, double>(2, 3, 1.0),
                new EquatableTaggedEdge<int, double>(3, 4, 1.0)
            };
            var path1 = new YenShortestPathsAlgorithm<int>.SortedPath(edges);
            var path2 = new YenShortestPathsAlgorithm<int>.SortedPath(edges);

            var path3 = new YenShortestPathsAlgorithm<int>.SortedPath(new[]
            {
                new EquatableTaggedEdge<int, double>(1, 2, 1.0),
                new EquatableTaggedEdge<int, double>(2, 3, 1.0),
                new EquatableTaggedEdge<int, double>(3, 4, 1.0)
            });

            path1.GetHashCode().Should().Be(path1.GetHashCode());
            path2.GetHashCode().Should().NotBe(path1.GetHashCode());
            path3.GetHashCode().Should().NotBe(path1.GetHashCode());
            path3.GetHashCode().Should().NotBe(path2.GetHashCode());
        }

        [Test]
        public void SortedPathEnumeration()
        {
            var edges = new[]
            {
                new EquatableTaggedEdge<int, double>(1, 2, 1.0),
                new EquatableTaggedEdge<int, double>(2, 3, 1.0),
                new EquatableTaggedEdge<int, double>(3, 4, 1.0)
            };

            var path = new YenShortestPathsAlgorithm<int>.SortedPath(edges);
            edges.Should().BeEquivalentTo(path);

            new YenShortestPathsAlgorithm<int>.SortedPath(
                Enumerable.Empty<EquatableTaggedEdge<int, double>>()).Should().BeEmpty();
        }
    }
}
