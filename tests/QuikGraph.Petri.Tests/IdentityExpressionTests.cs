using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace QuikGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="IdentityExpression{TToken}"/>.
    /// </summary>
    internal class IdentityExpressionTests
    {
        [Test]
        public void Evaluate()
        {
            var expression = new IdentityExpression<int>();

            var emptyMarkings = new List<int>();
            Assert.AreSame(emptyMarkings, expression.Evaluate(emptyMarkings));

            var markings = new List<int> { 1, 5, 16 };
            Assert.AreSame(markings, expression.Evaluate(markings));
        }

        [Test]
        public void Evaluate_Throws()
        {
            var expression = new IdentityExpression<int>();

            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => expression.Evaluate(null));
        }
    }
}