using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    /// <summary>This class contains parameterized unit tests for SoftHeap`2</summary>
    [TestFixture]
    internal class SoftHeapTests
    {
        private void Add([NotNull] int[] keys)
        {
            QuikGraphAssert.TrueForAll(keys, k => k < int.MaxValue);
            Assert.IsTrue(keys.Length > 0);

            var target = new SoftHeap<int, int>(1/4.0, int.MaxValue);
            foreach (int key in keys)
            {
                int count = target.Count;
                target.Add(key, key + 1);
                Assert.AreEqual(count + 1, target.Count);
            }

            int lastMin = int.MaxValue;
            int error = 0;
            while (target.Count > 0)
            {
                var kv = target.DeleteMin();
                if (lastMin < kv.Key)
                    ++error;
                lastMin = kv.Key;
                Assert.AreEqual(kv.Key + 1, kv.Value);
            }

            Assert.IsTrue(error / (double)keys.Length <= target.ErrorRate);
        }

        // TODO: Add real tests
    }
}
