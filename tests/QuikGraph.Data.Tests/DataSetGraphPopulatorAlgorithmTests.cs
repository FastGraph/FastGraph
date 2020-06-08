using System;
using System.Data;
using NUnit.Framework;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;
using static QuikGraph.Data.Tests.GraphTestHelpers;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Data.Tests
{
    /// <summary>
    /// Tests for <see cref="DataSetGraphPopulatorAlgorithm"/>.
    ///</summary>
    [TestFixture]
    internal class DataSetGraphPopulatorAlgorithmTests
    {
        [Test]
        public void Construction()
        {
            var dataSet = new DataSet();
            var graph = new AdjacencyGraph<DataTable, DataRelationEdge>();
            var algorithm = new DataSetGraphPopulatorAlgorithm(graph, dataSet);
            AssertAlgorithmProperties(algorithm, graph, dataSet);

            #region Local function

            void AssertAlgorithmProperties(
                DataSetGraphPopulatorAlgorithm algo,
                IMutableVertexAndEdgeSet<DataTable, DataRelationEdge> g,
                DataSet set)
            {
                AssertAlgorithmState(algo, g);
                Assert.AreSame(set, algo.DataSet);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            var dataSet = new DataSet();
            var graph = new AdjacencyGraph<DataTable, DataRelationEdge>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new DataSetGraphPopulatorAlgorithm(graph, null));
            Assert.Throws<ArgumentNullException>(() => new DataSetGraphPopulatorAlgorithm(null, dataSet));
            Assert.Throws<ArgumentNullException>(() => new DataSetGraphPopulatorAlgorithm(null, null));
            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ObjectCreationAsStatement
        }

        [Test]
        public void Compute()
        {
            // Empty data set
            var dataSet = new DataSet();
            var graph = new AdjacencyGraph<DataTable, DataRelationEdge>();
            var algorithm = new DataSetGraphPopulatorAlgorithm(graph, dataSet);
            algorithm.Compute();

            AssertEmptyGraph(graph);

            // Only tables data set
            dataSet = new DataSet();

            var customers = new DataTable("Customers");
            var customerIdCol = new DataColumn("CustomerID", typeof(int)) { Unique = true };
            customers.Columns.Add(customerIdCol);
            dataSet.Tables.Add(customers);

            var orders = new DataTable("Orders");
            var orderIdCol = new DataColumn("OrderID", typeof(int)) { Unique = true };
            orders.Columns.Add(orderIdCol);
            dataSet.Tables.Add(orders);

            graph = new AdjacencyGraph<DataTable, DataRelationEdge>();
            algorithm = new DataSetGraphPopulatorAlgorithm(graph, dataSet);
            algorithm.Compute();

            AssertHasVertices(graph, new[]{ customers, orders });
            AssertNoEdge(graph);

            // Table with relations
            dataSet = new DataSet();

            var addresses = new DataTable("Addresses");
            var addressIdCol = new DataColumn("AddressID", typeof(int)) { Unique = true };
            addresses.Columns.Add(addressIdCol);
            dataSet.Tables.Add(addresses);

            customers = new DataTable("Customers");
            customerIdCol = new DataColumn("CustomerID", typeof(int)) { Unique = true };
            customers.Columns.Add(customerIdCol);
            dataSet.Tables.Add(customers);

            orders = new DataTable("Orders");
            orderIdCol = new DataColumn("OrderID", typeof(int)) { Unique = true };
            orders.Columns.Add(orderIdCol);
            dataSet.Tables.Add(orders);

            var customerOrders = new DataRelation("CustomersOrders", customerIdCol, orderIdCol);
            dataSet.Relations.Add(customerOrders);
            var customersAddresses = new DataRelation("CustomersAddresses", customerIdCol, addressIdCol);
            dataSet.Relations.Add(customersAddresses);
            var warehousesAddresses = new DataRelation("WarehousesAddresses", orderIdCol, addressIdCol);
            dataSet.Relations.Add(warehousesAddresses);

            graph = new AdjacencyGraph<DataTable, DataRelationEdge>();
            algorithm = new DataSetGraphPopulatorAlgorithm(graph, dataSet);
            algorithm.Compute();

            AssertHasVertices(graph, new[]{ addresses, customers, orders });
            AssertHasRelations(
                graph,
                new[]
                {
                    new DataRelationEdge(customerOrders),
                    new DataRelationEdge(customersAddresses),
                    new DataRelationEdge(warehousesAddresses)
                });
        }
    }
}