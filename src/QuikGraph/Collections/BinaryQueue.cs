#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Collections
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class BinaryQueue<TVertex, TDistance> : 
        IPriorityQueue<TVertex>
    {
        private readonly Func<TVertex, TDistance> distances;
        private readonly BinaryHeap<TDistance, TVertex> heap;

        public BinaryQueue(
            Func<TVertex, TDistance> distances
            )
            : this(distances, Comparer<TDistance>.Default.Compare)
        { }

		public BinaryQueue(
            Func<TVertex, TDistance> distances,
            Comparison<TDistance> distanceComparison
            )
		{
#if SUPPORTS_CONTRACTS
            Contract.Requires(distances != null);
            Contract.Requires(distanceComparison != null);
#endif

			this.distances = distances;
            this.heap = new BinaryHeap<TDistance, TVertex>(distanceComparison);
		}

		public void Update(TVertex v)
		{
            this.heap.Update(this.distances(v), v);
        }

        public int Count
        {
            get { return this.heap.Count; }
        }

        public bool Contains(TVertex value)
        {
            return this.heap.IndexOf(value) > -1;
        }

        public void Enqueue(TVertex value)
        {
            this.heap.Add(this.distances(value), value);
        }

        public TVertex Dequeue()
        {
            return this.heap.RemoveMinimum().Value;
        }

        public TVertex Peek()
        {
            return this.heap.Minimum().Value;
        }

        public TVertex[] ToArray()
        {
            return this.heap.ToValueArray();
        }

        public KeyValuePair<TDistance, TVertex>[] ToArray2()
        {
            return heap.ToPriorityValueArray();
        }

        public string ToString2()
        {
            return heap.ToString2();
        }
    }
}
