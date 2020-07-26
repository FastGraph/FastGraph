namespace QuikGraph
{
    /// <summary>
    /// Helpers to work with hash codes.
    /// </summary>
    internal static class HashCodeHelpers
    {
        private const int Fnv1Prime32 = 16777619;
        private const int Fnv1Basis32 = unchecked((int)2166136261);

        private static int Fold(int hash, byte value)
        {
            return (hash * Fnv1Prime32) ^ value;
        }

        private static int Fold(int hash, int value)
        {
            return Fold(Fold(Fold(Fold(hash, (byte)value), 
                (byte)((uint)value >> 8)), 
                (byte)((uint)value >> 16)), 
                (byte)((uint)value >> 24));
        }

        /// <summary>
        /// Combines two hash codes in a strong way.
        /// </summary>
        /// <param name="x">First hash code.</param>
        /// <param name="y">Second hash code.</param>
        /// <returns>The combined hash code from two hash codes.</returns>
        public static int Combine(int x, int y)
        {
            return Fold(Fold(Fnv1Basis32, x), y);
        }

        /// <summary>
        /// Combines three hash codes in a strong way.
        /// </summary>
        /// <param name="x">First hash code.</param>
        /// <param name="y">Second hash code.</param>
        /// <param name="z">Third hash code.</param>
        /// <returns>The combined hash code from three hash codes.</returns>
        public static int Combine(int x, int y, int z)
        {
            return Fold(Combine(x, y), z);
        }

        /// <summary>
        /// Combines four hash codes in a strong way.
        /// </summary>
        /// <param name="x">First hash code.</param>
        /// <param name="y">Second hash code.</param>
        /// <param name="z">Third hash code.</param>
        /// <param name="w">Fourth hash code.</param>
        /// <returns>The combined hash code from four hash codes.</returns>
        public static int Combine(int x, int y, int z, int w)
        {
            return Fold(Combine(x, y, z), w);
        }
    }
}