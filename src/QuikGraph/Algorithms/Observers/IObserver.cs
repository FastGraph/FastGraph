using System;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// An algorithm observer
    /// </summary>
    /// <typeparam name="TAlgorithm">type of the algorithm</typeparam>
    /// <reference-ref id="gof02designpatterns" />
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(Contracts.IObserverContract<>))]
#endif
    public interface IObserver<TAlgorithm>
    {
        /// <summary>
        /// Attaches to the algorithm events
        /// and returns a disposable object that can be used
        /// to detach from the events
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        IDisposable Attach(TAlgorithm algorithm);
    }
}
