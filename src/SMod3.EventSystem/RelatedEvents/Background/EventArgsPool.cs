using System;

// Since Mirror (https://github.com/vis2k/Mirror) has a pool system,
// which is the best Queues, Bags in performance,
// I thought, why not implement the same productive solution

namespace SMod3.Module.EventSystem.Background
{
    public sealed class PooledEventArg<T> : IDisposable where T : EventArg, new()
    {
        public T Arg { get; }

        public PooledEventArg(T arg)
        {
            Arg = arg;
        }

        /// <summary>
        ///     Returns to pool.
        /// </summary>
        public void Dispose()
        {
            EventArgsPool<T>.Recycle(this);
        }
    }

    public static class EventArgsPool<T> where T : EventArg, new()
    {
        private static PooledEventArg<T>?[] _pool = new PooledEventArg<T>?[10];
        private static int _next = -1;
        private static readonly object _lockObject = new object();

        public static int Capacity
        {
            get => _pool.Length;
            set
            {
                Array.Resize(ref _pool, value);
                _next = Math.Min(_next, _pool.Length - 1);
            }
        }

        public static PooledEventArg<T> Get()
        {
            if (_next == -1)
            {
                return new PooledEventArg<T>(new T());
            }

            var pooledEventArg = _pool[_next];
            _pool[_next] = null;
            _next--;

            pooledEventArg!.Arg.Reset();

            return pooledEventArg;
        }

        /// <summary>
        ///     Recycle threadsafe.
        /// </summary>
        public static void Recycle(PooledEventArg<T> arg)
        {
            lock (_lockObject)
            {
                // Just increase the capacity if the value grows
                // The number can grow only with a large number of async events,
                // so we leave it to them as a cache
                if (_next >= _pool.Length - 1)
                    Capacity += 5;

                _next++;
                _pool[_next] = arg;
            }
        }
    }
}
