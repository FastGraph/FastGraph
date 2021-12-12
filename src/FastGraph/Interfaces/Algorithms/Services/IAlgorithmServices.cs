#nullable enable

namespace FastGraph.Algorithms.Services
{
    /// <summary>
    /// Represents common services available to algorithm instances.
    /// </summary>
    public interface IAlgorithmServices
    {
        /// <summary>
        /// Algorithm cancel manager.
        /// </summary>
        ICancelManager CancelManager { get; }
    }
}
