using System;
using JetBrains.Annotations;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Helpers to serialize a graph to be displayed in <see cref="Console"/>.
    /// </summary>
    internal static class GraphConsoleSerializer
    {
        public static void DisplayGraph<TVertex, TEdge>([NotNull] IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            Console.WriteLine($"{graph.VertexCount} vertices, {graph.EdgeCount} edges");
            foreach (TVertex vertex in graph.Vertices)
                Console.WriteLine($"\t{vertex}");
            foreach (TEdge edge in graph.Edges)
                Console.WriteLine($"\t{edge}");
        }

        public static void DisplayGraph<TVertex, TEdge>([NotNull] IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            Console.WriteLine($"{graph.VertexCount} vertices");
            foreach (TVertex vertex in graph.Vertices)
            {
                foreach (TEdge edge in graph.OutEdges(vertex))
                    Console.WriteLine($"\t{edge}");
            }
        }
    }
}
