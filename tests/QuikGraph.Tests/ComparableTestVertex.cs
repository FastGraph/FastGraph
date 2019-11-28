using System;
using JetBrains.Annotations;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Vertex type used for tests (comparable).
    /// </summary>
    internal class ComparableTestVertex : IComparable
    {
        public ComparableTestVertex()
            : this("Default")
        {
        }

        public ComparableTestVertex([NotNull] string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; }

        public int CompareTo(object obj)
        {
            if (obj is null)
                return 1;
            if (obj is ComparableTestVertex comparable)
                return string.Compare(Name, comparable.Name, StringComparison.Ordinal);
            throw new ArgumentException("Wrong type for compared object", nameof(obj));
        }
    }
}