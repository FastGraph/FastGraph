using System;
using NUnit.Framework;
using QuickGraph.Algorithms.TopologicalSort;
using QuikGraph.Tests;

namespace QuickGraph.Algorithms
{
    [TestFixture]
    internal class UndirectedTopologicalSortAlgorithmTest : QuikGraphUnitTests
    {
        public void Compute(IUndirectedGraph<string, Edge<string>> g)
        {
            UndirectedTopologicalSortAlgorithm<string, Edge<string>> topo =
                new UndirectedTopologicalSortAlgorithm<string, Edge<string>>(g);
            topo.AllowCyclicGraph = true;
            topo.Compute();

            Display(topo);
        }

        private void Display(UndirectedTopologicalSortAlgorithm<string, Edge<string>> topo)
        {
            int index = 0;
            foreach (string v in topo.SortedVertices)
                Console.WriteLine("{0}: {1}", index++, v);
        }
    }
}
