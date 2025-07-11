using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Job
{
    public class JobSerializer
    {
        JobTimer _timer = new JobTimer();
        Queue<IJob> _jobQueue = new Queue<IJob>();
        object _lock = new object();
        bool _flush = false;

        public void PushAfter(int tickAfter, Action action) { PushAfter(tickAfter, new Job(action)); }
        public void PushAfter<T>(int tickAfter, Action<T> action, T t) { PushAfter(tickAfter, new Job<T>(action, t)); }
        public void PushAfter<T, U>(int tickAfter, Action<T, U> action, T t, U u) { PushAfter(tickAfter, new Job<T, U>(action, t, u)); }
        public void PushAfter<T, U, V>(int tickAfter, Action<T, U, V> action, T t, U u, V v) { PushAfter(tickAfter, new Job<T, U, V>(action, t, u, v)); }

        public void PushAfter(int tickAfter, IJob job)
        {
            _timer.Push(job, tickAfter);
        }

        public void Push(Action action) { Push(new Job(action)); }
        public void Push<T>(Action<T> action, T t) { Push(new Job<T>(action, t)); }
        public void Push<T, U>(Action<T, U> action, T t, U u) { Push(new Job<T, U>(action, t, u)); }
        public void Push<T, U, V>(Action<T, U, V> action, T t, U u, V v) { Push(new Job<T, U, V>(action, t, u, v)); }

        public void Push(IJob job)
        {
            lock (_lock)
            {
                _jobQueue.Enqueue(job);
            }
        }

        public void Flush()
        {
            _timer.Flush();

            while (true)
            {
                IJob job = Pop();
                if (job == null)
                    return;

                job.Execute();
            }
        }

        private IJob Pop()
        {
            lock (_lock)
            {
                if (_jobQueue.Count == 0)
                {
                    _flush = false;
                    return null;
                }
                return _jobQueue.Dequeue();
            }
        }
    }
}
