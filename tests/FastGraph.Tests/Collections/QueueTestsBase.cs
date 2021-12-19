#nullable enable

using FastGraph.Collections;
using static FastGraph.Tests.AssertHelpers;

namespace FastGraph.Tests.Collections
{
    /// <summary>
    /// Base class for queue tests.
    /// </summary>
    internal abstract class QueueTestsBase
    {
        protected static void Contains_Test<TVertex>(
            IQueue<TVertex> queue,
            TVertex vertex1,
            TVertex vertex2)
            where TVertex : notnull
        {
            queue.Contains(vertex1).Should().BeFalse();
            queue.Contains(vertex2).Should().BeFalse();

            queue.Enqueue(vertex1);
            queue.Contains(vertex1).Should().BeTrue();
            queue.Contains(vertex2).Should().BeFalse();

            queue.Enqueue(vertex2);
            queue.Contains(vertex1).Should().BeTrue();
            queue.Contains(vertex2).Should().BeTrue();

            queue.Enqueue(vertex2);
            queue.Contains(vertex1).Should().BeTrue();
            queue.Contains(vertex2).Should().BeTrue();

            queue.Dequeue();
            queue.Contains(vertex1).Should().BeFalse();
            queue.Contains(vertex2).Should().BeTrue();

            queue.Dequeue();
            queue.Contains(vertex1).Should().BeFalse();
            queue.Contains(vertex2).Should().BeTrue();

            queue.Dequeue();
            queue.Contains(vertex1).Should().BeFalse();
            queue.Contains(vertex2).Should().BeFalse();
        }

        protected static void Enqueue_Test<TVertex>(
            IQueue<TVertex> queue,
            TVertex vertex1,
            TVertex vertex2)
            where TVertex : notnull
        {
            queue.Count.Should().Be(0);

            queue.Enqueue(vertex1);
            queue.Count.Should().Be(1);

            queue.Enqueue(vertex2);
            queue.Count.Should().Be(2);

            queue.Enqueue(vertex2);
            queue.Count.Should().Be(3);
        }

        protected static void Dequeue_Test<TVertex>(
            Func<Func<TVertex, double>, IQueue<TVertex>> createQueue,
            TVertex vertex1,
            TVertex vertex2,
            TVertex vertex3,
            TVertex vertex4)
            where TVertex : notnull
        {
            DequeueInternalTest();
            DequeueInternalSameDistanceTest();

            #region Local functions

            void DequeueInternalTest()
            {
                var order = new Stack<int>(new[] { 3, 2, 9, 1, 10 });
                IQueue<TVertex> queue = createQueue(_ => order.Pop());
                queue.Count.Should().Be(0);

                queue.Enqueue(vertex1);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex3);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex4);
                queue.Count.Should().Be(5);

                AssertEqual(vertex2, queue.Dequeue());
                queue.Count.Should().Be(4);

                AssertEqual(vertex2, queue.Dequeue());
                queue.Count.Should().Be(3);

                AssertEqual(vertex4, queue.Dequeue());
                queue.Count.Should().Be(2);

                AssertEqual(vertex3, queue.Dequeue());
                queue.Count.Should().Be(1);

                AssertEqual(vertex1, queue.Dequeue());
                queue.Count.Should().Be(0);
            }

            void DequeueInternalSameDistanceTest()
            {
                IQueue<TVertex> queue = createQueue(_ => 1.0);
                queue.Count.Should().Be(0);

                queue.Enqueue(vertex1);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex3);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex4);
                queue.Count.Should().Be(5);

                var counters = new Dictionary<TVertex, int>
                {
                    [vertex1] = 0,
                    [vertex2] = 0,
                    [vertex3] = 0,
                    [vertex4] = 0
                };
                // Cannot assert exactly dequeued item since
                // it depends on queue internal implementation
                ++counters[queue.Dequeue()];
                queue.Count.Should().Be(4);

                ++counters[queue.Dequeue()];
                queue.Count.Should().Be(3);

                ++counters[queue.Dequeue()];
                queue.Count.Should().Be(2);

                ++counters[queue.Dequeue()];
                queue.Count.Should().Be(1);

                ++counters[queue.Dequeue()];
                queue.Count.Should().Be(0);

                counters.Should().BeEquivalentTo(new[]
                {
                    new KeyValuePair<TVertex, int>(vertex1, 1),
                    new KeyValuePair<TVertex, int>(vertex2, 2),
                    new KeyValuePair<TVertex, int>(vertex3, 1),
                    new KeyValuePair<TVertex, int>(vertex4, 1)
                });
            }

            #endregion
        }

        protected static void Dequeue_Throws_Test<TVertex>(IQueue<TVertex> queue)
            where TVertex : notnull
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Invoking(() => queue.Dequeue()).Should().Throw<InvalidOperationException>();
        }

        protected static void Peek_Test<TVertex>(
            Func<Func<TVertex, double>, IQueue<TVertex>> createQueue,
            TVertex vertex1,
            TVertex vertex2,
            TVertex vertex3)
            where TVertex : notnull
        {
            PeekInternalTest();
            PeekInternalSameDistanceTest();

            #region Local functions

            void PeekInternalTest()
            {
                var order = new Stack<int>(new[] { 3, 2, 9, 1, 10 });
                IQueue<TVertex> queue = createQueue(_ => order.Pop());
                queue.Count.Should().Be(0);

                queue.Enqueue(vertex3);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex1);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex1);
                queue.Count.Should().Be(5);

                AssertEqual(vertex2, queue.Peek());
                queue.Count.Should().Be(5);

                queue.Dequeue();
                queue.Dequeue();
                queue.Count.Should().Be(3);

                AssertEqual(vertex1, queue.Peek());
                queue.Count.Should().Be(3);

                queue.Dequeue();
                queue.Dequeue();
                queue.Count.Should().Be(1);

                AssertEqual(vertex3, queue.Peek());
                queue.Count.Should().Be(1);
            }

            void PeekInternalSameDistanceTest()
            {
                IQueue<TVertex> queue = createQueue(_ => 1.0);
                queue.Count.Should().Be(0);

                queue.Enqueue(vertex3);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex1);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex1);
                queue.Count.Should().Be(5);

                // ReSharper disable ReturnValueOfPureMethodIsNotUsed
                // Cannot assert exactly peeked item since
                // it depends on queue internal implementation
                Invoking((Func<TVertex>)(() => queue.Peek())).Should().NotThrow();
                queue.Count.Should().Be(5);

                queue.Dequeue();
                queue.Dequeue();
                queue.Count.Should().Be(3);

                Invoking((Func<TVertex>)(() => queue.Peek())).Should().NotThrow();
                queue.Count.Should().Be(3);

                queue.Dequeue();
                queue.Dequeue();
                queue.Count.Should().Be(1);

                Invoking((Func<TVertex>)(() => queue.Peek())).Should().NotThrow();
                queue.Count.Should().Be(1);
                // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            }

            #endregion
        }

        protected static void Peek_Throws_Test<TVertex>(IQueue<TVertex> queue)
            where TVertex : notnull
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Invoking(() => queue.Peek()).Should().Throw<InvalidOperationException>();
        }

        protected static void Update_Test<TVertex>(
            Func<Func<TVertex, double>, IPriorityQueue<TVertex>> createQueue,
            TVertex vertex1,
            TVertex vertex2)
            where TVertex : notnull
        {
            var distances = new Stack<double>(new[] { 0.5, 10.0, 5.0, 1.0 });
            IPriorityQueue<TVertex> queue = createQueue(_ => distances.Pop());
            queue.Count.Should().Be(0);

            queue.Enqueue(vertex1);
            queue.Enqueue(vertex2);
            queue.Count.Should().Be(2);

            AssertEqual(vertex1, queue.Peek());

            queue.Update(vertex1);  // Distance from 1.0 to 10.0
            queue.Count.Should().Be(2);

            AssertEqual(vertex2, queue.Peek());
        }

        protected static void ToArray_Test<TVertex>(
            Func<Func<TVertex, double>, IQueue<TVertex>> createQueue,
            TVertex vertex1,
            TVertex vertex2,
            TVertex vertex3,
            TVertex vertex4)
            where TVertex : notnull
        {
            var distances = new Stack<double>(new[] { 123.0, 3.0, 2.0, 4.0, 5.0, 1.0 });
            IQueue<TVertex> queue = createQueue(_ => distances.Pop());

            // Empty heap
            queue.ToArray().Should().BeEmpty();

            queue.Enqueue(vertex1);
            queue.Enqueue(vertex2);
            queue.Enqueue(vertex2);
            queue.Enqueue(vertex3);
            queue.Enqueue(vertex1);

            // Array not sorted with distance
            queue.ToArray().Should().BeEquivalentTo(new[] { vertex1, vertex2, vertex2, vertex3, vertex1 });

            queue.Dequeue();
            queue.Dequeue();

            // Array not sorted with distance
            queue.ToArray().Should().BeEquivalentTo(new[] { vertex1, vertex2, vertex2 });

            queue.Dequeue();
            queue.Dequeue();
            queue.Dequeue();

            queue.ToArray().Should().BeEmpty();

            queue.Enqueue(vertex4);
            new[] { vertex4 }.Should().BeEquivalentTo(queue.ToArray());
        }
    }
}
