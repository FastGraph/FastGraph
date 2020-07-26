using System;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// Represents an algorithm observer.
    /// </summary>
    /// <typeparam name="TAlgorithm">Algorithm type.</typeparam>
    /// <reference-ref id="gof02designpatterns" />
    public interface IObserver<in TAlgorithm>
    {
        /// <summary>
        /// Attaches to the algorithm events and returns a <see cref="IDisposable"/>
        /// object that can be used to detach from the events.
        /// </summary>
        /// <param name="algorithm">Algorithm to observe.</param>
        /// <returns><see cref="IDisposable"/> allowing to detach from registered events.</returns>
        [NotNull]
        IDisposable Attach([NotNull] TAlgorithm algorithm);
    }
}