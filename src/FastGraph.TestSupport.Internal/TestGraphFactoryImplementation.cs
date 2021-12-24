#nullable enable

using JetBrains.Annotations;

namespace FastGraph.Tests
{
    /// <summary>
    /// Factory to select a sampling of source graphs and then load them as a specified graph type for use in test cases.
    /// When `FULL_SLOW_TESTS_RUN` is defined, all graphs are returned instead of a sampling.
    /// </summary>
    internal class TestGraphFactoryImplementation : ITestGraphFactory
    {
        private const uint SlowTestRate = 5;

        private IEnumerable<NamedTestGraphSource> TestGraphSources { get; }

        public TestGraphFactoryImplementation()
            : this(TestGraphSourceProvider.Instance.AllGeneralPurpose)
        {
        }

        public TestGraphFactoryImplementation(IEnumerable<NamedTestGraphSource> testGraphSources)
        {
            TestGraphSources = testGraphSources;
        }

        /// <summary>
        /// Creates adjacency graphs.
        /// </summary>
        [Pure]
        public IEnumerable<TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string>> SampleAdjacencyGraphs() =>
            SampleAdjacencyGraphSources().Select(tgs => tgs.CreateInstanceHandle());

        /// <summary>
        /// Creates adjacency graphs (version manageable with define for slow tests).
        /// </summary>
        [Pure]
        public IEnumerable<TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string>> SampleAdjacencyGraphs(uint rateWhenNotSlowTestsRun) =>
            SampleAdjacencyGraphSources(rateWhenNotSlowTestsRun).Select(tgs => tgs.CreateInstanceHandle());

        /// <summary>
        /// Creates bidirectional graphs.
        /// </summary>
        [Pure]
        public IEnumerable<TestGraphInstance<BidirectionalGraph<string, Edge<string>>, string>> SampleBidirectionalGraphs() =>
            SampleBidirectionalGraphSources().Select(tgs => tgs.CreateInstanceHandle());

        /// <summary>
        /// Creates undirected graphs.
        /// </summary>
        [Pure]
        public IEnumerable<TestGraphInstance<UndirectedGraph<string, Edge<string>>, string>> SampleUndirectedGraphs() =>
            SampleUndirectedGraphSources().Select(tgs => tgs.CreateInstanceHandle());

        /// <summary>
        /// Creates undirected graphs (version manageable with define for slow tests).
        /// </summary>
        [Pure]
        public IEnumerable<TestGraphInstance<UndirectedGraph<string, Edge<string>>, string>> SampleUndirectedGraphs(uint rateWhenNotSlowTestsRun) =>
            SampleUndirectedGraphSources(rateWhenNotSlowTestsRun).Select(tgs => tgs.CreateInstanceHandle());

        /// <summary>
        /// Creates adjacency graphs (filterable).
        /// </summary>
        [Pure]
        private IEnumerable<DeserializableNamedTestGraphSource<AdjacencyGraph<string, Edge<string>>, string>> SampleAdjacencyGraphSources()
        {
            yield return DeserializableNamedTestGraphSource<AdjacencyGraph<string, Edge<string>>, string>.CreateEmpty();
            foreach (var graphSource in Sample(TestGraphSources))
            {
                yield return graphSource.DeferDeserializeAsAdjacencyGraph();
            }
        }

        /// <summary>
        /// Creates adjacency graphs (filterable).
        /// </summary>
        [Pure]
        private IEnumerable<DeserializableNamedTestGraphSource<AdjacencyGraph<string, Edge<string>>, string>> SampleAdjacencyGraphSources(uint rateWhenNotSlowTestsRun)
        {
            yield return DeserializableNamedTestGraphSource<AdjacencyGraph<string, Edge<string>>, string>.CreateEmpty();
            foreach (var graphSource in Sample(TestGraphSources, rateWhenNotSlowTestsRun))
            {
                yield return graphSource.DeferDeserializeAsAdjacencyGraph();
            }
        }

        /// <summary>
        /// Creates bidirectional graphs (filterable).
        /// </summary>
        [Pure]
        private IEnumerable<DeserializableNamedTestGraphSource<BidirectionalGraph<string, Edge<string>>, string>> SampleBidirectionalGraphSources()
        {
            yield return DeserializableNamedTestGraphSource<BidirectionalGraph<string, Edge<string>>, string>.CreateEmpty();
            foreach (var graphSource in Sample(TestGraphSources))
            {
                yield return graphSource.DeferDeserializeAsBidirectionalGraph();
            }
        }

        /// <summary>
        /// Creates undirected graphs (filterable).
        /// </summary>
        [Pure]
        private IEnumerable<DeserializableNamedTestGraphSource<UndirectedGraph<string, Edge<string>>, string>> SampleUndirectedGraphSources()
        {
            yield return DeserializableNamedTestGraphSource<UndirectedGraph<string, Edge<string>>, string>.CreateEmpty();
            foreach (var graphSource in Sample(TestGraphSources))
            {
                yield return graphSource.DeferDeserializeAsUndirectedGraph();
            }
        }

        /// <summary>
        /// Creates undirected graphs (filterable).
        /// </summary>
        [Pure]
        private IEnumerable<DeserializableNamedTestGraphSource<UndirectedGraph<string, Edge<string>>, string>> SampleUndirectedGraphSources(uint rateWhenNotSlowTestsRun)
        {
            yield return DeserializableNamedTestGraphSource<UndirectedGraph<string, Edge<string>>, string>.CreateEmpty();
            foreach (var graphSource in Sample(TestGraphSources, rateWhenNotSlowTestsRun))
            {
                yield return graphSource.DeferDeserializeAsUndirectedGraph();
            }
        }

        [Pure]
        private static IEnumerable<NamedTestGraphSource> Sample(IEnumerable<NamedTestGraphSource> sources)
        {
#if !FULL_SLOW_TESTS_RUN
            var rateWhenNotSlowTestsRun = SlowTestRate;
#else
            return sources;
#endif

#if !FULL_SLOW_TESTS_RUN
            // 1 over SlowTestRate
            return sources.Where((_, index) => index % rateWhenNotSlowTestsRun == 0);
#endif
        }

        [Pure]
        private static IEnumerable<NamedTestGraphSource> Sample(IEnumerable<NamedTestGraphSource> sources, uint rateWhenNotSlowTestsRun)
        {
#if !FULL_SLOW_TESTS_RUN
            // 1 over SlowTestRate
            return sources.Where((_, index) => index % rateWhenNotSlowTestsRun == 0);
#else
            return sources;
#endif
        }
    }
}
