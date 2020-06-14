using System;
using NUnit.Framework;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.MSAGL.Tests
{
    /// <summary>
    /// Tests related to <see cref="MsaglIdentifiableGraphPopulator{TVertex,TEdge}"/>.
    /// </summary>
    internal class MsaglIdentifiableGraphPopulatorTests : MsaglGraphPopulatorTestsBase
    {
        [Test]
        public void Constructor()
        {
            VertexIdentity<int> vertexIdentity = vertex => vertex.ToString();
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var populator = new MsaglIdentifiableGraphPopulator<int, Edge<int>>(graph, vertexIdentity);
            AssertPopulatorProperties(populator, graph);

            var undirectedGraph = new UndirectedGraph<int, Edge<int>>();
            populator = new MsaglIdentifiableGraphPopulator<int, Edge<int>>(undirectedGraph, vertexIdentity);
            AssertPopulatorProperties(populator, undirectedGraph);

            #region Local function

            void AssertPopulatorProperties<TVertex, TEdge>(
                MsaglIdentifiableGraphPopulator<TVertex, TEdge> p,
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
            VertexIdentity<int> vertexIdentity = vertex => vertex.ToString();
            var graph = new AdjacencyGraph<int, Edge<int>>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new MsaglIdentifiableGraphPopulator<int, Edge<int>>(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new MsaglIdentifiableGraphPopulator<int, Edge<int>>(null, vertexIdentity));
            Assert.Throws<ArgumentNullException>(
                () => new MsaglIdentifiableGraphPopulator<int, Edge<int>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Compute()
        {
            Compute_Test(graph => new MsaglIdentifiableGraphPopulator<int, Edge<int>>(graph, vertex => vertex.ToString()));
        }

        [Test]
        public void Handlers()
        {
            Handlers_Test(graph => new MsaglIdentifiableGraphPopulator<int, Edge<int>>(graph, vertex => vertex.ToString()));
        }

        [Test]
        public void VertexId()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange(new[]
            {
                new Edge<int>(1, 2),
                new Edge<int>(2, 3)
            });
            graph.AddVertexRange(new[] { 5, 6 });

            var populator = new MsaglIdentifiableGraphPopulator<int, Edge<int>>(graph, vertex => $"MyTestId{vertex}");
            populator.Compute();

            // Check vertices has been well formatted
            Assert.IsNull(populator.MsaglGraph.FindNode("MyTestId0"));
            Assert.IsNotNull(populator.MsaglGraph.FindNode("MyTestId1"));
        }
    }
}