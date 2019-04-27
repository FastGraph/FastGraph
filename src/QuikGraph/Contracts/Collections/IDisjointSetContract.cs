#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;

namespace QuikGraph.Collections.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IDisjointSet{T}"/>.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    [ContractClassFor(typeof(IDisjointSet<>))]
    internal abstract class DisjointSetContract<T> : IDisjointSet<T>
    {
        int IDisjointSet<T>.SetCount => default(int);

        int IDisjointSet<T>.ElementCount => default(int);

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IDisjointSet<T> explicitThis = this;

            Contract.Invariant(0 <= explicitThis.SetCount);
            Contract.Invariant(explicitThis.SetCount <= explicitThis.ElementCount);
        }

        void IDisjointSet<T>.MakeSet(T value)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IDisjointSet<T> explicitThis = this;

            Contract.Requires(value != null);
            Contract.Requires(!explicitThis.Contains(value));
            Contract.Ensures(explicitThis.Contains(value));
            Contract.Ensures(
                explicitThis.SetCount == Contract.OldValue(explicitThis.SetCount) + 1);
            Contract.Ensures(
                explicitThis.ElementCount == Contract.OldValue(explicitThis.ElementCount) + 1);
        }

        T IDisjointSet<T>.FindSet(T value)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IDisjointSet<T> explicitThis = this;

            Contract.Requires(value != null);
            Contract.Requires(explicitThis.Contains(value));

            return default(T);
        }

        bool IDisjointSet<T>.Union(T left, T right)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IDisjointSet<T> explicitThis = this;

            Contract.Requires(left != null);
            Contract.Requires(explicitThis.Contains(left));
            Contract.Requires(right != null);
            Contract.Requires(explicitThis.Contains(right));

            return default(bool);
        }

        bool IDisjointSet<T>.Contains(T value)
        {
            return default(bool);
        }

        bool IDisjointSet<T>.AreInSameSet(T left, T right)
        {
            // ReSharper disable once RedundantAssignment, Justification: Code contract.
            IDisjointSet<T> explicitThis = this;

            Contract.Requires(left != null);
            Contract.Requires(right != null);
            Contract.Requires(explicitThis.Contains(left));
            Contract.Requires(explicitThis.Contains(right));

            return default(bool);
        }
    }
}
#endif