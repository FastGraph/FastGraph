#nullable enable

using System.Text;
using FastGraph.Algorithms.Condensation;

namespace FastGraph.Graphviz
{
    /// <summary>
    /// Edge merge condensation graph to DOT renderer.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public class EdgeMergeCondensatedGraphRenderer<TVertex, TEdge> : GraphRendererBase<TVertex, MergedEdge<TVertex, TEdge>>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeMergeCondensatedGraphRenderer{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="graph">Graph to convert to DOT.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        public EdgeMergeCondensatedGraphRenderer(
            IEdgeListGraph<TVertex, MergedEdge<TVertex, TEdge>> graph)
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

        private static void OnFormatVertex(object sender, FormatVertexEventArgs<TVertex> args)
        {
            args.VertexFormat.Label = args.Vertex.ToString();
        }

        private static void OnFormatEdge(object sender, FormatEdgeEventArgs<TVertex, MergedEdge<TVertex, TEdge>> args)
        {
            var builder = new StringBuilder();
            builder.AppendLine(args.Edge.Edges.Count.ToString());

            foreach (TEdge edge in args.Edge.Edges)
            {
                builder.AppendLine($"  {edge}");
            }

            args.EdgeFormat.Label.Value = builder.ToString();
        }
    }
}
