using JetBrains.Annotations;

namespace QuikGraph.Petri
{
    /// <summary>
    /// A node of a net, taken from the transition kind.
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>
    /// <remarks>Usually represented by a rectangle.</remarks>
    public interface ITransition<TToken> : IPetriVertex
    {
        /// <summary>
        /// A boolean expression associated with the transition.
        /// </summary>
        [NotNull]
        IConditionExpression<TToken> Condition { get; set; }
    }
}