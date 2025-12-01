namespace AdventOfCode.Year2017;

internal static class Day4
{
    [AnswerMethod(2017, 4, 1)]
    public static string Part1(string[] input)
    {
        var ans = 0;

        foreach (var line in input)
        {
            var words = line.Split(' ');
            
            if (words.ToHashSet().Count == words.Length)
            {
                ans++;
            }
        }

        return ans.ToString();
    }

    [AnswerMethod(2017, 4, 2)]
    public static string Part2(string[] input)
    {
        var ans = 0;

        foreach (var line in input)
        {
            var words = line.Split(' ');

            if (words.Select(str => string.Join("", str.Order())).ToHashSet().Count == words.Length)
            {
                ans++;
            }
        }

        return ans.ToString();
    }
}
