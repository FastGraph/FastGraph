#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Algorithms.Observers
{
    /// <summary>
    /// Base class for observer tests.
    /// </summary>
    internal abstract class ObserverTestsBase
    {
        protected static void Attach_Throws_Test<TAlgorithm>(
            FastGraph.Algorithms.Observers.IObserver<TAlgorithm> observer)
            where TAlgorithm : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Throws<ArgumentNullException>(() => observer.Attach(default));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
