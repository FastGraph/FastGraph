#if SUPPORTS_SERIALIZATION
using System;
#endif

namespace FastGraph.Petri
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    internal sealed class PetriGraph<TToken> : BidirectionalGraph<IPetriVertex, IArc<TToken>>, IPetriGraph<TToken>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PetriGraph{TToken}"/> class.
        /// </summary>
        public PetriGraph()
            : base(true)
        {
        }
    }
}