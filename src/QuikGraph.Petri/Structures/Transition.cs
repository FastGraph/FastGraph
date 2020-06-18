using System;
using JetBrains.Annotations;

namespace QuikGraph.Petri
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    internal sealed class Transition<TToken> : ITransition<TToken>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transition{Token}"/> class.
        /// </summary>
        /// <param name="name">Transition name.</param>
        public Transition([NotNull] string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <inheritdoc />
        public string Name { get; }

        [NotNull]
        private IConditionExpression<TToken> _condition = new AlwaysTrueConditionExpression<TToken>();

        /// <inheritdoc />
        public IConditionExpression<TToken> Condition
        {
            get => _condition;
            set => _condition = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"T({Name})";
        }
    }
}