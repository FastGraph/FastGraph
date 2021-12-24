#nullable enable

using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace FastGraph.Tests;

public sealed class TestGraphVertexPreviews<TGraph, TVertex>
    where TGraph : IVertexSet<TVertex>
    where TVertex : notnull
{
    private static readonly ConcurrentDictionary<TestGraphSource, Lazy<ImmutableArray<TVertex>>> VerticesBySource = new();

    public static Lazy<ImmutableArray<TVertex>> GetOrAdd(TestGraphSource source, Func<TestGraphSource, TGraph> deserializer)
    {
        Lazy<ImmutableArray<TVertex>> CreateLazy(TestGraphSource source) =>
            new(deserializer(source).Vertices.ToImmutableArray());


        return VerticesBySource.GetOrAdd(source, CreateLazy);
    }
}

public static class TestGraphVertexPreviews
{
    public static ImmutableArray<TVertex> PreviewVertices<TGraph, TVertex>(TestGraphSource source,
        Func<TestGraphSource, TGraph> deserializer)
        where TGraph : IVertexSet<TVertex>
        where TVertex : notnull =>
        TestGraphVertexPreviews<TGraph, TVertex>.GetOrAdd(source, deserializer).Value;
}
