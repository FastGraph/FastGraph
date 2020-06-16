using JetBrains.Annotations;

namespace QuikGraph.Petri
{
    /// <summary>
    /// A mutable High Level Petri Graph.
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>
    public interface IMutablePetriNet<TToken> : IPetriNet<TToken>
    {
        /// <summary>
        /// Adds a <see cref="IPlace{TToken}"/> with given <paramref name="name"/> to this Petri net.
        /// </summary>
        /// <param name="name">Place name.</param>
        /// <returns>Added <see cref="IPlace{TToken}"/>.</returns>
        [NotNull]
        IPlace<TToken> AddPlace([NotNull] string name);
        
        /// <summary>
        /// Adds a <see cref="ITransition{TToken}"/> with given <paramref name="name"/> to this Petri net.
        /// </summary>
        /// <param name="name">Transition name.</param>
        /// <returns>Added <see cref="ITransition{TToken}"/>.</returns>
        [NotNull]
        ITransition<TToken> AddTransition([NotNull] string name);
        
        /// <summary>
        /// Adds an <see cref="IArc{TToken}"/> with given source <paramref name="place"/> and target <paramref name="transition"/> to this Petri net.
        /// </summary>
        /// <param name="place">Source place.</param>
        /// <param name="transition">Target transition.</param>
        /// <returns>Added <see cref="IArc{TToken}"/>.</returns>
        [NotNull]
        IArc<TToken> AddArc([NotNull] IPlace<TToken> place, [NotNull] ITransition<TToken> transition);
        
        /// <summary>
        /// Adds an <see cref="IArc{TToken}"/> with given source <paramref name="transition"/> and target <paramref name="place"/> to this Petri net.
        /// </summary>
        /// <param name="transition">Source transition.</param>
        /// <param name="place">Target place.</param>
        /// <returns>Added <see cref="IArc{TToken}"/>.</returns>
        [NotNull]
        IArc<TToken> AddArc([NotNull] ITransition<TToken> transition, [NotNull] IPlace<TToken> place);
    }
}