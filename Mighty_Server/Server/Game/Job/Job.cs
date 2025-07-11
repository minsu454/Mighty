using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Job
{
    public interface IJob
    {
        void Execute();
    }

    public class Job : IJob
    {
        private Action _action;

        public Job(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action.Invoke();
        }
    }

    public class Job<T> : IJob
    {
        private Action<T> _action;
        private T _t;

        public Job(Action<T> action, T t)
        {
            _action = action;
            _t = t;
        }

        public void Execute()
        {
            _action.Invoke(_t);
        }
    }

    public class Job<T, U> : IJob
    {
        private Action<T, U> _action;
        private T _t;
        private U _u;

        public Job(Action<T, U> action, T t, U u)
        {
            _action = action;
            _t = t;
            _u = u;
        }

        public void Execute()
        {
            _action.Invoke(_t, _u);
        }
    }

    public class Job<T, U, V> : IJob
    {
        private Action<T, U, V> _action;
        private T _t;
        private U _u;
        private V _v;

        public Job(Action<T, U, V> action, T t, U u, V v)
        {
            _action = action;
            _t = t;
            _u = u;
            _v = v;
        }

        public void Execute()
        {
            _action.Invoke(_t, _u, _v);
        }
    }
}
