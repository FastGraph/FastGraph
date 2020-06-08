using System;
using System.Data;
using JetBrains.Annotations;

namespace QuikGraph.Data
{
    /// <summary>
    /// Represents a relation between <see cref="DataTable"/>s.
    /// </summary>
    public sealed class DataRelationEdge : IEdge<DataTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataRelationEdge"/> class.
        /// </summary>
        /// <param name="relation">Data relation.</param>
        public DataRelationEdge([NotNull] DataRelation relation)
        {
            Relation = relation ?? throw new ArgumentNullException(nameof(relation));
        }

        /// <summary>
        /// Data relation hold by this edge.
        /// </summary>
        public DataRelation Relation { get; }

        /// <inheritdoc />
        public DataTable Source => Relation.ParentTable;

        /// <inheritdoc />
        public DataTable Target => Relation.ChildTable;
    }
}