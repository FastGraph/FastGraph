using System;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    /// <summary>
    /// Tests for <see cref="BinaryHeap{TPriority,TValue}"/>.
    /// </summary>
    [TestFixture]
    internal partial class BinaryHeapTests
    {
        #region Helpers

        /// <summary>
        /// Checks heap invariant values.
        /// </summary>
        private static void AssertInvariant<TPriority, TValue>([NotNull] BinaryHeap<TPriority, TValue> heap)
        {
            Assert.GreaterOrEqual(heap.Capacity, 0);
            Assert.GreaterOrEqual(heap.Count, 0);
            Assert.LessOrEqual(heap.Count, heap.Capacity);
        }

        private static void CheckHeapSizes(
            [NotNull] BinaryHeap<int, int> heap,
            int expectedCapacity,
            int expectedCount)
        {
            Assert.IsNotNull(heap);
            Assert.IsNotNull(heap.PriorityComparison);
            Assert.AreEqual(expectedCapacity, heap.Capacity);
            Assert.AreEqual(expectedCount, heap.Count);
        }

        #endregion

        [Test]
        public void UpdateTest()
        {
            BinaryHeap<int, int> heap = BinaryHeapFactory.ExampleHeap01();
            heap.Update(1, 4);
            Assert.AreEqual(15, heap.Count);
            heap.AssertInvariants();
        }

        [Test]
        public void UpdateTestUsing_DCT8()
        {
            BinaryHeap<int, int> heap = BinaryHeapFactory.ExampleHeapFromTopologicalSortOfDCT8();
            heap.Update(1, 320);
            heap.AssertInvariants();
        }

        [Test]
        public void RemoveMinimumTest()
        {
            BinaryHeap<int, int> heap = BinaryHeapFactory.ExampleHeap01();
            heap.RemoveMinimum();
            Assert.AreEqual(14, heap.Count);
            heap.AssertInvariants();
        }

        [Test]
        public void RemoveMinimumTestUsing_DCT8()
        {
            BinaryHeap<int, int> heap = BinaryHeapFactory.ExampleHeapFromTopologicalSortOfDCT8();
            heap.RemoveMinimum();
            heap.AssertInvariants();
        }

        [Test]
        public void RemoveMinimumOnEmpty()
        {
            Assert.Throws<InvalidOperationException>(() => new BinaryHeap<int, int>().RemoveMinimum());
        }

        [Test]
        public void MinimumOnEmpty()
        {
            Assert.Throws<InvalidOperationException>(() => new BinaryHeap<int, int>().Minimum());
        }
    }
}
