#if SUPPORTS_CONTRACTS
using System;
using System.Diagnostics.Contracts;

namespace QuikGraph.Algorithms.Observers.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IObserver{TAlgorithm}"/>.
    /// </summary>
    /// <typeparam name="TAlgorithm">Algorithm type.</typeparam>
    [ContractClassFor(typeof(IObserver<>))]
    internal abstract class ObserverContract<TAlgorithm> : IObserver<TAlgorithm>
    {
        IDisposable IObserver<TAlgorithm>.Attach(TAlgorithm algorithm)
        { 
            Contract.Requires(algorithm != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            // ReSharper disable once AssignNullToNotNullAttribute, Justification: Contract class.
            return default(IDisposable);
        }
    }
}
#endif