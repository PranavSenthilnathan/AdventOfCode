namespace AdventOfCode.Year2025;

internal static class Day1
{
    [AnswerMethod(2025, 1, 1)]
    public static string Part1(IEnumerable<string> input)
    {
        var n = 50;
        var ans = 0;

        foreach (var instr in input)
        {
            var rot = int.Parse(instr.Substring(1));
            var dir = instr[0];

            if (dir == 'R')
            {
                n = (n + rot) % 100;
            }
            else if (dir == 'L')
            {
                n = (n + 100 - rot) % 100;
            }

            if (n == 0)
            {
                ans++;
            }
        }

        return ans.ToString();
    }

    [AnswerMethod(2025, 1, 2)]
    public static string Part2(IEnumerable<string> input)
    {
        //input =
        //    """
        //    L68
        //    L30
        //    R48
        //    L5
        //    R60
        //    L55
        //    L1
        //    L99
        //    R14
        //    L82
        //    """.Split(['\n', '\r'], options: StringSplitOptions.RemoveEmptyEntries);

        var n = 50L;
        var ans = 0L;

        foreach (var instr in input)
        {
            var rot = int.Parse(instr.Substring(1));
            var dir = instr[0];

            if (dir == 'R')
            {
                while (rot != 0)
                {
                    n++;

                    if (n == 100)
                    {
                        ans++;
                        n = 0;
                    }

                    rot--;
                }
            }
            else if (dir == 'L')
            {
                while (rot != 0)
                {
                    n--;

                    if (n == 0)
                    {
                        ans++;
                    }

                    if (n == -1)
                    {
                        n = 99;
                    }

                    rot--;
                }
            }
        }

        return ans.ToString();
    }
}
