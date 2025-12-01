namespace AdventOfCode.Year2017;

internal static class Day1
{
    [AnswerMethod(2017, 1, 1)]
    public static string Part1(string input)
    {
        //input = "91212129";

        var sum = 0L;

        for (var i = 0; i < input.Length; i++)
        {
            var curr = input[i];
            var next = input[(i + 1) % input.Length];

            if (curr == next)
            {
                sum += next - '0';
            }
        }

        return sum.ToString();
    }

    [AnswerMethod(2017, 1, 2)]
    public static string Part2(string input)
    {
        //input = "12131415";
        var sum = 0L;

        for (var i = 0; i < input.Length; i++)
        {
            var curr = input[i];
            var next = input[(i + input.Length / 2) % input.Length];

            if (curr == next)
            {
                sum += next - '0';
            }
        }

        return sum.ToString();
    }
}
