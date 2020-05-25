using System.Text;
using JetBrains.Annotations;

namespace QuikGraph.Graphviz.Dot
{
    /// <summary>
    /// Graphviz record cell.
    /// </summary>
    public class GraphvizRecordCell
    {
        /// <summary>
        /// Record cells.
        /// </summary>
        [NotNull, ItemNotNull]
        public GraphvizRecordCellCollection Cells { get; } = new GraphvizRecordCellCollection();

        /// <summary>
        /// Record escaper.
        /// </summary>
        [NotNull]
        protected GraphvizRecordEscaper Escaper { get; } = new GraphvizRecordEscaper();

        /// <summary>
        /// Indicates if record has port.
        /// </summary>
        public bool HasPort
        {
            get
            {
                if (Port != null)
                {
                    return (Port.Length > 0);
                }
                return false;
            }
        }

        /// <summary>
        /// Port.
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Indicates if record has text.
        /// </summary>
        public bool HasText
        {
            get
            {
                if (Text != null)
                {
                    return (Text.Length > 0);
                }
                return false;
            }
        }

        /// <summary>
        /// Text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Converts this record cell to DOT.
        /// </summary>
        /// <returns>Record cell as DOT.</returns>
        [Pure]
        [NotNull]
        public string ToDot()
        {
            var builder = new StringBuilder();
            if (HasPort)
            {
                builder.AppendFormat("<{0}> ", Escaper.Escape(Port));
            }
            if (HasText)
            {
                builder.AppendFormat("{0}", Escaper.Escape(Text));
            }
            if (Cells.Count > 0)
            {
                builder.Append(" { ");
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
                builder.Append(" } ");
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