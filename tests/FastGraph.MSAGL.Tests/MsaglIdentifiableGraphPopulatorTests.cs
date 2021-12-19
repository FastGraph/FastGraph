#nullable enable

using NUnit.Framework;
using static FastGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace FastGraph.MSAGL.Tests
{
    /// <summary>
    /// Tests related to <see cref="MsaglIdentifiableGraphPopulator{TVertex,TEdge}"/>.
    /// </summary>
    internal sealed class MsaglIdentifiableGraphPopulatorTests : MsaglGraphPopulatorTestsBase
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
                where TVertex : notnull
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(p, g);
                p.MsaglGraph.Should().BeNull();
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
#pragma warning disable CS8625
            Invoking(() => new MsaglIdentifiableGraphPopulator<int, Edge<int>>(graph, default)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MsaglIdentifiableGraphPopulator<int, Edge<int>>(default, vertexIdentity)).Should().Throw<ArgumentNullException>();
            Invoking(() => new MsaglIdentifiableGraphPopulator<int, Edge<int>>(default, default)).Should().Throw<ArgumentNullException>();
#pragma warning restore CS8625
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
            populator.MsaglGraph!.FindNode("MyTestId0").Should().BeNull();
            populator.MsaglGraph.FindNode("MyTestId1").Should().NotBeNull();
        }
    }
}
