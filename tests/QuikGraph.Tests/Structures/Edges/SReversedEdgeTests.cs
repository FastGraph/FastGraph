using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="SReversedEdge{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal class SReversedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 2)), 2, 1);
            CheckEdge(new SReversedEdge<int, Edge<int>>(new Edge<int>(2, 1)), 1, 2);
            CheckEdge(new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 1)), 1, 1);

            // Struct break the contract with their implicit default constructor
            var defaultEdge = default(SReversedEdge<int, Edge<int>>);
            // ReSharper disable HeuristicUnreachableCode
            // Justification: Since struct has implicit default constructor it allows initialization of invalid edge
            Assert.IsNull(defaultEdge.OriginalEdge);
            // ReSharper disable  HeuristicUnreachableCode
            Assert.Throws<NullReferenceException>(() => { var _ = defaultEdge.Source; });
            Assert.Throws<NullReferenceException>(() => { var _ = defaultEdge.Target; });
            // ReSharper restore HeuristicUnreachableCode

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new SReversedEdge<TestVertex, Edge<TestVertex>>(new Edge<TestVertex>(v1, v2)), v2, v1);
            CheckEdge(new SReversedEdge<TestVertex, Edge<TestVertex>>(new Edge<TestVertex>(v2, v1)), v1, v2);
            CheckEdge(new SReversedEdge<TestVertex, Edge<TestVertex>>(new Edge<TestVertex>(v1, v1)), v1, v1);

            // Struct break the contract with their implicit default constructor
            var defaultEdge2 = default(SReversedEdge<TestVertex, Edge<TestVertex>>);
            // ReSharper disable HeuristicUnreachableCode
            // Justification: Since struct has implicit default constructor it allows initialization of invalid edge
            Assert.IsNull(defaultEdge2.OriginalEdge);
            // ReSharper disable  HeuristicUnreachableCode
            Assert.Throws<NullReferenceException>(() => { var _ = defaultEdge2.Source; });
            Assert.Throws<NullReferenceException>(() => { var _ = defaultEdge2.Target; });
            // ReSharper restore HeuristicUnreachableCode
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new SReversedEdge<TestVertex, Edge<TestVertex>>(null));
        }

        [Test]
        public void Equals()
        {
            var edge1 = new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 2));
            var edge2 = new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 2));
            var edge3 = new SReversedEdge<int, Edge<int>>(new Edge<int>(2, 1));

            Assert.AreEqual(edge1, edge1);
            Assert.AreNotEqual(edge1, edge2);
            Assert.AreNotEqual(edge1, edge3);

            Assert.AreNotEqual(edge1, null);
        }

        [Test]
        public void Equals2()
        {
            var edge1 = new SReversedEdge<int, EquatableEdge<int>>(new EquatableEdge<int>(1, 2));
            var edge2 = new SReversedEdge<int, EquatableEdge<int>>(new EquatableEdge<int>(1, 2));
            var edge3 = new SReversedEdge<int, EquatableEdge<int>>(new EquatableEdge<int>(2, 1));

            Assert.AreEqual(edge1, edge1);
            Assert.AreEqual(edge1, edge2);
            Assert.AreNotEqual(edge1, edge3);

            Assert.AreNotEqual(edge1, null);
        }
    }
}
