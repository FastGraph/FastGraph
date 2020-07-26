using System;

namespace QuikGraph.Algorithms.Services
{
    /// <summary>
    /// Represents an algorithm cancel manager.
    /// </summary>
    public interface ICancelManager
    {
        /// <summary>
        /// Fired when the cancel method is called.
        /// </summary>
        event EventHandler CancelRequested;

        /// <summary>
        /// Requests the component to cancel its computation.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Gets a value indicating if a cancellation request is pending.
        /// </summary>
        bool IsCancelling { get; }

        /// <summary>
        /// Fired when the cancel state has been resetting.
        /// </summary>
        event EventHandler CancelReset;

        /// <summary>
        /// Resets the cancel state.
        /// </summary>
        void ResetCancel();
    }
}