#nullable enable

#if SUPPORTS_SERIALIZATION
#endif

namespace FastGraph.Graphviz.Dot
{
    /// <summary>
    /// Enumeration of possible ratio modes.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public enum GraphvizRatioMode
    {
        /// <summary>
        /// Filling.
        /// </summary>
        Fill,

        /// <summary>
        /// Compressing.
        /// </summary>
        Compress,

        /// <summary>
        /// Automatic.
        /// </summary>
        Auto
    }
}
