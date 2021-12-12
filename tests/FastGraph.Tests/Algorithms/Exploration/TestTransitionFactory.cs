#nullable enable

using FastGraph.Algorithms.Exploration;

namespace FastGraph.Tests.Algorithms.Exploration
{
    internal sealed class TestTransitionFactory<TVertex> : ITransitionFactory<TVertex, Edge<TVertex>>
        where TVertex : CloneableTestVertex
    {
        private readonly Dictionary<TVertex, List<Edge<TVertex>>> _edges =
            new Dictionary<TVertex, List<Edge<TVertex>>>();

        public struct VertexEdgesSet
        {
            public VertexEdgesSet(
                TVertex vertex,
                IEnumerable<Edge<TVertex>> edges)
            {
                Vertex = vertex;
                Edges = edges;
            }

            public TVertex Vertex { get; }

            public IEnumerable<Edge<TVertex>> Edges { get; }
        }

        public TestTransitionFactory(
            TVertex vertex,
            IEnumerable<Edge<TVertex>> edges)
        {
            AddEdgeSet(vertex, edges);
        }

        public TestTransitionFactory(IEnumerable<VertexEdgesSet> sets)
        {
            foreach (VertexEdgesSet set in sets)
            {
                AddEdgeSet(set.Vertex, set.Edges);
            }
        }

        private void AddEdgeSet(
            TVertex vertex,
            IEnumerable<Edge<TVertex>> edges)
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
