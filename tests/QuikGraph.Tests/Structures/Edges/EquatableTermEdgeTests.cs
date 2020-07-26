using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="EquatableTermEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal class EquatableTermEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new EquatableTermEdge<int>(1, 2), 1, 2);
            CheckEdge(new EquatableTermEdge<int>(2, 1), 2, 1);
            CheckEdge(new EquatableTermEdge<int>(1, 1), 1, 1);

            CheckTermEdge(new EquatableTermEdge<int>(1, 2, 0, 1), 1, 2, 0, 1);
            CheckTermEdge(new EquatableTermEdge<int>(2, 1, 1, 0), 2, 1, 1, 0);
            CheckTermEdge(new EquatableTermEdge<int>(1, 1, 0, 0), 1, 1, 0, 0);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new EquatableTermEdge<TestVertex>(v1, v2), v1, v2);
            CheckEdge(new EquatableTermEdge<TestVertex>(v2, v1), v2, v1);
            CheckEdge(new EquatableTermEdge<TestVertex>(v1, v1), v1, v1);

            CheckTermEdge(new EquatableTermEdge<TestVertex>(v1, v2, 0, 1), v1, v2, 0, 1);
            CheckTermEdge(new EquatableTermEdge<TestVertex>(v2, v1, 1, 0), v2, v1, 1, 0);
            CheckTermEdge(new EquatableTermEdge<TestVertex>(v1, v1, 0, 0), v1, v1, 0, 0);
        }

        [Test]
        public void Construction_Throws()
        {
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(null, v1));
            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(v1, null));
            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(null, null));

            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(null, v1, 0, 1));
            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(v1, null, 0, 1));
            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(null, null, 0, 1));
            // ReSharper restore AssignNullToNotNullAttribute

            Assert.Throws<ArgumentException>(() => new EquatableTermEdge<TestVertex>(v1, v2, -1, 0));
            Assert.Throws<ArgumentException>(() => new EquatableTermEdge<TestVertex>(v1, v2, 0, -1));
            Assert.Throws<ArgumentException>(() => new EquatableTermEdge<TestVertex>(v1, v2, -1, -1));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = new EquatableTermEdge<int>(1, 2);
            var edge2 = new EquatableTermEdge<int>(1, 2);
            var edge3 = new EquatableTermEdge<int>(1, 2, 0, 0);
            var edge4 = new EquatableTermEdge<int>(1, 2, 0, 0);
            var edge5 = new EquatableTermEdge<int>(1, 2, 0, 1);
            var edge6 = new EquatableTermEdge<int>(1, 2, 0, 1);

            Assert.AreEqual(edge1, edge1);
            Assert.AreEqual(edge3, edge3);
            Assert.AreEqual(edge5, edge5);

            Assert.AreEqual(edge1, edge2);
            Assert.AreEqual(edge2, edge1);
            Assert.IsTrue(edge1.Equals((object)edge2));
            Assert.IsTrue(edge1.Equals(edge2));
            Assert.IsTrue(edge2.Equals(edge1));

            Assert.AreEqual(edge1, edge3);
            Assert.AreEqual(edge3, edge1);
            Assert.IsTrue(edge1.Equals((object)edge3));
            Assert.IsTrue(edge1.Equals(edge3));
            Assert.IsTrue(edge3.Equals(edge1));

            Assert.AreNotEqual(edge1, edge5);
            Assert.AreNotEqual(edge5, edge1);
            Assert.IsFalse(edge1.Equals((object)edge5));
            Assert.IsFalse(edge1.Equals(edge5));
            Assert.IsFalse(edge5.Equals(edge1));

            Assert.AreEqual(edge3, edge4);
            Assert.AreEqual(edge4, edge3);
            Assert.IsTrue(edge3.Equals((object)edge4));
            Assert.IsTrue(edge3.Equals(edge4));
            Assert.IsTrue(edge4.Equals(edge3));

            Assert.AreEqual(edge5, edge6);
            Assert.AreEqual(edge6, edge5);
            Assert.IsTrue(edge5.Equals((object)edge6));
            Assert.IsTrue(edge5.Equals(edge6));
            Assert.IsTrue(edge6.Equals(edge5));

            Assert.AreNotEqual(edge1, null);
            Assert.IsFalse(edge1.Equals(null));
        }

        [Test]
        public void Hashcode()
        {
            var edge1 = new EquatableTermEdge<int>(1, 2);
            var edge2 = new EquatableTermEdge<int>(1, 2);
            var edge3 = new EquatableTermEdge<int>(2, 1);

            Assert.AreEqual(edge1.GetHashCode(), edge2.GetHashCode());
            Assert.AreNotEqual(edge1.GetHashCode(), edge3.GetHashCode());
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new EquatableTermEdge<int>(1, 2);
            var edge2 = new EquatableTermEdge<int>(1, 2, 1, 5);
            var edge3 = new EquatableTermEdge<int>(2, 1);

            Assert.AreEqual("1 (0) -> 2 (0)", edge1.ToString());
            Assert.AreEqual("1 (1) -> 2 (5)", edge2.ToString());
            Assert.AreEqual("2 (0) -> 1 (0)", edge3.ToString());
        }
    }
}