using AdventOfCode.lib;

namespace AdventOfCode.Year2025;

internal static class Day4
{
    [AnswerMethod(2025, 4, 1)]
    public static string Part1(string[] input)
    {
        var ans = 0;
        var grid = input.CreateGrid();

        foreach (var node in grid)
        {
            if (node.Value != '@') continue;

            var numNeighbors = 0;
            foreach (var neighbor in node.Key.GetAllNeighbors())
            {
                if (grid.TryGetValue(neighbor, out var n) && n == '@')
                {
                    numNeighbors++;
                }
            }

            if (numNeighbors < 4) ans++;
        }

        return ans.ToString();
    }

    [AnswerMethod(2025, 4, 2)]
    public static string Part2(string[] input)
    {
        var grid = input.CreateGrid();
        var cnt = grid.Count;
        bool removed;
        do
        {
            removed = false;

            foreach (var node in grid)
            {
                if (node.Value != '@') continue;

                var numNeighbors = 0;
                foreach (var neighbor in node.Key.GetAllNeighbors())
                {
                    if (grid.TryGetValue(neighbor, out var n) && n == '@')
                    {
                        numNeighbors++;
                    }
                }

                if (numNeighbors < 4)
                {
                    grid.Remove(node.Key);
                    removed = true;
                }
            }
        } while (removed);

        return (cnt - grid.Count).ToString();
    }
}
