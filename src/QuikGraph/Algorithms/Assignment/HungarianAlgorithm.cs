using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Assignment
{
    /// <summary>
    /// A combinatorial optimization algorithm that solves the assignment problem, meaning
    /// finding, in a weighted bipartite graph, a matching in which the sum of weights of the
    /// edges is as large as possible.
    /// </summary>
    public sealed class HungarianAlgorithm
    {
        /// <summary>
        /// Hungarian algorithm steps.
        /// </summary>
        /// <remarks>See https://en.wikipedia.org/wiki/Hungarian_algorithm </remarks>
        public enum Steps
        {
            /// <summary>
            /// Initialization step.
            /// </summary>
            Init,

            /// <summary>
            /// Step 1.
            /// </summary>
            Step1,

            /// <summary>
            /// Step 2.
            /// </summary>
            Step2,

            /// <summary>
            /// Step 3.
            /// </summary>
            Step3,

            /// <summary>
            /// Step 4.
            /// </summary>
            Step4,

            /// <summary>
            /// End step.
            /// </summary>
            End
        }

        [NotNull]
        private readonly int[,] _costs;

        private int _width;
        private int _height;

        private byte[,] _masks;
        private bool[] _rowsCovered;
        private bool[] _colsCovered;

        private Steps _step;

        /// <summary>
        /// Computed assignments.
        /// </summary>
        public int[] AgentsTasks { get; private set; }

        private Location _pathStart;
        private Location[] _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="HungarianAlgorithm"/> class.
        /// </summary>
        /// <param name="costs">Costs matrix.</param>
        public HungarianAlgorithm([NotNull] int[,] costs)
        {
            _costs = costs ?? throw new ArgumentNullException(nameof(costs));
            _step = Steps.Init;
        }

        /// <summary>
        /// Returns assignments (without visualization).
        /// </summary>
        /// <returns>Array of assignments.</returns>
        [NotNull]
        public int[] Compute()
        {
            while (DoStep() != Steps.End)
            {
                // Nothing to do there
            }

            return AgentsTasks;
        }

        /// <summary>
        /// Returns iterations that can be used to visualize the algorithm.
        /// </summary>
        /// <returns>An enumerable of algorithm iterations.</returns>
        [Pure]
        [NotNull]
        public IEnumerable<HungarianIteration> GetIterations()
        {
            Steps step = Steps.Init;

            while (step != Steps.End)
            {
                step = DoStep();

                yield return new HungarianIteration(
                    (int[,])_costs.Clone(),
                    (byte[,])_masks.Clone(),
                    (bool[])_rowsCovered.Clone(),
                    (bool[])_colsCovered.Clone(),
                    step);
            }
        }

        private Steps DoStep()
        {
            if (_step == Steps.Init)
                return RunInitStep();

            if (_step != Steps.End)
                return ComputeStep(_step);

            UpdateAgentsTasks();

            return Steps.End;
        }

        private Steps ComputeStep(Steps step)
        {
            switch (step)
            {
                case Steps.Step1:
                {
                    Steps currentStep = step;
                    _step = RunStep1(_masks, _colsCovered, _width, _height);
                    return currentStep;
                }
                case Steps.Step2:
                {
                    Steps currentStep = step;
                    _step = RunStep2(_costs, _masks, _rowsCovered, _colsCovered, _width, _height, ref _pathStart);
                    return currentStep;
                }
                case Steps.Step3:
                {
                    Steps currentStep = step;
                    _step = RunStep3(_masks, _rowsCovered, _colsCovered, _width, _height, _path, _pathStart);
                    return currentStep;
                }
                case Steps.Step4:
                {
                    Steps currentStep = step;
                    _step = RunStep4(_costs, _rowsCovered, _colsCovered, _width, _height);
                    return currentStep;
                }
            }

            return Steps.End;
        }

        private void UpdateAgentsTasks()
        {
            AgentsTasks = new int[_height];

            for (int i = 0; i < _height; ++i)
            {
                for (int j = 0; j < _width; ++j)
                {
                    if (_masks[i, j] == 1)
                    {
                        AgentsTasks[i] = j;
                        break;
                    }
                }
            }
        }

        private void AssignJobs()
        {
            _masks = new byte[_height, _width];
            _rowsCovered = new bool[_height];
            _colsCovered = new bool[_width];

            for (int i = 0; i < _height; ++i)
            {
                for (int j = 0; j < _width; ++j)
                {
                    if (_costs[i, j] == 0 && !_rowsCovered[i] && !_colsCovered[j])
                    {
                        _masks[i, j] = 1;
                        _rowsCovered[i] = true;
                        _colsCovered[j] = true;
                    }
                }
            }
        }

        private Steps RunInitStep()
        {
            _height = _costs.GetLength(0);
            _width = _costs.GetLength(1);

            // Reduce by rows
            for (int i = 0; i < _height; ++i)
            {
                int min = int.MaxValue;
                for (int j = 0; j < _width; ++j)
                    min = Math.Min(min, _costs[i, j]);
                for (int j = 0; j < _width; ++j)
                    _costs[i, j] -= min;
            }

            // Set 1 where job assigned
            AssignJobs();

            ClearCovers(_rowsCovered, _colsCovered, _width, _height);

            _path = new Location[_width * _height];
            _pathStart = default(Location);
            _step = Steps.Step1;

            return Steps.Init;
        }

        private static Steps RunStep1(
            [NotNull] byte[,] masks,
            [NotNull] bool[] colsCovered,
            int width,
            int height)
        {
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (masks[i, j] == 1)
                        colsCovered[j] = true;
                }
            }

            int colsCoveredCount = 0;
            for (int j = 0; j < width; ++j)
            {
                if (colsCovered[j])
                    ++colsCoveredCount;
            }

            return colsCoveredCount == height ? Steps.End : Steps.Step2;
        }

        private static Steps RunStep2(
            [NotNull] int[,] costs,
            [NotNull] byte[,] masks,
            [NotNull] bool[] rowsCovered,
            [NotNull] bool[] colsCovered,
            int width,
            int height,
            ref Location pathStart)
        {
            // Search for another assignment
            Location loc = FindZero(costs, rowsCovered, colsCovered, width, height);

            // If there is not another options we should change matrix
            if (loc.Row == -1)
                return Steps.Step4;

            masks[loc.Row, loc.Column] = 2;
            int starCol = FindStarInRow(masks, width, loc.Row);
            if (starCol != -1)
            {
                rowsCovered[loc.Row] = true;
                colsCovered[starCol] = false;
            }
            else
            {
                pathStart = loc;
                return Steps.Step3;
            }

            return Steps.Step2;
        }

        private static Steps RunStep3(
            [NotNull] byte[,] masks,
            [NotNull] bool[] rowsCovered,
            [NotNull] bool[] colsCovered,
            int width,
            int height,
            [NotNull] Location[] path,
            Location pathStart)
        {
            int pathIndex = 0;
            path[0] = pathStart;
            int row = FindStarInColumn(masks, height, path[pathIndex].Column);
            while (row != -1)
            {
                ++pathIndex;
                path[pathIndex] = new Location(row, path[pathIndex - 1].Column);
                int col = FindPrimeInRow(masks, width, path[pathIndex].Row);

                ++pathIndex;
                path[pathIndex] = new Location(path[pathIndex - 1].Row, col);
                row = FindStarInColumn(masks, height, path[pathIndex].Column);
            }

            ConvertPath(masks, path, pathIndex + 1);
            ClearCovers(rowsCovered, colsCovered, width, height);
            ClearPrimes(masks, width, height);

            return Steps.Step1;
        }

        private static Steps RunStep4(
            [NotNull] int[,] costs,
            [NotNull] bool[] rowsCovered,
            [NotNull] bool[] colsCovered,
            int width,
            int height)
        {
            int minValue = FindMinimum(costs, rowsCovered, colsCovered, width, height);
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (rowsCovered[i])
                        costs[i, j] += minValue;
                    if (!colsCovered[j])
                        costs[i, j] -= minValue;
                }
            }

            return Steps.Step2;
        }

        private static void ConvertPath(
            [NotNull] byte[,] masks,
            [NotNull] Location[] path,
            int pathLength)
        {
            for (int i = 0; i < pathLength; ++i)
            {
                switch (masks[path[i].Row, path[i].Column])
                {
                    case 1:
                        masks[path[i].Row, path[i].Column] = 0;
                        break;
                    case 2:
                        masks[path[i].Row, path[i].Column] = 1;
                        break;
                }
            }
        }

        private static Location FindZero(
            [NotNull] int[,] costs,
            [NotNull] bool[] rowsCovered,
            [NotNull] bool[] colsCovered,
            int width,
            int height)
        {
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (costs[i, j] == 0 && !rowsCovered[i] && !colsCovered[j])
                        return new Location(i, j);
                }
            }

            return Location.InvalidLocation;
        }

        private static int FindMinimum(
            [NotNull] int[,] costs,
            [NotNull] bool[] rowsCovered,
            [NotNull] bool[] colsCovered,
            int width,
            int height)
        {
            int minValue = int.MaxValue;
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (!rowsCovered[i] && !colsCovered[j])
                        minValue = Math.Min(minValue, costs[i, j]);
                }
            }

            return minValue;
        }

        private static int FindStarInRow(
            [NotNull] byte[,] masks,
            int width,
            int row)
        {
            for (int j = 0; j < width; ++j)
            {
                if (masks[row, j] == 1)
                    return j;
            }

            return -1;
        }

        private static int FindStarInColumn(
            [NotNull] byte[,] masks,
            int height,
            int column)
        {
            for (int i = 0; i < height; ++i)
            {
                if (masks[i, column] == 1)
                    return i;
            }

            return -1;
        }

        private static int FindPrimeInRow(
            [NotNull] byte[,] masks,
            int width,
            int row)
        {
            for (int j = 0; j < width; ++j)
            {
                if (masks[row, j] == 2)
                    return j;
            }

            return -1;
        }

        private static void ClearCovers(
            [NotNull] bool[] rowsCovered,
            [NotNull] bool[] colsCovered,
            int width,
            int height)
        {
            for (int i = 0; i < height; ++i)
                rowsCovered[i] = false;
            for (int j = 0; j < width; ++j)
                colsCovered[j] = false;
        }

        private static void ClearPrimes(
            [NotNull] byte[,] masks,
            int width,
            int height)
        {
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (masks[i, j] == 2)
                        masks[i, j] = 0;
                }
            }
        }

        ///<summary>
        /// Represents coordinates: raw and column number.
        /// </summary>
        private struct Location
        {
            public static readonly Location InvalidLocation = new Location(-1, -1);

            public int Row { get; }
            public int Column { get; }

            public Location(int row, int col)
            {
                Row = row;
                Column = col;
            }
        }
    }
}