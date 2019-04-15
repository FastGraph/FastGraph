using System;
using JetBrains.Annotations;

namespace QuikGraph.Utils
{
    /// <summary>
    /// Helpers to work with <see cref="IDisposable"/>.
    /// </summary>
    internal static class DisposableHelpers
    {
        /// <summary>
        /// Calls an action when going out of scope.
        /// </summary>
        /// <param name="action">The action to call.</param>
        /// <returns>A <see cref="IDisposable"/> object to give to a using clause.</returns>
        [NotNull]
        public static IDisposable Finally([NotNull] Action action)
        {
            return new FinallyScope(action);
        }

        private class FinallyScope : IDisposable
        {
            [NotNull]
            private readonly Action _action;

            public FinallyScope([NotNull] Action action)
            {
                _action = action ?? throw new ArgumentNullException(nameof(action));
            }

            /// <inheritdoc />
            public void Dispose()
            {
                _action();
            }
        }
    }
}