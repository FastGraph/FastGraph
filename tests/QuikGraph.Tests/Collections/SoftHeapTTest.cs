using System;
using NUnit.Framework;
using QuikGraph.Tests;

namespace QuickGraph.Collections
{
    /// <summary>This class contains parameterized unit tests for SoftHeap`2</summary>
    [TestFixture]
    internal class SoftHeapTKeyTValueTest : QuikGraphUnitTests
    {
        public void Add(int[] keys)
        {
            QuikGraphAssert.TrueForAll(keys, k => k < int.MaxValue);
            Assert.IsTrue(keys.Length > 0);

            var target = new SoftHeap<int, int>(1/4.0, int.MaxValue);
            Console.WriteLine("expected error rate: {0}", target.ErrorRate);
            foreach (var key in keys)
            {
                var count = target.Count;
                target.Add(key, key + 1);
                Assert.AreEqual(count + 1, target.Count);
            }

            int lastMin = int.MaxValue;
            int error = 0;
            while (target.Count > 0)
            {
                var kv = target.DeleteMin();
                if (lastMin < kv.Key)
                    error++;
                lastMin = kv.Key;
                Assert.AreEqual(kv.Key + 1, kv.Value);
            }

            Console.WriteLine("error rate: {0}", error / (double)keys.Length);
            Assert.IsTrue(error / (double)keys.Length <= target.ErrorRate);
        }
    }
}
