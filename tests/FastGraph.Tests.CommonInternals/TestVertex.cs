#nullable enable

namespace FastGraph.Tests
{
    /// <summary>
    /// Vertex type used for tests.
    /// </summary>
    internal class TestVertex
    {
        private static int _counter;

        public TestVertex()
            : this($"TestVertex{_counter++}")
        {
        }

        public TestVertex(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
