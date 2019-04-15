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
        public const string EdgeFormatString = "{0}->{1}";

        /// <summary>
        /// Edge string formatting (with tag).
        /// </summary>
        public const string TaggedEdgeFormatString = "{0}->{1}:{2}";

        /// <summary>
        /// Undirected edge string formatting.
        /// </summary>
        public const string UndirectedEdgeFormatString = "{0}<->{1}";

        /// <summary>
        /// Undirected edge string formatting (with tag).
        /// </summary>
        public const string TaggedUndirectedEdgeFormatString = "{0}<->{1}:{2}";
    }
}