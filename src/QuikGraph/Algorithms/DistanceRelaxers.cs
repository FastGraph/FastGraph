using JetBrains.Annotations;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Implementation of several distance relaxers.
    /// </summary>
    public static class DistanceRelaxers
    {
        /// <summary>
        /// Shortest distance relaxer.
        /// </summary>
        [NotNull]
        public static readonly IDistanceRelaxer ShortestDistance = new ShortestDistanceRelaxer();

        private sealed class ShortestDistanceRelaxer : IDistanceRelaxer
        {
            /// <inheritdoc />
            public double InitialDistance => double.MaxValue;

            /// <inheritdoc />
            public int Compare(double x, double y)
            {
                return x.CompareTo(y);
            }

            /// <inheritdoc />
            public double Combine(double distance, double weight)
            {
                return distance + weight;
            }
        }

        /// <summary>
        /// Critical distance relaxer.
        /// </summary>
        [NotNull]
        public static readonly IDistanceRelaxer CriticalDistance = new CriticalDistanceRelaxer();

        private sealed class CriticalDistanceRelaxer : IDistanceRelaxer
        {
            /// <inheritdoc />
            public double InitialDistance => double.MinValue;

            /// <inheritdoc />
            public int Compare(double x, double y)
            {
                return -x.CompareTo(y);
            }

            /// <inheritdoc />
            public double Combine(double distance, double weight)
            {
                return distance + weight;
            }
        }

        /// <summary>
        /// Edge shortest distance relaxer.
        /// </summary>
        [NotNull]
        public static readonly IDistanceRelaxer EdgeShortestDistance = new EdgeDistanceRelaxer();

        private sealed class EdgeDistanceRelaxer : IDistanceRelaxer
        {
            /// <inheritdoc />
            public double InitialDistance => 0;

            /// <inheritdoc />
            public int Compare(double x, double y)
            {
                return x.CompareTo(y);
            }

            /// <inheritdoc />
            public double Combine(double distance, double weight)
            {
                return distance + weight;
            }
        }

        /// <summary>
        /// Prim relaxer.
        /// </summary>
        [NotNull]
        internal static readonly IDistanceRelaxer Prim = new PrimRelaxer();

        internal class PrimRelaxer : IDistanceRelaxer
        {
            public double InitialDistance => double.MaxValue;

            public int Compare(double x, double y)
            {
                return x.CompareTo(y);
            }

            public double Combine(double distance, double weight)
            {
                return weight;
            }
        }
    }
}