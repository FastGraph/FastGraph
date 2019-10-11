using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;

namespace QuikGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for <see cref="DijkstraShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class DijkstraShortestPathAlgorithmTests
    {
        #region Helpers

        private static void RunDijkstraAndCheck<TVertex, TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            [NotNull] TVertex root)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>(graph.EdgeCount);
            foreach (TEdge edge in graph.Edges)
                distances[edge] = graph.OutDegree(edge.Source) + 1;

            var algorithm = new DijkstraShortestPathAlgorithm<TVertex, TEdge>(
                graph,
                e => distances[e]);
            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (predecessors.Attach(algorithm))
                algorithm.Compute(root);

            Verify(algorithm, predecessors);
        }

        private static void Verify<TVertex, TEdge>(
            [NotNull] DijkstraShortestPathAlgorithm<TVertex, TEdge> algorithm,
            [NotNull] VertexPredecessorRecorderObserver<TVertex, TEdge> predecessors)
            where TEdge : IEdge<TVertex>
        {
            // Verify the result
            foreach (TVertex vertex in algorithm.VisitedGraph.Vertices)
            {
                if (!predecessors.VertexPredecessors.TryGetValue(vertex, out TEdge predecessor))
                    continue;
                if (predecessor.Source.Equals(vertex))
                    continue;
                bool found = algorithm.TryGetDistance(vertex, out _);
                Assert.AreEqual(found, algorithm.TryGetDistance(predecessor.Source, out _));
            }
        }

        #endregion

        [Test]
        public void Repro12359()
        {
            AdjacencyGraph<string, Edge<string>> graph = TestGraphFactory.LoadGraph(GetGraphFilePath("repro12359.graphml"));
            int i = 0;
            foreach (string root in graph.Vertices)
            {
                if (i++ > 5)
                    break;
                RunDijkstraAndCheck(graph, root);
            }
        }

        [Test]
        public void DijkstraAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_TMP())
            {
                foreach (string root in graph.Vertices)
                    RunDijkstraAndCheck(graph, root);
            }
        }

        [Test]
        public void RunOnLineGraph()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(true);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            graph.AddEdge(new Edge<int>(1, 2));
            graph.AddEdge(new Edge<int>(2, 3));

            var algorithm = new DijkstraShortestPathAlgorithm<int, Edge<int>>(graph, e => 1);
            algorithm.Compute(1);

            Assert.AreEqual(0d, algorithm.Distances[1]);
            Assert.AreEqual(2d, algorithm.Distances[3]);
            Assert.AreEqual(1d, algorithm.Distances[2]);
        }

        [Test]
        public void CheckPredecessorLineGraph()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(true);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            var e12 = new Edge<int>(1, 2); graph.AddEdge(e12);
            var e23 = new Edge<int>(2, 3); graph.AddEdge(e23);

            var algorithm = new DijkstraShortestPathAlgorithm<int, Edge<int>>(graph, e => 1);
            var vis = new VertexPredecessorRecorderObserver<int, Edge<int>>();
            using (vis.Attach(algorithm))
                algorithm.Compute(1);

            Assert.IsTrue(vis.TryGetPath(2, out IEnumerable<Edge<int>> path));
            Edge<int>[] pathArray = path.ToArray();
            Assert.AreEqual(1, pathArray.Length);
            Assert.AreEqual(e12, pathArray[0]);

            Assert.IsTrue(vis.TryGetPath(3, out path));
            pathArray = path.ToArray();
            Assert.AreEqual(2, pathArray.Length);
            Assert.AreEqual(e12, pathArray[0]);
            Assert.AreEqual(e23, pathArray[1]);
        }

        [Test]
        public void RunOnDoubleLineGraph()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(true);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            var e12 = new Edge<int>(1, 2); graph.AddEdge(e12);
            var e23 = new Edge<int>(2, 3); graph.AddEdge(e23);
            var e13 = new Edge<int>(1, 3); graph.AddEdge(e13);

            var algorithm = new DijkstraShortestPathAlgorithm<int, Edge<int>>(graph, e => 1);
            algorithm.Compute(1);

            Assert.AreEqual(0.0, algorithm.Distances[1]);
            Assert.AreEqual(1.0, algorithm.Distances[2]);
            Assert.AreEqual(1.0, algorithm.Distances[3]);
        }

        [Test]
        public void CheckPredecessorDoubleLineGraph()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(true);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            var e12 = new Edge<int>(1, 2); graph.AddEdge(e12);
            var e23 = new Edge<int>(2, 3); graph.AddEdge(e23);
            var e13 = new Edge<int>(1, 3); graph.AddEdge(e13);

            var algorithm = new DijkstraShortestPathAlgorithm<int, Edge<int>>(graph, e => 1);
            var vis = new VertexPredecessorRecorderObserver<int, Edge<int>>();
            using (vis.Attach(algorithm))
                algorithm.Compute(1);

            Assert.IsTrue(vis.TryGetPath(2, out IEnumerable<Edge<int>> path));
            Edge<int>[] pathArray = path.ToArray();
            Assert.AreEqual(1, pathArray.Length);
            Assert.AreEqual(e12, pathArray[0]);

            Assert.IsTrue(vis.TryGetPath(3, out path));
            pathArray = path.ToArray();
            Assert.AreEqual(1, pathArray.Length);
            Assert.AreEqual(e13, pathArray[0]);
        }

        [Test]
        public void CreateGraph()
        {
            var graph = new AdjacencyGraph<string, Edge<string>>(true);

            // Add some vertices to the graph
            graph.AddVertex("A");
            graph.AddVertex("B");
            graph.AddVertex("D");
            graph.AddVertex("C");
            graph.AddVertex("E");

            // Create the edges
            // ReSharper disable InconsistentNaming
            var a_b = new Edge<string>("A", "B");
            var a_c = new Edge<string>("A", "C");
            var b_e = new Edge<string>("B", "E");
            var c_d = new Edge<string>("C", "D");
            var d_e = new Edge<string>("D", "E");
            // ReSharper restore InconsistentNaming

            // Add edges to the graph
            graph.AddEdge(a_b);
            graph.AddEdge(a_c);
            graph.AddEdge(c_d);
            graph.AddEdge(d_e);
            graph.AddEdge(b_e);

            // Define some weights to the edges
            var weight = new Dictionary<Edge<string>, double>(graph.EdgeCount)
            {
                [a_b] = 30,
                [a_c] = 30,
                [b_e] = 60,
                [c_d] = 40,
                [d_e] = 4
            };

            var algorithm = new DijkstraShortestPathAlgorithm<string, Edge<string>>(graph, e => weight[e]);

            // Attach a Vertex Predecessor Recorder Observer to give us the paths
            var predecessorObserver = new VertexPredecessorRecorderObserver<string, Edge<string>>();
            using (predecessorObserver.Attach(algorithm))
                // Run the algorithm with A set to be the source
                algorithm.Compute("A");

            Assert.AreEqual(74, algorithm.Distances["E"], double.Epsilon);
        }

        [Test]
        public void Compute()
        {
            var graph = new AdjacencyGraph<char, Edge<char>>();
            var distances = new Dictionary<Edge<char>, double>();

            graph.AddVertexRange("ABCDE");
            AddEdge('A', 'C', 1);
            AddEdge('B', 'B', 2);
            AddEdge('B', 'D', 1);
            AddEdge('B', 'E', 2);
            AddEdge('C', 'B', 7);
            AddEdge('C', 'D', 3);
            AddEdge('D', 'E', 1);
            AddEdge('E', 'A', 1);
            AddEdge('E', 'B', 1);

            var algorithm = new DijkstraShortestPathAlgorithm<char, Edge<char>>(graph, AlgorithmExtensions.GetIndexer(distances));
            var predecessors = new VertexPredecessorRecorderObserver<char, Edge<char>>();
            using (predecessors.Attach(algorithm))
                algorithm.Compute('A');

            Assert.AreEqual(0, algorithm.Distances['A']);
            Assert.AreEqual(6, algorithm.Distances['B']);
            Assert.AreEqual(1, algorithm.Distances['C']);
            Assert.AreEqual(4, algorithm.Distances['D']);
            Assert.AreEqual(5, algorithm.Distances['E']);

            #region Local function

            void AddEdge(char source, char target, double weight)
            {
                var edge = new Edge<char>(source, target);
                distances[edge] = weight;
                graph.AddEdge(edge);
            }

            #endregion
        }
    }
}
