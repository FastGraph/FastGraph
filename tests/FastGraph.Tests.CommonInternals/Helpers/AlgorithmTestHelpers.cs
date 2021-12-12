#nullable enable

using NUnit.Framework;
using FastGraph.Algorithms;

namespace FastGraph.Tests.Algorithms
{
    /// <summary>
    /// Test helpers for algorithms.
    /// </summary>
    internal static class AlgorithmTestHelpers
    {
        #region Test helpers

        public static void AssertAlgorithmState<TGraph>(
            AlgorithmBase<TGraph> algorithm,
            TGraph treatedGraph,
            ComputationState state = ComputationState.NotRunning)
            where TGraph : notnull
        {
            Assert.IsNotNull(treatedGraph);
            Assert.AreSame(treatedGraph, algorithm.VisitedGraph);
            Assert.IsNotNull(algorithm.Services);
            Assert.IsNotNull(algorithm.SyncRoot);
            Assert.AreEqual(state, algorithm.State);
        }

        #endregion
    }
}
