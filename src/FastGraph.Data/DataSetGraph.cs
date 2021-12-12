#nullable enable

using System.Data;

namespace FastGraph.Data
{
    /// <summary>
    /// Represents a set of data as a graph.
    /// </summary>
    public class DataSetGraph : BidirectionalGraph<DataTable, DataRelationEdge>
    {
        /// <summary>
        /// Wrapped <see cref="T:System.Data.DataSet"/>.
        /// </summary>
        public DataSet DataSet { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetGraph"/> class.
        /// </summary>
        /// <param name="dataSet">Set of data.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="dataSet"/> is <see langword="null"/>.</exception>
        internal DataSetGraph(DataSet dataSet)
        {
            DataSet = dataSet ?? throw new ArgumentNullException(nameof(dataSet));
        }
    }
}
