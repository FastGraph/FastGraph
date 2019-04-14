using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.Cliques
{
    public abstract class MaximumCliqueAlgorithmBase<TVertex, TEdge>
        : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        protected MaximumCliqueAlgorithmBase(IAlgorithmComponent host, IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {}

        protected MaximumCliqueAlgorithmBase(IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {}
    }
}
