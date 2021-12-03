using System;
using JetBrains.Annotations;
using NUnit.Framework;

namespace FastGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Base class for observer tests.
    /// </summary>
    internal abstract class ObserverTestsBase
    {
        protected static void Attach_Throws_Test<TAlgorithm>(
            [NotNull] FastGraph.Algorithms.Observers.IObserver<TAlgorithm> observer)
            where TAlgorithm : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => observer.Attach(null));
        }
    }
}
