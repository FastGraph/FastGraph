#if SUPPORTS_CONTRACTS
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Contract related to <see cref="IEnumerable{T}"/>.
    /// </summary>
    internal static class EnumerableContract
    {
        [System.Diagnostics.Contracts.Pure]
        public static bool ElementsNotNull<T>(IEnumerable<T> elements)
        {
            Contract.Requires(elements != null);

#if DEBUG
            return elements.All(e => e != null);
#else
            return true;
#endif
        }

        [System.Diagnostics.Contracts.Pure]
        public static bool All(
            int lowerBound, 
            int exclusiveUpperBound,
            [NotNull, InstantHandle] Predicate<int> predicate)
        {
            for (int i = lowerBound; i < exclusiveUpperBound; ++i)
            {
                if (!predicate(i))
                    return false;
            }

            return true;
        }
    }
}
#endif