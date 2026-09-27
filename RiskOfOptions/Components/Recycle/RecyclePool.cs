using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiskOfOptions.Components.Recycle;

internal class RecyclePool<TPoolItem, TData> : IEnumerable<TPoolItem>
    where TPoolItem : Component, IRecycleViewItem<TData>
{
    private readonly List<TPoolItem> _pool = [];
    private int _firstIndexInPool;
    private int _lastIndexInPool;
    
    public int FirstIndex { get; private set; }
    public int LastIndex { get; private set; }

    public int Size => _pool.Count;

    public void Add(TPoolItem item)
    {
        _pool.Add(item);

        _lastIndexInPool = Size - 1;
        LastIndex = _lastIndexInPool;
    }

    public TPoolItem RecycleForwards()
    {
        _lastIndexInPool = _firstIndexInPool;
        _firstIndexInPool = (_firstIndexInPool + 1) % Size;

        FirstIndex++;
        LastIndex++;

        return _pool[_lastIndexInPool];
    }

    public TPoolItem RecycleBackwards()
    {
        _firstIndexInPool = _lastIndexInPool;
        _lastIndexInPool = (_lastIndexInPool - 1 + Size) % Size;

        FirstIndex--;
        LastIndex--;

        return _pool[_firstIndexInPool];
    }

    public TPoolItem GetFirst() => _pool[_firstIndexInPool];

    public TPoolItem GetLast() => _pool[_lastIndexInPool];

    public void Clear()
    {
        foreach (var item in _pool)
            item.Dispose();
        
        _pool.Clear();

        _firstIndexInPool = 0;
        _lastIndexInPool = 0;
        FirstIndex = _firstIndexInPool;
        LastIndex = _lastIndexInPool;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TPoolItem> GetEnumerator() => new RecyclePoolEnumerator(_pool, _firstIndexInPool);
    
    private class RecyclePoolEnumerator(IReadOnlyList<TPoolItem> pool, int firstIndex) : IEnumerator<TPoolItem>
    {
        private int _poolPos = firstIndex - 1;
        
        public bool MoveNext()
        {
            _poolPos++;

            return _poolPos < pool.Count;
        }

        public void Reset()
        {
            _poolPos = firstIndex - 1;
        }

        object IEnumerator.Current => Current;

        public TPoolItem Current => pool[_poolPos];

        public void Dispose() { }
    }
}