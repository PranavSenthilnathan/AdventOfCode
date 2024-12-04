namespace AdventOfCode
{
    internal static class Day4
    {
        [AnswerMethod(2024, 4, 1)]
        public static string Part1(string[] input)
        {
            var grid = new Dictionary<(int, int), char>();

            foreach (var line in input.Index())
            {
                foreach (var ch in line.Item.Index())
                {
                    grid[(line.Index, ch.Index)] = ch.Item;
                }
            }

            var tot = 0;
            for (var r = 0; r < input.Length; r++)
                for (int c = 0; c < input[0].Length; c++)
                    foreach (var d in ValueTupleExtensions.GetAllDirections<int, int>())
                        if (Find("XMAS", (r, c), d))
                            tot++;
            return tot.ToString();

            bool Find(ReadOnlySpan<char> ros, (int, int) start, (int, int) dir)
            {
                if (ros.Length == 0) return true;
                if (grid.TryGetValue(start, out var c) && c == ros[0])
                {
                    return Find(ros.Slice(1), start.Plus(dir), dir);
                }

                return false;
            }
        }

        [AnswerMethod(2024, 4, 2)]
        public static string Part2(string[] input)
        {
            var grid = new Dictionary<(int, int), char>();

            foreach (var line in input.Index())
            {
                foreach (var ch in line.Item.Index())
                {
                    grid[(line.Index, ch.Index)] = ch.Item;
                }
            }

            var tot = 0;
            for (var r = 0; r < input.Length; r++)
                for (int c = 0; c < input[0].Length; c++)
                    if (grid[(r,c)] == 'A' && Find((r, c)))
                        tot++;
            return tot.ToString();

            bool Find((int, int) start)
            {
                var corners = start.GetIntercardinalNeighbors().ToArray();
                for (var i = 0; i < 4; i++)
               {
                    if (grid.GetValueOrDefault(corners[i]) == 'M' &&
                        grid.GetValueOrDefault(corners[(i + 2) % 4]) == 'S' &&
                        grid.GetValueOrDefault(corners[(i + 1) % 4]) == 'M' &&
                        grid.GetValueOrDefault(corners[(i + 3) % 4]) == 'S')
                        return true;
                }
                
                return false;
            }
        }
    }
}
