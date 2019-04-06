#if SUPPORTS_SERIALIZATION
using System;
#endif
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuickGraph.Algorithms.Search;

namespace QuickGraph.Algorithms.TopologicalSort
{
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class TopologicalSortAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private IList<TVertex> vertices = new List<TVertex>();
        private bool allowCyclicGraph = false;

        public TopologicalSortAlgorithm(IVertexListGraph<TVertex, TEdge> g)
            : this(g, new List<TVertex>())
        { }

        public TopologicalSortAlgorithm(
            IVertexListGraph<TVertex, TEdge> g,
            IList<TVertex> vertices)
            : base(g)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
#endif

            this.vertices = vertices;
        }


        public IList<TVertex> SortedVertices
        {
            get
            {
                return vertices;
            }
        }

        public bool AllowCyclicGraph
        {
            get { return this.allowCyclicGraph; }
        }

        private void BackEdge(TEdge args)
        {
            if (!this.AllowCyclicGraph)
                throw new NonAcyclicGraphException();
        }

        private void VertexFinished(TVertex v)
        {
            vertices.Insert(0, v);
        }

        public event VertexAction<TVertex> DiscoverVertex;
        public event VertexAction<TVertex> FinishVertex;

        protected override void InternalCompute()
        {
            DepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
            try
            {
                dfs = new DepthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    this.VisitedGraph,
                    new Dictionary<TVertex, GraphColor>(this.VisitedGraph.VertexCount)
                    );
                dfs.BackEdge += BackEdge;
                dfs.FinishVertex += VertexFinished;
                dfs.DiscoverVertex += DiscoverVertex;
                dfs.FinishVertex += FinishVertex;

                dfs.Compute();
            }
            finally
            {
                if (dfs != null)
                {
                    dfs.BackEdge -= BackEdge;
                    dfs.FinishVertex -= VertexFinished;
                    dfs.DiscoverVertex -= DiscoverVertex;
                    dfs.FinishVertex -= FinishVertex;
                }
            }
        }

        public void Compute(IList<TVertex> vertices)
        {
            this.vertices = vertices;
            this.vertices.Clear();
            this.Compute();
        }
    }
}