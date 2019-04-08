#if SUPPORTS_KERNIGHANLIN_ALGORITHM
using System.Collections.Generic;

namespace QuickGraph.Algorithms.KernighanLinAlgoritm
{
    public class Partition<TVertex>
    {
        public SortedSet<TVertex> A { get; set; }
        public SortedSet<TVertex> B { get; set; }
        public double cutCost { get; set; }
        public Partition(SortedSet<TVertex> A, SortedSet<TVertex> B, double cutCost = 0)
        {
            this.A = A;
            this.B = B;
            this.cutCost = cutCost;
        }
    }
}
#endif