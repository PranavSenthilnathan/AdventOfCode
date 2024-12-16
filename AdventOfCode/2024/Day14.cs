using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using AdventOfCode.lib;

namespace AdventOfCode
{
    internal static class Day14
    {
        [AnswerMethod(2024, 14, 1)]
        public static string Part1(string[] input)
        {
            //input =
            //    """
            //    p=0,4 v=3,-3
            //    p=6,3 v=-1,-3
            //    p=10,3 v=-1,2
            //    p=2,0 v=2,-1
            //    p=0,0 v=1,3
            //    p=3,0 v=-2,-2
            //    p=7,6 v=-1,-3
            //    p=3,0 v=-1,-2
            //    p=9,3 v=2,3
            //    p=7,3 v=-1,2
            //    p=2,4 v=2,-3
            //    p=9,5 v=-3,-3
            //    """.Split('\n', StringSplitOptions.TrimEntries);

            int X = 101;
            int Y = 103;
            int[] res = new int[4];
            foreach (var line in input)
            {
                var inp = line.Split(['=',',',' ']);
                var p = (long.Parse(inp[1]), long.Parse(inp[2]));
                var v = (long.Parse(inp[4]), long.Parse(inp[5]));
                
                var x = (p.Item1 + (X + v.Item1) * 100) % X;
                var y = (p.Item2 + (Y + v.Item2) * 100) % Y;

                if (x < X / 2 && y < Y / 2)
                    res[0]++;
                else if (x < X / 2 && y > Y / 2)
                    res[1]++;
                else if (x > X / 2 && y < Y / 2)
                    res[2]++;
                else if (x > X / 2 && y > Y / 2)
                    res[3]++;
            }

            return res.Aggregate(1, (acc, m) => acc * m).ToString();
        }

        [AnswerMethod(2024, 14, 2)]
        public static string Part2(string[] input)
        {
            //input =
            //    """
            //    p=0,4 v=3,-3
            //    p=6,3 v=-1,-3
            //    p=10,3 v=-1,2
            //    p=2,0 v=2,-1
            //    p=0,0 v=1,3
            //    p=3,0 v=-2,-2
            //    p=7,6 v=-1,-3
            //    p=3,0 v=-1,-2
            //    p=9,3 v=2,3
            //    p=7,3 v=-1,2
            //    p=2,4 v=2,-3
            //    p=9,5 v=-3,-3
            //    """.Split('\n', StringSplitOptions.TrimEntries);

            int X = 101;
            int Y = 103;

            //int X = 11;
            //int Y = 7;
            int[] res = new int[4];
            var pts = new List<((long, long),(long,long))>();
            foreach (var line in input)
            {
                var inp = line.Split(['=', ',', ' ']);
                var p = (long.Parse(inp[1]), long.Parse(inp[2]));
                var v = (long.Parse(inp[4]), long.Parse(inp[5]));

                pts.Add((p, v));
            }

            var sec = 0;
            var maxScore = 0;
            while (true)
            {
                Run();
            }

            void Run()
            {
                var lst = CollectionsMarshal.AsSpan(pts);
                int[,] grid = new int[X, Y];
                for (global::System.Int32 i = 0; i < pts.Count; i++)
                {
                    lst[i] = (
                                (
                                    (lst[i].Item1.Item1 + lst[i].Item2.Item1 + 2 * X) % X,
                                    (lst[i].Item1.Item2 + lst[i].Item2.Item2 + 2 * Y) % Y
                                ),
                                lst[i].Item2
                            );

                    grid[lst[i].Item1.Item1, lst[i].Item1.Item2]++;
                }
                
                sec++;

                var score = 0;
                for (global::System.Int32 j = 0; j < Y; j++)
                {
                    for (global::System.Int32 i = 0; i < X; i++)
                    {
                        if (grid[i, j] == grid[X - i - 1, j])
                            score += grid[i, j];
                    }
                }

                if (score > maxScore)
                {
                    maxScore = score;

                    Console.WriteLine($"Second: {sec}");
                    for (global::System.Int32 j = 0; j < Y; j++)
                    {
                        for (global::System.Int32 i = 0; i < X; i++)
                        {
                            if (grid[i, j] != 0)
                                Console.Write(grid[i, j]);
                            else
                                Console.Write(".");
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine();
                }
            }

            throw new Exception();
        }
    }
}