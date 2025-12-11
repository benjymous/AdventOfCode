namespace AoC.Utils.Solver
{
    public record class SolverResult<TResult>(TResult Value)
    {
        public static implicit operator SolverResult<TResult>(TResult v) => new(v);
        public static implicit operator TResult(SolverResult<TResult> p) => p.Value;
        public override string ToString() => Value.ToString();
    }

    public class Solver<TElement> : Solver<TElement, int>;

    public class Solver<TElement, TResult>
    {
        readonly PriorityQueue<TElement, int> queue = new();
        readonly Dictionary<int, TResult> cache = [];
        public SolverResult<TResult> CurrentBest = default;
        Func<TResult, TResult, TResult> Filter = default;

        public int PreviousPriority = default;

        public static TResult Solve(TElement initialElement, Func<TElement, Solver<TElement, TResult>, SolverResult<TResult>> action, Func<TResult, TResult, TResult> filter)
        {
            Solver<TElement, TResult> solver = new()
            {
                Filter = filter,
            };
            solver.queue.Enqueue(initialElement, 0);

            return solver.Run(action);
        }

        public static void Solve(TElement initialElement, Action<TElement, Solver<TElement, TResult>> action)
        {
            Solver<TElement, TResult> solver = new();
            solver.queue.Enqueue(initialElement, 0);

            solver.Run(action);
        }

        public static void Solve(IEnumerable<TElement> initialElements, Action<TElement, Solver<TElement, TResult>> action)
        {
            Solver<TElement, TResult> solver = new();
            solver.queue.EnqueueRange(initialElements, 0);

            solver.Run(action);
        }

        public static TResult Solve(IEnumerable<TElement> initialElements, Func<TElement, Solver<TElement, TResult>, SolverResult<TResult>> action, Func<TResult, TResult, TResult> filter)
        {
            Solver<TElement, TResult> solver = new()
            {
                Filter = filter,
            };
            solver.queue.EnqueueRange(initialElements, 0);

            return solver.Run(action);
        }

        public bool IsBetterThanCurrentBest(TResult newVal) => CurrentBest == null || Filter(newVal, CurrentBest) != CurrentBest;

        public bool IsBetterThanSeen(object state, TResult newVal) => IsBetterThanSeen(state.GetHashCode(), newVal);

        public bool IsBetterThanSeen(int key, TResult newVal)
        {
            if ((CurrentBest == null || !Filter(newVal, CurrentBest.Value).Equals(CurrentBest.Value)) && (!cache.TryGetValue(key, out TResult seen) || (!seen.Equals(newVal) && Filter(newVal, seen).Equals(newVal))))
            {
                cache[key] = newVal;
                return true;
            }
            return false;
        }

        public void Enqueue(TElement element) => queue.Enqueue(element, queue.Count);
        public void Enqueue(TElement element, int priority) => queue.Enqueue(element, priority);
        public void EnqueueRange(IEnumerable<(TElement, int)> elements) => queue.EnqueueRange(elements);
        public void EnqueueRange(IEnumerable<TElement> elements) => elements.ForEach(Enqueue);

        private TResult Run(Func<TElement, Solver<TElement, TResult>, SolverResult<TResult>> action)
        {
            while (queue.TryDequeue(out var element, out PreviousPriority))
            {
                var res = action(element, this);
                if (CurrentBest == null) CurrentBest = res;
                else if (res != null) CurrentBest = Filter(CurrentBest, res.Value);
            }
            return CurrentBest != null ? CurrentBest : default(TResult);
        }

        private void Run(Action<TElement, Solver<TElement, TResult>> action)
        {
            while (queue.TryDequeue(out var element, out PreviousPriority))
            {
                action(element, this);
            }
        }

        public void Stop()
        {
            queue.Clear();
        }


        public int Count => queue.Count;

        public void CullQueue(int max)
        {
            if (queue.Count > max + (max / 3))
            {
                List<(TElement, int)> tmp = [];

                while (max-- > 0 && queue.TryDequeue(out var el, out var pri))
                    tmp.Add((el, pri));

                queue.Clear();
                queue.EnqueueRange(tmp);
            }
        }
    }
}