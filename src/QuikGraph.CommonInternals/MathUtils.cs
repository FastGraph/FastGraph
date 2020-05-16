using System;
using JetBrains.Annotations;

namespace QuikGraph.Utils
{
    /// <summary>
    /// Math utilities.
    /// </summary>
    internal static class MathUtils
    {
        /// <summary>
        /// Smallest value such that 1.0+<see cref="DoubleEpsilon"/> != 1.0
        /// </summary>
        public const double DoubleEpsilon = 2.2204460492503131e-016;

        /// <summary>
        /// Returns whether or not the double is "close" to 0, but this is faster.
        /// </summary>
        /// <returns>The result of the comparision.</returns>
        /// <param name="a">The double to compare to 0.</param>
        [Pure]
        public static bool IsZero(double a)
        {
            return Math.Abs(a) < 10.0 * DoubleEpsilon;
        }

        /// <summary>
        /// Returns whether or not two <see cref="double"/>s are "equal". That is, whether or
        /// not they are within epsilon of each other.
        /// </summary>
        /// <param name="a">The first <see cref="double"/> to compare.</param>
        /// <param name="b">The second <see cref="double"/> to compare.</param>
        /// <returns>The result of the comparision.</returns>
        [Pure]
        public static bool NearEqual(double a, double b)
        {
            // In case they are Infinities (then epsilon check does not work)
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (a == b)
                return true;

            // This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < DoubleEpsilon
            double eps = (Math.Abs(a) + Math.Abs(b) + 10.0) * DoubleEpsilon;
            double delta = a - b;
            return -eps < delta && eps > delta;
        }
    }
}