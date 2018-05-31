using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FoneDynamicsCodingChallenge
{
    /// <summary>
    /// IDictionary.Remove is shadowed, so much be invoked explicitly.
    /// </summary>
    public static class ConcurrentDictionaryExtension
    {
        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key)
        {
            return ((IDictionary<TKey, TValue>)self).Remove(key);
        }
    }
}