using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz
{
    /// <summary>
    /// An algorithm that renders a graph to the Graphviz DOT format.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class GraphvizAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [NotNull]
        private readonly Dictionary<TVertex, int> _verticesIds = new Dictionary<TVertex, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="graph">Graph to convert to DOT.</param>
        public GraphvizAlgorithm([NotNull] IEdgeListGraph<TVertex, TEdge> graph)
            : this(graph, GraphvizImageType.Png)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="graph">Graph to convert to DOT.</param>
        /// <param name="imageType">Target output image type.</param>
        public GraphvizAlgorithm(
            [NotNull] IEdgeListGraph<TVertex, TEdge> graph,
            GraphvizImageType imageType)
        {
            ClusterCount = 0;
            _visitedGraph = graph ?? throw new ArgumentNullException(nameof(graph));
            ImageType = imageType;
            GraphFormat = new GraphvizGraph();
            CommonVertexFormat = new GraphvizVertex();
            CommonEdgeFormat = new GraphvizEdge();
        }

        /// <summary>
        /// Graph format.
        /// </summary>
        [NotNull]
        public GraphvizGraph GraphFormat { get; }

        /// <summary>
        /// Common vertex format.
        /// </summary>
        [NotNull]
        public GraphvizVertex CommonVertexFormat { get; }

        /// <summary>
        /// Common edge format.
        /// </summary>
        [NotNull]
        public GraphvizEdge CommonEdgeFormat { get; }

        [NotNull]
        private IEdgeListGraph<TVertex, TEdge> _visitedGraph;

        /// <summary>
        /// Graph to convert.
        /// </summary>
        [NotNull]
        public IEdgeListGraph<TVertex, TEdge> VisitedGraph
        {
            get => _visitedGraph;
            set => _visitedGraph = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Dot output stream.
        /// </summary>
        /// <remarks>Not null after a run of <see cref="Generate()"/> or <see cref="Generate(IDotEngine,string)"/>.</remarks>
        public StringWriter Output { get; private set; }

        /// <summary>
        /// Current image output type.
        /// </summary>
        public GraphvizImageType ImageType { get; set; }

        internal int ClusterCount { get; set; }

        /// <summary>
        /// Fired when formatting a clustered graph.
        /// </summary>
        public event FormatClusterEventHandler<TVertex, TEdge> FormatCluster;

        private void OnFormatCluster([NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> cluster)
        {
            Debug.Assert(cluster != null);

            FormatClusterEventHandler<TVertex, TEdge> formatCluster = FormatCluster;
            if (formatCluster is null)
                return;

            var args = new FormatClusterEventArgs<TVertex, TEdge>(cluster, new GraphvizGraph());
            formatCluster(this, args);
            string dot = args.GraphFormat.ToDot();
            if (dot.Length != 0)
                Output.WriteLine(dot);
        }

        /// <summary>
        /// Fired when formatting a vertex.
        /// </summary>
        public event FormatVertexEventHandler<TVertex> FormatVertex;

        private void OnFormatVertex([NotNull] TVertex vertex)
        {
            Debug.Assert(vertex != null);

            Output.Write($"{_verticesIds[vertex]}");
            FormatVertexEventHandler<TVertex> formatVertex = FormatVertex;
            if (formatVertex != null)
            {
                var vertexFormat = new GraphvizVertex
                {
                    Label = vertex.ToString()
                };
                formatVertex(this, new FormatVertexEventArgs<TVertex>(vertex, vertexFormat));

                string dot = vertexFormat.ToDot();
                if (dot.Length != 0)
                    Output.Write($" [{dot}]");
            }
            Output.WriteLine(";");
        }

        /// <summary>
        /// Fired when formatting an edge.
        /// </summary>
        public event FormatEdgeAction<TVertex, TEdge> FormatEdge;

        private void OnFormatEdge([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            FormatEdgeAction<TVertex, TEdge> formatEdge = FormatEdge;
            if (formatEdge != null)
            {
                var edgeFormat = new GraphvizEdge();
                formatEdge(this, new FormatEdgeEventArgs<TVertex, TEdge>(edge, edgeFormat));

                string dot = edgeFormat.ToDot();
                if (dot.Length != 0)
                    Output.Write($" [{dot}]");
            }
            Output.WriteLine(";");
        }

        /// <summary>
        /// Generates the DOT corresponding to <see cref="VisitedGraph"/>.
        /// </summary>
        /// <returns>DOT serialization of <see cref="VisitedGraph"/>.</returns>
        [Pure]
        [NotNull]
        public string Generate()
        {
            ClusterCount = 0;
            _verticesIds.Clear();
            Output = new StringWriter();
            // Build vertex id map
            int i = 0;
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                _verticesIds.Add(vertex, i++);
            }

            Output.Write(VisitedGraph.IsDirected ? "digraph " : "graph ");
            Output.Write(GraphFormat.Name);
            Output.WriteLine(" {");

            string graphFormat = GraphFormat.ToDot();
            if (graphFormat.Length > 0)
                Output.WriteLine(graphFormat);
            string vertexFormat = CommonVertexFormat.ToDot();
            if (vertexFormat.Length > 0)
                Output.WriteLine($"node [{vertexFormat}];");
            string edgeFormat = CommonEdgeFormat.ToDot();
            if (edgeFormat.Length > 0)
                Output.WriteLine($"edge [{edgeFormat}];");

            // Initialize vertices map
            var verticesColors = new Dictionary<TVertex, GraphColor>();
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                verticesColors[vertex] = GraphColor.White;
            }

            var edgeColors = new Dictionary<TEdge, GraphColor>();
            foreach (TEdge edge in VisitedGraph.Edges)
            {
                edgeColors[edge] = GraphColor.White;
            }

            if (VisitedGraph is IClusteredGraph clusteredGraph)
            {
                WriteClusters(verticesColors, edgeColors, clusteredGraph);
            }

            WriteVertices(verticesColors, VisitedGraph.Vertices);
            WriteEdges(edgeColors, VisitedGraph.Edges);

            Output.Write("}");
            return Output.ToString();
        }

        /// <summary>
        /// Generates the DOT corresponding to <see cref="VisitedGraph"/> using <paramref name="dot"/> engine
        /// and puts result in <paramref name="outputFilePath"/>.
        /// </summary>
        /// <returns>File path containing DOT serialization of <see cref="VisitedGraph"/>.</returns>
        [NotNull]
        public string Generate([NotNull] IDotEngine dot, [NotNull] string outputFilePath)
        {
            if (dot is null)
                throw new ArgumentNullException(nameof(dot));
            if (string.IsNullOrEmpty(outputFilePath))
                throw new ArgumentException("Output file path cannot be null or empty.", nameof(outputFilePath));

            return dot.Run(ImageType, Generate(), outputFilePath);
        }

        private void WriteClusters(
            [NotNull] IDictionary<TVertex, GraphColor> verticesColors, 
            [NotNull] IDictionary<TEdge, GraphColor> edgeColors, 
            [NotNull] IClusteredGraph parent)
        {
            Debug.Assert(verticesColors != null);
            Debug.Assert(edgeColors != null);
            Debug.Assert(parent != null);

            ++ClusterCount;
            foreach (IVertexAndEdgeListGraph<TVertex, TEdge> subGraph in parent.Clusters)
            {
                Output.Write($"subgraph cluster{ClusterCount}");
                Output.WriteLine(" {");
                OnFormatCluster(subGraph);
                if (subGraph is IClusteredGraph clusteredGraph)
                {
                    WriteClusters(verticesColors, edgeColors, clusteredGraph);
                }

                if (parent.Collapsed)
                {
                    foreach (TVertex vertex in subGraph.Vertices)
                    {
                        verticesColors[vertex] = GraphColor.Black;
                    }

                    foreach (TEdge edge in subGraph.Edges)
                    {
                        edgeColors[edge] = GraphColor.Black;
                    }
                }
                else
                {
                    WriteVertices(verticesColors, subGraph.Vertices);
                    WriteEdges(edgeColors, subGraph.Edges);
                }
                Output.WriteLine("}");
            }
        }

        private void WriteVertices(
            [NotNull] IDictionary<TVertex, GraphColor> verticesColors,
            [NotNull, ItemNotNull] IEnumerable<TVertex> vertices)
        {
            Debug.Assert(verticesColors != null);
            Debug.Assert(vertices != null);

            foreach (TVertex vertex in vertices)
            {
                if (verticesColors[vertex] != GraphColor.White)
                    continue;

                OnFormatVertex(vertex);
                verticesColors[vertex] = GraphColor.Black;
            }
        }

        private void WriteEdges(
            [NotNull] IDictionary<TEdge, GraphColor> edgesColors,
            [NotNull, ItemNotNull] IEnumerable<TEdge> edges)
        {
            Debug.Assert(edgesColors != null);
            Debug.Assert(edges != null);

            foreach (TEdge edge in edges)
            {
                if (edgesColors[edge] != GraphColor.White)
                    continue;

                Output.Write(VisitedGraph.IsDirected
                    ? $"{_verticesIds[edge.Source]} -> {_verticesIds[edge.Target]}"
                    : $"{_verticesIds[edge.Source]} -- {_verticesIds[edge.Target]}");

                OnFormatEdge(edge);
                edgesColors[edge] = GraphColor.Black;
            }
        }
    }
}