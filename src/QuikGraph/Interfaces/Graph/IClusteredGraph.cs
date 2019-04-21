using System.Collections;

namespace QuikGraph
{
    /// <summary>
    /// Represents a graph cluster.
    /// </summary>
    public interface IClusteredGraph
    {
        /// <summary>
        /// Graph clusters.
        /// </summary>
        IEnumerable Clusters { get; }

        /// <summary>
        /// Number of clusters.
        /// </summary>
        int ClustersCount { get; }

        /// <summary>
        /// Gets or sets the collapse state of this cluster.
        /// </summary>
        bool Collapsed { get; set; }

        /// <summary>
        /// Adds a new cluster.
        /// </summary>
        /// <returns></returns>
        IClusteredGraph AddCluster();

        /// <summary>
        /// Removes the given graph from this cluster.
        /// </summary>
        /// <param name="graph">The graph.</param>
        void RemoveCluster(IClusteredGraph graph);
    }
}
