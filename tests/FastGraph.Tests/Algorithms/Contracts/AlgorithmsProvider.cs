#nullable enable

using JetBrains.Annotations;
using FastGraph.Algorithms;

namespace FastGraph.Tests.Algorithms.Contracts
{
    /// <summary>
    /// Provider of algorithm <see cref="Type"/> implementing contracts.
    /// </summary>
    internal static class AlgorithmsProvider
    {
        /// <summary>
        /// Gets all implementations of the given <typeparamref name="T"/> type.
        /// </summary>
        [Pure]
        private static IEnumerable<Type> GetImplementationsOf<T>()
        {
            return GetImplementationsOf(typeof(T));
        }

        /// <summary>
        /// Gets all implementations of the given <paramref name="targetType"/>.
        /// </summary>
        [Pure]
        private static IEnumerable<Type> GetImplementationsOf(Type targetType)
        {
            IEnumerable<Type> implementations = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsInterface && !type.IsAbstract)
                .SelectMany(
                    type => type.GetInterfaces(),
                    (type, inheritedOrImplementedInterface) => new
                    {
                        Type = type,
                        InheritedOrImplementedInterface = inheritedOrImplementedInterface
                    })
                .Where(pair =>
                    targetType.IsAssignableFrom(pair.InheritedOrImplementedInterface)
                    ||
                    pair.InheritedOrImplementedInterface.IsGenericType
                    && targetType.IsAssignableFrom(pair.InheritedOrImplementedInterface.GetGenericTypeDefinition()))
                .Select(pair => pair.Type);

            return implementations;
        }

        /// <summary>
        /// Gets all implementations of the <see cref="IVertexColorizerAlgorithm{TVertex}"/> interface.
        /// </summary>
        public static IEnumerable<Type> VertexColorizers => GetImplementationsOf(typeof(IVertexColorizerAlgorithm<>));

        /// <summary>
        /// Gets all implementations of the <see cref="IDistancesCollection{TVertex}"/> interface.
        /// </summary>
        public static IEnumerable<Type> DistanceCollectors => GetImplementationsOf(typeof(IDistancesCollection<>));
    }
}
