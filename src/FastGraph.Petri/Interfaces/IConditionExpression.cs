#nullable enable

using JetBrains.Annotations;

namespace FastGraph.Petri
{
    /// <summary>
    /// Represents a conditional expression evaluating tokens.
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>
    public interface IConditionExpression<TToken>
    {
        /// <summary>
        /// Checks if the condition is true (enabled), or not.
        /// </summary>
        [Pure]
        bool IsEnabled([ItemNotNull] IList<TToken> tokens);
    }
}
