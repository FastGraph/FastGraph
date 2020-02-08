using JetBrains.Annotations;

namespace QuikGraph.Constants
{
    /// <summary>
    /// Constants related to edges.
    /// </summary>
    internal static class EdgeConstants
    {
        /// <summary>
        /// Edge string formatting.
        /// </summary>
        [NotNull]
        public const string EdgeFormatString = "{0} -> {1}";

        /// <summary>
        /// Edge terminals string formatting.
        /// </summary>
        [NotNull]
        public const string EdgeTerminalFormatString = "{0} ({1}) -> {2} ({3})";

        /// <summary>
        /// Edge string formatting (with tag).
        /// </summary>
        [NotNull]
        public const string TaggedEdgeFormatString = "{0} -> {1} ({2})";

        /// <summary>
        /// Undirected edge string formatting.
        /// </summary>
        [NotNull]
        public const string UndirectedEdgeFormatString = "{0} <-> {1}";

        /// <summary>
        /// Undirected edge string formatting (with tag).
        /// </summary>
        [NotNull]
        public const string TaggedUndirectedEdgeFormatString = "{0} <-> {1} ({2})";
    }
}