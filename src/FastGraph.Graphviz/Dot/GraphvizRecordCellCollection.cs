#nullable enable

#if SUPPORTS_SERIALIZATION
#endif
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace FastGraph.Graphviz.Dot
{
    /// <summary>
    /// Graphviz record cell collection.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class GraphvizRecordCellCollection : Collection<GraphvizRecordCell>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizRecordCellCollection"/> class.
        /// </summary>
        public GraphvizRecordCellCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizRecordCellCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection that is wrapped by the new collection.</param>
        public GraphvizRecordCellCollection(IList<GraphvizRecordCell> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizRecordCellCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection that is wrapped by the new collection.</param>
        public GraphvizRecordCellCollection([ItemNotNull] GraphvizRecordCellCollection collection)
            : base(collection)
        {
        }
    }
}
