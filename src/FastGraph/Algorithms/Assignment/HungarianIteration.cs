#nullable enable

namespace FastGraph.Algorithms.Assignment
{
    /// <summary>
    /// State of an iteration of the Hungarian algorithm.
    /// </summary>
    public struct HungarianIteration
    {
        /// <summary>
        /// Costs matrix.
        /// </summary>
        public int[,] Matrix { get; }

        /// <summary>
        /// Matrix mask.
        /// </summary>
        public byte[,] Mask { get; }

        /// <summary>
        /// Array of treated rows.
        /// </summary>
        public bool[] RowsCovered { get; }

        /// <summary>
        /// Array of treated columns.
        /// </summary>
        public bool[] ColumnsCovered { get; }

        /// <summary>
        /// <see cref="HungarianAlgorithm.Steps"/> corresponding to this iteration.
        /// </summary>
        public HungarianAlgorithm.Steps Step { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HungarianIteration"/> struct.
        /// </summary>
        internal HungarianIteration(
            int[,] costs,
            byte[,] mask,
            bool[] rowsCovered,
            bool[] columnsCovered,
            HungarianAlgorithm.Steps step)
        {
            Matrix = costs;
            Mask = mask;
            RowsCovered = rowsCovered;
            ColumnsCovered = columnsCovered;
            Step = step;
        }
    }
}
