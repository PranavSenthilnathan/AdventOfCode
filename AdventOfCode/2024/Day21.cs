﻿using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.lib;

namespace AdventOfCode;

internal static partial class Day21
{
    [AnswerMethod(2024, 21, 1)]
    public static string Part1(string[] input)
    {
        //input = """
        //    029A
        //    980A
        //    179A
        //    456A
        //    379A
        //    """.Split('\n', StringSplitOptions.TrimEntries);
        var keypad = new Dictionary<char, (int, int)>()
        {
            ['A'] = (3, 1),
            ['0'] = (2, 1),
            ['1'] = (1, 2),
            ['2'] = (2, 2),
            ['3'] = (3, 2),
            ['4'] = (1, 3),
            ['5'] = (2, 3),
            ['6'] = (3, 3),
            ['7'] = (1, 4),
            ['8'] = (2, 4),
            ['9'] = (3, 4),
        };

        var keyLocations = keypad.Values.ToHashSet();

        var dpad = new Dictionary<char, (int, int)>()
        {
            ['A'] = (3, 2),
            ['^'] = (2, 2),
            ['<'] = (1, 1),
            ['v'] = (2, 1),
            ['>'] = (3, 1),
        };

        var dpadLocations = dpad.Values.ToHashSet();

        var ans = 0L;
        foreach (var line in input)
        {
            var min = long.MaxValue;
            var p1s = FindPaths((3, 1), line, keypad, [.. keypad.Values]).ToList();
            foreach (var p1 in p1s)
            {
                var p2s = FindPaths((3, 2), p1.ToList(), dpad, [.. dpad.Values]).ToList();
                foreach (var p2 in p2s)
                {
                    var p3s = FindPaths((3, 2), p2.ToList(), dpad, [.. dpad.Values]);
                    min = Math.Min(min, p3s.Min(p => p.Count()));
                }
            }
            Console.WriteLine(long.Parse(line[0..^1]));
            Console.WriteLine(min);
            ans += long.Parse(line[0..^1]) * min;
        }

        return ans.ToString();

        static IEnumerable<IEnumerable<char>> FindPaths((int, int) curr, IEnumerable<char> origPath, Dictionary<char, (int, int)> lookup, HashSet<(int, int)> locationSet)
        {
            if (!origPath.Any())
                return [[]];
            IEnumerable<IEnumerable<char>> paths = [];
            var ch = origPath.ElementAt(0);
            {
                var next = lookup[ch];
                if (next == curr)
                {
                    return FindPaths(curr, origPath.Skip(1), lookup, locationSet).Select(p => new List<char> { 'A' }.Concat(p)).ToList();
                }

                {
                    if (next.Item1 > curr.Item1)
                    {
                        if (locationSet.Contains(curr.Plus((1, 0))))
                        {
                            var temp = FindPaths(curr.Plus((1, 0)), origPath, lookup, locationSet);
                            paths = paths.Concat(temp.Select(p => new List<char> { '>' }.Concat(p)));
                        }
                    }
                    if (next.Item2 < curr.Item2)
                    {
                        if (locationSet.Contains(curr.Plus((0, -1))))
                        {
                            var temp = FindPaths(curr.Plus((0, -1)), origPath, lookup, locationSet);
                            paths = paths.Concat(temp.Select(p => new List<char> { 'v' }.Concat(p)));
                        }
                    }
                    if (next.Item2 > curr.Item2)
                    {
                        if (locationSet.Contains(curr.Plus((0, 1))))
                        {
                            var temp = FindPaths(curr.Plus((0, 1)), origPath, lookup, locationSet);
                            paths = paths.Concat(temp.Select(p => new List<char> { '^' }.Concat(p)));
                        }
                    }
                    if (next.Item1 < curr.Item1)
                    {
                        if (locationSet.Contains(curr.Plus((-1, 0))))
                        {
                            var temp = FindPaths(curr.Plus((-1, 0)), origPath, lookup, locationSet);
                            paths = paths.Concat(temp.Select(p => new List<char> { '<' }.Concat(p)));
                        }
                    }
                }
            }
            return paths;
        }
    }

    [AnswerMethod(2024, 21, 2)]
    public static string Part2(string[] input)
    {
        //input = """
        //    029A
        //    980A
        //    179A
        //    456A
        //    379A
        //    """.Split('\n', StringSplitOptions.TrimEntries);
        var keypad = new Dictionary<char, (int, int)>()
        {
            ['A'] = (3, 1),
            ['0'] = (2, 1),
            ['1'] = (1, 2),
            ['2'] = (2, 2),
            ['3'] = (3, 2),
            ['4'] = (1, 3),
            ['5'] = (2, 3),
            ['6'] = (3, 3),
            ['7'] = (1, 4),
            ['8'] = (2, 4),
            ['9'] = (3, 4),
        };

        var keyLocations = keypad.Values.ToHashSet();

        var dpad = new Dictionary<char, (int, int)>()
        {
            ['A'] = (3, 2),
            ['^'] = (2, 2),
            ['<'] = (1, 1),
            ['v'] = (2, 1),
            ['>'] = (3, 1),
        };

        var dpadLocations = dpad.Values.ToHashSet();

        var cache = new Dictionary<(char, char, int), long>();
        var ans = 0L;
        foreach (var line in input)
        {
            var p1s = FindPaths((3, 1), line, keypad, [.. keypad.Values]).ToList();
            var min = long.MaxValue;
            foreach (var p1 in p1s)
            {
                ReadOnlySpan<char> temp = p1.ToArray();
                var len = 0L;
                var start = 'A';
                for (global::System.Int32 i = 0; i < temp.Length; i++)
                {
                    var end = temp[i];
                    len += Length(start, end, 25);
                    start = end;
                }

                //for (global::System.Int32 i = 1; i <= 25; i++)
                //{
                //    temp = FindPath((3, 2), temp, dpad);
                //}

                min = Math.Min(min, len);
            }

            Console.WriteLine(min);
            ans += min * long.Parse(line[0..^1]);
        }

        return ans.ToString();

        long Length(char start, char end, int remainingIterations)
        {
            if (remainingIterations == 0) return 1;

            if (start == end) return 1; // just press A

            if (cache.TryGetValue((start, end, remainingIterations), out var cached))
                return cached;

            return cache[(start, end, remainingIterations)] = ((start, end) switch
            {
                ('>', '^') => long.Min(ComputeHelper('^', '<'), ComputeHelper('<', '^')),
                ('>', 'v') => ComputeHelper('<'),
                ('>', '<') => ComputeHelper('<', '<'),

                ('^', '>') => long.Min(ComputeHelper('>', 'v'), ComputeHelper('v', '>')),
                ('^', '<') => ComputeHelper('v', '<'),
                ('^', 'v') => ComputeHelper('v'),

                ('<', '>') => ComputeHelper('>', '>'),
                ('<', '^') => ComputeHelper('>', '^'),
                ('<', 'v') => ComputeHelper('>'),

                ('v', '>') => ComputeHelper('>'),
                ('v', '^') => ComputeHelper('^'),
                ('v', '<') => ComputeHelper('<'),

                ('A', '>') => ComputeHelper('v'),
                ('A', '^') => ComputeHelper('<'),
                ('A', '<') => long.Min(ComputeHelper('v', '<', '<'), ComputeHelper('<', 'v', '<')),
                ('A', 'v') => long.Min(ComputeHelper('v', '<'), ComputeHelper('<', 'v')),

                ('>', 'A') => ComputeHelper('^'),
                ('^', 'A') => ComputeHelper('>'),
                ('<', 'A') => long.Min(ComputeHelper('>', '>', '^'), ComputeHelper('>', '^', '>')),
                ('v', 'A') => long.Min(ComputeHelper('>', '^'), ComputeHelper('^', '>')),

                _ => throw new Exception(),
            });

            long ComputeHelper(params ReadOnlySpan<char> chars)
            {
                long res = 0;
                char start = 'A';
                for (global::System.Int32 i = 0; i < chars.Length; i++)
                {
                    res += Length(start, chars[i], remainingIterations - 1);
                    start = chars[i];
                }
                return res + Length(start, 'A', remainingIterations - 1);
            }
        }

        static IEnumerable<IEnumerable<char>> FindPaths((int, int) curr, IEnumerable<char> origPath, Dictionary<char, (int, int)> lookup, HashSet<(int, int)> locationSet)
        {
            if (!origPath.Any())
                return [[]];
            IEnumerable<IEnumerable<char>> paths = [];
            var ch = origPath.ElementAt(0);
            {
                var next = lookup[ch];
                if (next == curr)
                {
                    return FindPaths(curr, origPath.Skip(1), lookup, locationSet).Select(p => new List<char> { 'A' }.Concat(p)).ToList();
                }

                {
                    if (next.Item1 > curr.Item1)
                    {
                        if (locationSet.Contains(curr.Plus((1, 0))))
                        {
                            var temp = FindPaths(curr.Plus((1, 0)), origPath, lookup, locationSet);
                            paths = paths.Concat(temp.Select(p => new List<char> { '>' }.Concat(p)));
                        }
                    }
                    if (next.Item2 < curr.Item2)
                    {
                        if (locationSet.Contains(curr.Plus((0, -1))))
                        {
                            var temp = FindPaths(curr.Plus((0, -1)), origPath, lookup, locationSet);
                            paths = paths.Concat(temp.Select(p => new List<char> { 'v' }.Concat(p)));
                        }
                    }
                    if (next.Item2 > curr.Item2)
                    {
                        if (locationSet.Contains(curr.Plus((0, 1))))
                        {
                            var temp = FindPaths(curr.Plus((0, 1)), origPath, lookup, locationSet);
                            paths = paths.Concat(temp.Select(p => new List<char> { '^' }.Concat(p)));
                        }
                    }
                    if (next.Item1 < curr.Item1)
                    {
                        if (locationSet.Contains(curr.Plus((-1, 0))))
                        {
                            var temp = FindPaths(curr.Plus((-1, 0)), origPath, lookup, locationSet);
                            paths = paths.Concat(temp.Select(p => new List<char> { '<' }.Concat(p)));
                        }
                    }
                }
            }
            return paths;
        }
    }
}