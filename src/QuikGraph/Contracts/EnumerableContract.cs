using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Linq;

namespace QuickGraph
{
    public static class EnumerableContract
    {
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public static bool ElementsNotNull<T>(IEnumerable<T> elements)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(elements != null);
#endif

#if DEBUG

            return Enumerable.All(elements, e => e != null);
#else
            return true;
#endif
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public static bool All(int lowerBound, int exclusiveUpperBound, Func<int, bool> predicate)
        {
            for (int i = lowerBound; i < exclusiveUpperBound; i++)
            {
                if (!predicate(i))
                    return false;
            }
            return true;
        }
    }
}
