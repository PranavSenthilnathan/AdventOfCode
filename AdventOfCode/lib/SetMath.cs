using System.Buffers;

namespace AdventOfCode.lib;

internal static class SetMath
{
    public static IEnumerable<(T1, T2)> CartesianProduct<T1, T2>(this IEnumerable<T1> t1, IEnumerable<T2> t2)
    {
        return t1.SelectMany(x => t2.Select(y => (x, y)));
    }

    /// <summary>
    /// Given a set <paramref name="S"/>, returns the set of all values in the <paramref name="n"/>-dimensional space <paramref name="S"/>^<paramref name="n"/>.
    /// 
    /// <example>
    /// <code>
    /// [0,1].ToTheNthDimension(2) = [[0,0], [0,1], [1,0], [1,1]]
    /// </code>
    /// </example>
    /// </summary>
    public static IEnumerable<IEnumerable<T>> ToTheNthDimension<T>(this IEnumerable<T> S, int n)
    {
        if (n == 0) return [[]];
        return S.ToTheNthDimension(n - 1).ToArray().SelectMany(x => S.Select(s => Enumerable.Repeat(s, 1).Concat(x)));
    }
}
