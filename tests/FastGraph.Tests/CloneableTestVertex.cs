#nullable enable

namespace FastGraph.Tests
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

        public CloneableTestVertex(string name)
        {
            Name = name;
        }

        public string Name { get; }

        /// <inheritdoc />
        public virtual object Clone()
        {
            return new CloneableTestVertex(Name);
        }
    }
}
