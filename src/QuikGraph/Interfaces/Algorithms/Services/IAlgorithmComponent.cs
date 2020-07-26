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
        [NotNull]
        IAlgorithmServices Services { get; }

        /// <summary>
        /// Gets the service with given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <returns>Found service, otherwise null.</returns>
        [Pure]
        [CanBeNull]
        T GetService<T>();

        /// <summary>
        /// Tries to get the service with given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="service">Found service.</param>
        /// <returns>True if the service was found, false otherwise.</returns>
        [Pure]
        [ContractAnnotation("=> true, service:notnull;=> false, service:null")]
        bool TryGetService<T>(out T service);
    }
}