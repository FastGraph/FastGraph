using System;
using System.Diagnostics.Contracts;

namespace QuikGraph.MSAGL
{
    public static class MsaglGraphExtensions
    {
        public static MsaglGraphPopulator<TVertex, TEdge> CreateMsaglPopulator<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> visitedGraph,
            IFormatProvider formatProvider,
            string format)
            where TEdge : IEdge<TVertex>
        {
            return new MsaglToStringGraphPopulator<TVertex, TEdge>(visitedGraph, formatProvider, format);
        }

        public static MsaglGraphPopulator<TVertex, TEdge> CreateMsaglPopulator<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> visitedGraph)
            where TEdge : IEdge<TVertex>
        {
            return new MsaglDefaultGraphPopulator<TVertex, TEdge>(visitedGraph);
        }

        public static MsaglGraphPopulator<TVertex, TEdge> CreateIdentifiableGleePopulator<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> visitedGraph,
            VertexIdentity<TVertex> vertexIdentities)
            where TEdge : IEdge<TVertex>
        {
            return new MsaglIndentifiableGraphPopulator<TVertex, TEdge>(visitedGraph, vertexIdentities);
        }

        public static Microsoft.Msagl.Drawing.Graph ToMsaglGraph<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> visitedGraph,
            MsaglVertexNodeEventHandler<TVertex> nodeAdded,
            MsaglEdgeEventHandler<TVertex, TEdge> edgeAdded)
            where TEdge : IEdge<TVertex>
        {
            if (visitedGraph == null)
                throw new ArgumentNullException("visitedGraph");

            var populator = CreateMsaglPopulator(visitedGraph);
            try
            {
                if (nodeAdded != null)
                    populator.NodeAdded += nodeAdded;
                if (edgeAdded != null)
                    populator.EdgeAdded += edgeAdded;

                populator.Compute();
                return populator.MsaglGraph;
            }
            finally
            {
                if (nodeAdded != null)
                    populator.NodeAdded -= nodeAdded;
                if (edgeAdded != null)
                    populator.EdgeAdded -= edgeAdded;
            }
        }

        public static Microsoft.Msagl.Drawing.Graph ToIdentifiableMsaglGraph<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> visitedGraph,
            VertexIdentity<TVertex> vertexIdentities,
            MsaglVertexNodeEventHandler<TVertex> nodeAdded,
            MsaglEdgeEventHandler<TVertex, TEdge> edgeAdded
            )
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(visitedGraph != null);
            Contract.Requires(vertexIdentities != null);

            var populator = CreateIdentifiableGleePopulator(visitedGraph, vertexIdentities);
            try
            {
                if (nodeAdded != null)
                    populator.NodeAdded += nodeAdded;
                if (edgeAdded != null)
                    populator.EdgeAdded += edgeAdded;

                populator.Compute();
                return populator.MsaglGraph;
            }
            finally
            {
                if (nodeAdded != null)
                    populator.NodeAdded -= nodeAdded;
                if (edgeAdded != null)
                    populator.EdgeAdded -= edgeAdded;
            }
        }
    }
}