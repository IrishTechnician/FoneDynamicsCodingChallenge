using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FoneDynamicsCodingChallenge
{
    public class Cache<TKey, TValue>: ICache<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        private readonly int _capacity;
        private readonly object _concurrencyLock = new object();
        private readonly LinkedList<Tuple<int, TValue>> _accessOrder;
        private readonly ConcurrentDictionary<int, LinkedListNode<Tuple<int, TValue>>> _cache;
        private int _size;

        /// <param name="capacity">Number of items expected to keep in the cache</param>
        public Cache(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException (nameof(capacity),"The capacity argument must be greater than or equal to zero.");
           _capacity = capacity;
            _size = 0;

            // The higher the concurrencyLevel, the higher the theoretical number of operations
            // that could be performed concurrently on the ConcurrentDictionary.  However, global
            // operations like resizing the dictionary take longer as the concurrencyLevel rises. 
            // For the purposes of this example, we'll compromise at numCores * 2.
            var numProcs = Environment.ProcessorCount;
            var concurrencyLevel = numProcs * 2;

            _cache = new ConcurrentDictionary<int, LinkedListNode<Tuple<int, TValue>>>(concurrencyLevel, capacity);
            _accessOrder = new LinkedList<Tuple<int, TValue>>();
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            var hashKey = key.GetHashCode();

            lock (_concurrencyLock)
            {
                if (_cache.ContainsKey(hashKey))
                {
                    var node = _cache[hashKey];
                    _accessOrder.Remove(node);
                    node.Value = new Tuple<int, TValue>(hashKey, value);
                    _accessOrder.AddLast(node);
                }
                else
                {
                    while (_size >= _capacity)
                    {
                        var node = _accessOrder.First;
                        _cache.Remove(node.Value.Item1);
                        _accessOrder.RemoveFirst();
                        _size--;
                    }

                    _accessOrder.AddLast(new Tuple<int, TValue>(hashKey, value));
                    _cache[hashKey] = _accessOrder.Last;
                    _size++;

                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var hashKey = key.GetHashCode();

            if (_cache.ContainsKey(hashKey))
            {
                lock (_concurrencyLock)
                {
                    var node = _cache[hashKey];
                    _accessOrder.Remove(node);
                    _accessOrder.AddLast(node);
                    value = node.Value.Item2;
                }

                return true;
            }

            value = default(TValue);
            return false;
        }
    }
}
