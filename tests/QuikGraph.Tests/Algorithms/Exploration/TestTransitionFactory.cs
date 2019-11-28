using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Exploration;

namespace QuikGraph.Tests.Algorithms.Exploration
{
    internal sealed class TestTransitionFactory<TVertex> : ITransitionFactory<TVertex, Edge<TVertex>>
        where TVertex : CloneableTestVertex
    {
        private readonly Dictionary<TVertex, List<Edge<TVertex>>> _edges =
            new Dictionary<TVertex, List<Edge<TVertex>>>();

        public struct VertexEdgesSet
        {
            public VertexEdgesSet(
                [NotNull] TVertex vertex,
                [NotNull, ItemNotNull] IEnumerable<Edge<TVertex>> edges)
            {
                Vertex = vertex;
                Edges = edges;
            }

            [NotNull]
            public TVertex Vertex { get; }

            [NotNull, ItemNotNull]
            public IEnumerable<Edge<TVertex>> Edges { get; }
        }

        public TestTransitionFactory(
            [NotNull] TVertex vertex,
            [NotNull, ItemNotNull] IEnumerable<Edge<TVertex>> edges)
        {
            AddEdgeSet(vertex, edges);
        }

        public TestTransitionFactory([NotNull] IEnumerable<VertexEdgesSet> sets)
        {
            foreach (VertexEdgesSet set in sets)
            {
                AddEdgeSet(set.Vertex, set.Edges);
            }
        }

        private void AddEdgeSet(
            [NotNull] TVertex vertex,
            [NotNull, ItemNotNull] IEnumerable<Edge<TVertex>> edges)
        {
            _edges.Add(vertex, edges.ToList());
        }

        public bool IsValid(TVertex vertex)
        {
            return _edges.ContainsKey(vertex);
        }

        public IEnumerable<Edge<TVertex>> Apply(TVertex source)
        {
            return _edges[source];
        }
    }
}