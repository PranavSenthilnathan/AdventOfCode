namespace AdventOfCode.Year2017;

internal static class Day5
{
    [AnswerMethod(2017, 5, 1)]
    public static string Part1(string[] input)
    {
        var ans = 0;

        var offsets = input.Select(int.Parse).ToArray();

        for (int curr = 0; ;)
        {
            var offset = offsets[curr];
            var next = curr + offset;
            offsets[curr]++;
            curr = next;

            ans++;

            if (curr < 0 || curr >= offsets.Length)
            {
                break;
            }
        }

        return ans.ToString();
    }

    [AnswerMethod(2017, 5, 2)]
    public static string Part2(string[] input)
    {
        var ans = 0;

        var offsets = input.Select(int.Parse).ToArray();

        for (int curr = 0; ;)
        {
            var offset = offsets[curr];
            var next = curr + offset;
            
            if (offsets[curr] >= 3)
                offsets[curr]--;
            else
                offsets[curr]++;

            curr = next;

            ans++;

            if (curr < 0 || curr >= offsets.Length)
            {
                break;
            }
        }

        return ans.ToString();
    }
}
