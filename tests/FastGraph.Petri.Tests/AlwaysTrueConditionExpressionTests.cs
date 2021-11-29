using System.Collections.Generic;
using NUnit.Framework;

namespace FastGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="AlwaysTrueConditionExpression{TToken}"/>.
    /// </summary>
    internal sealed class AlwaysTrueConditionExpressionTests
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