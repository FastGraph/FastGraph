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
            Assert.AreEqual("MyTransition", transition.Name);
            Assert.IsInstanceOf<AlwaysTrueConditionExpression<int>>(transition.Condition);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => new Transition<int>(default));
#pragma warning restore CS8625
        }

        [Test]
        public void Condition()
        {
            var transition = new Transition<int>("MyTransition");
            Assert.IsNotNull(transition.Condition);

            var newCondition = new AlwaysFalseCondition();
            transition.Condition = newCondition;
            Assert.AreSame(newCondition, transition.Condition);

            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(() => transition.Condition = default);
#pragma warning restore CS8625
        }

        [Test]
        public void ObjectToString()
        {
            var transition = new Transition<int>("TestName");
            Assert.AreEqual("T(TestName)", transition.ToString());

            transition = new Transition<int>("OtherTestName");
            Assert.AreEqual("T(OtherTestName)", transition.ToString());
        }
    }
}
