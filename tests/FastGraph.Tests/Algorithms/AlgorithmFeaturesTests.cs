#nullable enable

using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using FastGraph.Algorithms;

namespace FastGraph.Tests.Algorithms
{
    /// <summary>
    /// Tests related to algorithm features (state, services).
    /// </summary>
    [TestFixture]
    internal sealed class AlgorithmFeaturesTests
    {
        #region Test classes

        private class TestAlgorithm : AlgorithmBase<AdjacencyGraph<int, Edge<int>>>
        {
            public TestAlgorithm()
                : base(new AdjacencyGraph<int, Edge<int>>())
            {
            }

            protected override void InternalCompute()
            {
            }

            protected override bool TryGetService(Type serviceType, [NotNullWhen(true)] out object? service)
            {
                if (serviceType == typeof(TestService))
                {
                    service = new TestService();
                    return true;
                }

                if (serviceType == typeof(TestNullService))
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
                    return base.TryGetService(default, out service);
#pragma warning restore CS8625
                }

                return base.TryGetService(serviceType, out service);
            }
        }

        private static readonly TimeSpan TimeoutDelay = TimeSpan.FromSeconds(5);

        private class ManageableTestAlgorithm : AlgorithmBase<AdjacencyGraph<int, Edge<int>>>
        {
            public ManualResetEvent InitializeEvent { get; }
            public ManualResetEvent InitializedEvent { get; } = new ManualResetEvent(false);

            public ManualResetEvent ComputeEvent { get; }
            public ManualResetEvent ComputedEvent { get; } = new ManualResetEvent(false);

            public ManualResetEvent CleanEvent { get; }
            public ManualResetEvent CleanedEvent { get; } = new ManualResetEvent(false);

            public ManageableTestAlgorithm(
                ManualResetEvent initialize,
                ManualResetEvent compute,
                ManualResetEvent clean)
                : base(new AdjacencyGraph<int, Edge<int>>())
            {
                InitializeEvent = initialize;
                ComputeEvent = compute;
                CleanEvent = clean;
            }

            protected override void Initialize()
            {
                InitializeEvent.Set();
                InitializedEvent.WaitOne(TimeoutDelay);
                ThrowIfCancellationRequested();
            }

            protected override void InternalCompute()
            {
                ComputeEvent.Set();
                ComputedEvent.WaitOne(TimeoutDelay);
                ThrowIfCancellationRequested();
            }

            protected override void Clean()
            {
                CleanEvent.Set();
                CleanedEvent.WaitOne(TimeoutDelay);
            }
        }

        private class TestService
        {
        }

        private class TestNotInService
        {
        }

        private class TestNullService
        {
        }

        #endregion

        [Test]
        public void AlgorithmNormalStates()
        {
            var initialize = new ManualResetEvent(false);
            var compute = new ManualResetEvent(false);
            var clean = new ManualResetEvent(false);
            var finished = new ManualResetEvent(false);

            var algorithm = new ManageableTestAlgorithm(initialize, compute, clean);

            bool hasStarted = false;
            bool hasFinished = false;
            var expectedStates = new Queue<ComputationState>();
            expectedStates.Enqueue(ComputationState.Running);
            expectedStates.Enqueue(ComputationState.Finished);

            algorithm.Started += (_, _) =>
            {
                if (hasStarted)
                    Assert.Fail($"{nameof(AlgorithmBase<object>.Started)} event called twice.");
                hasStarted = true;
            };
            algorithm.Finished += (_, _) =>
            {
                if (hasFinished)
                    Assert.Fail($"{nameof(AlgorithmBase<object>.Finished)} event called twice.");
                hasFinished = true;
                finished.Set();
            };
            algorithm.StateChanged += (_, _) =>
            {
                algorithm.State.Should().Be(expectedStates.Peek());
                expectedStates.Dequeue();
            };
            algorithm.Aborted += (_, _) =>
            {
                Assert.Fail($"{nameof(AlgorithmBase<object>.Aborted)} event called.");
            };

            algorithm.State.Should().Be(ComputationState.NotRunning);

            // Run the algorithm
            Task.Run(() =>
            {
                Invoking(algorithm.Compute).Should().NotThrow();
            });

            initialize.WaitOne(TimeoutDelay);
            algorithm.State.Should().Be(ComputationState.Running);
            hasStarted.Should().BeTrue();

            algorithm.InitializedEvent.Set();

            compute.WaitOne(TimeoutDelay);
            algorithm.State.Should().Be(ComputationState.Running);

            algorithm.ComputedEvent.Set();

            clean.WaitOne(TimeoutDelay);
            algorithm.State.Should().Be(ComputationState.Running);

            algorithm.CleanedEvent.Set();

            finished.WaitOne(TimeoutDelay);
            algorithm.State.Should().Be(ComputationState.Finished);
            hasFinished.Should().BeTrue();
            (expectedStates.Count == 0).Should().BeTrue();
        }

        [Test]
        public void AlgorithmStates_AbortBeforeStart()
        {
            var initialize = new ManualResetEvent(true);
            var compute = new ManualResetEvent(true);
            var clean = new ManualResetEvent(true);
            var end = new ManualResetEvent(false);

            var algorithm = new ManageableTestAlgorithm(initialize, compute, clean);

            algorithm.Started += (_, _) =>
            {
                Assert.Fail($"{nameof(AlgorithmBase<object>.Started)} event called.");
            };
            algorithm.Finished += (_, _) =>
            {
                Assert.Fail($"{nameof(AlgorithmBase<object>.Finished)} event called.");
            };
            algorithm.StateChanged += (_, _) =>
            {
                Assert.Fail($"{nameof(AlgorithmBase<object>.StateChanged)} event called.");
            };
            algorithm.Aborted += (_, _) =>
            {
                Assert.Fail($"{nameof(AlgorithmBase<object>.Aborted)} event called.");
            };

            algorithm.State.Should().Be(ComputationState.NotRunning);

            // Abort the algorithm
            Task.Run(() =>
            {
                Invoking(() =>
                {
                    algorithm.Abort();
                    algorithm.State.Should().Be(ComputationState.NotRunning);
                    end.Set();
                }).Should().NotThrow();
            });

            end.WaitOne(TimeoutDelay);
        }

        [Test]
        public void AlgorithmStates_AbortDuringRun()
        {
            var initialize = new ManualResetEvent(false);
            var compute = new ManualResetEvent(false);
            var clean = new ManualResetEvent(false);
            var aborted = new ManualResetEvent(false);

            var algorithm = new ManageableTestAlgorithm(initialize, compute, clean);

            bool hasStarted = false;
            bool hasAborted = false;
            var expectedStates = new Queue<ComputationState>();
            expectedStates.Enqueue(ComputationState.Running);
            expectedStates.Enqueue(ComputationState.PendingAbortion);
            expectedStates.Enqueue(ComputationState.Aborted);

            algorithm.Started += (_, _) =>
            {
                if (hasStarted)
                    Assert.Fail($"{nameof(AlgorithmBase<object>.Started)} event called twice.");
                hasStarted = true;
            };
            algorithm.Finished += (_, _) =>
            {
                Assert.Fail($"{nameof(AlgorithmBase<object>.Finished)} event called.");
            };
            algorithm.StateChanged += (_, _) =>
            {
                algorithm.State.Should().Be(expectedStates.Peek());
                expectedStates.Dequeue();
            };
            algorithm.Aborted += (_, _) =>
            {
                if (hasAborted)
                    Assert.Fail($"{nameof(AlgorithmBase<object>.Aborted)} event called twice.");
                hasAborted = true;
                aborted.Set();
            };

            algorithm.State.Should().Be(ComputationState.NotRunning);

            // Run the algorithm
            Task.Run(() =>
            {
                Invoking(algorithm.Compute).Should().NotThrow();
            });

            initialize.WaitOne(TimeoutDelay);
            algorithm.State.Should().Be(ComputationState.Running);
            hasStarted.Should().BeTrue();

            algorithm.InitializedEvent.Set();

            compute.WaitOne(TimeoutDelay);
            algorithm.State.Should().Be(ComputationState.Running);

            algorithm.Abort();
            algorithm.ComputedEvent.Set();

            algorithm.State.Should().Be(ComputationState.PendingAbortion);

            algorithm.CleanedEvent.Set();

            aborted.WaitOne(TimeoutDelay);
            algorithm.State.Should().Be(ComputationState.Aborted);
            hasAborted.Should().BeTrue();
            (expectedStates.Count == 0).Should().BeTrue();
        }

        [Test]
        public void AlgorithmStates_CancelDuringRun()
        {
            var initialize = new ManualResetEvent(false);
            var compute = new ManualResetEvent(false);
            var clean = new ManualResetEvent(false);
            var finished = new ManualResetEvent(false);

            var algorithm = new ManageableTestAlgorithm(initialize, compute, clean);

            bool hasStarted = false;
            bool hasFinished = false;
            var expectedStates = new Queue<ComputationState>();
            expectedStates.Enqueue(ComputationState.Running);
            expectedStates.Enqueue(ComputationState.Finished);

            algorithm.Started += (_, _) =>
            {
                if (hasStarted)
                    Assert.Fail($"{nameof(AlgorithmBase<object>.Started)} event called twice.");
                hasStarted = true;
            };
            algorithm.Finished += (_, _) =>
            {
                if (hasFinished)
                    Assert.Fail($"{nameof(AlgorithmBase<object>.Finished)} event called twice.");
                hasFinished = true;
                finished.Set();
            };
            algorithm.StateChanged += (_, _) =>
            {
                algorithm.State.Should().Be(expectedStates.Peek());
                expectedStates.Dequeue();
            };
            algorithm.Aborted += (_, _) =>
            {
                Assert.Fail($"{nameof(AlgorithmBase<object>.Aborted)} event called.");
            };

            algorithm.State.Should().Be(ComputationState.NotRunning);

            // Run the algorithm
            Task.Run(() =>
            {
                Invoking(algorithm.Compute).Should().NotThrow();
            });

            initialize.WaitOne(TimeoutDelay);
            algorithm.State.Should().Be(ComputationState.Running);
            hasStarted.Should().BeTrue();

            algorithm.InitializedEvent.Set();

            compute.WaitOne(TimeoutDelay);
            algorithm.State.Should().Be(ComputationState.Running);

            algorithm.Services.CancelManager.Cancel();
            algorithm.Services.CancelManager.ResetCancel(); // These calls don't change algorithm state
            algorithm.Services.CancelManager.Cancel();
            algorithm.ComputedEvent.Set();

            algorithm.State.Should().Be(ComputationState.Running);

            algorithm.CleanedEvent.Set();

            finished.WaitOne(TimeoutDelay);
            algorithm.State.Should().Be(ComputationState.Finished);
            hasFinished.Should().BeTrue();
            (expectedStates.Count == 0).Should().BeTrue();
        }

        [Test]
        public void GetService()
        {
            var algorithm = new TestAlgorithm();
            var service = algorithm.GetService<TestService>();
            service.Should().BeAssignableTo<TestService>();
        }

        [Test]
        public void GetService_Throws()
        {
            var algorithm = new TestAlgorithm();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Invoking(() => algorithm.GetService<TestNullService>()).Should().Throw<ArgumentNullException>();
            Invoking(() => algorithm.GetService<TestNotInService>()).Should().Throw<InvalidOperationException>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void TryGetService()
        {
            var algorithm = new TestAlgorithm();
            algorithm.TryGetService(out TestService? service).Should().BeTrue();
            service.Should().BeAssignableTo<TestService>();

            algorithm.TryGetService<TestNotInService>(out _).Should().BeFalse();
        }
    }
}
