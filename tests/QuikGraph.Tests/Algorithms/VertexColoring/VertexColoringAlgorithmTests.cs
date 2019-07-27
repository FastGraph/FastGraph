using System.Linq;
using NUnit.Framework;
using QuikGraph.Algorithms.VertexColoring;

namespace QuikGraph.Tests.Algorithms.GraphColoring
{
    [TestFixture]
    internal class VertexColoringAlgorithmTests : QuikGraphUnitTests
    {
        [Test]
        public void VertexColoringComputeSimpleGraph()
        {
            /* 
                                                  (1)
                                                 / | \ 
            Generate undirected simple graph: (0)  |  (3)-(4)
                                                 \ | /
                                                  (2)
            */
            var input = GenerateInputSimple();
            var algorithm = new VertexColoringAlgorithm<char, Edge<char>>(input);
            algorithm.Compute();

            var coloredVertices = algorithm.Colors;

            // Graph doesn't have third vertex color
            Assert.IsFalse(coloredVertices.Values.Contains(3));

            var result = coloredVertices.Values.ToArray();

            // Expecting to get 3 different colors
            Assert.AreEqual(3, result.Max() + 1);

            // Not equal to null 
            foreach (int? color in result)
            {
                Assert.AreNotEqual(null, color);
            }

            // and corresponding colors of vertices
            Assert.AreEqual(0, result[0]); // 0 vertex = 0 color
            Assert.AreEqual(1, result[1]); // 1 vertex = 1 color
            Assert.AreEqual(2, result[2]); // 2 vertex = 2 color
            Assert.AreEqual(0, result[3]); // 3 vertex = 0 color
            Assert.AreEqual(1, result[4]); // 4 vertex = 1 color

            #region Local function

            UndirectedGraph<char, Edge<char>> GenerateInputSimple()
            {
                var graph = new UndirectedGraph<char, Edge<char>>(true);

                graph.AddVertex('0'); // 1 Vertex
                graph.AddVertex('1'); // 2 Vertex
                graph.AddVertex('2'); // 3 Vertex
                graph.AddVertex('3'); // 4 Vertex
                graph.AddVertex('4'); // 5 Vertex

                graph.AddEdge(new Edge<char>('0', '1')); // 1 Edge
                graph.AddEdge(new Edge<char>('0', '2')); // 2 Edge
                graph.AddEdge(new Edge<char>('1', '2')); // 3 Edge
                graph.AddEdge(new Edge<char>('1', '3')); // 4 Edge
                graph.AddEdge(new Edge<char>('2', '3')); // 5 Edge
                graph.AddEdge(new Edge<char>('3', '4')); // 6 Edge

                return graph;
            }

            #endregion
        }

        [Test]
        public void VertexColoringComputeEmptyGraph()
        {
            /* 
                                      (1)
                                                     
            Generate empty graph: (0)     (3) (4)
                                                     
                                      (2)
            */
            var input = GenerateInputEmpty();
            var algorithm = new VertexColoringAlgorithm<char, Edge<char>>(input);
            algorithm.Compute();

            var coloredVertices = algorithm.Colors;

            // Graph doesn't have first vertex color
            Assert.IsFalse(coloredVertices.Values.Contains(1));

            var result = coloredVertices.Values.ToArray();

            // Expecting to get only 1 color
            Assert.AreEqual(1, result.Max() + 1);

            // Not equal to null 
            foreach (int? color in result)
            {
                Assert.AreNotEqual(null, color);
            }

            // and corresponding colors of vertices
            Assert.AreEqual(0, result[0]); // 0 vertex = 0 color
            Assert.AreEqual(0, result[1]); // 1 vertex = 0 color
            Assert.AreEqual(0, result[2]); // 2 vertex = 0 color
            Assert.AreEqual(0, result[3]); // 3 vertex = 0 color
            Assert.AreEqual(0, result[4]); // 4 vertex = 0 color

            #region Local function

            UndirectedGraph<char, Edge<char>> GenerateInputEmpty()
            {
                var graph = new UndirectedGraph<char, Edge<char>>(true);

                graph.AddVertex('0'); // 1 Vertex
                graph.AddVertex('1'); // 2 Vertex
                graph.AddVertex('2'); // 3 Vertex
                graph.AddVertex('3'); // 4 Vertex
                graph.AddVertex('4'); // 5 Vertex

                return graph;
            }

            #endregion
        }

        [Test]
        public void VertexColoringComputeFullGraph()
        {
            /* 
                                                _____(2)_____
                                               /    / | \    \
            Generate undirected full graph:  (0)-(1)--+--(4)-(5)  + edges: (0-4), (0-5) and (1-5)
                                               \    \ | /    /
                                                \____(3)____/
            */
            var input = GenerateInputFull();
            var algorithm = new VertexColoringAlgorithm<char, Edge<char>>(input);
            algorithm.Compute();

            var coloredVertices = algorithm.Colors;

            // Graph doesn't have sixth vertex color
            Assert.IsFalse(coloredVertices.Values.Contains(6));

            var result = coloredVertices.Values.ToArray();

            // Expecting to get 6 different colors
            Assert.AreEqual(6, result.Max() + 1);

            // Not equal to null 
            foreach (int? color in result)
            {
                Assert.AreNotEqual(null, color);
            }

            // and corresponding colors of vertices
            Assert.AreEqual(0, result[0]); // 0 vertex = 0 color
            Assert.AreEqual(1, result[1]); // 1 vertex = 1 color
            Assert.AreEqual(2, result[2]); // 2 vertex = 2 color
            Assert.AreEqual(3, result[3]); // 3 vertex = 3 color
            Assert.AreEqual(4, result[4]); // 4 vertex = 4 color
            Assert.AreEqual(5, result[5]); // 5 vertex = 5 color

            #region Local function

            UndirectedGraph<char, Edge<char>> GenerateInputFull()
            {
                var graph = new UndirectedGraph<char, Edge<char>>(true);

                graph.AddVertex('0'); // 1 Vertex
                graph.AddVertex('1'); // 2 Vertex
                graph.AddVertex('2'); // 3 Vertex
                graph.AddVertex('3'); // 4 Vertex
                graph.AddVertex('4'); // 5 Vertex
                graph.AddVertex('5'); // 6 Vertex

                graph.AddEdge(new Edge<char>('0', '1')); // 1  Edge
                graph.AddEdge(new Edge<char>('0', '2')); // 2  Edge
                graph.AddEdge(new Edge<char>('0', '3')); // 3  Edge
                graph.AddEdge(new Edge<char>('0', '4')); // 4  Edge
                graph.AddEdge(new Edge<char>('0', '5')); // 5  Edge
                graph.AddEdge(new Edge<char>('1', '2')); // 6  Edge
                graph.AddEdge(new Edge<char>('1', '3')); // 7  Edge
                graph.AddEdge(new Edge<char>('1', '4')); // 8  Edge
                graph.AddEdge(new Edge<char>('1', '5')); // 9  Edge
                graph.AddEdge(new Edge<char>('2', '3')); // 10 Edge
                graph.AddEdge(new Edge<char>('2', '4')); // 11 Edge
                graph.AddEdge(new Edge<char>('2', '5')); // 12 Edge
                graph.AddEdge(new Edge<char>('3', '4')); // 13 Edge
                graph.AddEdge(new Edge<char>('3', '5')); // 14 Edge
                graph.AddEdge(new Edge<char>('4', '5')); // 15 Edge

                return graph;
            }

            #endregion
        }

        [Test]
        public void VertexColoringComputeBipartiteGraph()
        {
            /*                                   
                                                 (3)
                                                / 
                                             (1)-(4)
                                                X     
            Generate undirected empty graph: (0)-(5)    + edges: (1-6) and (2-4)
                                                /     
                                             (2)-(6)
            
            */

            var input = GenerateInputBipartite();
            var algorithm = new VertexColoringAlgorithm<char, Edge<char>>(input);
            algorithm.Compute();

            var coloredVertices = algorithm.Colors;

            // Graph doesn't have second vertex color
            Assert.IsFalse(coloredVertices.Values.Contains(2));

            var result = coloredVertices.Values.ToArray();

            // Expecting to get 2 different colors
            Assert.AreEqual(2, result.Max() + 1);

            // Not equal to null 
            foreach (int? color in result)
            {
                Assert.AreNotEqual(null, color);
            }

            // and corresponding colors of vertices
            Assert.AreEqual(0, result[0]); // 0 vertex = 0 color
            Assert.AreEqual(0, result[1]); // 1 vertex = 0 color
            Assert.AreEqual(0, result[2]); // 2 vertex = 0 color
            Assert.AreEqual(1, result[3]); // 3 vertex = 1 color
            Assert.AreEqual(1, result[4]); // 4 vertex = 1 color
            Assert.AreEqual(1, result[5]); // 5 vertex = 1 color
            Assert.AreEqual(1, result[6]); // 6 vertex = 1 color

            #region Local function

            UndirectedGraph<char, Edge<char>> GenerateInputBipartite()
            {
                var graph = new UndirectedGraph<char, Edge<char>>(true);

                graph.AddVertex('0'); // 1 Vertex
                graph.AddVertex('1'); // 2 Vertex
                graph.AddVertex('2'); // 3 Vertex
                graph.AddVertex('3'); // 4 Vertex
                graph.AddVertex('4'); // 5 Vertex
                graph.AddVertex('5'); // 6 Vertex
                graph.AddVertex('6'); // 7 Vertex

                graph.AddEdge(new Edge<char>('0', '4')); // 1 Edge
                graph.AddEdge(new Edge<char>('0', '5')); // 2 Edge
                graph.AddEdge(new Edge<char>('1', '3')); // 3 Edge
                graph.AddEdge(new Edge<char>('1', '4')); // 4 Edge
                graph.AddEdge(new Edge<char>('1', '5')); // 5 Edge
                graph.AddEdge(new Edge<char>('1', '6')); // 6 Edge
                graph.AddEdge(new Edge<char>('2', '5')); // 7 Edge
                graph.AddEdge(new Edge<char>('2', '6')); // 8 Edge
                graph.AddEdge(new Edge<char>('2', '4')); // 9 Edge

                return graph;
            }

            #endregion
        }

        [Test]
        public void VertexColoringComputeTestGraph()
        {
            /* 
                                                  (2)      (7)-(5)
                                                 /   \     /
            Generate undirected some graph:    (1)   (4)-(0)
                                                 \   /
                                             (6)  (3)
            
            (this graph has a minimum number of vertex colors only if to swap (1) and (4) vertices)
            */
            var input = GenerateInputTest();
            var algorithm = new VertexColoringAlgorithm<char, Edge<char>>(input);
            algorithm.Compute();

            var coloredVertices = algorithm.Colors;

            // Graph doesn't have third vertex color
            Assert.IsFalse(coloredVertices.Values.Contains(3));

            var result = coloredVertices.Values.ToArray();

            // Expecting to get 3 different colors
            Assert.AreEqual(3, result.Max() + 1);

            // Not equal to null 
            foreach (int? color in result)
            {
                Assert.AreNotEqual(null, color);
            }

            // And corresponding colors of vertices
            Assert.AreEqual(0, result[0]); // 0 vertex = 0 color
            Assert.AreEqual(0, result[1]); // 1 vertex = 0 color
            Assert.AreEqual(1, result[2]); // 2 vertex = 1 color
            Assert.AreEqual(1, result[3]); // 3 vertex = 1 color
            Assert.AreEqual(2, result[4]); // 4 vertex = 2 color
            Assert.AreEqual(0, result[5]); // 5 vertex = 0 color
            Assert.AreEqual(0, result[6]); // 6 vertex = 0 color
            Assert.AreEqual(1, result[7]); // 7 vertex = 1 color

            #region Local function

            UndirectedGraph<char, Edge<char>> GenerateInputTest()
            {
                var graph = new UndirectedGraph<char, Edge<char>>(true);

                graph.AddVertex('0'); // 1 Vertex
                graph.AddVertex('1'); // 2 Vertex
                graph.AddVertex('2'); // 3 Vertex
                graph.AddVertex('3'); // 4 Vertex
                graph.AddVertex('4'); // 5 Vertex
                graph.AddVertex('5'); // 6 Vertex
                graph.AddVertex('6'); // 7 Vertex
                graph.AddVertex('7'); // 8 Vertex

                graph.AddEdge(new Edge<char>('0', '4')); // 1 Edge
                graph.AddEdge(new Edge<char>('1', '2')); // 2 Edge
                graph.AddEdge(new Edge<char>('1', '3')); // 3 Edge
                graph.AddEdge(new Edge<char>('2', '4')); // 4 Edge
                graph.AddEdge(new Edge<char>('3', '4')); // 5 Edge
                graph.AddEdge(new Edge<char>('5', '7')); // 6 Edge
                graph.AddEdge(new Edge<char>('7', '0')); // 7 Edge

                return graph;
            }

            #endregion
        }
    }
}