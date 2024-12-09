namespace AdventOfCode
{
    internal static class Day6
    {
        [AnswerMethod(2024, 6, 1)]
        public static string Part1(string[] input)
        {
            var curr = (0, 0);

            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[0].Length; j++)
                {
                    if (input[i][j] == '^') curr = (i, j);
                }
            }

            var dir = (-1, 0);
            var visited = new HashSet<(int, int)>();
            while (true)
            {
                visited.Add(curr);
                var next = curr.Plus(dir);
                if (next.Item1 < 0 || next.Item1 >= input.Length || next.Item2 < 0 || next.Item2 >= input[0].Length)
                    break;

                if (input[next.Item1][next.Item2] == '#')
                    TurnRight(ref dir);
                else
                    curr = next;
            }

            void TurnRight(ref (int, int) dir)
            {
                dir = (dir.Item2, -dir.Item1);
            }

            return visited.Count.ToString();
        }

        [AnswerMethod(2024, 6, 2)]
        public static string Part2(string[] input)
        {
            var curr = (0, 0);
            var grid = new char[input.Length][];

            for (var i = 0; i < input.Length; i++)
            {
                grid[i] = input[i].ToCharArray();
                for (var j = 0; j < input[0].Length; j++)
                {
                    if (input[i][j] == '^') curr = (i, j);
                }
            }

            var dir = (-1, 0);
            var visited = new HashSet<(int, int)>();
            var obstacles = new HashSet<(int, int)>();

            Simulate(grid, curr, dir, (p, d) =>
            {
                visited.Add(p);
                var obstacle = p.Plus(d);
                if (!(obstacle.Item1 < 0 || obstacle.Item1 >= grid.Length || obstacle.Item2 < 0 || obstacle.Item1 >= grid[0].Length))
                {
                    if (grid[obstacle.Item1][obstacle.Item2] != '#')
                    {
                        grid[obstacle.Item1][obstacle.Item2] = '#';
                        if (!Simulate(grid, curr, dir, (_, _) => { }))
                        {
                            if (!visited.Contains(obstacle)) obstacles.Add(obstacle);
                        }
                        grid[obstacle.Item1][obstacle.Item2] = '.';
                    }
                }
            });

            static void TurnRight(ref (int, int) dir)
            {
                dir = (dir.Item2, -dir.Item1);
            }

            static bool Simulate(char[][]grid, (int, int) startPos, (int, int) startDir, Action<(int, int), (int, int)> f)
            {
                var visited = new HashSet<(int, int)>();
                var pos = startPos;
                var dir = startDir;
                while (true)
                {
                    f(pos, dir);
                    var next = pos.Plus(dir);
                    if (next.Item1 < 0 || next.Item1 >= grid.Length || next.Item2 < 0 || next.Item2 >= grid[0].Length)
                        return true;

                    if (grid[next.Item1][next.Item2] == '#')
                    {
                        if (!visited.Add(pos)) return false;
                        TurnRight(ref dir);
                    }
                    else
                        pos = next;
                }
            }

            return (obstacles.Count()).ToString();
        }
    }
}