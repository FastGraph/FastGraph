using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Serialization;
using QuikGraph.Tests;

namespace QuikGraph.Algorithms
{
    [TestFixture]
    internal class EulerianTrailAlgorithmTest : QuikGraphUnitTests
    {
#if !SUPPORTS_SYSTEM_DELEGATES
        // System.Core and NUnit both define this delegate that is conflicting
        // Defining it here allows to use it without conflict.
        public delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);
#endif

        [Test]
        [Ignore("Was already ignored")]
        public void EulerianTrailAll()
        {
            foreach (var g in TestGraphFactory.GetAdjacencyGraphs())
            {
                this.ComputeTrail(g, (s, t) => new Edge<string>(s, t));
            }
        }

        public void ComputeTrail<TVertex,TEdge>(
            IMutableVertexAndEdgeListGraph<TVertex,TEdge> g,
            Func<TVertex, TVertex, TEdge> edgeCreator)
            where TEdge : IEdge<TVertex>
        {
            if (g.VertexCount == 0)
                return;

            int oddCount = 0;
            foreach (var v in g.Vertices)
                if (g.OutDegree(v) % 2 == 0)
                    oddCount++;

            int circuitCount = EulerianTrailAlgorithm<TVertex,TEdge>.ComputeEulerianPathCount(g);
            if (circuitCount == 0)
                return;

            var trail = new EulerianTrailAlgorithm<TVertex,TEdge>(g);
            trail.AddTemporaryEdges((s, t) => edgeCreator(s, t));
            trail.Compute();
            var trails = trail.Trails();
            trail.RemoveTemporaryEdges();

            //Console.WriteLine("trails: {0}", trails.Count);
            //int index = 0;
            //foreach (var t in trails)
            //{
            //    Console.WriteLine("trail {0}", index++);
            //    foreach (Edge<string> edge in t)
            //        Console.WriteLine("\t{0}", t);
            //}

            // lets make sure all the edges are in the trail
            var edgeColors = new Dictionary<TEdge, GraphColor>(g.EdgeCount);
            foreach (var edge in g.Edges)
                edgeColors.Add(edge, GraphColor.White);
            foreach (var t in trails)
                foreach (var edge in t)
                    Assert.IsTrue(edgeColors.ContainsKey(edge));

        }
    }
}
