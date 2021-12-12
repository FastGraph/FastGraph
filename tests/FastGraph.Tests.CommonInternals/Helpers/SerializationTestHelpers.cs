#nullable enable

using JetBrains.Annotations;

namespace FastGraph.Tests
{
    /// <summary>
    /// Test helpers related to serialization.
    /// </summary>
    internal static class SerializationTestHelpers
    {
        [Pure]
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", error: true)]
        public static T SerializeAndDeserialize<T>(T @object)
            where T : notnull
        {
            throw new NotSupportedException("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.");
        }
    }
}
