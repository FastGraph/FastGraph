﻿using System;
using JetBrains.Annotations;

namespace FastGraph.Algorithms.Services
{
    /// <summary>
    /// Default algorithm services implementation.
    /// </summary>
    internal sealed class AlgorithmServices : IAlgorithmServices
    {
        [NotNull]
        private readonly IAlgorithmComponent _host;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlgorithmServices"/> class.
        /// </summary>
        /// <param name="host">Algorithm host.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="host"/> is <see langword="null"/>.</exception>
        public AlgorithmServices([NotNull] IAlgorithmComponent host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }

        private ICancelManager _cancelManager;

        /// <inheritdoc />
        public ICancelManager CancelManager => 
            (_cancelManager ?? (_cancelManager = _host.GetService<ICancelManager>())) ?? throw new InvalidOperationException("No cancel manager service registered.");
    }
}