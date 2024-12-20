namespace AdventOfCode;

internal static class Day1
{
    [AnswerMethod(2024, 1, 1)]
    public static string Part1(IEnumerable<string> input)
    {
        var l = new List<int>();
        var r = new List<int>();

        foreach (var line in input)
        {
            var spl = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            l.Add(int.Parse(spl[0]));
            r.Add(int.Parse(spl[1]));
        }

        l.Sort();
        r.Sort();

        var sum = 0;
        for (var i = 0; i < l.Count; i++)
        {
            sum += Math.Abs(l[i] - r[i]);
        }

        return sum.ToString();
    }

    [AnswerMethod(2024, 1, 2)]
    public static string Part2(IEnumerable<string> input)
    {
        var l = new Dictionary<int, int>();
        var r = new Dictionary<int, int>();

        foreach (var line in input)
        {
            var spl = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            l.TryGetValue(spl[0], out var val);
            l[spl[0]] = val + 1;
            r.TryGetValue(spl[1], out val);
            r[spl[1]] = val + 1;
        }

        var sum = 0L;
        foreach (var kvp in l)
        {
            r.TryGetValue(kvp.Key, out var val);
            sum += val * kvp.Value * kvp.Key;
        }

        return sum.ToString();
    }
}
