#nullable enable

using NUnit.Framework;

namespace FastGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="Arc{TToken}"/>.
    /// </summary>
    internal sealed class ArcTests
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
            arc.IsInputArc.Should().BeTrue();
            arc.Source.Should().BeSameAs(place);
            arc.Place.Should().BeSameAs(place);
            arc.Target.Should().BeSameAs(transition);
            arc.Transition.Should().BeSameAs(transition);
            arc.Annotation.Should().NotBeNull();

            arc = new Arc<int>(transition, place);
            arc.IsInputArc.Should().BeFalse();
            arc.Source.Should().BeSameAs(place);
            arc.Place.Should().BeSameAs(place);
            arc.Target.Should().BeSameAs(transition);
            arc.Transition.Should().BeSameAs(transition);
            arc.Annotation.Should().NotBeNull();
        }

        [Test]
        public void Constructor_Throws()
        {
            var place = new TestPlace();
            var transition = new TestTransition();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new Arc<int>(default, transition)).Should().Throw<ArgumentNullException>();
            Invoking(() => new Arc<int>(place, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new Arc<int>((IPlace<int>?)default, default)).Should().Throw<ArgumentNullException>();

            Invoking(() => new Arc<int>(default, place)).Should().Throw<ArgumentNullException>();
            Invoking(() => new Arc<int>(transition, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new Arc<int>((ITransition<int>?)default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void ObjectToString()
        {
            var place = new TestPlace();
            var transition = new TestTransition();

            var arc = new Arc<int>(place, transition);
            arc.ToString().Should().Be("PlaceName -> TransitionName");

            arc = new Arc<int>(transition, place);
            arc.ToString().Should().Be("TransitionName -> PlaceName");
        }
    }
}
