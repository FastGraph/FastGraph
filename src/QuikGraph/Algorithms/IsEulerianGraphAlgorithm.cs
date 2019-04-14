using System.Linq;
using QuikGraph.Algorithms.ConnectedComponents;

namespace QuikGraph.Algorithms
{
    public enum ComponentWithEdges { NoComponent, OneComponent, ManyComponents }

    public class IsEulerianGraphAlgorithm<TVertex, TEdge> where TEdge : IUndirectedEdge<TVertex>
    {
        private UndirectedGraph<TVertex, UndirectedEdge<TVertex>> graph;

        public IsEulerianGraphAlgorithm(UndirectedGraph<TVertex, UndirectedEdge<TVertex>> graph)
        {
            var newGraph = new UndirectedGraph<TVertex, UndirectedEdge<TVertex>>(false, graph.EdgeEqualityComparer);
            newGraph.AddVertexRange(graph.Vertices);
            newGraph.AddEdgeRange(graph.Edges);
            EdgePredicate<TVertex, UndirectedEdge<TVertex>> isLoop = e => e.Source.Equals(e.Target);
            newGraph.RemoveEdgeIf(isLoop);
            this.graph = newGraph;
        }

        private struct TrueIndexes
        {
            public TrueIndexes(int? firstIndex, int? secondIndex)
            {
                FirstIndex = firstIndex;
                SecondIndex = secondIndex;
            }

            public int? FirstIndex { get; }
            public int? SecondIndex { get; }
        }

        private TrueIndexes FirstAndSecondIndexOfTrue(bool[] data)
        {
            // If no true elements returns (null, null)
            // If only one true element, returns (indexOfTrue, null)
            int? firstIndex = null, secondIndex = null;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i])
                {
                    if (!firstIndex.HasValue)
                    {
                        firstIndex = i;
                    }
                    else
                    {
                        return new TrueIndexes(firstIndex, i);
                    }
                }
            }
            return new TrueIndexes(firstIndex, secondIndex);
        }

        public ComponentWithEdges CheckComponentsWithEdges()
        {
            var componentsAlgo = new ConnectedComponentsAlgorithm<TVertex, UndirectedEdge<TVertex>>(this.graph);
            componentsAlgo.Compute();

            bool[] hasEdgesInComponent = new bool[componentsAlgo.ComponentCount];
            foreach (var verticeAndComponent in componentsAlgo.Components)
            {
                hasEdgesInComponent[verticeAndComponent.Value] = !graph.IsAdjacentEdgesEmpty(verticeAndComponent.Key);
            }

            TrueIndexes trueIndexes = FirstAndSecondIndexOfTrue(hasEdgesInComponent);
            if (!trueIndexes.FirstIndex.HasValue)
                return ComponentWithEdges.NoComponent;

            if (trueIndexes.SecondIndex.HasValue)
                return ComponentWithEdges.ManyComponents;

            return ComponentWithEdges.OneComponent;
        }

        public bool SatisfiesEulerianCondition(TVertex vertex)
        {
            return graph.AdjacentDegree(vertex) % 2 == 0;
        }

        public bool IsEulerian()
        {
            switch (CheckComponentsWithEdges())
            {
                case ComponentWithEdges.OneComponent:
                    return graph.Vertices.All<TVertex>(SatisfiesEulerianCondition);
                case ComponentWithEdges.NoComponent:
                    return graph.VertexCount == 1;
                case ComponentWithEdges.ManyComponents:
                default:
                    return false;
            }
        }
    }
}