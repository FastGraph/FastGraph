using NUnit.Framework;
using QuikGraph.Algorithms.RandomWalks;

namespace QuikGraph.Tests.Algorithms.RandomWalks
{
    /// <summary>
    /// Tests for <see cref="CyclePoppingRandomTreeAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal class CyclePoppingRandomTreeAlgorithmTests
    {
        [Test]
        public void CyclePoppingRandomTreeAll()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs())
            {
                foreach (string root in graph.Vertices)
                {
                    var target = new CyclePoppingRandomTreeAlgorithm<string, Edge<string>>(graph);
                    target.Compute(root);
                }
            }
        }

        [Test]
        public void Repro13160()
        {
            // Create a new graph
            var graph = new BidirectionalGraph<int, SEquatableEdge<int>>(false);

            // Adding vertices
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                    graph.AddVertex(i * 3 + j);
            }

            // Adding Width edges
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    graph.AddEdge(
                        new SEquatableEdge<int>(i * 3 + j, i * 3 + j + 1));
                }
            }

            // Adding Length edges
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    graph.AddEdge(
                        new SEquatableEdge<int>(i * 3 + j, (i + 1) * 3 + j));
                }
            }

            // Create cross edges 
            foreach (SEquatableEdge<int> edge in graph.Edges)
                graph.AddEdge(new SEquatableEdge<int>(edge.Target, edge.Source));

            // Breaking graph apart
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (i == 1)
                        graph.RemoveVertex(i * 3 + j);
                }
            }

            var target = new CyclePoppingRandomTreeAlgorithm<int, SEquatableEdge<int>>(graph);
            target.Compute(2);
        }

        [Test]
        public void IsolatedVertices()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(true);
            graph.AddVertex(0);
            graph.AddVertex(1);

            var target = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph);
            target.RandomTree();
        }

        [Test]
        public void IsolatedVerticesWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>(true);
            graph.AddVertex(0);
            graph.AddVertex(1);

            var target = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph);
            target.RandomTreeWithRoot(0);
        }

        [Test]
        public void RootIsNotAccessible()
        {
            AdjacencyGraph<int, Edge<int>> graph = new AdjacencyGraph<int, Edge<int>>(true);
            graph.AddVertex(0);
            graph.AddVertex(1);
            graph.AddEdge(new Edge<int>(0, 1));

            var target = new CyclePoppingRandomTreeAlgorithm<int, Edge<int>>(graph);
            target.RandomTreeWithRoot(0);
        }
    }
}