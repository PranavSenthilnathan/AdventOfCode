using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using AdventOfCode.lib;
using BenchmarkDotNet.Attributes;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode
{
    internal static partial class Day18
    {
        [AnswerMethod(2024, 18, 1)]
        public static string Part1(string[] input)
        {
            //input =
            //    """
            //    5,4
            //    4,2
            //    4,5
            //    3,0
            //    2,1
            //    6,3
            //    2,4
            //    1,5
            //    0,6
            //    3,3
            //    2,6
            //    5,1
            //    1,2
            //    5,5
            //    2,5
            //    6,5
            //    1,4
            //    0,4
            //    6,4
            //    1,1
            //    6,1
            //    1,0
            //    0,5
            //    1,6
            //    2,0
            //    """.Split('\n', StringSplitOptions.TrimEntries);
            var grid = new Dictionary<(int, int), char>();
            //var N = 70;
            var N = 70;
            var T = 1024;
            for (int i = 0; i <= N; i++)
            {
                for (global::System.Int32 j = 0; j <= N; j++)
                {
                    grid[(i, j)] = '.';
                }
            }

            foreach (var line in input.Take(T))
            {
                var spl = line.Split(",").Select(int.Parse).ToArray();
                var (c, r) = (spl[0], spl[1]);

                grid[(r, c)] = '#';
            }

            //grid.PrintGrid();

            var visited = new HashSet<(int, int)>();
            var next = new List<(int, int)>() { (0, 0) };

            var cnt = 0;
            while (next.Count > 0)
            {
                var newNext = new List<(int, int)>();

                foreach (var (r, c) in next)
                {
                    if ((r, c) == (N, N))
                    {
                        return cnt.ToString();
                    }

                    if (visited.Contains((r, c)))
                    {
                        continue;
                    }

                    visited.Add((r, c));

                    foreach (var n in (r, c).GetCardinalNeighbors().Where(x => grid.TryGetValue(x, out var y) && y != '#'))
                    {
                        newNext.Add(n);
                    }
                }

                cnt++;
                next = newNext;
            }

            throw new Exception();
        }

        //[AnswerMethod(2024, 18, 2)]
        public static string Part2(string[] input)
        {
            //input =
            //    """
            //    5,4
            //    4,2
            //    4,5
            //    3,0
            //    2,1
            //    6,3
            //    2,4
            //    1,5
            //    0,6
            //    3,3
            //    2,6
            //    5,1
            //    1,2
            //    5,5
            //    2,5
            //    6,5
            //    1,4
            //    0,4
            //    6,4
            //    1,1
            //    6,1
            //    1,0
            //    0,5
            //    1,6
            //    2,0
            //    """.Split('\n', StringSplitOptions.TrimEntries);
            var grid = new Dictionary<(int, int), char>();
            //var N = 70;
            var N = 70;
            for (int i = 0; i <= N; i++)
            {
                for (global::System.Int32 j = 0; j <= N; j++)
                {
                    grid[(i, j)] = '.';
                }
            }

            foreach (var line in input)
            {
                var spl = line.Split(",").Select(int.Parse).ToArray();
                var (c, r) = (spl[0], spl[1]);

                grid[(r, c)] = '#';

                if (Run(grid, N) == null)
                    return (c, r).ToString();
            }

            //grid.PrintGrid();
            throw new Exception();

            static string? Run(Dictionary<(int, int), char> grid, int N)
            {
                var visited = new HashSet<(int, int)>();
                var next = new List<(int, int)>() { (0, 0) };

                var cnt = 0;
                while (next.Count > 0)
                {
                    var newNext = new List<(int, int)>();

                    foreach (var (r, c) in next)
                    {
                        if ((r, c) == (N, N))
                        {
                            return cnt.ToString();
                        }

                        if (visited.Contains((r, c)))
                        {
                            continue;
                        }

                        visited.Add((r, c));

                        foreach (var n in (r, c).GetCardinalNeighbors().Where(x => grid.TryGetValue(x, out var y) && y != '#'))
                        {
                            if (!visited.Contains(n))
                            {
                                newNext.Add(n);
                            }
                        }
                    }

                    cnt++;
                    next = newNext;
                }

                return null;
            }
        }

        [AnswerMethod(2024, 18, 2)]
        public static string Part2_faster(string[] input)
        {
            var N = 70;

            var topright = (int.MinValue, int.MaxValue);
            var bottomleft = (int.MaxValue, int.MinValue);

            var uf = new UnionFind<(int, int)>();
            foreach (var line in input)
            {
                var idx = line.IndexOf(',');
                var curr = (int.Parse(line.AsSpan(idx + 1)), int.Parse(line.AsSpan(0, idx)));
                uf.Insert(curr);
                foreach (var n in curr.GetAllNeighbors())
                {
                    if (uf.Contains(n))
                    {
                        uf.Union(n, curr);
                        continue;
                    }

                    if (n.Item1 < 0 || n.Item2 > N)
                    {
                        uf.Union(curr, topright);
                    }
                    else if (n.Item1 > N || n.Item2 < 0)
                    {
                        uf.Union(curr, bottomleft);
                    }
                }

                if (uf.Find(topright) == uf.Find(bottomleft))
                {
                    return (curr.Item2, curr.Item1).ToString();
                }
            }

            throw new Exception();
        }
    }
}