#nullable enable

using NUnit.Framework;

namespace FastGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="Transition{TToken}"/>.
    /// </summary>
    internal sealed class TransitionTests
    {
        #region Test classes

        private class AlwaysFalseCondition : IConditionExpression<int>
        {
            public bool IsEnabled(IList<int> tokens)
            {
                return false;
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var transition = new Transition<int>("MyTransition");
            transition.Name.Should().Be("MyTransition");
            transition.Condition.Should().BeAssignableTo<AlwaysTrueConditionExpression<int>>();
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new Transition<int>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void Condition()
        {
            var transition = new Transition<int>("MyTransition");
            transition.Condition.Should().NotBeNull();

            var newCondition = new AlwaysFalseCondition();
            transition.Condition = newCondition;
            transition.Condition.Should().BeSameAs(newCondition);

            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => transition.Condition = default).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void ObjectToString()
        {
            var transition = new Transition<int>("TestName");
            transition.ToString().Should().Be("T(TestName)");

            transition = new Transition<int>("OtherTestName");
            transition.ToString().Should().Be("T(OtherTestName)");
        }
    }
}
