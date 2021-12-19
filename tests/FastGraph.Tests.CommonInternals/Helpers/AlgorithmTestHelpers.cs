#nullable enable

using FastGraph.Algorithms;
using FluentAssertions.Execution;

namespace FastGraph.Tests.Algorithms
{
    /// <summary>
    /// Test helpers for algorithms.
    /// </summary>
    internal static class AlgorithmTestHelpers
    {
        #region Test helpers

        [CustomAssertion]
        public static void AssertAlgorithmState<TGraph>(
            AlgorithmBase<TGraph> algorithm,
            TGraph treatedGraph,
            ComputationState state = ComputationState.NotRunning)
            where TGraph : notnull
        {
            using (_ = new AssertionScope())
            {
                treatedGraph.Should().NotBeNull();
                algorithm.VisitedGraph.Should().BeSameAs(treatedGraph);
                algorithm.Services.Should().NotBeNull();
                algorithm.SyncRoot.Should().NotBeNull();
                algorithm.State.Should().Be(state);
            }
        }

        #endregion
    }
}
