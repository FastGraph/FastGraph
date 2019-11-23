#if SUPPORTS_SERIALIZATION
using System;
using JetBrains.Annotations;
using NUnit.Framework;
using static QuikGraph.Tests.TestHelpers;

namespace QuikGraph.Tests.Exceptions
{
    /// <summary>
    /// Tests for exceptions.
    /// </summary>
    [TestFixture]
    internal class ExceptionTests
    {
        private static void ExceptionSerializationTest<TException>(
            [NotNull, InstantHandle] Func<TException> createException)
            where TException : Exception
        {
            Exception exception = createException();

            // Save the full ToString() value, including the exception message and stack trace.
            string exceptionToString = exception.ToString();

            Exception deserializedException = SerializeAndDeserialize(exception);

            // Double-check that the exception message and stack trace (owned by the base Exception) are preserved
            Assert.AreNotSame(exception, deserializedException);
            Assert.AreEqual(exceptionToString, deserializedException.ToString());
        }

        [Test]
        public void ExceptionsSerialization()
        {
            ExceptionSerializationTest(() => new NegativeCapacityException());
            ExceptionSerializationTest(() => new NegativeCycleGraphException());
            ExceptionSerializationTest(() => new NegativeWeightException());
            ExceptionSerializationTest(() => new NonAcyclicGraphException());
            ExceptionSerializationTest(() => new NonStronglyConnectedGraphException());
            ExceptionSerializationTest(() => new NoPathFoundException());
            ExceptionSerializationTest(() => new ParallelEdgeNotAllowedException());
            ExceptionSerializationTest(() => new VertexNotFoundException());
        }
    }
}
#endif