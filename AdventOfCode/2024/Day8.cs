using System.Runtime.InteropServices;

namespace AdventOfCode
{
    internal static class Day8
    {
        [AnswerMethod(2024, 8, 1)]
        public static string Part1(string[] input)
        {
            //input = """
            //                    ............
            //    ........0...
            //    .....0......
            //    .......0....
            //    ....0.......
            //    ......A.....
            //    ............
            //    ............
            //    ........A...
            //    .........A..
            //    ............
            //    ............
            //    """.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var antennae = new Dictionary<char, List<(int, int)>>();
            var grid = new HashSet<(int, int)>();
            for (int i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[0].Length; j++)
                {
                    var c = input[i][j];
                    if (c != '.')
                    {
                        ref var set = ref CollectionsMarshal.GetValueRefOrAddDefault(antennae, c, out var exists);
                        if (!exists) set = new List<(int, int)>();
                        set!.Add((i, j));
                    }
                }
            }

            foreach (var antennaType in antennae)
            {
                foreach (var antennaLocation1 in antennaType.Value)
                {
                    foreach (var antennaLocation2 in antennaType.Value)
                    {
                        if (antennaLocation1 == antennaLocation2) continue;

                        var dir = antennaLocation2.Minus(antennaLocation1);
                        var curr = antennaLocation2.Plus(dir);
                        if (0 <= curr.Item1 && curr.Item1 < input.Length && 0 <= curr.Item2 && curr.Item2 < input[0].Length)
                        {
                            grid.Add(curr);
                            curr = curr.Plus(dir);
                        }
                    }
                }
            }

            return grid.Count.ToString();
        }

        [AnswerMethod(2024, 8, 2)]
        public static string Part2(string[] input)
        {
            //input = """
            //                    ............
            //    ........0...
            //    .....0......
            //    .......0....
            //    ....0.......
            //    ......A.....
            //    ............
            //    ............
            //    ........A...
            //    .........A..
            //    ............
            //    ............
            //    """.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var antennae = new Dictionary<char, List<(int, int)>>();
            var grid = new HashSet<(int, int)>();
            for (int i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[0].Length; j++)
                {
                    var c = input[i][j];
                    if (c != '.')
                    {
                        ref var set = ref CollectionsMarshal.GetValueRefOrAddDefault(antennae, c, out var exists);
                        if (!exists) set = new List<(int, int)>();
                        set!.Add((i, j));
                    }
                }
            }

            foreach (var antennaType in antennae)
            {
                foreach (var antennaLocation1 in antennaType.Value)
                {
                    foreach (var antennaLocation2 in antennaType.Value)
                    {
                        if (antennaLocation1 == antennaLocation2) continue;

                        var dir = antennaLocation2.Minus(antennaLocation1);
                        var curr = antennaLocation2;
                        while (curr.GreaterThanOrEqualTo((0, 0)) && curr.LessThan((input.Length, input[0].Length)))
                        {
                            grid.Add(curr);
                            curr = curr.Plus(dir);
                        }
                    }
                }
            }

            return grid.Count.ToString();
        }
    }
}