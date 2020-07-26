#if SUPPORTS_SERIALIZATION
using System;
#endif

namespace QuikGraph
{
    /// <summary>
    /// Colors used in vertex coloring algorithms.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public enum GraphColor : byte
    {
        /// <summary>
        /// Usually initial color.
        /// </summary>
        White = 0,

        /// <summary>
        /// Usually intermediate color.
        /// </summary>
        Gray,

        /// <summary>
        /// Usually finished color.
        /// </summary>
        Black
    }
}