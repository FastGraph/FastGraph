using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Services
{
    /// <summary>
    /// Represents algorithm component (services).
    /// </summary>
    public interface IAlgorithmComponent
    {
        /// <summary>
        /// Algorithm common services.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        IAlgorithmServices Services { get; }

        /// <summary>
        /// Gets the service with given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <returns>Found service, otherwise null.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        [CanBeNull]
        T GetService<T>();

        /// <summary>
        /// Tries to get the service with given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="service">Found service.</param>
        /// <returns>True if the service was found, false otherwise.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [Pure]
        bool TryGetService<T>(out T service);
    }
}
