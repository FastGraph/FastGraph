using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms
{
    /// <summary>
    /// Algorithm that find Eulerian path in a graph.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class EulerianTrailAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IMutableVertexAndEdgeListGraph<TVertex, TEdge>>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [NotNull, ItemNotNull]
        private readonly List<TEdge> _temporaryCircuit = new List<TEdge>();

        [CanBeNull]
        private TVertex _currentVertex;

        [NotNull, ItemNotNull]
        private List<TEdge> _temporaryEdges = new List<TEdge>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EulerianTrailAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public EulerianTrailAlgorithm(
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EulerianTrailAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        public EulerianTrailAlgorithm(
            [CanBeNull] IAlgorithmComponent host,
            [NotNull] IMutableVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
            _currentVertex = default(TVertex);
        }

        [NotNull, ItemNotNull]
        private List<TEdge> _circuit = new List<TEdge>();

        /// <summary>
        /// Circuit.
        /// </summary>
        [NotNull, ItemNotNull]
        public TEdge[] Circuit => _circuit.ToArray();

        [Pure]
        private bool NotInCircuit([NotNull] TEdge edge)
        {
            return !_circuit.Contains(edge)
                   && !_temporaryCircuit.Contains(edge);
        }

        [Pure]
        [NotNull, ItemNotNull]
        private IEnumerable<TEdge> SelectOutEdgesNotInCircuit([NotNull] TVertex vertex)
        {
            return VisitedGraph.OutEdges(vertex).Where(NotInCircuit);
        }

        [Pure]
        private bool TrySelectSingleOutEdgeNotInCircuit([NotNull] TVertex vertex, out TEdge edge)
        {
            IEnumerable<TEdge> edgesNotInCircuit = SelectOutEdgesNotInCircuit(vertex);
            using (IEnumerator<TEdge> enumerator = edgesNotInCircuit.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    edge = default(TEdge);
                    return false;
                }

                edge = enumerator.Current;
                return true;
            }
        }

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is added to the circuit.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> CircuitEdge;

        private void OnCircuitEdge([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            CircuitEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when an edge is visited.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> VisitEdge;

        private void OnVisitEdge([NotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            VisitEdge?.Invoke(edge);
        }

        private bool Search([NotNull] TVertex vertex)
        {
            Debug.Assert(vertex != null);

            foreach (TEdge edge in SelectOutEdgesNotInCircuit(vertex))
            {
                OnTreeEdge(edge);

                TVertex target = edge.Target;
                // Add edge to temporary path
                _temporaryCircuit.Add(edge);

                // edge.Target should be equal to CurrentVertex.
                if (EqualityComparer<TVertex>.Default.Equals(edge.Target, _currentVertex))
                    return true;

                // Continue search
                if (Search(target))
                    return true;

                // Remove edge
                _temporaryCircuit.Remove(edge);
            }

            // It's a dead end.
            return false;
        }

        /// <summary>
        /// Looks for a new path to add to the current vertex.
        /// </summary>
        /// <returns>True a new path was found, false otherwise.</returns>
        private bool Visit()
        {
            // Find a vertex that needs to be visited
            foreach (TEdge edge in _circuit)
            {
                bool edgeFound = TrySelectSingleOutEdgeNotInCircuit(edge.Source, out TEdge foundEdge);
                if (!edgeFound)
                    continue;

                OnVisitEdge(foundEdge);
                _currentVertex = edge.Source;
                if (Search(_currentVertex))
                    return true;
            }

            // Could not augment circuit
            return false;
        }

        /// <summary>
        /// Computes the number of Eulerian trails in the graph.
        /// </summary>
        /// <param name="graph">Graph to visit.</param>
        /// <returns>Number of Eulerian trails.</returns>
        public static int ComputeEulerianPathCount(
            [NotNull] IVertexAndEdgeListGraph<TVertex, TEdge> graph)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));

            if (graph.EdgeCount < graph.VertexCount)
                return 0;

            int odd = graph.OddVertices().Count();
            if (odd == 0)
                return 1;
            if (odd % 2 != 0)
                return 0;
            return odd / 2;
        }

        /// <summary>
        /// Merges the temporary circuit with the current circuit.
        /// </summary>
        /// <returns>True if all the graph edges are in the circuit.</returns>
        private bool CircuitAugmentation()
        {
            var newCircuit = new List<TEdge>(_circuit.Count + _temporaryCircuit.Count);
            int i, j;

            // Follow C until w is found
            for (i = 0; i < _circuit.Count; ++i)
            {
                TEdge edge = _circuit[i];
                if (EqualityComparer<TVertex>.Default.Equals(edge.Source, _currentVertex))
                    break;
                newCircuit.Add(edge);
            }

            // Follow D until W is found again
            for (j = 0; j < _temporaryCircuit.Count; ++j)
            {
                TEdge edge = _temporaryCircuit[j];
                newCircuit.Add(edge);
                OnCircuitEdge(edge);
                if (EqualityComparer<TVertex>.Default.Equals(edge.Target, _currentVertex))
                    break;
            }
            _temporaryCircuit.Clear();

            // Continue C
            for (; i < _circuit.Count; ++i)
            {
                TEdge edge = _circuit[i];
                newCircuit.Add(edge);
            }

            // Set as new circuit
            _circuit = newCircuit;

            // Check if contains all edges
            if (_circuit.Count == VisitedGraph.EdgeCount)
                return true;

            return false;
        }

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            if (VisitedGraph.VertexCount == 0)
                return;

            if (!TryGetRootVertex(out TVertex root))
                root = VisitedGraph.Vertices.First();

            _currentVertex = root;

            // Start search
            // ReSharper disable once AssignNullToNotNullAttribute, Justification: Found vertex cannot be null
            Search(_currentVertex);
            if (CircuitAugmentation())
                return; // Circuit is found

            do
            {
                if (!Visit())
                    break; // Visit edges and build path
                if (CircuitAugmentation())
                    break; // Circuit is found
            } while (true);
        }

        #endregion

        private bool HasEdgeToward([NotNull] TVertex u, [NotNull] TVertex v)
        {
            bool foundEdge = false;
            foreach (TEdge edge in VisitedGraph.OutEdges(v))
            {
                if (EqualityComparer<TVertex>.Default.Equals(edge.Target, u))
                {
                    foundEdge = true;
                    break;
                }
            }

            return foundEdge;
        }

        private bool FindAdjacentOddVertex(
            [NotNull] TVertex u,
            [NotNull, ItemNotNull] ICollection<TVertex> oddVertices,
            [NotNull, InstantHandle] EdgeFactory<TVertex, TEdge> edgeFactory,
            out bool foundAdjacent)
        {
            bool found = false;
            foundAdjacent = false;
            foreach (TEdge edge in VisitedGraph.OutEdges(u))
            {
                TVertex v = edge.Target;
                if (!EqualityComparer<TVertex>.Default.Equals(v, u) && oddVertices.Contains(v))
                {
                    foundAdjacent = true;
                    // Check that v does not have an out-edge towards u
                    if (HasEdgeToward(u, v))
                        continue;

                    // Add temporary edge
                    AddTemporaryEdge(u, v, oddVertices, edgeFactory);

                    // Set u to null
                    found = true;
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Adds temporary edges to the graph to make all vertex even.
        /// </summary>
        /// <param name="edgeFactory">Edge factory method.</param>
        /// <returns>Temporary edges list.</returns>
        [NotNull, ItemNotNull]
        public TEdge[] AddTemporaryEdges([NotNull, InstantHandle] EdgeFactory<TVertex, TEdge> edgeFactory)
        {
            if (edgeFactory is null)
                throw new ArgumentNullException(nameof(edgeFactory));

            // First gather odd edges
            List<TVertex> oddVertices = VisitedGraph.OddVertices().ToList();

            // Check that there are an even number of them
            if (oddVertices.Count % 2 != 0)
                throw new InvalidOperationException("Number of odd vertices in not even.");

            // Add temporary edges to create even edges
            _temporaryEdges = new List<TEdge>();

            while (oddVertices.Count > 0)
            {
                TVertex u = oddVertices[0];
                // Find adjacent odd vertex
                bool found = FindAdjacentOddVertex(u, oddVertices, edgeFactory, out bool foundAdjacent);
                if (!foundAdjacent)
                {
                    // Pick another vertex
                    if (oddVertices.Count < 2)
                        throw new InvalidOperationException("Eulerian trail failure.");
                    TVertex v = oddVertices[1];

                    // Add to temporary edges
                    AddTemporaryEdge(u, v, oddVertices, edgeFactory);

                    // Set u to null
                    found = true;
                }

                if (!found)
                {
                    oddVertices.Remove(u);
                    oddVertices.Add(u);
                }
            }

            return _temporaryEdges.ToArray();
        }

        private void AddTemporaryEdge(
            [NotNull] TVertex u,
            [NotNull] TVertex v,
            [NotNull, ItemNotNull] ICollection<TVertex> oddVertices,
            [NotNull, InstantHandle] EdgeFactory<TVertex, TEdge> edgeFactory)
        {
            TEdge tempEdge = edgeFactory(u, v);
            if (!VisitedGraph.AddEdge(tempEdge))
                throw new InvalidOperationException("Cannot add temporary edge.");

            // Add to temporary edges
            _temporaryEdges.Add(tempEdge);

            // Remove u,v from oddVertices
            oddVertices.Remove(u);
            oddVertices.Remove(v);
        }

        /// <summary>
        /// Removes temporary edges.
        /// </summary>
        public void RemoveTemporaryEdges()
        {
            // Remove from graph
            foreach (TEdge edge in _temporaryEdges)
                VisitedGraph.RemoveEdge(edge);
            _temporaryEdges.Clear();
        }

        /// <summary>
        /// Computes the set of Eulerian trails that traverse the edge set.
        /// </summary>
        /// <remarks>
        /// This method returns a set of disjoint Eulerian trails. This set
        /// of trails spans the entire set of edges.
        /// </remarks>
        /// <returns>Eulerian trail set.</returns>
        [NotNull, ItemNotNull]
        public IEnumerable<ICollection<TEdge>> Trails()
        {
            var trail = new List<TEdge>();
            foreach (TEdge edge in _circuit)
            {
                if (_temporaryEdges.Contains(edge))
                {
                    // Store previous trail and start new one
                    if (trail.Count != 0)
                        yield return trail;

                    // Start new trail
                    trail = new List<TEdge>();
                }
                else
                    trail.Add(edge);
            }

            if (trail.Count != 0)
                yield return trail;
        }

        /// <summary>
        /// Computes a set of Eulerian trails, starting at <paramref name="startingVertex"/>
        /// that spans the entire graph.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method computes a set of Eulerian trails starting at <paramref name="startingVertex"/>
        /// that spans the entire graph. The algorithm outline is as follows:
        /// </para>
        /// <para>
        /// The algorithms iterates through the Eulerian circuit of the augmented
        /// graph (the augmented graph is the graph with additional edges to make
        /// the number of odd vertices even).
        /// </para>
        /// <para>
        /// If the current edge is not temporary, it is added to the current trail.
        /// </para>
        /// <para>
        /// If the current edge is temporary, the current trail is finished and
        /// added to the trail collection. The shortest path between the 
        /// start vertex <paramref name="startingVertex"/> and the target vertex of the
        /// temporary edge is then used to start the new trail. This shortest
        /// path is computed using the <see cref="BreadthFirstSearchAlgorithm{TVertex,TEdge}"/>.
        /// </para>
        /// </remarks>
        /// <param name="startingVertex">Starting vertex.</param>
        /// <returns>Eulerian trail set, all starting at <paramref name="startingVertex"/>.</returns>
        /// <exception cref="InvalidOperationException">Eulerian trail not computed yet.</exception>
        [NotNull, ItemNotNull]
        public IEnumerable<ICollection<TEdge>> Trails([NotNull] TVertex startingVertex)
        {
            if (startingVertex == null)
                throw new ArgumentNullException(nameof(startingVertex));

            return TrailsInternal(startingVertex);
        }

        private int FindFirstEdgeInCircuit([NotNull] TVertex startingVertex)
        {
            int i;
            for (i = 0; i < _circuit.Count; ++i)
            {
                TEdge edge = _circuit[i];
                if (_temporaryEdges.Contains(edge))
                    continue;
                if (EqualityComparer<TVertex>.Default.Equals(edge.Source, startingVertex))
                    break;
            }

            if (i == _circuit.Count)
                throw new InvalidOperationException("Did not find vertex in Eulerian trail?");

            return i;
        }

        [NotNull, ItemNotNull]
        private IEnumerable<ICollection<TEdge>> TrailsInternal([NotNull] TVertex startingVertex)
        {
            int index = FindFirstEdgeInCircuit(startingVertex);

            // Create trail
            var trail = new List<TEdge>();
            var bfs = new BreadthFirstSearchAlgorithm<TVertex, TEdge>(VisitedGraph);
            var vis = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (vis.Attach(bfs))
            {
                bfs.Compute(startingVertex);

                // Go through the edges and build the predecessor table
                int start = index;
                for (; index < _circuit.Count; ++index)
                {
                    TEdge edge = _circuit[index];
                    if (_temporaryEdges.Contains(edge))
                    {
                        // Store previous trail and start new one
                        if (trail.Count != 0)
                            yield return trail;

                        // Start new trail
                        // Take the shortest path from the start vertex to the target vertex
                        if (!vis.TryGetPath(edge.Target, out IEnumerable<TEdge> path))
                            throw new InvalidOperationException();
                        trail = new List<TEdge>(path);
                    }
                    else
                        trail.Add(edge);
                }

                // Starting again on the circuit
                for (index = 0; index < start; ++index)
                {
                    TEdge edge = _circuit[index];
                    if (_temporaryEdges.Contains(edge))
                    {
                        // Store previous trail and start new one
                        if (trail.Count != 0)
                            yield return trail;

                        // Start new trail
                        // Take the shortest path from the start vertex to the target vertex
                        if (!vis.TryGetPath(edge.Target, out IEnumerable<TEdge> path))
                            throw new InvalidOperationException();
                        trail = new List<TEdge>(path);
                    }
                    else
                        trail.Add(edge);
                }
            }

            // Adding the last element
            if (trail.Count != 0)
                yield return trail;
        }
    }
}