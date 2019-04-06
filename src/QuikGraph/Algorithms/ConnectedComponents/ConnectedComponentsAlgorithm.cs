#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuickGraph.Algorithms.Search;
using QuickGraph.Algorithms.Services;

namespace QuickGraph.Algorithms.ConnectedComponents
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class ConnectedComponentsAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>,
        IConnectedComponentAlgorithm<TVertex,TEdge,IUndirectedGraph<TVertex,TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private IDictionary<TVertex, int> components;
        private int componentCount=0;

        public ConnectedComponentsAlgorithm(IUndirectedGraph<TVertex, TEdge> g)
            :this(g, new Dictionary<TVertex, int>())
        { }

        public ConnectedComponentsAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, int> components)
            : this(null, visitedGraph, components)
        { }

        public ConnectedComponentsAlgorithm(
            IAlgorithmComponent host,
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, int> components)
            :base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(components != null);
#endif

            this.components = components;
        }

        public IDictionary<TVertex,int> Components
        {
            get
            {
                return this.components;
            }
        }

        public int ComponentCount
        {
            get { return this.componentCount; }
        }

        private void StartVertex(TVertex v)
        {
            ++this.componentCount;
        }

        private void DiscoverVertex(TVertex v)
        {
            this.Components[v] = this.componentCount;
        }

        protected override void InternalCompute()
        {
            this.components.Clear();
            if (this.VisitedGraph.VertexCount == 0)
            {
                this.componentCount = 0;
                return;
            }

            this.componentCount = -1;
            UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
            try
            {
                dfs = new UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    this.VisitedGraph,
                    new Dictionary<TVertex, GraphColor>(this.VisitedGraph.VertexCount)
                    );

                dfs.StartVertex += StartVertex;
                dfs.DiscoverVertex += DiscoverVertex;
                dfs.Compute();
                ++this.componentCount;
            }
            finally
            {
                if (dfs != null)
                {
                    dfs.StartVertex -= StartVertex;
                    dfs.DiscoverVertex -= DiscoverVertex;
                }
            }
        }
    }
}
