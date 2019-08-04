using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    internal partial class BinaryHeapTests
    {
        #region Helpers

        private static void Constructor<TPriority, TValue>(int capacity)
        {
            var heap = new BinaryHeap<TPriority, TValue>(capacity, Comparer<TPriority>.Default.Compare);
            Assert.AreEqual(heap.Capacity, capacity);
            AssertInvariant(heap);
        }

        #endregion

        [Test]
        public void Constructor1()
        {
            Constructor<int, Edge<int>>(0);
        }

        [Test]
        public void Constructor2()
        {
            Constructor<int, SEdge<int>>(0);
        }

        [Test]
        public void Constructor3()
        {
            Constructor<int, int>(0);
        }

        [Test]
        public void ConstructorThrowsContractException1()
        {
            Assert.Throws<ArgumentException>(() => Constructor<int, Edge<int>>(int.MinValue));
        }

        [Test]
        public void ConstructorThrowsContractException2()
        {
            Assert.Throws<ArgumentException>(() => Constructor<int, SEdge<int>>(int.MinValue));
        }

        [Test]
        public void ConstructorThrowsContractException3()
        {
            Assert.Throws<ArgumentException>(() => Constructor<int, int>(int.MinValue));
        }
    }
}
