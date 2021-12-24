#nullable enable

using JetBrains.Annotations;

namespace FastGraph.Tests;

/// <summary>
/// Factory to select a sampling of source graphs and then load them as a specified graph type for use in test cases.
/// When `FULL_SLOW_TESTS_RUN` is defined, all graphs are returned instead of a sampling.
/// </summary>
public class TestGraphFactory
{
    private static readonly ITestGraphFactory Instance = new TestGraphFactoryImplementation();

    /// <summary>
    /// Creates adjacency graphs.
    /// </summary>
    [Pure]
    public static IEnumerable<TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string>> SampleAdjacencyGraphs() =>
        Instance.SampleAdjacencyGraphs();

    /// <summary>
    /// Creates adjacency graphs (version manageable with define for slow tests).
    /// </summary>
    [Pure]
    public static IEnumerable<TestGraphInstance<AdjacencyGraph<string, Edge<string>>, string>> SampleAdjacencyGraphs(uint rateWhenNotSlowTestsRun) =>
        Instance.SampleAdjacencyGraphs(rateWhenNotSlowTestsRun);

    /// <summary>
    /// Creates bidirectional graphs.
    /// </summary>
    [Pure]
    public static IEnumerable<TestGraphInstance<BidirectionalGraph<string, Edge<string>>, string>> SampleBidirectionalGraphs() =>
        Instance.SampleBidirectionalGraphs();

    /// <summary>
    /// Creates undirected graphs.
    /// </summary>
    [Pure]
    public static IEnumerable<TestGraphInstance<UndirectedGraph<string, Edge<string>>, string>> SampleUndirectedGraphs() =>
        Instance.SampleUndirectedGraphs();

    /// <summary>
    /// Creates undirected graphs (version manageable with define for slow tests).
    /// </summary>
    [Pure]
    public static IEnumerable<TestGraphInstance<UndirectedGraph<string, Edge<string>>, string>> SampleUndirectedGraphs(uint rateWhenNotSlowTestsRun) =>
        Instance.SampleUndirectedGraphs(rateWhenNotSlowTestsRun);
}
