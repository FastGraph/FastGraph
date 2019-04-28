using System;
using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
using QuikGraph.Algorithms.Contracts;
#endif

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Represents a computation of something with control states.
    /// </summary>
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(ComputationContract))]
#endif
    public interface IComputation
    {
        /// <summary>
        /// Synchronizer object.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        object SyncRoot { get; }

        /// <summary>
        /// Current computation state.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        ComputationState State { get; }

        /// <summary>
        /// Runs the computation.
        /// </summary>
        void Compute();

        /// <summary>
        /// Abort the computation.
        /// </summary>
        void Abort();

        /// <summary>
        /// Fired when the computation state changed.
        /// </summary>
        event EventHandler StateChanged;

        /// <summary>
        /// Fired when the computation start.
        /// </summary>
        event EventHandler Started;

        /// <summary>
        /// Fired when the computation is finished.
        /// </summary>
        event EventHandler Finished;

        /// <summary>
        /// Fired when the computation is aborted.
        /// </summary>
        event EventHandler Aborted;
    }
}