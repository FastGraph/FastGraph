#nullable enable

using JetBrains.Annotations;

namespace FastGraph.Petri
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
        [ItemNotNull]
        IList<TToken> Evaluate([ItemNotNull] IList<TToken> markings);
    }
}
