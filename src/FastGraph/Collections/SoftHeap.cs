#nullable enable

using System.Collections;
using System.Diagnostics;

namespace FastGraph.Collections
{
    /// <summary>
    /// Soft heap, which aims to has a constant amortized time for
    /// creation of heap, inserting an element merging two heaps,
    /// deleting an element and finding the element with minimum key.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class SoftHeap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : notnull
    {
        private sealed class Cell
        {
            public TKey Key { get; }

            public TValue? Value { get; }

            public Cell? Next { get; internal set; }

            public Cell(TKey key, TValue? value)
            {
                Key = key;
                Value = value;
            }
        }

        private sealed class Node
        {
            public TKey CKey { get; internal set; }

            public int Rank { get; }

            public Node? Next { get; internal set; }

            public Node? Child { get; internal set; }

            // ReSharper disable once InconsistentNaming
            public Cell? IL { get; internal set; }

            // ReSharper disable once InconsistentNaming
            public Cell? ILTail { get; internal set; }

            public Node(Cell cell)
            {
                Rank = 0;
                CKey = cell.Key;
                IL = cell;
                ILTail = cell;
            }

            public Node(
                TKey cKey,
                int rank,
                Node next,
                Node child,
                Cell? il,
                Cell? ilTail)
            {
                CKey = cKey;
                Rank = rank;
                Next = next;
                Child = child;
                IL = il;
                ILTail = ilTail;
            }
        }

        private sealed class Head
        {
            public Node? Queue { get; internal set; }
            public Head? Next { get; internal set; }
            public Head? Prev { get; internal set; }
            public Head? SuffixMin { get; internal set; }
            public int Rank { get; internal set; }
        }

        private readonly Head _header;

        private readonly Head _tail;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftHeap{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="maximumErrorRate">Indicates the maximum error rate to respect.</param>
        /// <param name="keyMaxValue">Gives the maximum key value.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="keyMaxValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="maximumErrorRate"/> is not in range ]0, 0.5].</exception>
        public SoftHeap(double maximumErrorRate, TKey keyMaxValue)
            : this(maximumErrorRate, keyMaxValue, Comparer<TKey>.Default.Compare)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftHeap{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="maximumErrorRate">Indicates the maximum error rate to respect.</param>
        /// <param name="keyMaxValue">Gives the maximum key value.</param>
        /// <param name="comparison">Key comparer.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="keyMaxValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="comparison"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="maximumErrorRate"/> is not in range ]0, 0.5].</exception>
        public SoftHeap(double maximumErrorRate, TKey keyMaxValue, Comparison<TKey> comparison)
        {
            if (keyMaxValue == null)
                throw new ArgumentNullException(nameof(keyMaxValue));
            if (maximumErrorRate <= 0 || maximumErrorRate > 0.5)
                throw new ArgumentOutOfRangeException(nameof(maximumErrorRate), "Must be between ]0, 0.5]");

            KeyComparison = comparison ?? throw new ArgumentNullException(nameof(comparison));
            KeyMaxValue = keyMaxValue;
            _header = new Head();
            _tail = new Head { Rank = int.MaxValue };
            _header.Next = _tail;
            _tail.Prev = _header;
            ErrorRate = maximumErrorRate;
            MinRank = 2 + 2 * (int)Math.Ceiling(Math.Log(1.0 / ErrorRate, 2.0));
            Count = 0;
        }

        /// <summary>
        /// Minimal rank (based on <see cref="ErrorRate"/>).
        /// </summary>
        public int MinRank { get; }

        /// <summary>
        /// Key comparer.
        /// </summary>
        public Comparison<TKey> KeyComparison { get; }

        /// <summary>
        /// Maximal authorized key.
        /// </summary>
        public TKey KeyMaxValue { get; }

        /// <summary>
        /// Error rate.
        /// </summary>
        public double ErrorRate { get; }

        /// <summary>
        /// Number of element.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Adds the given <paramref name="value"/> with the given <paramref name="key"/> into the heap.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value to add.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="key"/> is superior to <see cref="KeyMaxValue"/>.</exception>
        public void Add(TKey key, TValue? value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (KeyComparison(key, KeyMaxValue) >= 0)
                throw new ArgumentException("Key is superior to the maximal authorized key.", nameof(key));

            var cell = new Cell(key, value);
            var node = new Node(cell);

            Meld(node);
            ++Count;
        }

        private void Meld(Node node)
        {
            Head toHead = _header.Next!;
            while (node.Rank > toHead.Rank)
            {
                Debug.Assert(toHead.Next != default);

                toHead = toHead.Next!;
            }

            Head prevHead = toHead.Prev!;
            while (node.Rank == toHead.Rank)
            {
                Node top, bottom;
                if (KeyComparison(toHead.Queue!.CKey, node.CKey) > 0)
                {
                    top = node;
                    bottom = toHead.Queue;
                }
                else
                {
                    top = toHead.Queue;
                    bottom = node;
                }

                node = new Node(top.CKey, top.Rank + 1, bottom, top, top.IL, top.ILTail);
                toHead = toHead.Next!;
            }

            Head head = prevHead == toHead.Prev
                ? new Head()
                : prevHead.Next!;

            head.Queue = node;
            head.Rank = node.Rank;
            head.Prev = prevHead;
            head.Next = toHead;
            prevHead.Next = head;
            toHead.Prev = head;

            FixMinList(head);
        }

        private void FixMinList(Head head)
        {
            Head tmpMin = head.Next == _tail
                ? head
                : head.Next!.SuffixMin!;

            while (head != _header)
            {
                if (KeyComparison(tmpMin.Queue!.CKey, head.Queue!.CKey) > 0)
                {
                    tmpMin = head;
                }

                head.SuffixMin = tmpMin;
                head = head.Prev!;
            }
        }

        private Node Shift(Node v)
        {
            v.IL = default;
            v.ILTail = default;
            if (v.Next is null && v.Child is null)
            {
                v.CKey = KeyMaxValue;
                return v;
            }

            v.Next = Shift(v.Next!);
            // Restore heap ordering that might be broken by shifting
            if (KeyComparison(v.Next.CKey, v.Child!.CKey) > 0)
            {
                Node tmp = v.Child;
                v.Child = v.Next;
                v.Next = tmp;
            }

            v.IL = v.Next.IL;
            v.ILTail = v.Next.ILTail;
            v.CKey = v.Next.CKey;

            // Softening the heap
            SoftenHeap(v);

            UpdateChildAndNext(v);

            return v;
        }

        private void SoftenHeap(Node node)
        {
            if (node.Rank > MinRank
                && (node.Rank % 2 == 1 || node.Child!.Rank < node.Rank - 1))
            {
                Debug.Assert(node.Next != default);

                node.Next = Shift(node.Next!);
                // Restore heap ordering that might be broken by shifting
                if (KeyComparison(node.Next.CKey, node.Child!.CKey) > 0)
                {
                    Node tmp = node.Child;
                    node.Child = node.Next;
                    node.Next = tmp;
                }

                if (KeyComparison(node.Next.CKey, KeyMaxValue) != 0 && node.Next.IL != default)
                {
                    node.Next.ILTail!.Next = node.IL;
                    node.IL = node.Next.IL;
                    if (node.ILTail == default)
                    {
                        node.ILTail = node.Next.ILTail;
                    }
                    node.CKey = node.Next.CKey;
                }
            } // End second shift
        }

        private void UpdateChildAndNext(Node node)
        {
            Debug.Assert(node.Child != default);
            if (KeyComparison(node.Child!.CKey, KeyMaxValue) == 0)
            {
                Debug.Assert(node.Next != default);
                if (KeyComparison(node.Next!.CKey, KeyMaxValue) == 0)
                {
                    node.Child = default;
                    node.Next = default;
                }
                else
                {
                    node.Child = node.Next.Child;
                    node.Next = node.Next.Next;
                }
            }
        }

        /// <summary>
        /// Gets and removes the minimal pair.
        /// </summary>
        /// <returns>The minimal pair.</returns>
        /// <exception cref="T:System.InvalidOperationException">The heap is empty.</exception>
        public KeyValuePair<TKey, TValue> RemoveMinimum()
        {
            if (Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            Head head = _header.Next!.SuffixMin!;
            while (head.Queue!.IL is null)
            {
                Node tmp = head.Queue;
                int childCount = 0;
                while (tmp.Next != default)
                {
                    tmp = tmp.Next;
                    ++childCount;
                }

                if (childCount < head.Rank / 2)
                {
                    head.Prev!.Next = head.Next;
                    head.Next!.Prev = head.Prev;
                    FixMinList(head.Prev);
                    tmp = head.Queue;
                    while (tmp.Next != default)
                    {
                        Meld(tmp.Child!);
                        tmp = tmp.Next;
                    }
                }
                else
                {
                    head.Queue = Shift(head.Queue);
                    if (KeyComparison(head.Queue.CKey, KeyMaxValue) == 0)
                    {
                        head.Prev!.Next = head.Next;
                        head.Next!.Prev = head.Prev;
                        head = head.Prev;
                    }

                    FixMinList(head);
                }

                head = _header.Next.SuffixMin!;
            } // End of outer while loop

            TKey min = head.Queue.IL.Key;
            TValue value = head.Queue.IL.Value!;
            head.Queue.IL = head.Queue.IL.Next;
            if (head.Queue.IL is null)
            {
                head.Queue.ILTail = default;
            }

            --Count;
            return new KeyValuePair<TKey, TValue>(min, value);
        }

        #region IEnumerable

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>>

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator();
        }

        private sealed class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            public Enumerator()
            {
                Current = new KeyValuePair<TKey, TValue>();
            }

            public bool MoveNext()
            {
                // Currently it is not possible to enumerate a soft heap
                return false;
            }

            public KeyValuePair<TKey, TValue> Current { get; }

            public void Dispose()
            {
                // Currently the enumerator does nothing
            }

            object IEnumerator.Current => Current;

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        #endregion
    }
}
