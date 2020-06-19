namespace QuikGraph.Petri
{
    /// <summary>
    /// Represents a read only Petri Graph (a bidirectional directed graph).
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>
    public interface IReadOnlyPetriGraph<TToken> : IBidirectionalGraph<IPetriVertex, IArc<TToken>>
    {
    }
}