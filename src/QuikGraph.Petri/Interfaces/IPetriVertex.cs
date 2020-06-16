using JetBrains.Annotations;

namespace QuikGraph.Petri
{
    /// <summary>
    /// A vertex (node) of a Petri Graph.
    /// </summary>
    public interface IPetriVertex
    {
        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the name of the node.
        /// </value>
        [NotNull]
        string Name { get; }
    }
}