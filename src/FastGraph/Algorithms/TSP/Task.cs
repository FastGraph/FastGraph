#nullable enable

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace FastGraph.Algorithms.TSP
{
    internal sealed class Task<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : EquatableEdge<TVertex>
    {
        private readonly BidirectionalGraph<TVertex, TEdge> _graph;

        private readonly IDictionary<EquatableEdge<TVertex>, double> _weight;

        public BidirectionalGraph<TVertex, TEdge> Path { get; }

        public string TaskName { get; }

        public double MinCost { get; private set; }

        public TaskPriority Priority { get; }

        public Task(
            BidirectionalGraph<TVertex, TEdge> graph,
            IDictionary<EquatableEdge<TVertex>, double> weights,
            BidirectionalGraph<TVertex, TEdge> path,
            double cost)
            : this(graph, weights, path, cost, "Init")
        {
        }

        public Task(
            BidirectionalGraph<TVertex, TEdge> graph,
            IDictionary<EquatableEdge<TVertex>, double> weights,
            BidirectionalGraph<TVertex, TEdge> path,
            double cost,
            string taskName)
        {
            TaskName = taskName;
            _graph = new BidirectionalGraph<TVertex, TEdge>(graph);
            _weight = new Dictionary<EquatableEdge<TVertex>, double>(weights);
            Path = path;
            MinCost = cost;
            Initialize();
            Priority = new TaskPriority(MinCost, path.EdgeCount);
        }

        private void Initialize()
        {
            if (Check())
                return;

            RemoveCycles();
            Reduce();
        }

        private bool Check()
        {
            if (_graph.EdgeCount == 1)
            {
                Path.AddEdge(_graph.Edges.First());
                if (Path.IsDirectedAcyclicGraph())
                {
                    Path.RemoveEdge(_graph.Edges.First());
                    return false;
                }

                return true;
            }

            return false;
        }

        private void RemoveCycles()
        {
            var edgesToRemove = new List<TEdge>();
            foreach (TEdge edge in _graph.Edges)
            {
                Path.AddEdge(edge);
                if (!Path.IsDirectedAcyclicGraph())
                {
                    edgesToRemove.Add(edge);
                    _weight.Remove(edge);
                }

                Path.RemoveEdge(edge);
            }

            edgesToRemove.ForEach(edge => _graph.RemoveEdge(edge));
        }

        private void Reduce()
        {
            if (_graph.IsEdgesEmpty)
            {
                MinCost = double.PositiveInfinity;
                return;
            }

            double sum = ReduceOutEdges();
            sum += ReduceInEdges();

            MinCost += sum;
        }

        private double ReduceOutEdges()
        {
            double sum = 0;
            foreach (TVertex vertex in _graph.Vertices)
            {
                if (!_graph.TryGetOutEdges(vertex, out IEnumerable<TEdge>? outEdges))
                    continue;

                TEdge[] outEdgesArray = outEdges.ToArray();
                if (outEdgesArray.Length <= 0)
                    continue;

                double min = outEdgesArray.Min(edge => _weight[edge]);
                foreach (TEdge edge in outEdgesArray)
                {
                    _weight[edge] -= min;
                }

                sum += min;
            }

            return sum;
        }

        private double ReduceInEdges()
        {
            double sum = 0;
            foreach (TVertex vertex in _graph.Vertices)
            {
                if (!_graph.TryGetInEdges(vertex, out IEnumerable<TEdge>? inEdges))
                    continue;

                TEdge[] inEdgesArray = inEdges.ToArray();
                if (inEdgesArray.Length <= 0)
                    continue;

                double min = inEdgesArray.Min(edge => _weight[edge]);
                foreach (TEdge edge in inEdgesArray)
                {
                    _weight[edge] -= min;
                }

                sum += min;
            }

            return sum;
        }

        private IEnumerable<TEdge> GetZeroEdges()
        {
            var zeros = new List<TEdge>();
            foreach (TVertex vertex in _graph.Vertices)
            {
                if (_graph.TryGetOutEdges(vertex, out IEnumerable<TEdge>? outEdges))
                {
                    zeros.AddRange(outEdges.Where(edge => Math.Abs(_weight[edge]) < double.Epsilon));
                }
            }

            return zeros;
        }

        [Pure]
        private double ComputeMaxCandidate(
            IEnumerable<TEdge> row,
            IEnumerable<TEdge> column,
            TVertex source,
            TVertex target)
        {
            return
                row.Where(edge => !EqualityComparer<TVertex?>.Default.Equals(edge.Target, target))
                    .DefaultIfEmpty(default)
                    .Min(edge => edge is null
                        ? double.PositiveInfinity
                        : _weight[edge])
                +
                column.Where(edge => !EqualityComparer<TVertex?>.Default.Equals(edge.Source, source))
                    .DefaultIfEmpty(default)
                    .Min(edge => edge is null
                        ? double.PositiveInfinity
                        : _weight[edge]);
        }

        private TEdge? ChooseEdgeForSplit()
        {
            TEdge? edgeForSplit = default;
            double max = double.NegativeInfinity;
            foreach (TEdge edge in GetZeroEdges())
            {
                TVertex v1 = edge.Source;
                TVertex v2 = edge.Target;

                if (_graph.TryGetOutEdges(v1, out IEnumerable<TEdge>? row)
                    && _graph.TryGetInEdges(v2, out IEnumerable<TEdge>? column))
                {
                    double maxCandidate = ComputeMaxCandidate(row, column, v1, v2);

                    if (maxCandidate > max)
                    {
                        max = maxCandidate;
                        edgeForSplit = edge;
                    }
                }
            }

            return edgeForSplit;
        }

        private bool CanSplit()
        {
            return MinCost < double.PositiveInfinity;
        }

        public bool Split([NotNullWhen(true)] out Task<TVertex, TEdge>? taskTake, [NotNullWhen(true)] out Task<TVertex, TEdge>? taskDrop)
        {
            if (!CanSplit())
            {
                taskTake = default;
                taskDrop = default;
                return false;
            }

            TEdge? edgeForSplit = ChooseEdgeForSplit();
            TVertex v1 = edgeForSplit!.Source;
            TVertex v2 = edgeForSplit.Target;

            BidirectionalGraph<TVertex, TEdge> graphTake = _graph.Clone();
            var weightsTake = new Dictionary<EquatableEdge<TVertex>, double>(_weight);
            var reverseEdge = new EquatableEdge<TVertex>(edgeForSplit.Target, edgeForSplit.Source);
            weightsTake.Remove(reverseEdge);
            graphTake.RemoveEdgeIf(edge => reverseEdge.Equals(edge));

            foreach (TEdge outEdge in graphTake.OutEdges(v1))
            {
                weightsTake.Remove(outEdge);
            }

            foreach (TEdge inEdge in graphTake.InEdges(v2))
            {
                weightsTake.Remove(inEdge);
            }

            graphTake.ClearOutEdges(v1);
            graphTake.ClearInEdges(v2);
            var pathTake = new BidirectionalGraph<TVertex, TEdge>(Path);
            pathTake.AddEdge(edgeForSplit);
            taskTake = new Task<TVertex, TEdge>(graphTake, weightsTake, pathTake, MinCost, $"Take{edgeForSplit}");

            BidirectionalGraph<TVertex, TEdge> graphDrop = _graph.Clone();
            var weightsDrop = new Dictionary<EquatableEdge<TVertex>, double>(_weight);
            weightsDrop.Remove(edgeForSplit);
            graphDrop.RemoveEdge(edgeForSplit);
            taskDrop = new Task<TVertex, TEdge>(graphDrop, weightsDrop, new BidirectionalGraph<TVertex, TEdge>(Path), MinCost, $"Drop{edgeForSplit}");

            return true;
        }

        /// <summary>
        /// Checks if the result is ready to be used.
        /// </summary>
        public bool IsResultReady()
        {
            return Path.EdgeCount == _graph.VertexCount;
        }
    }
}
