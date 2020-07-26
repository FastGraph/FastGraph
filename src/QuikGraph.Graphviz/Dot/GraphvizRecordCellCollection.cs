#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace QuikGraph.Graphviz.Dot
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
        /// <param name="list">The list that is wrapped by the new collection.</param>
        public GraphvizRecordCellCollection([NotNull, ItemNotNull] GraphvizRecordCell[] list)
            : base(list)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizRecordCellCollection"/> class.
        /// </summary>
        /// <param name="list">The list that is wrapped by the new collection.</param>
        public GraphvizRecordCellCollection([NotNull, ItemNotNull] GraphvizRecordCellCollection list)
            : base(list)
        {
        }
    }
}