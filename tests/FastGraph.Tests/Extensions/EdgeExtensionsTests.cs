#nullable enable

using NUnit.Framework;

namespace FastGraph.Tests.Extensions
{
    /// <summary>
    /// Tests related to <see cref="EdgeExtensions"/>.
    /// </summary>
    internal sealed class EdgeExtensionsTests
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

            edge1.IsSelfEdge().Should().BeTrue();
            edge2.IsSelfEdge().Should().BeFalse();
            edge3.IsSelfEdge().Should().BeFalse();
            edge4.IsSelfEdge().Should().BeTrue();
            edge5.IsSelfEdge().Should().BeFalse();
            edge6.IsSelfEdge().Should().BeFalse();

            // Edges cases
            var v1Bis = new TestVertex("1");
            var edge7 = new Edge<TestVertex>(v1, v1Bis);
            edge7.IsSelfEdge().Should().BeFalse();

            var equatableV1 = new EquatableTestVertex("1");
            var equatableV1Bis = new EquatableTestVertex("1");
            var edge8 = new Edge<EquatableTestVertex>(equatableV1, equatableV1Bis);
            edge8.IsSelfEdge().Should().BeTrue();
        }

        [Test]
        public void IsSelfEdge_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
#pragma warning disable CS8625
            Invoking(() => ((Edge<int>?)default).IsSelfEdge()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
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

            edge1.GetOtherVertex(1).Should().Be(1);
            edge2.GetOtherVertex(1).Should().Be(2);
            edge2.GetOtherVertex(2).Should().Be(1);
            edge3.GetOtherVertex(1).Should().Be(2);
            edge3.GetOtherVertex(2).Should().Be(1);

            edge4.GetOtherVertex(v1).Should().BeSameAs(v1);
            edge5.GetOtherVertex(v1).Should().BeSameAs(v2);
            edge5.GetOtherVertex(v2).Should().BeSameAs(v1);
            edge6.GetOtherVertex(v1).Should().BeSameAs(v2);
            edge6.GetOtherVertex(v2).Should().BeSameAs(v1);

            // Edges cases
            var vNotInEdge = new TestVertex("1");
            edge5.GetOtherVertex(vNotInEdge).Should().BeSameAs(v1);

            var equatableV1 = new EquatableTestVertex("1");
            var equatableV2 = new EquatableTestVertex("2");
            var equatableV1Bis = new EquatableTestVertex("1");
            var edge8 = new Edge<EquatableTestVertex>(equatableV1, equatableV2);
            edge8.GetOtherVertex(equatableV1Bis).Should().BeSameAs(equatableV2);
        }

        [Test]
        public void GetOtherVertex_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
#pragma warning disable CS8714
#pragma warning disable CS8625
            Invoking(() => ((Edge<int>?)default).GetOtherVertex(1)).Should().Throw<ArgumentNullException>();

            var testEdge = new Edge<TestVertex>(new TestVertex("1"), new TestVertex("2"));
            Invoking(() => testEdge.GetOtherVertex(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => ((Edge<TestVertex>?)default).GetOtherVertex(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
#pragma warning restore CS8714
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

            edge1.IsAdjacent(1).Should().BeTrue();
            edge1.IsAdjacent(2).Should().BeFalse();
            edge2.IsAdjacent(1).Should().BeTrue();
            edge2.IsAdjacent(2).Should().BeTrue();
            edge2.IsAdjacent(3).Should().BeFalse();
            edge3.IsAdjacent(1).Should().BeTrue();
            edge3.IsAdjacent(2).Should().BeTrue();
            edge3.IsAdjacent(3).Should().BeFalse();

            edge4.IsAdjacent(v1).Should().BeTrue();
            edge5.IsAdjacent(v1).Should().BeTrue();
            edge5.IsAdjacent(v2).Should().BeTrue();
            edge5.IsAdjacent(vNotInEdge).Should().BeFalse();
            edge6.IsAdjacent(v1).Should().BeTrue();
            edge6.IsAdjacent(v2).Should().BeTrue();
            edge6.IsAdjacent(vNotInEdge).Should().BeFalse();

            // Edges cases
            var equatableV1 = new EquatableTestVertex("1");
            var equatableV2 = new EquatableTestVertex("2");
            var equatableV3 = new EquatableTestVertex("3");
            var equatableV1Bis = new EquatableTestVertex("1");
            var edge8 = new Edge<EquatableTestVertex>(equatableV1, equatableV2);
            edge8.IsAdjacent(equatableV1Bis).Should().BeTrue();
            edge8.IsAdjacent(equatableV3).Should().BeFalse();
        }

        [Test]
        public void IsAdjacent_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
#pragma warning disable CS8625
#pragma warning disable CS8714
            Invoking(() => ((Edge<int>?)default).IsAdjacent(1)).Should().Throw<ArgumentNullException>();

            var testEdge = new Edge<TestVertex>(new TestVertex("1"), new TestVertex("2"));
            Invoking(() => testEdge.IsAdjacent(default)).Should().Throw<ArgumentNullException>();

            Invoking(() => ((Edge<TestVertex>?)default).IsAdjacent(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8625
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void IsPath()
        {
            Enumerable.Empty<Edge<int>>().IsPath<int, Edge<int>>().Should().BeTrue();

            var edge1 = new Edge<int>(1, 1);
            // 1 -> 1
            new[] { edge1 }.IsPath<int, Edge<int>>().Should().BeTrue();

            var edge2 = new Edge<int>(1, 2);
            // 1 -> 2
            new[] { edge2 }.IsPath<int, Edge<int>>().Should().BeTrue();

            var edge3 = new Edge<int>(2, 1);
            // 1 -> 2 -> 1
            new[] { edge2, edge3 }.IsPath<int, Edge<int>>().Should().BeTrue();
            // 1 -> 1 -> 2 -> 1 -> 1
            new[] { edge1, edge2, edge3, edge1 }.IsPath<int, Edge<int>>().Should().BeTrue();

            var edge4 = new Edge<int>(1, 4);
            // 1 -> 2 -> 1 -> 4
            new[] { edge2, edge3, edge4 }.IsPath<int, Edge<int>>().Should().BeTrue();
            // 1 -> 2 -> 1 -> 4-1 -> 2
            new[] { edge2, edge3, edge4, edge2 }.IsPath<int, Edge<int>>().Should().BeFalse();
            // 2 -> 1 -> 4-1 -> 2
            new[] { edge3, edge4, edge2 }.IsPath<int, Edge<int>>().Should().BeFalse();


            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            var v4 = new TestVertex("4");
            var edge5 = new Edge<TestVertex>(v1, v1);
            // 1 -> 1
            new[] { edge5 }.IsPath<TestVertex, Edge<TestVertex>>().Should().BeTrue();

            var edge6 = new Edge<TestVertex>(v1, v2);
            // 1 -> 2
            new[] { edge6 }.IsPath<TestVertex, Edge<TestVertex>>().Should().BeTrue();

            var edge7 = new Edge<TestVertex>(v2, v1);
            // 1 -> 2 -> 1
            new[] { edge6, edge7 }.IsPath<TestVertex, Edge<TestVertex>>().Should().BeTrue();
            // 1 -> 1 -> 2 -> 1 -> 1
            new[] { edge5, edge6, edge7, edge5 }.IsPath<TestVertex, Edge<TestVertex>>().Should().BeTrue();

            var edge8 = new Edge<TestVertex>(v1, v4);
            // 1 -> 2 -> 1 -> 4
            new[] { edge6, edge7, edge8 }.IsPath<TestVertex, Edge<TestVertex>>().Should().BeTrue();
            // 1 -> 2 -> 1 -> 4-1 -> 2
            new[] { edge6, edge7, edge8, edge6 }.IsPath<TestVertex, Edge<TestVertex>>().Should().BeFalse();
            // 2 -> 1 -> 4-1 -> 2
            new[] { edge7, edge8, edge6 }.IsPath<TestVertex, Edge<TestVertex>>().Should().BeFalse();


            // Edge cases
            var v2Bis = new TestVertex("2");
            var edge9 = new Edge<TestVertex>(v2Bis, v1);
            // 1 -> 1 -> 2-2Bis -> 1 -> 1 -> 1
            new[] { edge5, edge6, edge9, edge5 }.IsPath<TestVertex, Edge<TestVertex>>().Should().BeFalse();

            var equatableV1 = new EquatableTestVertex("1");
            var equatableV2 = new EquatableTestVertex("2");
            var equatableV2Bis = new EquatableTestVertex("2");
            var equatableV4 = new EquatableTestVertex("4");
            var edge10 = new Edge<EquatableTestVertex>(equatableV1, equatableV1);
            var edge11 = new Edge<EquatableTestVertex>(equatableV1, equatableV2);
            var edge12 = new Edge<EquatableTestVertex>(equatableV2Bis, equatableV1);
            var edge13 = new Edge<EquatableTestVertex>(equatableV1, equatableV4);
            // 1 -> 1 -> 2-2Bis -> 1 -> 4
            new[] { edge10, edge11, edge12, edge13 }.IsPath<EquatableTestVertex, Edge<EquatableTestVertex>>().Should().BeTrue();
        }

        [Test]
        public void IsPath_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
#pragma warning disable CS8625
            Invoking(() => ((IEnumerable<Edge<int>>?)default).IsPath<int, Edge<int>>()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void HasCycles()
        {
            Enumerable.Empty<Edge<int>>().HasCycles<int, Edge<int>>().Should().BeFalse();

            var edge1 = new Edge<int>(1, 1);
            // 1 -> 1
            new[] { edge1 }.HasCycles<int, Edge<int>>().Should().BeTrue();

            var edge2 = new Edge<int>(1, 2);
            // 1 -> 2
            new[] { edge2 }.HasCycles<int, Edge<int>>().Should().BeFalse();

            var edge3 = new Edge<int>(2, 1);
            // 1 -> 2 -> 1
            new[] { edge2, edge3 }.HasCycles<int, Edge<int>>().Should().BeTrue();
            // 2 -> 1 -> 2
            new[] { edge3, edge2 }.HasCycles<int, Edge<int>>().Should().BeTrue();

            var edge4 = new Edge<int>(1, 4);
            var edge5 = new Edge<int>(2, 3);
            var edge6 = new Edge<int>(3, 4);
            var edge7 = new Edge<int>(3, 1);
            var edge8 = new Edge<int>(3, 3);
            var edge9 = new Edge<int>(4, 3);
            // 1 -> 2 -> 1 -> 4
            new[] { edge2, edge3, edge4 }.HasCycles<int, Edge<int>>().Should().BeTrue();
            // 2 -> 1 -> 4 -> 3
            new[] { edge3, edge4, edge9 }.HasCycles<int, Edge<int>>().Should().BeFalse();
            // 2 -> 1 -> 4 -> 3 -> 1
            new[] { edge3, edge4, edge9, edge7 }.HasCycles<int, Edge<int>>().Should().BeTrue();
            // 2 -> 3 -> 4 -> 3 -> 3
            new[] { edge5, edge6, edge9, edge8 }.HasCycles<int, Edge<int>>().Should().BeTrue();

            var edge10 = new Edge<int>(2, 4);
            var edge11 = new Edge<int>(3, 2);
            var edge12 = new Edge<int>(2, 5);
            // 1 -> 4 -> 3 -> 2 -> 5
            new[] { edge4, edge9, edge11, edge12 }.HasCycles<int, Edge<int>>().Should().BeFalse();
            // 1 -> 2 -> 4 -> 3 -> 2 -> 5
            new[] { edge2, edge10, edge9, edge11, edge12 }.HasCycles<int, Edge<int>>().Should().BeTrue();
            // 1 -> 4 -> 3 -> 3 -> 2 -> 5
            new[] { edge4, edge9, edge8, edge11, edge12 }.HasCycles<int, Edge<int>>().Should().BeTrue();


            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            var v3 = new TestVertex("3");
            var v4 = new TestVertex("4");
            var v5 = new TestVertex("5");
            var edge13 = new Edge<TestVertex>(v1, v1);
            // 1 -> 1
            new[] { edge13 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();

            var edge14 = new Edge<TestVertex>(v1, v2);
            // 1 -> 2
            new[] { edge14 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();

            var edge15 = new Edge<TestVertex>(v2, v1);
            // 1 -> 2 -> 1
            new[] { edge14, edge15 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();
            // 2 -> 1 -> 2
            new[] { edge15, edge14 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();

            var edge16 = new Edge<TestVertex>(v1, v4);
            var edge17 = new Edge<TestVertex>(v2, v3);
            var edge18 = new Edge<TestVertex>(v3, v4);
            var edge19 = new Edge<TestVertex>(v3, v1);
            var edge20 = new Edge<TestVertex>(v3, v3);
            var edge21 = new Edge<TestVertex>(v4, v3);
            // 1 -> 2 -> 1 -> 4
            new[] { edge14, edge15, edge16 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();
            // 2 -> 1 -> 4 -> 3
            new[] { edge15, edge16, edge21 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();
            // 2 -> 1 -> 4 -> 3 -> 1
            new[] { edge15, edge16, edge21, edge19 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();
            // 2 -> 3 -> 4 -> 3 -> 3
            new[] { edge17, edge18, edge21, edge20 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();

            var edge22 = new Edge<TestVertex>(v2, v4);
            var edge23 = new Edge<TestVertex>(v3, v2);
            var edge24 = new Edge<TestVertex>(v2, v5);
            // 1 -> 4 -> 3 -> 2 -> 5
            new[] { edge16, edge21, edge23, edge24 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();
            // 1 -> 2 -> 4 -> 3 -> 2 -> 5
            new[] { edge14, edge22, edge21, edge23, edge24 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();
            // 1 -> 4 -> 3 -> 3 -> 2 -> 5
            new[] { edge16, edge21, edge20, edge23, edge24 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();

            // Edge cases
            var v2Bis = new TestVertex("2");
            var edge25 = new Edge<TestVertex>(v4, v2Bis);
            // 1 -> 2 -> 4 -> 2Bis
            new[] { edge14, edge22, edge25 }.HasCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();

            var equatableV1 = new EquatableTestVertex("1");
            var equatableV2 = new EquatableTestVertex("2");
            var equatableV2Bis = new EquatableTestVertex("2");
            var equatableV4 = new EquatableTestVertex("4");
            var edge26 = new Edge<EquatableTestVertex>(equatableV1, equatableV2);
            var edge27 = new Edge<EquatableTestVertex>(equatableV2, equatableV4);
            var edge28 = new Edge<EquatableTestVertex>(equatableV4, equatableV2Bis);
            // 1 -> 2 -> 4 -> 2Bis
            new[] { edge26, edge27, edge28 }.HasCycles<EquatableTestVertex, Edge<EquatableTestVertex>>().Should().BeTrue();
        }

        [Test]
        public void HasCycles_OnlyForPath()
        {
            // The method only work well if given a path
            // This test use edges that are not a path nor has cycle
            var edge14 = new Edge<int>(1, 4);
            var edge21 = new Edge<int>(2, 1);
            var edge43 = new Edge<int>(4, 3);
            new[] { edge14, edge21, edge43 }.HasCycles<int, Edge<int>>().Should().BeTrue();
        }

        [Test]
        public void HasCycles_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
#pragma warning disable CS8625
            Invoking(() => ((IEnumerable<Edge<int>>?)default).HasCycles<int, Edge<int>>()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void IsPathWithoutCycles()
        {
            Enumerable.Empty<Edge<int>>().IsPathWithoutCycles<int, Edge<int>>().Should().BeTrue();

            var edge1 = new Edge<int>(1, 1);
            // 1 -> 1
            new[] { edge1 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeFalse();

            var edge2 = new Edge<int>(1, 2);
            // 1 -> 2
            new[] { edge2 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeTrue();

            var edge3 = new Edge<int>(2, 1);
            // 1 -> 2 -> 1
            new[] { edge2, edge3 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeFalse();
            // 2 -> 1 -> 2
            new[] { edge3, edge2 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeFalse();

            var edge4 = new Edge<int>(1, 4);
            var edge5 = new Edge<int>(2, 3);
            var edge6 = new Edge<int>(3, 4);
            var edge7 = new Edge<int>(3, 1);
            var edge8 = new Edge<int>(3, 3);
            var edge9 = new Edge<int>(4, 3);
            // 1 -> 2 -> 1 -> 4
            new[] { edge2, edge3, edge4 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeFalse();
            // 2 -> 1 -> 4 -> 3
            new[] { edge3, edge4, edge9 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeTrue();
            // 2 -> 1 -> 4 -> 3 -> 1
            new[] { edge3, edge4, edge9, edge7 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeFalse();
            // 2 -> 3 -> 4 -> 3 -> 3
            new[] { edge5, edge6, edge9, edge8 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeFalse();

            var edge10 = new Edge<int>(2, 4);
            var edge11 = new Edge<int>(3, 2);
            var edge12 = new Edge<int>(2, 5);
            // 1 -> 4 -> 3 -> 2 -> 5
            new[] { edge4, edge9, edge11, edge12 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeTrue();
            // 1 -> 2 -> 4 -> 3 -> 2 -> 5
            new[] { edge2, edge10, edge9, edge11, edge12 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeFalse();
            // 1 -> 4 -> 3 -> 3 -> 2 -> 5
            new[] { edge4, edge9, edge8, edge11, edge12 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeFalse();

            // Not a path: 1 -> 2-4 -> 3
            new[] { edge2, edge9 }.IsPathWithoutCycles<int, Edge<int>>().Should().BeFalse();


            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            var v3 = new TestVertex("3");
            var v4 = new TestVertex("4");
            var v5 = new TestVertex("5");
            var edge13 = new Edge<TestVertex>(v1, v1);
            // 1 -> 1
            new[] { edge13 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();

            var edge14 = new Edge<TestVertex>(v1, v2);
            // 1 -> 2
            new[] { edge14 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();

            var edge15 = new Edge<TestVertex>(v2, v1);
            // 1 -> 2 -> 1
            new[] { edge14, edge15 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();
            // 2 -> 1 -> 2
            new[] { edge15, edge14 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();

            var edge16 = new Edge<TestVertex>(v1, v4);
            var edge17 = new Edge<TestVertex>(v2, v3);
            var edge18 = new Edge<TestVertex>(v3, v4);
            var edge19 = new Edge<TestVertex>(v3, v1);
            var edge20 = new Edge<TestVertex>(v3, v3);
            var edge21 = new Edge<TestVertex>(v4, v3);
            // 1 -> 2 -> 1 -> 4
            new[] { edge14, edge15, edge16 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();
            // 2 -> 1 -> 4 -> 3
            new[] { edge15, edge16, edge21 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();
            // 2 -> 1 -> 4 -> 3 -> 1
            new[] { edge15, edge16, edge21, edge19 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();
            // 2 -> 3 -> 4 -> 3 -> 3
            new[] { edge17, edge18, edge21, edge20 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();

            var edge22 = new Edge<TestVertex>(v2, v4);
            var edge23 = new Edge<TestVertex>(v3, v2);
            var edge24 = new Edge<TestVertex>(v2, v5);
            // 1 -> 4 -> 3 -> 2 -> 5
            new[] { edge16, edge21, edge23, edge24 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();
            // 1 -> 2 -> 4 -> 3 -> 2 -> 5
            new[] { edge14, edge22, edge21, edge23, edge24 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();
            // 1 -> 4 -> 3 -> 3 -> 2 -> 5
            new[] { edge16, edge21, edge20, edge23, edge24 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeFalse();

            // Edge cases
            var v2Bis = new TestVertex("2");
            var edge25 = new Edge<TestVertex>(v4, v2Bis);
            // 1 -> 2 -> 4 -> 2Bis
            new[] { edge14, edge22, edge25 }.IsPathWithoutCycles<TestVertex, Edge<TestVertex>>().Should().BeTrue();

            var equatableV1 = new EquatableTestVertex("1");
            var equatableV2 = new EquatableTestVertex("2");
            var equatableV2Bis = new EquatableTestVertex("2");
            var equatableV4 = new EquatableTestVertex("4");
            var edge26 = new Edge<EquatableTestVertex>(equatableV1, equatableV2);
            var edge27 = new Edge<EquatableTestVertex>(equatableV2, equatableV4);
            var edge28 = new Edge<EquatableTestVertex>(equatableV4, equatableV2Bis);
            // 1 -> 2 -> 4 -> 2Bis
            new[] { edge26, edge27, edge28 }.IsPathWithoutCycles<EquatableTestVertex, Edge<EquatableTestVertex>>().Should().BeFalse();
        }

        [Test]
        public void IsPathWithoutCycles_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
#pragma warning disable CS8625
            Invoking(() => ((IEnumerable<Edge<int>>?)default).IsPathWithoutCycles<int, Edge<int>>()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void ToVertexPair()
        {
            var edge1 = new Edge<int>(1, 1);
            edge1.ToVertexPair().Should().Be(new SEquatableEdge<int>(1, 1));

            var edge2 = new Edge<int>(1, 2);
            edge2.ToVertexPair().Should().Be(new SEquatableEdge<int>(1, 2));
        }

        [Test]
        public void ToVertexPair_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => ((Edge<int>?)default).ToVertexPair()).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }

        [Test]
        public void IsPredecessor()
        {
            var predecessors = new Dictionary<int, Edge<int>>();
            predecessors.IsPredecessor(1, 2).Should().BeFalse();

            predecessors.Add(1, new Edge<int>(0, 1));
            predecessors.IsPredecessor(1, 2).Should().BeFalse();

            predecessors.Add(2, new Edge<int>(0, 2));
            predecessors.IsPredecessor(1, 2).Should().BeFalse();

            predecessors.Add(3, new Edge<int>(1, 3));
            predecessors.Add(4, new Edge<int>(3, 4));
            predecessors.Add(5, new Edge<int>(2, 5));
            predecessors.IsPredecessor(1, 2).Should().BeFalse();

            predecessors[2] = new Edge<int>(1, 2);
            predecessors.IsPredecessor(1, 2).Should().BeTrue();

            predecessors[2] = new Edge<int>(4, 2);
            predecessors.IsPredecessor(1, 2).Should().BeTrue();

            predecessors[4] = new Edge<int>(4, 4);
            predecessors.IsPredecessor(1, 2).Should().BeFalse();

            predecessors.IsPredecessor(1, 1).Should().BeTrue();
        }

        [Test]
        public void IsPredecessor_Throws()
        {
            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((Dictionary<TestVertex, Edge<TestVertex>>?)default).IsPredecessor(v1, v2)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((Dictionary<TestVertex, Edge<TestVertex>>?)default).IsPredecessor(default, v2)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((Dictionary<TestVertex, Edge<TestVertex>>?)default).IsPredecessor(v1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((Dictionary<TestVertex, Edge<TestVertex>>?)default).IsPredecessor(default, default)).Should().Throw<ArgumentNullException>();

            var predecessors = new Dictionary<TestVertex, Edge<TestVertex>>();
            Invoking(() => predecessors.IsPredecessor(default, v2)).Should().Throw<ArgumentNullException>();
            Invoking(() => predecessors.IsPredecessor(v1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => predecessors.IsPredecessor(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void TryGetPath()
        {
            var predecessors = new Dictionary<int, Edge<int>>();
            predecessors.TryGetPath(2, out _).Should().BeFalse();

            var edge1 = new Edge<int>(0, 1);
            predecessors.Add(1, edge1);
            predecessors.TryGetPath(2, out _).Should().BeFalse();

            var edge2 = new Edge<int>(0, 2);
            predecessors.Add(2, edge2);
            predecessors.TryGetPath(2, out IEnumerable<Edge<int>>? path).Should().BeTrue();
            new[] { edge2 }.Should().BeEquivalentTo(path);

            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(3, 4);
            var edge5 = new Edge<int>(2, 5);
            predecessors.Add(3, edge3);
            predecessors.Add(4, edge4);
            predecessors.Add(5, edge5);
            predecessors.TryGetPath(2, out path).Should().BeTrue();
            new[] { edge2 }.Should().BeEquivalentTo(path);

            var edge6 = new Edge<int>(1, 2);
            predecessors[2] = edge6;
            predecessors.TryGetPath(2, out path).Should().BeTrue();
            new[] { edge1, edge6 }.Should().BeEquivalentTo(path);

            var edge7 = new Edge<int>(4, 2);
            predecessors[2] = edge7;
            predecessors.TryGetPath(2, out path).Should().BeTrue();
            new[] { edge1, edge3, edge4, edge7 }.Should().BeEquivalentTo(path);

            var edge8 = new Edge<int>(3, 3);
            predecessors[3] = edge8;
            predecessors.TryGetPath(2, out path).Should().BeTrue();
            new[] { edge4, edge7 }.Should().BeEquivalentTo(path);
        }

        [Test]
        public void TryGetPath_Throws()
        {
            var v1 = new TestVertex("1");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8620
#pragma warning disable CS8714
            Invoking(() => ((Dictionary<TestVertex, Edge<TestVertex>>?)default).TryGetPath(v1, out _)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((Dictionary<TestVertex, Edge<TestVertex>>?)default).TryGetPath(default, out _)).Should().Throw<ArgumentNullException>();

            var predecessors = new Dictionary<TestVertex, Edge<TestVertex>>();
            Invoking(() => predecessors.TryGetPath(default, out _)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8620
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void UndirectedVertexEquality()
        {
            var edge11 = new Edge<int>(1, 1);
            edge11.UndirectedVertexEquality(1, 1).Should().BeTrue();
            edge11.UndirectedVertexEquality(1, 2).Should().BeFalse();
            edge11.UndirectedVertexEquality(2, 1).Should().BeFalse();
            edge11.UndirectedVertexEquality(2, 2).Should().BeFalse();

            var edge12 = new Edge<int>(1, 2);
            edge12.UndirectedVertexEquality(1, 1).Should().BeFalse();
            edge12.UndirectedVertexEquality(1, 2).Should().BeTrue();
            edge12.UndirectedVertexEquality(2, 1).Should().BeTrue();
            edge12.UndirectedVertexEquality(2, 2).Should().BeFalse();
        }

        [Test]
        public void UndirectedVertexEquality_Throws()
        {
            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8714
            Invoking(() => ((Edge<TestVertex>?)default).UndirectedVertexEquality(v1, v2)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((Edge<TestVertex>?)default).UndirectedVertexEquality(default, v2)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((Edge<TestVertex>?)default).UndirectedVertexEquality(v1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((Edge<TestVertex>?)default).UndirectedVertexEquality(default, default)).Should().Throw<ArgumentNullException>();

            var edge = new Edge<TestVertex>(v1, v2);
            Invoking(() => edge.UndirectedVertexEquality(default, v2)).Should().Throw<ArgumentNullException>();
            Invoking(() => edge.UndirectedVertexEquality(v1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => edge.UndirectedVertexEquality(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void SortedVertexEquality()
        {
            var edge11 = new Edge<int>(1, 1);
            edge11.SortedVertexEquality(1, 1).Should().BeTrue();
            edge11.SortedVertexEquality(1, 2).Should().BeFalse();
            edge11.SortedVertexEquality(2, 1).Should().BeFalse();
            edge11.SortedVertexEquality(2, 2).Should().BeFalse();

            var edge12 = new Edge<int>(1, 2);
            edge12.SortedVertexEquality(1, 1).Should().BeFalse();
            edge12.SortedVertexEquality(1, 2).Should().BeTrue();
            edge12.SortedVertexEquality(2, 1).Should().BeFalse();
            edge12.SortedVertexEquality(2, 2).Should().BeFalse();
        }

        [Test]
        public void SortedVertexEquality_Throws()
        {
            var v1 = new TestVertex("1");
            var v2 = new TestVertex("2");
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable CS8625
#pragma warning disable CS8714
            Invoking(() => ((Edge<TestVertex>?)default).SortedVertexEquality(v1, v2)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((Edge<TestVertex>?)default).SortedVertexEquality(default, v2)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((Edge<TestVertex>?)default).SortedVertexEquality(v1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => ((Edge<TestVertex>?)default).SortedVertexEquality(default, default)).Should().Throw<ArgumentNullException>();

            var edge = new Edge<TestVertex>(v1, v2);
            Invoking(() => edge.SortedVertexEquality(default, v2)).Should().Throw<ArgumentNullException>();
            Invoking(() => edge.SortedVertexEquality(v1, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => edge.SortedVertexEquality(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8714
#pragma warning restore CS8625
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void ReverseEdges()
        {
            EdgeExtensions.ReverseEdges<int, Edge<int>>(Enumerable.Empty<Edge<int>>()).Should().BeEmpty();

            var edge1 = new Edge<int>(1, 2);
            new[] { new SReversedEdge<int, Edge<int>>(edge1) }.Should().BeEquivalentTo(EdgeExtensions.ReverseEdges<int, Edge<int>>(new[] { edge1 }));

            var edge2 = new Edge<int>(2, 2);
            var edge3 = new Edge<int>(3, 1);
            new[]
            {
                new SReversedEdge<int, Edge<int>>(edge1),
                new SReversedEdge<int, Edge<int>>(edge2),
                new SReversedEdge<int, Edge<int>>(edge3)
            }.Should().BeEquivalentTo(EdgeExtensions.ReverseEdges<int, Edge<int>>(new[] { edge1, edge2, edge3 }));
        }

        [Test]
        public void ReverseEdges_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
            Invoking(() => EdgeExtensions.ReverseEdges<int, Edge<int>>(default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
        }
    }
}
