#nullable enable

#if SUPPORTS_SERIALIZATION
#endif

namespace FastGraph.Collections
{
    /// <inheritdoc cref="IQueue{T}" />
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class Queue<T> : System.Collections.Generic.Queue<T>, IQueue<T>
    {
    }
}
