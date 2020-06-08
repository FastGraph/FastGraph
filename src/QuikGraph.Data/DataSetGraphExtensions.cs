using System.Data;
using JetBrains.Annotations;

namespace QuikGraph.Data
{
    /// <summary>
    /// Extensions related to <see cref="DataSet"/> and <see cref="DataSetGraph"/>.
    /// </summary>
    public static class DataSetGraphExtensions
    {
        /// <summary>
        /// Converts this <paramref name="dataSet"/> into a graph representation of it.
        /// </summary>
        /// <param name="dataSet"><see cref="DataSet"/> to convert to graph.</param>
        /// <returns>Graph representing the <see cref="DataSet"/>.</returns>
        [Pure]
        [NotNull]
        public static DataSetGraph ToGraph([NotNull] this DataSet dataSet)
        {
            var graph = new DataSetGraph(dataSet);
            var populator = new DataSetGraphPopulatorAlgorithm(graph, dataSet);
            populator.Compute();

            return graph;
        }

        /// <summary>
        /// Renders a graph to the Graphviz DOT format.
        /// </summary>
        /// <param name="graph">Graph to convert.</param>
        /// <returns>Graph serialized in DOT format.</returns>
        [Pure]
        [NotNull]
        public static string ToGraphviz([NotNull] this DataSetGraph graph)
        {
            var algorithm = new DataSetGraphvizAlgorithm(graph);
            return algorithm.Generate();
        }
    }
}