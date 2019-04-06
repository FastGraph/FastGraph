using System;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using QuickGraph.Algorithms.Services;

namespace QuickGraph.Algorithms
{
    public abstract class RootedSearchAlgorithmBase<TVertex, TGraph>
        : RootedAlgorithmBase<TVertex, TGraph>
    {
        private TVertex _goalVertex;
        private bool hasGoalVertex;

        protected RootedSearchAlgorithmBase(
            IAlgorithmComponent host,
            TGraph visitedGraph)
            :base(host, visitedGraph)
        {}

        public bool TryGetGoalVertex(out TVertex goalVertex)
        {
            if (this.hasGoalVertex)
            {
                goalVertex = this._goalVertex;
                return true;
            }
            else
            {
                goalVertex = default(TVertex);
                return false;
            }
        }

        public void SetGoalVertex(TVertex goalVertex)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(goalVertex != null);
#endif

            bool changed = !Comparison<TVertex>.Equals(this._goalVertex, goalVertex);
            this._goalVertex = goalVertex;
            if (changed)
                this.OnGoalVertexChanged(EventArgs.Empty);
            this.hasGoalVertex = true;
        }

        public void ClearGoalVertex()
        {
            this._goalVertex = default(TVertex);
            this.hasGoalVertex = false;
        }

        public event EventHandler GoalReached;
        protected virtual void OnGoalReached()
        {
            var eh = this.GoalReached;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        public event EventHandler GoalVertexChanged;
        protected virtual void OnGoalVertexChanged(EventArgs e)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(e != null);
#endif

            var eh = this.GoalVertexChanged;
            if (eh != null)
                eh(this, e);
        }

        public void Compute(TVertex root, TVertex goal)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(root != null);
            Contract.Requires(goal != null);
#endif

            this.SetGoalVertex(goal);
            this.Compute(root);
        }
    }
}
