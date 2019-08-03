using System;
using JetBrains.Annotations;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.TSP
{
    internal class TasksManager<TVertex, TEdge>
        where TEdge : EquatableEdge<TVertex>
    {
        [NotNull]
        private readonly BinaryHeap<TaskPriority, Task<TVertex, TEdge>> _tasksQueue;

        public TasksManager()
        {
            _tasksQueue = new BinaryHeap<TaskPriority, Task<TVertex, TEdge>>();
        }

        /// <summary>
        /// Adds the given <paramref name="task"/> into the <see cref="TasksManager{TVertex,TEdge}"/>.
        /// </summary>
        /// <param name="task">Task to add.</param>
        public void AddTask([NotNull] Task<TVertex, TEdge> task)
        {
            if (task is null)
                throw new ArgumentNullException(nameof(task));

            if (task.MinCost < double.PositiveInfinity)
            {
                _tasksQueue.Add(task.Priority, task);
            }
        }

        /// <summary>
        /// Gets and removes the task with minimal priority.
        /// </summary>
        /// <returns>The <see cref="Task{TVertex,TEdge}"/>.</returns>
        [NotNull]
        public Task<TVertex, TEdge> GetTask()
        {
            return _tasksQueue.RemoveMinimum().Value;
        }

        /// <summary>
        /// Checks if there are pending tasks.
        /// </summary>
        /// <returns>True if there are pending tasks, false otherwise.</returns>
        [JetBrains.Annotations.Pure]
        public bool HasTasks()
        {
            return _tasksQueue.Count > 0;
        }
    }

}
