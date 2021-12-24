#nullable enable

using System.Collections.Immutable;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using FastGraph.Serialization;

namespace FastGraph.Tests;

public record TestGraphSource(string SourceContent, BigInteger SourceContentSha256);

public record NamedTestGraphSource(string Name, string SourceContent, BigInteger SourceContentSha256, string? SourcePath)
    : TestGraphSource(SourceContent, SourceContentSha256)
{
    public static NamedTestGraphSource FromFile(FileInfo file)
    {
        using var streamReader = file.OpenText();
        string? content = streamReader.ReadToEnd();

        byte[]? contentAsUtf8Bytes = System.Text.Encoding.UTF8.GetBytes(content);
        using var hasher = SHA256.Create();
        byte[]? contentSha256Bytes = hasher.ComputeHash(contentAsUtf8Bytes);
        var contentSha256AsInteger = new BigInteger(contentSha256Bytes);

        return new NamedTestGraphSource(System.IO.Path.GetFileNameWithoutExtension(file.Name), content, contentSha256AsInteger, file.FullName);
    }

    protected override bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"Name = '{Name}', SourceContent = {{ Length: {SourceContent.Length} }}, SourceContentSha256 = {SourceContentSha256} ");
        return true;
    }
}

public record DeserializableNamedTestGraphSource<TGraph, TVertex>(string Name, Func<TestGraphSource, TGraph> Deserializer, string SourceContent, BigInteger SourceContentSha256)
    : NamedTestGraphSource(Name, SourceContent, SourceContentSha256, default)
    where TGraph : IVertexSet<TVertex>, new()
    where TVertex : notnull
{
    public DeserializableNamedTestGraphSource(NamedTestGraphSource source, Func<TestGraphSource, TGraph> deserializer)
        : this(source.Name, deserializer, source.SourceContent, source.SourceContentSha256)
    {}

    public static DeserializableNamedTestGraphSource<TGraph, TVertex> CreateEmpty() =>
        new(String.Empty, _ => new TGraph(), String.Empty, BigInteger.Zero);

    public ImmutableArray<TVertex> AllVerticesPreviewed() =>
        TestGraphVertexPreviews.PreviewVertices<TGraph, TVertex>(this, Deserializer);

    public TGraph DeserializeNewInstance() =>
        Deserializer(this);

    public TestGraphInstance<TGraph, TVertex> CreateInstanceHandle() =>
        new(this);

    protected override bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"Name = '{Name}', SourceContent = {{ Length: {SourceContent.Length} }}, SourceContentSha256 = {SourceContentSha256} ");
        return true;
    }
}

public record struct TestGraphInstance<TGraph, TVertex>(DeserializableNamedTestGraphSource<TGraph, TVertex> Source)
    where TGraph : IVertexSet<TVertex>, new()
    where TVertex : notnull
{
    private readonly Lazy<TGraph> _lazyInstance = new(Source.DeserializeNewInstance);
    private readonly Lazy<int> _lazyVertexCount = new(Source.AllVerticesPreviewed().Count);

    public string Name => Source.Name;

    public TGraph Instance => _lazyInstance.Value;

    public int VertexCount => _lazyVertexCount.Value;

    public override string ToString()
    {
        // Skips members `Instance` and `VertexCount` because we don't want them evaluated on account of being `ToString`-ed.
        return $"{GetType().Name} {{ Source = {Source} }}";
    }

    /// <remarks>No more than enough information to uniquely identify the test case, and with included members sorted for left-to-right readability.</remarks>
    public string DescribeForTestCase()
    {
        return $"{GetType().Name} {{ Name = '{Name}', SourceContentSha256 = {Source.SourceContentSha256}  }}";
    }

    public IEnumerable<TestGraphInstanceWithSelectedVertex<TGraph, TVertex>> Select()
    {
        var @this = this;
        return Enumerable.Range(0, VertexCount).Select(i => @this.Select(i));
    }

    public IEnumerable<TResult> Select<TResult>(Func<TestGraphInstanceWithSelectedVertex<TGraph, TVertex>, TResult> projection)
    {
        var @this = this;
        return Enumerable.Range(0, VertexCount).Select(i => projection(@this.Select(i)));
    }

    private TestGraphInstanceWithSelectedVertex<TGraph, TVertex> Select(Index vertexIndex) =>
        new(Source, vertexIndex);
}

public record struct TestGraphInstanceWithSelectedVertex<TGraph, TVertex>(DeserializableNamedTestGraphSource<TGraph, TVertex> Source, Index VertexIndex)
    where TGraph : IVertexSet<TVertex>, new()
    where TVertex : notnull
{
    private readonly Lazy<TGraph> _lazyInstance = new(Source.DeserializeNewInstance);
    private readonly Lazy<int> _lazyVertexCount = new(Source.AllVerticesPreviewed().Count);
    private readonly Lazy<TVertex> _lazySelectedVertex = new(Source.AllVerticesPreviewed()[VertexIndex]);

    public string Name => Source.Name;

    public TGraph Instance => _lazyInstance.Value;

    public int VertexCount => _lazyVertexCount.Value;

    public TVertex SelectedVertex => _lazySelectedVertex.Value;

    public override string ToString()
    {
        // Skips members `Instance` and `VertexCount` because we don't want them evaluated on account of being `ToString`-ed.
        return $"{GetType().Name} {{ Source = {Source}, VertexIndex = {VertexIndex} }}";
    }

    /// <remarks>No more than enough information to uniquely identify the test case, and with included members sorted for left-to-right readability.</remarks>
    public string DescribeForTestCase()
    {
        return $"{GetType().Name} {{ Name = '{Name}', VertexIndex = {VertexIndex}, SourceContentSha256 = {Source.SourceContentSha256} }}";
    }

    public IEnumerable<TestGraphInstanceWithSelectedVertexPair<TGraph, TVertex>> Select()
    {
        var @this = this;
        return Enumerable.Range(0, VertexCount).Select(i => @this.Select(i));
    }

    public IEnumerable<TResult> Select<TResult>(Func<TestGraphInstanceWithSelectedVertexPair<TGraph, TVertex>, TResult> projection)
    {
        var @this = this;
        return Enumerable.Range(0, VertexCount).Select(i => projection(@this.Select(i)));
    }

    private TestGraphInstanceWithSelectedVertexPair<TGraph, TVertex> Select(Index additionalVertexIndex) =>
        new(Source, VertexIndex, additionalVertexIndex);
}

public record struct TestGraphInstanceWithSelectedVertexPair<TGraph, TVertex>(DeserializableNamedTestGraphSource<TGraph, TVertex> Source, Index VertexIndex0, Index VertexIndex1)
    where TGraph : IVertexSet<TVertex>, new()
    where TVertex : notnull
{
    private readonly Lazy<TGraph> _lazyInstance = new(Source.DeserializeNewInstance);
    private readonly Lazy<int> _lazyVertexCount = new(Source.AllVerticesPreviewed().Count);
    private readonly Lazy<TVertex> _lazySelectedVertex0 = new(Source.AllVerticesPreviewed()[VertexIndex0]);
    private readonly Lazy<TVertex> _lazySelectedVertex1 = new(Source.AllVerticesPreviewed()[VertexIndex1]);

    public string Name => Source.Name;

    public TGraph Instance => _lazyInstance.Value;

    public int VertexCount => _lazyVertexCount.Value;

    public TVertex SelectedVertex0 => _lazySelectedVertex0.Value;

    public TVertex SelectedVertex1 => _lazySelectedVertex1.Value;

    public override string ToString()
    {
        // Skips members `Instance` and `VertexCount` because we don't want them evaluated on account of being `ToString`-ed.
        return $"{GetType().Name} {{ Source = {Source}, VertexIndex0 = {VertexIndex0}, VertexIndex1 = {VertexIndex1} }}";
    }

    /// <remarks>No more than enough information to uniquely identify the test case, and with included members sorted for left-to-right readability.</remarks>
    public string DescribeForTestCase()
    {
        return $"{GetType().Name} {{ Name = '{Name}', VertexIndex0 = {VertexIndex0}, VertexIndex1 = {VertexIndex1}, SourceContentSha256 = {Source.SourceContentSha256} }}";
    }
}

public static class TestGraphSourceExtensions
{
    public static AdjacencyGraph<string, Edge<string>> DeserializeAsAdjacencyGraph(this TestGraphSource source)
    {
        var graph = new AdjacencyGraph<string, Edge<string>>();
        using var reader = new StringReader(source.SourceContent);
        graph.DeserializeFromGraphML(
            reader,
            id => id,
            (source, target, _) => new Edge<string>(source, target));
        return graph;
    }

    public static BidirectionalGraph<string, Edge<string>> DeserializeAsBidirectionalGraph(this TestGraphSource source)
    {
        var graph = new BidirectionalGraph<string, Edge<string>>();
        using var reader = new StringReader(source.SourceContent);
        graph.DeserializeFromGraphML(
            reader,
            id => id,
            (source, target, _) => new Edge<string>(source, target));
        return graph;
    }

    public static UndirectedGraph<string, Edge<string>> DeserializeAsUndirectedGraph(this TestGraphSource source)
    {
        var adjacencyGraphBasis = source.DeserializeAsAdjacencyGraph();

        var graph = new UndirectedGraph<string, Edge<string>>();
        graph.AddVerticesAndEdgeRange(adjacencyGraphBasis.Edges);
        return graph;
    }

    public static DeserializableNamedTestGraphSource<AdjacencyGraph<string, Edge<string>>, string> DeferDeserializeAsAdjacencyGraph(this NamedTestGraphSource source) =>
        new(source, DeserializeAsAdjacencyGraph);

    public static DeserializableNamedTestGraphSource<BidirectionalGraph<string, Edge<string>>, string> DeferDeserializeAsBidirectionalGraph(this NamedTestGraphSource source) =>
        new(source, DeserializeAsBidirectionalGraph);

    public static DeserializableNamedTestGraphSource<UndirectedGraph<string, Edge<string>>, string> DeferDeserializeAsUndirectedGraph(this NamedTestGraphSource source) =>
        new(source, DeserializeAsUndirectedGraph);
}
