#nullable enable

namespace FastGraph.Petri
{
    /// <summary>
    /// A High Level Petri Graph.
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>
    /// <remarks>
    /// This object is called a Petri Net in honor of Petri's work. In fact,
    /// it should be named High Level Petri Graph.
    /// </remarks>
    public interface IPetriNet<TToken>
    {
        /// <summary>
        /// Gets a collection of <see cref="IPlace{TToken}"/> instances.
        /// </summary>
        /// <value>
        /// A collection of <see cref="IPlace{TToken}"/> instances.
        /// </value>
        IEnumerable<IPlace<TToken>> Places { get; }

        /// <summary>
        /// Gets a collection of <see cref="ITransition{TToken}"/> instances.
        /// </summary>
        /// <value>
        /// A collection of <see cref="ITransition{TToken}"/> instances.
        /// </value>
        IEnumerable<ITransition<TToken>> Transitions { get; }

        /// <summary>
        /// Gets a collection of <see cref="IArc{TToken}"/> instances.
        /// </summary>
        /// <value>
        /// A collection of <see cref="IArc{TToken}"/> instances.
        /// </value>
        IEnumerable<IArc<TToken>> Arcs { get; }

        /// <summary>
        /// Gets the Petri Graph.
        /// </summary>
        IReadOnlyPetriGraph<TToken> Graph { get; }
    }
}
