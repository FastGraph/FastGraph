using System;

namespace QuikGraph.Algorithms.TSP
{
    internal class TaskPriority : IComparable<TaskPriority>
    {
        private readonly double _cost;
        private readonly int _pathSize;

        public TaskPriority(double cost, int pathSize)
        {
            _cost = cost;
            _pathSize = pathSize;
        }

        public int CompareTo(TaskPriority other)
        {
            var costCompare = _cost.CompareTo(other._cost);
            if (costCompare == 0)
                return -_pathSize.CompareTo(other._pathSize);
            return costCompare;
        }
    }
}
