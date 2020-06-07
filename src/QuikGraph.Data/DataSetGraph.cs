using System.Data;
using System.Diagnostics.Contracts;

namespace QuikGraph.Data
{
    public class DataSetGraph : BidirectionalGraph<DataTable, DataRelationEdge>
    {
        readonly DataSet dataSet;
        public DataSet DataSet
        {
            get { return this.dataSet; }
        }

        internal DataSetGraph(DataSet dataSet)
        {
            Contract.Requires(dataSet != null);

            this.dataSet = dataSet;
        }
    }
}