using System.IO;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Condensation;

namespace QuikGraph.Graphviz
{
    /// <summary>
    /// Condensation graph to DOT renderer.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <typeparam name="TGraph">Graph type.</typeparam>
    public class CondensatedGraphRenderer<TVertex, TEdge, TGraph> : GraphRendererBase<TGraph, CondensedEdge<TVertex, TEdge, TGraph>>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CondensatedGraphRenderer{TVertex,TEdge,TGraph}"/> class.
        /// </summary>
        /// <param name="graph">Graph to convert to DOT.</param>
        public CondensatedGraphRenderer(
            [NotNull] IEdgeListGraph<TGraph, CondensedEdge<TVertex, TEdge, TGraph>> graph)
            : base(graph)
        {
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            Graphviz.FormatVertex += OnFormatVertex;
            Graphviz.FormatEdge += OnFormatEdge;
        }

        /// <inheritdoc />
        protected override void Clean()
        {
            Graphviz.FormatEdge -= OnFormatEdge;
            Graphviz.FormatVertex -= OnFormatVertex;

            base.Clean();
        }

        private void OnFormatVertex([NotNull] object sender, [NotNull] FormatVertexEventArgs<TGraph> args)
        {
            using (var writer = new StringWriter())
            {
                writer.WriteLine($"{args.Vertex.VertexCount}-{args.Vertex.EdgeCount}");
                foreach (TVertex vertex in args.Vertex.Vertices)
                {
                    writer.WriteLine($"  {vertex}");
                }
                foreach (TEdge edge in args.Vertex.Edges)
                {
                    writer.WriteLine($"  {edge}");
                }
                args.VertexFormat.Label = Graphviz.Escape(writer.ToString());
            }
        }

        private void OnFormatEdge(
            [NotNull] object sender,
            [NotNull] FormatEdgeEventArgs<TGraph, CondensedEdge<TVertex, TEdge, TGraph>> args)
        {
            using (var writer = new StringWriter())
            {
                writer.WriteLine(args.Edge.Edges.Count);
                foreach (TEdge edge in args.Edge.Edges)
                {
                    writer.WriteLine($"  {edge}");
                }
                args.EdgeFormat.Label.Value = Graphviz.Escape(writer.ToString());
            }
        }
    }
}