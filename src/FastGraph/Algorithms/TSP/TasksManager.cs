#nullable enable

using JetBrains.Annotations;
using FastGraph.Collections;

namespace FastGraph.Algorithms.TSP
{
    internal sealed class TasksManager<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : EquatableEdge<TVertex>
    {
        private readonly BinaryHeap<TaskPriority, Task<TVertex, TEdge>> _tasksQueue;

        public TasksManager()
        {
            _tasksQueue = new BinaryHeap<TaskPriority, Task<TVertex, TEdge>>();
        }

        /// <summary>
        /// Adds the given <paramref name="task"/> into the <see cref="TasksManager{TVertex,TEdge}"/>.
        /// </summary>
        /// <param name="task">Task to add.</param>
        public void AddTask(Task<TVertex, TEdge> task)
        {
            if (task.MinCost < double.PositiveInfinity)
            {
                _tasksQueue.Add(task.Priority, task);
            }
        }

        /// <summary>
        /// Gets and removes the task with minimal priority.
        /// </summary>
        /// <returns>The <see cref="Task{TVertex,TEdge}"/>.</returns>
        [Pure]
        public Task<TVertex, TEdge> GetTask()
        {
            return _tasksQueue.RemoveMinimum().Value;
        }

        /// <summary>
        /// Checks if there are pending tasks.
        /// </summary>
        /// <returns>True if there are pending tasks, false otherwise.</returns>
        [Pure]
        public bool HasTasks()
        {
            return _tasksQueue.Count > 0;
        }
    }
}
