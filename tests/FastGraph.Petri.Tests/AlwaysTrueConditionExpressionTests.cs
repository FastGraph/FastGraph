#nullable enable

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
            expression.IsEnabled(emptyTokens).Should().BeTrue();

            var tokens = new List<int> { 1, 5, 16 };
            expression.IsEnabled(tokens).Should().BeTrue();
        }
    }
}
