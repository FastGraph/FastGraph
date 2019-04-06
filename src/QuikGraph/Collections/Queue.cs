#if SUPPORTS_SERIALIZATION
using System;
#endif

namespace QuickGraph.Collections
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class Queue<T> : System.Collections.Generic.Queue<T>, IQueue<T>
    {
    }
}
