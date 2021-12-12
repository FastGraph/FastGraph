#nullable enable

using NUnit.Framework;
using FastGraph.Tests;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.MSAGL.Tests
{
    /// <summary>
    /// Tests related to <see cref="MsaglDefaultGraphPopulator{TVertex,TEdge}"/>.
    /// </summary>
    internal sealed class MsaglDefaultGraphPopulatorTests : MsaglGraphPopulatorTestsBase
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
#pragma warning disable CS8625
                Assert.Throws<ArgumentNullException>(() => base.AddNode(default));
#pragma warning restore CS8625
            }

            public void AddEdge_Test()
            {
                // ReSharper disable once AssignNullToNotNullAttribute
#pragma warning disable CS8625
                Assert.Throws<ArgumentNullException>(() => base.AddEdge(default));
#pragma warning restore CS8625
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
                where TVertex : notnull
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
#pragma warning disable CS8625
            Assert.Throws<ArgumentNullException>(
                () => new MsaglDefaultGraphPopulator<int, Edge<int>>(default));
#pragma warning restore CS8625
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
