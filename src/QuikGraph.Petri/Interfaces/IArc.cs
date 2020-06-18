using JetBrains.Annotations;

namespace QuikGraph.Petri
{
    /// <summary>
    /// A directed edge of a net which may connect a <see cref="IPlace{TToken}"/>
    /// to a <see cref="ITransition{TToken}"/> or a <see cref="ITransition{TToken}"/> to
    /// a <see cref="IPlace{TToken}"/>.
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>
    /// <remarks>Usually represented by an arrow.</remarks>
    public interface IArc<TToken> : IEdge<IPetriVertex>
    {
        /// <summary>
        /// Gets a value indicating if the <see cref="IArc{TToken}"/>
        /// instance is an <strong>input arc</strong>.
        /// </summary>
        /// <remarks>
        /// An arc that leads from an input <see cref="IPlace{TToken}"/> to a
        /// <see cref="ITransition{TToken}"/> is called an <em>Input Arc</em> of
        /// the transition.
        /// </remarks>
        bool IsInputArc { get; }

        /// <summary>
        /// Gets the <see cref="IPlace{TToken}"/> instance attached to the <see cref="IArc{TToken}"/>.
        /// </summary>
        /// <value>
        /// The <see cref="IPlace{TToken}"/> attached to the <see cref="IArc{TToken}"/>.
        /// </value>
        [NotNull]
        IPlace<TToken> Place { get; }

        /// <summary>
        /// Gets or sets the <see cref="ITransition{TToken}"/> instance attached to the <see cref="IArc{TToken}"/>.
        /// </summary>
        /// <value>
        /// The <see cref="ITransition{TToken}"/> attached to the <see cref="IArc{TToken}"/>.
        /// </value>
        [NotNull]
        ITransition<TToken> Transition { get; }

        /// <summary>
        /// Gets or sets the arc annotation.
        /// </summary>
        /// <value>
        /// The <see cref="IExpression{TToken}"/> annotation instance.
        /// </value>
        /// <remarks>
        /// An expression that may involve constants, variables and operators
        /// used to annotate the arc. The expression evaluates over the type
        /// of the arc's associated place.
        /// </remarks>
        [NotNull]
        IExpression<TToken> Annotation { get; set; }
    }
}