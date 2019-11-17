#if !SUPPORTS_SORTEDSET
using System.Security;
using JetBrains.Annotations;

namespace QuikGraph.Utils
{
    internal unsafe class BitHelper // Should not be serialized
    {
        private const byte MarkedBitFlag = 1;
        private const byte IntSize = 32;

        // _length of underlying int array (not logical bit array)
        private readonly int _length;

        // Ptr to stack alloc'd array of ints
        [SecurityCritical]
        private readonly int* _arrayPtr;

        // Array of ints
        private readonly int[] _array;

        // Whether to operate on stack alloc'd or heap alloc'd array 
        private readonly bool _useStackAlloc;

        /// <summary>
        /// Instantiates a <see cref="BitHelper"/> with a heap alloc'd array of ints.
        /// </summary>
        /// <param name="bitArrayPtr">int array to hold bits.</param>
        /// <param name="length">length of int array.</param>
        [SecurityCritical]
        internal BitHelper(int* bitArrayPtr, int length)
        {
            _arrayPtr = bitArrayPtr;
            _length = length;
            _useStackAlloc = true;
        }

        /// <summary>
        /// Instantiates a <see cref="BitHelper"/> with a heap alloc'd array of ints.
        /// </summary>
        /// <param name="bitArray">int array to hold bits</param>
        /// <param name="length">length of int array</param>
        internal BitHelper([NotNull] int[] bitArray, int length)
        {
            _array = bitArray;
            _length = length;
        }

        /// <summary>
        /// Mark bit at specified position.
        /// </summary>
        [SecurityCritical]
        internal void MarkBit(int bitPosition)
        {
            if (_useStackAlloc)
            {
                int bitArrayIndex = bitPosition / IntSize;
                if (bitArrayIndex < _length && bitArrayIndex >= 0)
                {
                    _arrayPtr[bitArrayIndex] |= (MarkedBitFlag << (bitPosition % IntSize));
                }
            }
            else
            {
                int bitArrayIndex = bitPosition / IntSize;
                if (bitArrayIndex < _length && bitArrayIndex >= 0)
                {
                    _array[bitArrayIndex] |= (MarkedBitFlag << (bitPosition % IntSize));
                }
            }
        }

        /// <summary>
        /// Is bit at specified position marked?
        /// </summary>
        [SecurityCritical]
        internal bool IsMarked(int bitPosition)
        {
            if (_useStackAlloc)
            {
                int bitArrayIndex = bitPosition / IntSize;
                if (bitArrayIndex < _length && bitArrayIndex >= 0)
                {
                    return ((_arrayPtr[bitArrayIndex] & (MarkedBitFlag << (bitPosition % IntSize))) != 0);
                }
                return false;
            }
            else
            {
                int bitArrayIndex = bitPosition / IntSize;
                if (bitArrayIndex < _length && bitArrayIndex >= 0)
                {
                    return ((_array[bitArrayIndex] & (MarkedBitFlag << (bitPosition % IntSize))) != 0);
                }
                return false;
            }
        }

        /// <summary>
        /// How many ints must be allocated to represent n bits. Returns (n+31)/32, but
        /// avoids overflow.
        /// </summary>
        internal static int ToIntArrayLength(int n)
        {
            return n > 0 ? ((n - 1) / IntSize + 1) : 0;
        }
    }
}
#endif