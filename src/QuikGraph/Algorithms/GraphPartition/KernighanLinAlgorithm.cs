#if SUPPORTS_KERNIGHANLIN_ALGORITHM
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Linq;

namespace QuikGraph.Algorithms.GraphPartition
{
    /// <summary>
    /// Algorithm that separate a graph into two disjoint subsets A and B of equal (or nearly equal) in size,
    /// in a way that minimizes the sum of the weights of the subset of edges that cross from A to B.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class KernighanLinAlgorithm<TVertex, TEdge> : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : TaggedUndirectedEdge<TVertex, double>
    {
        private readonly int _nbIterations;
        private readonly int _partitionSize;

        private SortedSet<TVertex> _vertexSetA;
        private SortedSet<TVertex> _vertexSetB;

        private SortedSet<TVertex> _unSwappedSetA;
        private SortedSet<TVertex> _unSwappedSetB;

        /// <summary>
        /// Initializes a new instance of the <see cref="KernighanLinAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="nbIterations">Number of iterations to perform.</param>
        public KernighanLinAlgorithm(
            [NotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph, 
            int nbIterations)
            : base(visitedGraph)
        {
            _nbIterations = nbIterations;
            _partitionSize = visitedGraph.VertexCount / 2;
        }

        /// <summary>
        /// Partition created by the algorithm.
        /// </summary>
        public Partition<TVertex> Partition { get; private set; }

        private Partition<TVertex> DoAllSwaps()
        {
            var swaps = new List<Tuple<TVertex, TVertex>>();
            double minCost = double.MaxValue;
            int minId = -1;

            for (int i = 0; i < _partitionSize; i++)
            {
                double cost = DoSingleSwap(swaps);
                if (cost < minCost)
                {
                    minCost = cost;
                    minId = i;
                }
            }

            // Back to swap step with min cut cost
            while (swaps.Count - 1 > minId)
            {
                Tuple<TVertex, TVertex> pair = swaps.Last();
                swaps.Remove(pair);
                SwapVertices(_vertexSetA, pair.Item2, _vertexSetB, pair.Item1);
            }

            return new Partition<TVertex>(_vertexSetA, _vertexSetB, minCost);
        }

        private double DoSingleSwap([NotNull, ItemNotNull] ICollection<Tuple<TVertex, TVertex>> swaps)
        {
            Tuple<TVertex, TVertex> maxPair = null;
            double maxGain = double.MinValue;
            foreach (TVertex vertexFromA in _unSwappedSetA)
            {
                foreach (TVertex vertexFromB in _unSwappedSetB)
                {
                    bool foundEdge = FindEdge(vertexFromA, vertexFromB, out TEdge edge);
                    double edgeCost = foundEdge ? edge.Tag : 0.0;

                    double gain = GetVertexCost(vertexFromA) + GetVertexCost(vertexFromB) - 2 * edgeCost;
                    if (gain > maxGain)
                    {
                        maxPair = new Tuple<TVertex, TVertex>(vertexFromA, vertexFromB);
                        maxGain = gain;
                    }
                }
            }

#if SUPPORTS_CONTRACTS
            Contract.Assert(maxPair != null, "Must find a swap.");
#endif

            SwapVertices(_vertexSetA, maxPair.Item1, _vertexSetB, maxPair.Item2);
            swaps.Add(maxPair);
            _unSwappedSetA.Remove(maxPair.Item1);
            _unSwappedSetB.Remove(maxPair.Item2);

            return GetCutCost();
        }

        [JetBrains.Annotations.Pure]
        private double GetVertexCost([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            double cost = 0;
            bool vertexIsInA = _vertexSetA.Contains(vertex);

            foreach (TVertex neighborVertex in GetNeighbors(vertex))
            {
                bool vertexNeighborIsInA = _vertexSetA.Contains(neighborVertex);
                if (!FindEdge(vertex, neighborVertex, out TEdge edge))
                    continue;

                if (vertexIsInA != vertexNeighborIsInA) // External
                    cost += edge.Tag;
                else
                    cost -= edge.Tag;
            }

            return cost;
        }

        [JetBrains.Annotations.Pure]
        [NotNull, ItemNotNull]
        private IEnumerable<TVertex> GetNeighbors([NotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            var neighbors = new HashSet<TVertex>();
            foreach (TEdge edge in VisitedGraph.AdjacentEdges(vertex))
            {
                neighbors.Add(edge.Source);
                neighbors.Add(edge.Target);

            }

            if (neighbors.Contains(vertex))
                neighbors.Remove(vertex);

            return neighbors;
        }

        private static void SwapVertices(
            [NotNull, ItemNotNull] ISet<TVertex> setA,
            [NotNull] TVertex vertexA,
            [NotNull, ItemNotNull] ISet<TVertex> setB,
            [NotNull] TVertex vertexB)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(setA != null);
            Contract.Requires(vertexA != null);
            Contract.Requires(setB != null);
            Contract.Requires(vertexB != null);
#endif

            if (!setA.Contains(vertexA)
                || setA.Contains(vertexB)
                || !setB.Contains(vertexB)
                || setB.Contains(vertexA))
                throw new InvalidOperationException("Invalid swap.");

            setA.Remove(vertexA);
            setA.Add(vertexB);

            setB.Remove(vertexB);
            setB.Add(vertexA);
        }

        private double GetCutCost()
        {
            double cost = 0;
            foreach (TEdge edge in VisitedGraph.Edges)
            {
                if (_vertexSetA.Contains(edge.Source) != _vertexSetA.Contains(edge.Target))
                {
                    cost += edge.Tag;
                }
            }

            return cost;
        }

        /// <summary>
        /// Searches for an edge that links <paramref name="vertexFromA"/> and <paramref name="vertexFromB"/>.
        /// </summary>
        [JetBrains.Annotations.Pure]
        private bool FindEdge([NotNull] TVertex vertexFromA, [NotNull] TVertex vertexFromB, out TEdge foundEdge)
        {
            foreach (TEdge edge in VisitedGraph.AdjacentEdges(vertexFromA))
            {
                if (edge.Target.Equals(vertexFromB) || edge.Source.Equals(vertexFromB))
                {
                    foundEdge = edge;
                    return true;
                }
            }

            foundEdge = default(TEdge);
            return false;
        }

        private void GetStartPartition()
        {
            int i = 0;
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                if (i < _partitionSize)
                    _vertexSetA.Add(vertex);
                else
                    _vertexSetB.Add(vertex);

                ++i;
            }
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            _vertexSetA = new SortedSet<TVertex>();
            _vertexSetB = new SortedSet<TVertex>();

            GetStartPartition();

            _unSwappedSetA = new SortedSet<TVertex>(_vertexSetA);
            _unSwappedSetB = new SortedSet<TVertex>(_vertexSetB);

            var bestPartition = new Partition<TVertex>(_vertexSetA, _vertexSetB);
            double minCost = double.MaxValue;

            for (int i = 0; i < _nbIterations; i++)
            {
                Partition<TVertex> tmpPartition = DoAllSwaps();
                double tmpCutCost = tmpPartition.CutCost;

                _vertexSetA = tmpPartition.VertexSetA;
                _vertexSetB = tmpPartition.VertexSetB;
                _unSwappedSetA = _vertexSetA;
                _unSwappedSetB = _vertexSetB;

                if (tmpCutCost < minCost)
                {
                    bestPartition = tmpPartition;
                    minCost = tmpCutCost;
                }
            }

            Partition = bestPartition;
        }

        #endregion
    }
}
#endif