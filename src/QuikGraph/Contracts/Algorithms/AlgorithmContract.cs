#if SUPPORTS_CONTRACTS
using System;
using System.Diagnostics.Contracts;

namespace QuikGraph.Algorithms.Contracts
{
    /// <summary>
    /// Contract class for <see cref="IAlgorithm{TGraph}"/>.
    /// </summary>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    [ContractClassFor(typeof(IAlgorithm<>))]
    internal abstract class AlgorithmContract<TGraph> : IAlgorithm<TGraph>
    {
        #region IAlgorithm<TGraph>

        TGraph IAlgorithm<TGraph>.VisitedGraph
        {
            get
            {
                Contract.Ensures(Contract.Result<TGraph>() != null);

                // ReSharper disable once AssignNullToNotNullAttribute, Justification: Contract class.
                return default(TGraph);
            }
        }

        #endregion

        #region IComputation

        object IComputation.SyncRoot => throw new NotImplementedException();

        ComputationState IComputation.State => throw new NotImplementedException();

        void IComputation.Compute()
        {
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