﻿#nullable enable

namespace FastGraph.Petri
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
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        IPlace<TToken> AddPlace(string name);

        /// <summary>
        /// Adds a <see cref="ITransition{TToken}"/> with given <paramref name="name"/> to this Petri net.
        /// </summary>
        /// <param name="name">Transition name.</param>
        /// <returns>Added <see cref="ITransition{TToken}"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        ITransition<TToken> AddTransition(string name);

        /// <summary>
        /// Adds an <see cref="IArc{TToken}"/> with given source <paramref name="place"/> and target <paramref name="transition"/> to this Petri net.
        /// </summary>
        /// <param name="place">Source place.</param>
        /// <param name="transition">Target transition.</param>
        /// <returns>Added <see cref="IArc{TToken}"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="place"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="transition"/> is <see langword="null"/>.</exception>
        IArc<TToken> AddArc(IPlace<TToken> place, ITransition<TToken> transition);

        /// <summary>
        /// Adds an <see cref="IArc{TToken}"/> with given source <paramref name="transition"/> and target <paramref name="place"/> to this Petri net.
        /// </summary>
        /// <param name="transition">Source transition.</param>
        /// <param name="place">Target place.</param>
        /// <returns>Added <see cref="IArc{TToken}"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="transition"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="place"/> is <see langword="null"/>.</exception>
        IArc<TToken> AddArc(ITransition<TToken> transition, IPlace<TToken> place);
    }
}
