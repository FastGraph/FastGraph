using System;
using JetBrains.Annotations;

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

        #region Equality

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            return obj is TaskPriority priority && Equals(priority);
        }

        protected bool Equals([NotNull] TaskPriority other)
        {
            return _cost.Equals(other._cost) 
                   && _pathSize == other._pathSize;
        }

        public static bool operator ==(TaskPriority priority1, TaskPriority priority2)
        {
            if (priority1 is null)
                return priority2 is null;
            if (priority2 is null)
                return false;
            return priority1.Equals(priority2);
        }

        public static bool operator !=(TaskPriority priority1, TaskPriority priority2)
        {
            return !(priority1 == priority2);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (_cost.GetHashCode() * 397) ^ _pathSize;
        }

        #endregion

        #region IComparable<T>

        /// <inheritdoc />
        public int CompareTo(TaskPriority other)
        {
            if (other is null)
                return 1;

            int costCompare = _cost.CompareTo(other._cost);
            if (costCompare == 0)
                return -_pathSize.CompareTo(other._pathSize);
            return costCompare;
        }

        public static bool operator <(TaskPriority left, TaskPriority right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(TaskPriority left, TaskPriority right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(TaskPriority left, TaskPriority right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(TaskPriority left, TaskPriority right)
        {
            return left.CompareTo(right) >= 0;
        }

        #endregion
    }
}