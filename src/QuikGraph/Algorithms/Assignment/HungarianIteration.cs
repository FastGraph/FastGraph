using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Assignment
{
    /// <summary>
    /// State of an iteration of the Hungarian algorithm.
    /// </summary>
    public struct HungarianIteration
    {
        /// <summary>
        /// Costs matrix.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public int[,] Matrix { get; }

        /// <summary>
        /// Matrix mask.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public byte[,] Mask { get; }

        /// <summary>
        /// Array of treated rows.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public bool[] RowsCovered { get; }

        /// <summary>
        /// Array of treated columns.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public bool[] ColumnsCovered { get; }

        /// <summary>
        /// <see cref="HungarianAlgorithm.Steps"/> corresponding to this iteration.
        /// </summary>
        public HungarianAlgorithm.Steps Step { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HungarianIteration"/> struct.
        /// </summary>
        internal HungarianIteration(
            [NotNull] int[,] costs,
            [NotNull] byte[,] mask,
            [NotNull] bool[] rowsCovered,
            [NotNull] bool[] columnsCovered, 
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
