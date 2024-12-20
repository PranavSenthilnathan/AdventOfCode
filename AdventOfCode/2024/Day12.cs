using System.Runtime.InteropServices;
using AdventOfCode.lib;

namespace AdventOfCode;

internal static class Day12
{
    [AnswerMethod(2024, 12, 1)]
    public static string Part1(string[] input)
    {
        //input =
        //    """
        //    AAAA
        //    BBCD
        //    BBCC
        //    EEEC
        //    """.Split('\n', StringSplitOptions.TrimEntries);
        var visited = new HashSet<(int, int)>();
        var ans = 0L;
        for (int i = 0; i < input.Length; i++)
        {
            for (global::System.Int32 j = 0; j < input[0].Length; j++)
            {
                if (!visited.Contains((i,j)))
                {
                    var pair = DFS((i, j));
                    ans += pair.Item1 * (pair.Item1 * 4L - pair.Item2);
                }
            }
        }

        (int, int) DFS((int x, int y) pt)
        {
            visited.Add(pt);
            var ret = 1;
            var sharedEdges = 0;
            foreach (var n in pt.GetCardinalNeighbors())
            {
                if ((0, 0).LessThanOrEqualTo(n) && n.LessThan((input.Length, input[0].Length)) &&
                    input[n.Item1][n.Item2] == input[pt.x][pt.y])
                {
                    sharedEdges++;

                    if (!visited.Contains(n))
                    {
                        (ret, sharedEdges) = (ret, sharedEdges).Plus(DFS(n));
                    }                        
                }
            }

            return (ret, sharedEdges);
        }
        return ans.ToString();
    }

    [AnswerMethod(2024, 12, 2)]
    public static string Part2(string[] input)
    {
        //input =
        //    """
        //RRRRIICCFF
        //RRRRIICCCF
        //VVRRRCCFFF
        //VVRCCCJFFF
        //VVVVCJJCFE
        //VVIVCCJJEE
        //VVIIICJJEE
        //MIIIIIJJEE
        //MIIISIJEEE
        //MMMISSJEEE
        //""".Split('\n', StringSplitOptions.TrimEntries);
        var visited = new HashSet<(int, int)>();
        var ans = 0L;
        for (int i = 0; i < input.Length; i++)
        {
            for (global::System.Int32 j = 0; j < input[0].Length; j++)
            {
                var visited2 = new HashSet<(int, int)>();
                if (!visited.Contains((i, j)))
                {
                    var pair = DFS((i, j));
                    var gridPt = new Dictionary<(int, int), int>();
                    foreach (var item in visited2)
                    {
                        CollectionsMarshal.GetValueRefOrAddDefault(gridPt, item, out _)++;
                        CollectionsMarshal.GetValueRefOrAddDefault(gridPt, item.Plus((0, 1)), out _)++;
                        CollectionsMarshal.GetValueRefOrAddDefault(gridPt, item.Plus((1, 1)), out _)++;
                        CollectionsMarshal.GetValueRefOrAddDefault(gridPt, item.Plus((1, 0)), out _)++;
                    }
                    var turns = 0;
                    foreach (var item in gridPt.Select(kvp => kvp.Key).ToList())
                    {
                        if (gridPt[item] == 3 || gridPt[item] == 1)
                        {
                            turns++;
                        }
                        if (gridPt[item] == 2)
                        {
                            if (visited2.Contains(item) && visited2.Contains(item.Minus((1,1))))
                                turns += 2;
                            if (visited2.Contains(item.Minus((1,0))) && visited2.Contains(item.Minus((0, 1))))
                                turns += 2;
                        }
                    }
                    var prod = pair.Item1 * turns;
                    ans += prod;
                }

                (int, int) DFS((int x, int y) pt)
                {
                    visited.Add(pt);
                    visited2.Add(pt);
                    var ret = 1;
                    var sharedEdges = 0;
                    foreach (var n in pt.GetCardinalNeighbors())
                    {
                        if ((0, 0).LessThanOrEqualTo(n) && n.LessThan((input.Length, input[0].Length)) &&
                            input[n.Item1][n.Item2] == input[pt.x][pt.y])
                        {
                            sharedEdges++;

                            if (!visited.Contains(n))
                            {
                                (ret, sharedEdges) = (ret, sharedEdges).Plus(DFS(n));
                            }
                        }
                    }

                    return (ret, sharedEdges);
                }
            }
        }

        
        return ans.ToString();
    }


    [AnswerMethod(2024, 12, 2)]
    public static string Part2_clean(string[] input)
    {
        var grid = input.CreateGrid();
        var visited = new HashSet<(int, int)>();
        var ans = 0L;
        foreach (var pt in grid.Keys)
        { 
            var visited2 = new HashSet<(int, int)>();
            if (!visited.Contains(pt))
            {
                var pair = DFS(pt);
                var corners = new Dictionary<(int, int), int>();
                foreach (var item in visited2)
                {
                    corners.GetValueRefOrAddDefault(item, out _)++;
                    corners.GetValueRefOrAddDefault(item.Plus((0, 1)), out _)++;
                    corners.GetValueRefOrAddDefault(item.Plus((1, 1)), out _)++;
                    corners.GetValueRefOrAddDefault(item.Plus((1, 0)), out _)++;
                }

                var turns = 0;
                foreach (var item in corners.Select(kvp => kvp.Key).ToList())
                {
                    if (corners[item] == 3 || corners[item] == 1)
                    {
                        turns++;
                    }
                    if (corners[item] == 2)
                    {
                        if (visited2.Contains(item) && visited2.Contains(item.Minus((1, 1))))
                            turns += 2;
                        if (visited2.Contains(item.Minus((1, 0))) && visited2.Contains(item.Minus((0, 1))))
                            turns += 2;
                    }
                }
                var prod = pair.Item1 * turns;
                ans += prod;
            }

            (int, int) DFS((int, int) pt)
            {
                visited.Add(pt);
                visited2.Add(pt);
                var v = 1;
                var e = 0;
                foreach (var n in pt.GetCardinalNeighbors())
                {
                    if (grid.TryGetValue(n, out var c) && c == grid[pt])
                    {
                        e++;

                        if (!visited.Contains(n))
                        {
                            (v, e) = (v, e).Plus(DFS(n));
                        }
                    }
                }

                return (v, e);
            }
        }

        return ans.ToString();
    }
}