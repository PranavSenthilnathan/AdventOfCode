using System.Runtime.InteropServices;

namespace AdventOfCode.lib;

public class UnionFind<T>
    where T : notnull, IEquatable<T>
{
    Dictionary<T, T> parents = new Dictionary<T, T>();

    public T Find(T a)
    {
        ref var ptr = ref parents.GetValueRefOrAddDefault(a, out var exists);
        if (!exists)
            return ptr = a;

        return FindImpl(ptr!);
    }

    T FindImpl(T a)
    {
        ref var pRef = ref CollectionsMarshal.GetValueRefOrNullRef(parents, a);
        if (pRef!.Equals(a))
            return a;

        return pRef = FindImpl(pRef);
    }

    public T Union(T a, T b) => parents[Find(a)] = Find(b);

    public void Insert(T a) => parents[a] = a;

    public bool Contains(T a) => parents.ContainsKey(a);
}