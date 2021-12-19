#nullable enable

using NUnit.Framework;

namespace FastGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="Place{TToken}"/>.
    /// </summary>
    internal sealed class PlaceTests
    {
        [Test]
        public void Constructor()
        {
            var place = new Place<int>("MyPlace");
            place.Name.Should().Be("MyPlace");
            place.Marking.Should().BeEmpty();
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => new Place<int>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void ToStringWithMarking()
        {
            var place = new Place<int>("TestName");
            string expectedString = "P(TestName|0)";
            place.ToStringWithMarking().Should().Be(expectedString);

            place.Marking.Add(1);
            place.Marking.Add(3);
            place.Marking.Add(5);
            place.Marking.Add(2);
            expectedString =
                "P(TestName|4)" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                "\tInt32";
            place.ToStringWithMarking().Should().Be(expectedString);
        }

        [Test]
        public void ObjectToString()
        {
            var place = new Place<int>("TestName");
            place.ToString().Should().Be("P(TestName|0)");

            place = new Place<int>("OtherTestName");
            place.ToString().Should().Be("P(OtherTestName|0)");

            place = new Place<int>("TestName_1");
            place.Marking.Add(1);
            place.Marking.Add(3);
            place.Marking.Add(5);
            place.ToString().Should().Be("P(TestName_1|3)");
        }
    }
}
