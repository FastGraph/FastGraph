using System;
using System.Data;
using JetBrains.Annotations;

namespace QuikGraph.Data
{
    /// <summary>
    /// Represents a set of data as a graph.
    /// </summary>
    public class DataSetGraph : BidirectionalGraph<DataTable, DataRelationEdge>
    {
        /// <summary>
        /// Wrapped <see cref="System.Data.DataSet"/>.
        /// </summary>
        public DataSet DataSet { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetGraph"/> class.
        /// </summary>
        /// <param name="dataSet">Set of data.</param>
        internal DataSetGraph([NotNull] DataSet dataSet)
        {
            DataSet = dataSet ?? throw new ArgumentNullException(nameof(dataSet));
        }
    }
}