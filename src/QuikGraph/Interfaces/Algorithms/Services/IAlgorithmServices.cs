using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Services
{
    /// <summary>
    /// Represents common services available to algorithm instances.
    /// </summary>
    public interface IAlgorithmServices
    {
        /// <summary>
        /// Algorithm cancel manager.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        ICancelManager CancelManager { get; }
    }
}
