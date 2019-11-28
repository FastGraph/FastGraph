using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace QuikGraph.Tests.Extensions
{
    /// <summary>
    /// Tests related to <see cref="EdgeExtensions"/>.
    /// </summary>
    internal class EdgeExtensionsTests
    {
        [Test]
        public void IsSelfEdge()
        {
            var edge1 = new Edge<int>(1, 1);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(2, 1);

            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            var edge4 = new Edge<TestVertex>(v1, v1);
            var edge5 = new Edge<TestVertex>(v1, v2);
            var edge6 = new Edge<TestVertex>(v2, v1);

            Assert.IsTrue(edge1.IsSelfEdge());
            Assert.IsFalse(edge2.IsSelfEdge());
            Assert.IsFalse(edge3.IsSelfEdge());
            Assert.IsTrue(edge4.IsSelfEdge());
            Assert.IsFalse(edge5.IsSelfEdge());
            Assert.IsFalse(edge6.IsSelfEdge());

            // Edges cases
            var v1Bis = new TestVertex("1");
            var edge7 = new Edge<TestVertex>(v1, v1Bis);
            Assert.IsFalse(edge7.IsSelfEdge());

            var equatableV1 = new EquatableTestVertex("1");
            var equatableV1Bis = new EquatableTestVertex("1");
            var edge8 = new Edge<EquatableTestVertex>(equatableV1, equatableV1Bis);
            Assert.IsTrue(edge8.IsSelfEdge());
        }

        [Test]
        public void IsSelfEdge_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => ((Edge<int>)null).IsSelfEdge());
        }

        [Test]
        public void GetOtherVertex()
        {
            var edge1 = new Edge<int>(1, 1);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(2, 1);

            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            var edge4 = new Edge<TestVertex>(v1, v1);
            var edge5 = new Edge<TestVertex>(v1, v2);
            var edge6 = new Edge<TestVertex>(v2, v1);

            Assert.AreEqual(1, edge1.GetOtherVertex(1));
            Assert.AreEqual(2, edge2.GetOtherVertex(1));
            Assert.AreEqual(1, edge2.GetOtherVertex(2));
            Assert.AreEqual(2, edge3.GetOtherVertex(1));
            Assert.AreEqual(1, edge3.GetOtherVertex(2));

            Assert.AreSame(v1, edge4.GetOtherVertex(v1));
            Assert.AreSame(v2, edge5.GetOtherVertex(v1));
            Assert.AreSame(v1, edge5.GetOtherVertex(v2));
            Assert.AreSame(v2, edge6.GetOtherVertex(v1));
            Assert.AreSame(v1, edge6.GetOtherVertex(v2));

            // Edges cases
            var vNotInEdge = new TestVertex("1");
            Assert.AreSame(v1, edge5.GetOtherVertex(vNotInEdge));

            var equatableV1 = new EquatableTestVertex("1");
            var equatableV2 = new EquatableTestVertex("2");
            var equatableV1Bis = new EquatableTestVertex("1");
            var edge8 = new Edge<EquatableTestVertex>(equatableV1, equatableV2);
            Assert.AreSame(equatableV2, edge8.GetOtherVertex(equatableV1Bis));
        }

        [Test]
        public void GetOtherVertex_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => ((Edge<int>)null).GetOtherVertex(1));

            var testEdge = new Edge<TestVertex>(new TestVertex("1"), new TestVertex("2"));
            Assert.Throws<ArgumentNullException>(() => testEdge.GetOtherVertex(null));

            Assert.Throws<ArgumentNullException>(() => ((Edge<TestVertex>)null).GetOtherVertex(null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void IsAdjacent()
        {
            var edge1 = new Edge<int>(1, 1);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(2, 1);

            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            var vNotInEdge = new TestVertex("1");
            var edge4 = new Edge<TestVertex>(v1, v1);
            var edge5 = new Edge<TestVertex>(v1, v2);
            var edge6 = new Edge<TestVertex>(v2, v1);

            Assert.IsTrue(edge1.IsAdjacent(1));
            Assert.IsFalse(edge1.IsAdjacent(2));
            Assert.IsTrue(edge2.IsAdjacent(1));
            Assert.IsTrue(edge2.IsAdjacent(2));
            Assert.IsFalse(edge2.IsAdjacent(3));
            Assert.IsTrue(edge3.IsAdjacent(1));
            Assert.IsTrue(edge3.IsAdjacent(2));
            Assert.IsFalse(edge3.IsAdjacent(3));

            Assert.IsTrue(edge4.IsAdjacent(v1));
            Assert.IsTrue(edge5.IsAdjacent(v1));
            Assert.IsTrue(edge5.IsAdjacent(v2));
            Assert.IsFalse(edge5.IsAdjacent(vNotInEdge));
            Assert.IsTrue(edge6.IsAdjacent(v1));
            Assert.IsTrue(edge6.IsAdjacent(v2));
            Assert.IsFalse(edge6.IsAdjacent(vNotInEdge));

            // Edges cases
            var equatableV1 = new EquatableTestVertex("1");
            var equatableV2 = new EquatableTestVertex("2");
            var equatableV3 = new EquatableTestVertex("3");
            var equatableV1Bis = new EquatableTestVertex("1");
            var edge8 = new Edge<EquatableTestVertex>(equatableV1, equatableV2);
            Assert.IsTrue(edge8.IsAdjacent(equatableV1Bis));
            Assert.IsFalse(edge8.IsAdjacent(equatableV3));
        }

        [Test]
        public void IsAdjacent_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => ((Edge<int>)null).IsAdjacent(1));

            var testEdge = new Edge<TestVertex>(new TestVertex("1"), new TestVertex("2"));
            Assert.Throws<ArgumentNullException>(() => testEdge.IsAdjacent(null));

            Assert.Throws<ArgumentNullException>(() => ((Edge<TestVertex>)null).IsAdjacent(null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void IsPath()
        {
            Assert.IsTrue(Enumerable.Empty<Edge<int>>().IsPath<int, Edge<int>>());

            var edge1 = new Edge<int>(1, 1);
            // 1 -> 1
            Assert.IsTrue(new[] { edge1 }.IsPath<int, Edge<int>>());

            var edge2 = new Edge<int>(1, 2);
            // 1 -> 2
            Assert.IsTrue(new[] { edge2 }.IsPath<int, Edge<int>>());

            var edge3 = new Edge<int>(2, 1);
            // 1 -> 2 -> 1
            Assert.IsTrue(new[] { edge2, edge3 }.IsPath<int, Edge<int>>());
            // 1 -> 1 -> 2 -> 1 -> 1
            Assert.IsTrue(new[] { edge1, edge2, edge3, edge1 }.IsPath<int, Edge<int>>());

            var edge4 = new Edge<int>(1, 4);
            // 1 -> 2 -> 1 -> 4
            Assert.IsTrue(new[] { edge2, edge3, edge4 }.IsPath<int, Edge<int>>());
            // 1 -> 2 -> 1 -> 4-1 -> 2
            Assert.IsFalse(new[] { edge2, edge3, edge4, edge2 }.IsPath<int, Edge<int>>());
            // 2 -> 1 -> 4-1 -> 2
            Assert.IsFalse(new[] { edge3, edge4, edge2 }.IsPath<int, Edge<int>>());


            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            var v4 = new TestVertex("4");
            var edge5 = new Edge<TestVertex>(v1, v1);
            // 1 -> 1
            Assert.IsTrue(new[] { edge5 }.IsPath<TestVertex, Edge<TestVertex>>());

            var edge6 = new Edge<TestVertex>(v1, v2);
            // 1 -> 2
            Assert.IsTrue(new[] { edge6 }.IsPath<TestVertex, Edge<TestVertex>>());

            var edge7 = new Edge<TestVertex>(v2, v1);
            // 1 -> 2 -> 1
            Assert.IsTrue(new[] { edge6, edge7 }.IsPath<TestVertex, Edge<TestVertex>>());
            // 1 -> 1 -> 2 -> 1 -> 1
            Assert.IsTrue(new[] { edge5, edge6, edge7, edge5 }.IsPath<TestVertex, Edge<TestVertex>>());

            var edge8 = new Edge<TestVertex>(v1, v4);
            // 1 -> 2 -> 1 -> 4
            Assert.IsTrue(new[] { edge6, edge7, edge8 }.IsPath<TestVertex, Edge<TestVertex>>());
            // 1 -> 2 -> 1 -> 4-1 -> 2
            Assert.IsFalse(new[] { edge6, edge7, edge8, edge6 }.IsPath<TestVertex, Edge<TestVertex>>());
            // 2 -> 1 -> 4-1 -> 2
            Assert.IsFalse(new[] { edge7, edge8, edge6 }.IsPath<TestVertex, Edge<TestVertex>>());


            // Edge cases
            var v2Bis = new TestVertex("2");
            var edge9 = new Edge<TestVertex>(v2Bis, v1);
            // 1 -> 1 -> 2-2Bis -> 1 -> 1 -> 1
            Assert.IsFalse(new[] { edge5, edge6, edge9, edge5 }.IsPath<TestVertex, Edge<TestVertex>>());

            var equatableV1 = new EquatableTestVertex("1");
            var equatableV2 = new EquatableTestVertex("2");
            var equatableV2Bis = new EquatableTestVertex("2");
            var equatableV4 = new EquatableTestVertex("4");
            var edge10 = new Edge<EquatableTestVertex>(equatableV1, equatableV1);
            var edge11 = new Edge<EquatableTestVertex>(equatableV1, equatableV2);
            var edge12 = new Edge<EquatableTestVertex>(equatableV2Bis, equatableV1);
            var edge13 = new Edge<EquatableTestVertex>(equatableV1, equatableV4);
            // 1 -> 1 -> 2-2Bis -> 1 -> 4
            Assert.IsTrue(new[] { edge10, edge11, edge12, edge13 }.IsPath<EquatableTestVertex, Edge<EquatableTestVertex>>());
        }

        [Test]
        public void IsPath_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => ((IEnumerable<Edge<int>>)null).IsPath<int, Edge<int>>());
        }

        [Test]
        public void HasCycles()
        {
            Assert.IsFalse(Enumerable.Empty<Edge<int>>().HasCycles<int, Edge<int>>());

            var edge1 = new Edge<int>(1, 1);
            // 1 -> 1
            Assert.IsTrue(new[] { edge1 }.HasCycles<int, Edge<int>>());

            var edge2 = new Edge<int>(1, 2);
            // 1 -> 2
            Assert.IsFalse(new[] { edge2 }.HasCycles<int, Edge<int>>());

            var edge3 = new Edge<int>(2, 1);
            // 1 -> 2 -> 1
            Assert.IsTrue(new[] { edge2, edge3 }.HasCycles<int, Edge<int>>());
            // 2 -> 1 -> 2
            Assert.IsTrue(new[] { edge3, edge2 }.HasCycles<int, Edge<int>>());

            var edge4 = new Edge<int>(1, 4);
            var edge5 = new Edge<int>(2, 3);
            var edge6 = new Edge<int>(3, 4);
            var edge7 = new Edge<int>(3, 1);
            var edge8 = new Edge<int>(3, 3);
            var edge9 = new Edge<int>(4, 3);
            // 1 -> 2 -> 1 -> 4
            Assert.IsTrue(new[] { edge2, edge3, edge4 }.HasCycles<int, Edge<int>>());
            // 2 -> 1 -> 4 -> 3
            Assert.IsFalse(new[] { edge3, edge4, edge9 }.HasCycles<int, Edge<int>>());
            // 2 -> 1 -> 4 -> 3 -> 1
            Assert.IsTrue(new[] { edge3, edge4, edge9, edge7 }.HasCycles<int, Edge<int>>());
            // 2 -> 3 -> 4 -> 3 -> 3
            Assert.IsTrue(new[] { edge5, edge6, edge9, edge8 }.HasCycles<int, Edge<int>>());

            var edge10 = new Edge<int>(2, 4);
            var edge11 = new Edge<int>(3, 2);
            var edge12 = new Edge<int>(2, 5);
            // 1 -> 4 -> 3 -> 2 -> 5
            Assert.IsFalse(new[] { edge4, edge9, edge11, edge12 }.HasCycles<int, Edge<int>>());
            // 1 -> 2 -> 4 -> 3 -> 2 -> 5
            Assert.IsTrue(new[] { edge2, edge10, edge9, edge11, edge12 }.HasCycles<int, Edge<int>>());
            // 1 -> 4 -> 3 -> 3 -> 2 -> 5
            Assert.IsTrue(new[] { edge4, edge9, edge8, edge11, edge12 }.HasCycles<int, Edge<int>>());


            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            var v3 = new TestVertex("3");
            var v4 = new TestVertex("4");
            var v5 = new TestVertex("5");
            var edge13 = new Edge<TestVertex>(v1, v1);
            // 1 -> 1
            Assert.IsTrue(new[] { edge13 }.HasCycles<TestVertex, Edge<TestVertex>>());

            var edge14 = new Edge<TestVertex>(v1, v2);
            // 1 -> 2
            Assert.IsFalse(new[] { edge14 }.HasCycles<TestVertex, Edge<TestVertex>>());

            var edge15 = new Edge<TestVertex>(v2, v1);
            // 1 -> 2 -> 1
            Assert.IsTrue(new[] { edge14, edge15 }.HasCycles<TestVertex, Edge<TestVertex>>());
            // 2 -> 1 -> 2
            Assert.IsTrue(new[] { edge15, edge14 }.HasCycles<TestVertex, Edge<TestVertex>>());

            var edge16 = new Edge<TestVertex>(v1, v4);
            var edge17 = new Edge<TestVertex>(v2, v3);
            var edge18 = new Edge<TestVertex>(v3, v4);
            var edge19 = new Edge<TestVertex>(v3, v1);
            var edge20 = new Edge<TestVertex>(v3, v3);
            var edge21 = new Edge<TestVertex>(v4, v3);
            // 1 -> 2 -> 1 -> 4
            Assert.IsTrue(new[] { edge14, edge15, edge16 }.HasCycles<TestVertex, Edge<TestVertex>>());
            // 2 -> 1 -> 4 -> 3
            Assert.IsFalse(new[] { edge15, edge16, edge21 }.HasCycles<TestVertex, Edge<TestVertex>>());
            // 2 -> 1 -> 4 -> 3 -> 1
            Assert.IsTrue(new[] { edge15, edge16, edge21, edge19 }.HasCycles<TestVertex, Edge<TestVertex>>());
            // 2 -> 3 -> 4 -> 3 -> 3
            Assert.IsTrue(new[] { edge17, edge18, edge21, edge20 }.HasCycles<TestVertex, Edge<TestVertex>>());

            var edge22 = new Edge<TestVertex>(v2, v4);
            var edge23 = new Edge<TestVertex>(v3, v2);
            var edge24 = new Edge<TestVertex>(v2, v5);
            // 1 -> 4 -> 3 -> 2 -> 5
            Assert.IsFalse(new[] { edge16, edge21, edge23, edge24 }.HasCycles<TestVertex, Edge<TestVertex>>());
            // 1 -> 2 -> 4 -> 3 -> 2 -> 5
            Assert.IsTrue(new[] { edge14, edge22, edge21, edge23, edge24 }.HasCycles<TestVertex, Edge<TestVertex>>());
            // 1 -> 4 -> 3 -> 3 -> 2 -> 5
            Assert.IsTrue(new[] { edge16, edge21, edge20, edge23, edge24 }.HasCycles<TestVertex, Edge<TestVertex>>());

            // Edge cases
            var v2Bis = new TestVertex("2");
            var edge25 = new Edge<TestVertex>(v4, v2Bis);
            // 1 -> 2 -> 4 -> 2Bis
            Assert.IsFalse(new[] { edge14, edge22, edge25 }.HasCycles<TestVertex, Edge<TestVertex>>());

            var equatableV1 = new EquatableTestVertex("1");
            var equatableV2 = new EquatableTestVertex("2");
            var equatableV2Bis = new EquatableTestVertex("2");
            var equatableV4 = new EquatableTestVertex("4");
            var edge26 = new Edge<EquatableTestVertex>(equatableV1, equatableV2);
            var edge27 = new Edge<EquatableTestVertex>(equatableV2, equatableV4);
            var edge28 = new Edge<EquatableTestVertex>(equatableV4, equatableV2Bis);
            // 1 -> 2 -> 4 -> 2Bis
            Assert.IsTrue(new[] { edge26, edge27, edge28 }.HasCycles<EquatableTestVertex, Edge<EquatableTestVertex>>());
        }

        [Test]
        public void HasCycles_OnlyForPath()
        {
            // The method only work well if given a path
            // This test use edges that are not a path nor has cycle
            var edge14 = new Edge<int>(1, 4); 
            var edge21 = new Edge<int>(2, 1); 
            var edge43 = new Edge<int>(4, 3); 
            Assert.IsTrue(new[] { edge14, edge21, edge43 }.HasCycles<int, Edge<int>>());
        }

        [Test]
        public void HasCycles_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => ((IEnumerable<Edge<int>>)null).HasCycles<int, Edge<int>>());
        }

        [Test]
        public void IsPathWithoutCycles()
        {
            Assert.IsTrue(Enumerable.Empty<Edge<int>>().IsPathWithoutCycles<int, Edge<int>>());

            var edge1 = new Edge<int>(1, 1);
            // 1 -> 1
            Assert.IsFalse(new[] { edge1 }.IsPathWithoutCycles<int, Edge<int>>());

            var edge2 = new Edge<int>(1, 2);
            // 1 -> 2
            Assert.IsTrue(new[] { edge2 }.IsPathWithoutCycles<int, Edge<int>>());

            var edge3 = new Edge<int>(2, 1);
            // 1 -> 2 -> 1
            Assert.IsFalse(new[] { edge2, edge3 }.IsPathWithoutCycles<int, Edge<int>>());
            // 2 -> 1 -> 2
            Assert.IsFalse(new[] { edge3, edge2 }.IsPathWithoutCycles<int, Edge<int>>());

            var edge4 = new Edge<int>(1, 4);
            var edge5 = new Edge<int>(2, 3);
            var edge6 = new Edge<int>(3, 4);
            var edge7 = new Edge<int>(3, 1);
            var edge8 = new Edge<int>(3, 3);
            var edge9 = new Edge<int>(4, 3);
            // 1 -> 2 -> 1 -> 4
            Assert.IsFalse(new[] { edge2, edge3, edge4 }.IsPathWithoutCycles<int, Edge<int>>());
            // 2 -> 1 -> 4 -> 3
            Assert.IsTrue(new[] { edge3, edge4, edge9 }.IsPathWithoutCycles<int, Edge<int>>());
            // 2 -> 1 -> 4 -> 3 -> 1
            Assert.IsFalse(new[] { edge3, edge4, edge9, edge7 }.IsPathWithoutCycles<int, Edge<int>>());
            // 2 -> 3 -> 4 -> 3 -> 3
            Assert.IsFalse(new[] { edge5, edge6, edge9, edge8 }.IsPathWithoutCycles<int, Edge<int>>());

            var edge10 = new Edge<int>(2, 4);
            var edge11 = new Edge<int>(3, 2);
            var edge12 = new Edge<int>(2, 5);
            // 1 -> 4 -> 3 -> 2 -> 5
            Assert.IsTrue(new[] { edge4, edge9, edge11, edge12 }.IsPathWithoutCycles<int, Edge<int>>());
            // 1 -> 2 -> 4 -> 3 -> 2 -> 5
            Assert.IsFalse(new[] { edge2, edge10, edge9, edge11, edge12 }.IsPathWithoutCycles<int, Edge<int>>());
            // 1 -> 4 -> 3 -> 3 -> 2 -> 5
            Assert.IsFalse(new[] { edge4, edge9, edge8, edge11, edge12 }.IsPathWithoutCycles<int, Edge<int>>());

            // Not a path: 1 -> 2-4 -> 3
            Assert.IsFalse(new[] { edge2, edge9 }.IsPathWithoutCycles<int, Edge<int>>());


            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            var v3 = new TestVertex("3");
            var v4 = new TestVertex("4");
            var v5 = new TestVertex("5");
            var edge13 = new Edge<TestVertex>(v1, v1);
            // 1 -> 1
            Assert.IsFalse(new[] { edge13 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());

            var edge14 = new Edge<TestVertex>(v1, v2);
            // 1 -> 2
            Assert.IsTrue(new[] { edge14 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());

            var edge15 = new Edge<TestVertex>(v2, v1);
            // 1 -> 2 -> 1
            Assert.IsFalse(new[] { edge14, edge15 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());
            // 2 -> 1 -> 2
            Assert.IsFalse(new[] { edge15, edge14 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());

            var edge16 = new Edge<TestVertex>(v1, v4);
            var edge17 = new Edge<TestVertex>(v2, v3);
            var edge18 = new Edge<TestVertex>(v3, v4);
            var edge19 = new Edge<TestVertex>(v3, v1);
            var edge20 = new Edge<TestVertex>(v3, v3);
            var edge21 = new Edge<TestVertex>(v4, v3);
            // 1 -> 2 -> 1 -> 4
            Assert.IsFalse(new[] { edge14, edge15, edge16 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());
            // 2 -> 1 -> 4 -> 3
            Assert.IsTrue(new[] { edge15, edge16, edge21 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());
            // 2 -> 1 -> 4 -> 3 -> 1
            Assert.IsFalse(new[] { edge15, edge16, edge21, edge19 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());
            // 2 -> 3 -> 4 -> 3 -> 3
            Assert.IsFalse(new[] { edge17, edge18, edge21, edge20 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());

            var edge22 = new Edge<TestVertex>(v2, v4);
            var edge23 = new Edge<TestVertex>(v3, v2);
            var edge24 = new Edge<TestVertex>(v2, v5);
            // 1 -> 4 -> 3 -> 2 -> 5
            Assert.IsTrue(new[] { edge16, edge21, edge23, edge24 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());
            // 1 -> 2 -> 4 -> 3 -> 2 -> 5
            Assert.IsFalse(new[] { edge14, edge22, edge21, edge23, edge24 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());
            // 1 -> 4 -> 3 -> 3 -> 2 -> 5
            Assert.IsFalse(new[] { edge16, edge21, edge20, edge23, edge24 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());

            // Edge cases
            var v2Bis = new TestVertex("2");
            var edge25 = new Edge<TestVertex>(v4, v2Bis);
            // 1 -> 2 -> 4 -> 2Bis
            Assert.IsTrue(new[] { edge14, edge22, edge25 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>());

            var equatableV1 = new EquatableTestVertex("1");
            var equatableV2 = new EquatableTestVertex("2");
            var equatableV2Bis = new EquatableTestVertex("2");
            var equatableV4 = new EquatableTestVertex("4");
            var edge26 = new Edge<EquatableTestVertex>(equatableV1, equatableV2);
            var edge27 = new Edge<EquatableTestVertex>(equatableV2, equatableV4);
            var edge28 = new Edge<EquatableTestVertex>(equatableV4, equatableV2Bis);
            // 1 -> 2 -> 4 -> 2Bis
            Assert.IsFalse(new[] { edge26, edge27, edge28 }.IsPathWithoutCycles<EquatableTestVertex, Edge<EquatableTestVertex>>());
        }

        [Test]
        public void IsPathWithoutCycles_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => ((IEnumerable<Edge<int>>)null).IsPathWithoutCycles<int, Edge<int>>());
        }

        [Test]
        public void ToVertexPair()
        {
            var edge1 = new Edge<int>(1, 1);
            Assert.AreEqual(
                new SEquatableEdge<int>(1, 1),
                edge1.ToVertexPair());

            var edge2 = new Edge<int>(1, 2);
            Assert.AreEqual(
                new SEquatableEdge<int>(1, 2),
                edge2.ToVertexPair());
        }

        [Test]
        public void ToVertexPair_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((Edge<int>)null).ToVertexPair());
        }

        [Test]
        public void IsPredecessor()
        {
            var predecessors = new Dictionary<int, Edge<int>>();
            Assert.IsFalse(predecessors.IsPredecessor(1, 2));

            predecessors.Add(1, new Edge<int>(0, 1));
            Assert.IsFalse(predecessors.IsPredecessor(1, 2));

            predecessors.Add(2, new Edge<int>(0, 2));
            Assert.IsFalse(predecessors.IsPredecessor(1, 2));

            predecessors.Add(3, new Edge<int>(1, 3));
            predecessors.Add(4, new Edge<int>(3, 4));
            predecessors.Add(5, new Edge<int>(2, 5));
            Assert.IsFalse(predecessors.IsPredecessor(1, 2));

            predecessors[2] = new Edge<int>(1, 2);
            Assert.IsTrue(predecessors.IsPredecessor(1, 2));

            predecessors[2] = new Edge<int>(4, 2);
            Assert.IsTrue(predecessors.IsPredecessor(1, 2));

            predecessors[4] = new Edge<int>(4, 4);
            Assert.IsFalse(predecessors.IsPredecessor(1, 2));

            Assert.IsTrue(predecessors.IsPredecessor(1, 1));
        }

        [Test]
        public void IsPredecessor_Throws()
        {
            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((Dictionary<TestVertex, Edge<TestVertex>>)null).IsPredecessor(v1, v2));
            Assert.Throws<ArgumentNullException>(
                () => ((Dictionary<TestVertex, Edge<TestVertex>>)null).IsPredecessor(null, v2));
            Assert.Throws<ArgumentNullException>(
                () => ((Dictionary<TestVertex, Edge<TestVertex>>)null).IsPredecessor(v1, null));
            Assert.Throws<ArgumentNullException>(
                () => ((Dictionary<TestVertex, Edge<TestVertex>>)null).IsPredecessor(null, null));

            var predecessors = new Dictionary<TestVertex, Edge<TestVertex>>();
            Assert.Throws<ArgumentNullException>(
                () => predecessors.IsPredecessor(null, v2));
            Assert.Throws<ArgumentNullException>(
                () => predecessors.IsPredecessor(v1, null));
            Assert.Throws<ArgumentNullException>(
                () => predecessors.IsPredecessor(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void TryGetPath()
        {
            var predecessors = new Dictionary<int, Edge<int>>();
            Assert.IsFalse(predecessors.TryGetPath(2, out _));

            var edge1 = new Edge<int>(0, 1);
            predecessors.Add(1, edge1);
            Assert.IsFalse(predecessors.TryGetPath(2, out _));

            var edge2 = new Edge<int>(0, 2);
            predecessors.Add(2, edge2);
            Assert.IsTrue(predecessors.TryGetPath(2, out IEnumerable<Edge<int>> path));
            CollectionAssert.AreEqual(
                new[] { edge2 },
                path);

            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(3, 4);
            var edge5 = new Edge<int>(2, 5);
            predecessors.Add(3, edge3);
            predecessors.Add(4, edge4);
            predecessors.Add(5, edge5);
            Assert.IsTrue(predecessors.TryGetPath(2, out path));
            CollectionAssert.AreEqual(
                new[] { edge2 },
                path);

            var edge6 = new Edge<int>(1, 2);
            predecessors[2] = edge6;
            Assert.IsTrue(predecessors.TryGetPath(2, out path));
            CollectionAssert.AreEqual(
                new[] { edge1, edge6 },
                path);

            var edge7 = new Edge<int>(4, 2);
            predecessors[2] = edge7;
            Assert.IsTrue(predecessors.TryGetPath(2, out path));
            CollectionAssert.AreEqual(
                new[] { edge1, edge3, edge4, edge7 },
                path);

            var edge8 = new Edge<int>(3, 3);
            predecessors[3] = edge8;
            Assert.IsTrue(predecessors.TryGetPath(2, out path));
            CollectionAssert.AreEqual(
                new[] { edge4, edge7 },
                path);
        }

        [Test]
        public void TryGetPath_Throws()
        {
            var v1 = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((Dictionary<TestVertex, Edge<TestVertex>>)null).TryGetPath(v1, out _));
            Assert.Throws<ArgumentNullException>(
                () => ((Dictionary<TestVertex, Edge<TestVertex>>)null).TryGetPath(null, out _));

            var predecessors = new Dictionary<TestVertex, Edge<TestVertex>>();
            Assert.Throws<ArgumentNullException>(
                () => predecessors.TryGetPath(null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void UndirectedVertexEquality()
        {
            var edge11 = new Edge<int>(1, 1);
            Assert.IsTrue(edge11.UndirectedVertexEquality(1, 1));
            Assert.IsFalse(edge11.UndirectedVertexEquality(1, 2));
            Assert.IsFalse(edge11.UndirectedVertexEquality(2, 1));
            Assert.IsFalse(edge11.UndirectedVertexEquality(2, 2));

            var edge12 = new Edge<int>(1, 2);
            Assert.IsFalse(edge12.UndirectedVertexEquality(1, 1));
            Assert.IsTrue(edge12.UndirectedVertexEquality(1, 2));
            Assert.IsTrue(edge12.UndirectedVertexEquality(2, 1));
            Assert.IsFalse(edge12.UndirectedVertexEquality(2, 2));
        }

        [Test]
        public void UndirectedVertexEquality_Throws()
        {
            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((Edge<TestVertex>)null).UndirectedVertexEquality(v1, v2));
            Assert.Throws<ArgumentNullException>(
                () => ((Edge<TestVertex>)null).UndirectedVertexEquality(null, v2));
            Assert.Throws<ArgumentNullException>(
                () => ((Edge<TestVertex>)null).UndirectedVertexEquality(v1, null));
            Assert.Throws<ArgumentNullException>(
                () => ((Edge<TestVertex>)null).UndirectedVertexEquality(null, null));

            var edge = new Edge<TestVertex>(v1, v2);
            Assert.Throws<ArgumentNullException>(
                () => edge.UndirectedVertexEquality(null, v2));
            Assert.Throws<ArgumentNullException>(
                () => edge.UndirectedVertexEquality(v1, null));
            Assert.Throws<ArgumentNullException>(
                () => edge.UndirectedVertexEquality(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void SortedVertexEquality()
        {
            var edge11 = new Edge<int>(1, 1);
            Assert.IsTrue(edge11.SortedVertexEquality(1, 1));
            Assert.IsFalse(edge11.SortedVertexEquality(1, 2));
            Assert.IsFalse(edge11.SortedVertexEquality(2, 1));
            Assert.IsFalse(edge11.SortedVertexEquality(2, 2));

            var edge12 = new Edge<int>(1, 2);
            Assert.IsFalse(edge12.SortedVertexEquality(1, 1));
            Assert.IsTrue(edge12.SortedVertexEquality(1, 2));
            Assert.IsFalse(edge12.SortedVertexEquality(2, 1));
            Assert.IsFalse(edge12.SortedVertexEquality(2, 2));
        }

        [Test]
        public void SortedVertexEquality_Throws()
        {
            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => ((Edge<TestVertex>)null).SortedVertexEquality(v1, v2));
            Assert.Throws<ArgumentNullException>(
                () => ((Edge<TestVertex>)null).SortedVertexEquality(null, v2));
            Assert.Throws<ArgumentNullException>(
                () => ((Edge<TestVertex>)null).SortedVertexEquality(v1, null));
            Assert.Throws<ArgumentNullException>(
                () => ((Edge<TestVertex>)null).SortedVertexEquality(null, null));

            var edge = new Edge<TestVertex>(v1, v2);
            Assert.Throws<ArgumentNullException>(
                () => edge.SortedVertexEquality(null, v2));
            Assert.Throws<ArgumentNullException>(
                () => edge.SortedVertexEquality(v1, null));
            Assert.Throws<ArgumentNullException>(
                () => edge.SortedVertexEquality(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ReverseEdges()
        {
            CollectionAssert.IsEmpty(EdgeExtensions.ReverseEdges<int, Edge<int>>(Enumerable.Empty<Edge<int>>()));

            var edge1 = new Edge<int>(1, 2);
            CollectionAssert.AreEqual(
                new[] { new SReversedEdge<int, Edge<int>>(edge1) },
                EdgeExtensions.ReverseEdges<int, Edge<int>>(new[] { edge1 }));

            var edge2 = new Edge<int>(2, 2);
            var edge3 = new Edge<int>(3, 1);
            CollectionAssert.AreEqual(
                new[]
                {
                    new SReversedEdge<int, Edge<int>>(edge1),
                    new SReversedEdge<int, Edge<int>>(edge2),
                    new SReversedEdge<int, Edge<int>>(edge3)
                },
                EdgeExtensions.ReverseEdges<int, Edge<int>>(new[] { edge1, edge2, edge3 }));
        }

        [Test]
        public void ReverseEdges_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => EdgeExtensions.ReverseEdges<int, Edge<int>>(null));
        }
    }
}