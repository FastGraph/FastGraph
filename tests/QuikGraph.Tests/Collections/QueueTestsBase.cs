using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;
using static QuikGraph.Tests.AssertHelpers;

namespace QuikGraph.Tests.Collections
{
    /// <summary>
    /// Base class for queue tests.
    /// </summary>
    internal abstract class QueueTestsBase
    {
        protected static void Contains_Test<TVertex>(
            [NotNull] IQueue<TVertex> queue,
            [NotNull] TVertex vertex1,
            [NotNull] TVertex vertex2)
        {
            Assert.IsFalse(queue.Contains(vertex1));
            Assert.IsFalse(queue.Contains(vertex2));

            queue.Enqueue(vertex1);
            Assert.IsTrue(queue.Contains(vertex1));
            Assert.IsFalse(queue.Contains(vertex2));

            queue.Enqueue(vertex2);
            Assert.IsTrue(queue.Contains(vertex1));
            Assert.IsTrue(queue.Contains(vertex2));

            queue.Enqueue(vertex2);
            Assert.IsTrue(queue.Contains(vertex1));
            Assert.IsTrue(queue.Contains(vertex2));

            queue.Dequeue();
            Assert.IsFalse(queue.Contains(vertex1));
            Assert.IsTrue(queue.Contains(vertex2));

            queue.Dequeue();
            Assert.IsFalse(queue.Contains(vertex1));
            Assert.IsTrue(queue.Contains(vertex2));

            queue.Dequeue();
            Assert.IsFalse(queue.Contains(vertex1));
            Assert.IsFalse(queue.Contains(vertex2));
        }

        protected static void Enqueue_Test<TVertex>(
            [NotNull] IQueue<TVertex> queue,
            [NotNull] TVertex vertex1,
            [NotNull] TVertex vertex2)
        {
            Assert.AreEqual(0, queue.Count);

            queue.Enqueue(vertex1);
            Assert.AreEqual(1, queue.Count);

            queue.Enqueue(vertex2);
            Assert.AreEqual(2, queue.Count);

            queue.Enqueue(vertex2);
            Assert.AreEqual(3, queue.Count);
        }

        protected static void Dequeue_Test<TVertex>(
            [NotNull] Func<Func<TVertex, double>, IQueue<TVertex>> createQueue,
            [NotNull] TVertex vertex1,
            [NotNull] TVertex vertex2,
            [NotNull] TVertex vertex3,
            [NotNull] TVertex vertex4)
        {
            DequeueInternalTest();
            DequeueInternalSameDistanceTest();

            #region Local functions

            void DequeueInternalTest()
            {
                var order = new Stack<int>(new[] { 3, 2, 9, 1, 10 });
                IQueue<TVertex> queue = createQueue(vertex => order.Pop());
                Assert.AreEqual(0, queue.Count);

                queue.Enqueue(vertex1);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex3);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex4);
                Assert.AreEqual(5, queue.Count);

                AssertEqual(vertex2, queue.Dequeue());
                Assert.AreEqual(4, queue.Count);

                AssertEqual(vertex2, queue.Dequeue());
                Assert.AreEqual(3, queue.Count);

                AssertEqual(vertex4, queue.Dequeue());
                Assert.AreEqual(2, queue.Count);

                AssertEqual(vertex3, queue.Dequeue());
                Assert.AreEqual(1, queue.Count);

                AssertEqual(vertex1, queue.Dequeue());
                Assert.AreEqual(0, queue.Count);
            }

            void DequeueInternalSameDistanceTest()
            {
                IQueue<TVertex> queue = createQueue(vertex => 1.0);
                Assert.AreEqual(0, queue.Count);

                queue.Enqueue(vertex1);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex3);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex4);
                Assert.AreEqual(5, queue.Count);

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
                Assert.AreEqual(4, queue.Count);

                ++counters[queue.Dequeue()];
                Assert.AreEqual(3, queue.Count);

                ++counters[queue.Dequeue()];
                Assert.AreEqual(2, queue.Count);

                ++counters[queue.Dequeue()];
                Assert.AreEqual(1, queue.Count);

                ++counters[queue.Dequeue()];
                Assert.AreEqual(0, queue.Count);

                CollectionAssert.AreEquivalent(
                    new[]
                    {
                        new KeyValuePair<TVertex, int>(vertex1, 1),
                        new KeyValuePair<TVertex, int>(vertex2, 2),
                        new KeyValuePair<TVertex, int>(vertex3, 1),
                        new KeyValuePair<TVertex, int>(vertex4, 1)
                    },
                    counters);
            }

            #endregion
        }

        protected static void Dequeue_Throws_Test<TVertex>([NotNull] IQueue<TVertex> queue)
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        protected static void Peek_Test<TVertex>(
            [NotNull] Func<Func<TVertex, double>, IQueue<TVertex>> createQueue,
            [NotNull] TVertex vertex1,
            [NotNull] TVertex vertex2,
            [NotNull] TVertex vertex3)
        {
            PeekInternalTest();
            PeekInternalSameDistanceTest();

            #region Local functions

            void PeekInternalTest()
            {
                var order = new Stack<int>(new[] { 3, 2, 9, 1, 10 });
                IQueue<TVertex> queue = createQueue(vertex => order.Pop());
                Assert.AreEqual(0, queue.Count);

                queue.Enqueue(vertex3);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex1);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex1);
                Assert.AreEqual(5, queue.Count);

                AssertEqual(vertex2, queue.Peek());
                Assert.AreEqual(5, queue.Count);

                queue.Dequeue();
                queue.Dequeue();
                Assert.AreEqual(3, queue.Count);

                AssertEqual(vertex1, queue.Peek());
                Assert.AreEqual(3, queue.Count);

                queue.Dequeue();
                queue.Dequeue();
                Assert.AreEqual(1, queue.Count);

                AssertEqual(vertex3, queue.Peek());
                Assert.AreEqual(1, queue.Count);
            }

            void PeekInternalSameDistanceTest()
            {
                IQueue<TVertex> queue = createQueue(vertex => 1.0);
                Assert.AreEqual(0, queue.Count);

                queue.Enqueue(vertex3);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex1);
                queue.Enqueue(vertex2);
                queue.Enqueue(vertex1);
                Assert.AreEqual(5, queue.Count);

                // ReSharper disable ReturnValueOfPureMethodIsNotUsed
                // Cannot assert exactly peeked item since
                // it depends on queue internal implementation
                Assert.DoesNotThrow(() => queue.Peek());
                Assert.AreEqual(5, queue.Count);

                queue.Dequeue();
                queue.Dequeue();
                Assert.AreEqual(3, queue.Count);

                Assert.DoesNotThrow(() => queue.Peek());
                Assert.AreEqual(3, queue.Count);

                queue.Dequeue();
                queue.Dequeue();
                Assert.AreEqual(1, queue.Count);

                Assert.DoesNotThrow(() => queue.Peek());
                Assert.AreEqual(1, queue.Count);
                // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            }

            #endregion
        }

        protected static void Peek_Throws_Test<TVertex>([NotNull] IQueue<TVertex> queue)
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
        }

        protected static void Update_Test<TVertex>(
            [NotNull] Func<Func<TVertex, double>, IPriorityQueue<TVertex>> createQueue,
            [NotNull] TVertex vertex1,
            [NotNull] TVertex vertex2)
        {
            var distances = new Stack<double>(new[] { 0.5, 10.0, 5.0, 1.0 });
            IPriorityQueue<TVertex> queue = createQueue(vertex => distances.Pop());
            Assert.AreEqual(0, queue.Count);

            queue.Enqueue(vertex1);
            queue.Enqueue(vertex2);
            Assert.AreEqual(2, queue.Count);

            AssertEqual(vertex1, queue.Peek());

            queue.Update(vertex1);  // Distance from 1.0 to 10.0
            Assert.AreEqual(2, queue.Count);

            AssertEqual(vertex2, queue.Peek());
        }

        protected static void ToArray_Test<TVertex>(
            [NotNull] Func<Func<TVertex, double>, IQueue<TVertex>> createQueue,
            [NotNull] TVertex vertex1,
            [NotNull] TVertex vertex2,
            [NotNull] TVertex vertex3,
            [NotNull] TVertex vertex4)
        {
            var distances = new Stack<double>(new[] { 123.0, 3.0, 2.0, 4.0, 5.0, 1.0 });
            IQueue<TVertex> queue = createQueue(vertex => distances.Pop());

            // Empty heap
            CollectionAssert.IsEmpty(queue.ToArray());

            queue.Enqueue(vertex1);
            queue.Enqueue(vertex2);
            queue.Enqueue(vertex2);
            queue.Enqueue(vertex3);
            queue.Enqueue(vertex1);

            // Array not sorted with distance
            CollectionAssert.AreEquivalent(
                new[] { vertex1, vertex2, vertex2, vertex3, vertex1 },
                queue.ToArray());

            queue.Dequeue();
            queue.Dequeue();

            // Array not sorted with distance
            CollectionAssert.AreEquivalent(
                new[] { vertex1, vertex2, vertex2 },
                queue.ToArray());

            queue.Dequeue();
            queue.Dequeue();
            queue.Dequeue();

            CollectionAssert.IsEmpty(queue.ToArray());

            queue.Enqueue(vertex4);
            CollectionAssert.AreEqual(
                new[] { vertex4 },
                queue.ToArray());
        }
    }
}