#nullable enable

namespace FastGraph.Tests.Algorithms
{
    /// <summary>
    /// Class containing information needed for building graphs when testing contracts.
    /// </summary>
    internal sealed class ContractScenario<TVertex>
        where TVertex : notnull
    {
        /// <summary>
        /// Edges in the graph. These should be converted to compatible edges in the constructor for the graph
        /// and both edges and vertices should be added.
        /// </summary>
        public IEnumerable<Edge<TVertex>> EdgesInGraph { get; set; } = new Edge<TVertex>[0];

        /// <summary>
        /// Vertices not connected to any other vertices.
        /// </summary>
        public IEnumerable<TVertex> SingleVerticesInGraph { get; set; } = new TVertex[0];

        /// <summary>
        /// The vertex that will be used as root vertex in the test.
        /// </summary>
        public TVertex? Root { get; set; } = default;

        /// <summary>
        /// Vertices expected to be accessible from the root, not including the root itself.
        /// </summary>
        public IEnumerable<TVertex> AccessibleVerticesFromRoot { get; set; } = new TVertex[0];

        /// <summary>
        /// Flag indicating whether the algorithm should be computed after it has been instantiated.
        /// </summary>
        public bool DoComputation { get; set; } = true;
    }
}
