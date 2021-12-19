#nullable enable

#if SUPPORTS_SERIALIZATION
#endif

namespace FastGraph.Graphviz.Dot
{
    /// <summary>
    /// Enumeration of possible label justification.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public enum GraphvizLabelJustification
    {
        /// <summary>
        /// Left justification.
        /// </summary>
        L,

        /// <summary>
        /// Right justification.
        /// </summary>
        R,

        /// <summary>
        /// Centered.
        /// </summary>
        C
    }
}
