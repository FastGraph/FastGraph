using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Petri
{
    /// <summary>
    /// Represents an expression evaluating tokens (markings).
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>
    public interface IExpression<TToken>
    {
        /// <summary>
        /// Evaluates <paramref name="markings"/>.
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="markings"/> is <see langword="null"/>.</exception>
        [NotNull, ItemNotNull]
        IList<TToken> Evaluate([NotNull, ItemNotNull] IList<TToken> markings);
    }
}