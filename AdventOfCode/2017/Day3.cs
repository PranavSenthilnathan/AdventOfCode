namespace AdventOfCode.Year2017;

internal static class Day3
{
    [AnswerMethod(2017, 3, 1)]
    public static string Part1(string input)
    {
        var m = new Dictionary<(int, int), int>();
        var c = (0, 0);
        var d = (1, 0);
        m[c] = 1;

        for (int i = 2; i <= int.Parse(input); i++)
        {
            var n = c + d.TurnLeft();

            if (m.ContainsKey(n))
            {
                n = c + d;
            }
            else
            {
                d = d.TurnLeft();
            }

            m[n] = i;
            c = n;
        }

        return (Math.Abs(c.Item1) + Math.Abs(c.Item2)).ToString();
    }

    [AnswerMethod(2017, 3, 2)]
    public static string Part2(string input)
    {
        var m = new Dictionary<(int, int), int>();
        var c = (0, 0);
        var d = (1, 0);
        m[c] = 1;

        for (int i = 2; ; i++)
        {
            var n = c + d.TurnLeft();

            if (m.ContainsKey(n))
            {
                n = c + d;
            }
            else
            {
                d = d.TurnLeft();
            }

            c = n;

            m[c] = c.GetAllNeighbors().Select(neighbor => m.GetValueOrDefault(neighbor)).Sum();

            if (m[c] > int.Parse(input))
            {
                return m[c].ToString();
            }
        }
    }
}
