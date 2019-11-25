using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuikGraph.Algorithms;
using static QuikGraph.Tests.TestHelpers;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests for <see cref="IsHamiltonianGraphAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class HamiltonianGraphAlgorithmTests
    {
        [Test]
        public void IsHamiltonianEmpty()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(Enumerable.Empty<Vertices>());

            var algorithm = new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsFalse(algorithm.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonian()
        {
            // Hamiltonian
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(2, 3),
                new Vertices(1, 3),
                new Vertices(2, 4),
                new Vertices(3, 4)
            });

            var algorithm = new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsTrue(algorithm.IsHamiltonian());

            // Not Hamiltonian
            graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(2, 3),
                new Vertices(2, 4),
                new Vertices(3, 4)
            });

            algorithm = new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsFalse(algorithm.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianOneVertexWithCycle()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 1)
            });

            var algorithm = new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsTrue(algorithm.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianTwoVertices()
        {
            // Hamiltonian
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2)
            });

            var algorithm = new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsTrue(algorithm.IsHamiltonian());

            // Not Hamiltonian
            graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 1),
                new Vertices(2, 2)
            });

            algorithm = new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsFalse(algorithm.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianWithLoops()
        {
            var graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 1),
                new Vertices(1, 1),
                new Vertices(2, 2),
                new Vertices(2, 2),
                new Vertices(2, 2),
                new Vertices(3, 3),
                new Vertices(3, 3)
            });

            var algorithm = new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsFalse(algorithm.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianWithParallelEdges()
        {
            var graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(1, 2),
                new Vertices(3, 4),
                new Vertices(3, 4)
            });

            var algorithm = new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsFalse(algorithm.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianDiracsTheorem()
        {
            // This graph is Hamiltonian and satisfies Dirac's theorem. This test should work faster
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(1, 3),
                new Vertices(1, 4),
                new Vertices(1, 7),
                new Vertices(1, 8),
                new Vertices(1, 10),
                new Vertices(2, 6),
                new Vertices(2, 9),
                new Vertices(2, 4),
                new Vertices(2, 5),
                new Vertices(3, 4),
                new Vertices(3, 6),
                new Vertices(3, 7),
                new Vertices(3, 8),
                new Vertices(3, 8),
                new Vertices(4, 6),
                new Vertices(4, 5),
                new Vertices(4, 7),
                new Vertices(5, 7),
                new Vertices(5, 6),
                new Vertices(5, 9),
                new Vertices(5, 10),
                new Vertices(6, 9),
                new Vertices(6, 10),
                new Vertices(6, 7),
                new Vertices(7, 8),
                new Vertices(8, 9),
                new Vertices(8, 10),
                new Vertices(9, 10)
            });

            var algorithm = new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsTrue(algorithm.IsHamiltonian());
        }

        [Test]
        public void IsHamiltonianNotDiracsTheorem()
        {
            // This graph is Hamiltonian but don't satisfy Dirac's theorem. This test should work slowlier
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(1, 3),
                new Vertices(1, 4),
                new Vertices(1, 7),
                new Vertices(1, 8),
                new Vertices(1, 10),
                new Vertices(2, 6),
                new Vertices(2, 9),
                new Vertices(2, 4),
                new Vertices(3, 4),
                new Vertices(3, 6),
                new Vertices(3, 7),
                new Vertices(3, 8),
                new Vertices(4, 6),
                new Vertices(4, 5),
                new Vertices(4, 7),
                new Vertices(5, 7),
                new Vertices(5, 6),
                new Vertices(5, 9),
                new Vertices(5, 10),
                new Vertices(6, 9),
                new Vertices(6, 10),
                new Vertices(6, 7),
                new Vertices(7, 8),
                new Vertices(8, 9),
                new Vertices(8, 10),
                new Vertices(9, 10)
            });

            var algorithm = new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(graph);
            Assert.IsTrue(algorithm.IsHamiltonian());
        }

        #region Test helpers

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
                if (seq1 is null)
                    return seq2 is null;
                if (seq2 is null)
                    return false;
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

        private static int Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }

        #endregion

        [Test]
        public void IsHamiltonianCyclesBuilder()
        {
            UndirectedGraph<int, UndirectedEdge<int>> graph = CreateUndirectedGraph(new[]
            {
                new Vertices(1, 2),
                new Vertices(1, 3),
                new Vertices(1, 4),
                new Vertices(1, 7),
                new Vertices(1, 8),
                new Vertices(1, 10),
                new Vertices(2, 6),
                new Vertices(2, 9),
                new Vertices(2, 4),
                new Vertices(3, 4),
                new Vertices(3, 6),
                new Vertices(3, 7),
                new Vertices(3, 8),
                new Vertices(4, 6),
                new Vertices(4, 5),
                new Vertices(4, 7),
                new Vertices(5, 7),
                new Vertices(5, 6),
                new Vertices(5, 9),
                new Vertices(5, 10),
                new Vertices(6, 9),
                new Vertices(6, 10),
                new Vertices(6, 7),
                new Vertices(7, 8),
                new Vertices(8, 9),
                new Vertices(8, 10),
                new Vertices(9, 10)
            });

            var algorithm = new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(graph);

            var hashSet = new HashSet<List<int>>(new SequenceComparer<int>());
            hashSet.UnionWith(algorithm.GetPermutations());

            Assert.AreEqual(hashSet.Count, Factorial(graph.VertexCount));
        }

        [Test]
        public void IsHamiltonian_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new IsHamiltonianGraphAlgorithm<int, UndirectedEdge<int>>(null));

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(
                () => IsHamiltonianGraphAlgorithm.IsHamiltonian<int, UndirectedEdge<int>>(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}