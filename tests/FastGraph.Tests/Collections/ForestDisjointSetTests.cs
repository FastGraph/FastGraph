#nullable enable

using NUnit.Framework;
using FastGraph.Collections;
using FluentAssertions.Execution;
using static FastGraph.Tests.AssertHelpers;

namespace FastGraph.Tests.Collections
{
    /// <summary>
    /// Tests for <see cref="ForestDisjointSet{T}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class ForestDisjointSetTests
    {
        #region Test helpers

        [CustomAssertion]
        private static void AssertSetSizes<T>(
            ForestDisjointSet<T> set,
            int expectedSetCount,
            int expectedCount)
            where T : notnull
        {
            using (_ = new AssertionScope())
            {
                set.SetCount.Should().Be(expectedSetCount);
                set.ElementCount.Should().Be(expectedCount);
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            AssertSetProperties(new ForestDisjointSet<int>());
            AssertSetProperties(new ForestDisjointSet<int>(12));

            #region Local function

            void AssertSetProperties<T>(ForestDisjointSet<T> set)
                where T : notnull
            {
                AssertSetSizes(set, 0, 0);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            Invoking(() => new ForestDisjointSet<int>(-1)).Should().Throw<ArgumentOutOfRangeException>();
            Invoking(() => new ForestDisjointSet<int>(int.MaxValue)).Should().Throw<ArgumentOutOfRangeException>();
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
                where TValue : notnull
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
#pragma warning disable CS8625
            Invoking(() => set.MakeSet(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625

            set.MakeSet(testObject);
            // Double insert
            Invoking(() => set.MakeSet(testObject)).Should().Throw<ArgumentException>();
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
                where TValue : notnull
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
#pragma warning disable CS8625
            Invoking(() => set.FindSet(default)).Should().Throw<ArgumentNullException>();
            Invoking(() => set.FindSet(vertex)).Should().Throw<KeyNotFoundException>();
#pragma warning restore CS8625
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
                where TValue : notnull
            {
                var set = new ForestDisjointSet<TValue>();
                AssertSetSizes(set, 0, 0);

                set.MakeSet(value1);
                set.AreInSameSet(value1, value1).Should().BeTrue();

                set.MakeSet(value2);
                set.AreInSameSet(value1, value1).Should().BeTrue();
                set.AreInSameSet(value1, value2).Should().BeFalse();
                set.AreInSameSet(value2, value1).Should().BeFalse();
                set.AreInSameSet(value2, value2).Should().BeTrue();

                set.Union(value1, value2);
                set.AreInSameSet(value1, value1).Should().BeTrue();
                set.AreInSameSet(value1, value2).Should().BeTrue();
                set.AreInSameSet(value2, value1).Should().BeTrue();
                set.AreInSameSet(value2, value2).Should().BeTrue();
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
#pragma warning disable CS8625
            Invoking(() => set.AreInSameSet(vertex, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => set.AreInSameSet(default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => set.AreInSameSet(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute

            Invoking(() => set.AreInSameSet(vertex, vertex)).Should().Throw<KeyNotFoundException>();
            Invoking(() => set.AreInSameSet(vertex, vertex2)).Should().Throw<KeyNotFoundException>();
            Invoking(() => set.AreInSameSet(vertex2, vertex)).Should().Throw<KeyNotFoundException>();
            Invoking(() => set.AreInSameSet(vertex2, vertex2)).Should().Throw<KeyNotFoundException>();

            set.MakeSet(vertex);
            Invoking((Func<bool>)(() => set.AreInSameSet(vertex, vertex))).Should().NotThrow();
            Invoking(() => set.AreInSameSet(vertex, vertex2)).Should().Throw<KeyNotFoundException>();
            Invoking(() => set.AreInSameSet(vertex2, vertex)).Should().Throw<KeyNotFoundException>();
            Invoking(() => set.AreInSameSet(vertex2, vertex2)).Should().Throw<KeyNotFoundException>();
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
                where TValue : notnull
            {
                ForestDisjointSet<TValue> set = MakeAndFillSet();
                set.Union(value1, value2);
                AssertSetSizes(set, 4, 5);
                set.AreInSameSet(value1, value2).Should().BeTrue();

                set.Union(value4, value3);
                AssertSetSizes(set, 3, 5);
                set.AreInSameSet(value3, value4).Should().BeTrue();

                set.Union(value1, value4);
                AssertSetSizes(set, 2, 5);
                set.AreInSameSet(value1, value2).Should().BeTrue();
                set.AreInSameSet(value1, value3).Should().BeTrue();
                set.AreInSameSet(value1, value4).Should().BeTrue();

                set.Union(value1, value5);
                AssertSetSizes(set, 1, 5);
                set.AreInSameSet(value1, value2).Should().BeTrue();
                set.AreInSameSet(value1, value3).Should().BeTrue();
                set.AreInSameSet(value1, value4).Should().BeTrue();
                set.AreInSameSet(value1, value5).Should().BeTrue();

                // Already merged
                set.Union(value1, value1);
                AssertSetSizes(set, 1, 5);
                set.AreInSameSet(value1, value2).Should().BeTrue();
                set.AreInSameSet(value1, value3).Should().BeTrue();
                set.AreInSameSet(value1, value4).Should().BeTrue();
                set.AreInSameSet(value1, value5).Should().BeTrue();

                set.Union(value1, value4);
                AssertSetSizes(set, 1, 5);
                set.AreInSameSet(value1, value2).Should().BeTrue();
                set.AreInSameSet(value1, value3).Should().BeTrue();
                set.AreInSameSet(value1, value4).Should().BeTrue();
                set.AreInSameSet(value1, value5).Should().BeTrue();

                set.Union(value4, value1);
                AssertSetSizes(set, 1, 5);
                set.AreInSameSet(value1, value2).Should().BeTrue();
                set.AreInSameSet(value1, value3).Should().BeTrue();
                set.AreInSameSet(value1, value4).Should().BeTrue();
                set.AreInSameSet(value1, value5).Should().BeTrue();


                set = MakeAndFillSet();
                set.Union(value1, value2);
                AssertSetSizes(set, 4, 5);
                set.AreInSameSet(value1, value2).Should().BeTrue();

                set.Union(value2, value4);
                AssertSetSizes(set, 3, 5);
                set.AreInSameSet(value1, value2).Should().BeTrue();
                set.AreInSameSet(value1, value4).Should().BeTrue();

                set.Union(value5, value2);
                AssertSetSizes(set, 2, 5);
                set.AreInSameSet(value1, value2).Should().BeTrue();
                set.AreInSameSet(value1, value4).Should().BeTrue();
                set.AreInSameSet(value1, value5).Should().BeTrue();

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
#pragma warning disable CS8625
            Invoking(() => set.Union(vertex, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => set.Union(default, vertex)).Should().Throw<ArgumentNullException>();
            Invoking(() => set.Union(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute

            Invoking(() => set.Union(vertex, vertex)).Should().Throw<KeyNotFoundException>();
            Invoking(() => set.Union(vertex, vertex2)).Should().Throw<KeyNotFoundException>();
            Invoking(() => set.Union(vertex2, vertex)).Should().Throw<KeyNotFoundException>();
            Invoking(() => set.Union(vertex2, vertex2)).Should().Throw<KeyNotFoundException>();

            set.MakeSet(vertex);
            Invoking((Func<bool>)(() => set.Union(vertex, vertex))).Should().NotThrow();
            Invoking(() => set.Union(vertex, vertex2)).Should().Throw<KeyNotFoundException>();
            Invoking(() => set.Union(vertex2, vertex)).Should().Throw<KeyNotFoundException>();
            Invoking(() => set.Union(vertex2, vertex2)).Should().Throw<KeyNotFoundException>();
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
                where TValue : notnull
            {
                var set = new ForestDisjointSet<TValue>();
                set.Contains(value1).Should().BeFalse();
                set.Contains(value2).Should().BeFalse();

                set.MakeSet(value1);
                set.Contains(value1).Should().BeTrue();
                set.Contains(value2).Should().BeFalse();

                set.MakeSet(value2);
                set.Contains(value1).Should().BeTrue();
                set.Contains(value2).Should().BeTrue();

                set.Union(value1, value2);
                set.Contains(value1).Should().BeTrue();
                set.Contains(value2).Should().BeTrue();
            }

            #endregion
        }

        [Test]
        public void Contains_Throws()
        {
            var set = new ForestDisjointSet<TestVertex>();
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => set.Contains(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }
    }
}
