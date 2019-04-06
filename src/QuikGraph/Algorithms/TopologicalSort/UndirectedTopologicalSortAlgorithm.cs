using System;
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
    public sealed class UndirectedTopologicalSortAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private IList<TVertex> vertices;
        private bool allowCyclicGraph = false;

        public UndirectedTopologicalSortAlgorithm(IUndirectedGraph<TVertex, TEdge> g)
            : this(g, new List<TVertex>())
        { }

        public UndirectedTopologicalSortAlgorithm(
            IUndirectedGraph<TVertex, TEdge> g,
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
            set { this.allowCyclicGraph = value; }
        }

        private void BackEdge(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> a)
        {
            if (!this.AllowCyclicGraph)
                throw new NonAcyclicGraphException();
        }

        private void FinishVertex(TVertex v)
        {
            vertices.Insert(0, v);
        }

        protected override void InternalCompute()
        {
            UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge> dfs = null;
            try
            {
                dfs = new UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    new Dictionary<TVertex, GraphColor>(this.VisitedGraph.VertexCount)
                    );
                dfs.BackEdge += BackEdge;
                dfs.FinishVertex += FinishVertex;

                dfs.Compute();
            }
            finally
            {
                if (dfs != null)
                {
                    dfs.BackEdge -= BackEdge;
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
