using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// The default struct based reversed <see cref="IEdge{TVertex}"/> implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay("{" + nameof(Source) + "}<-{" + nameof(Target) + "}")]
    public struct SReversedEdge<TVertex, TEdge> : IEdge<TVertex>, IEquatable<SReversedEdge<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SReversedEdge{TVertex, TEdge}"/> struct.
        /// </summary>
        /// <param name="originalEdge">Original edge.</param>
        public SReversedEdge([NotNull] TEdge originalEdge)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(originalEdge != null);
#endif

            OriginalEdge = originalEdge;
        }

        /// <summary>
        /// Original edge.
        /// </summary>
        [NotNull]
        public TEdge OriginalEdge { get; }

        /// <inheritdoc />
        public TVertex Source => OriginalEdge.Target;

        /// <inheritdoc />
        public TVertex Target => OriginalEdge.Source;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is SReversedEdge<TVertex, TEdge> reversedEdge
                   && Equals(reversedEdge);
        }

        /// <inheritdoc />
        public bool Equals(SReversedEdge<TVertex, TEdge> other)
        {
            return OriginalEdge.Equals(other.OriginalEdge);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return OriginalEdge.GetHashCode() ^ 16777619;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"R({OriginalEdge})";
        }
    }
}
