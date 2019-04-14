using System;
using System.Diagnostics;

namespace QuikGraph
{
    /// <summary>
    /// An equatable edge implementation
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}->{" + nameof(Target) + "}")]
    public class EquatableEdge<TVertex> : Edge<TVertex>, IEquatable<EquatableEdge<TVertex>>
    {
        public EquatableEdge(TVertex source, TVertex target)
            : base(source, target)
        {
        }

        /// <inheritdoc />
        public bool Equals(EquatableEdge<TVertex> other)
        {
            if (other is null)
                return false;
            return Source.Equals(other.Source) 
                   && Target.Equals(other.Target);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as EquatableEdge<TVertex>);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCodeHelpers.Combine(Source.GetHashCode(), Target.GetHashCode());
        }
    }
}
