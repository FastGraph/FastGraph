using System;
using System.Collections.Generic;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.MaximumFlow
{
    /// <summary>
    /// Abstract base class for maximum flow algorithms.
    /// </summary>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public abstract class MaximumFlowAlgorithm<TVertex, TEdge> :
        AlgorithmBase<IMutableVertexAndEdgeListGraph<TVertex, TEdge>>,
        IVertexColorizerAlgorithm<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        private TVertex source;
        private TVertex sink;

        protected MaximumFlowAlgorithm(
            IAlgorithmComponent host,
            IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph,
            Func<TEdge, double> capacities,
            EdgeFactory<TVertex, TEdge> edgeFactory
            )
            : base(host, visitedGraph)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(capacities != null);
#endif

            this.Capacities = capacities;
            this.Predecessors = new Dictionary<TVertex, TEdge>();
            this.EdgeFactory = edgeFactory;
            this.ResidualCapacities = new Dictionary<TEdge, double>();
            this.VertexColors = new Dictionary<TVertex, GraphColor>();
        }

#region Properties

        public Dictionary<TVertex,TEdge> Predecessors { get; private set; }

        public Func<TEdge,double> Capacities { get; private set; }
       
        public Dictionary<TEdge,double> ResidualCapacities { get; private set; }

        public EdgeFactory<TVertex, TEdge> EdgeFactory { get; private set; }

        public Dictionary<TEdge, TEdge> ReversedEdges { get; protected set; }

        public Dictionary<TVertex,GraphColor> VertexColors { get; private set; }

        public GraphColor GetVertexColor(TVertex vertex)
        {
            return this.VertexColors[vertex];
        }

        public TVertex Source
        {
            get { return this.source; }
            set 
            {
#if SUPPORTS_CONTRACTS
                Contract.Requires(value != null);
#endif

                this.source = value; 
            }
        }

        public TVertex Sink
        {
            get { return this.sink; }
            set 
            {
#if SUPPORTS_CONTRACTS
                Contract.Requires(value != null);
#endif
                this.sink = value; 
            }
        }

        public double MaxFlow { get; set; }

#endregion

        public double Compute(TVertex source, TVertex sink)
        {
            this.Source = source;
            this.Sink = sink;
            this.Compute();
            return this.MaxFlow;
        }
    }

}
