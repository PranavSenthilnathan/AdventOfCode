namespace AdventOfCode.Year2017;

internal static class Day2
{
    [AnswerMethod(2017, 2, 1)]
    public static string Part1(string[] input)
    {
        var ans = 0L;

        foreach (var row in input)
        {
            var max = int.MinValue;
            var min = int.MaxValue;

            foreach (var n in row.Split('\t'))
            {
                var curr = int.Parse(n);

                max = int.Max(max, curr);
                min = int.Min(min, curr);
            }

            ans += (max - min);
        }

        return ans.ToString();
    }

    [AnswerMethod(2017, 2, 2)]
    public static string Part2(string[] input)
    {
        var ans = 0L;

        foreach (var row in input)
        {
            var nums = row.Split('\t').Select(int.Parse).ToArray();

            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    var a = int.Max(nums[i], nums[j]);
                    var b = int.Min(nums[i], nums[j]);

                    if (a % b == 0)
                    {
                        ans += a / b;
                        goto end;
                    }
                }
            }

        end:
            ;
        }

        return ans.ToString();
    }
}
