#nullable enable

namespace FastGraph.Tests
{
    /// <summary>
    /// Vertex type used for tests (comparable).
    /// </summary>
    internal sealed class ComparableTestVertex : IComparable
    {
        public ComparableTestVertex()
            : this("Default")
        {
        }

        public ComparableTestVertex(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public int CompareTo(object? obj)
        {
            if (obj is null)
                return 1;
            if (obj is ComparableTestVertex comparable)
                return string.Compare(Name, comparable.Name, StringComparison.Ordinal);
            throw new ArgumentException("Wrong type for compared object", nameof(obj));
        }
    }
}
