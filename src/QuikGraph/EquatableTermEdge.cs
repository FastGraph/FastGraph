using System;
using System.Diagnostics;

namespace QuickGraph
{
    /// <summary>
    /// An equatable term edge implementation.
    /// </summary>
    /// <typeparam name="TVertex">Type of the vertices.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}->{" + nameof(Target) + "}")]
    public class EquatableTermEdge<TVertex> : TermEdge<TVertex>, IEquatable<EquatableTermEdge<TVertex>>
    {
        public EquatableTermEdge(TVertex source, TVertex target, int sourceTerminal, int targetTerminal)
            : base(source, target, sourceTerminal, targetTerminal)
        {
        }

        public EquatableTermEdge(TVertex source, TVertex target)
            : base(source, target)
        {
        }

        /// <inheritdoc />
        public bool Equals(EquatableTermEdge<TVertex> other)
        {
            if (other is null)
                return false;
            return Source.Equals(other.Source)
                   && Target.Equals(other.Target)
                   && SourceTerminal.Equals(other.SourceTerminal)
                   && TargetTerminal.Equals(other.TargetTerminal);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as EquatableTermEdge<TVertex>);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCodeHelper.Combine(
                Source.GetHashCode(),
                Target.GetHashCode(),
                SourceTerminal.GetHashCode(),
                TargetTerminal.GetHashCode());
        }
    }
}
