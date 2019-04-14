using System;
using System.Collections.Generic;
using System.Text;

namespace QuikGraph.Collections
{
    public interface IQueue<T>
    {
        int Count { get; }

        bool Contains(T value);
        void Enqueue(T value);
        T Dequeue();
        T Peek();

        T[] ToArray();
    }
}
