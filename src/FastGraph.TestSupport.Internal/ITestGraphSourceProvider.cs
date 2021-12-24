#nullable enable

using JetBrains.Annotations;
using NUnit.Framework;

namespace FastGraph.Tests;

public interface ITestGraphSourceProvider
{
    /// <summary>
    /// Gets graph ML content from source files.
    /// </summary>
    IEnumerable<NamedTestGraphSource> AllGeneralPurpose { [Pure] get; }

    NamedTestGraphSource DCT8_with_missing_attribute { [Pure] get; }
    NamedTestGraphSource DCT8_with_missing_source_id { [Pure] get; }
    NamedTestGraphSource DCT8_with_missing_target_id { [Pure] get; }
    NamedTestGraphSource emptyGraph { [Pure] get; }
    NamedTestGraphSource DCT8 { [Pure] get; }
    NamedTestGraphSource DCT8_invalid_tag { [Pure] get; }
    NamedTestGraphSource DCT8_missing_graph_tag { [Pure] get; }
    NamedTestGraphSource DCT8_missing_graphml_tag { [Pure] get; }
    NamedTestGraphSource repro12359 { [Pure] get; }
    NamedTestGraphSource testGraph { [Pure] get; }
    NamedTestGraphSource G_42_34 { [Pure] get; }
    NamedTestGraphSource G_10_0 { [Pure] get; }
}

public sealed class TestGraphSourceProvider : ITestGraphSourceProvider
{
    public static readonly ITestGraphSourceProvider Instance = new TestGraphSourceProvider();

    private TestGraphSourceProvider() { }

    /// <summary>
    /// Gets graph ML content from source files.
    /// </summary>
    public IEnumerable<NamedTestGraphSource> AllGeneralPurpose { get; } =
        GetGraphMLDirectoryFiles("g.*.graphml")
            .Select(NamedTestGraphSource.FromFile)
            .Memoize();

    public NamedTestGraphSource DCT8 { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("DCT8.graphml"));

    public NamedTestGraphSource DCT8_invalid_tag { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("DCT8_invalid_tag.graphml"));

    public NamedTestGraphSource DCT8_missing_graph_tag { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("DCT8_missing_graph_tag.graphml"));

    public NamedTestGraphSource DCT8_missing_graphml_tag { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("DCT8_missing_graphml_tag.graphml"));

    public NamedTestGraphSource DCT8_with_missing_attribute { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("DCT8_with_missing_attribute.graphml"));

    public NamedTestGraphSource DCT8_with_missing_source_id { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("DCT8_with_missing_source_id.graphml"));

    public NamedTestGraphSource DCT8_with_missing_target_id { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("DCT8_with_missing_target_id.graphml"));

    public NamedTestGraphSource emptyGraph { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("emptyGraph.xml"));

    public NamedTestGraphSource repro12359 { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("repro12359.graphml"));

    public NamedTestGraphSource testGraph { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("testGraph.xml"));

    public NamedTestGraphSource G_42_34 { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("g.42.34.graphml"));

    public NamedTestGraphSource G_10_0 { [Pure] get; } =
        NamedTestGraphSource.FromFile(GetGraphMLDirectorySingleFile("g.10.0.graphml"));

    [Pure]
    private static DirectoryInfo GetGraphMLDirectory()
    {
        string graphMLDirectoryPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "GraphML");

        var directory = new DirectoryInfo(graphMLDirectoryPath);
        if (!directory.Exists)
        {
            throw new AssertionException("GraphML folder must exist.");
        }

        return directory;
    }

    [Pure]
    private static IReadOnlyList<FileInfo> GetGraphMLDirectoryFiles(string searchPattern) => GetGraphMLDirectory().GetFiles(searchPattern);

    [Pure]
    private static FileInfo GetGraphMLDirectorySingleFile(string searchPattern) => GetGraphMLDirectoryFiles(searchPattern).Single();
}
