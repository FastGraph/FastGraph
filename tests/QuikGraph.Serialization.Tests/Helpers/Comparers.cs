using System;
using System.Collections;
using System.Collections.Generic;

namespace QuikGraph.Serialization.Tests
{
    /// <summary>
    /// <see cref="IComparer{Single}"/> for floats with user-specified tolerance.
    /// </summary>
    internal class FloatComparer : IComparer, IComparer<float>
    {
        /// <summary>
        /// Default tolerance.
        /// </summary>
        public static float DefaultTolerance { get; } = 1e-5f;

        /// <summary>
        /// Tolerance used during equality comparison.
        /// </summary>
        public float Tolerance { get; }

        /// <summary>
        /// Constructs <see cref="FloatComparer"/> with default tolerance.
        /// </summary>
        public FloatComparer()
            : this(DefaultTolerance)
        {
        }

        /// <summary>
        /// Constructs <see cref="FloatComparer"/> with user-given tolerance.
        /// </summary>
        /// <param name="tolerance">Tolerance.</param>
        public FloatComparer(float tolerance)
        {
            Tolerance = tolerance;
        }

        /// <inheritdoc />
        public int Compare(object x, object y)
        {
            if (x is null)
                return y is null ? 0 : -1;
            if (y is null)
                return 1;
            if (x is float f1 && y is float f2)
                return Compare(f1, f2);
            throw new ArgumentException("Must compare float values.");
        }

        /// <inheritdoc />
        public int Compare(float x, float y)
        {
            float abs = Math.Abs(x - y);
            if (abs <= Tolerance)
                return 0;
            return -Math.Sign(abs);
        }
    }

    /// <summary>
    /// <see cref="IComparer{Double}"/> for doubles with user-specified tolerance.
    /// </summary>
    internal class DoubleComparer : IComparer, IComparer<double>
    {
        /// <summary>
        /// Default tolerance.
        /// </summary>
        public static double DefaultTolerance { get; } = 1e-5f;

        /// <summary>
        /// Tolerance used during equality comparison.
        /// </summary>
        public double Tolerance { get; }

        /// <summary>
        /// Constructs <see cref="DoubleComparer"/> with default tolerance.
        /// </summary>
        public DoubleComparer()
            : this(DefaultTolerance)
        {
        }

        /// <summary>
        /// Constructs <see cref="DoubleComparer"/> with user-given tolerance.
        /// </summary>
        /// <param name="tolerance">Tolerance.</param>
        public DoubleComparer(double tolerance)
        {
            Tolerance = tolerance;
        }

        /// <inheritdoc />
        public int Compare(object x, object y)
        {
            if (x is null)
                return y is null ? 0 : -1;
            if (y is null)
                return 1;
            if (x is double d1 && y is double d2)
                return Compare(d1, d2);
            throw new ArgumentException("Must compare double values.");
        }

        /// <inheritdoc />
        public int Compare(double x, double y)
        {
            double abs = Math.Abs(x - y);
            if (abs <= Tolerance)
                return 0;
            return -Math.Sign(abs);
        }
    }
}