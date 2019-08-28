
#if SUPPORTS_SERIALIZATION
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using NUnit.Framework;

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
            Exception ex = createException();

            // Save the full ToString() value, including the exception message and stack trace.
            string exceptionToString = ex.ToString();

            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                // "Save" object state
                bf.Serialize(ms, ex);

                // Re-use the same stream for de-serialization
                ms.Seek(0, 0);

                // Replace the original exception with de-serialized one
                ex = (TException)bf.Deserialize(ms);
            }

            // Double-check that the exception message and stack trace (owned by the base Exception) are preserved
            Assert.AreEqual(exceptionToString, ex.ToString());
        }

        [Test]
        public void ExceptionsSerialization()
        {
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