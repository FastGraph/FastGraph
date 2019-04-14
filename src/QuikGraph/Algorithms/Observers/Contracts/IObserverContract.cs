using System;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuikGraph.Algorithms.Observers.Contracts
{
#if SUPPORTS_CONTRACTS
    [ContractClassFor(typeof(IObserver<>))]
#endif
    abstract class IObserverContract<TAlgorithm>
        : IObserver<TAlgorithm>
    {
        IDisposable IObserver<TAlgorithm>.Attach(TAlgorithm algorithm)
        { 
#if SUPPORTS_CONTRACTS
            Contract.Requires(algorithm != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);
#endif

            return default(IDisposable);
        }
    }
}
