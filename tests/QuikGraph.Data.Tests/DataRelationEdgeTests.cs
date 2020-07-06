using System;
using System.Data;
using NUnit.Framework;

namespace QuikGraph.Data.Tests
{
    /// <summary>
    /// Tests for <see cref="DataRelationEdge"/>.
    ///</summary>
    [TestFixture]
    internal class DataRelationEdgeTests
    {
        [Test]
        public void Construction()
        {
            DataRelation relation = SetupTestRelation();

            CheckRelation(new DataRelationEdge(relation), relation);

            #region Local functions

            DataRelation SetupTestRelation()
            {
                var dataSet = new DataSet();

                var customers = new DataTable("Customers");
                var customerIdCol = new DataColumn("CustomerID", typeof(int)) { Unique = true };
                customers.Columns.Add(customerIdCol);
                dataSet.Tables.Add(customers);

                var orders = new DataTable("Orders");
                var orderIdCol = new DataColumn("OrderID", typeof(int)) { Unique = true };
                orders.Columns.Add(orderIdCol);
                dataSet.Tables.Add(orders);

                return new DataRelation("CustomersOrders", customerIdCol, orderIdCol);
            }

            void CheckRelation(DataRelationEdge e, DataRelation r)
            {
                Assert.IsNotNull(e.Source);
                Assert.AreSame(r.ParentTable, e.Source);

                Assert.IsNotNull(e.Target);
                Assert.AreSame(r.ChildTable, e.Target);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new DataRelationEdge(null));
        }

        [Test]
        public void Equals()
        {
            CreateDataColumns(
                out DataColumn customerIdCol,
                out DataColumn orderIdCol);

            var relation1 = new DataRelation("CustomersOrders", customerIdCol, orderIdCol);
            var relation2 = new DataRelation("CustomersOrders", customerIdCol, orderIdCol);
            var relation3 = new DataRelation("CustomersOrders", orderIdCol, customerIdCol);

            Assert.AreEqual(relation1, relation1);

            Assert.AreNotEqual(relation1, relation2);
            Assert.AreNotEqual(relation2, relation1);
            Assert.IsFalse(relation1.Equals(relation2));
            Assert.IsFalse(relation2.Equals(relation1));

            Assert.AreNotEqual(relation1, relation3);
            Assert.AreNotEqual(relation3, relation1);
            Assert.IsFalse(relation1.Equals(relation3));
            Assert.IsFalse(relation3.Equals(relation1));

            Assert.AreNotEqual(relation1, null);
            Assert.IsFalse(relation1.Equals(null));

            #region Local function

            void CreateDataColumns(out DataColumn custIdCol, out DataColumn ordIdCol)
            {
                var dataSet = new DataSet();

                var customers = new DataTable("Customers");
                custIdCol = new DataColumn("CustomerID", typeof(int)) { Unique = true };
                customers.Columns.Add(custIdCol);
                dataSet.Tables.Add(customers);

                var orders = new DataTable("Orders");
                ordIdCol = new DataColumn("OrderID", typeof(int)) { Unique = true };
                orders.Columns.Add(ordIdCol);
                dataSet.Tables.Add(orders);
            }

            #endregion
        }
    }
}