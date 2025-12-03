namespace AdventOfCode.Year2025;

internal static class Day3
{
    [AnswerMethod(2025, 3, 1)]
    public static string Part1(string[] input)
    {
        var ans = 0;
        foreach (var line in input)
        {
            var max = 0;
            for (var i = 0; i < line.Length; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    var num = (line[i] - '0') + (line[j] - '0') * 10;
                    max = int.Max(max, num);
                }
            }
            ans += max;
        }
        return ans.ToString();
    }

    [AnswerMethod(2025, 3, 2)]
    public static string Part2(string[] input)
    {
        //input =
        //    """
        //    987654321111111
        //    811111111111119
        //    234234234234278
        //    818181911112111
        //    """.Split('\n', StringSplitOptions.TrimEntries).ToArray();

        var ans = 0L;
        foreach (var line in input)
        {
            var max = FindMax(line.Select(c => c - '0').ToArray(), 12);
            ans += max;
        }

        return ans.ToString();

        static long FindMax(ReadOnlySpan<int> digits, int length)
        {
            if (length == 1) return digits.Max();

            for (var d = 9; d > 0; d--)
            {
                var idx = digits.IndexOf(d);
                if (idx < 0 || digits.Length - idx < length)
                    continue;

                var max = FindMax(digits.Slice(idx + 1), length - 1);

                if (max != -1)
                {
                    return 10L.Pow(length - 1) * d + max;
                }
            }

            return -1;
        }
    }

    [AnswerMethod(2025, 3, 2)]
    public static string Part2_DP(string[] input)
    {
        //input =
        //    """
        //    987654321111111
        //    811111111111119
        //    234234234234278
        //    818181911112111
        //    """.Split('\n', StringSplitOptions.TrimEntries).ToArray();

        var ans = 0L;
        foreach (var line in input)
        {
            var max = FindMax([], line.Select(c => c - '0').ToArray(), 12);
            ans += max;
        }

        return ans.ToString();

        static long FindMax(Dictionary<(int, int), long> memo, ReadOnlySpan<int> digits, int length)
        {
            if (memo.TryGetValue((digits.Length, length), out var ret)) return ret;

            long max;

            if (length == 1)
            {
                max = digits.Max();
            }
            else
            {
                max = FindMax(memo, digits[1..], length - 1) + 10L.Pow(length - 1) * digits[0];

                if (digits.Length - 1 >= length)
                    max = long.Max(max, FindMax(memo, digits[1..], length));
            }

            return memo[(digits.Length, length)] = max;
        }
    }
}
