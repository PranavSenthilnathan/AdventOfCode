namespace AdventOfCode
{
    internal static class Day10
    {
        [AnswerMethod(2024, 10, 1)]
        public static string Part1(string[] input)
        {
            //input =
            //    """
            //    89010123
            //    78121874
            //    87430965
            //    96549874
            //    45678903
            //    32019012
            //    01329801
            //    10456732
            //    """.Split('\n', StringSplitOptions.TrimEntries);
            var starts = new HashSet<(int, int)>();
            for (int i = 0; i < input.Length; i++)
            {
                for (global::System.Int32 j = 0; j < input[0].Length; j++)
                {
                    if (input[i][j] == '0') starts.Add((i, j));
                }
            }

            var ans = 0;
            foreach (var start in starts)
            {
                var visited = new HashSet<(int, int)>();
                Climb(start);
                ans += visited.Count;

                void Climb((int, int) curr)
                {
                    if (input[curr.Item1][curr.Item2] == '9')
                    {
                        visited.Add(curr);
                        return;
                    }

                    foreach (var n in ValueTupleExtensions.GetCardinalNeighbors(curr))
                    {
                        if ((0, 0).LessThanOrEqualTo(n) && n.LessThan((input.Length, input[0].Length)) && input[n.Item1][n.Item2] == input[curr.Item1][curr.Item2] + 1)
                        {
                            Climb(n);
                        }
                    }
                }
            }

            return ans.ToString();
        }

        [AnswerMethod(2024, 10, 2)]
        public static string Part2(string[] input)
        {
            //input =
            //    """
            //    89010123
            //    78121874
            //    87430965
            //    96549874
            //    45678903
            //    32019012
            //    01329801
            //    10456732
            //    """.Split('\n', StringSplitOptions.TrimEntries);
            var starts = new HashSet<(int, int)>();
            for (int i = 0; i < input.Length; i++)
            {
                for (global::System.Int32 j = 0; j < input[0].Length; j++)
                {
                    if (input[i][j] == '0') starts.Add((i, j));
                }
            }

            var ans = 0;
            foreach (var start in starts)
            {
                Climb(start);

                void Climb((int, int) curr)
                {
                    if (input[curr.Item1][curr.Item2] == '9')
                    {
                        ans++;
                        return;
                    }

                    foreach (var n in ValueTupleExtensions.GetCardinalNeighbors(curr))
                    {
                        if ((0, 0).LessThanOrEqualTo(n) && n.LessThan((input.Length, input[0].Length)) && input[n.Item1][n.Item2] == input[curr.Item1][curr.Item2] + 1)
                        {
                            Climb(n);
                        }
                    }
                }
            }

            return ans.ToString();
        }
    }
}