#nullable enable

using NUnit.Framework;

namespace FastGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="IdentityExpression{TToken}"/>.
    /// </summary>
    internal sealed class IdentityExpressionTests
    {
        [Test]
        public void Evaluate()
        {
            var expression = new IdentityExpression<int>();

            var emptyMarkings = new List<int>();
            expression.Evaluate(emptyMarkings).Should().BeSameAs(emptyMarkings);

            var markings = new List<int> { 1, 5, 16 };
            expression.Evaluate(markings).Should().BeSameAs(markings);
        }

        [Test]
        public void Evaluate_Throws()
        {
            var expression = new IdentityExpression<int>();

            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => expression.Evaluate(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }
    }
}
