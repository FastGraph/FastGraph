#nullable enable

using FastGraph.Graphviz.Dot;
using FastGraph.Utils;

namespace FastGraph.Graphviz
{
    /// <summary>
    /// Base class for Graph to DOT renderer.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public abstract class GraphRendererBase<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphvizAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="graph">Graph to convert to DOT.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        protected GraphRendererBase(IEdgeListGraph<TVertex, TEdge> graph)
        {
            Graphviz = new GraphvizAlgorithm<TVertex, TEdge>(graph);
            InternalInitialize();
        }

        private void InternalInitialize()
        {
            Graphviz.CommonVertexFormat.Style = GraphvizVertexStyle.Filled;
            Graphviz.CommonVertexFormat.FillColor = GraphvizColor.LightYellow;
            Graphviz.CommonVertexFormat.Font = new GraphvizFont("Tahoma", 8.25f);
            Graphviz.CommonVertexFormat.Shape = GraphvizVertexShape.Box;

            Graphviz.CommonEdgeFormat.Font = new GraphvizFont("Tahoma", 8.25f);
        }

        /// <summary>
        /// Initializes renderer for generation.
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// Cleans renderer after generation.
        /// </summary>
        protected virtual void Clean()
        {
        }

        /// <summary>
        /// Graph to DOT algorithm.
        /// </summary>
        public GraphvizAlgorithm<TVertex, TEdge> Graphviz { get; }

        /// <inheritdoc cref="GraphvizAlgorithm{TVertex,TEdge}.VisitedGraph"/>
        public IEdgeListGraph<TVertex, TEdge> VisitedGraph => Graphviz.VisitedGraph;

        /// <inheritdoc cref="GraphvizAlgorithm{TVertex,TEdge}.Generate(IDotEngine,string)"/>
        public string Generate(IDotEngine dot, string outputFilePath)
        {
            using (GenerationScope())
            {
                return Graphviz.Generate(dot, outputFilePath);
            }

            #region Local function

            IDisposable GenerationScope()
            {
                Initialize();
                return DisposableHelpers.Finally(Clean);
            }

            #endregion
        }
    }
}
