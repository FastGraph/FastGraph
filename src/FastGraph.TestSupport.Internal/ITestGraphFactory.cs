#nullable enable

namespace FastGraph.Tests;

/// <summary>
/// Factory to select a sampling of source graphs and then load them as a specified graph type for use in test cases.
/// When `FULL_SLOW_TESTS_RUN` is defined, all graphs are returned instead of a sampling.
/// </summary>
internal interface ITestGraphFactory
{
    /// <summary>
    /// Creates adjacency graphs.
    /// </summary>
    IEnumerable<TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string>> SampleAdjacencyGraphs();

    /// <summary>
    /// Creates adjacency graphs (version manageable with define for slow tests).
    /// </summary>
    IEnumerable<TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string>> SampleAdjacencyGraphs(uint rateWhenNotSlowTestsRun);

    /// <summary>
    /// Creates bidirectional graphs.
    /// </summary>
    IEnumerable<TestGraphInstance<BidirectionalGraph<string, Edge<string>>, string>> SampleBidirectionalGraphs();

    /// <summary>
    /// Creates undirected graphs.
    /// </summary>
    IEnumerable<TestGraphInstance<UndirectedGraph<string, Edge<string>>, string>> SampleUndirectedGraphs();

    /// <summary>
    /// Creates undirected graphs (version manageable with define for slow tests).
    /// </summary>
    IEnumerable<TestGraphInstance<UndirectedGraph<string, Edge<string>>, string>> SampleUndirectedGraphs(uint rateWhenNotSlowTestsRun);
}
