using System;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Runtime.InteropServices;

namespace QuikGraph
{
    /// <summary>
    /// A reversed edge
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TEdge">type of the edges</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay("{Source}<-{Target}")]
    public struct SReversedEdge<TVertex, TEdge> : IEdge<TVertex>, IEquatable<SReversedEdge<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly TEdge originalEdge;
        public SReversedEdge(TEdge originalEdge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(originalEdge != null);
#endif

            this.originalEdge = originalEdge;
        }

        public TEdge OriginalEdge
        {
            get { return this.originalEdge; }
        }

        public TVertex Source
        {
            get { return this.OriginalEdge.Target; }
        }

        public TVertex Target
        {
            get { return this.OriginalEdge.Source; }
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public override bool Equals(object obj)
        {
            if (!(obj is SReversedEdge<TVertex, TEdge>))
                return false;

            return Equals((SReversedEdge<TVertex, TEdge>)obj);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public override int GetHashCode()
        {
            return this.OriginalEdge.GetHashCode() ^ 16777619;
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public override string ToString()
        {
            return String.Format("R({0})", this.OriginalEdge);
        }

#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public bool Equals(SReversedEdge<TVertex, TEdge> other)
        {
            return this.OriginalEdge.Equals(other.OriginalEdge);
        }
    }
}
