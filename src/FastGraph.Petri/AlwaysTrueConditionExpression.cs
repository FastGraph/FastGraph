#nullable enable

#if SUPPORTS_SERIALIZATION
#endif

namespace FastGraph.Petri
{
    /// <summary>
    /// Represents an always enabled condition.
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class AlwaysTrueConditionExpression<TToken> : IConditionExpression<TToken>
    {
        /// <inheritdoc />
        public bool IsEnabled(IList<TToken> tokens)
        {
            return true;
        }
    }
}
