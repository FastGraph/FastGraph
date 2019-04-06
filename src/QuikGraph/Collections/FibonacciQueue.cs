using System;
using System.Collections.Generic;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Linq;
using QuickGraph.Algorithms;

namespace QuickGraph.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    public sealed class FibonacciQueue<TVertex, TDistance> :
        IPriorityQueue<TVertex>
    {
        public FibonacciQueue(Func<TVertex, TDistance> distances)
            : this(0, null, distances, Comparer<TDistance>.Default.Compare)
        { }

        public FibonacciQueue(
            int valueCount,
            IEnumerable<TVertex> values,
            Func<TVertex, TDistance> distances
            )
            : this(valueCount, values, distances, Comparer<TDistance>.Default.Compare)
        { }

        public FibonacciQueue(
            int valueCount,
            IEnumerable<TVertex> values,
            Func<TVertex, TDistance> distances,
            Comparison<TDistance> distanceComparison
            )
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(valueCount >= 0);
            Contract.Requires(valueCount == 0 || (values != null && valueCount == Enumerable.Count(values)));
            Contract.Requires(distances != null);
            Contract.Requires(distanceComparison != null);
#endif

            this.distances = distances;
            this.cells = new Dictionary<TVertex, FibonacciHeapCell<TDistance, TVertex>>(valueCount);
            if (valueCount > 0)
                foreach (var x in values)
                    this.cells.Add(
                        x,
                        new FibonacciHeapCell<TDistance, TVertex>
                        {
                            Priority = this.distances(x),
                            Value = x,
                            Removed = true
                        }
                    );
            this.heap = new FibonacciHeap<TDistance, TVertex>(HeapDirection.Increasing, distanceComparison);
        }

        public FibonacciQueue(
            Dictionary<TVertex, TDistance> values,
            Comparison<TDistance> distanceComparison
            )
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(values != null);
            Contract.Requires(distanceComparison != null);
#endif

            this.distances = AlgorithmExtensions.GetIndexer(values);
            this.cells = new Dictionary<TVertex, FibonacciHeapCell<TDistance, TVertex>>(values.Count);
            foreach (var kv in values)
                this.cells.Add(
                    kv.Key,
                    new FibonacciHeapCell<TDistance, TVertex>
                    {
                        Priority = kv.Value,
                        Value = kv.Key,
                        Removed = true
                    }
                );
            this.heap = new FibonacciHeap<TDistance, TVertex>(HeapDirection.Increasing, distanceComparison);
        }

        public FibonacciQueue(
            Dictionary<TVertex, TDistance> values)
            : this(values, Comparer<TDistance>.Default.Compare)
        { }

        private readonly FibonacciHeap<TDistance, TVertex> heap;
        private readonly Dictionary<TVertex, FibonacciHeapCell<TDistance, TVertex>> cells;        
        private readonly Func<TVertex, TDistance> distances;
#region IQueue<TVertex> Members

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public int Count => heap.Count;

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool Contains(TVertex value)
        {
            FibonacciHeapCell<TDistance, TVertex> result;
            return 
                this.cells.TryGetValue(value, out result) && 
                !result.Removed;
        }

        public void Update(TVertex v)
        {
            this.heap.ChangeKey(this.cells[v], this.distances(v));
        }

        public void Enqueue(TVertex value)
        {
            this.cells[value] = this.heap.Enqueue(this.distances(value), value);
        }

        public TVertex Dequeue()
        {
            var result = heap.Top;
#if SUPPORTS_CONTRACTS
            Contract.Assert(result != null);
#endif

            heap.Dequeue();            
            return result.Value;
        }

        public TVertex Peek()
        {
#if SUPPORTS_CONTRACTS
            Contract.Assert(this.heap.Top != null);
#endif

            return this.heap.Top.Value;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public TVertex[] ToArray()
        {
            TVertex[] result = new TVertex[this.heap.Count];
            int i = 0;
            foreach (var entry in this.heap)
                result[i++] = entry.Value;
            return result;
        }

#endregion
    }
}
