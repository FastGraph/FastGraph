using System;
using System.Threading;

namespace QuikGraph.Algorithms.Services
{
    /// <summary>
    /// Default algorithm cancel manager implementation.
    /// </summary>
    internal class CancelManager : ICancelManager
    {
        /// <inheritdoc />
        public event EventHandler CancelRequested;

        /// <inheritdoc />
        public void Cancel()
        {
            int value = Interlocked.Increment(ref _cancelling);
            if (value == 0)
                CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        private int _cancelling;

        /// <inheritdoc />
        public bool IsCancelling => _cancelling > 0;

        /// <inheritdoc />
        public event EventHandler CancelReset;

        /// <inheritdoc />
        public void ResetCancel()
        {
            int value = Interlocked.Exchange(ref _cancelling, 0);
            if (value != 0)
                CancelReset?.Invoke(this, EventArgs.Empty);
        }
    }
}