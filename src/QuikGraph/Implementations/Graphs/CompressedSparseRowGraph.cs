#if SUPPORTS_SERIALIZATION || SUPPORTS_CLONEABLE
using System;
#endif
using System.Collections.Generic;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Directed graph representation using a compressed sparse row representation.
    /// (http://www.cs.utk.edu/~dongarra/etemplates/node373.html)
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("VertexCount = {" + nameof(VertexCount) + "}, EdgeCount = {" + nameof(EdgeCount) + "}")]
    public sealed class CompressedSparseRowGraph<TVertex> : IEdgeSet<TVertex, SEquatableEdge<TVertex>>, IVertexListGraph<TVertex, SEquatableEdge<TVertex>>
#if SUPPORTS_CLONEABLE
        , ICloneable
#endif
    {
#if SUPPORTS_SERIALIZATION
        [Serializable]
#endif
        private struct Range
        {
            public readonly int Start;
            public readonly int End;

            public Range(int start, int end)
            {
#if SUPPORTS_CONTRACTS
                Contract.Requires(start >= 0);
                Contract.Requires(start <= end);
                Contract.Ensures(Contract.ValueAtReturn(out this).Start == start);
                Contract.Ensures(Contract.ValueAtReturn(out this).End == end);
#endif

                Start = start;
                End = end;
            }

            public int Length
            {
                get
                {
#if SUPPORTS_CONTRACTS
                    Contract.Ensures(Contract.Result<int>() >= 0);
#endif

                    return End - Start;
                }
            }
        }

        private CompressedSparseRowGraph(
            [NotNull] Dictionary<TVertex, Range> outEdgeStartRanges,
            [NotNull, ItemNotNull] TVertex[] outEdges)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(outEdgeStartRanges != null);
            Contract.Requires(outEdges != null);
#endif

            _outEdgeStartRanges = outEdgeStartRanges;
            _outEdges = outEdges;
        }

        /// <summary>
        /// Converts the given <paramref name="visitedGraph"/> to a <see cref="CompressedSparseRowGraph{TVertex}"/>.
        /// </summary>
        /// <param name="visitedGraph">Graph to convert.</param>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <returns>A corresponding <see cref="CompressedSparseRowGraph{TVertex}"/>.</returns>
        [NotNull]
        public static CompressedSparseRowGraph<TVertex> FromGraph<TEdge>(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            where TEdge : IEdge<TVertex>
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(visitedGraph != null);
            Contract.Ensures(Contract.Result<CompressedSparseRowGraph<TVertex>>() != null);
#endif

            var outEdgeStartRanges = new Dictionary<TVertex, Range>(visitedGraph.VertexCount);
            var outEdges = new TVertex[visitedGraph.EdgeCount];

            const int start = 0;
            int index = 0;
            foreach (var vertex in visitedGraph.Vertices)
            {
                int end = start + visitedGraph.OutDegree(vertex);
                var range = new Range(start, end);
                outEdgeStartRanges.Add(vertex, range);
                foreach (var edge in visitedGraph.OutEdges(vertex))
                    outEdges[index++] = edge.Target;
#if SUPPORTS_CONTRACTS
                Contract.Assert(index == end);
#endif
            }
#if SUPPORTS_CONTRACTS
            Contract.Assert(index == outEdges.Length);
#endif

            return new CompressedSparseRowGraph<TVertex>(outEdgeStartRanges, outEdges);
        }

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => true;

        /// <inheritdoc />
        public bool AllowParallelEdges => false;

        #endregion

        #region IVertexSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsVerticesEmpty => _outEdgeStartRanges.Count > 0;

        /// <inheritdoc />
        public int VertexCount => _outEdgeStartRanges.Count;

        /// <inheritdoc />
        public IEnumerable<TVertex> Vertices => _outEdgeStartRanges.Keys;

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            return _outEdgeStartRanges.ContainsKey(vertex);
        }

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty => _outEdges.Length > 0;

        /// <inheritdoc />
        public int EdgeCount => _outEdges.Length;

        [NotNull, ItemNotNull]
        private readonly TVertex[] _outEdges;

        [NotNull]
        private readonly Dictionary<TVertex, Range> _outEdgeStartRanges;

        /// <inheritdoc />
        public IEnumerable<SEquatableEdge<TVertex>> Edges
        {
            get
            {
                foreach (KeyValuePair<TVertex, Range> pair in _outEdgeStartRanges)
                {
                    TVertex source = pair.Key;
                    Range range = pair.Value;
                    for (int i = range.Start; i < range.End; ++i)
                    {
                        TVertex target = _outEdges[i];
                        yield return new SEquatableEdge<TVertex>(source, target);
                    }
                }
            }
        }

        /// <inheritdoc />
        public bool ContainsEdge(SEquatableEdge<TVertex> edge)
        {
            return ContainsEdge(edge.Source, edge.Target);
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            if (_outEdgeStartRanges.TryGetValue(source, out Range range))
            {
                for (int i = range.Start; i < range.End; ++i)
                {
                    if (_outEdges[i].Equals(target))
                        return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out SEquatableEdge<TVertex> edge)
        {
            if (ContainsEdge(source, target))
            {
                edge = new SEquatableEdge<TVertex>(source, target);
                return true;
            }

            edge = default(SEquatableEdge<TVertex>);
            return false;
        }

        /// <inheritdoc />
        public bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<SEquatableEdge<TVertex>> edges)
        {
            if (ContainsEdge(source, target))
            {
                edges = new[] { new SEquatableEdge<TVertex>(source, target) };
                return true;
            }

            edges = null;
            return false;
        }

        #endregion

        #region IImplicitGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsOutEdgesEmpty(TVertex vertex)
        {
            return _outEdgeStartRanges[vertex].Length == 0;
        }

        /// <inheritdoc />
        public int OutDegree(TVertex vertex)
        {
            return _outEdgeStartRanges[vertex].Length;
        }

        /// <inheritdoc />
        public IEnumerable<SEquatableEdge<TVertex>> OutEdges(TVertex vertex)
        {
            Range range = _outEdgeStartRanges[vertex];
            for (int i = range.Start; i < range.End; ++i)
                yield return new SEquatableEdge<TVertex>(vertex, _outEdges[i]);
        }

        /// <inheritdoc />
        public bool TryGetOutEdges(TVertex vertex, out IEnumerable<SEquatableEdge<TVertex>> edges)
        {
            Range range = _outEdgeStartRanges[vertex];
            if (range.Length > 0)
            {
                edges = OutEdges(vertex);
                return false;
            }

            edges = null;
            return false;
        }

        /// <inheritdoc />
        public SEquatableEdge<TVertex> OutEdge(TVertex vertex, int index)
        {
            Range range = _outEdgeStartRanges[vertex];
            int targetIndex = range.Start + index;
#if SUPPORTS_CONTRACTS
            Contract.Assert(targetIndex < range.End);
#endif

            return new SEquatableEdge<TVertex>(vertex, _outEdges[targetIndex]);
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Clones this graph.
        /// </summary>
        /// <returns>Cloned graph.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull]
        public CompressedSparseRowGraph<TVertex> Clone()
        {
            var ranges = new Dictionary<TVertex, Range>(_outEdgeStartRanges);
            var edges = (TVertex[])_outEdges.Clone();
            return new CompressedSparseRowGraph<TVertex>(ranges, edges);
        }

#if SUPPORTS_CLONEABLE
        /// <inheritdoc />
        object ICloneable.Clone()
        {
            return Clone();
        }
#endif

        #endregion
    }
}
