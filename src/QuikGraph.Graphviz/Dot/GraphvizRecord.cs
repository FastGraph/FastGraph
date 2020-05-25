using System.Text;
using JetBrains.Annotations;

namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Graphviz record.
    /// </summary>
    public class GraphvizRecord
    {
        /// <summary>
        /// Record cells.
        /// </summary>
        [NotNull, ItemNotNull]
        public GraphvizRecordCellCollection Cells { get; } = new GraphvizRecordCellCollection();

        /// <summary>
        /// Converts this record to DOT.
        /// </summary>
        /// <returns>Record as DOT.</returns>
        [Pure]
        [NotNull]
        public string ToDot()
        {
            if (Cells.Count == 0)
                return string.Empty;

            var builder = new StringBuilder();
            bool flag = false;
            foreach (GraphvizRecordCell cell in Cells)
            {
                if (flag)
                {
                    builder.AppendFormat(" | {0}", cell.ToDot());
                    continue;
                }
                builder.Append(cell.ToDot());
                flag = true;
            }
            return builder.ToString();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToDot();
        }
    }
}