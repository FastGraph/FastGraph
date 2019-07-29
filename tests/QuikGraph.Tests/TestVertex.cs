using JetBrains.Annotations;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Vertex type used for tests.
    /// </summary>
    internal class TestVertex
    {
        public TestVertex([NotNull] string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; }
    }
}