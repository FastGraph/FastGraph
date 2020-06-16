using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Petri
{
    /// <summary>
    /// A place in the HLPN framework.
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>
    /// <remarks>
    /// <para>
    /// A <see cref="Place{TToken}"/> is characterized by a set of tokens, called the <see cref="Marking"/> of the place.
    /// </para>
    /// <para>
    /// Usually represented by an ellipses (often circles).
    /// </para>
    /// </remarks>
    public interface IPlace<TToken> : IPetriVertex
    {
        /// <summary>
        /// Set of tokens (marking of the place).
        /// </summary>
        [NotNull, ItemNotNull]
        IList<TToken> Marking { get; }

        /// <summary>
        /// Converts this <see cref="IPlace{TToken}"/> to string (includes <see cref="Marking"/>).
        /// </summary>
        [Pure]
        [NotNull]
        string ToStringWithMarking();
    }
}