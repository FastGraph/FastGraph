#if SUPPORTS_SERIALIZATION
using System;
#endif

namespace QuikGraph.Collections
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class Queue<T> : System.Collections.Generic.Queue<T>, IQueue<T>
    {
    }
}
