#nullable enable

#if SUPPORTS_SERIALIZATION
#endif
using System.Collections;

namespace FastGraph.Collections
{
    /// <summary>
    /// Represents a list of <see cref="FibonacciHeapCell{TPriority,TValue}"/>.
    /// </summary>
    /// <typeparam name="TPriority">Priority type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class FibonacciHeapLinkedList<TPriority, TValue> : IEnumerable<FibonacciHeapCell<TPriority, TValue>> where TPriority : notnull
    {
        private FibonacciHeapCell<TPriority, TValue>? _last;

        /// <summary>
        /// First <see cref="FibonacciHeapCell{TPriority,TValue}"/>.
        /// </summary>
        public FibonacciHeapCell<TPriority, TValue>? First { get; private set; }

        internal FibonacciHeapLinkedList()
        {
            First = default;
            _last = default;
        }

        /// <summary>
        /// Merges the given <paramref name="cells"/> at the end of this cells list.
        /// </summary>
        /// <param name="cells">Cells to merge.</param>
        internal void MergeLists(FibonacciHeapLinkedList<TPriority, TValue> cells)
        {
            if (cells.First is null)
                return;

            if (_last != default)
            {
                _last.Next = cells.First;
            }

            cells.First.Previous = _last;
            _last = cells._last;

            if (First is null)
            {
                First = cells.First;
            }
        }

        /// <summary>
        /// Adds the given <paramref name="cell"/> at the end of this cells list.
        /// </summary>
        /// <param name="cell">Cell to add.</param>
        internal void AddLast(FibonacciHeapCell<TPriority, TValue> cell)
        {
            if (_last != default)
            {
                _last.Next = cell;
            }

            cell.Previous = _last;
            _last = cell;

            if (First is null)
            {
                First = cell;
            }
        }

        /// <summary>
        /// Removes the given <paramref name="cell"/> from this cells list.
        /// </summary>
        /// <param name="cell">Cell to remove.</param>
        internal void Remove(FibonacciHeapCell<TPriority, TValue> cell)
        {
            if (cell.Previous != default)
            {
                cell.Previous.Next = cell.Next;
            }
            else if (First == cell)
            {
                First = cell.Next;
            }

            if (cell.Next != default)
            {
                cell.Next.Previous = cell.Previous;
            }
            else if (_last == cell)
            {
                _last = cell.Previous;
            }

            cell.Next = default;
            cell.Previous = default;
        }

        #region IEnumerable

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<FibonacciHeapNode<TPriority,TValue>>

        /// <inheritdoc />
        public IEnumerator<FibonacciHeapCell<TPriority, TValue>> GetEnumerator()
        {
            FibonacciHeapCell<TPriority, TValue>? current = First;
            while (current != default)
            {
                yield return current;
                current = current.Next;
            }
        }

        #endregion
    }
}
