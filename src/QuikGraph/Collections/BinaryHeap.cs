using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace QuikGraph.Collections
{
    /// <summary>
    /// Binary heap.
    /// </summary>
    /// <remarks>
    /// Indexing rules:
    /// 
    /// parent index: (index - 1)/2
    /// left child: 2 * index + 1
    /// right child: 2 * index + 2
    /// 
    /// Reference:
    /// http://dotnetslackers.com/Community/files/folders/data-structures-and-algorithms/entry28722.aspx
    /// </remarks>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <typeparam name="TPriority">Priority metric type.</typeparam>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class BinaryHeap<TPriority, TValue> : IEnumerable<KeyValuePair<TPriority, TValue>>
    {
        private int _version;

        private const int DefaultCapacity = 16;

        [NotNull]
        private KeyValuePair<TPriority, TValue>[] _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryHeap{TPriority,TValue}"/> class.
        /// </summary>
        public BinaryHeap()
            : this(DefaultCapacity, Comparer<TPriority>.Default.Compare)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryHeap{TPriority,TValue}"/> class.
        /// </summary>
        /// <param name="priorityComparison">Priority comparer.</param>
        public BinaryHeap([NotNull] Comparison<TPriority> priorityComparison)
            : this(DefaultCapacity, priorityComparison)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryHeap{TPriority,TValue}"/> class.
        /// </summary>
        /// <param name="capacity">Heap capacity.</param>
        /// <param name="priorityComparison">Priority comparer.</param>
        public BinaryHeap(int capacity, [NotNull] Comparison<TPriority> priorityComparison)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(capacity >= 0);
            Contract.Requires(priorityComparison != null);
#else
            if (capacity < 0)
                throw new ArgumentException("Capacity must be positive.");
#endif

            _items = new KeyValuePair<TPriority, TValue>[capacity];
            PriorityComparison = priorityComparison ?? throw new ArgumentNullException(nameof(priorityComparison));
        }


#if SUPPORTS_CONTRACTS
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_items != null);
            Contract.Invariant(Count > -1 && Count <= _items.Length);
            Contract.Invariant(
                EnumerableContract.All(0, Count, index =>
                {
                    int left = 2 * index + 1;
                    int right = 2 * index + 2;
                    return (left >= Count || LessOrEqual(index, left))
                           && (right >= Count || LessOrEqual(index, right));
                })
            );
        }
#endif

        /// <summary>
        /// Priority comparer.
        /// </summary>
        [NotNull]
        public Comparison<TPriority> PriorityComparison { get; }

        /// <summary>
        /// Heap capacity.
        /// </summary>
        public int Capacity => _items.Length;

        /// <summary>
        /// Number of element.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Adds the given <paramref name="value"/> (with priority) into the heap.
        /// </summary>
        /// <param name="priority">Item priority.</param>
        /// <param name="value">The value.</param>
        public void Add([NotNull] TPriority priority, [CanBeNull] TValue value)
        {
#if BINARY_HEAP_DEBUG
            Console.WriteLine($"Add({priority}, {value})");
#endif

            ++_version;
            ResizeArray();
            _items[Count++] = new KeyValuePair<TPriority, TValue>(priority, value);
            MinHeapifyUp(Count - 1);

#if BINARY_HEAP_DEBUG
            Console.WriteLine("Add: {0}", ToString2());
#endif
        }

        private void MinHeapifyUp(int start)
        {
#if BINARY_HEAP_DEBUG
            Console.WriteLine("MinHeapifyUp");
#endif
            int current = start;
            int parent = (current - 1) / 2;
            while (current > 0 && Less(current, parent))
            {
                Swap(current, parent);
                current = parent;
                parent = (current - 1) / 2;
            }
        }

        /// <summary>
        /// Gets all heap values.
        /// </summary>
        /// <returns>Array of heap values.</returns>
        [JetBrains.Annotations.Pure]
        [NotNull]
        public TValue[] ToValueArray()
        {
            return _items.Select(pair => pair.Value).ToArray();
        }

        /// <summary>
        /// Gets all values with their priorities.
        /// </summary>
        /// <returns>Array of heap priorities and values.</returns>
        [JetBrains.Annotations.Pure]
        [NotNull]
        public KeyValuePair<TPriority, TValue>[] ToPriorityValueArray()
        {
            return _items.ToArray();
        }

        /// <summary>
        /// Checks if this heap is consistent (fulfill indexing rule).
        /// </summary>
        /// <returns>True if the heap is consistent, false otherwise.</returns>
        public bool IsConsistent()
        {
            int wrong = -1;

            for (int i = 0; i < Count; i++)
            {
                int l = 2 * i + 1;
                int r = 2 * i + 2;
                if (l < Count && !LessOrEqual(i, l))
                    wrong = i;
                if (r < Count && !LessOrEqual(i, r))
                    wrong = i;
            }

            bool correct = wrong == -1;
            return correct;
        }

        [NotNull]
        private string EntryToString(int i)
        {
            if (i < 0 || i >= Count)
                return "null";

            KeyValuePair<TPriority, TValue> kvp = _items[i];
            TPriority k = kvp.Key;
            TValue v = kvp.Value;

            return $"{k.ToString()} {(v == null ? "null" : v.ToString())}";
        }

        private void ResizeArray()
        {
            if (Count == _items.Length)
            {
                var newItems = new KeyValuePair<TPriority, TValue>[Count * 2 + 1];
                Array.Copy(_items, newItems, Count);
                _items = newItems;
            }
        }

        /// <summary>
        /// Gets the minimum pair.
        /// </summary>
        /// <returns>The minimal pair.</returns>
        /// <exception cref="InvalidOperationException">The heap is empty.</exception>
        public KeyValuePair<TPriority, TValue> Minimum()
        {
            if (Count == 0)
                throw new InvalidOperationException("Heap is empty.");
            return _items[0];
        }

        /// <summary>
        /// Gets and removes the minimal pair.
        /// </summary>
        /// <returns>The minimal pair.</returns>
        /// <exception cref="InvalidOperationException">The heap is empty.</exception>
        public KeyValuePair<TPriority, TValue> RemoveMinimum()
        {
#if BINARY_HEAP_DEBUG
            Console.WriteLine("RemoveMinimum");
#endif

            if (Count == 0)
                throw new InvalidOperationException("Heap is empty.");

            // Shortcut for heap with 1 element.
            if (Count == 1)
            {
                ++_version;
                return _items[--Count];
            }

            Swap(0, Count - 1);
            --Count;
            MinHeapifyDown(0);

            return _items[Count];
        }

        private void MinHeapifyDown(int index)
        {
#if BINARY_HEAP_DEBUG
            Console.WriteLine("MinHeapifyDown");
#endif

            while (true)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int smallest = index;
                if (left < Count && Less(left, smallest))
                    smallest = left;
                if (right < Count && Less(right, smallest))
                    smallest = right;

                if (smallest == index)
                    break;

                Swap(smallest, index);
                index = smallest;
            }
        }

        /// <summary>
        /// Gets the index of the given <paramref name="value"/> in the heap.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Index of the value if found, otherwise -1.</returns>
        [JetBrains.Annotations.Pure]
        public int IndexOf([CanBeNull] TValue value)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Equals(value, _items[i].Value))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Updates the <paramref name="value"/> priority if the new priority is lower
        /// than the current <paramref name="value"/> priority (or add it if not present).
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the heap was updated, false otherwise.</returns>
        public bool MinimumUpdate([NotNull] TPriority priority, [NotNull] TValue value)
        {
            // Find index
            int index = IndexOf(value);
            if (index >= 0)
            {
                if (PriorityComparison(priority, _items[index].Key) <= 0)
                {
                    Update(priority, value);
                    return true;
                }

                return false;
            }

            // Not in collection
            Add(priority, value);
            return true;
        }

        /// <summary>
        /// Updates the <paramref name="value"/> priority (or add it if not present).
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="value">The value.</param>
        public void Update([NotNull] TPriority priority, [NotNull] TValue value)
        {
#if BINARY_HEAP_DEBUG
            Console.WriteLine($"Update({priority}, {value})");
#endif

            // Find index
            int index = IndexOf(value);

            // If it exists, update, else add
            if (index > -1)
            {
                TPriority newPriority = priority;
                TPriority oldPriority = _items[index].Key;
                _items[index] = new KeyValuePair<TPriority, TValue>(newPriority, value);

                if (PriorityComparison(newPriority, oldPriority) > 0)
                    MinHeapifyDown(index);
                else if (PriorityComparison(newPriority, oldPriority) < 0)
                    MinHeapifyUp(index);
            }
            else
            {
                Add(priority, value);
            }
        }

        [JetBrains.Annotations.Pure]
        private bool LessOrEqual(int i, int j)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(i >= 0 && i < Count
                              && j >= 0 && j < Count 
                              && i != j);
#endif

            return PriorityComparison(_items[i].Key, _items[j].Key) <= 0;
        }

        [JetBrains.Annotations.Pure]
        private bool Less(int i, int j)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(i >= 0 && i < Count
                              && j >= 0 && j < Count);
#endif

            return PriorityComparison(_items[i].Key, _items[j].Key) < 0;
        }

        private void Swap(int i, int j)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(i >= 0 && i < Count
                              && j >= 0 && j < Count);
#endif

            if (i == j)
                return;

            KeyValuePair<TPriority, TValue> kv = _items[i];
            _items[i] = _items[j];
            _items[j] = kv;
        }

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>>

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TPriority, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        private struct Enumerator : IEnumerator<KeyValuePair<TPriority, TValue>>
        {
            private BinaryHeap<TPriority, TValue> _owner;

            private KeyValuePair<TPriority, TValue>[] _items;

            private readonly int _count;
            private readonly int _version;
            private int _index;

            public Enumerator([NotNull] BinaryHeap<TPriority, TValue> owner)
            {
                _owner = owner;
                _items = owner._items;
                _count = owner.Count;
                _version = owner._version;
                _index = -1;
            }

            public KeyValuePair<TPriority, TValue> Current
            {
                get
                {
                    if (_version != _owner._version)
                        throw new InvalidOperationException();
                    if (_index < 0 || _index == _count)
                        throw new InvalidOperationException();
#if SUPPORTS_CONTRACTS
                    Contract.Assert(_index <= _count);
#endif

                    return _items[_index];
                }
            }

            void IDisposable.Dispose()
            {
                _owner = null;
                _items = null;
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_version != _owner._version)
                    throw new InvalidOperationException();
                return ++_index < _count;
            }

            public void Reset()
            {
                if (_version != _owner._version)
                    throw new InvalidOperationException();
                _index = -1;
            }
        }

        #endregion

        /// <summary>
        /// Gets a string representation of this heap.
        /// </summary>
        /// <returns>String representation.</returns>
        [JetBrains.Annotations.Pure]
        [NotNull]
        public string ToString2()
        {
            bool status = IsConsistent();
            return $"{status}: {string.Join(", ", Enumerable.Range(0, _items.Length).Select(EntryToString).ToArray())}";
        }

        /// <summary>
        /// Gets a string tree representation of this heap.
        /// </summary>
        /// <returns>String representation.</returns>
        [JetBrains.Annotations.Pure]
        [NotNull]
        public string ToStringTree()
        {
            bool status = IsConsistent();
            var str = new StringBuilder($"Consistent? {status}");

            for (int i = 0; i < Count; i++)
            {
                int l = 2 * i + 1;
                int r = 2 * i + 2;

                str.Append(
                    $"{Environment.NewLine}index{i.ToString()} {EntryToString(i)} -> {EntryToString(l)} and {EntryToString(r)}");
            }

            return str.ToString();
        }
    }
}
