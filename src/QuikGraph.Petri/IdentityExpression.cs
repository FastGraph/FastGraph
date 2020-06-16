using System;
using System.Collections.Generic;

namespace QuikGraph.Petri
{
    /// <summary>
    /// Identity evaluation expression.
    /// </summary>
    /// <typeparam name="TToken">Token type.</typeparam>
    public sealed class IdentityExpression<TToken> : IExpression<TToken>
    {
        /// <inheritdoc />
        public IList<TToken> Evaluate(IList<TToken> markings)
        {
            return markings ?? throw new ArgumentNullException(nameof(markings));
        }
    }
}