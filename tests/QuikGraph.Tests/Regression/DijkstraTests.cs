using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;

namespace QuikGraph.Tests.Regression
{
    /// <summary>
    /// Tests for <see cref="DijkstraShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class DijkstraTests
    {
        [Test]
        public void Scenario()
        {
            AdjacencyGraph<string, Edge<string>> graph = CreateGraph(out Dictionary<Edge<string>, double> edgeCosts);

            // Run Dijkstra on this graph
            var dijkstra = new DijkstraShortestPathAlgorithm<string, Edge<string>>(graph, e => edgeCosts[e]);

            // Attach a Vertex Predecessor Recorder Observer to give us the paths
            var predecessorObserver = new VertexPredecessorRecorderObserver<string, Edge<string>>();
            using (predecessorObserver.Attach(dijkstra))
            {
                // Run the algorithm with A as source
                dijkstra.Compute("A");
            }

            foreach (KeyValuePair<string, Edge<string>> pair in predecessorObserver.VerticesPredecessors)
                Console.WriteLine($"If you want to get to {pair.Key} you have to enter through the in edge {pair.Value}.");

            foreach (string vertex in graph.Vertices)
            {
                double distance = AlgorithmExtensions.ComputePredecessorCost(
                    predecessorObserver.VerticesPredecessors,
                    edgeCosts,
                    vertex);
                Console.WriteLine($"A -> {vertex}: {distance}");
            }


            #region Local function

            AdjacencyGraph<string, Edge<string>> CreateGraph(out Dictionary<Edge<string>, double> costs)
            {
                var g = new AdjacencyGraph<string, Edge<string>>(true);

                // Add some vertices to the graph
                g.AddVertex("A");
                g.AddVertex("B");
                g.AddVertex("C");
                g.AddVertex("D");
                g.AddVertex("E");
                g.AddVertex("F");
                g.AddVertex("G");
                g.AddVertex("H");
                g.AddVertex("I");
                g.AddVertex("J");

                // Create the edges
                // ReSharper disable InconsistentNaming
                var a_b = new Edge<string>("A", "B");
                var a_d = new Edge<string>("A", "D");
                var b_a = new Edge<string>("B", "A");
                var b_c = new Edge<string>("B", "C");
                var b_e = new Edge<string>("B", "E");
                var c_b = new Edge<string>("C", "B");
                var c_f = new Edge<string>("C", "F");
                var c_j = new Edge<string>("C", "J");
                var d_e = new Edge<string>("D", "E");
                var d_g = new Edge<string>("D", "G");
                var e_d = new Edge<string>("E", "D");
                var e_f = new Edge<string>("E", "F");
                var e_h = new Edge<string>("E", "H");
                var f_i = new Edge<string>("F", "I");
                var f_j = new Edge<string>("F", "J");
                var g_d = new Edge<string>("G", "D");
                var g_h = new Edge<string>("G", "H");
                var h_g = new Edge<string>("H", "G");
                var h_i = new Edge<string>("H", "I");
                var i_f = new Edge<string>("I", "F");
                var i_j = new Edge<string>("I", "J");
                var i_h = new Edge<string>("I", "H");
                var j_f = new Edge<string>("J", "F");
                // ReSharper restore InconsistentNaming

                // Add the edges
                g.AddEdge(a_b);
                g.AddEdge(a_d);
                g.AddEdge(b_a);
                g.AddEdge(b_c);
                g.AddEdge(b_e);
                g.AddEdge(c_b);
                g.AddEdge(c_f);
                g.AddEdge(c_j);
                g.AddEdge(d_e);
                g.AddEdge(d_g);
                g.AddEdge(e_d);
                g.AddEdge(e_f);
                g.AddEdge(e_h);
                g.AddEdge(f_i);
                g.AddEdge(f_j);
                g.AddEdge(g_d);
                g.AddEdge(g_h);
                g.AddEdge(h_g);
                g.AddEdge(h_i);
                g.AddEdge(i_f);
                g.AddEdge(i_h);
                g.AddEdge(i_j);
                g.AddEdge(j_f);

                // Define some weights to the edges
                costs = new Dictionary<Edge<string>, double>(g.EdgeCount)
                {
                    [a_b] = 4,
                    [a_d] = 1,
                    [b_a] = 74,
                    [b_c] = 2,
                    [b_e] = 12,
                    [c_b] = 12,
                    [c_f] = 74,
                    [c_j] = 12,
                    [d_e] = 32,
                    [d_g] = 22,
                    [e_d] = 66,
                    [e_f] = 76,
                    [e_h] = 33,
                    [f_i] = 11,
                    [f_j] = 21,
                    [g_d] = 12,
                    [g_h] = 10,
                    [h_g] = 2,
                    [h_i] = 72,
                    [i_f] = 31,
                    [i_h] = 18,
                    [i_j] = 7,
                    [j_f] = 8
                };

                return g;
            }

            #endregion
        }
    }
}