using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    [TestFixture]
    internal class FibonacciHeapTests
    {
        [Test]
        public void CreateHeap()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            Assert.IsNotNull(heap, "Heap is null");
        }

        [Test]
        public void SimpleEnqueueDequeIncreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            for (int i = 0; i < 10; ++i)
            {
                heap.Enqueue(i, i.ToString());
                count++;
            }

            int? lastValue = null;
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                if (lastValue > value.Key)
                {
                    Assert.Fail($"Heap condition has been violated: {lastValue} > {value.Key}");
                }

                lastValue = value.Key;
                --count;
            }

            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void SimpleEnqueueDequeDecreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            int count = 0;
            for (int i = 0; i < 10; ++i)
            {
                heap.Enqueue(i, i.ToString());
                ++count;
            }

            int? lastValue = null;
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                if (lastValue < value.Key)
                {
                    Assert.Fail("Heap condition has been violated.");
                }

                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void DecreaseKeyOnIncreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; ++i)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                ++count;
            }

            int? lastValue = null;
            heap.ChangeKey(cells[9], -1);
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                if (lastValue > value.Key)
                {
                    Assert.Fail("Heap condition has been violated.");
                }

                lastValue = value.Key;
                --count;
            }

            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void IncreaseKeyOnIncreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; ++i)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                ++count;
            }

            int? lastValue = null;
            heap.ChangeKey(cells[0], 100);
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                if (lastValue > value.Key)
                {
                    Assert.Fail("Heap condition has been violated.");
                }

                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void DecreaseKeyOnDecreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                ++count;
            }

            int? lastValue = null;
            heap.ChangeKey(cells[9], -1);
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                if (lastValue < value.Key)
                {
                    Assert.Fail("Heap condition has been violated.");
                }

                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void IncreaseKeyOnDecreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; ++i)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                ++count;
            }

            int? lastValue = null;
            heap.ChangeKey(cells[0], 100);
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                if (lastValue < value.Key)
                {
                    Assert.Fail("Heap condition has been violated.");
                }

                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void ChangeKeyToSelf()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; ++i)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                ++count;
            }

            int? lastValue = null;
            heap.ChangeKey(cells[0], 0);
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                if (lastValue < value.Key)
                {
                    Assert.Fail("Heap condition has been violated.");
                }

                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void IncreasingDecreaseKeyCascadeCut()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; ++i)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                ++count;
            }

            int lastValue = heap.Top.Priority;
            heap.Dequeue();
            --count;
            heap.ChangeKey(cells[6], 3);
            heap.ChangeKey(cells[7], 2);
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue > value.Key)
                {
                    Assert.Fail("Heap condition has been violated.");
                }

                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void IncreasingIncreaseKeyCascadeCut()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; ++i)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }

            int lastValue = heap.Top.Priority;
            heap.Dequeue();
            --count;
            heap.ChangeKey(cells[5], 10);
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue > value.Key)
                {
                    Assert.Fail($"Heap condition has been violated: {lastValue} > {value.Key}");
                }

                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void DeleteKey()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; ++i)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                ++count;
            }

            int? lastValue = null;
            heap.Dequeue();
            FibonacciHeapCell<int, string> deletedCell = cells[8];
            heap.Delete(deletedCell);
            count -= 2;
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                Assert.IsFalse(lastValue > value.Key, "Heap condition has been violated.");
                Assert.AreNotEqual(deletedCell, value, "Found item that was deleted.");
                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void EnumeratorIncreasing()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            for (int i = 10; i >= 0; --i)
            {
                heap.Enqueue(i, i.ToString());
                ++count;
            }

            int? lastValue = null;
            foreach (KeyValuePair<int, string> value in heap)
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                if (lastValue > value.Key)
                {
                    Assert.Fail("Heap condition has been violated.");
                }

                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void Merge()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var heap2 = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            for (int i = 11; i > 0; --i)
            {
                heap.Enqueue(i, i.ToString());
                heap2.Enqueue(i * 11, i.ToString());
                count += 2;
            }

            heap2.Merge(heap);

            int? lastValue = null;
            foreach (KeyValuePair<int, string> value in heap2.GetDestructiveEnumerator())
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                if (lastValue > value.Key)
                {
                    Assert.Fail("Heap condition has been violated.");
                }

                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void NextCutOnLessThan()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var heap2 = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var toCutNodes = new List<FibonacciHeapCell<int, string>>();

            heap.Enqueue(0, "0");
            toCutNodes.Add(heap.Enqueue(5, "5"));
            toCutNodes.Add(heap.Enqueue(6, "6"));
            toCutNodes.Add(heap.Enqueue(7, "7"));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            heap.Enqueue(1, "1");
            heap2.Enqueue(4, "4");
            heap2.Enqueue(5, "5");
            heap2.Enqueue(-10, "-10");
            heap2.Dequeue();
            heap.Merge(heap2);
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            toCutNodes.ForEach(x => heap.ChangeKey(x, -5));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();

            int count = 7;
            int? lastValue = null;
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                if (lastValue > value.Key)
                {
                    Assert.Fail("Heap condition has been violated.");
                }

                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void NextCutOnGreaterThan()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var heap2 = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var toCutNodes = new List<FibonacciHeapCell<int, string>>();

            heap.Enqueue(1, "1");
            toCutNodes.Add(heap.Enqueue(5, "5"));
            toCutNodes.Add(heap.Enqueue(6, "6"));
            toCutNodes.Add(heap.Enqueue(7, "7"));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            heap.Enqueue(0, "0");
            heap2.Enqueue(-1, "-1");
            heap2.Enqueue(5, "5");
            heap2.Enqueue(-10, "-10");
            heap2.Dequeue();
            heap.Merge(heap2);
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            toCutNodes.ForEach(x => heap.ChangeKey(x, -5));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();

            int count = 7;
            int? lastValue = null;
            foreach (KeyValuePair<int, string> value in heap.GetDestructiveEnumerator())
            {
                if (lastValue is null)
                {
                    lastValue = value.Key;
                }

                if (lastValue > value.Key)
                {
                    Assert.Fail("Heap condition has been violated.");
                }

                lastValue = value.Key;
                --count;
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }

        [Test]
        public void RandomTest()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var rand = new Random(10);
            const int numberOfRecords = 10000;
            const int rangeMultiplier = 10;

            int count = 0;
            var cells = new List<FibonacciHeapCell<int, string>>();
            for (int i = 0; i < numberOfRecords; ++i)
            {
                cells.Add(heap.Enqueue(rand.Next(0, numberOfRecords * rangeMultiplier), i.ToString()));
                ++count;
            }

            while (!heap.IsEmpty)
            {
                int action = rand.Next(1, 6);
                int i = 0;
                while (action == 1 && i < 2)
                {
                    action = rand.Next(1, 6);
                    ++i;
                }

                int lastValue = int.MinValue;   // Seems there is a flaw there
                switch (action)
                {
                    case 1:
                        cells.Add(heap.Enqueue(rand.Next(0, numberOfRecords * rangeMultiplier), "SomeValue"));
                        ++count;
                        break;
                    case 2:
                        Assert.IsFalse(lastValue > heap.Top.Priority, "Heap Condition Violated");
                        lastValue = heap.Top.Priority;
                        cells.Remove(heap.Top);
                        heap.Dequeue();
                        --count;
                        break;
                    case 3:
                        int deleteIndex = rand.Next(0, cells.Count);
                        heap.Delete(cells[deleteIndex]);
                        cells.RemoveAt(deleteIndex);
                        --count;
                        break;
                    case 4:
                        int decreaseIndex = rand.Next(0, cells.Count);
                        int newValue = rand.Next(0, cells[decreaseIndex].Priority);
                        if (newValue < lastValue)
                        {
                            lastValue = newValue;
                        }
                        heap.ChangeKey(cells[decreaseIndex], newValue);
                        break;
                    case 5:
                        int increaseIndex = rand.Next(0, cells.Count);
                        heap.ChangeKey(cells[increaseIndex], rand.Next(cells[increaseIndex].Priority, numberOfRecords * rangeMultiplier));
                        break;
                }
            }
            Assert.AreEqual(0, count, "Not all elements enqueued were dequeued.");
        }
    }
}
