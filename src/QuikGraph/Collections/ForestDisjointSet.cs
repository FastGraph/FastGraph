using System.Collections.Generic;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Collections
{
    /// <summary>
    /// Disjoint-set implementation with path compression and union-by-rank optimizations.
    /// optimization
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ForestDisjointSet<T>
        : IDisjointSet<T>
    {
#if DEBUG
        [DebuggerDisplay("{ID}:{Rank}->{Parent}")]
#endif
        class Element
        {
#if DEBUG
            public readonly int ID;
            static int nextID;
#endif
            public Element Parent;
            public int Rank;
            public readonly T Value;
            public Element(T value)
            {
#if DEBUG
                this.ID = nextID++;
#endif
                this.Parent = null;
                this.Rank = 0;
                this.Value = value;
            }
        }

        readonly Dictionary<T, Element> elements;
        int setCount;

        public ForestDisjointSet(int elementCapacity)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(elementCapacity >= 0 && elementCapacity < int.MaxValue);
#endif

            this.elements = new Dictionary<T, Element>(elementCapacity);
            this.setCount = 0;
        }

        public ForestDisjointSet()
        {
            this.elements = new Dictionary<T, Element>();
            this.setCount = 0;
        }

        public int SetCount
        {
            get { return this.setCount; }
        }

        public int ElementCount
        {
            get { return this.elements.Count; }
        }

        /// <summary>
        /// Adds a new set
        /// </summary>
        /// <param name="value"></param>
        public void MakeSet(T value)
        {
            var element = new Element(value);
            this.elements.Add(value, element);
            this.setCount++;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool Contains(T value)
        {
            return this.elements.ContainsKey(value);
        }

        public bool Union(T left, T right)
        {
            return this.Union(this.elements[left], this.elements[right]);
        }

        public T FindSet(T value)
        {
            return this.Find(this.elements[value]).Value;
        }

        public bool AreInSameSet(T left, T right)
        {
            return this.FindSet(left).Equals(this.FindSet(right));
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        private Element FindNoCompression(Element element)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(element != null);
            Contract.Ensures(Contract.Result<Element>() != null);
#endif

            // find root,
            var current = element;
            while (current.Parent != null)
                current = current.Parent;

            return current;
        }

        /// <summary>
        /// Finds the parent element, and applies path compression
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private Element Find(Element element)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(element != null);
            Contract.Ensures(Contract.Result<Element>() != null);
#endif

            var root = this.FindNoCompression(element);            
            CompressPath(element, root);
            return root;
        }

        private static void CompressPath(Element element, Element root)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(element != null);
            Contract.Requires(root != null);
#endif

            // path compression
            var current = element;
            while (current != root)
            {
                var temp = current;
                current = current.Parent;
                temp.Parent = root;
            }
        }

        private bool Union(Element left, Element right)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(left != null);
            Contract.Requires(right != null);
            Contract.Ensures(
                Contract.Result<bool>() 
                ? Contract.OldValue(this.SetCount) - 1 == this.SetCount             
                : Contract.OldValue(this.SetCount) == this.SetCount);
            Contract.Ensures(this.FindNoCompression(left) == this.FindNoCompression(right));
#endif

            // shortcut when already unioned,
            if (left == right) return false;

            var leftRoot = Find(left);
            var rightRoot = Find(right);

            // union by rank
            if (leftRoot.Rank > rightRoot.Rank)
                rightRoot.Parent = leftRoot;
            else if (leftRoot.Rank < rightRoot.Rank)
                leftRoot.Parent = rightRoot;
            else if (leftRoot != rightRoot)
            {
                rightRoot.Parent = leftRoot;
                leftRoot.Rank = leftRoot.Rank + 1;
            }
            else
                return false; // do not update the setcount

            this.setCount--;
            return true;
        }

#if SUPPORTS_CONTRACTS
        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            Contract.Invariant(this.setCount >= 0);
            Contract.Invariant(this.setCount <= this.elements.Count);
        }
#endif
    }
}
