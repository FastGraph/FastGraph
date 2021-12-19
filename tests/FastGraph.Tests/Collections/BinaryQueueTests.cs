#nullable enable

using NUnit.Framework;
using FastGraph.Collections;
using static FastGraph.Collections.HeapConstants;
using static FastGraph.Tests.AssertHelpers;

namespace FastGraph.Tests.Collections
{
    /// <summary>
    /// Tests for <see cref="BinaryQueue{TVertex,TDistance}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class BinaryQueueTests : QueueTestsBase
    {
        [Test]
        public void Constructors()
        {
            AssertQueueProperties(
                new BinaryQueue<int, double>(_ => 1.0));

            AssertQueueProperties(
                new BinaryQueue<int, double>(
                    _ => 1.0,
                    (dist1, dist2) => dist1.CompareTo(dist2)));

            #region Local function

            void AssertQueueProperties<TVertex, TDistance>(
                BinaryQueue<TVertex, TDistance> queue)
                where TVertex : notnull
                where TDistance : notnull
            {
                queue.Count.Should().Be(0);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new BinaryQueue<int, double>(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new BinaryQueue<int, double>(_ => 1.0, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new BinaryQueue<int, double>(default, (dist1, dist2) => dist1.CompareTo(dist2))).Should().Throw<ArgumentNullException>();
            Invoking(() => new BinaryQueue<int, double>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Contains()
        {
            Contains_Test(
                new BinaryQueue<int, double>(_ => 1.0),
                1,
                2);
            Contains_Test(
                new BinaryQueue<TestVertex, double>(_ => 1.0),
                new TestVertex("1"),
                new TestVertex("2"));
            Contains_Test(
                new BinaryQueue<EquatableTestVertex, double>(_ => 1.0),
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"));
        }

        [Test]
        public void Enqueue()
        {
            Enqueue_Test(
                new BinaryQueue<int, double>(_ => 1.0),
                1,
                2);
            Enqueue_Test(
                new BinaryQueue<TestVertex, double>(_ => 1.0),
                new TestVertex("1"),
                new TestVertex("2"));
            Enqueue_Test(
                new BinaryQueue<EquatableTestVertex, double>(_ => 1.0),
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"));
        }

        [Test]
        public void Dequeue()
        {
            Dequeue_Test(
                distFunc => new BinaryQueue<int, double>(distFunc),
                1,
                2,
                3,
                4);
            Dequeue_Test(
                distFunc => new BinaryQueue<TestVertex, double>(distFunc),
                new TestVertex("1"),
                new TestVertex("2"),
                new TestVertex("3"),
                new TestVertex("4"));
            Dequeue_Test(
                distFunc => new BinaryQueue<EquatableTestVertex, double>(distFunc),
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"),
                new EquatableTestVertex("3"),
                new EquatableTestVertex("4"));
        }

        [Test]
        public void Dequeue_Throws()
        {
            Dequeue_Throws_Test(
                new BinaryQueue<int, double>(_ => 1));
        }

        [Test]
        public void Peek()
        {
            Peek_Test(
                distFunc => new BinaryQueue<int, double>(distFunc),
                1,
                2,
                3);
            Peek_Test(
                distFunc => new BinaryQueue<TestVertex, double>(distFunc),
                new TestVertex("1"),
                new TestVertex("2"),
                new TestVertex("3"));
            Peek_Test(
                distFunc => new BinaryQueue<EquatableTestVertex, double>(distFunc),
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"),
                new EquatableTestVertex("3"));
        }

        [Test]
        public void Peek_Throws()
        {
            Peek_Throws_Test(
                new BinaryQueue<int, double>(_ => 1));
        }

        [Test]
        public void Update()
        {
            UpdateInternalTest(
                1,
                2,
                3);
            UpdateInternalTest(
                new TestVertex("1"),
                new TestVertex("2"),
                new TestVertex("3"));
            UpdateInternalTest(
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"),
                new EquatableTestVertex("3"));

            #region Local function

            void UpdateInternalTest<TVertex>(
                TVertex vertex1,
                TVertex vertex2,
                TVertex vertex3)
                where TVertex : notnull
            {
                BinaryQueue<TVertex, double>? queue = default;
                Update_Test(
                    distFunc => queue = new BinaryQueue<TVertex, double>(distFunc),
                    vertex1,
                    vertex2);

                queue!.Update(vertex3);  // Added with distance 0.5
                queue.Count.Should().Be(3);

                AssertEqual(vertex3, queue.Peek());
            }

            #endregion
        }

        [Test]
        public void ToArray()
        {
            ToArray_Test(
                distFunc => new BinaryQueue<int, double>(distFunc),
                1,
                2,
                3,
                4);
            ToArray_Test(
                distFunc => new BinaryQueue<TestVertex, double>(distFunc),
                new TestVertex("1"),
                new TestVertex("2"),
                new TestVertex("3"),
                new TestVertex("4"));
            ToArray_Test(
                distFunc => new BinaryQueue<EquatableTestVertex, double>(distFunc),
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"),
                new EquatableTestVertex("3"),
                new EquatableTestVertex("4"));
        }

        [Test]
        public void ToPairsArray()
        {
            ToPairsArrayTest(1, 2, 3, 4);
            ToPairsArrayTest(
                new TestVertex("1"),
                new TestVertex("2"),
                new TestVertex("3"),
                new TestVertex("4"));
            ToPairsArrayTest(
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"),
                new EquatableTestVertex("3"),
                new EquatableTestVertex("4"));

            #region Local function

            void ToPairsArrayTest<TVertex>(
                TVertex vertex1,
                TVertex vertex2,
                TVertex vertex3,
                TVertex vertex4)
                where TVertex : notnull
            {
                var distances = new Stack<double>(new[] { 123.0, 3.0, 2.0, 4.0, 5.0, 1.0 });
                var queue = new BinaryQueue<TVertex, double>(_ => distances.Pop());

                // Empty heap
                queue.ToPairsArray().Should<KeyValuePair<double, TVertex>>().BeEmpty();

                queue.Enqueue(vertex1);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex3);
                queue.Enqueue(vertex1);

                // Array not sorted with distance
                queue.ToPairsArray().Should().BeEquivalentTo(new[]
                {
                    new KeyValuePair<double, TVertex>(1.0, vertex1),
                    new KeyValuePair<double, TVertex>(5.0, vertex2),
                    new KeyValuePair<double, TVertex>(4.0, vertex2),
                    new KeyValuePair<double, TVertex>(2.0, vertex3),
                    new KeyValuePair<double, TVertex>(3.0, vertex1)
                });

                queue.Dequeue();
                queue.Dequeue();

                // Array not sorted with distance
                queue.ToPairsArray().Should().BeEquivalentTo(new[]
                {
                    new KeyValuePair<double, TVertex>(3.0, vertex1),
                    new KeyValuePair<double, TVertex>(5.0, vertex2),
                    new KeyValuePair<double, TVertex>(4.0, vertex2)
                });

                queue.Dequeue();
                queue.Dequeue();
                queue.Dequeue();

                queue.ToPairsArray().Should<KeyValuePair<double, TVertex>>().BeEmpty();

                queue.Enqueue(vertex4);
                new[] { new KeyValuePair<double, TVertex>(123.0, vertex4) }.Should().BeEquivalentTo(queue.ToPairsArray());
            }

            #endregion
        }

        [Test]
        public void ToString2()
        {
            ToStringTest(1, 2, 3, 4);
            ToStringTest(
                new TestVertex("1"),
                new TestVertex("2"),
                new TestVertex("3"),
                new TestVertex("4"));
            ToStringTest(
                new EquatableTestVertex("1"),
                new EquatableTestVertex("2"),
                new EquatableTestVertex("3"),
                new EquatableTestVertex("4"));

            #region Local function

            void ToStringTest<TVertex>(
                TVertex vertex1,
                TVertex vertex2,
                TVertex vertex3,
                TVertex vertex4)
                where TVertex : notnull
            {
                var queue = new BinaryQueue<TVertex, double>(_ => 1.0);

                // Empty heap => consistent
                StringAssert.StartsWith(Consistent, queue.ToString2());

                queue.Enqueue(vertex1);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex3);
                queue.Enqueue(vertex1);

                StringAssert.StartsWith(Consistent, queue.ToString2());

                queue.Dequeue();
                queue.Dequeue();

                StringAssert.StartsWith(Consistent, queue.ToString2());

                queue.Dequeue();
                queue.Dequeue();
                queue.Dequeue();

                StringAssert.StartsWith(Consistent, queue.ToString2());

                queue.Enqueue(vertex4);

                StringAssert.StartsWith(Consistent, queue.ToString2());
            }

            #endregion
        }
    }
}
