using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;
using static QuikGraph.Tests.AssertHelpers;

namespace QuikGraph.Tests.Collections
{
    /// <summary>
    /// Tests for <see cref="ForestDisjointSet{T}"/>.
    /// </summary>
    [TestFixture]
    internal class ForestDisjointSetTests
    {
        #region Test helpers

        private static void AssertSetSizes<T>(
            [NotNull] ForestDisjointSet<T> set,
            int expectedSetCount,
            int expectedCount)
        {
            Assert.AreEqual(expectedSetCount, set.SetCount);
            Assert.AreEqual(expectedCount, set.ElementCount);
        }

        #endregion

        [Test]
        public void Constructor()
        {
            AssertSetProperties(new ForestDisjointSet<int>());
            AssertSetProperties(new ForestDisjointSet<int>(12));

            #region Local function

            void AssertSetProperties<T>(ForestDisjointSet<T> set)
            {
                AssertSetSizes(set, 0, 0);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new ForestDisjointSet<int>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ForestDisjointSet<int>(int.MaxValue));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void MakeSet()
        {
            MakeSetTest(1, 2);
            MakeSetTest(
                new TestVertex("1"),
                new TestVertex("2"));
            MakeSetTest(
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"));

            #region Local function

            void MakeSetTest<TValue>(TValue value1, TValue value2)
            {
                var set = new ForestDisjointSet<TValue>();
                AssertSetSizes(set, 0, 0);

                set.MakeSet(value1);
                AssertSetSizes(set, 1, 1);

                set.MakeSet(value2);
                AssertSetSizes(set, 2, 2);
            }

            #endregion
        }

        [Test]
        public void MakeSet_Throws()
        {
            var testObject = new TestVertex();
            var set = new ForestDisjointSet<TestVertex>();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => set.MakeSet(null));

            set.MakeSet(testObject);
            // Double insert
            Assert.Throws<ArgumentException>(() => set.MakeSet(testObject));
        }

        [Test]
        public void FindSet()
        {
            FindSetTest(1, 2);
            FindSetTest(
                new TestVertex("1"),
                new TestVertex("2"));
            FindSetTest(
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"));

            #region Local function

            void FindSetTest<TValue>(TValue value1, TValue value2)
            {
                var set = new ForestDisjointSet<TValue>();
                AssertSetSizes(set, 0, 0);

                set.MakeSet(value1);
                AssertSetSizes(set, 1, 1);
                AssertEqual(value1, set.FindSet(value1));

                set.MakeSet(value2);
                AssertSetSizes(set, 2, 2);
                AssertEqual(value1, set.FindSet(value1));
                AssertEqual(value2, set.FindSet(value2));

                set.Union(value1, value2);
                AssertSetSizes(set, 1, 2);
                AssertEqual(value1, set.FindSet(value1));
                AssertEqual(value1, set.FindSet(value2));
            }

            #endregion
        }

        [Test]
        public void FindSet_Throws()
        {
            var vertex = new TestVertex();
            var set = new ForestDisjointSet<TestVertex>();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => set.FindSet(null));
            Assert.Throws<KeyNotFoundException>(() => set.FindSet(vertex));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void AreInSameSet()
        {
            AreInSameSetTest(1, 2);
            AreInSameSetTest(
                new TestVertex("1"),
                new TestVertex("2"));
            AreInSameSetTest(
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"));

            #region Local function

            void AreInSameSetTest<TValue>(TValue value1, TValue value2)
            {
                var set = new ForestDisjointSet<TValue>();
                AssertSetSizes(set, 0, 0);

                set.MakeSet(value1);
                Assert.IsTrue(set.AreInSameSet(value1, value1));

                set.MakeSet(value2);
                Assert.IsTrue(set.AreInSameSet(value1, value1));
                Assert.IsFalse(set.AreInSameSet(value1, value2));
                Assert.IsFalse(set.AreInSameSet(value2, value1));
                Assert.IsTrue(set.AreInSameSet(value2, value2));

                set.Union(value1, value2);
                Assert.IsTrue(set.AreInSameSet(value1, value1));
                Assert.IsTrue(set.AreInSameSet(value1, value2));
                Assert.IsTrue(set.AreInSameSet(value2, value1));
                Assert.IsTrue(set.AreInSameSet(value2, value2));
            }

            #endregion
        }

        [Test]
        public void AreInSameSet_Throws()
        {
            var vertex = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var set = new ForestDisjointSet<TestVertex>();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => set.AreInSameSet(vertex, null));
            Assert.Throws<ArgumentNullException>(() => set.AreInSameSet(null, vertex));
            Assert.Throws<ArgumentNullException>(() => set.AreInSameSet(null, null));
            // ReSharper restore AssignNullToNotNullAttribute

            Assert.Throws<KeyNotFoundException>(() => set.AreInSameSet(vertex, vertex));
            Assert.Throws<KeyNotFoundException>(() => set.AreInSameSet(vertex, vertex2));
            Assert.Throws<KeyNotFoundException>(() => set.AreInSameSet(vertex2, vertex));
            Assert.Throws<KeyNotFoundException>(() => set.AreInSameSet(vertex2, vertex2));

            set.MakeSet(vertex);
            Assert.DoesNotThrow(() => set.AreInSameSet(vertex, vertex));
            Assert.Throws<KeyNotFoundException>(() => set.AreInSameSet(vertex, vertex2));
            Assert.Throws<KeyNotFoundException>(() => set.AreInSameSet(vertex2, vertex));
            Assert.Throws<KeyNotFoundException>(() => set.AreInSameSet(vertex2, vertex2));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void Union()
        {
            UnionTest(1, 2, 3, 4, 5);
            UnionTest(
                new TestVertex("1"),
                new TestVertex("2"),
                new TestVertex("3"),
                new TestVertex("4"),
                new TestVertex("5"));
            UnionTest(
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"),
                new EquatableTestVertex("3"),
                new EquatableTestVertex("4"),
                new EquatableTestVertex("5"));

            #region Local function

            void UnionTest<TValue>(
                TValue value1,
                TValue value2,
                TValue value3,
                TValue value4,
                TValue value5)
            {
                ForestDisjointSet<TValue> set = MakeAndFillSet();
                set.Union(value1, value2);
                AssertSetSizes(set, 4, 5);
                Assert.IsTrue(set.AreInSameSet(value1, value2));

                set.Union(value4, value3);
                AssertSetSizes(set, 3, 5);
                Assert.IsTrue(set.AreInSameSet(value3, value4));

                set.Union(value1, value4);
                AssertSetSizes(set, 2, 5);
                Assert.IsTrue(set.AreInSameSet(value1, value2));
                Assert.IsTrue(set.AreInSameSet(value1, value3));
                Assert.IsTrue(set.AreInSameSet(value1, value4));

                set.Union(value1, value5);
                AssertSetSizes(set, 1, 5);
                Assert.IsTrue(set.AreInSameSet(value1, value2));
                Assert.IsTrue(set.AreInSameSet(value1, value3));
                Assert.IsTrue(set.AreInSameSet(value1, value4));
                Assert.IsTrue(set.AreInSameSet(value1, value5));

                // Already merged
                set.Union(value1, value1);
                AssertSetSizes(set, 1, 5);
                Assert.IsTrue(set.AreInSameSet(value1, value2));
                Assert.IsTrue(set.AreInSameSet(value1, value3));
                Assert.IsTrue(set.AreInSameSet(value1, value4));
                Assert.IsTrue(set.AreInSameSet(value1, value5));

                set.Union(value1, value4);
                AssertSetSizes(set, 1, 5);
                Assert.IsTrue(set.AreInSameSet(value1, value2));
                Assert.IsTrue(set.AreInSameSet(value1, value3));
                Assert.IsTrue(set.AreInSameSet(value1, value4));
                Assert.IsTrue(set.AreInSameSet(value1, value5));

                set.Union(value4, value1);
                AssertSetSizes(set, 1, 5);
                Assert.IsTrue(set.AreInSameSet(value1, value2));
                Assert.IsTrue(set.AreInSameSet(value1, value3));
                Assert.IsTrue(set.AreInSameSet(value1, value4));
                Assert.IsTrue(set.AreInSameSet(value1, value5));


                set = MakeAndFillSet();
                set.Union(value1, value2);
                AssertSetSizes(set, 4, 5);
                Assert.IsTrue(set.AreInSameSet(value1, value2));

                set.Union(value2, value4);
                AssertSetSizes(set, 3, 5);
                Assert.IsTrue(set.AreInSameSet(value1, value2));
                Assert.IsTrue(set.AreInSameSet(value1, value4));

                set.Union(value5, value2);
                AssertSetSizes(set, 2, 5);
                Assert.IsTrue(set.AreInSameSet(value1, value2));
                Assert.IsTrue(set.AreInSameSet(value1, value4));
                Assert.IsTrue(set.AreInSameSet(value1, value5));

                #region Local function

                ForestDisjointSet<TValue> MakeAndFillSet()
                {
                    var s = new ForestDisjointSet<TValue>();

                    s.MakeSet(value1);
                    s.MakeSet(value2);
                    s.MakeSet(value3);
                    s.MakeSet(value4);
                    s.MakeSet(value5);
                    AssertSetSizes(s, 5, 5);

                    return s;
                }

                #endregion
            }

            #endregion
        }

        [Test]
        public void Union_Throws()
        {
            var vertex = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var set = new ForestDisjointSet<TestVertex>();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => set.Union(vertex, null));
            Assert.Throws<ArgumentNullException>(() => set.Union(null, vertex));
            Assert.Throws<ArgumentNullException>(() => set.Union(null, null));
            // ReSharper restore AssignNullToNotNullAttribute

            Assert.Throws<KeyNotFoundException>(() => set.Union(vertex, vertex));
            Assert.Throws<KeyNotFoundException>(() => set.Union(vertex, vertex2));
            Assert.Throws<KeyNotFoundException>(() => set.Union(vertex2, vertex));
            Assert.Throws<KeyNotFoundException>(() => set.Union(vertex2, vertex2));

            set.MakeSet(vertex);
            Assert.DoesNotThrow(() => set.Union(vertex, vertex));
            Assert.Throws<KeyNotFoundException>(() => set.Union(vertex, vertex2));
            Assert.Throws<KeyNotFoundException>(() => set.Union(vertex2, vertex));
            Assert.Throws<KeyNotFoundException>(() => set.Union(vertex2, vertex2));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void Contains()
        {
            ContainsTest(1, 2);
            ContainsTest(
                new TestVertex("1"),
                new TestVertex("2"));
            ContainsTest(
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"));

            #region Local function

            void ContainsTest<TValue>(TValue value1, TValue value2)
            {
                var set = new ForestDisjointSet<TValue>();
                Assert.IsFalse(set.Contains(value1));
                Assert.IsFalse(set.Contains(value2));

                set.MakeSet(value1);
                Assert.IsTrue(set.Contains(value1));
                Assert.IsFalse(set.Contains(value2));

                set.MakeSet(value2);
                Assert.IsTrue(set.Contains(value1));
                Assert.IsTrue(set.Contains(value2));

                set.Union(value1, value2);
                Assert.IsTrue(set.Contains(value1));
                Assert.IsTrue(set.Contains(value2));
            }

            #endregion
        }

        [Test]
        public void Contains_Throws()
        {
            var set = new ForestDisjointSet<TestVertex>();
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => set.Contains(null));
        }
    }
}