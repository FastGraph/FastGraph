#if SUPPORTS_CONTRACTS
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace QuikGraph
{
    /// <summary>
    /// Contract related to <see cref="IEnumerable{T}"/>.
    /// </summary>
    internal static class EnumerableContract
    {
        [Pure]
        public static bool ElementsNotNull<T>(IEnumerable<T> elements)
        {
            Contract.Requires(elements != null);

#if DEBUG
            return elements.All(e => e != null);
#else
            return true;
#endif
        }
    }
}
#endif