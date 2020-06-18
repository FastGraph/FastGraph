using System;
using NUnit.Framework;
using QuikGraph.Tests;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.MSAGL.Tests
{
    /// <summary>
    /// Tests related to <see cref="MsaglDefaultGraphPopulator{TVertex,TEdge}"/>.
    /// </summary>
    internal class MsaglDefaultGraphPopulatorTests : MsaglGraphPopulatorTestsBase
    {
        #region Test classes

        private class TestMsaglDefaultGraphPopulator : MsaglDefaultGraphPopulator<TestVertex, Edge<TestVertex>>
        {
            public TestMsaglDefaultGraphPopulator()
                : base(new AdjacencyGraph<TestVertex, Edge<TestVertex>>())
            {
            }

            public void AddNode_Test()
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Assert.Throws<ArgumentNullException>(() => base.AddNode(null));
            }

            public void AddEdge_Test()
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Assert.Throws<ArgumentNullException>(() => base.AddEdge(null));
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var populator = new MsaglDefaultGraphPopulator<int, Edge<int>>(graph);
            AssertPopulatorProperties(populator, graph);

            var undirectedGraph = new UndirectedGraph<int, Edge<int>>();
            populator = new MsaglDefaultGraphPopulator<int, Edge<int>>(undirectedGraph);
            AssertPopulatorProperties(populator, undirectedGraph);

            #region Local function

            void AssertPopulatorProperties<TVertex, TEdge>(
                MsaglDefaultGraphPopulator<TVertex, TEdge> p,
                IEdgeListGraph<TVertex, TEdge> g)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(p, g);
                Assert.IsNull(p.MsaglGraph);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new MsaglDefaultGraphPopulator<int, Edge<int>>(null));
        }

        [Test]
        public void Compute()
        {
            Compute_Test(graph => new MsaglDefaultGraphPopulator<int, Edge<int>>(graph));
        }

        [Test]
        public void Handlers()
        {
            Handlers_Test(graph => new MsaglDefaultGraphPopulator<int, Edge<int>>(graph));
        }

        [Test]
        public void Add_Throws()
        {
            var testObject = new TestMsaglDefaultGraphPopulator();
            testObject.AddNode_Test();
            testObject.AddEdge_Test();
        }
    }
}