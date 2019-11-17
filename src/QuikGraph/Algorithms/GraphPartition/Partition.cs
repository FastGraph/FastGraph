#if SUPPORTS_SORTEDSET
using System.Collections.Generic;
#else
using QuikGraph.Collections;
#endif
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.GraphPartition
{
    /// <summary>
    /// Represents a graph partition in two sub sets.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public struct Partition<TVertex>
    {
        /// <summary>
        /// First sub set of vertices.
        /// </summary>
        [NotNull, ItemNotNull]
        public SortedSet<TVertex> VertexSetA { get; }

        /// <summary>
        /// Second sub set of vertices.
        /// </summary>
        [NotNull, ItemNotNull]
        public SortedSet<TVertex> VertexSetB { get; }

        /// <summary>
        /// Partition cut cost (to minimize).
        /// </summary>
        public double CutCost { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Partition{TVertex}"/> class.
        /// </summary>
        /// <param name="vertexSetA">First partition vertex set.</param>
        /// <param name="vertexSetB">Second partition vertex set.</param>
        /// <param name="cutCost">Cost of the partition cut.</param>
        public Partition(
            [NotNull, ItemNotNull] SortedSet<TVertex> vertexSetA,
            [NotNull, ItemNotNull] SortedSet<TVertex> vertexSetB, 
            double cutCost = 0)
        {
            VertexSetA = vertexSetA;
            VertexSetB = vertexSetB;
            CutCost = cutCost;
        }

        /// <summary>
        /// Checks if both partitions are equals or equivalent.
        /// </summary>
        /// <param name="partition1">First partition.</param>
        /// <param name="partition2">Second partition.</param>
        /// <returns>True if both partitions are at least equivalent, false otherwise.</returns>
        public static bool AreEquivalent(Partition<TVertex> partition1, Partition<TVertex> partition2)
        {
            return partition1.VertexSetA.SetEquals(partition2.VertexSetA) 
                   && partition1.VertexSetB.SetEquals(partition2.VertexSetB)
                   || 
                   partition1.VertexSetA.SetEquals(partition2.VertexSetB) 
                   && partition1.VertexSetB.SetEquals(partition2.VertexSetA);
        }
    }

    /// <summary>
    /// Helpers to work with <see cref="Partition{TVertex}"/>.
    /// </summary>
    public static class PartitionHelpers
    {
        /// <summary>
        /// Checks if both partitions are equals or equivalent.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <param name="partition1">First partition.</param>
        /// <param name="partition2">Second partition.</param>
        /// <returns>True if both partitions are at least equivalent, false otherwise.</returns>
        public static bool AreEquivalent<TVertex>(Partition<TVertex> partition1, Partition<TVertex> partition2)
        {
            return Partition<TVertex>.AreEquivalent(partition1, partition2);
        }
    }
}