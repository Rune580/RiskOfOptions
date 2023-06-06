using System.Collections.Generic;

namespace RiskOfOptions.Lib
{
    internal static class KeyValuePairExtensions
    {
        internal static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> keyValuePair, out TKey key, out TValue value)
        {
            key = keyValuePair.Key;
            value = keyValuePair.Value;
        } 
    }
}