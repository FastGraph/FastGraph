using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Predicates
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public sealed class InDictionaryVertexPredicate<TVertex, TValue>
    {
        private readonly IDictionary<TVertex, TValue> dictionary;

        public InDictionaryVertexPredicate(
            IDictionary<TVertex,TValue> dictionary)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(dictionary != null);
#endif

            this.dictionary = dictionary;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool Test(TVertex v)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(v != null);
#endif

            return this.dictionary.ContainsKey(v);
        }
    }
}
