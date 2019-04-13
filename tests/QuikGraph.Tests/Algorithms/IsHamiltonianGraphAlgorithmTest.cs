using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuikGraph.Tests;
using QuikGraph.Tests.Algorithms;

namespace QuickGraph.Tests.Algorithms
{
    [TestFixture]
    internal class IsHamiltonianGraphAlgorithmTest : QuikGraphUnitTests
    {
        private UndirectedGraph<int, UndirectedEdge<int>> constructGraph(IEnumerable<Vertices> vertices)
        {
            var g = new UndirectedGraph<int, UndirectedEdge<int>>();
            foreach (var pair in vertices)
            {
                g.AddVerticesAndEdge(new UndirectedEdge<int>(pair.Source, pair.Target));
            }
            return g;
        }

        [Test]
        public void IsHamiltonianTrue()
        {
            var g = constructGraph(new[] {new Vertices(1, 2), new Vertices(2, 3),
                    new Vertices(1, 3), new Vertices(2, 4), new Vertices(3, 4)});
            var gAlgo = new QuickGraph.Algorithms.IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianFalse()
        {
            var g = constructGraph(new[] { new Vertices(1, 2),
                    new Vertices(2, 3), new Vertices(2, 4), new Vertices(3, 4)});
            var gAlgo = new QuickGraph.Algorithms.IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianEmpty()
        {
            var g = constructGraph(Enumerable.Empty<Vertices>());
            var gAlgo = new QuickGraph.Algorithms.IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianOneVertexWithCycle()
        {
            var g = constructGraph(new[] { new Vertices(1, 1) });
            var gAlgo = new QuickGraph.Algorithms.IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianTwoVerticesTrue()
        {
            var g = constructGraph(new[] { new Vertices(1, 2) });
            var gAlgo = new QuickGraph.Algorithms.IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianTwoVerticesFalse()
        {
            var g = constructGraph(new[] { new Vertices(1, 1), new Vertices(2, 2) });
            var gAlgo = new QuickGraph.Algorithms.IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianWithLoops()
        {
            var g = constructGraph(new[] { new Vertices(1, 1), new Vertices(1, 1),
                new Vertices(2, 2), new Vertices(2, 2), new Vertices(2, 2),
                new Vertices(3, 3), new Vertices(3, 3)});
            var gAlgo = new QuickGraph.Algorithms.IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsHamiltonian());
        }


        [Test]
        public void IsHamiltonianWithParallelEdges()
        {
            var g = constructGraph(new[] { new Vertices(1, 2), new Vertices(1, 2),
                new Vertices(3, 4), new Vertices(3, 4)});
            var gAlgo = new QuickGraph.Algorithms.IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsFalse(gAlgo.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonian10VerticesDiracsTheorem()
        {
            // This graph is hamiltonian and satisfies Dirac's theorem. This test should work faster
            var g = constructGraph(new[] { new Vertices(1, 2),
                new Vertices(1, 3), new Vertices(1, 4), new Vertices(1, 7),
                new Vertices(1, 8), new Vertices(1, 10), new Vertices(2, 6),
                new Vertices(2, 9), new Vertices(2, 4), new Vertices(2, 5),
                new Vertices(3, 4), new Vertices(3, 6), new Vertices(3, 7),
                new Vertices(3, 8), new Vertices(4, 6), new Vertices(4, 5),
                new Vertices(4, 7), new Vertices(5, 7), new Vertices(5, 6),
                new Vertices(5, 9), new Vertices(5, 10), new Vertices(6, 9),
                new Vertices(6, 10), new Vertices(6, 7), new Vertices(7, 8),
                new Vertices(8, 9), new Vertices(8, 10), new Vertices(9, 10)
            });
            var gAlgo = new QuickGraph.Algorithms.IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonian10VerticesNotDiracsTheorem()
        {
            // This graph is hamiltonian but don't satisfy Dirac's theorem. This test should work slowlier
            var g = constructGraph(new[] { new Vertices(1, 2),
                new Vertices(1, 3), new Vertices(1, 4), new Vertices(1, 7),
                new Vertices(1, 8), new Vertices(1, 10), new Vertices(2, 6),
                new Vertices(2, 9), new Vertices(2, 4),
                new Vertices(3, 4), new Vertices(3, 6), new Vertices(3, 7),
                new Vertices(3, 8), new Vertices(4, 6), new Vertices(4, 5),
                new Vertices(4, 7), new Vertices(5, 7), new Vertices(5, 6),
                new Vertices(5, 9), new Vertices(5, 10), new Vertices(6, 9),
                new Vertices(6, 10), new Vertices(6, 7), new Vertices(7, 8),
                new Vertices(8, 9), new Vertices(8, 10), new Vertices(9, 10)
            });
            var gAlgo = new QuickGraph.Algorithms.IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(g);
            Assert.IsTrue(gAlgo.IsHamiltonian());
        }

        private class SequenceComparer<T>
#if SUPPORTS_ENUMERABLE_COVARIANT
            : IEqualityComparer<IEnumerable<T>>
#else
            : IEqualityComparer<List<T>>
#endif
        {
#if SUPPORTS_ENUMERABLE_COVARIANT
            public bool Equals(IEnumerable<T> seq1, IEnumerable<T> seq2)
#else
            public bool Equals(List<T> seq1, List<T> seq2)
#endif
            {
                return seq1.SequenceEqual(seq2);
            }

#if SUPPORTS_ENUMERABLE_COVARIANT
            public int GetHashCode(IEnumerable<T> seq)
#else
            public int GetHashCode(List<T> seq)
#endif
            {
                int hash = 1234567;
                foreach (T elem in seq)
                    hash = hash * 37 + elem.GetHashCode();
                return hash;
            }
        }

        private int Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }

        [Test]
        public void IsHamiltonianTestCyclesBuilder()
        {
            var g = constructGraph(new[] { new Vertices(1, 2),
                new Vertices(1, 3), new Vertices(1, 4), new Vertices(1, 7),
                new Vertices(1, 8), new Vertices(1, 10), new Vertices(2, 6),
                new Vertices(2, 9), new Vertices(2, 4),
                new Vertices(3, 4), new Vertices(3, 6), new Vertices(3, 7),
                new Vertices(3, 8), new Vertices(4, 6), new Vertices(4, 5),
                new Vertices(4, 7), new Vertices(5, 7), new Vertices(5, 6),
                new Vertices(5, 9), new Vertices(5, 10), new Vertices(6, 9),
                new Vertices(6, 10), new Vertices(6, 7), new Vertices(7, 8),
                new Vertices(8, 9), new Vertices(8, 10), new Vertices(9, 10)
            });
            var gAlgo = new QuickGraph.Algorithms.IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(g);

            var hashSet = new HashSet<List<int>>(new SequenceComparer<int>());
            hashSet.UnionWith(gAlgo.GetPermutations());

            Assert.AreEqual(hashSet.Count, Factorial(g.VertexCount));
        }
    }
}