using System;
using System.Diagnostics.Contracts;

namespace QuikGraph.MSAGL
{
    public static class GleeGraphExtensions
    {
        public static GleeGraphPopulator<TVertex, TEdge> CreateGleePopulator<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> visitedGraph,
            IFormatProvider formatProvider,
            string format)
            where TEdge : IEdge<TVertex>
        {
            return new GleeToStringGraphPopulator<TVertex, TEdge>(visitedGraph, formatProvider, format);
        }

        public static GleeGraphPopulator<TVertex, TEdge> CreateGleePopulator<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> visitedGraph)
            where TEdge : IEdge<TVertex>
        {
            return new GleeDefaultGraphPopulator<TVertex, TEdge>(visitedGraph);
        }

        public static GleeGraphPopulator<TVertex, TEdge> CreateIdentifiableGleePopulator<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> visitedGraph,
            VertexIdentity<TVertex> vertexIdentities)
            where TEdge : IEdge<TVertex>
        {
            return new GleeIndentifiableGraphPopulator<TVertex, TEdge>(visitedGraph, vertexIdentities);
        }

        public static Microsoft.Msagl.Drawing.Graph ToGleeGraph<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> visitedGraph,
            GleeVertexNodeEventHandler<TVertex> nodeAdded,
            GleeEdgeEventHandler<TVertex, TEdge> edgeAdded)
            where TEdge : IEdge<TVertex>
        {
            if (visitedGraph == null)
                throw new ArgumentNullException("visitedGraph");

            var populator = CreateGleePopulator(visitedGraph);
            try
            {
                if (nodeAdded != null)
                    populator.NodeAdded += nodeAdded;
                if (edgeAdded != null)
                    populator.EdgeAdded += edgeAdded;

                populator.Compute();
                return populator.GleeGraph;
            }
            finally
            {
                if (nodeAdded != null)
                    populator.NodeAdded -= nodeAdded;
                if (edgeAdded != null)
                    populator.EdgeAdded -= edgeAdded;
            }
        }

        public static Microsoft.Msagl.Drawing.Graph ToIdentifiableGleeGraph<TVertex, TEdge>(
            this IEdgeListGraph<TVertex, TEdge> visitedGraph,
            VertexIdentity<TVertex> vertexIdentities,
            GleeVertexNodeEventHandler<TVertex> nodeAdded,
            GleeEdgeEventHandler<TVertex, TEdge> edgeAdded
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
                return populator.GleeGraph;
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