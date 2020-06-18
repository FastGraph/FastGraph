using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace QuikGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="Transition{TToken}"/>.
    /// </summary>
    internal class TransitionTests
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
            Assert.Throws<ArgumentNullException>(() => new Transition<int>(null));
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
            Assert.Throws<ArgumentNullException>(() => transition.Condition = null);
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