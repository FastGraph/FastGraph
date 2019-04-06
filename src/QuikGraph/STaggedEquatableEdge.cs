using System;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using System.Runtime.InteropServices;

namespace QuickGraph
{
    /// <summary>
    /// A tagged edge as value type.
    /// </summary>
    /// <typeparam name="TVertex">type of the vertices</typeparam>
    /// <typeparam name="TTag"></typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay(EdgeExtensions.DebuggerDisplayTaggedEdgeFormatString)]
    public struct STaggedEquatableEdge<TVertex, TTag>
        : IEdge<TVertex>
        , IEquatable<STaggedEquatableEdge<TVertex, TTag>>
        , ITagged<TTag>
    {
        readonly TVertex source;
        readonly TVertex target;
        TTag tag;

        public STaggedEquatableEdge(TVertex source, TVertex target, TTag tag)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(source != null);
            Contract.Requires(target != null);
#endif

            this.source = source;
            this.target = target;
            this.tag = tag;
            this.TagChanged = null;
        }

        public TVertex Source
        {
            get { return this.source; }
        }

        public TVertex Target
        {
            get { return this.target; }
        }

        public event EventHandler TagChanged;

        void OnTagChanged(EventArgs e)
        {
            var eh = this.TagChanged;
            if (eh != null)
                eh(this, e);
        }

        public TTag Tag
        {
            get { return this.tag; }
            set
            {
                if (!object.Equals(this.tag, value))
                {
                    this.tag = value;
                    this.OnTagChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return String.Format(
                EdgeExtensions.EdgeFormatString,
                this.Source,
                this.Target);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(STaggedEquatableEdge<TVertex,TTag> other)
        {
#if SUPPORTS_CONTRACTS
            Contract.Ensures(
                Contract.Result<bool>() ==
                (this.Source.Equals(other.Source) &&
                this.Target.Equals(other.Target))
                );
#endif

            return
                this.source.Equals(other.source) &&
                this.target.Equals(other.target);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return
                obj is STaggedEquatableEdge<TVertex,TTag> &&
                this.Equals((STaggedEquatableEdge<TVertex, TTag>)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCodeHelper.Combine(
                this.source.GetHashCode(),
                this.target.GetHashCode());
        }
    }
}
