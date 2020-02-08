using JetBrains.Annotations;

namespace QuikGraph.Serialization
{
    internal static class XmlConstants
    {
        [NotNull]
        public const string DynamicMethodPrefix = "QuikGraph";

        #region Tags

        [NotNull]
        public const string GraphMLTag = "graphml";

        [NotNull]
        public const string GraphTag = "graph";

        [NotNull]
        public const string NodeTag = "node";

        [NotNull]
        public const string EdgeTag = "edge";

        [NotNull]
        public const string DataTag = "data";

        #endregion

        #region Attributes

        [NotNull]
        public const string IdAttribute = "id";

        [NotNull]
        public const string SourceAttribute = "source";

        [NotNull]
        public const string TargetAttribute = "target";

        #endregion
    }
}