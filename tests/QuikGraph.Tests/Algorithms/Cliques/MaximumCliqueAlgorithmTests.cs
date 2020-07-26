using System;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.Cliques;
using QuikGraph.Algorithms.Services;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.Cliques
{
    /// <summary>
    /// Tests for <see cref="MaximumCliqueAlgorithmBase{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class MaximumCliqueAlgorithmTests
    {
        #region Test classes

        private class TestMaximumCliqueAlgorithm<TVertex, TEdge> : MaximumCliqueAlgorithmBase<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            public TestMaximumCliqueAlgorithm([NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
                : base(visitedGraph)
            {
            }

            public TestMaximumCliqueAlgorithm([CanBeNull] IAlgorithmComponent host, [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
                : base(host, visitedGraph)
            {
            }

            /// <inheritdoc />
            protected override void InternalCompute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new TestMaximumCliqueAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmState(algorithm, graph);

            algorithm = new TestMaximumCliqueAlgorithm<int, Edge<int>>(null, graph);
            AssertAlgorithmState(algorithm, graph);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new TestMaximumCliqueAlgorithm<int, Edge<int>>(null));

            Assert.Throws<ArgumentNullException>(
                () => new TestMaximumCliqueAlgorithm<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}