#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Algorithms
{
#if SUPPORTS_CONTRACTS
    [ContractClass(typeof(Contracts.IAlgorithmContract<>))]
#endif
    public interface IAlgorithm<TGraph> :
        IComputation
    {
        TGraph VisitedGraph { get;}
    }
}
