using System;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Algorithms.Observers
{
    struct DisposableAction
        : IDisposable
    {
        public delegate void Action();

        Action action;

        public DisposableAction(Action action)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(action != null);
#endif

            this.action = action;
        }

        public void Dispose()
        {
            var a = this.action;
            this.action = null;
            if (a != null)
                a();
        }
    }
}
