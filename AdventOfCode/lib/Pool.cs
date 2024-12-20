namespace AdventOfCode.lib;

// Dumb pool that ignores capacity for now
abstract class Pool<T>
    where T : new()
{
    private readonly T[] _pool;
    private int _count = 0;

    public Pool(int size)
    {
        _pool = new T[size];
    }

    public T Rent()
    {
        lock (_pool)
        {
            if (_count == 0) 
                return new T();

            _count--;
            return _pool[_count];
        }
    }

    public void Return(T pooledObject)
    {
        Clear(pooledObject);
        lock (_pool)
        {
            if (_count != _pool.Length)
            {
                _pool[_count] = pooledObject;
                _count++;
            }
        }
    }

    protected abstract void Clear(T returnedObject);
}

class HashSetPool<T> : Pool<HashSet<T>>
{
    public HashSetPool(int size)
        : base(size)
    { 
    }

    protected override void Clear(HashSet<T> returnedObject)
    {
        returnedObject.Clear();
    }
}
