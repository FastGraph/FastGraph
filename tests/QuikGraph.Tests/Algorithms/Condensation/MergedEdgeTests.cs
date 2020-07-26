using System;
using NUnit.Framework;
using QuikGraph.Algorithms.Condensation;
using QuikGraph.Tests.Structures;

namespace QuikGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Tests for <see cref="MergedEdge{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal class MergedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new MergedEdge<int, Edge<int>>(1, 2), 1, 2);
            CheckEdge(new MergedEdge<int, Edge<int>>(2, 1), 2, 1);
            CheckEdge(new MergedEdge<int, Edge<int>>(1, 1), 1, 1);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new MergedEdge<TestVertex, Edge<TestVertex>>(v1, v2), v1, v2);
            CheckEdge(new MergedEdge<TestVertex, Edge<TestVertex>>(v2, v1), v2, v1);
            CheckEdge(new MergedEdge<TestVertex, Edge<TestVertex>>(v1, v1), v1, v1);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new MergedEdge<TestVertex, Edge<TestVertex>>(null, new TestVertex("v1")));
            Assert.Throws<ArgumentNullException>(() => new MergedEdge<TestVertex, Edge<TestVertex>>(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => new MergedEdge<TestVertex, Edge<TestVertex>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Edges()
        {
            var edge = new MergedEdge<int, Edge<int>>(1, 2);
            CollectionAssert.IsEmpty(edge.Edges);

            var subEdge1 = new Edge<int>(1, 2);
            edge.Edges.Add(subEdge1);
            CollectionAssert.AreEqual(new[] { subEdge1 }, edge.Edges);

            var subEdge2 = new MergedEdge<int, Edge<int>>(1, 2);
            edge.Edges.Add(subEdge2);
            CollectionAssert.AreEqual(new[] { subEdge1, subEdge2 }, edge.Edges);

            edge.Edges.RemoveAt(1);
            CollectionAssert.AreEqual(new[] { subEdge1 }, edge.Edges);

            edge.Edges.Remove(subEdge1);
            CollectionAssert.IsEmpty(edge.Edges);
        }

        [Test]
        public void Merge()
        {
            var emptyEdge1 = new MergedEdge<int, Edge<int>>(1, 2);
            var emptyEdge2 = new MergedEdge<int, Edge<int>>(1, 2);
            var subEdge1 = new Edge<int>(1, 2);
            var subEdge2 = new Edge<int>(1, 2);
            var subEdge3 = new Edge<int>(1, 2);
            var edge1 = new MergedEdge<int, Edge<int>>(1, 2);
            edge1.Edges.Add(subEdge1);
            var edge2 = new MergedEdge<int, Edge<int>>(1, 2);
            edge2.Edges.Add(subEdge2);
            edge2.Edges.Add(subEdge3);

            MergedEdge<int, Edge<int>> mergedEdge = MergedEdge.Merge(emptyEdge1, emptyEdge2);
            Assert.IsNotNull(mergedEdge);
            CollectionAssert.IsEmpty(mergedEdge.Edges);

            mergedEdge = MergedEdge.Merge(emptyEdge1, edge1);
            Assert.IsNotNull(mergedEdge);
            CollectionAssert.AreEqual(new[] { subEdge1 }, mergedEdge.Edges);

            mergedEdge = MergedEdge.Merge(edge1, emptyEdge1);
            Assert.IsNotNull(mergedEdge);
            CollectionAssert.AreEqual(new[] { subEdge1 }, mergedEdge.Edges);

            mergedEdge = MergedEdge.Merge(edge1, edge2);
            Assert.IsNotNull(mergedEdge);
            CollectionAssert.AreEqual(new[] { subEdge1, subEdge2, subEdge3 }, mergedEdge.Edges);
        }

        [Test]
        public void Merge_Throws()
        {
            var edge = new MergedEdge<int, Edge<int>>(1, 2);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => MergedEdge.Merge(edge, null));
            Assert.Throws<ArgumentNullException>(() => MergedEdge.Merge(null, edge));
            Assert.Throws<ArgumentNullException>(() => MergedEdge.Merge<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void Equals()
        {
            var edge1 = new MergedEdge<int, Edge<int>>(1, 2);
            var edge2 = new MergedEdge<int, Edge<int>>(1, 2);
            var edge3 = new MergedEdge<int, Edge<int>>(2, 1);
            var edge4 = new MergedEdge<int, Edge<int>>(1, 2);
            edge4.Edges.Add(edge1);

            Assert.AreEqual(edge1, edge1);
            Assert.AreNotEqual(edge1, edge2);
            Assert.AreNotEqual(edge1, edge3);
            Assert.AreNotEqual(edge1, edge4);

            Assert.AreNotEqual(edge1, null);
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new MergedEdge<int, Edge<int>>(1, 2);
            var edge2 = new MergedEdge<int, Edge<int>>(2, 1);

            Assert.AreEqual("1 -> 2", edge1.ToString());
            Assert.AreEqual("2 -> 1", edge2.ToString());
        }
    }
}