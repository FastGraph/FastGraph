#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms.Cliques;
using FastGraph.Algorithms.Services;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.Tests.Algorithms.Cliques
{
    /// <summary>
    /// Tests for <see cref="MaximumCliqueAlgorithmBase{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class MaximumCliqueAlgorithmTests
    {
        #region Test classes

        private class TestMaximumCliqueAlgorithm<TVertex, TEdge> : MaximumCliqueAlgorithmBase<TVertex, TEdge>
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            public TestMaximumCliqueAlgorithm(IUndirectedGraph<TVertex, TEdge> visitedGraph)
                : base(visitedGraph)
            {
            }

            public TestMaximumCliqueAlgorithm(IAlgorithmComponent? host, IUndirectedGraph<TVertex, TEdge> visitedGraph)
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

            algorithm = new TestMaximumCliqueAlgorithm<int, Edge<int>>(default, graph);
            AssertAlgorithmState(algorithm, graph);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625

            Invoking(() => new TestMaximumCliqueAlgorithm<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new TestMaximumCliqueAlgorithm<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();

#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
