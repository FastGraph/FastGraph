#nullable enable

using NUnit.Framework;
using static FastGraph.Tests.GraphTestHelpers;

namespace FastGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="PetriNet{TToken}"/>.
    /// </summary>
    internal sealed class PetriNetTests
    {
        [Test]
        public void Constructor()
        {
            var net = new PetriNet<int>();
            net.Places.Should().BeEmpty();
            net.Transitions.Should().BeEmpty();
            net.Arcs.Should().BeEmpty();
            net.Graph.Should().NotBeNull();
            AssertEmptyGraph(net.Graph);
        }

        [Test]
        public void MutableNetContent()
        {
            var net = new PetriNet<int>();
            var place1 = net.AddPlace("P1");
            net.Places.Should().BeEquivalentTo(new[] { place1 });
            net.Transitions.Should().BeEmpty();
            net.Arcs.Should().BeEmpty();
            AssertHasVertices(net.Graph, new[] { place1 });
            AssertNoEdge(net.Graph);

            var transition1 = net.AddTransition("T1");
            net.Places.Should().BeEquivalentTo(new[] { place1 });
            net.Transitions.Should().BeEquivalentTo(new[] { transition1 });
            net.Arcs.Should().BeEmpty();
            AssertHasVertices(net.Graph, new IPetriVertex[] { place1, transition1 });
            AssertNoEdge(net.Graph);

            var place2 = net.AddPlace("P2");
            var transition2 = net.AddTransition("T2");
            var place3 = net.AddPlace("P3");
            net.Places.Should().BeEquivalentTo(new[] { place1, place2, place3 });
            net.Transitions.Should().BeEquivalentTo(new[] { transition1, transition2 });
            net.Arcs.Should().BeEmpty();
            AssertHasVertices(net.Graph, new IPetriVertex[] { place1, place2, place3, transition1, transition2 });
            AssertNoEdge(net.Graph);

            var arc1 = net.AddArc(place1, transition1);
            net.Places.Should().BeEquivalentTo(new[] { place1, place2, place3 });
            net.Transitions.Should().BeEquivalentTo(new[] { transition1, transition2 });
            net.Arcs.Should().BeEquivalentTo(new[] { arc1 });
            AssertHasVertices(net.Graph, new IPetriVertex[] { place1, place2, place3, transition1, transition2 });
            AssertHasEdges(net.Graph, new[] { arc1 });

            var arc2 = net.AddArc(transition2, place3);
            var arc3 = net.AddArc(place2, transition2);
            net.Places.Should().BeEquivalentTo(new[] { place1, place2, place3 });
            net.Transitions.Should().BeEquivalentTo(new[] { transition1, transition2 });
            net.Arcs.Should().BeEquivalentTo(new[] { arc1, arc2, arc3 });
            AssertHasVertices(net.Graph, new IPetriVertex[] { place1, place2, place3, transition1, transition2 });
            AssertHasEdges(net.Graph, new[] { arc1, arc2, arc3 });
        }

        [Test]
        public void Clone()
        {
            var net = new PetriNet<int>();
            AssertEmpty(net);

            var clonedNet = net.Clone();
            clonedNet.Should().NotBeNull();
            AssertEmpty(clonedNet);

            clonedNet = (PetriNet<int>)((ICloneable)net).Clone();
            clonedNet.Should().NotBeNull();
            AssertEmpty(clonedNet);

            var place1 = net.AddPlace("p1");
            var place2 = net.AddPlace("p2");
            var transition1 = net.AddTransition("t1");
            AssertHasVertices(net.Graph, new IPetriVertex[] { place1, place2, transition1 });
            AssertNoEdge(net.Graph);

            clonedNet = net.Clone();
            clonedNet.Should().NotBeNull();
            AssertAreEqual(net, clonedNet);

            clonedNet = (PetriNet<int>)((ICloneable)net).Clone();
            clonedNet.Should().NotBeNull();
            AssertAreEqual(net, clonedNet);

            var place3 = net.AddPlace("p3");
            var transition2 = net.AddTransition("t2");
            var arc1 = net.AddArc(place1, transition1);
            var arc2 = net.AddArc(place2, transition1);
            var arc3 = net.AddArc(place3, transition2);
            var arc4 = net.AddArc(transition1, place3);
            var arc5 = net.AddArc(transition2, place1);
            AssertHasVertices(net.Graph, new IPetriVertex[] { place1, place2, place3, transition1, transition2 });
            AssertHasEdges(net.Graph, new[] { arc1, arc2, arc3, arc4, arc5 });

            clonedNet = net.Clone();
            clonedNet.Should().NotBeNull();
            AssertAreEqual(net, clonedNet);

            clonedNet = (PetriNet<int>)((ICloneable)net).Clone();
            clonedNet.Should().NotBeNull();
            AssertAreEqual(net, clonedNet);

            var place4 = net.AddPlace("p4");
            var transition3 = net.AddTransition("t3");
            AssertHasVertices(net.Graph, new IPetriVertex[] { place1, place2, place3, place4, transition1, transition2, transition3 });
            AssertHasEdges(net.Graph, new[] { arc1, arc2, arc3, arc4, arc5 });

            clonedNet = net.Clone();
            clonedNet.Should().NotBeNull();
            AssertAreEqual(net, clonedNet);

            clonedNet = (PetriNet<int>)((ICloneable)net).Clone();
            clonedNet.Should().NotBeNull();
            AssertAreEqual(net, clonedNet);

            #region Local function

            void AssertEmpty<TToken>(PetriNet<TToken> n)
            {
                n.Places.Should().BeEmpty();
                n.Transitions.Should().BeEmpty();
                n.Arcs.Should().BeEmpty();
                AssertEmptyGraph(n.Graph);
            }

            void AssertAreEqual<TToken>(PetriNet<TToken> expected, PetriNet<TToken> actual)
            {
                actual.Places.Should().BeEquivalentTo(expected.Places);
                actual.Transitions.Should().BeEquivalentTo(expected.Transitions);
                actual.Arcs.Should().BeEquivalentTo(expected.Arcs);
                AssertEquivalentGraphs(expected.Graph, actual.Graph);
            }

            #endregion
        }

        [Test]
        public void ObjectToString()
        {
            var net = new PetriNet<int>();
            string expectedString =
                "-----------------------------------------------" + Environment.NewLine +
                "Places (0)" + Environment.NewLine +
                "Transitions (0)" + Environment.NewLine +
                "Arcs" + Environment.NewLine;
            net.ToString().Should().Be(expectedString);

            var place1 = net.AddPlace("TestPlace");
            expectedString =
                "-----------------------------------------------" + Environment.NewLine +
                "Places (1)" + Environment.NewLine +
                "\tP(TestPlace|0)" + Environment.NewLine +
                Environment.NewLine +
                "Transitions (0)" + Environment.NewLine +
                "Arcs" + Environment.NewLine;
            net.ToString().Should().Be(expectedString);

            place1.Marking.Add(1);
            place1.Marking.Add(5);
            var place2 = net.AddPlace("TestPlace2");
            expectedString =
                "-----------------------------------------------" + Environment.NewLine +
                "Places (2)" + Environment.NewLine +
                "\tP(TestPlace|2)" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                Environment.NewLine +
                "\tP(TestPlace2|0)" + Environment.NewLine +
                Environment.NewLine +
                "Transitions (0)" + Environment.NewLine +
                "Arcs" + Environment.NewLine;
            net.ToString().Should().Be(expectedString);

            var transition = net.AddTransition("Transition");
            expectedString =
                "-----------------------------------------------" + Environment.NewLine +
                "Places (2)" + Environment.NewLine +
                "\tP(TestPlace|2)" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                Environment.NewLine +
                "\tP(TestPlace2|0)" + Environment.NewLine +
                Environment.NewLine +
                "Transitions (1)" + Environment.NewLine +
                "\tT(Transition)" + Environment.NewLine +
                Environment.NewLine +
                "Arcs" + Environment.NewLine;
            net.ToString().Should().Be(expectedString);

            net.AddArc(place1, transition);
            net.AddArc(transition, place2);
            expectedString =
                "-----------------------------------------------" + Environment.NewLine +
                "Places (2)" + Environment.NewLine +
                "\tP(TestPlace|2)" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                Environment.NewLine +
                "\tP(TestPlace2|0)" + Environment.NewLine +
                Environment.NewLine +
                "Transitions (1)" + Environment.NewLine +
                "\tT(Transition)" + Environment.NewLine +
                Environment.NewLine +
                "Arcs" + Environment.NewLine +
                "\tP(TestPlace|2) -> T(Transition)" + Environment.NewLine +
                "\tT(Transition) -> P(TestPlace2|0)" + Environment.NewLine;
            net.ToString().Should().Be(expectedString);
        }
    }
}
