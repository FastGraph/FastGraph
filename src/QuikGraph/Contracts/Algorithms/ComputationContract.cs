#if SUPPORTS_CONTRACTS
using System;
using System.Diagnostics.Contracts;

namespace QuikGraph.Algorithms.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IComputation"/>.
    /// </summary>
    [ContractClassFor(typeof(IComputation))]
    internal abstract class ComputationContract : IComputation
    {
        #region IComputation

        object IComputation.SyncRoot
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);

                // ReSharper disable once AssignNullToNotNullAttribute, Justification: Contract class.
                return null;
            }
        }

        ComputationState IComputation.State
        {
            get
            {
                Contract.Ensures(Enum.IsDefined(typeof(ComputationState), Contract.Result<ComputationState>()));

                return default(ComputationState);
            }
        }

        void IComputation.Compute()
        {
            // TODO contracts on events
            throw new NotImplementedException();
        }

        void IComputation.Abort()
        {
            throw new NotImplementedException();
        }

        event EventHandler IComputation.StateChanged
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        event EventHandler IComputation.Started
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        event EventHandler IComputation.Finished
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        event EventHandler IComputation.Aborted
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        #endregion
    }
}
#endif