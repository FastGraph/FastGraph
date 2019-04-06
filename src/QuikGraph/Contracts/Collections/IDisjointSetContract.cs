#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Collections.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IDisjointSet<>))]
#endif
    abstract class IDisjointSetContract<T>
        : IDisjointSet<T>
    {
        int IDisjointSet<T>.SetCount
        {
            get
            {
                return default(int);
            }
        }

        int IDisjointSet<T>.ElementCount
        {
            get
            {
                return default(int);
            }
        }

#if SUPPORTS_CONTRACTS
        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            IDisjointSet<T> ithis = this;
            Contract.Invariant(0 <= ithis.SetCount);
            Contract.Invariant(ithis.SetCount <= ithis.ElementCount);
        }
#endif

        void IDisjointSet<T>.MakeSet(T value)
        {
            IDisjointSet<T> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(value != null);
            Contract.Requires(!ithis.Contains(value));
            Contract.Ensures(ithis.Contains(value));
            Contract.Ensures(ithis.SetCount == Contract.OldValue(ithis.SetCount) + 1);
            Contract.Ensures(ithis.ElementCount == Contract.OldValue(ithis.ElementCount) + 1);
#endif
        }

        T IDisjointSet<T>.FindSet(T value)
        {
            IDisjointSet<T> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(value != null);
            Contract.Requires(ithis.Contains(value));
#endif

            return default(T);
        }

        bool IDisjointSet<T>.Union(T left, T right)
        {
            IDisjointSet<T> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(left != null);
            Contract.Requires(ithis.Contains(left));
            Contract.Requires(right != null);
            Contract.Requires(ithis.Contains(right));
#endif

            return default(bool);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        bool IDisjointSet<T>.Contains(T value)
        {
            return default(bool);
        }

        bool IDisjointSet<T>.AreInSameSet(T left, T right)
        {
            IDisjointSet<T> ithis = this;
#if SUPPORTS_CONTRACTS
            Contract.Requires(left != null);
            Contract.Requires(right != null);
            Contract.Requires(ithis.Contains(left));
            Contract.Requires(ithis.Contains(right));
#endif

            return default(bool);
        }
    }
}
