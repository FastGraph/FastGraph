using System;
using JetBrains.Annotations;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Vertex type used for tests (cloneable).
    /// </summary>
    internal class CloneableTestVertex : ICloneable
    {
        public CloneableTestVertex()
            : this("Default")
        {
        }

        public CloneableTestVertex([NotNull] string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; }

        /// <inheritdoc />
        public virtual object Clone()
        {
            return new CloneableTestVertex(Name);
        }
    }
}