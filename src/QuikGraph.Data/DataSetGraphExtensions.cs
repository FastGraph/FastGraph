using System.Data;
using System.Diagnostics.Contracts;
using QuikGraph.Graphviz;

namespace QuikGraph.Data
{
    public static class DataSetGraphExtensions
    {
        public static DataSetGraph ToGraph(this DataSet ds)
        {
            Contract.Requires(ds != null);

            var g = new DataSetGraph(ds);
            var populator = new DataSetGraphPopulatorAlgorithm(g, ds);
            populator.Compute();

            return g;
        }

        public static string ToGraphviz(this DataSetGraph visitedGraph)
        {
            Contract.Requires(visitedGraph != null);

            var algorithm = new DataSetGraphvizAlgorithm(visitedGraph);
            return algorithm.Generate();
        }
    }
}