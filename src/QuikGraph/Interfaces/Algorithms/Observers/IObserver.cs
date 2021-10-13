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
        /// Attaches to the algorithm events and returns a <see cref="T:System.IDisposable"/>
        /// object that can be used to detach from the events.
        /// </summary>
        /// <param name="algorithm">Algorithm to observe.</param>
        /// <returns><see cref="T:System.IDisposable"/> allowing to detach from registered events.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="algorithm"/> is <see langword="null"/>.</exception>
        [NotNull]
        IDisposable Attach([NotNull] TAlgorithm algorithm);
    }
}