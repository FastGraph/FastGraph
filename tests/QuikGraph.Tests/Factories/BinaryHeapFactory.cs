using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Collections;

namespace QuikGraph.Tests.Collections
{
    /// <summary>
    /// Factory to create <see cref="BinaryHeap{TPriority,TValue}"/>.
    /// </summary>
    internal static class BinaryHeapFactory
    {
        [Pure]
        [NotNull]
        public static BinaryHeap<int, int> Create(int capacity)
        {
            return new BinaryHeap<int, int>(capacity, (i, j) => i.CompareTo(j));
        }

        public static void AssertInvariants<TPriority, TValue>([NotNull] this BinaryHeap<TPriority, TValue> heap)
        {
            Assert.IsTrue(heap.Capacity >= 0, "Capacity test failed");
            Assert.IsTrue(heap.Count >= 0, "Count test failed.");
            Assert.IsTrue(heap.Count <= heap.Capacity, "Count and capacity comparison failed.");
            Assert.IsTrue(heap.IsConsistent(), "IsConsistent test failed.");
        }

        public static BinaryHeap<int, int> ExampleHeap01()
        {
            var heap = Create(20);
            heap.Add(1, 0);
            heap.Add(2, 1);
            heap.Add(1, 2);
            heap.Add(2, 3);
            heap.Add(2, 4);
            heap.Add(1, 5);
            heap.Add(1, 6);
            heap.Add(2, 7);
            heap.Add(2, 8);
            heap.Add(2, 9);
            heap.Add(2, 10);
            heap.Add(1, 11);
            heap.Add(1, 12);
            heap.Add(1, 13);
            heap.Add(1, 14);
            const string str = "True: 1 0, 2 1, 1 2, 2 3, 2 4, 1 5, 1 6, 2 7, 2 8, 2 9, 2 10, 1 11, 1 12, 1 13, 1 14, null, null, null, null, null";
            Assert.AreEqual(str, heap.ToString2());
            Assert.AreEqual(15, heap.Count);
            heap.AssertInvariants();

            return heap;
        }

        public static BinaryHeap<int, int> ExampleHeapFromTopologicalSortOfDCT8()
        {
            var heap = BinaryHeapFactory.Create(20);
            heap.Add(0, 255);
            heap.Add(0, 256);
            heap.Add(0, 257);
            heap.Add(0, 258);
            heap.Add(0, 259);
            heap.Add(0, 260);
            heap.Add(0, 261);
            heap.Add(0, 262);
            heap.Add(2, 263);
            heap.Add(2, 264);
            heap.Add(2, 265);
            heap.Add(2, 266);
            heap.Add(2, 267);
            heap.Add(2, 268);
            heap.Add(2, 269);
            heap.Add(2, 270);
            heap.Add(1, 271);
            heap.Add(1, 272);
            heap.Add(1, 273);
            heap.Add(1, 274);
            heap.Add(1, 275);
            heap.Add(1, 276);
            heap.Add(1, 277);
            heap.Add(1, 278);
            heap.Add(2, 279);
            heap.Add(2, 280);
            heap.Add(1, 281);
            heap.Add(1, 282);
            heap.Add(1, 283);
            heap.Add(1, 284);
            heap.Add(2, 285);
            heap.Add(2, 286);
            heap.Add(2, 287);
            heap.Add(2, 288);
            heap.Add(1, 289);
            heap.Add(1, 290);
            heap.Add(1, 291);
            heap.Add(1, 292);
            heap.Add(1, 293);
            heap.Add(1, 294);
            heap.Add(1, 295);
            heap.Add(1, 296);
            heap.Add(1, 297);
            heap.Add(1, 298);
            heap.Add(1, 299);
            heap.Add(2, 300);
            heap.Add(2, 301);
            heap.Add(2, 302);
            heap.Add(2, 303);
            heap.Add(1, 304);
            heap.Add(1, 305);
            heap.Add(1, 306);
            heap.Add(1, 307);
            heap.Add(2, 308);
            heap.Add(2, 309);
            heap.Add(2, 310);
            heap.Add(1, 311);
            heap.Add(2, 312);
            heap.Add(2, 313);
            heap.Add(2, 314);
            heap.Add(1, 315);
            heap.Add(1, 316);
            heap.Add(1, 317);
            heap.Add(1, 318);
            heap.Add(2, 319);
            heap.Add(2, 320);
            heap.Add(2, 321);
            heap.Add(2, 322);
            heap.Add(2, 323);
            heap.Add(2, 324);
            heap.Add(1, 325);
            heap.Add(2, 326);
            heap.Add(2, 327);
            heap.Add(2, 328);
            heap.Add(2, 329);
            heap.Add(1, 330);
            heap.Add(1, 331);
            heap.Add(1, 332);
            heap.Add(1, 333);
            heap.Add(0, 334);
            heap.Add(0, 335);
            heap.Add(0, 336);
            heap.Add(0, 337);
            heap.Add(0, 338);
            heap.AssertInvariants();
            return heap;
        }
    }
}