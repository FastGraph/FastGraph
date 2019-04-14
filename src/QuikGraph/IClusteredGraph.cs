using System.Collections;

namespace QuikGraph
{
    public interface IClusteredGraph
    {
        IEnumerable Clusters { get; }

        int ClustersCount { get; }

        bool Colapsed { get; set; }

        IClusteredGraph AddCluster();

        void RemoveCluster(IClusteredGraph g);
    }
}
