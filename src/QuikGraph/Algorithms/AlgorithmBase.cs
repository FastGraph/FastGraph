using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Base class for all graph algorithm.
    /// </summary>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    public abstract class AlgorithmBase<TGraph> : IAlgorithm<TGraph>, IAlgorithmComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlgorithmBase{TGraph}"/> class (with optional host).
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        protected AlgorithmBase([CanBeNull] IAlgorithmComponent host, [NotNull] TGraph visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(visitedGraph != null);
#endif

            if (host is null)
                host = this;
            VisitedGraph = visitedGraph;
            _algorithmServices = new AlgorithmServices(host);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlgorithmBase{TGraph}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        protected AlgorithmBase([NotNull] TGraph visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(visitedGraph != null);
#endif

            VisitedGraph = visitedGraph;
            _algorithmServices = new AlgorithmServices(this);
        }

        #region IComputation

        /// <inheritdoc />
        public object SyncRoot { get; } = new object();

        private volatile ComputationState _state = ComputationState.NotRunning;

        /// <inheritdoc />
        public ComputationState State
        {
            get
            {
                lock (SyncRoot)
                {
                    return _state;
                }
            }
        }

        /// <inheritdoc />
        public void Compute()
        {
            BeginComputation();

            Initialize();

            try
            {
                InternalCompute();
            }
            finally
            {
                Clean();
            }

            EndComputation();
        }

        /// <inheritdoc />
        public void Abort()
        {
            bool raise = false;
            lock (SyncRoot)
            {
                if (_state == ComputationState.Running)
                {
                    _state = ComputationState.PendingAbortion;
                    Services.CancelManager.Cancel();
                    raise = true;
                }
            }

            if (raise)
                OnStateChanged(EventArgs.Empty);
        }

        /// <inheritdoc />
        public event EventHandler StateChanged;

        /// <summary>
        /// Called on algorithm state changed.
        /// </summary>
        /// <param name="args"><see cref="EventArgs.Empty"/>.</param>
        protected virtual void OnStateChanged([NotNull] EventArgs args)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(args != null);
#endif

            StateChanged?.Invoke(this, args);
        }

        /// <inheritdoc />
        public event EventHandler Started;

        /// <summary>
        /// Called on algorithm start.
        /// </summary>
        /// <param name="args"><see cref="EventArgs.Empty"/>.</param>
        protected virtual void OnStarted([NotNull] EventArgs args)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(args != null);
#endif

            Started?.Invoke(this, args);
        }

        /// <inheritdoc />
        public event EventHandler Finished;

        /// <summary>
        /// Called on algorithm finished.
        /// </summary>
        /// <param name="args"><see cref="EventArgs.Empty"/>.</param>
        protected virtual void OnFinished([NotNull] EventArgs args)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(args != null);
#endif

            Finished?.Invoke(this, args);
        }

        /// <inheritdoc />
        public event EventHandler Aborted;

        /// <summary>
        /// Called on algorithm abort.
        /// </summary>
        /// <param name="args"><see cref="EventArgs.Empty"/>.</param>
        protected virtual void OnAborted([NotNull] EventArgs args)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(args != null);
#endif

            Aborted?.Invoke(this, args);
        }

        #endregion

        #region IAlgorithm<TGraph>

        /// <inheritdoc />
        public TGraph VisitedGraph { get; }

        #endregion

        #region IAlgorithmComponent

        [NotNull]
        private readonly AlgorithmServices _algorithmServices;

        /// <inheritdoc />
        public IAlgorithmServices Services => _algorithmServices;

        /// <inheritdoc />
        public T GetService<T>()
        {
            if (!TryGetService(out T service))
                throw new InvalidOperationException("Service not found.");
            return service;
        }

        /// <inheritdoc />
        public bool TryGetService<T>(out T service)
        {
            if (TryGetService(typeof(T), out object serviceObject))
            {
                service = (T)serviceObject;
                return true;
            }

            service = default(T);
            return false;
        }

        [CanBeNull]
        private Dictionary<Type, object> _services;

        /// <summary>
        /// Tries to get the service with given <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <param name="service">Found service.</param>
        /// <returns>True if the service was found, false otherwise.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        protected virtual bool TryGetService([NotNull] Type serviceType, out object service)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(serviceType != null);
#endif

            lock (SyncRoot)
            {
                if (_services is null)
                    _services = new Dictionary<Type, object>();

                if (_services.TryGetValue(serviceType, out service))
                    return service != null;

                if (serviceType == typeof(ICancelManager))
                    _services[serviceType] = service = new CancelManager();
                else
                    _services[serviceType] = null;

                return service != null;
            }
        }

        #endregion

        private void BeginComputation()
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(State == ComputationState.NotRunning);
#endif

            lock (SyncRoot)
            {
                _state = ComputationState.Running;
                Services.CancelManager.ResetCancel();
                OnStarted(EventArgs.Empty);
                OnStateChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called on algorithm initialization step.
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// Algorithm compute step.
        /// </summary>
        protected abstract void InternalCompute();

        /// <summary>
        /// Called on algorithm cleanup step.
        /// </summary>
        protected virtual void Clean()
        {
        }

        private void EndComputation()
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(
                State == ComputationState.Running || State == ComputationState.Aborted);
#endif

            lock (SyncRoot)
            {
                switch (_state)
                {
                    case ComputationState.Running:
                        _state = ComputationState.Finished;
                        OnFinished(EventArgs.Empty);
                        break;
                    case ComputationState.PendingAbortion:
                        _state = ComputationState.Aborted;
                        OnAborted(EventArgs.Empty);
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                Services.CancelManager.ResetCancel();
                OnStateChanged(EventArgs.Empty);
            }
        }
    }
}
