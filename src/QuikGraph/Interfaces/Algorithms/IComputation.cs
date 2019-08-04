using System;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Represents a computation of something with control states.
    /// </summary>
    public interface IComputation
    {
        /// <summary>
        /// Synchronizer object.
        /// </summary>
        [NotNull]
        object SyncRoot { get; }

        /// <summary>
        /// Current computation state.
        /// </summary>
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