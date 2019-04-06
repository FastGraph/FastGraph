using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif

namespace QuickGraph.Algorithms.Observers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TVertex">type of a vertex</typeparam>
    /// <typeparam name="TEdge">type of an edge</typeparam>
    /// <reference-ref idref="boost" />
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class VertexRecorderObserver<TVertex, TEdge> : IObserver<IVertexTimeStamperAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly IList<TVertex> vertices;
        public VertexRecorderObserver()
            : this(new List<TVertex>())
        { }

        public VertexRecorderObserver(IList<TVertex> vertices)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
#endif

            this.vertices = vertices;
        }

        public IEnumerable<TVertex> Vertices
        {
            get
            {
                return this.vertices;
            }
        }

        public IDisposable Attach(IVertexTimeStamperAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.DiscoverVertex += algorithm_DiscoverVertex;
            return new DisposableAction(() => algorithm.DiscoverVertex -= algorithm_DiscoverVertex);
        }

        void algorithm_DiscoverVertex(TVertex v)
        {
            this.vertices.Add(v);
        }
    }
}
