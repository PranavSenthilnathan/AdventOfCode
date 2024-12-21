using System.Runtime.InteropServices;

namespace AdventOfCode.lib;

internal static class Input
{
    public static Dictionary<(int, int), char> CreateGrid(this string[] input)
    {
        var ret = new Dictionary<(int i, int j), char>();
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                ret.Add((i, j), input[i][j]);
            }
        }

        return ret;
    }

    public static IEnumerable<((int i, int j) point, char value)> EnumerateGrid(this string[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                yield return ((i, j), input[i][j]);
            }
        }
    }

    public static (int i, int j) FindChar(this string[] input, char c)
    {
        for (int i = 0; i < input.Length; i++)
        {
            var j = input[i].IndexOf(c);
            if (j > -1)
                return (i, j);
        }

        throw new Exception("Char not found");
    }

    public static void PrintGrid<T>(this Dictionary<(int, int), T> map, (int, int) item1MinMax = default, (int, int) item2MinMax = default)
    {
        if (item1MinMax.Item2 == default)
            item1MinMax.Item2 = map.Max(kvp => kvp.Key.Item1);

        if (item2MinMax.Item2 == default)
            item2MinMax.Item2 = map.Max(kvp => kvp.Key.Item2);

        for (int i = item1MinMax.Item1; i <= item1MinMax.Item2; i++)
        {
            for (int j = item2MinMax.Item1; j <= item2MinMax.Item2; j++)
            {
                Console.Write(map.GetValueOrDefault((i, j)));
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public static ref TValue? GetValueRefOrAddDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, out bool exists) where TKey : notnull
        => ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out exists);

}
