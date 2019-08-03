
using System;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Services
{
    /// <summary>
    /// Default algorithm services implementation.
    /// </summary>
    internal class AlgorithmServices : IAlgorithmServices
    {
        [NotNull]
        private readonly IAlgorithmComponent _host;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlgorithmServices"/> class.
        /// </summary>
        /// <param name="host">Algorithm host.</param>
        public AlgorithmServices([NotNull] IAlgorithmComponent host)
        {
            if (host is null)
                throw new ArgumentNullException(nameof(host));

            _host = host;
        }

        private ICancelManager _cancelManager;

        /// <inheritdoc />
        public ICancelManager CancelManager => 
            (_cancelManager ?? (_cancelManager = _host.GetService<ICancelManager>())) ?? throw new InvalidOperationException("No cancel manager service registered.");
    }
}
