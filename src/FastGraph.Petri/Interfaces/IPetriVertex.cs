#nullable enable

namespace FastGraph.Petri
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
        /// A <see cref="T:System.String"/> representing the name of the node.
        /// </value>
        string Name { get; }
    }
}
