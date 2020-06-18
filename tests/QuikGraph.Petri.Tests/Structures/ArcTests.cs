using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace QuikGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="Arc{TToken}"/>.
    /// </summary>
    internal class ArcTests
    {
        #region Test classes

        private class TestPlace : IPlace<int>
        {
            public string Name => "PlaceName";
            public IList<int> Marking { get; } = new List<int>();
            public string ToStringWithMarking()
            {
                return string.Empty;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private class TestTransition : ITransition<int>
        {
            public string Name => "TransitionName";
            public IConditionExpression<int> Condition { get; set; } = new AlwaysTrueConditionExpression<int>();

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var place = new TestPlace();
            var transition = new TestTransition();
            
            var arc = new Arc<int>(place, transition);
            Assert.IsTrue(arc.IsInputArc);
            Assert.AreSame(place, arc.Source);
            Assert.AreSame(place, arc.Place);
            Assert.AreSame(transition, arc.Target);
            Assert.AreSame(transition, arc.Transition);
            Assert.IsNotNull(arc.Annotation);

            arc = new Arc<int>(transition, place);
            Assert.IsFalse(arc.IsInputArc);
            Assert.AreSame(place, arc.Source);
            Assert.AreSame(place, arc.Place);
            Assert.AreSame(transition, arc.Target);
            Assert.AreSame(transition, arc.Transition);
            Assert.IsNotNull(arc.Annotation);
        }

        [Test]
        public void Constructor_Throws()
        {
            var place = new TestPlace();
            var transition = new TestTransition();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new Arc<int>(null, transition));
            Assert.Throws<ArgumentNullException>(() => new Arc<int>(place, null));
            Assert.Throws<ArgumentNullException>(() => new Arc<int>((IPlace<int>)null, null));

            Assert.Throws<ArgumentNullException>(() => new Arc<int>(null, place));
            Assert.Throws<ArgumentNullException>(() => new Arc<int>(transition, null));
            Assert.Throws<ArgumentNullException>(() => new Arc<int>((ITransition<int>)null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void ObjectToString()
        {
            var place = new TestPlace();
            var transition = new TestTransition();

            var arc = new Arc<int>(place, transition);
            Assert.AreEqual("PlaceName -> TransitionName", arc.ToString());

            arc = new Arc<int>(transition, place);
            Assert.AreEqual("TransitionName -> PlaceName", arc.ToString());
        }
    }
}