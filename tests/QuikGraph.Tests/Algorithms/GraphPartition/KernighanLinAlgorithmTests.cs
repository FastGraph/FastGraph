#if SUPPORTS_KERNIGHANLIN_ALGORITHM
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.GraphPartition;

namespace QuikGraph.Tests.Algorithms.GraphPartitioning
{
    /// <summary>
    /// Tests for <see cref="KernighanLinAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class KernighanLinAlgorithmTests : QuikGraphUnitTests
    {
        [NotNull]
        private static UndirectedGraph<int, TaggedUndirectedEdge<int, double>> CreateGraph(
            [NotNull, ItemNotNull] IEnumerable<TaggedUndirectedEdge<int, double>> edges)
        {
            var graph = new UndirectedGraph<int, TaggedUndirectedEdge<int, double>>(true);
            foreach (TaggedUndirectedEdge<int, double> edge in edges)
            {
                graph.AddVerticesAndEdge(edge);
            }

            return graph;
        }

        [Test]
        public void GraphPartitioningTest1()
        {
            var edges = new List<TaggedUndirectedEdge<int, double>>
            {
                new TaggedUndirectedEdge<int,double>(0, 1, 100),
                new TaggedUndirectedEdge<int,double>(1, 2, 20),
                new TaggedUndirectedEdge<int,double>(2, 3, 10),
                new TaggedUndirectedEdge<int,double>(3, 1, 50)
            };

            UndirectedGraph<int, TaggedUndirectedEdge<int, double>> graph = CreateGraph(edges);
            var setA = new SortedSet<int>();
            var setB = new SortedSet<int>();
            setA.Add(0);
            setA.Add(1);
            setB.Add(3);
            setB.Add(2);

            var expected = new Partition<int>(setA, setB, 3);

            var algorithm = new KernighanLinAlgorithm<int, TaggedUndirectedEdge<int, double>>(
                graph,
                1);
            algorithm.Compute();

            Assert.IsTrue(PartitionHelpers.AreEquivalent(expected, algorithm.Partition));
        }

        [Test]
        public void GraphPartitioningTest2()
        {
            var edges = new List<TaggedUndirectedEdge<int, double>>
            {
                new TaggedUndirectedEdge<int, double>(0, 1, 1),
                new TaggedUndirectedEdge<int, double>(1, 2, 1),
                new TaggedUndirectedEdge<int, double>(2, 3, 1),
                new TaggedUndirectedEdge<int, double>(3, 4, 1),
                new TaggedUndirectedEdge<int, double>(4, 0, 1),
                new TaggedUndirectedEdge<int, double>(4, 1, 1)
            };

            UndirectedGraph<int, TaggedUndirectedEdge<int, double>> graph = CreateGraph(edges);
            var setA = new SortedSet<int>();
            var setB = new SortedSet<int>();
            for (int i = 0; i < 5; ++i)
            {
                if (i == 2 || i == 3)
                    setA.Add(i);
                else
                    setB.Add(i);
            }

            var expected = new Partition<int>(setA, setB, 3);

            var algorithm = new KernighanLinAlgorithm<int, TaggedUndirectedEdge<int, double>>(graph, 1);
            algorithm.Compute();

            Assert.IsTrue(PartitionHelpers.AreEquivalent(expected, algorithm.Partition));
        }

        /*
         9   10        11
         *    *         *
         *    *         *
         6****7*********8
         *    **       **
         *    *  *   *  *  
         *    *    *    *
         *    *  *   *  *
         *    **       **
         3****4*********5
              *         
              *         
         0****1*********2
        */
        [Test]
        public void GraphPartitioningTest3()
        {
            var edges = new List<TaggedUndirectedEdge<int, double>>
            {
                new TaggedUndirectedEdge<int, double>(0, 1, 1),
                new TaggedUndirectedEdge<int, double>(1, 2, 50),
                new TaggedUndirectedEdge<int, double>(1, 4, 5),
                new TaggedUndirectedEdge<int, double>(4, 3, 1),
                new TaggedUndirectedEdge<int, double>(3, 6, 10),
                new TaggedUndirectedEdge<int, double>(4, 5, 1),
                new TaggedUndirectedEdge<int, double>(4, 7, 25),
                new TaggedUndirectedEdge<int, double>(4, 8, 100),
                new TaggedUndirectedEdge<int, double>(5, 7, 1),
                new TaggedUndirectedEdge<int, double>(5, 8, 3),
                new TaggedUndirectedEdge<int, double>(6, 7, 1),
                new TaggedUndirectedEdge<int, double>(6, 9, 2),
                new TaggedUndirectedEdge<int, double>(7, 8, 1),
                new TaggedUndirectedEdge<int, double>(7, 10, 5),
                new TaggedUndirectedEdge<int, double>(8, 11, 1)
            };

            UndirectedGraph<int, TaggedUndirectedEdge<int, double>> graph = CreateGraph(edges);
            var setA = new SortedSet<int>();
            var setB = new SortedSet<int>();
            for (int i = 0; i < 12; i++)
            {
                if (i < 4 || i == 6 || i == 9)
                    setB.Add(i);
                else
                    setA.Add(i);
            }

            var expected = new Partition<int>(setA, setB, 3);
            var algorithm = new KernighanLinAlgorithm<int, TaggedUndirectedEdge<int, double>>(graph, 1);
            algorithm.Compute();

            Assert.IsTrue(PartitionHelpers.AreEquivalent(expected, algorithm.Partition));
        }
    }
}
#endif