using System;
using JetBrains.Annotations;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Vertex type used for tests (equatable & cloneable).
    /// </summary>
    internal class EquatableCloneableTestVertex
        : CloneableTestVertex
        , IEquatable<EquatableCloneableTestVertex>
    {
        public EquatableCloneableTestVertex()
            : this("Default")
        {
        }

        public EquatableCloneableTestVertex([NotNull] string name)
            : base(name)
        {
        }

        public bool Equals(EquatableCloneableTestVertex other)
        {
            if (other is null)
                return false;
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || Equals(obj as EquatableCloneableTestVertex);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <inheritdoc />
        public override object Clone()
        {
            return new EquatableCloneableTestVertex(Name);
        }
    }
}