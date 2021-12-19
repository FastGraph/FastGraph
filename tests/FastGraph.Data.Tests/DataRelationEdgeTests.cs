#nullable enable

using System.Data;
using NUnit.Framework;

namespace FastGraph.Data.Tests
{
    /// <summary>
    /// Tests for <see cref="DataRelationEdge"/>.
    ///</summary>
    [TestFixture]
    internal sealed class DataRelationEdgeTests
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
                e.Source.Should().NotBeNull();
                e.Source.Should().BeSameAs(r.ParentTable);

                e.Target.Should().NotBeNull();
                e.Target.Should().BeSameAs(r.ChildTable);
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Invoking(() => new DataRelationEdge(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
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

            relation1.Should().Be(relation1);

            relation2.Should().NotBe(relation1);
            relation1.Should().NotBe(relation2);
            relation1.Equals(relation2).Should().BeFalse();
            relation2.Equals(relation1).Should().BeFalse();

            relation3.Should().NotBe(relation1);
            relation1.Should().NotBe(relation3);
            relation1.Equals(relation3).Should().BeFalse();
            relation3.Equals(relation1).Should().BeFalse();

            relation1.Should().NotBe(default);
            relation1.Equals(default).Should().BeFalse();

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
