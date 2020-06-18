using System;
using NUnit.Framework;

namespace QuikGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="Place{TToken}"/>.
    /// </summary>
    internal class PlaceTests
    {
        [Test]
        public void Constructor()
        {
            var place = new Place<int>("MyPlace");
            Assert.AreEqual("MyPlace", place.Name);
            CollectionAssert.IsEmpty(place.Marking);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new Place<int>(null));
        }

        [Test]
        public void ToStringWithMarking()
        {
            var place = new Place<int>("TestName");
            string expectedString = "P(TestName|0)";
            Assert.AreEqual(expectedString, place.ToStringWithMarking());

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
            Assert.AreEqual(expectedString, place.ToStringWithMarking());
        }

        [Test]
        public void ObjectToString()
        {
            var place = new Place<int>("TestName");
            Assert.AreEqual("P(TestName|0)", place.ToString());

            place = new Place<int>("OtherTestName");
            Assert.AreEqual("P(OtherTestName|0)", place.ToString());

            place = new Place<int>("TestName_1");
            place.Marking.Add(1);
            place.Marking.Add(3);
            place.Marking.Add(5);
            Assert.AreEqual("P(TestName_1|3)", place.ToString());
        }
    }
}