using System.Collections.Generic;
using NUnit.Framework;

namespace QuikGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="AlwaysTrueConditionExpression{TToken}"/>.
    /// </summary>
    internal class AlwaysTrueConditionExpressionTests
    {
        [Test]
        public void IsEnabled()
        {
            var expression = new AlwaysTrueConditionExpression<int>();

            var emptyTokens = new List<int>();
            Assert.IsTrue(expression.IsEnabled(emptyTokens));

            var tokens = new List<int> { 1, 5, 16 };
            Assert.IsTrue(expression.IsEnabled(tokens));
        }
    }
}