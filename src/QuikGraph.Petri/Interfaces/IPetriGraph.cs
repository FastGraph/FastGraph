namespace QuikGraph.Petri
{
    /// <summary>
    /// Represents a Petri Graph (a mutable bidirectional directed graph).
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>
    public interface IPetriGraph<TToken> : IReadOnlyPetriGraph<TToken>, IMutableBidirectionalGraph<IPetriVertex, IArc<TToken>>
    {
    }
}