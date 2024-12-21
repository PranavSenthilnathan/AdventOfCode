using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode.lib;

namespace AdventOfCode;

internal static partial class Day20
{
    [AnswerMethod(2024, 20, 1)]
    public static string Part1(string[] input)
    {
        //input = """
        //    ###############
        //    #...#...#.....#
        //    #.#.#.#.#.###.#
        //    #S#...#.#.#...#
        //    #######.#.#.###
        //    #######.#.#...#
        //    #######.#.###.#
        //    ###..E#...#...#
        //    ###.#######.###
        //    #...###...#...#
        //    #.#####.#.###.#
        //    #.#...#.#.#...#
        //    #.#.#.#.#.#.###
        //    #...#...#...###
        //    ###############
        //    """.Split('\n', StringSplitOptions.TrimEntries);

        var grid = input.CreateGrid();
        var distanceToEnd = input.CreateGrid().ToDictionary(kvp => kvp.Key, kvp  => - 1);

        var start = grid.Single(kvp => kvp.Value == 'S').Key;
        var end = grid.Single(kvp => kvp.Value == 'E').Key;

        // var cheats = ValueTupleExtensions.GetCardinalDirections<int, int>().SelectMany(d => d.GetCardinalNeighbors()).Distinct().ToArray();

        var q = new Queue<(int, int)>();
        q.Enqueue(end);
        distanceToEnd[end] = 0;

        while (q.TryDequeue(out var curr))
        {
            foreach (var n in curr.GetCardinalNeighbors().Where(n => grid.TryGetValue(n, out var val) && val != '#'))
            {
                if (distanceToEnd[n] != -1)
                    continue;

                distanceToEnd[n] = distanceToEnd[curr] + 1;
                q.Enqueue(n);
            }
        }

        var cheats = new HashSet<(int, int)>();
        foreach (var kvp in grid.Where(kvp => kvp.Value == '#'))
        {
            foreach (var n1 in kvp.Key.GetCardinalNeighbors().Where(n => grid.TryGetValue(n, out var val) && val != '#'))
            {
                foreach (var n2 in kvp.Key.GetCardinalNeighbors().Where(n => grid.TryGetValue(n, out var val) && val != '#'))
                {
                    if (Math.Max(distanceToEnd[n1], distanceToEnd[n2]) >= Math.Min(distanceToEnd[n1], distanceToEnd[n2]) + 100 + 2)
                        cheats.Add((Math.Max(distanceToEnd[n1], distanceToEnd[n2]), Math.Min(distanceToEnd[n1], distanceToEnd[n2])));
                }
            }
        }

        return cheats.Count.ToString();
    }

    //[AnswerMethod(2024, 20, 2)]
    public static string Part2(string[] input)
    {
        //input = """
        //    ###############
        //    #...#...#.....#
        //    #.#.#.#.#.###.#
        //    #S#...#.#.#...#
        //    #######.#.#.###
        //    #######.#.#...#
        //    #######.#.###.#
        //    ###..E#...#...#
        //    ###.#######.###
        //    #...###...#...#
        //    #.#####.#.###.#
        //    #.#...#.#.#...#
        //    #.#.#.#.#.#.###
        //    #...#...#...###
        //    ###############
        //    """.Split('\n', StringSplitOptions.TrimEntries);

        var grid = input.CreateGrid();
        var distanceToEnd = input.CreateGrid().ToDictionary(kvp => kvp.Key, kvp => -1);

        var start = grid.Single(kvp => kvp.Value == 'S').Key;
        var end = grid.Single(kvp => kvp.Value == 'E').Key;

        var q = new Queue<(int, int)>();
        q.Enqueue(end);
        distanceToEnd[end] = 0;

        while (q.TryDequeue(out var curr))
        {
            foreach (var n in curr.GetCardinalNeighbors().Where(n => grid.TryGetValue(n, out var val) && val != '#'))
            {
                if (distanceToEnd[n] != -1)
                    continue;

                distanceToEnd[n] = distanceToEnd[curr] + 1;
                q.Enqueue(n);
            }
        }

        var cheats = new HashSet<((int, int), (int, int))>();
        var nAwayNeighbors = NAwayNeighbors(20);
        foreach (var (cheatStart, _) in grid.Where(kvp => kvp.Value != '#'))
        {
            foreach (var (cheatEnd, cost) in nAwayNeighbors.Select(d => (cheatStart.Plus(d.Key), d.Value)).Where(n => grid.TryGetValue(n.Item1, out var val) && val != '#'))
            {
                if (distanceToEnd[cheatStart] <= distanceToEnd[cheatEnd])
                    continue;

                var max = distanceToEnd[cheatStart];
                var min = distanceToEnd[cheatEnd];
                if (max >= min + 100 + cost)
                {
                    cheats.Add((cheatStart, cheatEnd));
                    //((cheatsDict.GetValueRefOrAddDefault(max - min - cost, out _)) ??= new HashSet<((int, int), (int, int))>()).Add((cheatStart, cheatEnd));
                }
            }
        }

        return cheats.Count.ToString();

        static Dictionary<(int, int), int> NAwayNeighbors(int N)
        {
            var ns = new Dictionary<(int, int), int>();
            for (var i = -N; i <= N; i++) 
            { 
                for (var j = -N; j <= N; j++)
                {
                    if (Math.Abs(i) + Math.Abs(j) <= N)
                        ns[(i, j)] = Math.Abs(i) + Math.Abs(j);
                }
            }
            return ns;
        }
    }

    [AnswerMethod(2024, 20, 2)]
    public static string Part2_faster(string[] input)
    {
        //input = """
        //    ###############
        //    #...#...#.....#
        //    #.#.#.#.#.###.#
        //    #S#...#.#.#...#
        //    #######.#.#.###
        //    #######.#.#...#
        //    #######.#.###.#
        //    ###..E#...#...#
        //    ###.#######.###
        //    #...###...#...#
        //    #.#####.#.###.#
        //    #.#...#.#.#...#
        //    #.#.#.#.#.#.###
        //    #...#...#...###
        //    ###############
        //    """.Split('\n', StringSplitOptions.TrimEntries);

        var grid = input.EnumerateGrid().Where(p => p.value != '#').Select(p => p.point).ToHashSet()!;
        var start = input.FindChar('S');
        var end = input.FindChar('E');

        var distanceToEnd = new Dictionary<(int, int), int>();
        var curr = end;
        distanceToEnd[end] = 0;
        while (curr != start)
        {
            foreach (var n in curr.GetCardinalNeighbors())
            {
                if (grid.Contains(n) && !distanceToEnd.ContainsKey(n))
                {
                    curr = n;
                    break;
                }
            }

            distanceToEnd[curr] = distanceToEnd.Count;
        }

        var ans = 0;
        var nAwayNeighbors = NAwayNeighbors(20);
        foreach (var (cheatEnd, cheatEndDistance) in distanceToEnd)
        {
            distanceToEnd.Remove(cheatEnd);
            foreach (var (pos, cost) in nAwayNeighbors)
            {
                var cheatStart = cheatEnd.Plus(pos);

                if (!distanceToEnd.TryGetValue(cheatStart, out var cheatStartDistance))
                    continue;

                if (cheatStartDistance >= cheatEndDistance + 100 + cost)
                {
                    ans++;
                }
            }
        }

        return ans.ToString();

        static ((int, int), int)[] NAwayNeighbors(int N)
        {
            var ns = new Dictionary<(int, int), int>();
            for (var i = -N; i <= N; i++)
            {
                for (var j = -N; j <= N; j++)
                {
                    if (Math.Abs(i) + Math.Abs(j) <= N)
                        ns[(i, j)] = Math.Abs(i) + Math.Abs(j);
                }
            }
            return [.. ns.Select(ValueTupleExtensions.ToValueTuple)];
        }
    }
}