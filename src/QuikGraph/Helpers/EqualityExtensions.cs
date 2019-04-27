using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Deep equality methods for collections (arrays, dictionaries) implemented as extension methods.
    /// </summary>
    /// <remarks>
    /// The default equality method, Equals, usually gives reference equality for arrays, dictionaries 
    /// and other collections. What if you have two distinct arrays and you want to check whether they
    /// have equal lengths and contain identical elements? That's when you use Equals1.
    /// What if you have arrays of arrays? Then use Equals2. And so on with Equals3, etc.
    /// </remarks>
    internal static class EqualityExtensions
    {
        #region For IList<T>

        /// <summary>
        /// Element-by-element array equality using a given equality comparer for elements.
        /// Two arrays are equal if they are both null, or are actually the same reference,
        /// or they have equal length and elements are equal for each index. 
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="lhs">First array.</param>
        /// <param name="rhs">Second array.</param>
        /// <param name="elementEquality">Equality comparer for type <typeparamref name="T"/>.</param>
        /// <returns>Whether the two arrays are equal.</returns>
        internal static bool Equals1<T>([CanBeNull] this IList<T> lhs, [CanBeNull] IList<T> rhs, [NotNull] IEqualityComparer<T> elementEquality)
        {
            if (lhs is null)
                return rhs is null;
            if (rhs is null)
                return false;

            if (ReferenceEquals(lhs, rhs))
                return true;

            if (lhs.Count != rhs.Count)
                return false;

            for (int i = 0; i < lhs.Count; i++)
            {
                if (!elementEquality.Equals(lhs[i], rhs[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Element-by-element array equality using <see cref="EqualityComparer{T}.Default"/> to equate elements.
        /// Two arrays are equal if they are both null, or are actually the same reference,
        /// or they have equal length and elements are equal for each index. 
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="lhs">First array.</param>
        /// <param name="rhs">Second array.</param>
        /// <returns>Whether the two arrays are equal.</returns>
        internal static bool Equals1<T>([CanBeNull] this IList<T> lhs, [CanBeNull] IList<T> rhs)
        {
            return Equals1(lhs, rhs, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Specialization of ArrayEquals(T) augmented with tolerance value
        /// to be used when equating two float values.
        /// </summary>
        /// <param name="lhs">First array.</param>
        /// <param name="rhs">Second array.</param>
        /// <param name="tolerance">Tolerance.</param>
        /// <returns>Whether the two arrays are equal.</returns>
        internal static bool Equals1([CanBeNull] this IList<float> lhs, [CanBeNull] IList<float> rhs, float tolerance)
        {
            return Equals1(lhs, rhs, new FloatEqualityComparer(tolerance));
        }

        /// <summary>
        /// Specialization of ArrayEquals{T} augmented with tolerance value
        /// to be used when equating two double values.
        /// </summary>
        /// <param name="lhs">First array.</param>
        /// <param name="rhs">Second array.</param>
        /// <param name="tolerance">Tolerance.</param>
        /// <returns>Whether the two arrays are equal.</returns>
        public static bool Equals1([CanBeNull] this IList<double> lhs, [CanBeNull] IList<double> rhs, double tolerance)
        {
            return Equals1(lhs, rhs, new DoubleEqualityComparer(tolerance));
        }

        #endregion

        #region For Array

        /// <summary>
        /// Element-by-element array equality using a given equality comparer for elements.
        /// Two arrays are equal if they are both null, or are actually the same reference,
        /// or they have equal length and elements are equal for each index. 
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="lhs">First array.</param>
        /// <param name="rhs">Second array.</param>
        /// <param name="elementEquality">Equality comparer for type <typeparamref name="T"/>.</param>
        /// <returns>Whether the two arrays are equal.</returns>
        internal static bool Equals1<T>([CanBeNull] this T[] lhs, [CanBeNull] T[] rhs, [NotNull] IEqualityComparer<T> elementEquality)
        {
            if (lhs is null)
                return rhs is null;
            if (rhs is null)
                return false;

            if (ReferenceEquals(lhs, rhs))
                return true;

            if (lhs.Length != rhs.Length)
                return false;

            for (int i = 0; i < lhs.Length; i++)
            {
                if (!elementEquality.Equals(lhs[i], rhs[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Element-by-element array equality using <see cref="EqualityComparer{T}.Default"/> to equate elements.
        /// Two arrays are equal if they are both null, or are actually the same reference,
        /// or they have equal length and elements are equal for each index. 
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="lhs">First array.</param>
        /// <param name="rhs">Second array.</param>
        /// <returns>Whether the two arrays are equal.</returns>
        internal static bool Equals1<T>([CanBeNull] this T[] lhs, [CanBeNull] T[] rhs)
        {
            return Equals1(lhs, rhs, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Specialization of ArrayEquals(T) augmented with tolerance value
        /// to be used when equating two float values.
        /// </summary>
        /// <param name="lhs">First array.</param>
        /// <param name="rhs">Second array.</param>
        /// <param name="tolerance">Tolerance.</param>
        /// <returns>Whether the two arrays are equal.</returns>
        internal static bool Equals1([CanBeNull] this float[] lhs, [CanBeNull] float[] rhs, float tolerance)
        {
            return Equals1(lhs, rhs, new FloatEqualityComparer(tolerance));
        }

        /// <summary>
        /// Specialization of ArrayEquals{T} augmented with tolerance value
        /// to be used when equating two double values.
        /// </summary>
        /// <param name="lhs">First array.</param>
        /// <param name="rhs">Second array.</param>
        /// <param name="tolerance">Tolerance.</param>
        /// <returns>Whether the two arrays are equal.</returns>
        internal static bool Equals1([CanBeNull] this double[] lhs, [CanBeNull] double[] rhs, double tolerance)
        {
            return Equals1(lhs, rhs, new DoubleEqualityComparer(tolerance));
        }

        /// <summary>
        /// 2-level deep equality for array of arrays of <typeparamref name="T"/> using a given element equality comparer.
        /// This has '2' in its name, because one may want to do partially deep equality on multi-dimensional array type.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="lhs">First array.</param>
        /// <param name="rhs">Second array.</param>
        /// <param name="elementEquality">Equality comparer for type <typeparamref name="T"/>.</param>
        /// <returns>Whether the two arrays are equal.</returns>
        internal static bool Equals2<T>([CanBeNull] this T[][] lhs, [CanBeNull] T[][] rhs, [NotNull] IEqualityComparer<T> elementEquality)
        {
            return Equals1(lhs, rhs, new ArrayEqualityComparer<T>(elementEquality));
        }

        /// <summary>
        /// 2-level deep equality for array of arrays of <typeparamref name="T"/> using <see cref="EqualityComparer{T}.Default"/> as element equality.
        /// This has '2' in its name, because one may want to do partially deep equality on multi-dimensional array type.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="lhs">First array.</param>
        /// <param name="rhs">Second array.</param>
        /// <returns>Whether the two arrays are equal.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Is it really an error?")]
        internal static bool Equals2<T>([CanBeNull] this T[][] lhs, [CanBeNull] T[][] rhs)
        {
            return Equals2(lhs, rhs, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// 3-level deep equality for array of arrays of arrays of T using a given element equality comparer.
        /// This has '3' in its name, because one may want to do partially deep equality on multi-dimensional array type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <param name="elementEquality"></param>
        /// <returns></returns>
        internal static bool Equals3<T>([CanBeNull] this T[][][] lhs, [CanBeNull] T[][][] rhs, [NotNull] IEqualityComparer<T> elementEquality)
        {
            return Equals2(lhs, rhs, new ArrayEqualityComparer<T>(elementEquality));
        }

        /// <summary>
        /// 3-level deep equality for array of arrays of arrays of <typeparamref name="T"/> using <see cref="EqualityComparer{T}.Default"/> as element equality.
        /// This has '3' in its name, because one may want to do partially deep equality on multi-dimensional array type.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="lhs">First array.</param>
        /// <param name="rhs">Second array.</param>
        /// <returns>Whether the two arrays are equal.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Is it really an error?")]
        internal static bool Equals3<T>([CanBeNull] this T[][][] lhs, [CanBeNull] T[][][] rhs)
        {
            return Equals3(lhs, rhs, EqualityComparer<T>.Default);
        }

        #endregion

        #region For IDictionary

        /// <summary>
        /// Element-by-element dictionary equality using a given equality comparer for values.
        /// Two dictionaries are equal if they are both null, or are actually the same reference,
        /// or they have identical sets of keys and elements are equal for each key. 
        /// </summary>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="lhs">First dictionary.</param>
        /// <param name="rhs">Second dictionary.</param>
        /// <param name="valueEquality">Equality comparer for values.</param>
        /// <returns>Whether the two dictionaries are equal.</returns>
        public static bool Equals1<TKey, TValue>([CanBeNull] this IDictionary<TKey, TValue> lhs, [CanBeNull] IDictionary<TKey, TValue> rhs, [NotNull] IEqualityComparer<TValue> valueEquality)
        {
            if (lhs is null)
                return rhs is null;
            if (rhs is null)
                return false;

            if (ReferenceEquals(lhs, rhs))
                return true;

            if (lhs.Count != rhs.Count)
                return false;

            foreach (TKey key in lhs.Keys)
            {
                if (!rhs.TryGetValue(key, out TValue rhsValue))
                    return false;

                if (!valueEquality.Equals(lhs[key], rhsValue))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Element-by-element dictionary equality using the default equality comparer for values.
        /// Two dictionaries are equal if they are both null, or are actually the same reference,
        /// or they have identical sets of keys and elements are equal for each key. 
        /// </summary>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="lhs">First dictionary.</param>
        /// <param name="rhs">Second dictionary.</param>
        /// <returns>Whether the two dictionaries are equal.</returns>
        public static bool Equals1<TKey, TValue>([CanBeNull] this IDictionary<TKey, TValue> lhs, [CanBeNull] IDictionary<TKey, TValue> rhs)
        {
            return Equals1(lhs, rhs, EqualityComparer<TValue>.Default);
        }

        #endregion
    }

    /// <summary>
    /// Equality comparer for array of <typeparamref name="T"/> using <see cref="EqualityExtensions.Equals1{T}(IList{T},IList{T},IEqualityComparer{T})"/> method.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    internal class ArrayEqualityComparer<T> : IEqualityComparer<T[]>
    {
        /// <summary>
        /// Element equality comparer.
        /// </summary>
        [NotNull]
        public IEqualityComparer<T> ElementEqualityComparer { get; }

        /// <summary>
        /// Constructor using default element equality comparer.
        /// </summary>
        public ArrayEqualityComparer()
            : this(EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Constructor using a given element equality comparer.
        /// </summary>
        /// <param name="elementEqualityComparer"></param>
        public ArrayEqualityComparer([NotNull] IEqualityComparer<T> elementEqualityComparer)
        {
            ElementEqualityComparer = elementEqualityComparer;
        }

        /// <inheritdoc />
        public bool Equals(T[] lhs, T[] rhs)
        {
            return lhs.Equals1(rhs, ElementEqualityComparer);
        }

        /// <inheritdoc />
        public int GetHashCode(T[] array)
        {
            var hashcode = 0;
            foreach (var elem in array)
            {
                hashcode ^= ElementEqualityComparer.GetHashCode(elem);
            }
            return hashcode;
        }
    }

    /// <summary>
    /// IEqualityComparer for floats with user-specified tolerance.
    /// </summary>
    public class FloatEqualityComparer : IEqualityComparer<float>
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
        /// Constructs FloatEqualityComparer with default tolerance.
        /// </summary>
        public FloatEqualityComparer()
            : this(DefaultTolerance)
        {
        }

        /// <summary>
        /// Constructs FloatEqualityComparer with user-given tolerance.
        /// </summary>
        /// <param name="tolerance">Tolerance.</param>
        public FloatEqualityComparer(float tolerance)
        {
            Tolerance = tolerance;
        }

        /// <inheritdoc />
        public bool Equals(float x, float y)
        {
            return Math.Abs(x - y) <= Tolerance;
        }

        /// <inheritdoc />
        public int GetHashCode(float x)
        {
            return x.GetHashCode();
        }
    }

    /// <summary>
    /// IEqualityComparer for doubles with user-specified tolerance.
    /// </summary>
    public class DoubleEqualityComparer : IEqualityComparer<double>
    {
        /// <summary>
        /// Default tolerance.
        /// </summary>
        public static double DefaultTolerance { get; } = 1e-5;

        /// <summary>
        /// Tolerance used during equality comparison.
        /// </summary>
        public double Tolerance { get; }

        /// <summary>
        /// Constructs DoubleEqualityComparer with default tolerance.
        /// </summary>
        public DoubleEqualityComparer()
            : this(DefaultTolerance)
        {
        }

        /// <summary>
        /// Constructs DoubleEqualityComparer with user-given tolerance.
        /// </summary>
        /// <param name="tolerance">Tolerance.</param>
        public DoubleEqualityComparer(double tolerance)
        {
            Tolerance = tolerance;
        }

        /// <inheritdoc />
        public bool Equals(double x, double y)
        {
            return Math.Abs(x - y) <= Tolerance;
        }

        /// <inheritdoc />
        public int GetHashCode(double x)
        {
            return x.GetHashCode();
        }
    }

    /// <summary>
    /// EqualityComparer that uses <see cref="object.ReferenceEquals"/> to compare items.
    /// </summary>
    /// <typeparam name="T">Type of items to compare.</typeparam>
    public class ReferenceEqualityComparer<T> : EqualityComparer<T>
    {
        /// <summary>
        /// Default singleton instance of this class.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "Yes, it's true that type T cannot be inferred while using this static instance, but in this case, it's ok.")]
        public static ReferenceEqualityComparer<T> Instance { get; } = new ReferenceEqualityComparer<T>();

        // Make constructor private so that clients must use Instance.
        private ReferenceEqualityComparer()
        {
        }

        /// <inheritdoc />
        public override bool Equals(T lhs, T rhs)
        {
            return ReferenceEquals(lhs, rhs);
        }

        /// <inheritdoc />
        public override int GetHashCode(T obj)
        {
            return obj?.GetHashCode() ?? 0;
        }
    }
}
