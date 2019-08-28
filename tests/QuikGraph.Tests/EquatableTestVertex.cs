using System;
using JetBrains.Annotations;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Vertex type used for tests (equatable).
    /// </summary>
    internal sealed class EquatableTestVertex : TestVertex, IEquatable<EquatableTestVertex>
    {
        private static int _counter;

        public EquatableTestVertex()
            : this($"EquatableTestVertex{_counter++}")
        {
        }

        public EquatableTestVertex([NotNull] string name)
            : base(name)
        {
        }

        public bool Equals(EquatableTestVertex other)
        {
            if (other is null)
                return false;
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || Equals(obj as EquatableTestVertex);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}